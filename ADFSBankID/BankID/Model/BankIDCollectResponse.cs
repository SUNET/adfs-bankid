using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDCollectResponse
    {
        [JsonProperty(PropertyName = "orderRef")]
        public string OrderRef { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// HintCodes available at https://www.bankid.com/assets/bankid/rp/bankid-relying-party-guidelines-v3.5.pdf
        /// as of 20/12/14.
        /// </summary>
        [JsonProperty(PropertyName = "hintCode")]
        public string HintCode { get; set; } = null;

        [JsonProperty(PropertyName = "completionData")]
        public BankIDCompletionData CompletionData { get; set; } = null;
    }
}
