using Infrastructure.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace UsersEdit.Authentication
{
    public class UserIdentity : IIdentity
    {
        public UserIdentity(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                Invalidate();
                return;
            }

            var data = JsonConvert.DeserializeObject<UserCookie>(ticket.UserData);

            if (data == null)
            {
                Invalidate();
                return;
            }


            foreach (PropertyInfo prop in data.GetType().GetProperties())
            {
                PropertyInfo thisProp = this.GetType().GetProperty(prop.Name);
                var dataValue = prop.GetValue(data);
                thisProp.SetValue(this, dataValue);
            }
        }

        private void Invalidate()
        {
            Login = String.Empty;
            Role = String.Empty;
        }

        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
        public bool RememberMe { get; set; }
        public string Email { get; set; }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return (Login != String.Empty && Role != String.Empty); }
        }

        public string Name
        {
            get { return Login; }
        }
    }
}