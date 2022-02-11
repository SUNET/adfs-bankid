using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ADFSBankID.Application.Settings
{
    [DataContract]
    public class SqlSettings
    {
        [DataMember]
        public List<SqlSetting> Settings { get; set; }
    }

}
