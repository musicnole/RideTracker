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

namespace WMMCRCNational.Models
{
    public class MilesController : Controller
    {
        private WMMCRC db = new WMMCRC();

        // GET: Miles
       // [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page, string currentId,  int chapter = 0)
        {
            searchString = Helpers.Rides.GetChapterName(db, chapter);
            
            // Put in the current id and this line to make sure that the dd retains the value when paging
            if (!string.IsNullOrEmpty(currentId) && chapter == 0) chapter = Convert.ToInt32(currentId);
           
            FillDropDowns(chapter);
            //https://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "FromName" : "";

            ViewBag.ToSortParm = sortOrder == "ToName" ? "ToName_desc" : "ToName";

            ViewBag.MilesSortParm = sortOrder == "Miles" ? "Miles_desc" : "Miles";

            //Code added for Paging
            //The first time the page is displayed, or if the user hasn't clicked a paging or sorting link, all the parameters will be null.  If a paging link is clicked, the page variable will contain the page number to display.
            //A ViewBag property provides the view with the current sort order, because this must be included in the paging links in order to keep the sort order the same while paging:

            ViewBag.CurrentSort = sortOrder;
            
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.currentId = chapter;
            //End of first set of code for paging

            List<Mile> milesListOrig = Helpers.Miles.GetClubhouseName(searchString, db);

            IOrderedEnumerable<Mile> returnMiles;
            List<Mile> milesList = Helpers.Miles.GetSearchView(searchString, milesListOrig);


            returnMiles = Helpers.Miles.SortMilesVIew(sortOrder, milesList);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            //return View(db.Miles.ToList());
            //return View(milesList);
            return View(returnMiles.ToPagedList(pageNumber, pageSize));


        }

        // GET: Miles/Details/5
       // [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mile mile = db.Miles.Find(id);
            Helpers.Miles.GetChapterNames(mile, db);
            mile.FromChapterAddress = Helpers.Miles.GetChapterAddresses(mile,db).First();
            mile.ToChapterAddress = Helpers.Miles.GetChapterAddresses(mile, db).ElementAt(1);


            if (mile == null)
            {
                return HttpNotFound();
            }
            return View(mile);
        }

        public ActionResult OpenGoogleMap(string MapUri)
        {

            return Redirect(MapUri);
        }

        // GET: Miles/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Miles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "MilesId,FromId,ToId,Miles,Active,DateModified,DateCreated,GoogleUri")] Mile mile)
        {
            if (ModelState.IsValid)
            {
                db.Miles.Add(mile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mile);
        }

        // GET: Miles/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mile mile = db.Miles.Find(id);

            Helpers.Miles.GetChapterNames(mile,db);

            if (mile == null)
            {
                return HttpNotFound();
            }
            return View(mile);
        }

        // POST: Miles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "MilesId,FromId,ToId,Miles,Active,DateModified,DateCreated,GoogleUri")] Mile mile)
        {
            mile.DateModified = System.DateTime.Now;
            mile.DateCreated = mile.DateCreated;
            if (ModelState.IsValid)
            {
                db.Entry(mile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mile);
        }

        // GET: Miles/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mile mile = db.Miles.Find(id);
            if (mile == null)
            {
                return HttpNotFound();
            }
            return View(mile);
        }

        // POST: Miles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Mile mile = db.Miles.Find(id);
            db.Miles.Remove(mile);
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


        private void FillDropDowns(int chapter)
        {
            //Ride From dd
            //Putting this code in to select the correct Chapter when Paging, paging uses currentFilterb but need to get the filters ID
            var rideFromdd = new SelectList(Helpers.Chapters.GetChapters(db,"True"), "ChapterId", "ChapterName", selectedValue: chapter );

            ViewData["ChapterDD"] = rideFromdd;
        
        }




    }
}
