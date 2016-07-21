using System;
using System.Collections.Generic;

namespace JewishCalendarUWP.Parsha
{
    /// <summary>
    ///     A class that contains data about a particular Torah reading.
    /// </summary>
    public class TorahReading
    {
        /// <summary>
        ///     The parsha that is being read that week. If it is a Yom Tov reading then it returns null.
        /// </summary>
        public Enum.TorahPortion? TorahPortion { get; set; }

        /// <summary>
        ///     A list of the Aliyos being read. Doesn't include Maftir or Haftorah.
        /// </summary>
        public List<PosukSpan> Aliyos { get; set; }

        /// <summary>
        ///     The Maftir being read. If there is no Maftir then it returns null.
        /// </summary>
        public PosukSpan Maftir { get; set; }

        /// <summary>
        ///     The Haftorah being read. If there is no Haftorah then it returns null.
        /// </summary>
        public PosukSpan Haftorah { get; set; }
    }
}
