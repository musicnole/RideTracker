using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMMCRCNational.Models;

namespace WMMCRCNational.Helpers
{
    public class Rides
    {
        public static List<Ride> GetLisMemberNames(WMMCRC db, string yearSearchString)
        {
            List<Ride> allRidesList = new List<Ride>();
            List<Ride> returnRideList = new List<Ride>();

            allRidesList = db.Rides.ToList();

            int searchYear = 0;

            if (string.IsNullOrWhiteSpace(yearSearchString))
            {

                returnRideList = GetThisYearRides(allRidesList);
            }
            else
            {
                searchYear = Convert.ToInt32(yearSearchString);
                returnRideList = GetThisYearRides(allRidesList, searchYear);
            }

            Dictionary<int, string> dictMemberName = new Dictionary<int, string>();
            dictMemberName = db.Members.ToDictionary(x => x.MemberId, x => x.FullName);

            Dictionary<int, string> dictChapterName = new Dictionary<int, string>();
            dictChapterName = db.Chapters.ToDictionary(y => y.ChapterId, y => y.ChapterName);
             

            foreach (Ride ride in returnRideList)
            {
                ride.MemberName = dictMemberName[ride.MemberId];
                ride.RideFromName = dictChapterName[ride.RideFrom];
                ride.RideToName = dictChapterName[ride.RideTo];

            }
            return returnRideList;
        }

        public static IOrderedEnumerable<Ride> GetSearchView(int? MemberId, List<Ride> ridesListOrig)
        {

            IOrderedEnumerable<Ride> ridesList;

            if (MemberId != null && MemberId!=0)
            {
                ridesList = (from ch in ridesListOrig
                             //where ch.MemberName.ToLower().Contains(searchString.ToLower())
                             where ch.MemberId == MemberId 
                             select ch).ToList().OrderByDescending(s => s.DateCreated);
            }
            else
            {
                ridesList = ridesListOrig.OrderByDescending(s => s.DateCreated);
            }
                       
           
            return ridesList;
        }

        public static IOrderedEnumerable<Ride> GetSearchDateView(IOrderedEnumerable<Ride> ridesListOrig, string year) 
        {
            IOrderedEnumerable<Ride> ridesList;
            
            IEnumerable<Ride> ridesListobj  =new List<Ride>();
            //need to get only the years ride from 12/1 of previous year to 11/30 of same year
            string fromYear = Convert.ToString(Convert.ToInt32(year) - 1);
            DateTime fromDate = Convert.ToDateTime(string.Concat("11-30-", (fromYear)));
            DateTime toDate = Convert.ToDateTime(string.Concat("11-30-", (year)));

            //ridesListobj = ridesListOrig.Where(s => s.RideDate.ToShortDateString().Contains(year));

            ridesListobj = ridesListOrig.Where(s => s.RideDate >fromDate && s.RideDate < toDate );

            ridesList = ridesListobj.OrderByDescending(s => s.DateCreated);

            return ridesList;
        }


        public static IOrderedEnumerable<Ride> GetSearchRideNotes(IOrderedEnumerable<Ride> ridesListOrig, string notes)
        {
            IOrderedEnumerable<Ride> ridesList;

            if (!string.IsNullOrWhiteSpace(notes))
            {
                ridesList = (from ch in ridesListOrig
                             //where ch.MemberName.ToLower().Contains(searchString.ToLower())
                             where  ch.RideNotes != null
                             && ch.RideNotes.ToString().ToLower().Contains(notes.ToLower())
                             select ch).ToList().OrderByDescending(s => s.DateCreated);
            }
            else
            {
                ridesList = ridesListOrig.OrderByDescending(s => s.RideDate);
            }
            

            return ridesList;
        }
      
        public static string GetMemberName(WMMCRC db, int memberId)
        {
            string memberName = string.Empty;

            memberName = (from a in db.Members
                          where a.MemberId == memberId
                          select a.FullName).FirstOrDefault();

            return memberName;
        }
        
        public static string GetChapterName(WMMCRC db, int chapterId)
        {
            string chapterName = string.Empty;

            chapterName = (from a in db.Chapters
                          where a.ChapterId == chapterId
                          select a.ChapterName).FirstOrDefault();

            return chapterName;
        }

        public static List<Ride> GetThisYearRides(List<Ride> allRides)
        {
            int thisYear = DateTime.Now.Year;
            int thisMonth = DateTime.Now.Month;
            DateTime fromDate;
            DateTime toDate;
            
            if(thisMonth == 12)
            {
                fromDate = Convert.ToDateTime(string.Concat("11-30-", thisYear.ToString()));
                toDate = Convert.ToDateTime(string.Concat("12-01-", (thisYear +1).ToString()));
            }
            else
            {
                fromDate = Convert.ToDateTime(string.Concat("11-30-", (thisYear - 1).ToString()));
                toDate = Convert.ToDateTime(string.Concat("12-01-", thisYear.ToString()));
            }

            List<Ride> ridesListCurrent = new List<Ride>();
            ridesListCurrent = allRides.Where(x => x.DateCreated > fromDate && x.DateCreated < toDate).ToList();

            return ridesListCurrent;
        }
        public static List<Ride> GetThisYearRides(List<Ride> allRides, int specifiedYear)
        {
            
            DateTime fromDate = Convert.ToDateTime(string.Concat("11-30-", (specifiedYear - 1).ToString()));
            DateTime toDate = Convert.ToDateTime(string.Concat("12-01-", specifiedYear.ToString()));
            List<Ride> ridesListCurrent = new List<Ride>();
            ridesListCurrent = allRides.Where(x => x.DateCreated > fromDate && x.DateCreated < toDate).ToList();

            return ridesListCurrent;
        }
    }
}