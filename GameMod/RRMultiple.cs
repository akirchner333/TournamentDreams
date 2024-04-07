using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Series;

namespace Tournament.GameMod
{
    public class RRMultiple<Player> : ITournament<Player>
    {
        public int Games { get; private set; }
        public RoundRobin<Player> RoundRobin;
        public RepeatGames<Player> RepeatGames;
        public int Round
        {
            get
            {
                return RoundRobin.Round;
            }
        }
        public virtual int Turn
        {
            get
            {
                return RoundRobin.Turn * Games + RepeatGames.Turn;
            }
        }
        public (Player, WinRecord)[] Players
        {
            get
            {
                return RoundRobin.Players;
            }
        }
        public RRMultiple(Player[] players, int games)
        {
            Games = games;
            RoundRobin = new RoundRobin<Player>(players);
            RepeatGames = new RepeatGames<Player>(RoundRobin.NextGame(), games);
        }

        public (Player, Player) NextGame()
        {                
            return RepeatGames.NextGame();
        }

        public void GameResults(Result result)
        {   
            RepeatGames.GameResults(result);
            if (RepeatGames.Finished())
            {
                // We're storing the canonical records in RoundRobin and only updating them after each RepeatGame round ends
                // Might be necessary to change it later
                // Also might be useful to add some methods to access players and their information more easily in the future
                var (a, b) = RoundRobin.NextIndexes();
                RoundRobin.Players[a].record += RepeatGames.Players[0].record;
                RoundRobin.Players[b].record += RepeatGames.Players[1].record;

                RoundRobin.IncrementTurn();
                RepeatGames = new RepeatGames<Player>(RoundRobin.NextGame(), Games);
            }
        }

        public bool Finished()
        {
            return RoundRobin.Finished();
        }

        public List<(Player, Player)> CurrentRound()
        {
            // Something more complicated should go here, shrug
            return RoundRobin.CurrentRound();
        }

        public (Player, int, int)[] Rankings()
        {
            return RoundRobin.Rankings();
        }
    }
}
