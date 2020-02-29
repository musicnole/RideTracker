using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WMMCRCNational.Models
{
    public class ChapterVisitReport
    {
        [Key]
        public int ReportId { get; set; }

        public int MemberId { get; set; }

        [StringLength(150)]
        [Display(Name = "Member")]
        public string FullName { get; set; }

        public int ChapterId { get; set; }

        [StringLength(250)]
        [Display(Name = "Chapter")]
        public string ChapterName { get; set; }

        [Display(Name = "Number of Visits")]
        public int ClubhouseVisit { get; set; }

    }
}