using BankID.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID
{
    public class BankIDHintCodeTranslator : IBankIDHintCodeTranslator
    {
        private readonly Dictionary<string, Dictionary<string, string>> _dictionary;
        private readonly Dictionary<string, string> _englishDictionary;
        private readonly Dictionary<string, string> _swedishDictionary;

        /// <summary>
        /// Initiates dictionary. Possible to extend dictonary:
        /// <languageAsString, <key, value>>.
        /// 
        /// This dictionary is created for a quick setup of BankID. The dictionary 
        /// supports a manual BankID initiation(start BankID app manually) and 
        /// an animated QR code flow. If there are requirements deviating from 
        /// afore mentioned assumptions extend or overwrite dictionary or make 
        /// your own translation implementation.
        /// </summary>
        /// <param name="extendDictionary"></param>
        public BankIDHintCodeTranslator(IDictionary<string, IDictionary<string, string>> extendDictionary = null)
        {
            _dictionary = new Dictionary<string, Dictionary<string, string>>();

            _englishDictionary = new Dictionary<string, string>();
            _swedishDictionary = new Dictionary<string, string>();


            // Reference: https://www.bankid.com/assets/bankid/rp/bankid-relying-party-guidelines-v3.5.pdf
            // Collect - pending orders 
            _englishDictionary.Add("outstandingTransaction", "Start your BankID app.");
            _swedishDictionary.Add("outstandingTransaction", "Starta BankID-appen.");
            _englishDictionary.Add("noClient", "Start your BankID app.");
            _swedishDictionary.Add("noClient", "Starta BankID-appen.");
            _englishDictionary.Add("started", "Searching for BankID:s, it may take a little while… If a few seconds have passed and still no BankID has been found, you probably don’t have a BankID which can be used for this identification/signing on this device. If you don’t have a BankID you can order one from your internet bank.");
            _swedishDictionary.Add("started", "Söker efter BankID, det kan ta en liten stund… Om det har gått några sekunder och inget BankID har hittats har du sannolikt inget BankID som går att använda för den aktuella identifieringen/underskriften i den här enheten. Om du inte har något BankID kan du hämta ett hos din internetbank.");
            _englishDictionary.Add("userSign", "Enter your security code in the BankID app and select Identify or Sign.");
            _swedishDictionary.Add("userSign", "Skriv in din säkerhetskod i BankIDappen och välj Legitimera eller Skriv under.");

            // Collect - failed orders
            _englishDictionary.Add("expiredTransaction", "The BankID app is not responding. Please check that the program is started and that you have internet access. If you don’t have a valid BankID you can get one from your bank. Try again.");
            _swedishDictionary.Add("expiredTransaction", "BankID-appen svarar inte. Kontrollera att den är startad och att du har internetanslutning. Om du inte har något giltigt BankID kan du hämta ett hos din Bank. Försök sedan igen.");
            _englishDictionary.Add("certificateErr", "The BankID you are trying to use is revoked or too old. Please use another BankID or order a new one from your internet bank.");
            _swedishDictionary.Add("certificateErr", "Det BankID du försöker använda är för gammalt eller spärrat. Använd ett annat BankID eller hämta ett nytt hos din internetbank.");
            _englishDictionary.Add("userCancel", "Action cancelled.");
            _swedishDictionary.Add("userCancel", "Åtgärden avbruten.");
            _englishDictionary.Add("cancelled", "Action cancelled. Please try again.");
            _swedishDictionary.Add("cancelled", "Åtgärden avbruten. Försök igen.");
            _englishDictionary.Add("startFailed", "Failed to scan the QR code. Start the BankID app and scan the QR code. Check that the BankID app is up to date. If you don't have the BankID app, you need to install it and order a BankID from your internet bank. Install the app from your app store or https://install.bankid.com.  ");
            _swedishDictionary.Add("startFailed", "Misslyckades att läsa av QR koden. Starta BankID-appen och läs av QR koden. Kontrollera att BankID-appen är uppdaterad. Om du inte har BankID-appen måste du installera den och hämta ett BankID hos din internetbank. Installera appen från din appbutik eller https://install.bankid.com.");

            _dictionary.Add("en", _englishDictionary);
            _dictionary.Add("sv", _swedishDictionary);

            this.ExtendDictionary(extendDictionary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hintCode"></param>
        /// <param name="language">en/sv</param>
        /// <returns></returns>
        public string TranslateHintCode(string hintCode, string language)
        {
            if (hintCode == null)
            {
                return null;
            }

            if (!_dictionary.ContainsKey(language))
            {
                throw new ArgumentException($"Unsupported language \"{language}\" provided.");
            }

            var languageDictionary = _dictionary[language];

            if (!languageDictionary.ContainsKey(hintCode))
            {
                throw new ArgumentException($"Unsupported hintCode \"{hintCode}\". Please contact administrator to update hintCode translations.");
            }

            return languageDictionary[hintCode];
        }

        /// <summary>
        /// Extend predefined dictionary.
        /// </summary>
        /// <param name="extendDictionary"></param>
        private void ExtendDictionary(IDictionary<string, IDictionary<string, string>> extendDictionary)
        {
            if (extendDictionary == null)
            {
                return;
            }

            extendDictionary.Keys
                .ToList()
                .ForEach(language => {

                    if (!_dictionary.ContainsKey(language))
                    {
                        _dictionary.Add(language, new Dictionary<string, string>());
                    }

                    extendDictionary[language].Keys
                        .ToList()
                        .ForEach(hintCode => {
                            _dictionary[language].Add(hintCode, extendDictionary[language][hintCode]);
                        });
                }); 
        }
    }
}
