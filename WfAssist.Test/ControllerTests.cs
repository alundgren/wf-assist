using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WfAssist.Controllers;
using WfAssist.Logic;

namespace WfAssist.Test
{
    //TODO: Do these tests really pull their own weight?
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void IndexDoesNotExplode()
        {
            var words = new Mock<IWordProvider>();
            var controller = new HomeController(words.Object);

            controller.Index();
        }

        [TestMethod]
        public void WordsCallsTheProvider()
        {
            var words = new Mock<IWordProvider>();
            words
                .Setup(x => x.FindWords("sv", "päron"))
                .Returns(new[] {"päron"}.ToList());
            var controller = new HomeController(words.Object);

            controller.Words("sv", "päron");

            words.Verify(x => x.FindWords("sv", "päron"), Times.Once());
        }

        [TestMethod]
        public void WordsFilteredCallsTheProvider()
        {
            var words = new Mock<IWordProvider>();
            words
                .Setup(x => x.FindWordsFiltered("sv", "päron", "päron"))
                .Returns(new[] { "päron" }.ToList());
            var controller = new HomeController(words.Object);

            controller.WordsFiltered("sv", "päron", "päron");

            words.Verify(x => x.FindWordsFiltered("sv", "päron", "päron"), Times.Once());
        }
    }
}
