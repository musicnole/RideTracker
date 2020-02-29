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
    public class ASPNetUserRolesController : Controller
    {
        private WMMCRC db = new WMMCRC();

        // GET: ASPNetUserRoles
        public ActionResult Index()
        {
            List<ASPNetUserRoles> aSPNetUserRoles = db.ASPNetUserRoles.ToList();

            foreach (ASPNetUserRoles userRole in aSPNetUserRoles)
            {
                userRole.MemberName = Helpers.ASPNetUserRoles.GetUserName(db, userRole.UserId);
                userRole.RoleName = Helpers.ASPNetUserRoles.GetRole(db, userRole.RoleId);
            }
            
            return View(aSPNetUserRoles);
        }

        // GET: ASPNetUserRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASPNetUserRoles aSPNetUserRoles = db.ASPNetUserRoles.Find(id);

            aSPNetUserRoles.MemberName = Helpers.ASPNetUserRoles.GetUserName(db, aSPNetUserRoles.UserId);
            aSPNetUserRoles.RoleName = Helpers.ASPNetUserRoles.GetRole(db, aSPNetUserRoles.RoleId);

            if (aSPNetUserRoles == null)
            {
                return HttpNotFound();
            }
            return View(aSPNetUserRoles);
        }

        // GET: ASPNetUserRoles/Create
        public ActionResult Create()
        {
            FillDropDowns();
            return View();
        }

        private void FillDropDowns()
        {
           
            //Ride From dd
            var userdd = new SelectList(db.AspNetUsers.ToList(), "Id", "UserName");
            ViewData["UserDD"] = userdd;
            //using viewbag  
            ViewBag.rideFromdd = new SelectList(db.AspNetUsers.ToList(), "Id", "UserName");

            //RideToDD
            var roleDD = new SelectList(db.AspNetRoles.ToList(), "Id", "Name");
            ViewData["RoleDD"] = roleDD;
            //using viewbag  
            ViewBag.rideTodd = new SelectList(db.AspNetRoles.ToList(), "Id", "name");

        }

        // POST: ASPNetUserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,RoleId,MemberName,RoleName")] ASPNetUserRoles aSPNetUserRoles)
        {
            if (ModelState.IsValid)
            {
                db.ASPNetUserRoles.Add(aSPNetUserRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aSPNetUserRoles);
        }

        // GET: ASPNetUserRoles/Edit/5
        public ActionResult Edit(string id, string role)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASPNetUserRoles aSPNetUserRoles = db.ASPNetUserRoles.Find(id,role);

            aSPNetUserRoles.RoleName = Helpers.ASPNetUserRoles.GetRole(db, aSPNetUserRoles.RoleId);
            aSPNetUserRoles.UserId = Helpers.ASPNetUserRoles.GetUserName(db, aSPNetUserRoles.UserId);

            if (aSPNetUserRoles == null)
            {
                return HttpNotFound();
            }
            return View(aSPNetUserRoles);
        }

        // POST: ASPNetUserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,RoleId,MemberName,RoleName")] ASPNetUserRoles aSPNetUserRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aSPNetUserRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aSPNetUserRoles);
        }

        // GET: ASPNetUserRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASPNetUserRoles aSPNetUserRoles = db.ASPNetUserRoles.Find(id);
            if (aSPNetUserRoles == null)
            {
                return HttpNotFound();
            }
            return View(aSPNetUserRoles);
        }

        // POST: ASPNetUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ASPNetUserRoles aSPNetUserRoles = db.ASPNetUserRoles.Find(id);
            db.ASPNetUserRoles.Remove(aSPNetUserRoles);
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
