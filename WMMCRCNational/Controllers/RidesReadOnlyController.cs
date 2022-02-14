using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Models;
using PagedList;
using System.Data.Entity.SqlServer;
using System.Web.UI.HtmlControls;

namespace WMMCRCNational.Views
{
    public class RidesReadOnlyController : Controller
    {
        public int? memberSelected { get; set; }
        private WMMCRC db = new WMMCRC();
        private int chapterId { get; set; }

        // GET: RidesReadOnly
        public ActionResult Index(string searchString, string currentFilter, int? page, int? MemberId, string years, string button, string chapter)
        {
            Rides_MembersModel viewModel = new Rides_MembersModel();


            if (Session["ChapterId"] != null && !string.IsNullOrEmpty(Session["ChapterId"].ToString()))
            {
                chapterId = Convert.ToInt32(Session["ChapterId"]);
            }
            else if(!string.IsNullOrEmpty(chapter) && chapter != null){
                
                chapterId = Convert.ToInt32(chapter);
            }

            FillDropDowns(null,chapterId);
            //Code added for Paging
            //The first time the page is displayed, or if the user hasn't clicked a paging or sorting link, all the parameters will be null.  If a paging link is clicked, the page variable will contain the page number to display.
            //A ViewBag property provides the view with the current sort order, because this must be included in the paging links in order to keep the sort order the same while paging:

            ViewBag.memberId = MemberId;
            ViewBag.searchYear = years;
            ViewBag.chapter = chapterId;

            //If years is null default to current year
            if (string.IsNullOrEmpty(years))
            {
                years = System.DateTime.Now.Year.ToString();
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //End of first set of code for paging

            IOrderedEnumerable<Ride> returnRides;

            // This Returns all the rides and the Clubhouse Names
            List<Ride> ridesList = Helpers.Rides.GetLisMemberNames(db, years, chapterId);

            // If the year is not specified then want to default to this years rides
            if (string.IsNullOrWhiteSpace(years))
            {
                List<Ride> rideListCurrentYear = Helpers.Rides.GetThisYearRides(ridesList);
                //This returns the list of rindes for a specific Member
                returnRides = Helpers.Rides.GetSearchView(MemberId, rideListCurrentYear);
            }
            else
            {
                //This returns the list of rindes for a specific Member
                returnRides = Helpers.Rides.GetSearchView(MemberId, ridesList);
            }

            if (!string.IsNullOrWhiteSpace(years))
            {
                returnRides = Helpers.Rides.GetSearchDateView(returnRides, years);
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                returnRides = Helpers.Rides.GetSearchRideNotes(returnRides, searchString);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(returnRides.ToPagedList(pageNumber, pageSize));

        }


        private void FillDropDowns(int? memberSelected,int? chapter)
        {
           
            if (Session["ChapterId"] != null && !string.IsNullOrEmpty(Session["ChapterId"].ToString()))
            {
                chapterId = Convert.ToInt32(Session["ChapterId"]);
            }

            //Chapters
            var chapterSearch = new SelectList(db.Chapters.ToList(), "ChapterId", "ChapterName");
            ViewData["ChapterDD"] = chapterSearch;
            ViewBag.chapter = new SelectList(chapterSearch);

            //Member dd
            var memberdd = (from a in db.Members
                            where a.Active == true
                            && a.ChapterId == chapterId
                            select a.FullName).FirstOrDefault();

            ViewData["MemberDD"] = memberdd;
            //using viewbag  
            ViewBag.memberdd = new SelectList(db.Members.ToList().Where(a => a.Active == true && a.ChapterId == chapterId), "MemberId", "FullName");
            if (memberSelected != null)
            {
                ViewBag.MemberId = memberSelected;
            }

            //Ride From dd
            var rideFromdd = new SelectList(db.Chapters.ToList(), "ChapterId", "ChapterName");
            ViewData["RideFromDD"] = rideFromdd;
            //RideToDD
            var rideTodd = new SelectList(db.Chapters.ToList(), "ChapterId", "ChapterName");
            ViewData["RideToDD"] = rideTodd;

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


        // GET: RidesReadOnly/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);
            if (ride == null)
            {
                return HttpNotFound();
            }
            return View(ride);
        }

        // GET: RidesReadOnly/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RidesReadOnly/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RideId,MemberId,RideDate,RideFrom,RideTo,Miles,DateModified,DateCreated,Partial,VerifiableEvent,Cage,RideNotes")] Ride ride)
        {
            if (ModelState.IsValid)
            {
                db.Rides.Add(ride);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ride);
        }

        // GET: RidesReadOnly/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);
            if (ride == null)
            {
                return HttpNotFound();
            }
            return View(ride);
        }

        // POST: RidesReadOnly/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RideId,MemberId,RideDate,RideFrom,RideTo,Miles,DateModified,DateCreated,Partial,VerifiableEvent,Cage,RideNotes")] Ride ride)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ride).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ride);
        }

        // GET: RidesReadOnly/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);
            if (ride == null)
            {
                return HttpNotFound();
            }
            return View(ride);
        }

        // POST: RidesReadOnly/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ride ride = db.Rides.Find(id);
            db.Rides.Remove(ride);
            db.SaveChanges();
            return RedirectToAction("Index");
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
