using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.JsonPatch;
using System.Net.Http;

namespace ClaimClient.Helpers
{
    public static class HttpClientExtension
    {
        public static Task<HttpResponseMessage> PatchAsync( this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return client.SendAsync( request);
        }
    }
}
