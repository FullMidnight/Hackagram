using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hackagram.DAL;
using Hackagram.Models;

namespace Hackagram.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private AdminContext adminContext = new AdminContext();
        /// <summary>
        /// Add user's claims to viewbag
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;    //You get the user’s first and last name below:
            ViewBag.Name = userClaims?.FindFirst("name")?.Value;

            // The 'preferred_username' claim can be used for showing the username
            ViewBag.Username = userClaims?.FindFirst("preferred_username")?.Value;

            // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // TenantId is the unique Tenant Id - which represents an organization in Azure AD
            ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            return View();
        }

        public ActionResult AddUser()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

            var firstName = userClaims?.FindFirst("name")?.Value?.Split(' ')[0];
            var lastName = userClaims?.FindFirst("name")?.Value?.Split(' ')[1];
            var user = new User(firstName, lastName);
            adminContext.Users.Add(user);
            adminContext.SaveChanges();

            return View("Index");
        }
    }
}