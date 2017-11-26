namespace FireDaemon.Api
{
    using System.IO;
    using System.Net;

    using Newtonsoft.Json;

    public class FireDaemonApiClient
    {
        public FireDaemonApiClient(string url)
        {
            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            BaseUrl = url;
        }

        public string BaseUrl { get; set; }
        public string AuthenticationCookie { get; set; }

        public bool Authenticate(string username, string password)
        {
            var queryString = $"username={username}&password={password}";
            var httpWebRequest = WebRequest.Create($"{BaseUrl}login") as HttpWebRequest;

            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = queryString.Length;
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(queryString);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            AuthenticationCookie = httpWebResponse.Headers.Get("Set-Cookie");

            return !string.IsNullOrEmpty(AuthenticationCookie);
        }

        public bool StartService(string serviceName)
        {
            return MakeRequest(serviceName, true);
        }

        public bool StopService(string serviceName)
        {
            return MakeRequest(serviceName, false);
        }

        private bool MakeRequest(string serviceName, bool start)
        {
            var action = start ? "start" : "stop";

            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, AuthenticationCookie);

            var requestUrl = $"{BaseUrl}fd/{action}ajax?svcname={serviceName}";
            var response = webClient.DownloadString(requestUrl);
            var deserialised = JsonConvert.DeserializeObject<ApiResponse>(response);

            return (deserialised?.ResultType ?? string.Empty) == "success";
        }
    }
}
