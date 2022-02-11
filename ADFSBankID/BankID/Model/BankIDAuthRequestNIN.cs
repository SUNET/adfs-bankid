using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDAuthRequestNIN
    {
        public BankIDAuthRequestNIN()
        {
            Requirement = new Dictionary<string, IList<string>>(); 
        }

        [JsonProperty(PropertyName = "endUserIp")]
        public string EndUserIP { get; set; }

        [JsonProperty(PropertyName = "personalNumber")]
        public string PersonalNumber { get; set; }

        /// <summary>
        /// Certificate policies for auth with Mobile BankID
        /// </summary>
        [JsonProperty(PropertyName = "requirement")]
        public IDictionary<string, IList<string>> Requirement { get; set; }
        // = new Dictionary<string, IList<string>>() {
            //{ "certificatePolicies", new List<string>(){ "1.2.752.78.1.5" } }
        
    }
}
