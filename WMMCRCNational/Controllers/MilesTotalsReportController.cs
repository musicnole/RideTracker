using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Models;

namespace WMMCRCNational.Controllers
{
    public class MilesTotalsReportController : Controller
    {
        private WMMCRC db = new WMMCRC();
        // GET: MilesTotalsReport
      // [Authorize]
        public ActionResult Index(string YearSelected)
        {

            var report = from a in db.Members
                         where a.Active == true
                         select new { a.MemberId, a.FullName };

            List<MilesTotalsReport> rptList = new List<MilesTotalsReport>();

            foreach (var item in report)
            {
                rptList.Add(new MilesTotalsReport()
                {
                    MemberId = item.MemberId,
                    FullName = item.FullName,
                    Miles = Helpers.MilesTotalsReport.GetMilesTotal(item.MemberId, YearSelected, db)
                });
            }

            var rptReturn = rptList.OrderByDescending(c => c.Miles);

            return View(rptReturn.AsEnumerable());
        }

       
    }
}