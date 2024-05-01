using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Rankings
{
    public static class Elo
    {
        // What rating does the a completely new player get?
        // Best I can tell, 1400 is the minimum rating a new player receive (FIDE regulation 7.1.4, effective March 1st 2024)
        // https://handbook.fide.com/chapter/B022024
        // Cannot figure out how an unrated player's initial ranking is determined
        public const double NEW_RATING = 1400;

        public static (double, double) NewRatings(double aScore, double bScore, Result result, double k = 32)
        {
            var aChance = Probability(aScore, bScore);
            var delta = ScoreChange(ResultValue(result), aChance, k);
            return (aScore + delta, bScore - delta);
        }

        public static double Probability(double aScore, double bScore)
        {
            return 1d / (1d + Math.Pow(10d, (bScore - aScore) / 400d));
        }

        public static double ResultValue(Result result)
        {
            return result switch
            {
                Result.A_WINS => 1.0,
                Result.B_WINS => 0.0,
                Result.DRAW => 0.5,
                _ => throw new ArgumentOutOfRangeException(nameof(result), "Invalid result entered")
            };
        }

        public static double ScoreChange(double result, double chance, double k)
        {
            return k * (result - chance);
        }
    }
}
