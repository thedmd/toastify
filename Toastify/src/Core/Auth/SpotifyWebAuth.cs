using log4net;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToastifyAPI.Core.Auth;
using ToastifyAPI.Events;
using ToastifyAPI.Helpers;
using IToken = ToastifyAPI.Core.Auth.IToken;

namespace Toastify.src.Core.Auth
{
    public class SpotifyWebAuth : ISpotifyWebAuth
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SpotifyWebAuth));

        private object _lock = new object();

        public string Scopes { get; }

        public SpotifyWebAuth(string scopes)
        {
            Scopes = scopes;
        }

        private IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var configuration = builder.Build();
            return configuration;
        }

        private string CLIENT_ID => GetConfiguration().GetValue<string>("ToasitfyClientId");
        private string CLIENT_SECRET => GetConfiguration().GetValue<string>("ToasitfyClientSecret");

        private async Task<IToken> GetTokenAsync()
        {
            using (var _server = new EmbedIOAuthServer(new Uri("http://localhost:4002/callback"), 4002))
            {
                await _server.Start();

                TaskCompletionSource<Token> resultTask = new TaskCompletionSource<Token>();

                _server.AuthorizationCodeReceived += async (object sender, AuthorizationCodeResponse response) =>
                {
                    AuthorizationCodeTokenResponse token = await new OAuthClient().RequestToken(
                        new AuthorizationCodeTokenRequest(CLIENT_ID,
                                                          CLIENT_SECRET,
                                                          response.Code,
                                                          _server.BaseUri));

                    resultTask.SetResult(new Token(token));
                };

                _server.ErrorReceived += (object sender, string error, string state) =>
                {
                    Console.WriteLine($"Aborting authorization, error received: {error}");

                    resultTask.SetResult(null);

                    return Task.CompletedTask;
                };

                var loginRequest = new LoginRequest(_server.BaseUri, CLIENT_ID, LoginRequest.ResponseType.Code)
                {
                    Scope = new List<string>(this.Scopes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                };

                Uri uri = loginRequest.ToUri();
                try
                {
                    BrowserUtil.Open(uri);
                }
                catch (Exception)
                {
                    logger.ErrorFormat("Unable to open URL, manually open: {0}", uri);
                }

                Token result = null;
                try
                {
                    result = await resultTask.Task.WaitAsync(TimeSpan.FromSeconds(30));
                }
                finally
                {
                    await _server.Stop();
                }

                return result;
            }
        }

        public Task<IToken> GetToken()
        {
            return GetTokenAsync();
        }

        private async Task<IToken> RefreshTokenAsync(IToken token)
        {
            AuthorizationCodeRefreshResponse response = await new OAuthClient().RequestToken(
                        new AuthorizationCodeRefreshRequest(CLIENT_ID, CLIENT_SECRET, token.RefreshToken));

            return new Token(response, token.RefreshToken);
        }

        public Task<IToken> RefreshToken(IToken token)
        {
            return RefreshTokenAsync(token);
        }

        public void Dispose()
        {
        }

        private OAuthClient GetOAuthClient()
        {
            return new OAuthClient();
        }

        private Task<ClientCredentialsTokenResponse> RequestToken()
        {
            var request = new ClientCredentialsRequest(CLIENT_ID, CLIENT_SECRET);
            return new OAuthClient().RequestToken(request);
        }
    }
}
