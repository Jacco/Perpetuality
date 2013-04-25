using Perpetuality.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Perpetuality.App_Code
{
    public class Model_EmailConfirmation
    {
        public string RecipientEmail = "";
        public string ConfirmHash;
        public Model_EmailConfirmation(string id)
        {
            var ctx = new DatabaseDataContext();
            string email = "";
            ctx.GetUserEmail(long.Parse(id), ref email);
            RecipientEmail = email;
            ConfirmHash = ctx.GetConfirmationHash(email).SingleOrDefault().strConfirmHash;
        }
    }
}