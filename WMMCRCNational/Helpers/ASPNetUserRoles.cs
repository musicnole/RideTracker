using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMMCRCNational.Models;

namespace WMMCRCNational.Helpers
{
    public class ASPNetUserRoles
    {
        public static string GetUserName(WMMCRC db, string Id)
        {
            string userName = string.Empty;

            userName = (from a in db.AspNetUsers
                           where a.Id == Id
                           select a.UserName).FirstOrDefault();

            return userName;
        }

        public static string GetRole(WMMCRC db, string Id)
        {
            string role = string.Empty;

            role = (from a in db.AspNetRoles
                        where a.Id == Id
                        select a.Name).FirstOrDefault();

            return role;
        }
    }
}