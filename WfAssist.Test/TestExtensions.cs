using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WfAssist.Test
{
    public static class TestExtensions
    {
        public static void AssertEquals(this IList<string> source, params string[] words)
        {
            var expected = String.Join(", ", words);
            var actual = String.Join(", ", source.ToArray());
            Assert.AreEqual(expected, actual);
        } 
    }
}
