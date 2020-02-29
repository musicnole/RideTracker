using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Models;

namespace WMMCRCNational.Controllers
{
    public class HomeController : Controller
    {
        private WMMCRC db = new WMMCRC();
       // [Authorize]
        public ActionResult Index()
        {
           // List<Event> eventsList = Helpers.Events.LookupChapterNames(db);
            
            var eventListObj = (from a in db.Events
                                where a.EventDate >= System.DateTime.Now
                                select new { a.EventId, a.ChapterId, a.EventTitle, a.EventDate, a.DateModified, a.DateCreated });

            List<Event> eventsList = new List<Event>();
           
            foreach (var item in eventListObj)
            {
                var chapterName = (from a in db.Chapters
                                   where a.ChapterId == item.ChapterId
                                   select a.ChapterName).FirstOrDefault();

                eventsList.Add(new Event { EventId = item.EventId, ChapterName= chapterName, ChapterId = item.ChapterId, EventTitle = item.EventTitle, EventDate = item.EventDate, DateModified = item.DateModified, DateCreated = item.DateCreated });
            }

           return View(eventsList.OrderBy(t=>t.EventDate));
        }
        //[Authorize]Welcome To The Ride Tracker Application. An application for motorcycle enthusiasts to keep track of the rides they have been on.
        public ActionResult About()
        {
            ViewBag.Message = "Welcome To The Ride Tracker Application. An application for motorcycle enthusiasts to keep track of the rides they have been on.";

            return View();
        }
       // [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = " Joe D ";

            return View();
        }
    }
}