using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WfAssist.Logic;

namespace WfAssist.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWordProvider _wordProvider;

        public HomeController(IWordProvider wordProvider)
        {
            _wordProvider = wordProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        private IList<string> ToScoredViewModel(IList<Tuple<string, int>> wordsWithScores)
        {
            return wordsWithScores
                .Select(x => string.Format("({0}){1}", x.Item2, x.Item1))
                .ToList();
        }

        public JsonResult Words(string language, string pattern)
        {
            return Json(ToScoredViewModel(_wordProvider.FindWords(language, pattern)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult WordsFiltered(string language, string pattern, string availableLetters)
        {
            return Json(ToScoredViewModel(_wordProvider.FindWordsFiltered(language, pattern, availableLetters)), JsonRequestBehavior.AllowGet);
        }
    }
}
