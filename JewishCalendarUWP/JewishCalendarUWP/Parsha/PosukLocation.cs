using System;

namespace JewishCalendarUWP.Parsha
{
    /// <summary>
    ///     A class describing a location in תנ"ך
    /// </summary>
    public struct PosukLocation
    {
        /// <summary>
        ///     The sefer that contains the posuk. E.G. Bereishis or Yehoshua
        /// </summary>
        public string Sefer { get; private set; }

        /// <summary>
        ///     The perek that contains the posuk
        /// </summary>
        public int Perek { get; private set; }

        /// <summary>
        ///     The number of the posuk
        /// </summary>
        public int Posuk { get; private set; }

        public PosukLocation(string Sefer, int Perek, int Posuk)
        {
            this.Sefer = Sefer;
            this.Perek = Perek;
            this.Posuk = Posuk;
        }
    }
}
