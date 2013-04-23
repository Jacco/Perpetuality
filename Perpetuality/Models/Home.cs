using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Perpetuality.Models
{
    public class Profile
    {
        public Profile() { }

        public Profile(Data.GetUserProfileResult profile)
        {
            EmailAddress = profile.strEmailAddress;
            Name = profile.strName;
            Language = profile.strLanguage;
        }

        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
    }
}