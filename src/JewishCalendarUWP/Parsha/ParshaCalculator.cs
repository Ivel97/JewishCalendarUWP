using System;
using System.Globalization;
using static JewishCalendarUWP.Parsha.Enum;

namespace JewishCalendarUWP.Parsha
{
    public class ParshaCalculator
    {
        //Todo:
        //Add logic for parshios finding. Generate the year types
        //Check if YomTov
        //Check if Special Shabbos
        //Return TorahReading/TorahPortion

        /// <summary>
        /// Gets the TorahPortion that is read on that week's Shabbos. If, however, a YomTov falls on the Shabbos, and the Parsha gets postponed, it returns null.
        /// </summary>
        /// <param name="Date">Jewish Date in that week</param>
        /// <param name="inIsrael">A boolean value indicating the user's location, this has an effect on the Parsha</param>
        /// <returns>The Torah portion that is read that Shabbos</returns>
        public TorahPortion? GetTorahPortion(JewishDate Date, bool inIsrael)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            Date = Date.FindNextDay(DayOfWeek.Saturday); //Assign Date to the following Shabbos  

            int week = hebCal.GetWeekOfYear(Date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

            int rhStartDay = (int)JewishCalendar.GetRHDayOfWeek(Date) + 1;
            int addedDays = Convert.ToInt32(!JewishCalendar.IsCheshvanShort(Date.Year)) + Convert.ToInt32(JewishCalendar.IsKislevLong(Date.Year));//Either 0,1 or 2            
            bool isLeap = JewishCalendar.IsLeapYear(Date);

            if (!isLeap)
            {
                switch (rhStartDay)
                {
                    case 2:
                        if (addedDays == 0 || (addedDays == 2 && inIsrael))
                        {
                            return MonShort_MonLongIsrael_TueNormalIsrael[week - 1];
                        }
                        else
                        {
                            return MonLong_TueNormal[week - 1];
                        }
                        break;
                    case 3:
                        if (inIsrael)
                        {
                            return MonShort_MonLongIsrael_TueNormalIsrael[week - 1];
                        }
                        else
                        {
                            return MonLong_TueNormal[week - 1];
                        }
                        break;
                    case 5:
                        if (addedDays == 1)
                        {
                            return inIsrael ? ThuNormalIsrael[week - 1] : ThuNormal[week - 1];
                        }
                        else
                        {
                            return ThuLong[week - 1];
                        }
                        break;
                    case 7:
                        if (addedDays == 0)
                        {
                            return SatShort[week - 1];
                        }
                        else
                        {
                            return SatLong[week - 1];
                        }
                        break;
                    default:
                        throw new NotSupportedException("This type of year doesn't exist: " + rhStartDay + ", " + addedDays + ", " + isLeap);
                        break;
                }
            }
            else
            {
                switch (rhStartDay)
                {
                    case 2:
                        if (addedDays == 0)
                        {
                            return inIsrael ? MonShortLeapIsrael[week - 1] : MonShortLeap[week - 1];
                        }
                        else
                        {
                            return inIsrael ? MonLongLeapIsrael_TueNormalLeapIsrael[week - 1] : MonLongLeap_TueNormalLeap[week - 1];
                        }
                        break;
                    case 3:
                        return inIsrael ? MonLongLeapIsrael_TueNormalLeapIsrael[week - 1] : MonLongLeap_TueNormalLeap[week - 1];
                        break;
                    case 5:
                        if (addedDays == 0)
                        {
                            return ThuShortLeap[week - 1];
                        }
                        else
                        {
                            return ThuLongLeap[week - 1];
                        }
                        break;
                    case 7:
                        if (addedDays == 0 || (addedDays == 2 && inIsrael))
                        {
                            return SatShortLeap_SatLongLeapIsrael[week - 1];
                        }
                        else
                        {
                            return SatLongLeap[week - 1];
                        }
                        break;
                    default:
                        throw new NotSupportedException("This type of year doesn't exist: " + rhStartDay + ", " + addedDays + ", " + isLeap);
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the TorahPortion that is read on that week's Shabbos. If, however, a YomTov falls on the Shabbos, and the Parsha gets pushed off, it returns null.
        /// </summary>
        /// <param name="Year">That Jewish year</param>
        /// <param name="Week">The number of the week starting from Rosh Hashanah</param>
        /// <param name="inIsrael">A boolean value indicating the user's location, this has an effect on the Parsha</param>
        /// <returns>The Torah portion that is read that Shabbos</returns>
        public TorahPortion? GetTorahPortion(int Year, int Week, bool inIsrael)
        {
            HebrewCalendar hebCal = new HebrewCalendar();

            JewishDate date = hebCal.AddWeeks(new JewishDate(Year, 1, 1), Week - 1);
            
            return GetTorahPortion(date, inIsrael);
        }

        /// <summary>
        /// Returns detailed information about that week's reading, including Aliyos, Haftorah....
        /// </summary>
        /// <param name="Date">Jewish Date in that week</param>
        /// <param name="inIsrael">A boolean value indicating the user's location, this has an effect on the Parsha</param>
        /// <returns>Detailed information about the week's Torah reading</returns>
        public TorahReading GetTorahReading(JewishDate Date, bool inIsrael)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns detailed information about that week's reading, including Aliyos, Haftorah....
        /// </summary>
        /// <param name="Year">That Jewish year</param>
        /// <param name="Week">The number of the week starting from Rosh Hashanah</param>
        /// <param name="inIsrael">A boolean value indicating the user's location, this has an effect on the Parsha</param>
        /// <returns>Detailed information about the week's Torah reading</returns>
        public TorahReading GetTorahReading(int Year, int Week, bool inIsrael)
        {
            throw new NotImplementedException();
        }

        //Arrays describing the order of the parshiot for any given year type. Generated using misc/GenerateParshiotTables.cs
        private TorahPortion?[] MonShort_MonLongIsrael_TueNormalIsrael = { TorahPortion.Vayeilech, TorahPortion.HaAzinu, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel_Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, null, TorahPortion.Shmini, TorahPortion.Tazria_Metzora, TorahPortion.Achrei_Mot_Kedoshim, TorahPortion.Emor, TorahPortion.Behar_Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
        private TorahPortion?[] MonLong_TueNormal = { TorahPortion.Vayeilech, TorahPortion.HaAzinu, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel_Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, null, TorahPortion.Shmini, TorahPortion.Tazria_Metzora, TorahPortion.Achrei_Mot_Kedoshim, TorahPortion.Emor, TorahPortion.Behar_Bechukotai, TorahPortion.Bamidbar, null, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat_Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
        private TorahPortion?[] ThuNormal = { TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel_Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, null, null, TorahPortion.Shmini, TorahPortion.Tazria_Metzora, TorahPortion.Achrei_Mot_Kedoshim, TorahPortion.Emor, TorahPortion.Behar_Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim };
        private TorahPortion?[] ThuNormalIsrael = { TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel_Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, null, null, TorahPortion.Shmini, TorahPortion.Tazria_Metzora, TorahPortion.Achrei_Mot_Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo };
        private TorahPortion?[] ThuLong = { TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, null, TorahPortion.Shmini, TorahPortion.Tazria_Metzora, TorahPortion.Achrei_Mot_Kedoshim, TorahPortion.Emor, TorahPortion.Behar_Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim };
        private TorahPortion?[] SatShort = { null, TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel_Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, null, TorahPortion.Shmini, TorahPortion.Tazria_Metzora, TorahPortion.Achrei_Mot_Kedoshim, TorahPortion.Emor, TorahPortion.Behar_Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim };
        private TorahPortion?[] SatLong = { null, TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel_Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, null, TorahPortion.Shmini, TorahPortion.Tazria_Metzora, TorahPortion.Achrei_Mot_Kedoshim, TorahPortion.Emor, TorahPortion.Behar_Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
        private TorahPortion?[] MonShortLeap = { TorahPortion.Vayeilech, TorahPortion.HaAzinu, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, null, TorahPortion.Achrei_Mot, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, null, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat_Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
        private TorahPortion?[] MonShortLeapIsrael = { TorahPortion.Vayeilech, TorahPortion.HaAzinu, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, null, TorahPortion.Achrei_Mot, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
        private TorahPortion?[] MonLongLeap_TueNormalLeap = { TorahPortion.Vayeilech, TorahPortion.HaAzinu, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, null, null, TorahPortion.Achrei_Mot, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim };
        private TorahPortion?[] MonLongLeapIsrael_TueNormalLeapIsrael = { TorahPortion.Vayeilech, TorahPortion.HaAzinu, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, null, null, TorahPortion.Achrei_Mot, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot, TorahPortion.Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo };
        private TorahPortion?[] ThuShortLeap = { TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, TorahPortion.Achrei_Mot, null, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot, TorahPortion.Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim };
        private TorahPortion?[] ThuLongLeap = { TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, TorahPortion.Achrei_Mot, null, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot, TorahPortion.Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
        private TorahPortion?[] SatShortLeap_SatLongLeapIsrael = { null, TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, null, TorahPortion.Achrei_Mot, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat, TorahPortion.Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
        private TorahPortion?[] SatLongLeap = { null, TorahPortion.HaAzinu, null, null, TorahPortion.Bereshit, TorahPortion.Noach, TorahPortion.Lech_Lecha, TorahPortion.Vayera, TorahPortion.Chayei_Sara, TorahPortion.Toldot, TorahPortion.Vayetzei, TorahPortion.Vayishlach, TorahPortion.Vayeshev, TorahPortion.Miketz, TorahPortion.Vayigash, TorahPortion.Vayechi, TorahPortion.Shemot, TorahPortion.Vaera, TorahPortion.Bo, TorahPortion.Beshalach, TorahPortion.Yitro, TorahPortion.Mishpatim, TorahPortion.Terumah, TorahPortion.Tetzaveh, TorahPortion.Ki_Tisa, TorahPortion.Vayakhel, TorahPortion.Pekudei, TorahPortion.Vayikra, TorahPortion.Tzav, TorahPortion.Shmini, TorahPortion.Tazria, TorahPortion.Metzora, null, TorahPortion.Achrei_Mot, TorahPortion.Kedoshim, TorahPortion.Emor, TorahPortion.Behar, TorahPortion.Bechukotai, TorahPortion.Bamidbar, null, TorahPortion.Nasso, TorahPortion.Behaalotcha, TorahPortion.Shlach, TorahPortion.Korach, TorahPortion.Chukat_Balak, TorahPortion.Pinchas, TorahPortion.Matot_Masei, TorahPortion.Devarim, TorahPortion.Vaetchanan, TorahPortion.Eikev, TorahPortion.ReEh, TorahPortion.Shoftim, TorahPortion.Ki_Teitzei, TorahPortion.Ki_Tavo, TorahPortion.Nitzavim_Vayeilech };
    }
}
