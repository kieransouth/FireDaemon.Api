namespace FireDaemon.Api
{
    using Newtonsoft.Json;

    public class ApiResponse
    {
        [JsonProperty("resultType")]
        public string ResultType { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
