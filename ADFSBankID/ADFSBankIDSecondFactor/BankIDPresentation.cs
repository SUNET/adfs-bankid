using ADFSBankID.Application.Utils;
using Microsoft.IdentityServer.Web.Authentication.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADFSBankIDSecondFactor
{
    public class BankIDPresentation : IAdapterPresentationForm
    {
        private readonly ExternalAuthenticationException _ex = null;
        private readonly string _civicNumber;
        private readonly Dictionary<string, string> _dynamicContents = new Dictionary<string, string>()
        {
            //{Constants.DynamicContentLabels.markerPageBanner, String.Empty},
            //{Constants.DynamicContentLabels.markerOverallError, String.Empty},
            //{Constants.DynamicContentLabels.markerActionUrl, String.Empty},
            {Constants.DynamicContentLabels.markerSubmitButton, String.Empty},
            {Constants.DynamicContentLabels.markerPageBankIdInstruction, String.Empty},
            {Constants.DynamicContentLabels.markerPageBankIdIntroductionTitle, String.Empty},
            {Constants.DynamicContentLabels.markerPageBankIdIntroductionText, String.Empty},
            {Constants.DynamicContentLabels.markerPageLoginMessage,String.Empty },
        };
        public BankIDPresentation(string civicNumber)
        {
            _civicNumber = civicNumber;
        }
        public BankIDPresentation(string civicnumber, ExternalAuthenticationException ex)
        {
            _civicNumber = civicnumber;
            _ex = ex;
        }
        public string GetFormHtml(int lcid)
        {
            var dynamicContents = new Dictionary<string, string>(_dynamicContents)
            {
                [Constants.DynamicContentLabels.markerPageBankIdIntroductionText] =
                GetResource(Constants.ResourceNames.PageBankIdIntroductionText,lcid),
                [Constants.DynamicContentLabels.markerPageBankIdInstruction] =
                GetResource(Constants.ResourceNames.PageBankIdInstruction,lcid),
                [Constants.DynamicContentLabels.markerPageBankIdIntroductionTitle]=
                GetResource(Constants.ResourceNames.PageBankIdIntroductionTitle,lcid),
                [Constants.DynamicContentLabels.markerPageBankIDCivicNumberInPut] = MaskCivicnumber(_civicNumber),
                [Constants.DynamicContentLabels.markerSubmitButton] =
                GetResource(Constants.ResourceNames.SubmitButtonLabel, lcid),
            };
            if (_ex != null)
            {
                //_ex.Message
                Log.WriteEntry("BankID presentationform error: " + _ex.Message, EventLogEntryType.Error, 335);
                dynamicContents[Constants.DynamicContentLabels.markerPageLoginMessage] = GetResource(_ex.Message, lcid);
                if (_ex.Context != null)
                {
                    //dynamicContents[Constants.DynamicContentLabels.markerPageFrejaCivicNumberInPut] = _ex.Context.Data["CivicNumber"].ToString();
                }
            }
            string authPageTemplate = ResourceHandler.GetPresentationResource(Constants.ResourceNames.AuthPageBankIDTemplate, lcid);
            Log.WriteEntry("BankID presentationform med pnr: " + _civicNumber, EventLogEntryType.Information, 335);
            return Replace(authPageTemplate, dynamicContents);
        }

        /// <summary>
        /// Replace template markers with explicitly given replacements.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replacements"></param>
        /// <returns></returns>
        private static string Replace(string input, Dictionary<string, string> replacements)
        {
            if (string.IsNullOrEmpty(input) || null == replacements)
            {
                return input;
            }

            // Use StringBuiler and allocate buffer 3 times larger
            StringBuilder sb = new StringBuilder(input, input.Length * 3);
            foreach (string key in replacements.Keys)
            {
                sb.Replace(key, replacements[key]);
            }
            return sb.ToString();
        }

        #region IAdapterPresentationIndirect Members
        public string GetFormPreRenderHtml(int lcid)
        {
            return null;
        }

        public string GetPageTitle(int lcid)
        {
            return GetResource(Constants.ResourceNames.PageTitle, lcid);
        }
        #endregion

        protected string GetResource(string resourceName, int lcid)
        {
            return ResourceHandler.GetResource(resourceName, lcid);
        }
        private string MaskCivicnumber(string civicNumber)
        {
            return civicNumber.Substring(0, 8) + "XXXX";
        }
    }
}
