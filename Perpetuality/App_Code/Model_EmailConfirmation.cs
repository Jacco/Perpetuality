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
        public string RecipientEmail = "jacco@jaap.nl";
        public string ConfirmHash;
        public Model_EmailConfirmation(string id, string hash)
        {
            var ctx = new DatabaseDataContext();
            ConfirmHash = hash;
        }
    }
}