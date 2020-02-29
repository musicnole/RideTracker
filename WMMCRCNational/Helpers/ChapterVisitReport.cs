using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMMCRCNational.Models;

namespace WMMCRCNational.Helpers
{
    public class ChapterVisitReport
    {
        public static DataTable CreateReport(WMMCRC db, string year)
        {
            Dictionary<int, string> memberList = GetMembersList(db);

            Dictionary<int, string> chapterList = GetChaptersList(db);

            DataTable reportDt  = CreateReportDt(memberList, chapterList);

            PopulateReportTable(db, memberList, chapterList, reportDt, year);

            return reportDt;
        }

        private static void PopulateReportTable(WMMCRC db, Dictionary<int, string> memberList, Dictionary<int, string> chapterList, DataTable reportDt, string year)
        {

            int memberId = 0;
            int chapterId = 0;
            string memberName = string.Empty;
            string chapterName = string.Empty;
            int trips = 0;
            
            foreach (DataRow row in reportDt.Rows)
            {
                chapterName = row[0].ToString();
                chapterId = chapterList.FirstOrDefault(x => x.Value == chapterName).Key;

                if (chapterId == 1) continue;

                foreach (DataColumn column in reportDt.Columns)
                {
                    memberName = column.ColumnName;
                    memberId = memberList.FirstOrDefault(x => x.Value == memberName).Key;

                    //SAVE MAY BE ABLE TO USE FOR MILES TOTALS
                    //List<string> trips = (from a in db.Rides
                    //            where a.MemberId == memberId && a.RideTo == chapterId
                    //            select a.Miles).ToList();

                    DateTime fromDate;
                    DateTime toDate;
                    GetDateParameter(year,out fromDate, out toDate);

                    trips = (from a in db.Rides
                             where a.MemberId == memberId && a.RideTo == chapterId
                             && (a.RideDate >= fromDate && a.RideDate <= toDate)
                             select a.Miles).Count();
                    //if(trips.Count>0)
                    //{
                    //    //SAVE MAY BE ABLE TO USE FOR MILES TOTALS
                    //    //   List<int> tripsMilesNum = trips.Select(int.Parse).ToList();
                    //    // int numOfVisits = tripsMilesNum.Sum();
                    //}


                    if (trips > 0)
                    {
                        row[column] = trips;
                        reportDt.AcceptChanges();
                        row.SetModified();
                    }
                }

                SetTotals(reportDt, row);
            }
        }

        private static void GetDateParameter(string year, out DateTime fromDate, out DateTime toDate)
        {
            DateTime defaultDate = System.DateTime.Now;
            string defaultToString = string.Empty;
            string defaultFromString = string.Empty;


               if(string.IsNullOrEmpty(year)) 
               {
                   //if the current month  =  12 then we are starting a new year so incrementing the year
                   if (System.DateTime.Today.Month == 12)
                   {
                       defaultFromString = string.Concat("12/01/", System.DateTime.Today.Year);
                       defaultToString = string.Concat("12/01/", System.DateTime.Today.Year + 1 );
                   }
                   else
                   {
                       defaultFromString = string.Concat("12/01/", System.DateTime.Today.Year-1);
                       defaultToString = string.Concat("12/01/", System.DateTime.Today.Year);
                   }

               }
              

            
            switch (year)
            {
                case "2016":
                    fromDate = DateTime.Parse("12/01/2015");
                    toDate = DateTime.Parse("12/01/2016");
                    break;
                case "2017":
                    fromDate = DateTime.Parse("12/01/2016");
                    toDate = DateTime.Parse("12/01/2017");
                    break;
                case "2018":
                    fromDate = DateTime.Parse("12/01/2017");
                    toDate = DateTime.Parse("12/01/2018");
                    break;
                case "2019":
                    fromDate = DateTime.Parse("12/01/2018");
                    toDate = DateTime.Parse("12/01/2019");
                    break;
                case "2020":
                    fromDate = DateTime.Parse("12/01/2019");
                    toDate = DateTime.Parse("12/01/2020");
                    break;
                case "2021":
                    fromDate = DateTime.Parse("12/01/2020");
                    toDate = DateTime.Parse("12/01/2021");
                    break;
                case "2022":
                    fromDate = DateTime.Parse("12/01/2021");
                    toDate = DateTime.Parse("12/01/2022");
                    break;
                case "2023":
                    fromDate = DateTime.Parse("12/01/2022");
                    toDate = DateTime.Parse("12/01/2023");
                    break;
                case "2024":
                    fromDate = DateTime.Parse("12/01/2023");
                    toDate = DateTime.Parse("12/01/2024");
                    break;
                case "2025":
                    fromDate = DateTime.Parse("12/01/2024");
                    toDate = DateTime.Parse("12/01/2025");
                    break;
                default:
                    toDate = Convert.ToDateTime(defaultToString);
                    fromDate = Convert.ToDateTime(defaultFromString);
                    break;
            }
            
        }

        private static void SetTotals(DataTable reportDt, DataRow row)
        {
            int chTotal = 0;

            foreach (DataColumn columnChTotals in reportDt.Columns)
            {
                if (columnChTotals.Caption == "Chapter Name") continue;

                foreach (DataRow rowChTotals in reportDt.Rows)
                {

                    if (rowChTotals.ItemArray[0].ToString() == "Totals")
                    {
                        rowChTotals[columnChTotals] = chTotal.ToString();
                        reportDt.AcceptChanges();
                        row.SetModified();
                        chTotal = 0;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(rowChTotals[columnChTotals].ToString()))
                            chTotal = chTotal + Int32.Parse(rowChTotals[columnChTotals].ToString());
                    }
                }

            }
        }

        private static DataTable CreateReportDt(Dictionary<int, string> memberList, Dictionary<int, string> chapterList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Chapter Name");

            foreach (var member in memberList)
            {
                dt.Columns.Add(member.Value);
            }

            foreach (var chapter in chapterList)
            {
                if(chapter.Key !=1)
                dt.Rows.Add(chapter.Value);
            }
            dt.Rows.Add("Totals");
            return dt;
        }

        private static Dictionary<int, string> GetChaptersList(WMMCRC db)
        {
            Dictionary<int, string> chapterList = new Dictionary<int, string>();
            
            chapterList = db.Chapters
                          .ToDictionary(a => a.ChapterId, a => a.ChapterName);

            return chapterList;
        }

        private static Dictionary<int, string> GetMembersList(WMMCRC db)
        {
            Dictionary<int, string> memberList = new Dictionary<int, string>();
            memberList = db.Members
                           .Where(a => a.Active == true)
                            .ToDictionary(a => a.MemberId,
                                           a => a.FullName);
            return memberList;
        }

        public static List<string>FillDropDowns()
        {
            //Years
            List<string> years = new List<string>();
            years.Add("2017");
            years.Add("2018");
            years.Add("2019");
            years.Add("2020");
            years.Add("2021");
            years.Add("2022");
            years.Add("2023");
            years.Add("2024");
            years.Add("2025");
            return years;
        }


    }
}
//Validation SQL
//SELECT c.FullName
//         ,B.ChapterName
//       ,A.[MemberId]
//      ,A.[RideFrom]
//      ,A.[RideTo]
//      ,A.[Miles]
//FROM [WMMCRC].[dbo].[Rides] A
//LEFT JOIN Chapters B ON A.RideTo = b.ChapterId
//LEFT JOIN Members C ON A.MemberId = c.MemberId
//where a.MemberId = 1
//and A.[RideTo] <>1
