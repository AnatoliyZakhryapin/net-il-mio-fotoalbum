using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string currentController = ControllerContext.RouteData.Values["controller"].ToString();
            string currentAction = ControllerContext.RouteData.Values["action"].ToString();
            string currentPage = $"{currentController}/{currentAction}";

            ViewData["CurrentPage"] = currentPage;

            using FotoAlbumContext db = new FotoAlbumContext();

            var allImagesQuery = db.Images.Include(img => img.Categories).Include(img => img.Profile).Where(img => img.HasPermitVisibility == true && img.IsVisibile == true).AsQueryable();

            List<Image> allImages = allImagesQuery.ToList();

            var allCategories = AdminManager.GetAllCategories();

            ImageIndexViewModel viewModel = new ImageIndexViewModel
            {
                AllImages = allImages,
                AllCategories = allCategories
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Contact()
        {
            Message message = new Message();
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Message messageToSave)
        {
            if (!ModelState.IsValid)
            {
                return View("Contact", messageToSave);
            }

            try
            {
                using FotoAlbumContext db = new FotoAlbumContext();

                messageToSave.ProfileId = 1;
                messageToSave.SendAt = DateTime.Now;

                db.Messages.Add(messageToSave);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }
    }
}
