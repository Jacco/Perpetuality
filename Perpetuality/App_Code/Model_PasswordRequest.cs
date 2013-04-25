using Perpetuality.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Perpetuality.App_Code
{
    public class Model_PasswordRequest
    {
        public string RecipientEmail = "";
        public string Password;
        public Model_PasswordRequest(string id)
        {
            var IDs = id.Split(',');

            var ctx = new DatabaseDataContext();
            string email = "";
            ctx.GetUserEmail(long.Parse(IDs[0]), ref email);
            RecipientEmail = email;
            Password = IDs[1];
        }
    }
}