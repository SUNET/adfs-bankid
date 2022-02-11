using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADFSBankID.Application.Interface
{
    public interface IPersonService
    {
        string GetCivicNumber(string uid);
    }
}
