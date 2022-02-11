using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Exceptions
{
    public class BankIDException : Exception
    {
        public BankIDException()
        {

        }
        public BankIDException(string message)
        : base(message)
        {

        }
        public BankIDException(string message, Exception inner)
        : base(message, inner)
        {

        }
    }
}
