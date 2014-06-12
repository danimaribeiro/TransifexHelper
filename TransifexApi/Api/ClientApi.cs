using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TransifexApi.Api
{
    public class ClientApi
    {
        private Base.Configuration _configuration;

        public ClientApi(Base.Configuration configuration)
        {
            _configuration = configuration;
        }

        public List<Languages> Languages()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://www.transifex.com/api/2/");
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync("languages").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<Languages>>().Result;
                }
                else
                    throw new Exception("Deu erro");
            }
        }

        public List<Projects> Projects()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://www.transifex.com/api/2/");
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync("projects").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<Projects>>().Result;
                }
                else
                    throw new Exception("Deu erro");
            }
        }

    }
}
