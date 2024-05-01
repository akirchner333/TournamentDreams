namespace Tournament
{
    public interface ITournament<Player>
    {
        public int Round { get; }
        public int Turn { get; }
        public (Player player, WinRecord record)[] Players { get; }
        public (Player, Player) NextGame();
        public List<(Player, Player)> CurrentRound();
        public void GameResults(Result result);
        public bool Finished();
        public (Player, int, int)[] Rankings();
    }

    public enum Result
    {
        A_WINS,
        B_WINS,
        DRAW
    }

    public abstract class AbstractTournament<Player> : ITournament<Player>
    {
        public (Player player, WinRecord record)[] Players { get; set; }
        public int Round { get; set; } = 0;
        public int Turn { get; set; } = 0;
        public AbstractTournament(Player[] players)
        {
            Players = new (Player, WinRecord)[players.Length];
            for(var i = 0; i < Players.Length; i++)
            {
                Players[i] = (players[i], new WinRecord());
            }
        }

        public (Player, Player) NextGame()
        {
            var (a, b) = NextIndexes();
            return (Players[a].player, Players[b].player);
        }

        public virtual void GameResults(Result result)
        {
            var (a, b) = NextIndexes();

            UpdateRecords(result, a, b);
            IncrementTurn();
        }

        public void UpdateRecords(Result result, int a, int b)
        {
            switch (result)
            {
                case Result.A_WINS:
                    Players[a].record.Wins++;
                    Players[b].record.Losses++;
                    break;
                case Result.B_WINS:
                    Players[a].record.Losses++;
                    Players[b].record.Wins++;
                    break;
                case Result.DRAW:
                    Players[a].record.Draws++;
                    Players[b].record.Draws++;
                    break;
            }
        }

        public void IncrementTurn()
        {
            Turn++;
            if (Turn >= TurnsInRound())
            {
                Turn = 0;
                Round++;
                SetupRound();
            }
        }

        public (int, int) NextIndexes()
        {
            return TurnIndexes(Turn);
        }

        public virtual void SetupRound() { }

        // Gets the whole current round
        public abstract List<(Player, Player)> CurrentRound();
        public abstract (int, int) TurnIndexes(int turn);
        public abstract int TurnsInRound();
        public abstract bool Finished();

        public (Player, int, int)[] Rankings()
        {
            Array.Sort(Players, ComparePlayers);
            int lastIndex = 1;
            int lastScore = -1;
            return Players.Select((p, index) => {
                if(p.record.Scores != lastScore)
                    lastIndex = index + 1;
                lastScore = p.record.Scores;
                return (p.player, lastIndex, p.record.Scores);

            }).ToArray();
        }

        public static int ComparePlayers((Player player, WinRecord record) a, (Player player, WinRecord record) b)
        {
            return  b.record.Scores - a.record.Scores;
        }
    }
}