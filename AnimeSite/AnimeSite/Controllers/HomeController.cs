using AnimeSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace AnimeSite.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        int idOfUser = 0;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        //////////////////////////////////////////
        public IActionResult Authorization()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(LogUser a)
        {
            User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Login == a.Login && predicate.Password == a.Password);
            if (user != null)
            {
                idOfUser = user.Id;
                return RedirectToAction("AdminUserPanel");               
            }
            return NotFound();
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
        public async Task<IActionResult> Create (User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("AdminUserPanel");
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
            IQueryable<Post> post = db.Posts;
            post= post.Where(p => p.UserId==idOfUser);
            int pagesize = 10;
            int page = 1;
            var item = await post.Skip((page - 1) * pagesize).Take(pagesize).ToArrayAsync();

            IndexViewModel viewModel = new IndexViewModel
            {
                Post = item
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            // IQueryable<User> users = db.Users;
            post.UserId = idOfUser;
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return View();
        }
        public IActionResult CreatePost()
        {
            return View();
        }
        // [HttpPost]
        //public async Task<IActionResult> Profile(int id, string photolink)
        //{


        //    return null;
        //}
        /////////////////////////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
