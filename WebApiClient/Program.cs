using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            HmacClientHandler handler = new HmacClientHandler();
            HttpClient client = HttpClientFactory.Create(new HmacClientHandler());
            var responseMessage = client.GetAsync("http://localhost:47635/api/values").Result;
        }
    }
}
