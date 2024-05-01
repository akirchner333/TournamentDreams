using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.GameMod
{
    // A game mod is an alteration to an existing tournament type
    // For example, if you want a single elimination but each match is best of 3
    // BaseTournament would be a single elimination tournament and the mod would be best of 3
    // There's no reason you couldn't put a game mod inside a game mod for that matter
    public abstract class AbstractGameMod<Player> : ITournament<Player>
    {
        public AbstractTournament<Player> BaseTournament { get; private set; }

        public int Round
        {
            get
            {
                return BaseTournament.Round;
            }
        }
        public virtual int Turn
        {
            get
            {
                return BaseTournament.Turn;
            }
        }

        public (Player player, WinRecord record)[] Players
        {
            get
            {
                return BaseTournament.Players;
            }
        }

        public AbstractGameMod(AbstractTournament<Player> tournament)
        {
            BaseTournament = tournament;
        }

        public abstract (Player, Player) NextGame();
        public abstract void GameResults(Result result);

        public abstract List<(Player, Player)> CurrentRound();

        public bool Finished()
        {
            return BaseTournament.Finished();
        }

        public (Player, int, int)[] Rankings()
        {
            return BaseTournament.Rankings();
        }
    }
}
