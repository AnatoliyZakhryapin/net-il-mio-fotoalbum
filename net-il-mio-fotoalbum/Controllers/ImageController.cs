using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        [HttpGet]
        [Route("/Admin/Images")]
        public IActionResult Index()
        {
            List<Image> images = AdminManager.GetAllImages();
            return View("/Views/Admin/Images/Index.cshtml", images);
        }

        [Route("/Admin/Images/{id}")]
        public IActionResult Show (long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            Image imageToShow = AdminManager.GetImageById(id);

            if (imageToShow.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                return View("/Views/Admin/Images/Show.cshtml", imageToShow);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Admin/Images/Create")]
        public IActionResult Create()
        {
            FormModel formModel = AdminManager.CreateFormModel();
            return View("/Views/Admin/Images/Create.cshtml", formModel);
        }

        [HttpPost]
        [Route("/Admin/Images/Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FormModel formModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Profile profile = AdminManager.GetProfileByUserId(userId);
            formModel.Image.ProfileId = profile.ProfileId;

            if (!ModelState.IsValid)
            {
                formModel.CreateCategories();
                return View("/Views/Admin/Images/Create.cshtml", formModel);
            }

            formModel.SetImageFileFromFormFile();
            formModel.Image.CreatedAt = DateTime.UtcNow;

            ViewData["IsSaved"] = true;
            bool IsSaved = AdminManager.AddNewImage(formModel.Image, formModel.SelectedCategories);

            if (IsSaved) 
                return RedirectToAction("Index");

            ViewData["IsSaved"] = false;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete (long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            Image imageToDelete = AdminManager.GetImageById(id);

            if (imageToDelete.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                AdminManager.DeleteImage(id);
            }

            return RedirectToAction("Index");
        }
    }
}
