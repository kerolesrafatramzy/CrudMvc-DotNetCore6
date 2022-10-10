using Microsoft.AspNetCore.Mvc;

namespace Simple_blog.Controllers
{
    public class Posts2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
