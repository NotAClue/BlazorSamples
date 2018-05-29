using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotAClue
{
    public static class DateTimeExtensionMethods
    {
        #region Constants
        private const int FIRST_WEEK = 1;
        private const int SECOND_WEEK = 2;
        private const int THIRD_WEEK = 3;
        private const int FOURTH_WEEK = 4;
        private const int LAST_WEEK = 5;
        #endregion

        #region fields
        /// <summary>
        /// DaysOfWeek Enum
        /// Sunday    = 0,
        /// Monday    = 1,
        /// Tuesday   = 2,
        /// Wednesday = 3,
        /// Thursday  = 4,
        /// Friday    = 5,
        /// Saturday  = 6
        /// </summary>
        private static Dictionary<String, DayOfWeek> _threeLetterDays = new Dictionary<String, DayOfWeek>()
        {
            {"Sun", DayOfWeek.Sunday },
            {"Mon", DayOfWeek.Monday },
            {"Tue", DayOfWeek.Tuesday },
            {"Wed", DayOfWeek.Wednesday },
            {"Thu", DayOfWeek.Thursday },
            {"Fri", DayOfWeek.Friday },
            {"Sat", DayOfWeek.Saturday },
        };
        #endregion

        /// <summary>
        /// Gets a range of dates all starting with the day of the week specified for the date range.
        /// </summary>
        /// <param name="startingDate">The starting date.</param>
        /// <param name="endingDate">The ending date.</param>
        /// <param name="dayOfTheWeek">The day of the week.</param>
        /// <returns>IEnumerable&lt;DateTime&gt;.</returns>
        public static IEnumerable<DateTime> GetRangeOfDates(this DateTime startingDate, DateTime endingDate, DayOfWeek dayOfTheWeek)
        {
            var dates = Enumerable.Range(0, 1 + endingDate.Subtract(startingDate).Days)
                .Select(offset => startingDate.AddDays(offset))
                .Where(d => d.DayOfWeek == dayOfTheWeek).ToList();

            return dates;
        }

        public static IDictionary<DateTime, TimeSpan> GetHoursPerDay(this DateTime startingDate, DateTime endingDate)
        {
            // create new list of Dates with time for that day
            var allDates = new Dictionary<DateTime, TimeSpan>();

            if (startingDate.DayOfYear == endingDate.DayOfYear)
            {
                allDates.Add(startingDate, endingDate - startingDate);
            }
            else
            {
                var previousDay = startingDate;
                for (DateTime currentDay = startingDate.AddDays(1).Date; currentDay <= endingDate; currentDay = currentDay.AddDays(1))
                {
                    allDates.Add(previousDay, currentDay - previousDay);
                    previousDay = currentDay;
                }
                if (endingDate > previousDay)
                    allDates.Add(previousDay.AddDays(1), endingDate - previousDay);
            }

            return allDates;
        }

        /// <summary>
        /// Times to next.
        /// </summary>
        /// <param name="currentDate">The current date.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan TimeToNext(this DateTime currentDate, DayOfWeek dayOfWeek)
        {
            var nextDayOf = currentDate.NextDayOfWeek(dayOfWeek);
            return nextDayOf - DateTime.Now;
        }

        /// <summary>
        /// Next the day of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="weekday">The weekday.</param>
        /// <returns>DateTime.</returns>
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek weekday)
        {
            return (from i in Enumerable.Range(1, 7)
                    where date.AddDays(i).DayOfWeek == weekday
                    select date.AddDays(i)).First();
        }

        /// <summary>
        /// Previous the day of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="weekday">The weekday.</param>
        /// <returns>DateTime.</returns>
        public static DateTime PreviousDayOfWeek(this DateTime date, DayOfWeek weekday)
        {
            var enumerableRange = Enumerable.Range(-6, 7);
            return (from i in enumerableRange
                    where date.AddDays(i).DayOfWeek == weekday
                    select date.AddDays(i)).First();
        }

        /// <summary>
        /// Converts a string to the a DayOfWeek type.
        /// </summary>
        /// <param name="dayText">The day text.</param>
        /// <returns>DayOfWeek.</returns>
        public static DayOfWeek ToDayOfWeek(this String dayText)
        {
            // three letter day names
            if (dayText.Length == 3 && _threeLetterDays.Keys.Contains(dayText))
                return _threeLetterDays[dayText];

            // full day names
            if (dayText.Length > 3)
                return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayText, true);

            return DayOfWeek.Sunday;
        }

        /// <summary>
        /// Ins the last quarter.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="currentDate">The current date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Boolean InLastQuarter(this DateTime date, DateTime currentDate)
        {
            var soq = currentDate.FirstDayOfQuarter();
            var eoq = currentDate.LastDayOfQuarter();
            return (date >= soq && date <= eoq);
        }

        /// <summary>
        /// Informations the last quarter.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="currentDate">The current date.</param>
        /// <returns>Boolean.</returns>
        public static Boolean InLastQuarter(this DateTime? date, DateTime currentDate)
        {
            if (date.HasValue)
                return ((DateTime)date).InLastQuarter(currentDate);
            else
                return false;
        }

        /// <summary>
        /// Gets the quarter from the date supplied.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetQuarter(this DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// Gets the quarter.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>System.Int32.</returns>
        public static int GetQuarter(this DateTime? date)
        {
            if (date.HasValue)
                return ((DateTime)date).GetQuarter();
            else
                return 0;
        }

        /// <summary>
        /// Firsts the day of year.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime FirstDayOfYear(this DateTime date)
        {
            return new DateTime(DateTime.Today.Year, 1, 1);
        }

        /// <summary>
        /// Firsts the day of year.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FirstDayOfYear(this DateTime? date)
        {
            if (date.HasValue)
                return ((DateTime)date).FirstDayOfYear();
            else
                return DateTime.MinValue.FirstDayOfYear();
        }
        /// <summary>
        /// Lasts the day of Q year.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime LastDayOfQYear(this DateTime date)
        {
            return new DateTime(DateTime.Today.Year, 12, 31);
        }

        /// <summary>
        /// Lasts the day of q year.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime LastDayOfQYear(this DateTime? date)
        {
            if (date.HasValue)
                return ((DateTime)date).LastDayOfQYear();
            else
                return DateTime.MinValue.LastDayOfQYear();
        }

        /// <summary>
        /// Returns first the day of quarter with quarters
        /// starting at the beginning of Jan/Apr/Jul/Oct
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FirstDayOfQuarter(this DateTime date)
        {
            var quarter = date.GetQuarter();
            var month = 3 * quarter - 2;
            return new DateTime(date.Year, month, 1);
        }

        /// <summary>
        /// Firsts the day of quarter.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FirstDayOfQuarter(this DateTime? date)
        {
            if (date.HasValue)
                return ((DateTime)date).FirstDayOfQuarter();
            else
                return DateTime.MinValue.FirstDayOfQuarter();
        }

        /// <summary>
        /// Returns the last the day of quarter with 
        /// quarters ending at the end of Mar/Jun/Sep/Dec
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime LastDayOfQuarter(this DateTime date)
        {
            var quarter = date.GetQuarter();
            var month = 3 * quarter;
            return new DateTime(date.Year, month, DateTime.DaysInMonth(date.Year, month));
        }

        /// <summary>
        /// Lasts the day of quarter.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime LastDayOfQuarter(this DateTime? date)
        {
            if (date.HasValue)
                return ((DateTime)date).LastDayOfQuarter();
            else
                return DateTime.MinValue.LastDayOfQuarter();
        }

        /// <summary>
        /// Gets last day of the week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Last day of the wee for the given date.</returns>
        /// <remarks></remarks>
        public static DateTime LastDayOfWeek(this DateTime date, DateTime today)
        {
            var dayOfWeek = (int)today.DayOfWeek + 1;
            return date.AddDays(dayOfWeek - 7);
        }

        /// <summary>
        /// Lasts the day of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime LastDayOfWeek(this DateTime? date, DateTime today)
        {
            if (date.HasValue)
                return ((DateTime)date).LastDayOfWeek(today);
            else
                return DateTime.MinValue.LastDayOfWeek(today);
        }

        /// <summary>
        /// Gets the first day of the week for the date supplied.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="firstDayOfTheWeek">The first day of the week.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FirstDayOfWeek(this DateTime date, DayOfWeek firstDayOfTheWeek = DayOfWeek.Sunday)
        {
            var startOfWeek = (int)firstDayOfTheWeek;
            var dayOfWeek = (int)date.DayOfWeek;
            return date.AddDays(-(dayOfWeek - startOfWeek));
        }

        /// <summary>
        /// Firsts the day of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="firstDayOfTheWeek">The first day of the week.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public static DateTime? FirstDayOfWeek(this DateTime? date, DayOfWeek firstDayOfTheWeek = DayOfWeek.Sunday)
        {
            if (date.HasValue)
                return ((DateTime)date).FirstDayOfWeek(firstDayOfTheWeek);

            return null;
        }

        /// <summary>
        /// Firsts the day of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime date, DateTime today)
        {
            var dayOfWeek = (int)today.DayOfWeek;
            return date.AddDays(-(dayOfWeek));
        }

        /// <summary>
        /// Firsts the day of week.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FirstDayOfWeek(this DateTime? date, DateTime today)
        {
            if (date.HasValue)
                return ((DateTime)date).FirstDayOfWeek(today);
            else
                return DateTime.MinValue.FirstDayOfWeek(today);
        }

        /// <summary>
        /// Gets the first the DayOfWeek in month as a DateTime.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FirstDayOfWeekInMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month));
            var day = (from d in daysInMonth
                       orderby d
                       where new DateTime(date.Year, date.Month, d).DayOfWeek == dayOfWeek
                       select new DateTime(date.Year, date.Month, d)).First();

            return day;
        }

        /// <summary>
        /// Adds the days.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="days">The days.</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan AddDays(this TimeSpan timeSpan, int days)
        {
            return new TimeSpan(days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        /// <summary>
        /// Firsts the day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Lasts the day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1);
        }


        ///// <summary>
        ///// Determines whether  the date passed in is a federal holiday.
        ///// </summary>
        ///// <param name="date">The date.</param>
        ///// <returns>Boolean.</returns>
        //public static Boolean IsStsHoliday(this DateTime date)
        //{
        //    var federalHolidays = GetStsHolidayList(date.Year);
        //    return federalHolidays.Where(d => d.DayOfYear == date.DayOfYear).Any();
        //}

        //private static List<DateTime> GetStsHolidayList(int vYear)
        //{
        //    List<DateTime> HolidayList = new List<DateTime>();

        //    //   http://www.usa.gov/citizens/holidays.shtml      
        //    //   http://archive.opm.gov/operating_status_schedules/fedhol/2013.asp

        //    // New Year's Day            Jan 1
        //    HolidayList.Add(new DateTime(vYear, 1, 1));

        //    //// Martin Luther King, Jr. third Mon in Jan
        //    //HolidayList.Add(GetNthDayOfNthWeek(new DateTime(vYear, 1, 1), DayOfWeek.Monday.ToInt(), THIRD_WEEK));

        //    //// Washington's Birthday third Mon in Feb
        //    //HolidayList.Add(GetNthDayOfNthWeek(new DateTime(vYear, 2, 1), DayOfWeek.Monday.ToInt(), THIRD_WEEK));

        //    // Memorial Day          last Mon in May
        //    HolidayList.Add(GetNthDayOfNthWeek(new DateTime(vYear, 5, 1), DayOfWeek.Monday.ToInt(), LAST_WEEK));

        //    // Independence Day      July 4
        //    HolidayList.Add(new DateTime(vYear, 7, 4));

        //    // Labor Day             first Mon in Sept
        //    HolidayList.Add(GetNthDayOfNthWeek(new DateTime(vYear, 9, 1), DayOfWeek.Monday.ToInt(), FIRST_WEEK));

        //    //// Columbus Day          second Mon in Oct
        //    //HolidayList.Add(GetNthDayOfNthWeek(new DateTime(vYear, 10, 1), DayOfWeek.Monday.ToInt(), SECOND_WEEK));

        //    //// Veterans Day          Nov 11
        //    //HolidayList.Add(new DateTime(vYear, 11, 11));

        //    // Thanksgiving Day      fourth Thur in Nov
        //    HolidayList.Add(GetNthDayOfNthWeek(new DateTime(vYear, 11, 1), DayOfWeek.Thursday.ToInt(), FOURTH_WEEK));

        //    // Christmas Eve         Dec 24
        //    HolidayList.Add(new DateTime(vYear, 12, 24));

        //    // Christmas Day         Dec 25
        //    HolidayList.Add(new DateTime(vYear, 12, 25));

        //    // New Year's Eve        Dec 31
        //    HolidayList.Add(new DateTime(vYear, 12, 31));

        //    //saturday holidays are moved to Fri; Sun to Mon
        //    for (int i = 0; i <= HolidayList.Count - 1; i++)
        //    {
        //        DateTime dt = HolidayList[i];
        //        if (dt.DayOfWeek == DayOfWeek.Saturday)
        //        {
        //            HolidayList[i] = dt.AddDays(-1);
        //        }
        //        if (dt.DayOfWeek == DayOfWeek.Sunday)
        //        {
        //            HolidayList[i] = dt.AddDays(1);
        //        }
        //    }

        //    return HolidayList;
        //}

        //private static DateTime GetNthDayOfNthWeek(DateTime dt, int dayOfWeek, int whichWeek)
        //{
        //    //specify which day of which week of a month and this function will get the date
        //    //this function uses the month and year of the date provided

        //    //get first day of the given date
        //    DateTime dtFirst = new DateTime(dt.Year, dt.Month, 1);

        //    //get first DayOfWeek of the month
        //    DateTime dtRet = dtFirst.AddDays(6 - dtFirst.AddDays(-(dayOfWeek + 1)).DayOfWeek.ToInt());

        //    //get which week
        //    dtRet = dtRet.AddDays((whichWeek - 1) * 7);

        //    //if day is past end of month then adjust backwards a week
        //    if (dtRet >= dtFirst.AddMonths(1))
        //    {
        //        dtRet = dtRet.AddDays(-7);
        //    }

        //    return dtRet;
        //}
    }
}
