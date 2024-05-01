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

        // These two functions are the primary ones external applications would be interacting with

        // Returns the next pairing
        public (Player, Player) NextGame()
        {
            var (a, b) = NextIndexes();
            return (Players[a].player, Players[b].player);
        }

        // This is where you send the results of the current game.
        // Records the results and moves on to the next turn
        public virtual void GameResults(Result result)
        {
            var (a, b) = NextIndexes();

            UpdateRecords(result, a, b);
            IncrementTurn();
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

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

        // Run at the start of each round, does any setup and assignment required
        public virtual void SetupRound() { }

        // Gets the whole current round
        public abstract List<(Player, Player)> CurrentRound();
        public abstract (int, int) TurnIndexes(int turn);
        // How many turns in the current round?
        public abstract int TurnsInRound();
        // Has the tournament finished?
        public abstract bool Finished();

        // What is everyone's ranking?
        // Returns the player, their ranking (factoring in ties), and the score
        public virtual (Player, int, int)[] Rankings()
        {
            Array.Sort(Players, ComparePlayers);
            // If two players have the same final score, they get the same ranking
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