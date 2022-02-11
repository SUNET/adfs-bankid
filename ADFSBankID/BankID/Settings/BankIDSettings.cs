using System.Runtime.Serialization;

namespace BankID.Settings
{
    [DataContract]
    public class BankIDSettings
    {
        private string _baseUrl;
        [DataMember]
        public string BaseUrl
        {
            get { return _baseUrl; }
            set
            {
                if (!value.EndsWith("/"))
                {
                    _baseUrl = value + "/";
                }
            }
        }

        [DataMember]
        public string RPCertificateThumbprint { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string IssuerCertThumbprint { get; set; }

        [DataMember]
        public string RequirementsCertificate { get; set; }
    }
}
