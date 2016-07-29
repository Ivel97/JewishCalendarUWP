using System;

namespace JewishCalendarUWP.Parsha
{
    /// <summary>
    ///     Defines a span of pesukim. Used for defining Aliyos
    /// </summary>
    public class PosukSpan
    {
        /// <summary>
        ///     The first posuk
        /// </summary>
        public PosukLocation StartPosuk { get; set; }

        /// <summary>
        ///     The last posuk
        /// </summary>
        public PosukLocation EndPosuk { get; set; }
    }
}
