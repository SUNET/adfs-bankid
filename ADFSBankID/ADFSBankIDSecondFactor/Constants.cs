using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADFSBankIDSecondFactor
{
    public class Constants
    {
        public const string BANKIDMFA = "http://bankid.com/mfa";
        public const string UPNCLAIMTYPE = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
        public const string CLIENTIP = "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-client-ip";
        public const string FORWARDEDCLIENTIP = "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-forwarded-client-ip";
        public const string ADMINFRIENDLYNAME = "BankID";

        public static class AuthContextKeys
        {
            public const string SessionId = "sessionid";
            public const string Identity = "id";
            public const string CivicNumber = "nin";
        }

        public static class DynamicContentLabels
        {
            public const string markerPageBankIdInstruction = "%PageBankIdInstruction%";
            public const string markerPageBankIdIntroductionTitle = "%PageBankIdIntroductionTitle%";
            public const string markerPageBankIdIntroductionText = "%PageBankIdIntroductionText%"; 
            public const string markerSubmitButton = "%PageSubmitButtonLabel%";
            public const string markerPageBankIDCivicNumberInPut = "%CivicNumberInput%";
            public const string markerPageLoginMessage = "%PageLoginMessage%";
        }

        public static class ResourceNames
        {
            public const string AdminFriendlyName = "AdminFriendlyName";
            public const string Description = "Description";
            public const string FriendlyName = "FriendlyName";
            public const string ErrorInvalidSessionId = "ErrorInvalidSessionId";
            public const string ErrorInvalidContext = "ErrorInvalidContext";
            public const string ErrorNoUserIdentity = "ErrorNoUserIdentity";
            public const string ErrorNoAnswerProvided = "ErrorNoAnswerProvided";
            public const string CivicNumber = "CivicNumber";
            public const string AuthPageBankIDTemplate = "LoginBankIDHtml";
            public const string PageTitle = "PageTitle";
            public const string PageBankIdInstruction = "PageBankIdInstruction";
            public const string PageBankIdIntroductionTitle = "PageBankIdIntroductionTitle";
            public const string PageBankIdIntroductionText = "PageBankIdIntroductionText";
            public const string SubmitButtonLabel = "SubmitButtonLabel";
        }

        public static class PropertyNames
        {
            //public const string Username = "Username";
            //public const string View = "View";
            public const string CivicNumber = "CivicNumberInput";
        }

        public static class Lcid
        {
            public const int En = 0x9;
            public const int Sv = 0x1D;

        }

    }
}
