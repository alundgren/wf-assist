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

        public JsonResult Words(string language, string pattern)
        {
            return Json(_wordProvider.FindWords(language, pattern), JsonRequestBehavior.AllowGet);
        }

        public JsonResult WordsFiltered(string language, string pattern, string availableLetters)
        {
            return Json(_wordProvider.FindWordsFiltered(language, pattern, availableLetters), JsonRequestBehavior.AllowGet);
        }
    }
}
