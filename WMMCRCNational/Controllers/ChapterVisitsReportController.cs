using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMMCRCNational.Models
{
    public class ChapterVisitsReportController : Controller
    {
        private WMMCRC db = new WMMCRC();

        // GET: ChapterVisitsReport
        //[Authorize]
        public ActionResult Index(string Years)
        {
            var years = Helpers.ChapterVisitReport.FillDropDowns();
            var rideYear = new SelectList(years);
            ViewData["YearDD"] = years;
            ViewBag.years = new SelectList(years);

            ViewData["YearsSelected"] = Years;

            List <ChapterVisitReport> rpt = new List<ChapterVisitReport>();
            
            DataTable dtReport = Helpers.ChapterVisitReport.CreateReport(db,Years);

            ViewData.Model = dtReport;
            
            ViewBag.Data = dtReport;

            return View();
        }

   

    }
}