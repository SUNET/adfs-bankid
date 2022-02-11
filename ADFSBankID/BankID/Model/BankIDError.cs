using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDError
    {
        [JsonProperty(PropertyName = "errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }
    }
}
