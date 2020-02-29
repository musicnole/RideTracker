using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.SqlServer;

namespace WMMCRCNational.Models
{
    public class Rides_MembersModel
    {
        public IEnumerable<WMMCRCNational.Models.Ride> Rides { get; set; }
        public IEnumerable<WMMCRCNational.Models.Member> Members { get; set; }

        private WMMCRC db = new WMMCRC();

        public string hidmemid { get; set; }

        public Rides_MembersModel()
        {
            Members = (from m in db.Members
                         where m.Active == true
                         select m).ToList().OrderByDescending(s => s.DateCreated);
           // Rides = Helpers.Rides.GetLisMemberNames(db,string.Empty);
            
        }
    }
}