using ADFSBankIDSecondFactor.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADFSBankIDSecondFactor
{
    public static class ResourceHandler
    {
        public static string GetResource(string resourceName, int lcid)
        {
            if (lcid != Constants.Lcid.En && lcid != Constants.Lcid.Sv)
            {
                lcid = Constants.Lcid.Sv;
            }
            LangText text = (from tt in texts.Where(t => t.Key == resourceName && t.Lcid == lcid) select tt).SingleOrDefault();
            if (text == null)
            {
                throw new ArgumentNullException();

            }
            return text.Value;
            //if (String.IsNullOrEmpty(resourceName))
            //{
            //    throw new ArgumentNullException("resourceName");
            //}

            //return StringResources.ResourceManager.GetString(resourceName, new CultureInfo(lcid));
        }
        public static string GetPresentationResource(string resourceName, int lcid)
        {
            if (String.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }
            return PresentationResource.ResourceManager.GetString(resourceName, new CultureInfo(lcid));
        }

        private static List<LangText> texts =
            new List<LangText>() {
            //en
            new LangText(){Key="AdminFriendlyName",Lcid=9,Value="BankID"},
            new LangText(){Key="BankIDError_Expired",Lcid=9,Value="BankID login request has expired."},
            new LangText(){Key="BankIDError_NoCivicNumber",Lcid=9,Value="Can´t proceed with BankID, no civicnumber found."},
            new LangText(){Key="BankIDError_NoMatch",Lcid=9,Value="Civic number doesn´t match in BankID."},
            new LangText(){Key="CivicNumber",Lcid=9,Value="Civic number"},
            new LangText(){Key="Description",Lcid=9,Value="BankID as second factor"},
            new LangText(){Key="ErrorInvalidContext",Lcid=9,Value="ErrorInvalidContext"},
            new LangText(){Key="ErrorInvalidSessionId",Lcid=9,Value="ErrorInvalidSessionId"},
            new LangText(){Key="ErrorMissingCivicNo",Lcid=9,Value="No civicnumber found!"},
            new LangText(){Key="ErrorNoAnswerProvided",Lcid=9,Value="ErrorNoAnswerProvided"},
            new LangText(){Key="ErrorNoUserIdentity",Lcid=9,Value="ErrorNoUserIdentity"},
            new LangText(){Key="FriendlyName",Lcid=9,Value="BankID"},
            new LangText(){Key="PageBankIdInstruction",Lcid=9,Value="Launch the BankID application on your phone and complete the login"},
            new LangText(){Key="PageBankIdIntroductionText",Lcid=9,Value="Complete the login by verifying your personal identity with BankID. Your Swedish personal identity number is shown below"},
            new LangText(){Key="PageBankIdIntroductionTitle",Lcid=9,Value="Enter civic number"},
            new LangText(){Key="PageTitle",Lcid=9,Value="BankID MFA"},
            new LangText(){Key="SubmitButtonLabel",Lcid=9,Value="Sign in"},
                //sv
            new LangText(){Key="AdminFriendlyName",Lcid=29,Value="BankID"},
            new LangText(){Key="BankIDError_Expired",Lcid=29,Value="Inloggningsbegäran mot BankID har upphört att gälla."},
            new LangText(){Key="BankIDError_NoCivicNumber",Lcid=29,Value="Kan inte gå vidare med Bankidinloggning, kan inte hitta något personnummer."},
            new LangText(){Key="BankIDError_NoMatch",Lcid=29,Value="Ingen matchning på personnummer i BankID."},
            new LangText(){Key="CivicNumber",Lcid=29,Value="Personnummer"},
            new LangText(){Key="Description",Lcid=29,Value="BankID som andra faktor"},
            new LangText(){Key="ErrorInvalidContext",Lcid=29,Value="ErrorInvalidContext"},
            new LangText(){Key="ErrorInvalidSessionId",Lcid=29,Value="ErrorInvalidSessionId"},
            new LangText(){Key="ErrorMissingCivicNo",Lcid=29,Value="Inget personnummer kunde hittas!"},
            new LangText(){Key="ErrorNoAnswerProvided",Lcid=29,Value="ErrorNoAnswerProvided"},
            new LangText(){Key="ErrorNoUserIdentity",Lcid=29,Value="ErrorNoUserIdentity"},
            new LangText(){Key="FriendlyName",Lcid=29,Value="BankID"},
            new LangText(){Key="PageBankIdInstruction",Lcid=29,Value="Öppna appen BankID i din mobiltelefon och följ instruktionerna för att logga in"},
            new LangText(){Key="PageBankIdIntroductionText",Lcid=29,Value="Slutför inloggningen genom att verifiera din personliga identitet med BankID. Ditt personnummer visas nedan."},
            new LangText(){Key="PageBankIdIntroductionTitle",Lcid=29,Value="Skriv in personnummer"},
            new LangText(){Key="PageTitle",Lcid=29,Value="BankID MFA"},
            new LangText(){Key="SubmitButtonLabel",Lcid=29,Value="Logga in"},
            };
       
    }
}
