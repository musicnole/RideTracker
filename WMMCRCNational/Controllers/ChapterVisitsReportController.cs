using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Helpers;

namespace WMMCRCNational.Models
{
    public class ChapterVisitsReportController : Controller
    {
        private WMMCRC db = new WMMCRC();
        public int ChapterID { get; set; }

        // GET: ChapterVisitsReport
        //[Authorize]
        public ActionResult Index(string Years)
        {
            CheckSecurity();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("AccessError", "Account");
            }

            var years = Helpers.ChapterVisitReport.FillDropDowns();
            var rideYear = new SelectList(years);
            ViewData["YearDD"] = years;
            ViewBag.years = new SelectList(years);

            ViewData["YearsSelected"] = Years;

            List <ChapterVisitReport> rpt = new List<ChapterVisitReport>();
                        
            DataTable dtReport = Helpers.ChapterVisitReport.CreateReport(db,Years,ChapterID);

            ViewData.Model = dtReport;
            
            ViewBag.Data = dtReport;

            return View();
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