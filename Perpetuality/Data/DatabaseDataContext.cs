using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Perpetuality.Data
{
    public static class ExceptionHelper
    {
        public static int ErrorCode(this Exception ex)
        {
            var m = new Regex(@"^[\d]+").Match(ex.Message);
            if (m.Success)
                return int.Parse(m.Value);
            else
                return -1;
        }

	    internal static int AddInputParameter(this IDbCommand cmd, string name, string value)
	    {
		    var p = cmd.CreateParameter();
		    p.ParameterName = name;
		    p.Value = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
		    return cmd.Parameters.Add(p);
	    }
    }

    public partial class DatabaseDataContext
    {
        private static string GenerateConfirmationHash(string email)
        {
            if (!string.IsNullOrEmpty(email))
                return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(email)));
            else
                throw new ApplicationException("Error generating confirmation hash.");
        }

        public long GetUserIDByEmail(string userName)
        {
            bool? confirmed = null;
            long? id = null;
            try
            {
                FindUser(userName, "empty", ref id, ref confirmed);
            }
            catch(Exception e)
            {
                throw new ApplicationException("61998 Get user ID by email failed.");
            }
            if (id.HasValue)
            {
                return id.Value;
            }
            else
            {
                throw new ApplicationException("61998 Get user ID by email failed.");
            }
        }

        private static string NormalizeEmailAddress(string email)
        {
            var test = new MailAddress(email);
            return test.User + "@" + test.Host.ToLower();
        }

        public long RegisterNewUser(string userName, string password, bool partnerMail, string redirectUrl)
        {
            long? userID = null;
            userName = userName.Trim();
            password = password.Trim();
            // validate the email address
            try
            {
                userName = NormalizeEmailAddress(userName);
            }
            catch
            {
                throw new ApplicationException("60001 The supplied username is not a valid email address.");
            }
            // validate the password
            if (string.IsNullOrEmpty(password) | password.Length < 6)
            {
                throw new ApplicationException("60002 The supplied password is empty or too short.");
            }
            // store in DB
            var conifrmationpwd = GenerateConfirmationHash(userName);
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            var tran = Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            Transaction = tran;
            try
            {
                if (RegisterNewUser(userName, password, conifrmationpwd, partnerMail, ref userID) == 0)
                    throw new ApplicationException("60003 Registering new user failed.");
                // send a confirmation mail
                try
                {
                    SendConfirmationMail(new MailAddress(userName), "", conifrmationpwd, "REGISTER_NEW_USER", redirectUrl);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("60005 Sending confirmation mail failed.", e);
                }
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            if (!userID.HasValue)
                throw new ApplicationException("60005 Sending confirmation mail failed.");
            return userID.Value;
        }

        private void SendConfirmationMail(MailAddress mailAddress, string p1, string conifrmationpwd, string p2, string redirectUrl)
        {
            // TODO SEND A MAIL
        }

        public void ConfirmEmailAddress(string hash)
        {
            if (_ConfirmEmailAddress(hash) == 0)
                    throw new ApplicationException("60010 Email activation failed.");
        }

        public string LoginUser(string userName, string password, string ipAddress)
        {
            userName = userName.Trim();
            try
            {
                userName = NormalizeEmailAddress(userName);
            }
            catch
            {
                throw new ApplicationException("60020 The supplied username is not a valid email address.");
            }
            password = password.Trim();
            Guid? session = Guid.Empty;
            if (AuthenticateUser(userName, password, ipAddress, ref session) == 0)
                throw new ApplicationException("60023 Login user failed.");
            if (!session.HasValue | session == Guid.Empty)
            {
                throw new ApplicationException("60023 Login user failed.");
            }
            return session.Value.ToString();
        }

        public void LogoutUser(string token, string ipAddress)
        {
            Guid id = Guid.Empty;
            try
            {
                id = new Guid(token);
            }
            catch
            {
                throw new ApplicationException("60030 Supplied token could not be converted to a guid.");
            }
            if (EndSession(id, ipAddress) == 0)
                throw new ApplicationException("60031 Logging out user failed.");
        }

        public string GetSetting(string name)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "select strValue from tblSetting where strName = @Name";
                cmd.AddInputParameter("@Name", name);
                return (string)cmd.ExecuteScalar();
            }
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(Connection.ConnectionString);
        }

        public GetUserProfileResult GetUserProfile(string token, string ipAddress)
        {
            Guid id = Guid.Empty;
            try
            {
              id = new Guid(token);
            }
            catch
            {
              throw new ApplicationException("60100 Supplied token could not be converted to a guid.");
            }
            var ctx = new DatabaseDataContext();
            var x = ctx._GetUserProfile(id, ipAddress);
            if ((int)x.ReturnValue == 0)
                throw new ApplicationException("60102 Get user profile failed.");
            return x.FirstOrDefault();
        }
    }
}