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
    public class ChapterAddressesController : Controller
    {
        private WMMCRC db = new WMMCRC();

        // GET: ChapterAddresses
        //[Authorize]
        public ActionResult Index()
        {
            return View(db.ChapterAddresses.ToList());
        }

        // GET: ChapterAddresses/Details/5
        //[Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChapterAddress chapterAddress = db.ChapterAddresses.Find(id);
            if (chapterAddress == null)
            {
                return HttpNotFound();
            }
            return View(chapterAddress);
        }

        // GET: ChapterAddresses/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            ViewData["ChapterId"] = id;
            //use the id to get the name
            ViewData["ChapterName"] = (from s in db.Chapters
                                       where s.ChapterId == id
                                       select s.ChapterName).FirstOrDefault();
            ViewBag.statedd = new SelectList(db.States.ToList(), "Id", "Name");

            return View();
        }

        // POST: ChapterAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ChapterAddressId,ChapterId,StreetAddress1,StreetAddress2,City,StateId,Zip,Active,DateModified,DateCreated")] ChapterAddress chapterAddress)
        {
            chapterAddress.DateModified = System.DateTime.Now;
            chapterAddress.DateCreated = System.DateTime.Now;

            var chapterId = Request.Form["ChapterId"].ToString();
            chapterAddress.ChapterId = int.Parse(chapterId);

            if (ModelState.IsValid)
            {
                db.ChapterAddresses.Add(chapterAddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chapterAddress);
        }

        // GET: ChapterAddresses/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            ChapterAddress chapterAddress = db.ChapterAddresses.Find(id);

            var statedd = new SelectList(db.States.ToList(), "Id", "Name");
            ViewData["StateDD"] = statedd;
            //using viewbag  
            ViewBag.statedd = new SelectList(db.States.ToList(), "Id", "Name");

            if (id != null && chapterAddress == null)
            {
                return RedirectToAction("Create", new { id });
            }

            if (chapterAddress == null)
            {
                return HttpNotFound();
            }
            return View(chapterAddress);
        }

        // POST: ChapterAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ChapterAddressId,ChapterId,StreetAddress1,StreetAddress2,City,StateId,Zip,Active,DateModified,DateCreated")] ChapterAddress chapterAddress)
        {
            chapterAddress.DateModified = System.DateTime.Now;
            chapterAddress.DateCreated = chapterAddress.DateCreated;

            if (ModelState.IsValid)
            {
                db.Entry(chapterAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Chapters");
            }
            return View(chapterAddress);
        }

        // GET: ChapterAddresses/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChapterAddress chapterAddress = db.ChapterAddresses.Find(id);
            if (chapterAddress == null)
            {
                return HttpNotFound();
            }
            return View(chapterAddress);
        }

        // POST: ChapterAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            ChapterAddress chapterAddress = db.ChapterAddresses.Find(id);
            db.ChapterAddresses.Remove(chapterAddress);
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
