using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMMCRCNational.Models;

namespace WMMCRCNational.Helpers
{
    public static class Events
    {

        public static IOrderedEnumerable<Event> LookupChapterNames(WMMCRC database)
        {
           // This Method returns only this years events

           List<Event> eventsList = database.Events.ToList();

            Dictionary<int, string> dictChapterName = new Dictionary<int, string>();
            dictChapterName = database.Chapters.ToDictionary(y => y.ChapterId, y => y.ChapterName);

            List<Event> thisYearEvent = GetThisYearEvents(eventsList);

            foreach (Event item in thisYearEvent)
            {
                //item.ChapterName = (from a in database.Chapters
                //                    where a.ChapterId == item.ChapterId
                //                    select a.ChapterName).FirstOrDefault();
                item.ChapterName = dictChapterName[item.ChapterId];

            }
            
            return thisYearEvent.OrderByDescending(s => s.EventDate); 
        }

        public static string GetChapterName(int? eventId,WMMCRC database)
        {

            int chapterId = (from b in database.Events
                             where b.EventId == eventId
                             select b.ChapterId).FirstOrDefault();

            string chapterName = (from a in database.Chapters
                                  where a.ChapterId == chapterId
                                  select a.ChapterName).FirstOrDefault();
            return chapterName;

        }

        public static List<Event> GetThisYearEvents(List<Event> allEvents)
        {
            int thisYear = DateTime.Now.Year;
            int thisMonth = DateTime.Now.Month;
            List<Event> eventsListCurrent = new List<Event>();
            eventsListCurrent = allEvents.Where(x => x.EventDate.Value.Year == thisYear).ToList();

            return eventsListCurrent;
        }

        public static IOrderedEnumerable<Event> GetSearchDateView(WMMCRC database,string year)
        {
            IOrderedEnumerable<Event> eventList;

            IEnumerable<Event> eventListobj = new List<Event>();
            int thisYear = DateTime.Now.Year;
            List<Event> eventsList = database.Events.ToList();


            eventListobj = eventsList.Where(s => s.EventDate.Value.Year == Convert.ToInt32(year));

            eventList = eventListobj.OrderByDescending(s => s.EventDate);

            LookupChapterNames(database,eventList);

            return eventList;
        }

        public static IOrderedEnumerable<Event> LookupChapterNames(WMMCRC database, IOrderedEnumerable<Event> noChapterEvents)
        {
            
            Dictionary<int, string> dictChapterName = new Dictionary<int, string>();
            dictChapterName = database.Chapters.ToDictionary(y => y.ChapterId, y => y.ChapterName);
                       

            foreach (Event item in noChapterEvents)
            {
                //item.ChapterName = (from a in database.Chapters
                //                    where a.ChapterId == item.ChapterId
                //                    select a.ChapterName).FirstOrDefault();
                item.ChapterName = dictChapterName[item.ChapterId];

            }

            return noChapterEvents;
        }
        public static IOrderedEnumerable<Event> GetSearchEventName(IOrderedEnumerable<Event> eventListOrig, string searchName)
        {
            IOrderedEnumerable<Event> eventList;

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                eventList = (from ch in eventListOrig
                             where ch.EventTitle != null
                             && ch.EventTitle.ToString().ToLower().Contains(searchName.ToLower())
                             select ch).ToList().OrderByDescending(s => s.EventDate);
            }
            else
            {
                eventList = eventListOrig.OrderByDescending(s => s.EventDate);
            }


            return eventList;
        }

       public static IOrderedEnumerable<Event> GetSearchChapter(IOrderedEnumerable<Event> eventListOrig, string chapterId)
        {
            Int32 chapter = 0;

            IOrderedEnumerable<Event> eventList;
            chapter = Convert.ToInt32(chapterId);
            eventList = (from ch in eventListOrig
                         where ch.ChapterId == chapter
                         select ch).ToList().OrderByDescending(s => s.EventDate);



            return eventList;
        }
    }
}