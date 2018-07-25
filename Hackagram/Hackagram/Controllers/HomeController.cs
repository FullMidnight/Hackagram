using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Hackagram.DAL;
using Hackagram.Models;

namespace Hackagram.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private AdminContext adminContext = new AdminContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Questions(string excerciseName = "")
        {
            List<Question> questions = new List<Question>();
            if (!string.IsNullOrEmpty(excerciseName))
            {
                questions = (from b in adminContext.Questions
                            where b.Excercise.Equals(excerciseName)
                            select b).ToList();

            }
            ViewBag.excercise = excerciseName;
            ViewBag.questions =  questions;
            return View();
        }

        [HttpPost]
        public JsonResult ValidateAnswer(Question question)
        {
            return null;
        }

        /// <summary>
        /// Send an OpenID Connect sign-in request.
        /// Alternatively, you can just decorate the SignIn method with the [Authorize] attribute
        /// </summary>
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}