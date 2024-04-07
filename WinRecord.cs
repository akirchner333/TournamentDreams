using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament
{
    public struct WinRecord
    {
        public WinRecord() { }
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int Scores
        {
            get { return Wins * 2 + Draws; }
        }
        public static WinRecord operator +(WinRecord a, WinRecord b)
        {
            return new WinRecord()
            {
                Wins = a.Wins + b.Wins,
                Losses = a.Losses + b.Losses,
                Draws = a.Draws + b.Draws,
            };
        }
    }
}
