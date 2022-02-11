using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDCompletionData
    {
        [JsonProperty(PropertyName = "user")]
        public BankIDUser BankIDUser { get; set; }
        [JsonProperty(PropertyName = "device")]
        public BankIDDevice BankIDDevice { get; set; }
        [JsonProperty(PropertyName = "cert")]
        public BankIDCert BankIDCert { get; set; }
        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }
        [JsonProperty(PropertyName = "ocspResponse")]
        public string OcspResponse { get; set; }
    }
}
