using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Aleab.Common.Net.Model
{
    [Serializable]
    public abstract class BaseModel
    {
        private ResponseInfo responseInfo;

        internal void AddResponseInfo(ResponseInfo responseInfo)
        {
            this.responseInfo = responseInfo;
        }

        public string GetHeader(string key)
        {
            return this.GetHeaders().TryGetValues(key, out IEnumerable<string> values) ? values.FirstOrDefault() : null;
        }

        public HttpResponseHeaders GetHeaders()
        {
            return this.responseInfo.Headers;
        }

        public HttpStatusCode GetStatusCode()
        {
            return this.responseInfo.StatusCode;
        }
    }
}