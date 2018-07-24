using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hackagram.Controllers
{
    [RequireHttps]
    public class HackagramController : Controller
    {
        // GET: Hackagram
        public ActionResult Index()
        {
            return View();
        }
        
        // GET: Hackagram
        public ActionResult Login()
        {


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
    }
}
