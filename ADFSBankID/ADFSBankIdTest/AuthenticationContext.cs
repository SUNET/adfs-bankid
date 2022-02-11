using Microsoft.IdentityServer.Web.Authentication.External;
using System.Collections.Generic;

namespace ADFSBankIdTest
{
    public class AuthenticationContext : IAuthenticationContext
    {
        public AuthenticationContext()
        {
            //ActivityId = activityId;
        }
        public string ActivityId { get; set; }

        public string ContextId { get; set; }

        public int Lcid { get; set; }

        public Dictionary<string, object> Data { get; set; }
    }
}
