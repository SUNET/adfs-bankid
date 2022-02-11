using ADFSBankID.Application.Interface;
using ADFSBankID.Application.Services;
using ADFSBankID.Application.Settings;
using ADFSBankID.Application.Utils;
using ADFSBankIDSecondFactor.Services;
using BankID.Exceptions;
using BankID.Interface;
using BankID.Model;
using BankID.Service;
using Microsoft.IdentityServer.Web.Authentication.External;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace ADFSBankIDSecondFactor
{
    public class BankIDAdapter : IAuthenticationAdapter
    {
        public IBankIDService _bankIDService;
        public IPersonService _personService;
        private static BankIDMFASettings _bankIDMFASettings { get; set; }
        protected IAdapterPresentationForm CreateAdapterPresentation(string civicnumber)
        {
            return new BankIDPresentation(civicnumber);
        }
        protected IAdapterPresentationForm CreateAdapterPresentationOnError(string civicnumber, ExternalAuthenticationException ex)
        {
            return new BankIDPresentation(civicnumber, ex);
        }
        public IAuthenticationAdapterMetadata Metadata => new BankIDMetadata();

        public IAdapterPresentation BeginAuthentication(Claim identityClaim, HttpListenerRequest request, IAuthenticationContext context)
        {
            AuthenticationContext usercontext = (AuthenticationContext)context;
            Log.WriteEntry("Enter begin Authentication in BankIDAdapter", EventLogEntryType.Information, 335);
            if (null == identityClaim) throw new ArgumentNullException(nameof(identityClaim));

            if (null == usercontext) throw new ArgumentNullException(nameof(usercontext));

            if (String.IsNullOrEmpty(identityClaim.Value))
            {
                throw new InvalidDataException(ResourceHandler.GetResource(Constants.ResourceNames.ErrorNoUserIdentity, usercontext.Lcid));
            }

            Log.WriteEntry("Proceeding with " + identityClaim.Value, EventLogEntryType.Information, 335);
            usercontext.Data.Add(Constants.AuthContextKeys.Identity, identityClaim.Value);
            var civicNumber = _personService.GetCivicNumber(identityClaim.Value).Trim();
            usercontext.Data.Add(Constants.AuthContextKeys.CivicNumber, civicNumber);
            
            if (civicNumber != null)
            {
                return CreateAdapterPresentation(civicNumber);
            }
            else
            {
                return CreateAdapterPresentationOnError("", new ExternalAuthenticationException("ErrorMissingCivicNo", usercontext));
            }

        }

        public bool IsAvailableForUser(Claim identityClaim, IAuthenticationContext context)
        {
            return true;
        }

        public void OnAuthenticationPipelineLoad(IAuthenticationMethodConfigData configData)
        {
            Log.WriteEntry("Loading BankIDAdapter", EventLogEntryType.Information, 335);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (configData != null)
            {
                if (configData.Data != null)
                {
                    using (StreamReader reader = new StreamReader(configData.Data, Encoding.UTF8))
                    {
                        //Config should be in a json format, and needs to be registered with the 
                        //-ConfigurationFilePath parameter when registering the MFA Adapter (Register-AdfsAuthenticationProvider cmdlet)
                        try
                        {
                            var config = reader.ReadToEnd();
                            var js = new DataContractJsonSerializer(typeof(BankIDMFASettings));
                            var ms = new MemoryStream(UTF8Encoding.UTF8.GetBytes(config));
                            var mfaConfig = (BankIDMFASettings)js.ReadObject(ms);
                            _bankIDMFASettings = mfaConfig;
                            Log.WriteEntry("BankID configuration loaded with the loginUrl: " + mfaConfig.BankIDConfig.BaseUrl, EventLogEntryType.Information, 335);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteEntry("Unable to load BankID config data. Check that it is registered and correct." + ex.Message + ex.StackTrace, EventLogEntryType.Error, 335);
                            throw new ArgumentException();
                        }
                    }
                    try
                    {
                        if (_bankIDMFASettings != null)
                        {
                            if (_bankIDMFASettings.BankIDConfig != null)
                            {
                                _bankIDService = new BankIDService(_bankIDMFASettings.BankIDConfig);
                                Log.WriteEntry("BankID created service", EventLogEntryType.Information, 335);
                                switch (_bankIDMFASettings.UserLookupMethod.ToUpper())
                                {
                                    case "LDAP":
                                        if (_bankIDMFASettings.LdapConfig != null)
                                        {
                                            //Set up LdapService provided
                                            Log.WriteEntry("BankID configuration, got ldap settings", EventLogEntryType.Information, 335);
                                            _personService = new PersonServiceLdap(_bankIDMFASettings.LdapConfig);
                                        }
                                        else
                                        {
                                            Log.WriteEntry("BankID configuration, didn't get ldap settings", EventLogEntryType.Information, 335);
                                        }
                                        break;
                                    case "SQL":
                                        if (_bankIDMFASettings.SqlConfig != null)
                                        {
                                            Log.WriteEntry("BankID configuration, got sql settings", EventLogEntryType.Information, 335);
                                            _personService = new PersonServiceSql(_bankIDMFASettings.SqlConfig);
                                        }
                                        else
                                        {
                                            Log.WriteEntry("BankID configuration, didn't get sql settings", EventLogEntryType.Information, 335);
                                        }
                                        break;
                                }
                            }
                            else {
                                Log.WriteEntry("No Settings provided for BankID api", EventLogEntryType.Error, 335); 
                            }
                        }
                        else { Log.WriteEntry("No Settings provided for BankID MFA Adapter", EventLogEntryType.Error, 335); }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteEntry("Unable to configure BankID" + ex.Message + ex.StackTrace, EventLogEntryType.Information, 335);
                        throw new ArgumentException();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void OnAuthenticationPipelineUnload()
        {
            _bankIDService = null;
            _bankIDMFASettings = null;
        }

        public IAdapterPresentation OnError(HttpListenerRequest request, ExternalAuthenticationException ex)
        {
            throw new NotImplementedException();
        }

        public IAdapterPresentation TryEndAuthentication(IAuthenticationContext authContext, IProofData proofData, HttpListenerRequest request, out Claim[] outgoingClaims)
        {
            var civicNumber = "";
            if (null == authContext)
            {
                throw new ArgumentNullException(nameof(authContext));
            }
            Log.WriteEntry("Context: " + authContext.ActivityId + " " + authContext.ContextId, EventLogEntryType.Information, 335);
            outgoingClaims = new Claim[0];

           

            if (!authContext.Data.ContainsKey(Constants.AuthContextKeys.Identity))
            {
                Log.WriteEntry( "TryEndAuthentication Context does not contains userID.",EventLogEntryType.Error,335);
                throw new ArgumentOutOfRangeException(Constants.AuthContextKeys.Identity);
            }


            if (!authContext.Data.ContainsKey(Constants.AuthContextKeys.CivicNumber))
            {
                Log.WriteEntry("TryEndAuthentication Context does not contains civicnumber.",EventLogEntryType.Error,335);
                throw new ExternalAuthenticationException(ResourceHandler.GetResource(Constants.ResourceNames.ErrorNoAnswerProvided, authContext.Lcid), authContext);
            }
            else
            {
                civicNumber = (string)authContext.Data[Constants.AuthContextKeys.CivicNumber];
                Log.WriteEntry("Got civicnumber from authcontext (" + civicNumber + ")", EventLogEntryType.Information, 335);
            }

            Log.WriteEntry("Starting BankID login with civicno: " + civicNumber, EventLogEntryType.Information, 335);
            string orderRef = "";

            try
            {
                var authRequest = new BankIDAuthRequestNIN();
                authRequest.EndUserIP = "127.0.0.1";
                authRequest.PersonalNumber = civicNumber;
                var a =_bankIDService.Auth(authRequest).GetAwaiter().GetResult();
                orderRef = a.OrderRef;
                Log.WriteEntry("Got response from BankIDAuthRequestNIN (orderRef: " + orderRef+")", EventLogEntryType.Information, 335);
            }
            catch (Exception ex)
            {
                Log.WriteEntry("Error in BankID login:  " + ex.Message + " " + ex.StackTrace, EventLogEntryType.Error, 335);
                if (ex.GetType().Equals(typeof(BankIDException)))
                {
                    return CreateAdapterPresentationOnError(civicNumber, new ExternalAuthenticationException(ex.Message, authContext));
                }
                return CreateAdapterPresentationOnError(civicNumber, new ExternalAuthenticationException("BankIDError_NoCivicNumber", authContext));
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Log.WriteEntry("Starting Timer for login", EventLogEntryType.Information, 335);
            var collectRequest = new BankIDCollectRequest() { OrderRef = orderRef };
            while (stopwatch.Elapsed.TotalSeconds < 60)
            {
                Thread.Sleep(3000);
                try
                    {
                        Log.WriteEntry("Check login status for OrderRef: " + orderRef, EventLogEntryType.Information, 335);
                        var resp = _bankIDService.Collect(collectRequest).GetAwaiter().GetResult();
                        Log.WriteEntry("Login status: " + resp.Status, EventLogEntryType.Information, 335);

                        if (_bankIDService.ValidAuthResponse(resp.Status))
                        {
                            if (resp.CompletionData.BankIDUser.PersonalNumber == civicNumber)
                            {
                                outgoingClaims = new[]
                                {
                                        new Claim( "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod",
                                        Constants.BANKIDMFA)
                                    };
                                break;
                            }
                            else
                            {

                            }
                        }
                        if(_bankIDService.CancelledAuthResponse(resp.Status))
                        {
                            // check for type of error and return page with message
                            return CreateAdapterPresentationOnError(civicNumber, new ExternalAuthenticationException("BankIDError_NoCivicNumber", authContext));
                        }
                    
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType().Equals(typeof(BankIDException)))
                        {
                            return CreateAdapterPresentationOnError(civicNumber, new ExternalAuthenticationException(ex.Message, authContext));
                        }
                    }
            }

            if (outgoingClaims.Length > 0)
            {
                Log.WriteEntry("Got claim, finished loginflow", EventLogEntryType.Information, 335);
                return null;
            }
            else
            {
                Log.WriteEntry("No claim, return error page", EventLogEntryType.Information, 335);
                return CreateAdapterPresentationOnError(civicNumber, new ExternalAuthenticationException("BankIDError_Expired", authContext));
            }

        }
    }

}
