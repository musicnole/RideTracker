using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Helpers;
using WMMCRCNational.Models;

namespace WMMCRCNational.Controllers
{
    public class MilesTotalsReportController : Controller
    {
        private WMMCRC db = new WMMCRC();
        public int ChapterID { get; set; }
        // GET: MilesTotalsReport
        // [Authorize]
        public ActionResult Index(string YearSelected)
        {
            
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("AccessError", "Account");
            }
            CheckSecurity();


            var report = from a in db.Members
                         where a.Active == true
                         && a.ChapterId == ChapterID
                         select new { a.MemberId, a.FullName };

            List<MilesTotalsReport> rptList = new List<MilesTotalsReport>();

            foreach (var item in report)
            {
                rptList.Add(new MilesTotalsReport()
                {
                    MemberId = item.MemberId,
                    FullName = item.FullName,
                    Miles = Helpers.MilesTotalsRpt.GetMilesTotal(item.MemberId, YearSelected, db)
                });
            }

            var rptReturn = rptList.OrderByDescending(c => c.Miles);

            return View(rptReturn.AsEnumerable());
        }
        private void CheckSecurity()
        {
            SecurityObjectWM secObj = GlobalHelper.SetSecurity(db);
            ViewBag.admin = secObj.adminRole;
            ViewBag.roadCaptain = secObj.roadCaptainRole;
            ViewBag.member = secObj.memberRole;
            ChapterID = secObj.chapterId;

        }

    }
}