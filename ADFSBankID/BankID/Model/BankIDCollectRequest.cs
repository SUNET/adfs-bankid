using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDCollectRequest
    {
        [JsonProperty(PropertyName = "orderRef")]
        public string OrderRef { get; set; }
    }
}
