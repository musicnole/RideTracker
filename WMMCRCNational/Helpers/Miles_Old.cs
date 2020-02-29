using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMMCRCCheco.Models;
using PagedList;
using System;
using System.Collections.Generic;

namespace WMMCRCCheco.Helpers
{
    public static class Miles
    {

        public static List<Mile> GetClubhouseName(string searchString, WMMCRC database)
        {
            // List<Mile> milesList = new List<Mile>();
            int toId = 0;
            int fromId = 0;

            //Taking too long to load all so start with just checo when page loads
            if (string.IsNullOrWhiteSpace(searchString))
            {
                //Checo Id
                fromId = 1;
            }
            else
            {
                fromId = GetClubhouseIdFromName(searchString, database);
            }
            
            //milesList = database.Miles.ToList();
            var milesListObj = (from a in database.Miles
                                    where a.FromId == fromId
                                // select new Mile (a.MilesId,a.FromId,a.ToId,a.Miles,a.Active,a.DateModified,a.DateCreated)).ToList();
                                select new { a.MilesId, a.FromId, a.ToId, a.Miles, a.Active, a.DateModified, a.DateCreated }).ToList();
            
            List<Mile> milesList = new List<Mile>();
            foreach(var item in milesListObj)
            {
                milesList.Add(new Mile (item.MilesId, item.FromId, item.ToId, item.Miles, item.Active, item.DateModified, item.DateCreated));
            }


            foreach (var mile in milesList)
            {
                toId = mile.ToId;
                fromId = mile.FromId;
                GetChapterNames(mile,database);
            }
            return milesList;
        }

        private static int GetClubhouseIdFromName(string searchString,WMMCRC database)
        {
            var chapterId = 0;
            if (searchString.Length == 1)
            {
                chapterId = (from a in database.Chapters
                             where a.ChapterName.Substring(0,1) == searchString
                             select a.ChapterId).FirstOrDefault();
            }
            else
            {
                chapterId = (from a in database.Chapters
                             where a.ChapterName.Contains(searchString)
                             select a.ChapterId).FirstOrDefault();

            }

            return chapterId;
        }

        public static void GetChapterNames(Mile mile, WMMCRC database)
        {
            mile.ToName = (from s in database.Chapters
                           where s.ChapterId == mile.ToId
                           select s.ChapterName).FirstOrDefault();

            mile.FromName = (from f in database.Chapters
                             where f.ChapterId == mile.FromId
                             select f.ChapterName).FirstOrDefault();
        }

       public static List<string> GetChapterAddresses(Mile mile, WMMCRC db)
       {
           List<string> fromToAddressList = new List<string>();

           var FromChapterAddress = (from a in db.ChapterAddresses
                                      where a.ChapterId == mile.FromId
                                      select string.Concat(a.StreetAddress1, " ", a.StreetAddress2, " ", a.City, " ",
                                      (from b in db.States
                                       where b.Id == a.StateId
                                       select b.Name).FirstOrDefault()," ",a.Zip
                                      )).FirstOrDefault();
           fromToAddressList.Add(FromChapterAddress);

           var ToChapterAddress = (from c in db.ChapterAddresses
                                     where c.ChapterId == mile.ToId
                                   select string.Concat(c.StreetAddress1, " ", c.StreetAddress2, " ", c.City, " ",
                                     (from d in db.States
                                      where d.Id == c.StateId
                                      select d.Name).FirstOrDefault(), " ", c.Zip
                                     )).FirstOrDefault();
           
           fromToAddressList.Add(ToChapterAddress);

           return fromToAddressList;
       }

       public static IOrderedEnumerable<Mile> SortMilesVIew(string sortOrder, List<Mile> milesList)
       {
           IOrderedEnumerable<Mile> returnMiles;

           switch (sortOrder)
           {
               case "FromName":
                   returnMiles = milesList.OrderByDescending(s => s.FromName);
                   break;

               case "ToName":
                   returnMiles = milesList.OrderBy(s => s.ToName);
                   break;
               case "ToName_desc":
                   returnMiles = milesList.OrderByDescending(s => s.ToName);
                   break;

               case "Miles":
                   returnMiles = milesList.OrderBy(s => s.Miles);
                   break;

               case "Miles_desc":
                   returnMiles = milesList.OrderByDescending(s => s.Miles);
                   break;

               default:
                   returnMiles = milesList.OrderBy(s => s.FromName);
                   break;
           }
           return returnMiles;
       }

       public static List<Mile> GetSearchView(string searchString, List<Mile> milesListOrig)
       {


           List<Mile> milesList = new List<Mile>();
           if (!string.IsNullOrEmpty(searchString))
           {
               milesList = (from ch in milesListOrig
                            where ch.FromName.ToLower().Contains(searchString.ToLower())
                            select ch).ToList();
           }
           else
           {
               milesList = milesListOrig;
           }
           return milesList;
       }

    }

}