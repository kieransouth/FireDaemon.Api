namespace FireDaemon.Api.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class FireDaemonApiClientTests
    {
        public FireDaemonApiClient ApiClient
        {
            get
            {
                if (_apiClient == null)
                {
                    _apiClient = new FireDaemonApiClient("http://localhost:20604");
                }

                return _apiClient;
            }
            set
            {
                _apiClient = value;
            }
        }

        private FireDaemonApiClient _apiClient;

        [TestMethod()]
        public void AuthenticateTest()
        {
            ApiClient.Authenticate("username", "password");
        }

        [TestMethod()]
        public void StartServiceTest()
        {
            ApiClient.StartService("ServiceName");
        }

        [TestMethod()]
        public void StopServiceTest()
        {
            ApiClient.StopService("ServiceName");
        }
    }
}