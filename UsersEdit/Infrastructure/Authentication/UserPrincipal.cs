using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace UsersEdit.Authentication
{
    public class UserPrincipal : IPrincipal
    {
        private UserIdentity identity;

        public UserPrincipal(UserIdentity identity) 
        {
            this.identity = identity;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public UserIdentity Information
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return string.Compare(identity.Role, role, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}