using Microsoft.AspNetCore.Mvc;
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
    }
}
