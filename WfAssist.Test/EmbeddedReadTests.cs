using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WfAssist.Logic;

namespace WfAssist.Test
{
    [TestClass]
    public class EmbeddedReadTests
    {
        [TestMethod]
        public void ReadSwedishDatabase()
        {
            var db = EmbeddedDictionaries.LoadSwedishDatabase();
            Assert.AreEqual(416601, db.Words.Count);
            db.FindWords("päron").AssertEquals("päron");
        }
    }
}
