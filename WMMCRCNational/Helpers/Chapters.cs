using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMMCRCNational.Models;

namespace WMMCRCNational.Helpers
{
    public class Chapters
    {
        public static List<Chapter> GetChapters(WMMCRC db, string active)
        {
            List<Chapter> chapterList = new List<Chapter>();

            switch (active)
            {
                case "True":
                    chapterList = (from a in db.Chapters
                                  where a.Active == true
                                  select a).ToList();
                    break;
                case "False":
                    chapterList = (from a in db.Chapters
                                   where a.Active == false
                                  select a).ToList();
                    break;
                default:
                    chapterList = db.Chapters.ToList();
                    break;
            }

            return chapterList;

        }
    }
}