using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class HmacClientHandler : DelegatingHandler
    {
        private byte[] Key;
        public HmacClientHandler()
        {
            Key = Convert.FromBase64String("SGVsbG93b3JsZA==");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            using (var hmac = new HMACSHA256(Key))
            {
                var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes("password")));
                request.Headers.Authorization = new AuthenticationHeaderValue("sharedKey", signature);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
