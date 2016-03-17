using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.Azure.Authentication;

namespace Zen.Infrastructure.ActiveDirectory
{
    public class Class1
    {
        private readonly string _clientId;

        public Class1(string clientId)
        {
            _clientId = clientId;
        }

        public Task SignupAsTenant()
        {
            var context = new AuthenticationContext("https://login.microsoftonline.com/401174ab-9fc9-4c84-ab0d-8ed037398501/");
        }
    }
}
