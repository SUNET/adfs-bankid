using BankID.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADFSBankIdTest
{
    public class BaseTest
    {
        public BankIDSettings Settings { get; set; }

        public string CivicNumber { get; set; }
        public BaseTest()
        {
            Settings = new BankIDSettings()
            {
                IssuerCertThumbprint = ConfigurationManager.AppSettings["IssuerCertificateThumbPrint"],
                RPCertificateThumbprint = ConfigurationManager.AppSettings["RPCertificateThumbPrint"],
                BaseUrl = ConfigurationManager.AppSettings["BaseUrl"],
                RequirementsCertificate = ConfigurationManager.AppSettings["RequirementCertificate"]
            };
            CivicNumber = ConfigurationManager.AppSettings["civicNumber"];
        }


    }
}
