using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        [Route("/Admin/Profile")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/Admin/Profile/Create")]
        public IActionResult Create()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            //if (loggedProfile == null)
            //    return RedirectToAction("Create", "Profile");


           FormModelProfile formModel = new FormModelProfile();

            return View("/Views/Admin/Profiles/Create.cshtml", formModel); 
        }

        [HttpPost]
        [Route("/Admin/Profile/Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FormModelProfile formModel)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            if (loggedProfile != null)
                return RedirectToAction("Index", "Admin");

            formModel.Profile.UserId = userId;

            if (!ModelState.IsValid)
            {
                return View("/Views/Admin/Profiles/Create.cshtml", formModel);
            }

            AdminManager.AddNewProfile(formModel.Profile);
            return RedirectToAction("Index", "Admin");
        }
    }
}
