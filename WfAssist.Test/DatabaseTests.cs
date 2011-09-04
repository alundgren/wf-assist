using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WfAssist.Logic;
using System.Globalization;

namespace WfAssist.Test
{
    [TestClass]
    public class DatabaseTests
    {
        private static WfDatabase MockDb(params string[] words)
        {
            return new WfDatabase(words.ToList(), CultureInfo.GetCultureInfo("sv-SE"));
        }

        [TestMethod]
        public void SearchingForExactWordFindsOnlyThatWord()
        {
            var db = MockDb("cat", "cats", "scat");

            db.FindWords("cat").AssertEquals("cat");
        }

        [TestMethod]
        public void EmptyStringMatchesNothing()
        {
            var db = MockDb("test");

            db.FindWords("").AssertEquals();
        }

        [TestMethod]
        public void QuestionMarkMatchesExactlyOneChar()
        {
            var db = MockDb("bar", "bard", "bards");

            db.FindWords("bar?").AssertEquals("bard");
        }

        [TestMethod]
        public void StarMatchesSuffix()
        {
            var db = MockDb("bar", "bard", "bards");

            db.FindWords("bar*").AssertEquals("bar", "bard", "bards");
        }

        [TestMethod]
        public void InnerQuestionMarkAcceptesAnyChar()
        {
            var db = MockDb("abc", "bdc", "aec", "dabcd", "abce", "eabc");

            db.FindWords("a?c").AssertEquals("abc", "aec");
        }

        [TestMethod]
        public void QuestionMarkRespectsAvailableLetters()
        {
            var db = MockDb("cat");

            db.FindWords("c?t", "b").AssertEquals();
            db.FindWords("c?t", "a").AssertEquals("cat");
        }

        [TestMethod]
        public void StarRespectsAvailableLetters()
        {
            var db = MockDb("bards");

            db.FindWords("b*ds", "r").AssertEquals();
            db.FindWords("b*ds", "a").AssertEquals();
            db.FindWords("b*ds", "ar").AssertEquals("bards");
        }

        [TestMethod]
        public void QuestionMarkRespectsAvailableLetterCount()
        {
            var db = MockDb("axiomatic");

            db.FindWords("ax?omat?c", "i").AssertEquals();
            db.FindWords("ax?omat?c", "ii").AssertEquals("axiomatic");
        }

        [TestMethod]
        public void StarRespectsAvailableLetterCount1()
        {
            var db = MockDb("chambermaid");

            db.FindWords("cha*er*id", "abm").AssertEquals();
        }

        [TestMethod]
        public void StarRespectsAvailableLetterCount2()
        {
            var db = MockDb("chambermaid");

            db.FindWords("cha*er*id", "abmm").AssertEquals("chambermaid");
        }

        [TestMethod]
        public void MaxHitCountAndOrderRespected()
        {
            var words = Enumerable
                .Range(0, 1000)
                .Select(i => new string('a', i))
                .OrderBy(x => Guid.NewGuid()) //Will sort of randomize the array (unlikely to pass a true randomness test but close enough for this purpose)
                .ToArray();
            
            var db = MockDb(words);

            db.FindWords("*").AssertEquals(words.OrderBy(x => x.Length).Take(30).ToArray());
        }
    }
}
