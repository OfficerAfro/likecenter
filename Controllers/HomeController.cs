using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using likecenter.Models;

namespace likecenter.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("RegCheck")]
        public IActionResult RegCheck(User u)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                u.Password = Hasher.HashPassword(u, u.Password);
                dbContext.Add(u);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UID", u.UserId);
                return Redirect("LikeCenter");
            }
            return View("Index");
        }

        [HttpPost("LogCheck")]
        public IActionResult LogCheck(LUser l)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == l.LEmail);

                if(userInDb == null)
                {

                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Index");
                }
            

                var hasher = new PasswordHasher<LUser>();

                var result = hasher.VerifyHashedPassword(l, userInDb.Password, l.LPassword);
            

                if(result == 0)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UID", userInDb.UserId);
                return Redirect("LikeCenter");
            }
            return View("Index");
        }
        [HttpGet("LikeCenter")]
        public IActionResult LikeCenter()
        {
            int? Sess = HttpContext.Session.GetInt32("UID");
            if(Sess == null)
            {
                return Redirect("/");
            }
            
            //Query for the logged in user
            User loggedIn = dbContext.Users.FirstOrDefault(u => u.UserId == (int)Sess);
            //Put the user in a viewbag
            ViewBag.User = loggedIn;
            var AllUsers = dbContext.Users.ToList();
            ViewBag.AllPosts = dbContext.Posts.Include(k => k.Likers).OrderByDescending(s => s.Likers.Count).ToList();
            return View("LikeCenter");
        }
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        [HttpPost("Add")]
        public IActionResult Add(Post g)
        {
            int? Sess = HttpContext.Session.GetInt32("UID");
            if(Sess == null)
            {
                return Redirect("/");
            }
            User loggedIn = dbContext.Users.FirstOrDefault(u => u.UserId == (int)Sess);
            ViewBag.User = loggedIn;
            if(ModelState.IsValid)
            {
               
                    dbContext.Add(g);
                    dbContext.SaveChanges();
                    return RedirectToAction("DashboardRedir");
                
            }
          

            ViewBag.AllPosts = dbContext.Posts.Include(k => k.Likers).OrderBy(s => s.LikeCount).ToList();


            return View("LikeCenter");
        }
        [HttpGet("DashboardRedir")]
        public IActionResult DashboardRedir()
        {
            return RedirectToAction("LikeCenter");
        }
        [HttpGet("Delete/{PostId}")]
        public IActionResult DeleteAct(int PostId)
        {
            Post DeletedGame = dbContext.Posts.FirstOrDefault(g => g.PostId == PostId);
            dbContext.Remove(DeletedGame);
            dbContext.SaveChanges();
            return RedirectToAction("LikeCenter");
        }
        [HttpGet("like/{UserId}/{PostId}")]
        public IActionResult like(int UserId, int PostId)
        {
            Synergy JoinFan = new Synergy();
            JoinFan.UserId = UserId;
            JoinFan.PostId = PostId;
            ViewBag.ThisPost = dbContext.Posts.Include(g => g.Likers).FirstOrDefault(c => c.PostId == PostId);
            ViewBag.ThisUser = dbContext.Users.Include(l => l.Synergies).FirstOrDefault(c => c.UserId == UserId);
            var ThisPost = ViewBag.ThisPost;
            var ThisUser = ViewBag.ThisUser;
            ThisUser.LikeCounter =+ 1;
            dbContext.Add(JoinFan);
            dbContext.SaveChanges();
            return RedirectToAction("LikeCenter");
        }
        [HttpGet("unlike/{UserId}/{PostId}")]
        public IActionResult unlike(int UserId, int PostId)
        {
            var Unjoin = dbContext.Synergies.Where(a => a.UserId == UserId).FirstOrDefault(b => b.PostId == PostId);
            ViewBag.ThisUser = dbContext.Users.Include(l => l.Synergies).FirstOrDefault(c => c.UserId == UserId);
            var ThisUser = ViewBag.ThisUser;
            ThisUser.LikeCounter =- 1;
            dbContext.Remove(Unjoin);
            dbContext.SaveChanges();
            return RedirectToAction("DashboardRedir");
        }
        [HttpGet("ViewLike/{PostId}")]
        public IActionResult ViewLike(int PostId)
        {
            int? Sess = HttpContext.Session.GetInt32("UID");
            if(Sess == null)
            {
                return Redirect("/");
            }
            ViewBag.ThisPost = dbContext.Posts.Include(v => v.Likers).ThenInclude(z => z.User).FirstOrDefault(c => c.PostId == PostId);
            return View("ViewLike", ViewBag.User);
        }
        [HttpGet("ViewUser/{UserId}")]
        public IActionResult ViewUser(int UserId)
        {

            ViewBag.ThisUser = dbContext.Users.Include(l => l.Synergies).FirstOrDefault(c => c.UserId == UserId);
            var ThisUSer = ViewBag.ThisUser;
            ViewBag.ThisUsersPosts = dbContext.Posts.Where(m => m.CreatorId == UserId).ToList();
            return View("ViewUser");
        }
        


    }
}
