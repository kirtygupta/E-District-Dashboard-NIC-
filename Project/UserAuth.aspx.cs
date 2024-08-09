using System;
using System.Text;
using System.Security.Cryptography;
using Npgsql;

namespace CombinedProject
{
    public partial class UserAuth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userId = txtUserId.Text.Trim();
            string password = txtPassword.Text.Trim();
            string hashedPassword = GenerateSHA512(password);

            if (AuthenticateUser(userId, hashedPassword))
            {
                Session["UserAuthenticated"] = true;
                Response.Redirect("Default.aspx");
            }
            else
            {
                lblMessage.Text = "User ID or Password is incorrect. Please try again.";
            }
        }

        public static string GenerateSHA512(string inputStr)
        {
            StringBuilder _hashBuilder = new StringBuilder();
            try
            {
                using (SHA512Managed HashTool = new SHA512Managed())
                {
                    Byte[] PasswordAsByte = System.Text.Encoding.UTF8.GetBytes(inputStr);
                    byte[] hash = HashTool.ComputeHash(PasswordAsByte);
                    HashTool.Clear();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        _hashBuilder.Append(hash[i].ToString("X2"));
                    }
                }
            }
            catch (Exception ex)
            {
                string _strErr = "Error in HashCode : " + ex.Message;
            }
            return _hashBuilder.ToString().ToLower();
        }

        private bool AuthenticateUser(string userId, string hashedPassword)
        {
            bool isAuthenticated = false;

            string connString = "Host=10.128.119.22;Username=postgres;Password=postgres;Database=Edistrict";
            string query = "SELECT COUNT(*) FROM usermaster WHERE userid = @userid AND password = @passwd";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.Parameters.AddWithValue("@passwd", hashedPassword);

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                isAuthenticated = (count > 0);
            }

            return isAuthenticated;
        }
    }
}