using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WfAssist.Logic
{
    public class WfDatabase
    {
        private readonly IList<string> _words;
        private readonly CultureInfo _culture;

        /// <summary>
        /// NOTE: Words are assumed to be all lowercase and there is assumed to
        ///       be no duplicates.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="culture"></param>
        public WfDatabase(IList<string> words, CultureInfo culture)
        {
            _words = words;
            _culture = culture;
        }

        public IList<string> Words
        {
            get { return _words; }
        }
        /// <summary>
        /// Pattern syntax:
        /// *: Any letter 0 or more times
        /// ?: Exactly one of any letter
        /// 
        /// Examples:
        /// Lets say we have this database of words:
        /// [hamster, hamsters, hamstersadness]
        /// 
        /// ?amster -> [hamster]
        /// ?hamster? -> [hamsters]
        /// ?hamster* -> [hamster, hamsters, hamstersadness]
        /// ?hamsters* -> [hamsters, hamstersadness]
        /// 
        /// </summary>
        /// <param name="pattern">See summary for pattern syntax</param>
        /// <param name="availableLetters">available letters (the number of times each letter occurs will be respecred)</param>
        /// <returns>A list of at most 100 words matching the query, ordered from shortest to longest</returns>
        public IList<string> FindWords(string pattern, string availableLetters = null)
        {
            var letters = (availableLetters ?? "")
                .Trim()
                .ToLower(_culture)
                .ToCharArray();

            //First pass. Filter using regex
            var letterClass = 
                letters.Length == 0
                    ? "."
                    : String.Format("[{0}]",
                                    String.Concat(letters
                                    .Distinct()
                                    .Select(x => x.ToString())
                                    .ToArray()));
            
            var actualPattern = pattern.ToLower(_culture).Replace("?", letterClass);
            actualPattern = actualPattern.Replace("*", letterClass + "*");
            var r = new Regex(string.Format(@"^{0}$", actualPattern));
            var passingWords = _words.Where(x => r.IsMatch(x));

            //Second pass, letter count
            if (letters.Length > 0)
            {
                var letterCounts = letters
                    .Concat(pattern.ToLower(_culture))
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                passingWords = passingWords
                    .Where(x => RespectsLetterCounts(x, letterCounts)); 
            }

            //Sort and limit count
            return passingWords
                .OrderBy(x => x.Length)
                .Take(100)
                .ToList();
        }

        private bool RespectsLetterCounts(string word, IDictionary<char, int> letterCounts)
        {
            var wordLetterCounts = word
                 .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            return wordLetterCounts
                .Where(kv => 
                    ! (letterCounts.ContainsKey(kv.Key) 
                    && letterCounts[kv.Key] >= kv.Value))
                .Count() == 0;
        }
    }
}