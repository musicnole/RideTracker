using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMMCRCNational.Models
{
    public class ChapterDetails
    {

        public IList<Chapter> Chapters { get; set; }
        public IList<ChapterAddress> ChapterAddresses { get; set; }
        public string State { get; set; }

        public string GoogleLink { get; set; }
    }
}