using System;
using System.Globalization;

namespace JewishCalendarUWP
{
    /// <summary>
    /// A class that simplifies converting between Hebrew and Gregorian dates.
    /// </summary>
    public struct JewishDate
    {
        public DateTime GregDate { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public DayOfWeek DayOfWeek { get; }
        public int DayOfYear { get; }

        /// <summary>
        /// Sets the date according to the Hebrew Calendar
        /// </summary>
        /// <param name="year">Number of years since Creation</param>
        /// <param name="month">Month of the year</param>
        /// <param name="day">Day of the month</param>
        public JewishDate(int year, int month, int day)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            GregDate = hebCal.ToDateTime(year, month, day, 0, 0, 0, 0);

            Year = year;
            Month = month;
            Day = day;

            DayOfWeek = hebCal.GetDayOfWeek(GregDate);
            DayOfYear = hebCal.GetDayOfYear(GregDate);
        }

        /// <summary>
        /// Sets the date according to the Gregorian Calendar
        /// </summary>
        /// <param name="date">The Gregorian date</param>
        public JewishDate(DateTime date)
        {
            GregDate = date;

            HebrewCalendar hebCal = new HebrewCalendar();

            Year = hebCal.GetYear(date);
            Month = hebCal.GetMonth(date);
            Day = hebCal.GetDayOfMonth(date);

            DayOfWeek = hebCal.GetDayOfWeek(GregDate);
            DayOfYear = hebCal.GetDayOfYear(GregDate);
        }

        public static implicit operator DateTime(JewishDate date)
        {
            return date.GregDate;
        }

        public static implicit operator JewishDate(DateTime date)
        {
            return new JewishDate(date);
        }

        /// <summary>
        /// Returns a new JewishDate with the TimeSpan added.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to be added</param>
        /// <returns></returns>
        public JewishDate Add(TimeSpan timeSpan)
        {
            return new JewishDate(GregDate.Add(timeSpan));
        }

        /// <summary>
        /// Returns a new JewishDate with the TimeSpan subtracted.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to be subtracted</param>
        /// <returns></returns>
        public JewishDate Subtract(TimeSpan timeSpan)
        {
            return new JewishDate(GregDate.Subtract(timeSpan));
        }

        /// <summary>
        /// Returns the first occurrence of the specified <see cref="System.DayOfWeek"/> after the <see cref="JewishDate"/>. 
        /// If the <see cref="JewishDate"/> falls on the specified <see cref="System.DayOfWeek"/> then no days are added.
        /// </summary>
        /// <param name="day">The day of the week to search for</param>
        /// <returns>The <see cref="JewishDate"/> that matches the <paramref name="day"/>, could be the original <see cref="JewishDate"/></returns>
        public JewishDate FindNextDay(DayOfWeek day)
        {
            JewishDate result = this;

            while (result.DayOfWeek != day)
            {
                result = result.Add(new TimeSpan(1, 0, 0, 0));
            }

            return result;
        }
    }
}