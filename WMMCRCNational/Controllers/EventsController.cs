using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Models;

namespace WMMCRCNational.Controllers
{
    public class EventsController : Controller
    {
        private WMMCRC db = new WMMCRC();

        // GET: Events
        public ActionResult Index(string searchString, string years)
        {
            FillDropDowns();
            ViewBag.searchYear = years;

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
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
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
    }
}
