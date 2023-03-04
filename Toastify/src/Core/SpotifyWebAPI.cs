using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aleab.Common.Extensions;
using JetBrains.Annotations;
using log4net;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http;
using Toastify.Model;
using Toastify.src.Core.Auth;
using ToastifyAPI.Core;
using ToastifyAPI.Core.Auth;
using ToastifyAPI.Events;
using ToastifyAPI.Model.Interfaces;
using IToken = ToastifyAPI.Core.Auth.IToken;
using SpotifyClient = SpotifyAPI.Web.SpotifyClient;

namespace Toastify.Core
{
    public class SpotifyWebAPI : ISpotifyWebAPI
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SpotifyWebAPI));

        private SpotifyClient _spotifyWebApi;
        private Token _token;

        #region Non-Public Properties

        private SpotifyClient SpotifyWebApi
        {
            get
            {
                if (_spotifyWebApi == null)
                {
                    var config = SpotifyClientConfig.CreateDefault();

                    var proxyConfig = App.ProxyConfig.ProxyConfig;
                    if (proxyConfig != null)
                        config = config.WithHTTPClient(new NetHttpClient(proxyConfig));

                    if (Token != null)
                        config = config.WithToken(Token.AccessToken, Token.TokenType);

                    _spotifyWebApi = new SpotifyClient(config);
                }

                return _spotifyWebApi;
            }
        }

        private ITokenManager TokenManager { get; }

        #endregion

        #region Public Properties

        public IToken Token
        {
            get { return this._token; }
            set { this._token = value as Token; }
        }

        #endregion

        public SpotifyWebAPI([NotNull] ITokenManager tokenManager)
        {
            this.TokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            this.TokenManager.TokenChanged += this.TokenManager_TokenChanged;
        }

        public async Task<ICurrentlyPlayingObject> GetCurrentlyPlayingTrackAsync()
        {
            if (this.SpotifyWebApi == null || !this.WaitForTokenRefresh())
                return null;

            CurrentlyPlaying currentlyPlaying = await this.PerformRequest(
                async () => await this.SpotifyWebApi.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).ConfigureAwait(false),
                "Couldn't get the current playback context.").ConfigureAwait(false);

            return currentlyPlaying != null ? new CurrentlyPlayingObject(currentlyPlaying) : null;
        }

        public async Task<ISpotifyUserProfile> GetUserPrivateProfileAsync()
        {
            if (this.SpotifyWebApi == null || !this.WaitForTokenRefresh())
                return null;

            PrivateUser profile = await this.PerformRequest(
                async () => await this.SpotifyWebApi.UserProfile.Current().ConfigureAwait(false),
                "Couldn't get the current user's private profile.").ConfigureAwait(false);

            return profile != null ? new SpotifyUserProfile(profile) : null;
        }

        private async Task<T> PerformRequest<T>(Func<Task<T>> request, string errorMsg) where T : class
        {
            return await this.PerformRequest(request, errorMsg, true).ConfigureAwait(false);
        }

        private async Task<T> PerformRequest<T>(Func<Task<T>> request, string errorMsg, bool retry) where T : class
        {
            T response = null;
            string apiMessage = null;
            try
            {
                response = await request();
            }
            catch (APIException apiException)
            {
                apiMessage = apiException.Message;
            }
            var lastResponse = this.SpotifyWebApi.LastResponse;
            if (response == null || lastResponse == null || lastResponse.StatusCode == HttpStatusCode.NoContent)
                return null;

            LogReturnedValueIfError(errorMsg, lastResponse, apiMessage);

            var statusCode = lastResponse.StatusCode;
            if (statusCode == (HttpStatusCode)431)
            {
                logger.Debug("HTTP 431 received: a new instance of SpotifyWebApi will be created.");
                this._spotifyWebApi = null;

                // Retry
                if (retry)
                    response = await this.PerformRequest(request, errorMsg, false).ConfigureAwait(false);
            }

            return response;
        }

        private bool WaitForTokenRefresh()
        {
            if (!this.TokenManager.RefreshingTokenEvent.WaitOne(TimeSpan.FromSeconds(30)))
            {
                logger.Warn($"Timeout while waiting for the token to refresh: API request cancelled!{Environment.NewLine}{new StackTrace().GetFrames(1, 2)}");
                return false;
            }

            return true;
        }

        private void TokenManager_TokenChanged(object sender, SpotifyTokenChangedEventArgs e)
        {
            this.Token = e.NewToken;
        }

        #region Static Members

        private static void LogReturnedValueIfError(string msg, IResponse ret, string apiMessage)
        {
            if (ret == null || ret.StatusCode != HttpStatusCode.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{msg} Returned value = ");
                if (ret != null)
                {
                    sb.Append($"{{{Environment.NewLine}")
                      .Append($"   StatusCode: \"{ret.StatusCode}\"");
                    if (apiMessage != null)
                    {
                        sb.Append($",{Environment.NewLine}")
                          .Append($"   Error: {{{Environment.NewLine}")
                          //.Append($"      Status: {ret.Error.Status},{Environment.NewLine}")
                          .Append($"      Message: \"{apiMessage}\"{Environment.NewLine}")
                          .Append("   }");
                    }

                    sb.Append($"{Environment.NewLine}}}");
                }
                else
                    sb.Append("null");

                logger.Warn(sb.ToString());
            }
        }

        #endregion
    }
}