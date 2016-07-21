using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Globalization.DateTimeFormatting;
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
    //Add special Shabbos'

    //Either have methods for checking each date, or include it into one and they search it's result
    //Maybe create a helper class that easily return the Greg or Jewish 

    //It is going to be more efficient to split it up into separate methods. However, its more confusing.
    //Performance decrease shouldn't really be noticeable.
    public class JewishCalendar
    {
        // Measured in ticks.
        private static long LunerMonthRemainder = 1322433333069;
        private static long SimpleLunerYearRemainder = 3773199999708;
        private static long LeapLunerYearRemainder = 5095633333137;
        private static long NineteenYearCycleRemainder = 2323833333135;

        /// <summary>
        /// Returns the Molad as a TimeSpan. The number of days mark the day of the week
        /// </summary>
        /// <param name="year">Jewish Year</param>
        /// <param name="month">Jewish Month</param>
        /// <returns></returns>
        public static TimeSpan GetMolad(int year, int month)
        {
            TimeSpan molad = TimeSpan.FromTicks(1698799999932); //6 hours have been subtracted from the Rambam's Molad Tohu as our night starts at 12AM
          
            year--; //Number of complete years

            int cycleNum = year / 19;
            int yearInCycle = year % 19;

            molad = molad.Add(TimeSpan.FromTicks(cycleNum * NineteenYearCycleRemainder)); //For past cycles

            for (int i = 1; i <= yearInCycle; i++) 
            {
                if (IsLeapYear(cycleNum * 19 + i))
                {
                    molad = molad.Add(TimeSpan.FromTicks(LeapLunerYearRemainder));
                }
                else
                {
                    molad = molad.Add(TimeSpan.FromTicks(SimpleLunerYearRemainder));
                }
            }

            molad = molad.Add(TimeSpan.FromTicks((month - 1) * LunerMonthRemainder)); //For every month after Tishrei

            int moladDays = molad.Days % 7;
            if (moladDays == 0)
            {
                moladDays = 7;
            }

            molad = new TimeSpan(moladDays, molad.Hours, molad.Minutes, molad.Seconds);

            return molad;
        }

        public static IList<SpecialDates> GetDayInfo(JewishDate date, bool inIsrael)
        {
            IList<SpecialDates> specialDates = new List<SpecialDates>();

            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            bool isShabbos = IsShabbos(date);
            bool isYesterdayShabbos = IsYesterdayShabbos(date);
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
                        if ((day == 11 && IsShabbos(date.GregDate.Subtract(new TimeSpan(1, 0, 0, 0)))) || (day == 13 && !isShabbos))
                        {
                            specialDates.Add(SpecialDates.FastOfEsther);
                        }
                        else if (day == 14)
                        {
                            specialDates.Add(SpecialDates.Purim);
                        }
                        else if (day == 15)
                        {
                            specialDates.Add(SpecialDates.ShushanPurim);
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
                    if ((day == 11 && IsShabbos(date.GregDate.Subtract(new TimeSpan(1, 0, 0, 0)))) || (day == 13 && !isShabbos))
                    {
                        specialDates.Add(SpecialDates.FastOfEsther);
                    }
                    else if (day == 14)
                    {
                        specialDates.Add(SpecialDates.Purim);
                    }
                    else if (day == 15)
                    {
                        specialDates.Add(SpecialDates.ShushanPurim);
                    }
                    break;
                case Months.Nissan:
                    if ((day == 14 && !isShabbos) || (day == 12 && IsShabbos(date.GregDate.Subtract(new TimeSpan(1, 0, 0, 0)))))
                    {
                        specialDates.Add(SpecialDates.FastOfTheFirstborn);
                    }
                    else if ((day > 14 && day < 22) || (day == 22 && !inIsrael))
                    {
                        specialDates.Add(SpecialDates.Pesach);
                    }
                    break;
                case Months.Iyar:
                    if (day == 14)
                    {
                        specialDates.Add(SpecialDates.PesachSheni);
                    }
                    else if (day == 18)
                    {
                        specialDates.Add(SpecialDates.LagBaomer);
                    }
                    break;
                case Months.Sivan:
                    if (day == 6 || (day == 7 && !inIsrael))
                    {
                        specialDates.Add(SpecialDates.Shovuos);
                    }
                    break;
                case Months.Tamuz:
                    if ((day == 17 && !isShabbos) || (day == 18 && isYesterdayShabbos))
                    {
                        specialDates.Add(SpecialDates.SeventeenthOfTamuz);
                    }
                    break;
                case Months.Av:
                    if ((day == 9 && !isShabbos) || (day == 10 && isYesterdayShabbos))
                    {
                        specialDates.Add(SpecialDates.NinthOfAv);
                    }
                    else if (day == 15)
                    {
                        specialDates.Add(SpecialDates.TuBeAv);
                    }
                    break;
                case Months.Elul:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(month), month, null);
            }

            //Check if Rosh Chodesh
            if (IsRoshChodesh(date))
            {
                specialDates.Add(SpecialDates.RoshChodesh);
            }

            return specialDates;
        }

        public static bool IsLeapYear(JewishDate date)
        {
            return IsLeapYear(date.Year);
        }

        public static bool IsLeapYear(int year)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            return hebCal.IsLeapYear(year);
        }

        public static bool IsShabbos(JewishDate date)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            return (hebCal.GetDayOfWeek(date) == DayOfWeek.Saturday);
        }

        public static bool IsYesterdayShabbos(JewishDate date)
        {
            DateTime yesterday = date.GregDate.Subtract(new TimeSpan(1, 0, 0, 0));

            return IsShabbos(yesterday);
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

        public static bool IsRoshChodesh(JewishDate date)
        {
            //Rosh Hashana isn't considered a Rosh Chodesh
            if (date.Day == 30 || (date.Day == 1 && date.Month != (int)Months.Tishrei))
            {
                return true;
            }

            return false;
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
        ShushanPurim,
        FastOfTheFirstborn,
        Pesach,
        PesachSheni,
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