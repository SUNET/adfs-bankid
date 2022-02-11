using System.Runtime.Serialization;
using BankID.Settings;
namespace ADFSBankID.Application.Settings
{

    [DataContract(Name = "BankIDMFASettings")]
    public class BankIDMFASettings
    {
        [DataMember(Name = "BankIDSettings")]
        public BankIDSettings BankIDConfig { get; set; }
        [DataMember(Name = "UserLookupMethod")]
        public string UserLookupMethod { get; set; }
        [DataMember(Name = "LdapSettings")]
        public LdapSettings LdapConfig { get; set; }
        [DataMember(Name = "SqlSettings")]
        public SqlSettings SqlConfig { get; set; }
    }
}
