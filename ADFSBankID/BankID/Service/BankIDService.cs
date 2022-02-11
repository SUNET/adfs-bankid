using BankID.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using BankID.Model;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Net.Security;
using System.Net.Http.Headers;
using BankID.Utils;
using BankID.Interface;
using BankID.Exceptions;

namespace BankID.Service
{
    public class BankIDService : IBankIDService
    {
        private HttpClient _client;
        private readonly BankIDSettings _settings;
        public BankIDService(BankIDSettings settings)
        {
            _settings = settings;
            InitClient();
        }
        public void InitClient()
        {
            var rpCertThumbprint = _settings.RPCertificateThumbprint;
            var rpCert = CertificateUtils.LoadCertificateFromStore(rpCertThumbprint, StoreName.My, StoreLocation.LocalMachine) ??
                CertificateUtils.LoadCertificateFromStore(rpCertThumbprint, StoreName.My, StoreLocation.CurrentUser);

            var issuerCertThumbprint = _settings.IssuerCertThumbprint;
            var issuerCert = CertificateUtils.LoadCertificateFromStore(issuerCertThumbprint, StoreName.Root, StoreLocation.LocalMachine) ??
                CertificateUtils.LoadCertificateFromStore(issuerCertThumbprint, StoreName.Root, StoreLocation.CurrentUser);

            if (rpCert is null)
            {
                //TODO_log.Warn($"RP certificate is missing from store and cannot be appended to BankIDClient. Thumbprint provided: {rpCertThumbprint}");
                throw new Exception(string.Format("RP certificate is missing from store and cannot be appended to BankIDClient. Thumbprint provided: {0}", _settings.RPCertificateThumbprint));
            }

            if (issuerCert is null)
            {
                //TODO_log.Warn($"Issuer certificate is missing from store and cannot be appended to BankIDClient. Thumbprint provided: {issuerCertThumbprint}");
                throw new Exception(string.Format("Issuer certificate is missing from store and cannot be appended to BankIDClient. Thumbprint provided: {0}", _settings.IssuerCertThumbprint));
            }
            _client = new HttpClient(GetHttpHandler(rpCert, issuerCert));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;
            _client.BaseAddress = new Uri(_settings.BaseUrl);
            _client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Send an authentication request to initiate process.
        /// </summary>
        /// <param name="authBody"></param>
        /// <returns></returns>
        public async Task<BankIDAuthResponse> Auth(BankIDAuthRequestQR authBody)
        {
            try
            {
                using (var content = MakeStringContent(authBody))
                {
                    var response = await _client.PostAsync("auth", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"BankID.Auth with QR call unsuccessful. " +
                            response.ReasonPhrase +
                            " -- " +
                            response.Content.ReadAsStringAsync().Result
                        );
                    }
                    var res = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BankIDAuthResponse>(res);
                    //return await response.Content.r.ReadAsAsync<BankIDAuthResponse>();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to initiate BankID QR signin process.", e);
            }
        }


        /// <summary>
        /// Send an authentication request to initiate process.
        /// </summary>
        /// <param name="authBody"></param>
        /// <returns></returns>
        public async Task<BankIDAuthResponse> Auth(BankIDAuthRequestNIN authBody)
        {
            try
            {
                using (var content = MakeStringContent(authBody))
                {
                    var response = await _client.PostAsync("auth", content);
                    var res = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        var err = JsonConvert.DeserializeObject<BankIDError>(res);
                        throw new BankIDException(err.ErrorCode);
                        //throw new Exception($"BankID.Auth with NIN call unsuccessful. " +
                        //    response.ReasonPhrase +
                        //    " -- " +
                        //    response.Content.ReadAsStringAsync().Result
                        //);
                    }
                    
                    return JsonConvert.DeserializeObject<BankIDAuthResponse>(res);
                    //return await response.Content.ReadAsAsync<BankIDAuthResponse>();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to initiate BankID NIN signin process.", e);
            }
        }

        /// <summary>
        /// Polls current sign in status.
        /// RP should keep on calling collect every two seconds as long as status indicates pending.
        /// </summary>
        /// <param name="collectBody"></param>
        /// <returns></returns>
        public async Task<BankIDCollectResponse> Collect(BankIDCollectRequest collectBody)
        {
            try
            {
                using (var content = MakeStringContent(collectBody))
                {
                    var response = await _client.PostAsync("collect", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var ex = JsonConvert.DeserializeObject<BankIDCollectError>(await response.Content.ReadAsStringAsync());
                        var resp = new BankIDCollectResponse()
                        {
                            Status = "cancelled"
                        };
                        return resp;
                        //throw new Exception($"BankID.Collect call unsuccessful. " +
                        //    response.ReasonPhrase +
                        //    " -- " +
                        //    response.Content.ReadAsStringAsync().Result
                        //);
                    }
                    var res = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BankIDCollectResponse>(res);
                    //return await response.Content.ReadAsAsync<BankIDCollectResponse>();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to collect BankID status.", e);
            }
        }

        /// <summary>
        /// Cancels signin process.
        /// </summary>
        /// <param name="cancelBody"></param>
        /// <returns></returns>
        public async Task<BankIDCancelError> Cancel(BankIDCancelRequest cancelBody)
        {
            try
            {
                using (var content = MakeStringContent(cancelBody))
                {
                    var response = await _client.PostAsync("collect", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"BankID.Collect call unsuccessful. " +
                            response.ReasonPhrase +
                            " -- " +
                            response.Content.ReadAsStringAsync().Result
                        );
                    }
                    else
                    {
                        var res = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<BankIDCancelError>(res);
                        //return await response.Content.ReadAsAsync<BankIDCancelError>();
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Unable to cancel BankID signin process.", e);
            }
        }

        /// <summary>
        /// Wrapping HttpClient with a custom httpHandler.
        /// </summary>
        /// <param name="rpCertificate">Provided from BankID.</param>
        /// <returns>HttpClientHandler</returns>
        private static HttpClientHandler GetHttpHandler(X509Certificate2 rpCertificate, X509Certificate2 issuerCertificate)
        {
            if (rpCertificate == null)
            {
                throw new Exception("No rpCertificate Provided.");
            }

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = GetCertificateValidator(issuerCertificate)
            };
            handler.ClientCertificates.Add(rpCertificate);

            return handler;
        }

        /// <summary>
        /// Serialize object, initiate StringContent and remove CharSet 
        /// from header.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private StringContent MakeStringContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            /*
             * Adding a "charset" parameter after
             * 'application/json' is not allowed since the
             * MIME type "application/json" has neither
             * optional nor required parameters.
             * See implementation reference for UnsupportedMediaType 
             * error.
             */
            content.Headers.ContentType.CharSet = "";
            content.Headers.ContentType.MediaType = "application/json";
            return content;
        }

        /// <summary>
        /// Validating receiving certificate.
        /// </summary>
        /// <param name="issuerCertificate"></param>
        /// <returns></returns>
        private static Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> GetCertificateValidator(X509Certificate2 issuerCertificate)
        {
            return (HttpRequestMessage msg, X509Certificate2 cert, X509Chain chain, SslPolicyErrors policy) =>
            {
                // Adding known/trusted root.
                chain.ChainPolicy.ExtraStore.Add(issuerCertificate);
                // Will allow BankId as CA.
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;

                bool isChainValid = chain.Build(cert);

                if (!isChainValid)
                {
                    string[] errors = chain.ChainStatus
                        .Select(x => String.Format("{0} ({1})", x.StatusInformation.Trim(), x.Status))
                        .ToArray();
                    string certificateErrorsString = "Unknown errors.";

                    if (errors?.Length > 0)
                    {
                        certificateErrorsString = String.Join(", ", errors);
                    }

                    throw new Exception("Trust chain did not complete to the known authority anchor. Errors: " + certificateErrorsString);
                }

                // This piece makes sure it actually matches your known root (thumbnail matching).
                var valid = chain.ChainElements
                    .Cast<X509ChainElement>()
                    .Any(x => x.Certificate.RawData.SequenceEqual(issuerCertificate.RawData));

                if (!valid)
                {
                    throw new Exception("Trust chain did not complete to the known authority anchor. Certificate did not match known valid certificates.");
                }

                return true;
            };
        }

        public void Dispose()
        {
            this._client = null;
        }

        public bool ValidAuthResponse(string status)
        {
            ValidAuthStatusCodes statusCode;
            if(Enum.TryParse<ValidAuthStatusCodes>(status,out statusCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CancelledAuthResponse(string status)
        {
            InvalidAuthStatusCodes statusCode;

            if (Enum.TryParse<InvalidAuthStatusCodes>(status, out statusCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
