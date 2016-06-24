using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Security.Cryptography.Core;

namespace JewishCalendarUWP
{
    //Static class, includes:
    //contains ... yom tov, purim, chanukah .... isru chag
    //return all info for date
    //get dates of yomim tovim for that year
    //get molad for that month
    //Probably have enum for all the chagim, check date info if contains type Yomtov....
    //Get parsha for that week
    //Or for that day if it's YomTov, rosh chodesh
    //Might split up YomTov into yomtov and chol hamoed
    //Either have methods for checking each date, or include it into one and they search it's result
    //Maybe create a helper class that easily return the Greg or Jewish 

    //It is going to be more efficient to split it up into separate methods. However, its more confusing.
    //Performance decrease shouldn't really be noticeable.
    public class JewishCalendar
    {
        public static IList<SpecialDates> GetDayInfo(DateTime date, bool inIsrael)
        {
            JewishDate jDate = new JewishDate(date);

            return GetDayInfo(jDate.Year, jDate.Month, jDate.Day, inIsrael);
        }

        public static IList<SpecialDates> GetDayInfo(int year, int month, int day, bool inIsrael)
        {
            IList<SpecialDates> specialDates = new List<SpecialDates>();

            bool isShabbos = IsShabbos(year, month, day);
            bool isYesterdayShabbos = IsYesterdayShabbos(year, month, day);
            bool isCheshvanShort = IsCheshvanShort(year);
            bool isKislevLong = IsKislevLong(year);
            bool isLeapYear = IsLeapYear(year);
           
            if (!isLeapYear && month > 6)
            {
                //This is to keep it in-line with the enum Months. If it is a leap year Nissan = 7, if it isn't it's 6. This lines them up
                month++;
            }



            switch ((Months)month)
            {
                case Months.Tishrei:
                    if (day == 1 || day == 2)
                    {
                        specialDates.Add(SpecialDates.RoshHashana);
                    }
                    else if ((day == 3 && !isShabbos) || (day == 4 && isYesterdayShabbos))
                    {
                        specialDates.Add(SpecialDates.FastOfGedalia);
                    }
                    else if (day == 10)
                    {
                        specialDates.Add(SpecialDates.YomKippur);
                    }
                    else if ((day > 14 && day < 22))
                    {
                        specialDates.Add(SpecialDates.Sukkos);
                    }
                    else if (day == 22 && !inIsrael)
                    {
                        specialDates.Add(SpecialDates.ShminiAtzeres);
                    }
                    else if ((day == 22 && inIsrael) || (day == 23 && !inIsrael))
                    {
                        specialDates.Add(SpecialDates.SimchasTorah);
                    }
                    break;
                case Months.Cheshvan:
                    break;
                case Months.Kislev:
                    if (day > 24)
                    {
                        specialDates.Add(SpecialDates.Chanukah);
                    }
                    break;
                case Months.Teves:
                    if (day < 3 || (day == 3 && !isKislevLong))
                    {
                        specialDates.Add(SpecialDates.Chanukah);
                    }
                    else if (day == 10)
                    {
                        specialDates.Add(SpecialDates.TenthOfTeves);
                    }
                    break;
                case Months.Shvat:
                    if (day == 15)
                    {
                        specialDates.Add(SpecialDates.TuBishvat);
                    }
                    break;
                case Months.AdarI:
                    if (!isLeapYear)
                    {
                        if ((day == 11 && IsShabbos(year, month, day + 2)) || (day == 13 && !isShabbos))
                        {
                            specialDates.Add(SpecialDates.FastOfEsther);
                        }
                        else if (day == 14)
                        {
                            specialDates.Add(SpecialDates.Purim);
                        }
                    }
                    else
                    {
                        if (day == 14)
                        {
                            specialDates.Add(SpecialDates.PurimKatan);
                        }
                    }
                    break;
                case Months.AdarII:
                    if ((day == 11 && IsShabbos(year, month, day + 2)) || (day == 13 && !isShabbos))
                    {
                        specialDates.Add(SpecialDates.FastOfEsther);
                    }
                    else if (day == 14)
                    {
                        specialDates.Add(SpecialDates.Purim);
                    }
                    break;
                case Months.Nissan:
                    break;
                case Months.Iyar:
                    break;
                case Months.Sivan:
                    break;
                case Months.Tamuz:
                    break;
                case Months.Av:
                    break;
                case Months.Elul:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(month), month, null);
            }


            return specialDates;
        }

        public static bool IsLeapYear(DateTime date)
        {
            return IsLeapYear(new JewishDate(date).Year);
        }

        public static bool IsLeapYear(int year)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            return hebCal.IsLeapYear(year);
        }

        public static bool IsShabbos(DateTime date)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            return (hebCal.GetDayOfWeek(date) == DayOfWeek.Saturday);
        }

        public static bool IsShabbos(int year, int month, int day)
        {
            return IsShabbos(new JewishDate(year, month, day).GregDate);
        }

        public static bool IsYesterdayShabbos(DateTime date)
        {
            DateTime yesterday = date.Subtract(new TimeSpan(1, 0, 0, 0));

            return IsShabbos(yesterday);
        }

        public static bool IsYesterdayShabbos(int year, int month, int day)
        {
            return IsYesterdayShabbos(new JewishDate(year, month, day).GregDate);
        }

        public static bool IsCheshvanShort(int year)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            return (hebCal.GetDaysInMonth(year, 2) == 29);
        }

        public static bool IsKislevLong(int year)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            return (hebCal.GetDaysInMonth(year, 3) == 30);
        }

    }

    

    public enum SpecialDates
    {
        RoshChodesh,
        RoshHashana,
        FastOfGedalia,
        YomKippur,
        Sukkos,
        ShminiAtzeres,
        SimchasTorah,
        Chanukah,
        TenthOfTeves,
        TuBishvat,
        FastOfEsther,
        PurimKatan,
        Purim,
        SushanPurim,
        Pesach,
        LagBaomer,
        Shovuos,
        SeventeenthOfTamuz,
        NinthOfAv,
        TuBeAv
    }

    internal enum Months
    {
        Tishrei = 1,
        Cheshvan,
        Kislev,
        Teves,
        Shvat,
        AdarI,
        AdarII,
        Nissan,
        Iyar,
        Sivan,
        Tamuz,
        Av,
        Elul
    }


}