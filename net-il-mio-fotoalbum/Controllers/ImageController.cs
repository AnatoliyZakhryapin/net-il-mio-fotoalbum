using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public IActionResult Index(string? titleFilter, List<string>? categoryFilter, string? sortBy)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");

            using FotoAlbumContext db = new FotoAlbumContext();

            var userImagesQuery = db.Images.Include(i => i.Categories).Include(i => i.Profile).Where(i => i.Profile.UserId == userId).AsQueryable();

            IQueryable<Image> allImagesQuery = Enumerable.Empty<Image>().AsQueryable();

            if (isAdmin)
                allImagesQuery = db.Images.Include(i => i.Categories).Include(i => i.Profile).Where(img => img.Profile.UserId != userId);

            if (!string.IsNullOrEmpty(titleFilter))
            {
                userImagesQuery = userImagesQuery.Where(img => img.Title.Contains(titleFilter));

                if (isAdmin)
                    allImagesQuery = allImagesQuery.Where(img => img.Title.Contains(titleFilter));
            }

            if (categoryFilter != null && categoryFilter.Any())
            {
                userImagesQuery = userImagesQuery.Where(img => img.Categories.Any(cat => categoryFilter.Contains(cat.Name)));

                if (isAdmin)
                    allImagesQuery = allImagesQuery.Where(img => img.Categories.Any(cat => categoryFilter.Contains(cat.Name)));
            }

            switch (sortBy)
            {
                case "createdAt":
                    userImagesQuery = userImagesQuery.OrderBy(img => img.CreatedAt);
                    if (isAdmin)
                        allImagesQuery = allImagesQuery.OrderBy(img => img.CreatedAt);
                    break;
                case "lastUpdatedAt":
                    userImagesQuery = userImagesQuery.OrderBy(img => img.LastUpdatedAt);
                    if (isAdmin)
                        allImagesQuery = allImagesQuery.OrderBy(img => img.LastUpdatedAt);
                    break;
                default:
                    break;
            }

            List<Image> userImages = userImagesQuery.ToList();
            List<Image> allImages = new List<Image>();
            if (isAdmin)
                allImages = allImagesQuery.ToList();

            var allCategories = AdminManager.GetAllCategories();

            ImageIndexViewModel viewModel = new ImageIndexViewModel
            {
                UserImages = userImages,
                AllImages = allImages,
                AllCategories = allCategories
            };

            return View("/Views/Admin/Images/Index.cshtml", viewModel);
        }

        [Route("/Admin/Images/{id}")]
        public IActionResult Show (long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            Image image = AdminManager.GetImageById(id);

            if (image.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                return View("/Views/Admin/Images/Show.cshtml", image);
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

        [HttpGet]
        public IActionResult Update(long id ) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            Image image = AdminManager.GetImageById(id);

            if (image.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                if (image != null)
                {
                    FormModel model = AdminManager.CreateFormModel(image);
                    return View("/Views/Admin/Images/Update.cshtml", model);
                }
                else
                    return NotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(long id, FormModel formModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            Image image = AdminManager.GetImageById(id);

            if (image.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                if (!ModelState.IsValid)
                {
                    if (ModelState["ImageFormFile"].ValidationState != ModelValidationState.Invalid || ModelState.Values.Count(v => v.ValidationState == ModelValidationState.Invalid) != 1)
                    {
                        formModel.CreateCategories();
                        formModel.Image.ImageId = id;
                        formModel.Image.ImageFile = image.ImageFile;
                        return View("/Views/Admin/Images/Update.cshtml", formModel);
                    }
                }

                formModel.SetImageFileFromFormFile();
                formModel.Image.ProfileId = loggedProfile.ProfileId;

                bool result = AdminManager.UpdateImage(id, formModel.Image, formModel.SelectedCategories);

                if (result == true)
                    return RedirectToAction("Show", new { id = id });
                else
                            return NotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete (long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            Image image = AdminManager.GetImageById(id);

            if (image.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                AdminManager.DeleteImage(id);
            }

            return RedirectToAction("Index");
        }
    }
}
