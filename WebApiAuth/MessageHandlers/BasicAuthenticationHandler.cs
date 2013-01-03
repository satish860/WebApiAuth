using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApiAuth.MessageHandlers
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        IDictionary<string, string> UserNamePasswordCombo;

        public BasicAuthenticationHandler()
        {
            UserNamePasswordCombo = new Dictionary<string, string>();
            UserNamePasswordCombo.Add("user", "password");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null && 
                request.Headers.Authorization.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase))
            {
                if (ExtractandValidatePassword(request.Headers.Authorization.Parameter) != null)
                {
                   
                    return base.SendAsync(request, cancellationToken);
                }
            }
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("basic"));
            return Task.Factory.StartNew<HttpResponseMessage>(() => { return response; });

           
        }

        private string ExtractandValidatePassword(string basicCredentials)
        {
            string UserNamepasswordPair;
            try
            {
                UserNamepasswordPair = Encoding.ASCII.GetString(Convert.FromBase64String(basicCredentials));
            }
            catch (FormatException)
            {
                return null;
            }
            var index = UserNamepasswordPair.IndexOf(":");
            var userName = UserNamepasswordPair.Substring(0, index);
            var password = UserNamepasswordPair.Substring(index + 1);
            return UserNamePasswordCombo.ContainsKey(userName) ? userName : null;

        }
    }
}