using Newtonsoft.Json;
namespace BankID.Model
{
    public class BankIDAuthResponse
    {
        [JsonProperty(PropertyName = "orderRef")]
        public string OrderRef { get; set; }

        [JsonProperty(PropertyName = "autoStartToken")]
        public string AutoStartToken { get; set; }

        [JsonProperty(PropertyName = "qrStartToken")]
        public string QrStartToken { get; set; }

        [JsonProperty(PropertyName = "qrStartSecret")]
        public string QrStartSecret { get; set; }
    }
}
