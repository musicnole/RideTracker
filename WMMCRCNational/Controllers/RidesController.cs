using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.SqlServer;
using System.Web.UI.HtmlControls;

namespace WMMCRCNational.Models
{
    public class RidesController : Controller
    {
        public int? memberSelected { get; set; }
        private WMMCRC db = new WMMCRC();

        // GET: Rides
        // [Authorize]
        
        public ActionResult Index(string searchString, string currentFilter, int? page, int? MemberId, string years, string button)
        {
            Rides_MembersModel viewModel = new Rides_MembersModel();
            
            FillDropDowns(null);
            //Code added for Paging
            //The first time the page is displayed, or if the user hasn't clicked a paging or sorting link, all the parameters will be null.  If a paging link is clicked, the page variable will contain the page number to display.
            //A ViewBag property provides the view with the current sort order, because this must be included in the paging links in order to keep the sort order the same while paging:

            ViewBag.memberId = MemberId;
            ViewBag.searchYear = years;

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
            List<Ride> ridesList = Helpers.Rides.GetLisMemberNames(db, years);

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
           // return View(viewModel);
        }
        
        // GET: Rides/Details/5
        //[Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);

            ride.MemberName = Helpers.Rides.GetMemberName(db, ride.MemberId);
            ride.RideToName = Helpers.Rides.GetChapterName(db, ride.RideTo);
            ride.RideFromName = Helpers.Rides.GetChapterName(db, ride.RideFrom);

            if (ride == null)
            {
                return HttpNotFound();
            }
            return View(ride);
        }

        // GET: Rides/Create
        [Authorize]
        public ActionResult Create(int? id)
        {

             memberSelected =id;
             FillDropDowns(memberSelected);

            return View();
        }

        private void FillDropDowns(int? memberSelected)
        {
            //Member dd

            //// OLD var memberdd = new SelectList(db.Members.ToList(), "MemberId", "FullName");

            var memberdd = (from a in db.Members
                            where a.Active == true
                            select a.FullName).FirstOrDefault();

            ViewData["MemberDD"] = memberdd;
            //using viewbag  
            ViewBag.memberdd = new SelectList(db.Members.ToList().Where(a => a.Active == true), "MemberId", "FullName");
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
        
        // Cannot put into helpers this is used to update miles text box
        public string UpdateMiles(int FromID, int ToID)
        {
            var miles = (from s in db.Miles
                         where s.FromId == FromID && s.ToId == ToID
                         select s.Miles).FirstOrDefault();

            return miles.ToString();
        }

        // POST: Rides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "RideId,MemberId,RideDate,RideFrom,RideTo,Miles,DateModified,DateCreated,Partial,VerifiableEvent,Cage,RideNotes")] Ride ride)
        {
            var memberId = Request.Form["MemberId"].ToString();

            //This is the article to fix the Jquery issue on loading the miles texbox
            //http://stackoverflow.com/questions/4193916/how-to-set-textbox-value-in-jquery

            //this is the url to fix the saving error message, needed to follow the following steps
            //            Right click on the edmx file, select Open with, XML editor
            //Locate the entity in the edmx:StorageModels element
            //Remove the DefiningQuery entirely
            //Rename the store:Schema="dbo" to Schema="dbo" (otherwise, the code will generate an error saying the name is invalid)
            //http://stackoverflow.com/questions/15322894/because-it-has-a-definingquery-and-no-insertfunction-element-exists-in-the-mo

            ride.DateCreated = System.DateTime.Now;
            ride.DateModified = System.DateTime.Now;

            var partialCheck = ride.Partial;
            if (!partialCheck)
            {
                var intMiles = Int32.Parse(ride.Miles);
                ride.Miles = (intMiles * 2).ToString();
            }
            var cage = ride.Cage;
            if (cage)
            {
                ride.Miles = "0";
            }

            FillDropDowns(null);

            if (ModelState.IsValid)
            {
                db.Rides.Add(ride);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ride);
        }

        // GET: Rides/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {

            FillDropDowns(null);

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

        // POST: Rides/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "RideId,MemberId,RideDate,RideFrom,RideTo,Miles,DateModified,DateCreated,Partial,VerifiableEvent,Cage,RideNotes")] Ride ride)
        {
            var memberId = Request.Form["MemberId"].ToString();

            ride.DateModified = System.DateTime.Now;
            //If In Edit mode do not multiply miles again on save
            //var partialCheck = ride.Partial;
            //if (!partialCheck)
            //{
            //    var intMiles = Int32.Parse(ride.Miles);
            //    ride.Miles = (intMiles * 2).ToString();
            //}
            var cage = ride.Cage;
            if (cage)
            {
                ride.Miles = "0";
            }

            FillDropDowns(null);
            if (ModelState.IsValid)
            {
                db.Entry(ride).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ride);
        }

        // GET: Rides/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ride ride = db.Rides.Find(id);
            ride.MemberName = Helpers.Rides.GetMemberName(db, ride.MemberId);
            ride.RideToName = Helpers.Rides.GetChapterName(db, ride.RideTo);
            ride.RideFromName = Helpers.Rides.GetChapterName(db, ride.RideFrom);
            if (ride == null)
            {
                return HttpNotFound();
            }
            return View(ride);
        }

        // POST: Rides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
