using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aleab.Common.Net.WebSockets
{
    public abstract class BaseWebSocketHostStartup
    {
        public IHostingEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        public virtual int ReceiveBufferSize { get; } = 4 * 1024;

        protected HashSet<WebSocket> Connections { get; } = new HashSet<WebSocket>();

        protected BaseWebSocketHostStartup(IHostingEnvironment env, IConfiguration config)
        {
            this.HostingEnvironment = env;
            this.Configuration = config;
        }

        public abstract void ConfigureServices(IServiceCollection services);

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = this.ReceiveBufferSize
            };
            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
                    if (this.ShouldAcceptConnection(context, webSocket))
                    {
                        this.OnWebSocketConnected(context, webSocket);

                        await this.ConfigureWebSocketRequestPipeline(context, next, webSocket).ConfigureAwait(false);

                        this.OnWebSocketClosed(context, webSocket);
                    }
                    else
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None).ConfigureAwait(false);
                        context.Response.StatusCode = 400;
                    }
                }
                else
                    context.Response.StatusCode = 400;
            });
        }

        protected abstract Task ConfigureWebSocketRequestPipeline(HttpContext context, Func<Task> next, WebSocket webSocket);

        protected virtual bool ShouldAcceptConnection(HttpContext context, WebSocket webSocket)
        {
            return true;
        }

        protected virtual void OnWebSocketConnected(HttpContext context, WebSocket webSocket)
        {
            if (webSocket != null)
                this.Connections.Add(webSocket);
        }

        protected virtual void OnWebSocketClosed(HttpContext context, WebSocket webSocket)
        {
            if (webSocket != null)
                this.Connections.Remove(webSocket);
        }
    }
}