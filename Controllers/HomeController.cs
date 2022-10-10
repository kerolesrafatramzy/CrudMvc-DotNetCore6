using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_blog.Models;
using System.Diagnostics;

namespace Simple_blog.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.OrderByDescending(p => p.Id).ToListAsync();
            return View(posts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            return View("Details", post);
        }


    }
}