using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WfAssist.Logic
{
    public interface IWordProvider
    {
        IList<string> FindWords(string language, string pattern);
        IList<string> FindWordsFiltered(string language, string pattern, string availableLetters);
    }
}
