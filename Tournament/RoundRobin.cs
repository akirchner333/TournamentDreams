using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament
{
    public class RoundRobin<Player> : AbstractTournament<Player>
    {
        public List<(int, int)> ThisRound = new List<(int, int)>();
        public RoundRobin(Player[] players) : base(players)
        {
            SetupRound(); 
        }

        public override (int, int) TurnIndexes(int turn)
        {
            return ThisRound[turn];
        }

        public override int TurnsInRound()
        {
            return Players.Length / 2;
        }

        public override List<(Player, Player)> CurrentRound()
        {
            return ThisRound.Select(indexes =>
            {
                var (a, b) = indexes;
                return (Players[a].player, Players[b].player);
            }).ToList();
        }

        // https://en.wikipedia.org/wiki/Round-robin_tournament#Circle_method
        public override void SetupRound()
        {
            ThisRound.Clear();
            var wheel = MakeWheel().ToArray();
            for (var i = 0; i < TurnsInRound() + 1; i++)
            {
                var (a, b) = PairFromWheel(wheel, i);
                // Skip if it's up against the placeholder
                if (a == -1 || b == -1)
                {
                    continue;
                }
                ThisRound.Add((a, b));
            }
        }

        public override bool Finished()
        {
            return Round >= (Players.Length + ((Players.Length % 2 == 0) ? - 1 : 0));
        }

        public Queue<int> MakeWheel()
        {
            // Make a list of all the indexes except the last one
            var wheel = new Queue<int>();
            for (var i = 0; i < Players.Length - 1; i++)
            {
                wheel.Enqueue(i);
            }

            // If there's an odd number of players, add an extra placeholder player
            if (Players.Length % 2 == 1)
                wheel.Enqueue(-1);

            // Rotate the wheel Round times
            for (var i = 0; i < Round; i++)
            {
                var holder = wheel.Dequeue();
                wheel.Enqueue(holder);
            }

            return wheel;
        }

        public (int, int) PairFromWheel(int[] wheel, int turn)
        {
            if (turn == 0)
                return (Players.Length - 1, wheel[^1]);
            return (wheel[turn - 1], wheel[^(turn + 1)]);
        }
    }
}
