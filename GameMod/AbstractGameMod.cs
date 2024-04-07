using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.GameMod
{
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
