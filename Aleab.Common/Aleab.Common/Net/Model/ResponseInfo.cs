using System.Net;
using System.Net.Http.Headers;

namespace Aleab.Common.Net.Model
{
    public class ResponseInfo
    {
        #region Static Fields and Properties

        public static readonly ResponseInfo Empty = new ResponseInfo();

        #endregion

        public HttpResponseHeaders Headers { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string ReasonPhrase { get; set; }
    }
}