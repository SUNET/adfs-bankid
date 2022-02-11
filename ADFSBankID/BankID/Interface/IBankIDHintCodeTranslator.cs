using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Interface
{
    public interface IBankIDHintCodeTranslator
    {
        string TranslateHintCode(string hintCode, string language);
    }
}
