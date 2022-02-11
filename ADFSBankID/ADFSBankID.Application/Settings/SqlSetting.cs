using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ADFSBankID.Application.Settings
{
    public class SqlSetting
    {
        [DataMember]
        public string ConnectionString { get; set; }
        [DataMember]
        public string Command { get; set; }
    }
}
