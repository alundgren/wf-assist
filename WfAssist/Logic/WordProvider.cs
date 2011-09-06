using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WfAssist.Logic
{
    public class WordProvider : IWordProvider
    {
        private readonly Lazy<WfDatabase> _swedishDb = new Lazy<WfDatabase>(
            EmbeddedDictionaries.LoadSwedishDatabase,
            LazyThreadSafetyMode.ExecutionAndPublication);
        private readonly Lazy<IDictionary<char, int>> _swedishScores = new Lazy<IDictionary<char, int>>(
            EmbeddedDictionaries.LoadSwedishPoints,
            LazyThreadSafetyMode.ExecutionAndPublication); 

        private WfDatabase Db(string language)
        {
            return language == "sv"
                ? _swedishDb.Value 
                : null;
        }

        public IList<Tuple<string, int>> FindWords(string language, string pattern)
        {
            var db = Db(language);
            return
                SortLimitAndScoreResult(
                db == null
                    ? new List<string>()
                    : db.FindWords(pattern));
        }

        public IList<Tuple<string, int>> FindWordsFiltered(string language, string pattern, string availableLetters)
        {
            var db = Db(language);
            return SortLimitAndScoreResult(
                db == null 
                    ? new List<string>() 
                    : db.FindWords(pattern, availableLetters));
        }

        private IList<Tuple<string, int>> SortLimitAndScoreResult(IList<string> input)
        {
            return input
                .Select(x => Tuple.Create(x, GetWordScore(x)))
                .OrderByDescending(x => x.Item2)
                .Take(30)
                .ToList();
        }

        private int GetWordScore(string word)
        {
            return word
                .Select(c => _swedishScores.Value.ContainsKey(c) ? _swedishScores.Value[c] : 0)
                .Sum();
        }
    }
}