using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Dynamic.Core;
using Simple_blog.Models;

namespace Simple_blog.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostssController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostssController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostApi()
        {

            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];


            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];


            IQueryable<Post> posts = _context.Posts.Where(m => string.IsNullOrEmpty(searchValue)
            ? true : (m.Title.Contains(searchValue) || m.Published_In.Contains(searchValue)));



            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                posts = posts.OrderBy(string.Concat(sortColumn, " ", sortColumnDirection));



            var data = await posts.Skip(skip).Take(pageSize).ToListAsync();

            var recordsTotal = posts.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }

    }
}
