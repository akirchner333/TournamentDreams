using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament
{
    // A class for making graphs and assigning them maximum matches
    // This is useful for assigning tournaments that have complicated rules about who can play who (e.g. swiss)
    // Connect each player to all the players they can legally play and then use FindMatching to assign them
    public class Graph
    {
        public int VertexCount;
        public int[] Matches;
        public List<int>[] Connections;

        public Graph(int n)
        {
            VertexCount = n;
            Connections = new List<int>[n];
            Matches = new int[n];
            for (var i = 0; i < n; i++)
            {
                Connections[i] = new List<int>();
                Matches[i] = -1;
            }
        }
        public void AddConnection(int a, int b)
        {
            if (Connections[a].Contains(b))
                return;

            Connections[a].Add(b);
            Connections[b].Add(a);
        }

        public void Match(int a, int b)
        {
            Matches[a] = b;
            Matches[b] = a;
        }

        public List<(int, int)> MatchList()
        {
            var matches = new List<(int, int)>();
            for (var i = 0; i < VertexCount; i++)
            {
                if (Matches[i] == -1 || Matches[i] < i)
                    continue;
                matches.Add((i, Matches[i]));
            }
            return matches;
        }

        public List<int> Exposed()
        {
            var exposed = new List<int>();
            for (var i = 0; i < VertexCount; i++)
            {
                if (Matches[i] == -1)
                    exposed.Add(i);
            }
            return exposed;
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Blossom algorithm ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Code based on https://medium.com/@ckildalbrandt/demystifying-edmonds-blossom-algorithm-with-python-code-6353eb043311
        // converted into c#, cleaned up to make more readable, and then placed inside this object
        public List<(int, int)> FindMatches()
        {
            var parents = new int[VertexCount];

            for (var i = 0; i < VertexCount; i++)
            {
                if (Matches[i] == -1)
                {
                    var v = FindPath(parents, i);
                    while (v != -1)
                    {
                        var pv = parents[v];
                        var ppv = Matches[pv];
                        Match(v, pv);
                        v = ppv;
                    }
                }
            }

            return MatchList();
        }

        public int FindPath(int[] parents, int root)
        {
            var used = new bool[VertexCount];
            used[root] = true;
            var queue = new Queue<int>();
            queue.Enqueue(root);
            var baseArr = new int[VertexCount];
            for (var i = 0; i < VertexCount; i++)
            {
                parents[i] = -1;
                baseArr[i] = i;
            }

            while (queue.Count > 0)
            {
                var a = queue.Dequeue();
                foreach (var b in Connections[a])
                {
                    if (baseArr[a] == baseArr[b] || Matches[a] == b)
                        continue;

                    if (b == root || (Matches[b] != -1 && parents[Matches[b]] != -1))
                    {
                        var curbase = LowestCommonAncestor(baseArr, parents, a, b);
                        var blossom = new bool[VertexCount];
                        MarkPath(baseArr, blossom, parents, a, curbase, b);
                        MarkPath(baseArr, blossom, parents, b, curbase, a);
                        for (var i = 0; i < VertexCount; i++)
                        {
                            if (blossom[baseArr[i]])
                            {
                                baseArr[i] = curbase;
                                if (!used[i])
                                {
                                    used[i] = true;
                                    queue.Enqueue(i);
                                }
                            }
                        }
                    }
                    else if (parents[b] == -1)
                    {
                        parents[b] = a;
                        if (Matches[b] == -1)
                        {
                            return b;
                        }
                        var bMatch = Matches[b];
                        used[bMatch] = true;
                        queue.Enqueue(bMatch);
                    }
                }
            }
            return -1;
        }

        // int[] baseArr: I /think/ this is supposed to be a list of all the items currently in the graph
        // int[] parents: the parent of node i is held in parents[i]
        // int a: start point
        // int b: end point
        public int LowestCommonAncestor(int[] baseArr, int[] parents, int a, int b)
        {
            var used = new bool[VertexCount];
            // Travels up the tree to the root, marking all the places we've been
            while (true)
            {
                a = baseArr[a];
                used[a] = true;
                // We know match[a] == -1 is the root cause augmented paths always start with an exposed node
                if (Matches[a] == -1)
                    break;
                a = parents[Matches[a]];
            }

            // travels up the tree until we hit a node we've visited before, that's the lca
            while (true)
            {
                b = baseArr[b];
                if (used[b])
                {
                    return b;
                }
                b = parents[Matches[b]];
            }
        }

        // I think this function builds the blossom - marking all the nodes which are inside a blossom
        public void MarkPath(int[] baseArr, bool[] blossom, int[] parents, int v, int b, int child)
        {
            while (baseArr[v] != b)
            {
                blossom[baseArr[v]] = true;
                blossom[baseArr[Matches[v]]] = true;
                parents[v] = child;
                child = Matches[v];
                v = parents[Matches[v]];
            }
        }
    }
}
