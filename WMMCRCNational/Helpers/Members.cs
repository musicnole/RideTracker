using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMMCRCNational.Models;

namespace WMMCRCNational.Helpers
{
    public class Members
    {
        public static List<Member> GetMembers(WMMCRC db, string active)
        {
            List<Member> memberList = new List<Member>();

            switch (active)
            {
                case "True":
                    memberList = (from a in db.Members
                                  where a.Active == true
                                  select a).ToList();
                    break;
                case "False":
                    memberList = (from a in db.Members
                                  where a.Active == false
                                  select a).ToList();
                    break;
                default:
                    memberList = db.Members.ToList();
                    break;
            }

            return memberList;

        }
    }
}