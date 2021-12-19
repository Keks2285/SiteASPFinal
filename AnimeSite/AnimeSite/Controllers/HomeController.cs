using AnimeSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AnimeSite.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _app;
        private ApplicationContext db;
        int idOfUser;
        public HomeController(ApplicationContext context, IWebHostEnvironment app)
        {
            db = context;
            _app = app;
        }

        //public IActionResult AddFiles()
        //{
        //    return View(db.Files.ToList());
        //}

        [HttpPost]
        public async Task<IActionResult> AddFiles(IFormFile file)
        {
            if (file != null)
            {

                string path = "/files/" + file.FileName;
                using (FileStream fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                FileModel fileModel = new FileModel
                {
                    Name = file.FileName,
                    Path = path
                };
                // db.Files.Add(fileModel);
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == HttpContext.Session.GetInt32("UserId"));
                user.PhotoLink = path;
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Profile");


            }
            return RedirectToAction("Profile");
        }
    


        //////////////////////////////////////////
        public IActionResult Authorization()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(User a)
        {
            User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Login == a.Login && predicate.Password == a.Password);
            if (user != null)
            {
               // idOfUser = user.Id;
                if (user.Admin == true)
                {
                    return RedirectToAction("AdminUserPanel");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Profile");
                }


            }
            return RedirectToAction("Registration");
        }
        ///////////////////////////////////////////
        public IActionResult Index()
        {
            return View();
        }
        ///////////////////////////////////////////

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateByAdmin(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("AdminUserpanel");
        }
        [HttpPost]
        public async Task<IActionResult> Create (User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Authorization");
        }
        public IActionResult Registration()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        // [ActionName("DeletePostByAdmin")]
        public async Task<IActionResult> DeletePostByAdmin(int? id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (post != null)
                {
                    return View(post);
                }
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("DeletePostByAdmin")]
        public async Task<IActionResult> ConfirmDeletePostByAdmin(int? id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (post != null)
                {
                    db.Posts.Remove(post);
                    await db.SaveChangesAsync();
                    return RedirectToAction("AdminPostPanel");
                }
            }
            return NotFound();
        }

        [HttpGet]
        [ActionName("Delete")] //название действия к оторому обращаемся в представлении
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    return View(user); //что и как это
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("AdminUserPanel");//переходим к действию Index
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AdminEditPost(Post post)
        {
            db.Posts.Update(post);
            await db.SaveChangesAsync();
            return RedirectToAction("AdminPostPanel");
        }

        public async Task<IActionResult> AdminEditPost(int? id)
        {
            Post post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> PostEditByUser(Post post)
        {
            db.Posts.Update(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> PostEditByUser(int? id)
        {
            Post post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
            
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> EditByUser(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Profile");
        }
        
        public async Task<IActionResult> EditByUser()
        {

             User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == HttpContext.Session.GetInt32("UserId"));
                
                    return View(user);                       
            //User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == idOfUser); //idOfUser
            //db.Users.Update(user);
            //await db.SaveChangesAsync();
            //return View("Profile");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    return View(user);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("AdminUserPanel");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    return View(user);
                }
            }
            return NotFound();

        }
        /////////////////////////
        public async Task<IActionResult> ViewPosts(int? id, int page = 1)
        {
            IQueryable<Post> posts = db.Posts;
            if (id != null && id > 0)
            {
                posts = posts.Where(p => p.UserId == id);
            }
            int pagesize = 5;
            var count = await posts.CountAsync();
            var item = await posts.Skip((page - 1) * pagesize).Take(pagesize).ToArrayAsync();
            IndexViewModel viewModel = new IndexViewModel
            {
                FilterPostViewModel = new FilterPostViewModel(id),
                PageViewModel = new PageViewModel(count, page, pagesize),                
                Posts = item
            };
            return View(viewModel);
        }
            public async Task<IActionResult> AdminPostPanel(int? id, int page = 1, SortState sortorder = SortState.IdAsc)
        {
           // ViewBag.Id = HttpContext.Session.GetInt32("UserId");
            IQueryable<Post> posts = db.Posts;
            if (id != null && id > 0)
            {
                posts = posts.Where(p => p.Id == id);
            }
            switch (sortorder)
            {
                case SortState.IdAsc:
                    {
                        posts = posts.OrderBy(p => p.Id);
                        break;
                    }
                case SortState.IdDesc:
                    {
                        posts = posts.OrderByDescending(p => p.Id);
                        break;
                    }
            }
            int pagesize = 5;
            var count = await posts.CountAsync();
            var item = await posts.Skip((page - 1) * pagesize).Take(pagesize).ToArrayAsync();
            IndexViewModel viewModel = new IndexViewModel
            {
                FilterPostViewModel = new FilterPostViewModel(id),
                PageViewModel = new PageViewModel(count, page, pagesize),
                SortViewModel = new SortViewModel(sortorder),
                Posts = item
            };
            return View(viewModel);
        }

        public async Task<IActionResult> SearchUsers(string login, int page = 1)
        {
            IQueryable<User> users = db.Users;
            if (!String.IsNullOrEmpty(login))
            {
                users = users.Where(p => p.Login.Contains(login));
            }

            int pagesize = 5;
            var count = await users.CountAsync();
            var item = await users.Skip((page - 1) * pagesize).Take(pagesize).ToArrayAsync();

            IndexViewModel viewModel = new IndexViewModel
            {
                FilterUsersViewModel = new FilterUsersViewModel(login),
                PageViewModel = new PageViewModel(count, page, pagesize),
                Users = item
            };
            return View(viewModel);
        }


            public async Task<IActionResult> AdminUserPanel(int? id, string email, int page=1, SortState sortorder = SortState.IdAsc)
            {

                IQueryable<User> users = db.Users;
                // Фильтрация или поиск

                if (id != null && id > 0)
                {
                    users = users.Where(p => p.Id == id);
                }
                if (!String.IsNullOrEmpty(email))
                {
                    users = users.Where(p => p.Email.Contains(email));
                }
                //сортировка
                switch (sortorder)
                {
                    case SortState.IdAsc:
                        {
                            users = users.OrderBy(p => p.Id);
                            break;
                        }
                    case SortState.IdDesc:
                        {
                            users = users.OrderByDescending(p => p.Id);
                            break;
                        }
                    case SortState.EmailAsc:
                        {
                            users = users.OrderBy(p => p.Email);
                            break;
                        }
                    case SortState.EmailDesc:
                        {
                            users = users.OrderByDescending(p => p.Email);
                            break;
                        }
            };
            // Пaгинация
            int pagesize = 5;
            var count = await users.CountAsync();
            var item = await users.Skip((page - 1) * pagesize).Take(pagesize).ToArrayAsync();

            IndexViewModel viewModel = new IndexViewModel
            {
                FilterViewModel = new FilterViewModel(id, email),
                PageViewModel = new PageViewModel(count, page, pagesize),
                SortViewModel = new SortViewModel(sortorder),
                Users = item
            };
            return View(viewModel);

        }
        /////////////////////////
        public async Task<IActionResult>  Profile()
        {
            User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == HttpContext.Session.GetInt32("UserId"));
            IQueryable<Post> post = db.Posts;
            post= post.Where(p => p.UserId== HttpContext.Session.GetInt32("UserId"));
            int pagesize = 10;
            int page = 1;
            var item = await post.Skip((page - 1) * pagesize).Take(pagesize).ToArrayAsync();

            IndexViewModel viewModel = new IndexViewModel
            {
                Posts = item,
                User = user
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AdminAddPost(Post post)
        {
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return RedirectToAction("AdminPostPanel");
        }
        public IActionResult AdminAddPost()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {          
            post.UserId = (int)HttpContext.Session.GetInt32("UserId");
            db.Posts.Add(post);
            await db.SaveChangesAsync();
           return  RedirectToAction("Profile");
        }
        public IActionResult CreatePost()
        {
            return View();
        }
        
        /////////////////////////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
