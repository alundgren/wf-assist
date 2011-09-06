using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WfAssist.Logic
{
    public interface IWordProvider
    {
        IList<Tuple<string, int>> FindWords(string language, string pattern);
        IList<Tuple<string, int>> FindWordsFiltered(string language, string pattern, string availableLetters);
    }
}
