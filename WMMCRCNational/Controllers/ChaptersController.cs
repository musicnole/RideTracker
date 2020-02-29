using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WMMCRCNational.Models
{
    public class ChaptersController : Controller
    {
        private WMMCRC db = new WMMCRC();

        // GET: Chapters
//        [Authorize]
        public ActionResult Index()
        {
            List<Chapter> chapters = new List<Chapter>();

            chapters = (from ch in db.Chapters
                        //where ch.Active == true
                        select ch).ToList();
            foreach (Chapter ch in chapters)
            {
                if (ch.Active)
                { ch.activeValue = "Yes"; }
                else
                { ch.activeValue = "No"; }

            }
            return View(chapters);
        }

        // GET: Chapters/Details/5
  //      [Authorize]
        public ActionResult Details(int? id)
        {
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

            //var tuple = new Tuple<Chapter, ChapterAddress>(chapter,chapterAddress);
            //return View(chapter);
            //return View(tuple);
            return View(cd);
        }

        // GET: Chapters/Create
        [Authorize]
        public ActionResult Create()
        {
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
        public ActionResult Edit([Bind(Include = "ChapterId,ChapterName,ChapterNickName,ClubHouseAddressID,DateModified,DateCreated,Active")] Chapter chapter)
        {
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
            Chapter chapter = db.Chapters.Find(id);

            DeleteChapterAddress(id);

            db.Chapters.Remove(chapter);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        private void DeleteChapterAddress(int chapterId)
        {
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
        
    }
}
