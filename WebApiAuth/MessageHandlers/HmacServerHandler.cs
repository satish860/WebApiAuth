using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApiAuth.MessageHandlers
{
    public class HmacServerHandler : DelegatingHandler
    {
        private byte[] Key;
        public HmacServerHandler()
        {

            Key = Convert.FromBase64String("SGVsbG93b3JsZA==");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var Authorization = request.Headers.Authorization;
            if (Authorization != null && Authorization.Scheme.Equals("sharedKey", StringComparison.OrdinalIgnoreCase))
            {
                using (var hmac = new HMACSHA256(Key))
                {
                    var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes("password")));
                    if (signature == Authorization.Parameter)
                    {
                        return base.SendAsync(request, cancellationToken);
                    }

                }

            }

            return Task.Factory.StartNew<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            });


        }
    }
}