using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDCollectClientResponse
    {
        [JsonProperty(PropertyName = "orderRef", Required = Required.DisallowNull)]
        public string OrderRef { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.DisallowNull)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "hint")]
        public string Hint { get; set; }

        /// <summary>
        /// Signin resulted in the following liuids. If there are more than 
        /// one alternative the client must make a finalisation call in authentication 
        /// controller.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public IList<string> AssociatedLiuIds { get; set; } = new List<string>();
    }
}
