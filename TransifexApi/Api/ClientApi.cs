using System;
using System.Collections.Generic;
using System.IO;
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

        public Projects InfoProject(string projectName)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://www.transifex.com/api/2/");
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync("project/" + projectName).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<Projects>().Result;
                }
                else
                    return null;
            }
        }

        public List<Resources> ProjectResources(string projectName)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://www.transifex.com/api/2/");
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync("project/" + projectName + "/resources/").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<Resources>>().Result;
                }
                else
                    return null;
            }
        }

        public List<Translation> Translations(string projectName, string resource)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://www.transifex.com/api/2/");
                client.DefaultRequestHeaders.Accept.Clear();

                var url = string.Format("project/{0}/resource/{1}/translation/{2}/strings/", projectName, resource, _configuration.LanguadeCode);
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<Translation>>().Result;
                }
                else
                    return null;
            }
        }

        public Translation GetTranslation(string projectName, string resource, Translation translation)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://www.transifex.com/api/2/");
                client.DefaultRequestHeaders.Accept.Clear();

                var url = string.Format("project/{0}/resource/{1}/translation/{2}/string/{3}", projectName, resource, _configuration.LanguadeCode, translation.CalculateHash());
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<Translation>().Result;
                }
                else
                    return null;
            }

        }

        public bool UpdateTranslation(string projectName, string resource, Translation translation)
        {
            try
            {
                var url = string.Format("https://www.transifex.com/api/2/project/{0}/resource/{1}/translation/{2}/string/{3}", projectName, resource, _configuration.LanguadeCode, translation.CalculateHash());
                var request = (HttpWebRequest)WebRequest.Create(url);

                var postData = Newtonsoft.Json.JsonConvert.SerializeObject(new TranslationUpdate() { translation = translation.translation });
                var data = Encoding.ASCII.GetBytes(postData);

                request.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return "OK" == responseString;
            }
            catch (Exception)
            {
                return false;
            }
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);

            //using (var client = new HttpClient(handler))
            //{
            //    client.BaseAddress = new Uri("https://www.transifex.com/api/2/");                
            //    client.DefaultRequestHeaders.Clear();                

            //    var url = string.Format("project/{0}/resource/{1}/translation/{2}/string/{3}", projectName, resource, _configuration.LanguadeCode, translation.CalculateHash());
            //    HttpResponseMessage response = client.PutAsJsonAsync<TranslationUpdate>(url, new TranslationUpdate() { translation = translation.translation }).Result;
            //    return response.IsSuccessStatusCode;
            //}
        }

    }
}
