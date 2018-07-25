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
    //private class Value
    //{
    //    string 
    //}
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
        public JsonResult ValidateAnswer(string excerciseName, int questionNumber, string answer)
        {

            string userEmail = User.Identity.Name;
            bool correct = false;
            if (excerciseName == "Hackagram" && questionNumber == 2)
            {
                //Check for HTML and Javascript
                if (answer.Contains("<script>") && answer.Contains("</script>"))
                {
                    correct = true;
                    //Add SQL call to insert  for questions answered.
                    var qAnswered = new QuestionAnswered(excerciseName,userEmail,questionNumber);
                    adminContext.QuestionsAnswered.Add(qAnswered);
                    adminContext.SaveChanges();
                }
            }
            else
            {
                string answerFromDB = (from q in adminContext.Questions
                              where q.Excercise == excerciseName && q.QuestionNumber == questionNumber
                              select q.Answer).First();

                if (answer == answerFromDB)
                {
                    correct = true;
                    //Add SQL call to insert  for questions answered.
                    var qAnswered = new QuestionAnswered(excerciseName, userEmail, questionNumber);
                    adminContext.QuestionsAnswered.Add(qAnswered);
                    adminContext.SaveChanges();
                }

            }



            JsonResult r = new JsonResult();
            return new JsonResult();
        }
        [HttpPost]
        public JsonResult ValidateAnswer(Question question)
        {

            string userEmail = User.Identity.Name;
            bool correct = false;
            if (question.Excercise == "Hackagram" && question.QuestionNumber == 2)
            {
                //Check for HTML and Javascript
                if (question.Answer.Contains("<script>") && question.Answer.Contains("</script>"))
                {
                    correct = true;
                    //Add SQL call to insert  for questions answered.
                    var qAnswered = new QuestionAnswered(question.Excercise, userEmail, question.QuestionNumber);
                    adminContext.QuestionsAnswered.Add(qAnswered);
                    adminContext.SaveChanges();
                }
            }
            else
            {
                string answerFromDB = (from q in adminContext.Questions
                                       where q.Excercise == question.Excercise && q.QuestionNumber == question.QuestionNumber
                                       select q.Answer).First();

                if (question.Answer == answerFromDB)
                {
                    correct = true;
                    //Add SQL call to insert  for questions answered.
                    var qAnswered = new QuestionAnswered(question.Excercise, userEmail, question.QuestionNumber);
                    adminContext.QuestionsAnswered.Add(qAnswered);
                    adminContext.SaveChanges();
                }

            }



            JsonResult r = new JsonResult();
            return new JsonResult();
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