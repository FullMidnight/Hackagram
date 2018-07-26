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
using System.Security.Claims;

namespace Hackagram.Controllers
{
    [RequireHttps]
    [ValidateInput(false)]
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
        [Authorize]
        public ActionResult Questions(string excerciseName = "Hackagram")
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<Claim> claims = identity.Claims.ToList();
            string userEmail = claims.Where(c => c.Type == "preferred_username").First().Value;
            List<Question> questions = new List<Question>();
            var tempQ = (from q in adminContext.Questions
                         join aq in adminContext.AnsweredQuestions.Where(x => x.email == userEmail)
                         on q.ID equals aq.qID into gj
                         from x in gj.DefaultIfEmpty()
                         select new
                         {
                             q.ID,
                             q.Excercise,
                             q.Answer,
                             q.Points,
                             q.QuestionNumber,
                             q.QuestionText,
                             q.Hint,
                             Done = (x == null ? false : true)
                         });
            List<PersonalAnsweredQuestion> answerQuests = new List<PersonalAnsweredQuestion>();
            foreach (var q in tempQ)
            {
                if(!answerQuests.Exists(a => a.ID == q.ID))
                    answerQuests.Add(new PersonalAnsweredQuestion(q.ID, q.Excercise, q.QuestionNumber, q.Answer, q.QuestionText, q.Points, q.Hint, q.Done));
            }
            
            if (!string.IsNullOrEmpty(excerciseName))
            {
                questions = (from b in adminContext.Questions
                            where b.Excercise.Equals(excerciseName)
                            select b).ToList();

            }
            ViewBag.excercise = excerciseName;
            ViewBag.questions =  answerQuests;
            return View();
        }

        [HttpPost]
        public ActionResult ValidateAnswer(Question question)
        {
            try {
                var identity = (ClaimsIdentity)User.Identity;
                List<Claim> claims = identity.Claims.ToList();
                string userEmail = claims.Where(c => c.Type == "preferred_username").First().Value;
                bool correct = false;
                string hint = string.Empty;
                if (question.Excercise == "Hackagram" && question.QuestionNumber == 2)
                {
                    //Check for HTML and Javascript
                    if (question.Answer.Contains("<script>") && question.Answer.Contains("</script>"))
                    {
                        correct = true;
                        //Add SQL call to insert  for questions answered.
                        var checkIfExists = (from q in adminContext.AnsweredQuestions
                                               where q.excercise == question.Excercise && q.questionNumber == question.QuestionNumber && q.email == userEmail
                                                select q.email).FirstOrDefault();
                        if (checkIfExists == null)
                        {
                            var qAnswered = new AnsweredQuestion(question.Excercise, userEmail, question.QuestionNumber, question.ID);
                            adminContext.AnsweredQuestions.Add(qAnswered);
                            adminContext.SaveChanges();
                        }

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
                        var checkIfExists = (from q in adminContext.AnsweredQuestions
                                                where q.excercise == question.Excercise && q.questionNumber == question.QuestionNumber && q.email == userEmail
                                                select q.email).FirstOrDefault();
                        if (checkIfExists == null)
                        {
                            var qAnswered = new AnsweredQuestion(question.Excercise, userEmail, question.QuestionNumber, question.ID);
                            adminContext.AnsweredQuestions.Add(qAnswered);
                            adminContext.SaveChanges();
                        }
                    }
                    else
                    {
                        hint = (from q in adminContext.Questions
                                where q.Excercise == question.Excercise && q.QuestionNumber == question.QuestionNumber
                                select q.Hint).First();

                    }

                }

                if (correct)
                    return Json(new Value("success", question.QuestionNumber));
                else
                    return Json(new Value("failure", question.QuestionNumber, hint, question.Answer));

            }
            catch (Exception e)
            {
                return Json(new Value("error", question.QuestionNumber, "Unauthenticated",question.Answer));
            }

            
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