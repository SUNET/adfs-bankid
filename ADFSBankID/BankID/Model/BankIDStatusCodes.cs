using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Model
{
    public enum AuthStatusCodes
    {
        pending,
        cancelled,
        complete,
        failed
    }
    public enum ValidAuthStatusCodes
    {
        complete
    }
    public enum InvalidAuthStatusCodes
    {
        cancelled,
        failed
    }
}
