using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Helpers;
using WMMCRCNational.Models;

namespace WMMCRCNational.Controllers
{
    public class EventsController : Controller
    {
        private WMMCRC db = new WMMCRC();

        // GET: Events
        public ActionResult Index(string searchString, string years, string chapter)
        {
            CheckSecurity();
            FillDropDowns();
            ViewBag.searchYear = years;
            ViewBag.searchChapter = chapter;

            IOrderedEnumerable<Event> returnEvents;

            if (!string.IsNullOrWhiteSpace(years))
            {
                returnEvents = Helpers.Events.GetSearchDateView(db, years);
            }
            else
            {
                returnEvents = Helpers.Events.LookupChapterNames(db);
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                returnEvents = Helpers.Events.GetSearchEventName(returnEvents, searchString);
            }

            if (!string.IsNullOrWhiteSpace(chapter))
            {
                returnEvents = Helpers.Events.GetSearchChapter(returnEvents, chapter);
            }

            return View(returnEvents.OrderBy(t=> t.EventDate));
        }



        private void FillDropDowns()
        {
            //Years
            List<string> years = new List<string>();
            years.Add("2017");
            years.Add("2018");
            years.Add("2019");
            years.Add("2020");
            years.Add("2021");
            years.Add("2022");
            years.Add("2023");
            years.Add("2024");
            years.Add("2025");
            var rideYear = new SelectList(years);
            ViewData["YearDD"] = years;
            ViewBag.years = new SelectList(years);

            //Chapters
            var chapterSearch = new SelectList(db.Chapters.ToList(), "ChapterId", "ChapterName");
            ViewData["ChapterDD"] = chapterSearch;
            ViewBag.chapter = new SelectList(chapterSearch);

        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            CheckSecurity();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);

            @event.ChapterName = Helpers.Events.GetChapterName(id, db);

            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }
            CreateChapterDD();
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,ChapterId,EventDate,EventTitle,DateModified,DateCreated")] Event @event)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }

            @event.DateModified = System.DateTime.Now;
            @event.DateCreated = System.DateTime.Now;
            CreateChapterDD();
           
            var chapterId = Request.Form["ChapterId"].ToString();
            @event.ChapterId = int.Parse(chapterId);

            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }
            CreateChapterDD();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,ChapterId,EventDate,EventTitle,DateModified,DateCreated")] Event @event)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }
            CreateChapterDD();
            @event.DateModified = System.DateTime.Now;
            @event.DateCreated = @event.DateCreated;
            
            var chapterId = Request.Form["ChapterId"].ToString();
            @event.ChapterId = Convert.ToInt32(chapterId);

            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            @event.ChapterName = Helpers.Events.GetChapterName(id, db);

            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }

            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void CreateChapterDD()
        {
            var chapterdd = new SelectList(db.Chapters.ToList(), "ChapterId", "ChapterName");
            ViewData["ChapterDD"] = chapterdd;
            //using viewbag  
            ViewBag.chapterdd = new SelectList(db.Chapters.ToList(), "ChapterId", "ChapterName");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private void CheckSecurity()
        {
            SecurityObjectWM secObj = GlobalHelper.SetSecurity(db);
            ViewBag.admin = secObj.adminRole;
            ViewBag.roadCaptain = secObj.roadCaptainRole;
            ViewBag.member = secObj.memberRole;

        }
    }
}
