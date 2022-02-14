using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Helpers;

namespace WMMCRCNational.Models
{
    public class ChaptersController : Controller
    {
        private WMMCRC db = new WMMCRC();
        private bool admin { get; set; }
        private bool roadCaptain { get; set; }
        private bool member { get; set; }
        private int chapterId { get; set; }

        // GET: Chapters
        //        [Authorize]
        public ActionResult Index(string active)
        {
            CheckSecurity();

            FillDropDowns();
            List<Chapter> chapters = new List<Chapter>();
            if (active == null) active = "True";
            chapters = Helpers.Chapters.GetChapters(db, active);

            foreach (Chapter ch in chapters)
            {
                if (ch.Active)
                { ch.activeValue = "True"; }
                else
                { ch.activeValue = "False"; }

            }
            return View(chapters);
        }

       
        // GET: Chapters/Details/5
        //      [Authorize]
        public ActionResult Details(int? id)
        {
            CheckSecurity();
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ChapterDetails cd = new ChapterDetails();

            cd.Chapters = (from ch in db.Chapters
                           where ch.ChapterId == id
                           select ch).ToList();

            cd.ChapterAddresses = (from a in db.ChapterAddresses
                                   where a.ChapterId == id
                                   select a).ToList();

            var stateid = (from s in cd.ChapterAddresses
                           select s.StateId.Value).FirstOrDefault();

            var stateName = from a in db.States
                            where a.Id == stateid
                            select a.Name;

            cd.State = stateName.FirstOrDefault();

            if (cd == null)
            {
                return HttpNotFound();
            }
            return View(cd);
        }

        // GET: Chapters/Create
        [Authorize]
        public ActionResult Create()
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }
            return View();
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ChapterId,ChapterName,ChapterNickName,ClubHouseAddressID,DateModified,DateCreated,Active")] Chapter chapter)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }

            chapter.DateModified = System.DateTime.Now;
            chapter.DateCreated = System.DateTime.Now;

            if (ModelState.IsValid)
            {

                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chapter);
        }

        // GET: Chapters/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
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
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ChapterId,ChapterName,ChapterNickName,ClubHouseAddressID,DateModified,DateCreated,Active,GoogleLink")] Chapter chapter)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }

            chapter.DateModified = System.DateTime.Now;
            chapter.DateCreated = chapter.DateCreated;

            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        [Authorize]
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
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin))
            {
                return RedirectToAction("AccessError", "Account");
            }

            Chapter chapter = db.Chapters.Find(id);

            DeleteChapterAddress(id);

            db.Chapters.Remove(chapter);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        private void DeleteChapterAddress(int chapterId)
        {
            CheckSecurity();
            var chapterAddressID = (from s in db.ChapterAddresses
                                    where s.ChapterId == chapterId
                                    select s.ChapterAddressId).FirstOrDefault();

            ChapterAddress chapterAddress = db.ChapterAddresses.Find(chapterAddressID);
            db.ChapterAddresses.Remove(chapterAddress);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult OpenGoogleMap(string MapUri)
        {

            return Redirect(MapUri);
        }

        private void FillDropDowns()
        {
            //Active dd

            //This is a great way to select the distinct Values to populate the DD
            //var activedd = new SelectList(db.Members.ToList(), "Active", "Active");
            //SelectList distinctActive = new SelectList(activedd.GroupBy( o => o.Value).Select(v => v.FirstOrDefault().Value.ToString()));

            List<string> activeList = new List<string>();
            activeList.Add("True");
            activeList.Add("False");
            activeList.Add("ALL");
            var rideYear = new SelectList(activeList);
            ViewData["ActiveDD"] = activeList;
            ViewBag.active = new SelectList(activeList);

        }
        private void CheckSecurity()
        {
            SecurityObjectWM secObj = GlobalHelper.SetSecurity(db);
            ViewBag.admin = secObj.adminRole;
            ViewBag.roadCaptain = secObj.roadCaptainRole;
            ViewBag.member = secObj.memberRole;
            chapterId = secObj.chapterId;

        }
    }
}
