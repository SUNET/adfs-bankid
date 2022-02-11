using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using BankID;
using BankID.Model;
using BankID.Service;
using BankID.Interface;
using BankID.Exceptions;
using Newtonsoft.Json;
using System.IO;
using Microsoft.IdentityServer.Web.Authentication.External;
using System.Security.Claims;
using ADFSBankIDSecondFactor;

namespace ADFSBankIdTest
{
    [TestClass]
    public class BankIDTests : BaseTest
    {
        public BankIDTests() : base()
        {

        }
        
        
        [TestMethod]
        [DeploymentItem("BankIDSettings.json")]
        public void AuthenticationTest()
        {
            if (string.IsNullOrEmpty(this.CivicNumber)) { Assert.Fail("No Civicnumber provider, set in app.config"); }
            var uid = "someuid@domain.se";
            var d = new Dictionary<string, object>();
            d.Add("id", uid);
            d.Add("nin", this.CivicNumber);

            IAuthenticationContext authContext = new AuthenticationContext()
            {
                ActivityId = "minAktivitet",
                ContextId = "minContext",
                Lcid = 1033,
                Data = d
            };

            IAuthenticationMethodConfigData configData = new ConfigData()
            {
                Data = File.Open("BankIDSettings.json", FileMode.Open)
            };

            IProofData proofData = null;// new ProofData();
            var outgoingClaims = new Claim[] { };
            var adapter = new BankIDAdapter();
            
            //Load configdata
            adapter.OnAuthenticationPipelineLoad(configData);

            // No proofdata required as this is a mfa adapter
            proofData = new ProofData();// { Properties = GetFirstFactorProperties() };
            try
            {
                IAdapterPresentation presentation =  adapter.TryEndAuthentication(authContext, proofData, null, out outgoingClaims);
                if (presentation != null)
                {
                    Assert.Fail("Misslyckades med inloggning");
                }
                else
                {
                    Console.Out.WriteLine("Success");
                }
            }
            catch (TypeInitializationException typeEx)
            {
                var e = typeEx.Message;
            }

        }
        
    }
}
