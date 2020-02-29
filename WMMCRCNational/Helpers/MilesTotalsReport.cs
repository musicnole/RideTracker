using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMMCRCNational.Models;


namespace WMMCRCNational.Helpers
{
    public class MilesTotalsReport
    {
        public static int GetMilesTotal(int MemberId, string year,WMMCRC db)
        {
            int MilesTotal = 0;
            DateTime fromDate;
            DateTime toDate;
            GetDateParameter(year, out fromDate, out toDate);

            var milesList = (from a in db.Rides
                             where a.MemberId == MemberId && a.Miles != null
                              && (a.RideDate >= fromDate && a.RideDate <= toDate)
                             select a.Miles);
            foreach (string mile in milesList)
            {
                MilesTotal = MilesTotal + Int32.Parse(mile);
            }

            return MilesTotal;
        }

        private static void GetDateParameter(string year, out DateTime fromDate, out DateTime toDate)
        {
            DateTime defaultDate = System.DateTime.Now;
            string defaultToString = string.Empty;
            string defaultFromString = string.Empty;


            if (string.IsNullOrEmpty(year))
            {
                //if the current month  =  12 then we are starting a new year so incrementing the year
                if (System.DateTime.Today.Month == 12)
                {
                    defaultFromString = string.Concat("12/01/", System.DateTime.Today.Year);
                    defaultToString = string.Concat("12/01/", System.DateTime.Today.Year + 1);
                }
                else
                {
                    defaultFromString = string.Concat("12/01/", System.DateTime.Today.Year - 1);
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
    }
}