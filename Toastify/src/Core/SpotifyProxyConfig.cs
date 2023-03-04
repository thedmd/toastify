using JetBrains.Annotations;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using ToastifyAPI.Core;
using IProxyConfig = ToastifyAPI.Core.IProxyConfig;

namespace Toastify.Core
{
    /// <summary>
    /// An adapter for <see cref="SpotifyAPI.ProxyConfig"/>
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class SpotifyProxyConfig : IProxyConfig
    {
        internal ProxyConfig ProxyConfig
        {
            get
            {
                if (!IsValid())
                    return null;

                var result = new ProxyConfig(Host, Port);
                result.User = Username;
                result.Password = Password;
                result.BypassProxyOnLocal = BypassProxyOnLocal;
                result.SkipSSLCheck = SkipSSLCheck;
                return result;
            }
        }

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; } = 80;

        public string Username { get; set; }

        public string Password { get; set; }

        public bool BypassProxyOnLocal { get; set; }

        public bool SkipSSLCheck { get; set; }

        /// <inheritdoc />
        public bool UseDefaultCredentials
        {
            get { return !string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.Password); }
        }
        public SpotifyProxyConfig()
        {
        }

        internal SpotifyProxyConfig([CanBeNull] ProxyConfig proxyConfig)
        {
            Set(proxyConfig);
        }

        internal void Set([CanBeNull] ProxyConfig proxyConfig)
        {
            Host = proxyConfig?.Host;
            Port = proxyConfig?.Port ?? 80;
            Username = proxyConfig?.User;
            Password = proxyConfig?.Password;
            BypassProxyOnLocal = proxyConfig?.BypassProxyOnLocal ?? false;
            SkipSSLCheck = proxyConfig?.SkipSSLCheck ?? false;
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Host) && Port > 0;
        }

        /// <inheritdoc />
        public IWebProxy CreateWebProxy()
        {
            if (!IsValid())
                return null;

            WebProxy webProxy = new WebProxy
            {
                Address = new UriBuilder(Host) { Port = Port }.Uri,
                UseDefaultCredentials = true,
                BypassProxyOnLocal = BypassProxyOnLocal
            };

            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                webProxy.UseDefaultCredentials = false;
                webProxy.Credentials = new NetworkCredential(Username, Password);
            }

            return webProxy;
        }

        /// <inheritdoc />
        public object Clone()
        {
            return new SpotifyProxyConfig(this.ProxyConfig);
        }

        public override string ToString()
        {
            return this.IsValid() ? $"{(!string.IsNullOrEmpty(this.Username) ? $"{this.Username}@" : "")}{this.Host}:{this.Port}" : "";
        }

        public string ToString(bool objectHash)
        {
            string @string = this.ToString();
            if (objectHash)
            {
                if (this.ProxyConfig != null)
                    @string += $" @ {RuntimeHelpers.GetHashCode(this.ProxyConfig)}";
                @string = @string.Trim();
            }

            return @string;
        }
    }
}