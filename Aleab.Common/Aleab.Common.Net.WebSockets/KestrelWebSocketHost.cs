using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Aleab.Common.Net.WebSockets
{
    public sealed class KestrelWebSocketHost<T> where T : BaseWebSocketHostStartup
    {
        private readonly IWebHost webHost;

        public Uri Uri { get; }

        public KestrelWebSocketHost(string address)
        {
            this.Uri = new Uri(address);
            this.webHost = new WebHostBuilder()
                          .UseKestrel()
                          .UseStartup<T>()
                          .UseUrls(address)
                          .Build();
        }

        public void Start()
        {
            this.webHost.Start();
        }

        public async Task StopAsync()
        {
            if (this.webHost != null)
                await this.webHost.StopAsync().ConfigureAwait(false);
        }

        public async Task StopAsync(TimeSpan timeout)
        {
            if (this.webHost != null)
                await this.webHost.StopAsync(timeout).ConfigureAwait(false);
        }
    }
}