using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTest
{
    [TestClass]
    public class SwissFideTest
    {
        [TestMethod]
        public void SideReqMatchTest()
        {
            Assert.IsTrue(SideReq.WHITE_MAND.Matching(SideReq.BLACK_MAND));
            Assert.IsTrue(SideReq.WHITE_MAND.Matching(SideReq.WHITE_PREF));
            Assert.IsTrue(SideReq.WHITE_MAND.Matching(SideReq.BLACK_PREF));
            Assert.IsTrue(SideReq.BLACK_MAND.Matching(SideReq.WHITE_MAND));
            Assert.IsTrue(SideReq.BLACK_MAND.Matching(SideReq.WHITE_PREF));
            Assert.IsTrue(SideReq.BLACK_MAND.Matching(SideReq.BLACK_PREF));

            Assert.IsFalse(SideReq.BLACK_MAND.Matching(SideReq.BLACK_MAND));
            Assert.IsFalse(SideReq.WHITE_MAND.Matching(SideReq.WHITE_MAND));

            foreach (var req in Enum.GetValues(typeof(SideReq)))
            {
                Assert.IsTrue(SideReq.WHITE_PREF.Matching((SideReq)req));
                Assert.IsTrue(SideReq.BLACK_PREF.Matching((SideReq)req));
            }
        }

        [TestMethod]
        public void NextSideTest()
        {
            Assert.AreEqual(new PlayHistory(0).NextSide(), SideReq.WHITE_PREF);

            var playTwice = new PlayHistory(0)
            {
                WhitePlays = 2,
                BlackPlays = 2,
                PrevPlay = new bool[] { true, true }
            };
            Assert.AreEqual(playTwice.NextSide(), SideReq.BLACK_MAND);

            var colorDiff = new PlayHistory(0)
            {
                WhitePlays = 1,
                BlackPlays = 3,
                PrevPlay = new bool[] { true, false }
            };
            Assert.AreEqual(colorDiff.NextSide(), SideReq.WHITE_MAND);
        }

        [TestMethod]
        public void SortTest()
        {
            var swiss = new SwissFide<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
            var ranking = swiss.CurrentRound();
            Assert.AreEqual(4, ranking.Count);
            Assert.AreEqual((1, 0), ranking[0]);
            Assert.AreEqual((3, 2), ranking[1]);
            Assert.AreEqual((5, 4), ranking[2]);
            Assert.AreEqual((7, 6), ranking[3]);

            swiss.GameResults(Result.A_WINS);
            swiss.GameResults(Result.B_WINS);
            swiss.GameResults(Result.B_WINS);
            swiss.GameResults(Result.A_WINS);

            ranking = swiss.CurrentRound();
            Assert.AreEqual((3, 0), ranking[0]);
            Assert.AreEqual((6, 5), ranking[1]);
            Assert.AreEqual((2, 1), ranking[2]);
            Assert.AreEqual((7, 4), ranking[3]);
        }

        [TestMethod]
        public void ByeTest()
        {
            //var swiss = new Swiss<int>(new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            //var ranking = swiss.CurrentRound();
            //Assert.AreEqual(4, ranking.Count);
            //Assert.AreEqual((3, 2), ranking[0]);
            //Assert.AreEqual((5, 4), ranking[1]);
            //Assert.AreEqual((7, 6), ranking[2]);
            //Assert.AreEqual((0, 8), ranking[3]);

            //// Winners - 3, 4, 6, 0 (and 1 by default)
            //swiss.GameResults(Result.A_WINS);
            //swiss.GameResults(Result.B_WINS);
            //swiss.GameResults(Result.B_WINS);
            //swiss.GameResults(Result.A_WINS);

            //ranking = swiss.CurrentRound();
            //Console.WriteLine(String.Join(' ', ranking));
            //Assert.AreEqual((0, 1), ranking[0]);
            //Assert.AreEqual((6, 7), ranking[1]);
            //Assert.AreEqual((2, 3), ranking[2]);
            //Assert.AreEqual((5, 8), ranking[3]);
        }
    }
}
