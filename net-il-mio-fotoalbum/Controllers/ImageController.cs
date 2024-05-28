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
            Image image = AdminManager.GetImageById(id);
            return View("/Views/Admin/Images/Show.cshtml", image);
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
            formModel.Image.CreatedByProfileId = profile.ProfileId;

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
    }
}
