using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankID.Model
{
    public class BankIDUser
    {
        [JsonProperty(PropertyName = "personalNumber")]
        public string PersonalNumber { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "givenName")]
        public string GivenName { get; set; }
        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }
    }
}
