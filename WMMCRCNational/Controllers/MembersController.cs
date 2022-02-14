using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WMMCRCNational.Helpers;

namespace WMMCRCNational.Models
{
    public class MembersController : Controller
    {
        private WMMCRC db = new WMMCRC();
        private bool admin { get; set; }
        private bool roadCaptain { get; set; }
        private bool member { get; set; }
        private int chapterId { get; set; }

        // GET: Members
        //[Authorize]
        public ActionResult Index(string active)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin && !ViewBag.roadCaptain))
            {
                return RedirectToAction("AccessError", "Account");
            }

            FillDropDowns();

            if (active == null) active = "True";

            List<Member> membersList = new List<Member>();

            return View(membersList = Helpers.Members.GetMembers(db, active,chapterId));
        }

       


        // GET: Members/Details/5
        //[Authorize]
        public ActionResult Details(int? id)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin && !ViewBag.roadCaptain))
            {
                return RedirectToAction("AccessError", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        [Authorize]
        public ActionResult Create()
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin && !ViewBag.roadCaptain))
            {
                return RedirectToAction("AccessError", "Account");
            }
            
            FillDropDowns();
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "MemberId,FirstName,LastName,RoadName,StandupDate,PatchDate,Active,DateModified,DateCreated,ChapterId,Email")] Member member)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin && !ViewBag.roadCaptain))
            {
                return RedirectToAction("AccessError", "Account");
            }

            member.DateModified = System.DateTime.Now;
            member.DateCreated = System.DateTime.Now;
            member.ChapterId = chapterId;
            member.FullName = string.Concat(member.FirstName, " ", member.LastName);
           
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin && !ViewBag.roadCaptain))
            {
                return RedirectToAction("AccessError", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "MemberId,FirstName,LastName,RoadName,StandupDate,PatchDate,Active,DateModified,DateCreated,FullName,Email")] Member member)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin && !ViewBag.roadCaptain))
            {
                return RedirectToAction("AccessError", "Account");
            }

            member.DateModified = System.DateTime.Now;
            member.DateCreated = member.DateCreated;
            member.ChapterId = chapterId;
            member.FullName = string.Concat(member.FirstName, " ", member.LastName);

            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            CheckSecurity();
            //Security if not logged in then redirect
            if ((System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false) || (!ViewBag.admin && !ViewBag.roadCaptain))
            {
                return RedirectToAction("AccessError", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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

            //Chapters
            var chapterSearch = new SelectList(db.Chapters.ToList(), "ChapterId", "ChapterName");
            ViewData["ChapterDD"] = chapterSearch;
            ViewBag.chapter = new SelectList(chapterSearch);
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
