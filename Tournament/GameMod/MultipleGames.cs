using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.GameMod
{
    // A tournament extension where each round a player plays multiple rounds
    // Each round is recorded seperately, so it probably wouldn't well with elimination type tourneys
    public class MultipleGames<Player> : AbstractGameMod<Player>
    {
        public int Games { get; private set; } = 1;

        public override int Turn {
            get
            {
                return BaseTournament.Turn * Games + SubTurn;
            }
        }

        private int SubTurn = 0;
        private WinRecord _aRecord = new WinRecord();
        private WinRecord _bRecord = new WinRecord();

        public MultipleGames(AbstractTournament<Player> tournament, int games) : base(tournament)
        {
            Games = games;
        }

        public override (Player, Player) NextGame()
        {
            var (a, b) = BaseTournament.NextGame();
            if (SubTurn % 2 == 0)
                return (a, b);
            else
                return (b, a);
        }

        public override void GameResults(Result result)
        {
            var (a, b) = BaseTournament.NextIndexes();
            if(SubTurn % 2 == 0)
                BaseTournament.UpdateRecords(result, a, b);
            else
                BaseTournament.UpdateRecords(result, b, a);

            SubTurn++;
            if (SubTurn >= Games)
            {
                BaseTournament.IncrementTurn();
                _aRecord = new WinRecord();
                _bRecord = new WinRecord();
                SubTurn = 0;
            }
        }

        public Result SumResults()
        {
            var score = _aRecord.Scores - _bRecord.Scores;
            if (score == 0)
                return Result.DRAW;
            else if (score > 0)
                return Result.A_WINS;
            else
                return Result.B_WINS;
        }

        public override List<(Player, Player)> CurrentRound()
        {
            var list = new List<(Player, Player)>();
            BaseTournament.CurrentRound().ForEach(round =>
            {
                var (a, b) = round;
                for(var i = 0; i < Games; i++)
                {
                    list.Add(i % 2 == 0 ? (a, b) : (b, a));
                }
            });

            return list;
        }
    }
}
