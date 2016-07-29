using System;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}
