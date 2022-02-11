using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDCert
    {
        [JsonProperty(PropertyName = "notBefore")]
        public string NotBefore { get; set; }
        [JsonProperty(PropertyName = "notAfter")]
        public string NotAfter { get; set; }
    }
}
