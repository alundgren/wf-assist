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

        private WfDatabase Db(string language)
        {
            return language == "sv"
                ? _swedishDb.Value 
                : null;
        }

        public IList<string> FindWords(string language, string pattern)
        {
            var db = Db(language);
            return db == null
                ? new List<string>()
                : db.FindWords(pattern);
        }

        public IList<string> FindWordsFiltered(string language, string pattern, string availableLetters)
        {
            var db = Db(language);
            return db == null 
                ? new List<string>() 
                : db.FindWords(pattern, availableLetters);
        }
    }
}