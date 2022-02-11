using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Model
{
    public class BankIDAuthClientResponse
    {
        public string OrderRef { get; set; }
        public string AutoStartToken { get; set; }
    }
}
