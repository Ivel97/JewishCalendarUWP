using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace misc
{
    class GenerateParshiotTables
    {
        /// <summary>
        /// A method that returns a list of all the parshios in a year, week by week. If the Shabbos falls on a Yom Tov then null is inserted instead.
        /// </summary>
        /// <param name="rhStartDay">The day of the week that Rosh Hashanah starts, Sunday = 1, Monday = 2...</param>
        /// <param name="addedDays">The number of days added the Cheshvan and Kislev depending if the year is Choser, Keseder or Sholem</param>
        /// <param name="phStartDay">The day of the week that Pesach starts, Sunday = 1, Monday = 2...</param>
        /// <param name="inIsrael">A boolean value indicating if the parshios are being calculated for Israel or not</param>
        /// <param name="isLeap">A boolean value indicating if it is a leap year or not</param>     
        /// <returns>An enumerable list of TorahPortions corresponding to the specified year</returns>
        public IEnumerable<TorahPortion?> GenerateListOfParshios(int rhStartDay, int addedDays, int phStartDay, bool isLeap, bool inIsrael)
        {
            List<TorahPortion?> ListOfParshios = new List<TorahPortion?>();
            int currentWeek = 1; //Counter for the current week
            int currentParsha = 1; //Counter for the current parsha, as listed in TorahPortion

            //The day of week that the following year's Rosh Hashanah falls on
            int nextRhStartDay = (353 + rhStartDay + addedDays + Convert.ToInt32(isLeap) * 30) % 7;            
            if (nextRhStartDay == 0)
            {
                nextRhStartDay = 7;
            }

            int numOfDays = 353 + addedDays + (Convert.ToInt32(isLeap) * 30);          

            //Number of whole weeks
            int numOfShabboses = numOfDays / 7;

            //Checks if the partial week crosses a shabbos
            if ((numOfDays % 7) + (rhStartDay - 1) >= 7)
            {
                numOfShabboses++;
            }

            //Adds the parshiot up to and including Sukkos. The number of postponed Shabboses depends on what day of the year Rosh Hashonah starts on.
            switch (rhStartDay)
            {
                case 2:
                    
                case 3:                    
                    ListOfParshios.Add(TorahPortion.Vayeilech);
                    ListOfParshios.Add(TorahPortion.HaAzinu);
                    ListOfParshios.Add(null);
                    currentWeek = 4;
                    break;
                case 5:
                    ListOfParshios.Add(TorahPortion.HaAzinu);
                    ListOfParshios.Add(null);
                    ListOfParshios.Add(null);
                    currentWeek = 4;
                    break;
                case 7:
                    ListOfParshios.Add(null);
                    ListOfParshios.Add(TorahPortion.HaAzinu);
                    ListOfParshios.Add(null);
                    ListOfParshios.Add(null);
                    currentWeek = 5;
                    break;
                default:
                    break;
            }

            //Loop through the remaining weeks of the year
            while (currentWeek <= numOfShabboses)
            {
                //Checks if the parsha is postponed
                if (isShabbosPostponed(currentWeek,rhStartDay,addedDays,inIsrael,isLeap))
                {
                    ListOfParshios.Add(null);// Mark as postponed and move on to next week

                    currentWeek++;
                }
                else
                {
                    /*Check if this week is one of the parshiot that can be combined, if so then it checks if 
                     *this year satisfies the criteria for the particular parsha to be combined. The criteria 
                     *were obtained from: http://www.shoresh.org.il/spages/articles/parashathibur.htm */
                    switch (currentParsha)
                    {
                        case 22:
                            if (!isLeap && !(phStartDay == 1 && rhStartDay == 5))
                            {
                                ListOfParshios.Add(TorahPortion.Vayakhel_Pekudei);
                                currentParsha = currentParsha + 2;//To skip pekudai
                            }
                            else
                            {
                                ListOfParshios.Add(TorahPortion.Vayakhel);
                                currentParsha++;
                            }
                            break;
                        case 27:
                            if (!isLeap)
                            {
                                ListOfParshios.Add(TorahPortion.Tazria_Metzora);
                                currentParsha = currentParsha + 2;//To skip Metzora
                            }
                            else
                            {
                                ListOfParshios.Add(TorahPortion.Tazria);
                                currentParsha++;
                            }
                            break;
                        case 29:
                            if (!isLeap)
                            {
                                ListOfParshios.Add(TorahPortion.Achrei_Mot_Kedoshim);
                                currentParsha = currentParsha + 2;//To skip Kedoshim
                            }
                            else
                            {
                                ListOfParshios.Add(TorahPortion.Achrei_Mot);
                                currentParsha++;
                            }
                            break;
                        case 32:
                            if (!isLeap && !(inIsrael && rhStartDay == 5 && phStartDay == 7))
                            {
                                ListOfParshios.Add(TorahPortion.Behar_Bechukotai);
                                currentParsha = currentParsha + 2;//To skip Bechukotai
                            }
                            else
                            {
                                ListOfParshios.Add(TorahPortion.Behar);
                                currentParsha++;
                            }
                            break;
                        case 39:
                            if (!inIsrael && phStartDay == 5)
                            {
                                ListOfParshios.Add(TorahPortion.Chukat_Balak);
                                currentParsha = currentParsha + 2;//To skip Balak
                            }
                            else
                            {
                                ListOfParshios.Add(TorahPortion.Chukat);
                                currentParsha++;
                            }
                            break;
                        case 42:
                            if (!isLeap || rhStartDay == 7 || phStartDay == 5 || (!inIsrael && phStartDay == 7))
                            {
                                ListOfParshios.Add(TorahPortion.Matot_Masei);
                                currentParsha = currentParsha + 2;//To skip Masei
                            }
                            else
                            {
                                ListOfParshios.Add(TorahPortion.Matot);
                                currentParsha++;
                            }
                            break;
                        case 51:
                            if (nextRhStartDay == 5 || nextRhStartDay == 7)
                            {
                                ListOfParshios.Add(TorahPortion.Nitzavim_Vayeilech);
                                currentParsha = currentParsha + 2;//To skip Vayeilech
                            }
                            else
                            {
                                ListOfParshios.Add(TorahPortion.Nitzavim);
                                currentParsha++;
                            }
                            break;
                        default:                            
                            ListOfParshios.Add((TorahPortion)currentParsha);
                            currentParsha++;
                            break;
                    }

                    //Move on to the following week
                    currentWeek++;
                }
            }

            return ListOfParshios;
        }

        /// <summary>
        /// Method that calculates if a week's parsha is postponed a week
        /// </summary>
        /// <param name="numOfShabboses">The number of Shabboses that have passed since Rosh Hashanah, inclusive.</param>
        /// <param name="rhStartDay">The day of the week that Rosh Hashanah starts, Sunday = 1, Monday = 2...</param>
        /// <param name="addedDays">The number of days added the Cheshvan and Kislev depending if the year is Choser, Keseder or Sholem</param>
        /// <param name="inIsrael">A boolean value indicating if the parshios are being calculated for Israel or not</param>
        /// <param name="isLeap">A boolean value indicating if it is a leap year or not</param>
        /// <returns>True if the parsha is pushed off a week, false if not</returns>
        private bool isShabbosPostponed(int numOfShabboses, int rhStartDay, int addedDays, bool inIsrael, bool isLeap)
        {
            int dayPesahcStarts = 30 * 2 + 29 * 4 + addedDays + 15 + (Convert.ToInt32(isLeap) * 30);
            int dayShovuosStarts = 30 * 3 + 29 * 5 + addedDays + 6 + (Convert.ToInt32(isLeap) * 30);
            int daysPast = (numOfShabboses - 1) * 7 + (8 - rhStartDay);//Full weeks plus the first partial week

            //If the Shabbos falls on RH, YK or Sukkos
            if (daysPast == 1 || daysPast == 9 || (daysPast > 14 && daysPast < 23) || (daysPast == 23 && !inIsrael))
            {
                return true;
            }

            //If the Shabbos falls on Pesach
            if ((daysPast >= dayPesahcStarts && daysPast < dayPesahcStarts + 7) || (daysPast == dayPesahcStarts + 8 && !inIsrael))
            {
                return true;
            }

            //If the Shabbos falls on Shovuos
            if (daysPast == dayShovuosStarts || (daysPast == dayShovuosStarts + 1 && !inIsrael))
            {
                return true;
            }

            return false; //If doesn't match any of the criteria above then return false.

        }

        /// <summary>
        /// An enum class that contains a list of all the possible parshiot, including joined parshiot
        /// </summary>
        public enum TorahPortion
        {
            Bereshit = 1,
            Noach,
            Lech_Lecha,
            Vayera,
            Chayei_Sara,
            Toldot,
            Vayetzei,
            Vayishlach,
            Vayeshev,
            Miketz,
            Vayigash,
            Vayechi,
            Shemot,
            Vaera,
            Bo,
            Beshalach,
            Yitro,
            Mishpatim,
            Terumah,
            Tetzaveh,
            Ki_Tisa,
            Vayakhel,
            Pekudei,
            Vayikra,
            Tzav,
            Shmini,
            Tazria,
            Metzora,
            Achrei_Mot,
            Kedoshim,
            Emor,
            Behar,
            Bechukotai,
            Bamidbar,
            Nasso,
            Behaalotcha,
            Shlach,
            Korach,
            Chukat,
            Balak,
            Pinchas,
            Matot,
            Masei,
            Devarim,
            Vaetchanan,
            Eikev,
            ReEh,
            Shoftim,
            Ki_Teitzei,
            Ki_Tavo,
            Nitzavim,
            Vayeilech,
            HaAzinu,
            Vezot_Haberakhah,
            Vayakhel_Pekudei,
            Tazria_Metzora,
            Achrei_Mot_Kedoshim,
            Behar_Bechukotai,
            Chukat_Balak,
            Matot_Masei,
            Nitzavim_Vayeilech
        }
    }
}
