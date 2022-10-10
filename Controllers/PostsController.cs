using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Simple_blog.Models;
using Simple_blog.ViewModels;
using NToastNotify;

namespace Simple_blog.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly List<string> _allowedExtentions = new List<string> { ".jpg", ".png" };
        private readonly long _maxAllowedImageSize = 1048576;


        public PostsController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.OrderByDescending(p => p.Id).ToListAsync();
            var recordsTotal = posts.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = posts };

            return View(posts);
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = new PostFormViewModel
            {
                Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync()
            };
            return View("PostForm",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                return View("PostForm", model);
            }

            var files = Request.Form.Files; 

            if (!files.Any())
            {
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Image", "Please select an image!");
                return View("PostForm", model);
            }

            var image = files.FirstOrDefault();

            if (!_allowedExtentions.Contains(Path.GetExtension(image.FileName).ToLower()))
            {
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Image", "Only .PNG, .JPG images are allowed!");
                return View("PostForm", model);
            }


            if (image.Length > _maxAllowedImageSize)
            {
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Image", "image cannot be more than 1 Mb!");
                return View("PostForm", model);
            }

            using var dataStream = new MemoryStream();

            await image.CopyToAsync(dataStream);

            var posts = new Post
            {
                Title = model.Title,
                CategoryId = model.CategoryId,
                Content = model.Content,
                Rate = model.Rate,
                Image = dataStream.ToArray(),
                Published_In = model.Published_In
            };

            _context.Posts.Add(posts);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Post created successfully!");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            var viewModel = new PostFormViewModel
            {
                Id = post.Id,
                CategoryId=post.CategoryId,
                Title = post.Title,
                Content = post.Content,
                Rate=post.Rate,
                Image=post.Image,
                Published_In = post.Published_In,
                Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync()
            };

            return View("PostForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                return View("PostForm", model);
            }

            var post = await _context.Posts.FindAsync(model.Id);

            if (post == null)
                return NotFound();


            var files = Request.Form.Files;

            if (files.Any())
            {
                var image = files.FirstOrDefault();

                using var dataStream = new MemoryStream();

                await image.CopyToAsync(dataStream);

                model.Image = dataStream.ToArray();

                if (!_allowedExtentions.Contains(Path.GetExtension(image.FileName).ToLower()))
                {
                    model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Image", "Only .PNG, .JPG images are allowed!");
                    return View("PostForm", model);
                }


                if (image.Length > _maxAllowedImageSize)
                {
                    model.Categories = await _context.Categories.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Image", "image cannot be more than 1 Mb!");
                    return View("PostForm", model);
                }

                post.Image = model.Image;
            }

            



            post.CategoryId = model.CategoryId;
            post.Title = model.Title;
            post.Content = model.Content;
            post.Rate = model.Rate;
            post.Published_In = model.Published_In;

            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Post updated successfully!");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            _context.Posts.Remove(post);
            _context.SaveChanges();
            //_toastNotification.AddSuccessToastMessage("Post deleted successfully");
            return RedirectToAction("Index");
        }

    }
}
