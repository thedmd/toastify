using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Aleab.Common.Logging;
using Aleab.Common.Logging.Extensions;
using Aleab.Common.Logging.Interfaces;
using Aleab.Common.Net.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aleab.Common.Net
{
    public class JsonHttpClient
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(JsonHttpClient));

        #region Static Fields and Properties

        private static HttpClient HttpClient { get; } = CreateHttpClient(null);

        #endregion

        private Encoding Encoding { get; } = Encoding.UTF8;
        private string ContentType { get; } = "application/json";

        public JsonSerializerSettings JsonSettings { get; set; }

        #region Events

        public event EventHandler<ErrorEventArgs> JsonSerializerError;

        #endregion

        public JsonHttpClient()
        {
            this.JsonSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.DateTime,
                FloatFormatHandling = FloatFormatHandling.String,
                FloatParseHandling = FloatParseHandling.Double,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Error = this.ErrorHandler
            };
        }

        public void ErrorHandler(object sender, ErrorEventArgs e)
        {
            switch (e.ErrorContext.Error)
            {
                case JsonSerializationException jsonSerializationException:
                    logger.Warn($"Error while serializing or deserializing \"{e.CurrentObject.GetType().FullName}\"", jsonSerializationException);
                    e.ErrorContext.Handled = true;
                    break;
            }

            this.JsonSerializerError?.Invoke(sender, e);
        }

        #region Static members

        public static HttpClient CreateHttpClient(HttpClientHandler handler)
        {
            HttpClientHandler httpClientHandler = handler ?? new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = false,
                UseProxy = false
            };

            return new HttpClient(httpClientHandler)
            {
                DefaultRequestHeaders =
                {
                    Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") }
                }
            };
        }

        #endregion

        #region Get*Async

        public async Task<T> GetDataAsync<T>(string url, Dictionary<string, string> headers = null) where T : BaseModel
        {
            Tuple<ResponseInfo, T> response = await this.GetJsonAsync<T>(url, headers).ConfigureAwait(false);
            if (response.Item1.StatusCode != HttpStatusCode.OK)
            {
                // TODO: Handle non-OK status codes
            }

            T data = response.Item2;
            data.AddResponseInfo(response.Item1);
            return data;
        }

        public async Task<T> GetDataAsync<T, TElem>(string url, Dictionary<string, string> headers = null)
            where T : Collection<TElem>, new()
            where TElem : BaseModel
        {
            Tuple<ResponseInfo, List<TElem>> response = await this.GetJsonAsync<List<TElem>>(url, headers).ConfigureAwait(false);
            if (response.Item1.StatusCode != HttpStatusCode.OK)
            {
                // TODO: Handle non-OK status codes
            }

            var data = new T
            {
                Items = response.Item2
            };
            data.AddResponseInfo(response.Item1);
            return data;
        }

        public async Task<Tuple<ResponseInfo, T>> GetJsonAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, string> response = await this.GetAsync(url, headers).ConfigureAwait(false);
            return new Tuple<ResponseInfo, T>(response.Item1, JsonConvert.DeserializeObject<T>(response.Item2 ?? "{}", this.JsonSettings));
        }

        public async Task<Tuple<ResponseInfo, string>> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, byte[]> raw = await this.GetRawAsync(url, headers).ConfigureAwait(false);
            return new Tuple<ResponseInfo, string>(raw.Item1, raw.Item2.Length > 0 ? this.Encoding.GetString(raw.Item2) : null);
        }

        public async Task<Tuple<ResponseInfo, byte[]>> GetRawAsync(string url, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            using (HttpResponseMessage response = await HttpClient.SendAsync(request).ConfigureAwait(false))
            {
                return new Tuple<ResponseInfo, byte[]>(new ResponseInfo
                {
                    Headers = response.Headers,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase
                }, await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false));
            }
        }

        #endregion Get*Async

        #region Post*Async

        public async Task<string> PostDataAsync<TReq>(string url, TReq contentData, Dictionary<string, string> headers = null)
        {
            string json = JsonConvert.SerializeObject(contentData, this.JsonSettings);

            Tuple<ResponseInfo, string> response = await this.PostJsonAsync<string>(url, json, headers).ConfigureAwait(false);
            if (response.Item1.StatusCode != HttpStatusCode.OK)
            {
                // TODO: Handle non-OK status codes
            }

            return response.Item2;
        }

        public async Task<TRes> PostDataAsync<TReq, TRes>(string url, TReq contentData, Dictionary<string, string> headers = null) where TRes : BaseModel
        {
            string json = JsonConvert.SerializeObject(contentData, this.JsonSettings);

            Tuple<ResponseInfo, TRes> response = await this.PostJsonAsync<TRes>(url, json, headers).ConfigureAwait(false);
            if (response.Item1.StatusCode != HttpStatusCode.OK)
            {
                // TODO: Handle non-OK status codes
            }

            TRes data = response.Item2;
            data.AddResponseInfo(response.Item1);
            return data;
        }

        public async Task<Tuple<ResponseInfo, string>> PostJsonAsync(string url, string json, Dictionary<string, string> headers = null)
        {
            return await this.PostAsync(url, json, this.ContentType, headers).ConfigureAwait(false);
        }

        public async Task<Tuple<ResponseInfo, TRes>> PostJsonAsync<TRes>(string url, string json, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, string> response = await this.PostJsonAsync(url, json, headers).ConfigureAwait(false);
            return new Tuple<ResponseInfo, TRes>(response.Item1, JsonConvert.DeserializeObject<TRes>(response.Item2 ?? "{}", this.JsonSettings));
        }

        public async Task<Tuple<ResponseInfo, string>> PostAsync(string url, string content, string contentType = null, Dictionary<string, string> headers = null)
        {
            Tuple<ResponseInfo, byte[]> raw = await this.PostRawAsync(url, content, contentType).ConfigureAwait(false);
            return new Tuple<ResponseInfo, string>(raw.Item1, raw.Item2.Length > 0 ? this.Encoding.GetString(raw.Item2) : null);
        }

        public async Task<Tuple<ResponseInfo, byte[]>> PostRawAsync(string url, string content, string contentType = null, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            request.Content = new StringContent(content, this.Encoding, contentType);

            using (HttpResponseMessage response = await HttpClient.SendAsync(request).ConfigureAwait(false))
            {
                return new Tuple<ResponseInfo, byte[]>(new ResponseInfo
                {
                    Headers = response.Headers,
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase
                }, await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false));
            }
        }

        #endregion Post*Async
    }
}