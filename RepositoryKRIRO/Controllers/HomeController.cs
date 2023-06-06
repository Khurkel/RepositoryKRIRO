using Microsoft.AspNetCore.Mvc;
using RepositoryKRIRO.Models;
using System.Diagnostics;
using System.Text.Json;

namespace RepositoryKRIRO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpGet httpGet = new HttpGet("https://kriro.ru/news/");
            List<News> AllNews = httpGet.GetNewsAsync().Result;
            return View(AllNews);
        }

        public IActionResult Privacy()
        {
            List<Practice> AllPractice = new List<Practice>();
            HttpGet httpGet = new HttpGet(@"https://komiedu.ru/sistema-obrazovaniya-rk/obrazovanie/razvitie-obrazovaniya/luchshie-praktiki/rk/index.php?arrFilter_pf%5BEVENT_TYPE%5D=685&set_filter=Y&PAGEN_1=1");
            int num_page = httpGet.GetNumAsync().Result;
            for (int i = 1; i< num_page; i++)
            {
                httpGet = new HttpGet(@"https://komiedu.ru/sistema-obrazovaniya-rk/obrazovanie/razvitie-obrazovaniya/luchshie-praktiki/rk/index.php?arrFilter_pf%5BEVENT_TYPE%5D=685&set_filter=Y&PAGEN_1="+i.ToString());
                List<Practice> practices = httpGet.GetPraticeAsync().Result;
                AllPractice.AddRange(practices);
            }
           
            return View(AllPractice);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}