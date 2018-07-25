using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Hackagram.Controllers
{
    [RequireHttps]
    public class HackagramController : Controller
    {
        // GET: Hackagram
        public ActionResult Index(string userHash = "", bool invalidLogin = false, string invalidLoginMessage = "")
        {
            ViewBag.Username = "";
            ViewBag.invalidLogin = invalidLogin;
            ViewBag.invalidLoginMessage = invalidLoginMessage;
            if (!string.IsNullOrEmpty(userHash))
            {
                string connection = "Server=tcp:latenightsecurity.database.windows.net,1433;Initial Catalog=Admin;Persist Security Info=False;User ID=Validator;Password=BasicPasswordForValidation!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                string expirationDateString = string.Empty;
                string userName = string.Empty;
                using (SqlConnection myConnection = new SqlConnection(connection))
                {
                    string oString = "SELECT [UserName], [HashExpireOn]  FROM [dbo].[Users] where LastUserHash = '" + userHash + "' ";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            expirationDateString = oReader["HashExpireOn"].ToString();
                            userName = oReader["UserName"].ToString();
                        }

                        myConnection.Close();
                    }
                }

                if (!string.IsNullOrEmpty(expirationDateString))
                {
                    if (DateTime.Now < DateTime.Parse(expirationDateString))
                        ViewBag.Username = userName;
                }

            }


            return View();
        }

        // GET: Hackagram/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Hackagram/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hackagram/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hackagram/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Hackagram/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hackagram/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Hackagram/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Hackagram/Delete/5
        [HttpPost]
        public ActionResult ValidateLogin(FormCollection collection)
        {

            //Has three vulns, Blind SQL Injection, Reflected SQL Injection and Pass the Hash
            string uName = collection["Username"].ToString();
            string pass = collection["Password"].ToString();
            bool userExists = false;
            string connection = "Server=tcp:latenightsecurity.database.windows.net,1433;Initial Catalog=Hackagram;Persist Security Info=False;User ID=Validator;Password=BasicPasswordForValidation!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            userExists = !string.IsNullOrEmpty(DBCall("select Username from Users where Username = '"+uName+"'", connection, "Username"));

            if(!userExists)
                return RedirectToAction("Index", new { userHash = "", invalidLogin = true, invalidLoginMessage = "User Does Not Exist" });

            if(Int32.Parse(DBCall("SELECT COUNT(*) as TotalCount FROM Users where [Username] = '"+uName+"' AND [Password] = '"+pass+"'", connection,"TotalCount")) > 0)
            {
                SHA256 hasher = SHA256Managed.Create(); ;
                string hash = GetSha256Hash(hasher, uName +DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                DateTime expireOn = DateTime.Now.AddHours(3);
                using (SqlConnection myConnection = new SqlConnection(connection))
                {
                    string oString = "Update Users SET LastUserHash = '" + hash+"' where Username ='" + uName+ "'; Update Users SET LastLogin = '" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "'  where Username ='" + uName + "'; Update Users SET HashExpireOn = '" + expireOn.ToLongDateString() + " "+expireOn.ToLongTimeString() + "'  where Username ='" + uName + "'";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                        }

                        myConnection.Close();
                    }
                }
                return RedirectToAction("Index", new { userHash = hash}); 
                
            }
            else
                return RedirectToAction("Index", new { userHash = "", invalidLogin = true, invalidLoginMessage = "Invalid Login" });
        }
        private string GetSha256Hash(SHA256 shaHash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        private string DBCall(string query, string connection, string columnName)
        {
             string results = string.Empty;
            using (SqlConnection myConnection = new SqlConnection(connection))
            {
                SqlCommand oCmd = new SqlCommand(query, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                            results = oReader[columnName].ToString();
                    }

                    myConnection.Close();
                }
            }
            return results;
        }

    }
}
