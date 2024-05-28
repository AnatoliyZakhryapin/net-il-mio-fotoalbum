using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Globalization;

namespace net_il_mio_fotoalbum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index([FromQuery] APIQueryParameters queryParameters)
        {
            using FotoAlbumContext db = new FotoAlbumContext();

            var allImagesQuery = db.Images.Include(img => img.Categories).Include(img => img.Profile).Where(img => img.HasPermitVisibility == true && img.IsVisibile == true).AsQueryable();


            if (!string.IsNullOrEmpty(queryParameters.TitleFilter))
            {
                allImagesQuery = allImagesQuery.Where(img => img.Title.Contains(queryParameters.TitleFilter));
            }

            if (queryParameters.CategoryFilter != null && queryParameters.CategoryFilter.Any())
            {
                allImagesQuery = allImagesQuery.Where(img => img.Categories.Any(cat => queryParameters.CategoryFilter.Contains(cat.Name)));
            }

            switch (queryParameters.SortBy)
            {
                case "createdAt":
                    allImagesQuery = allImagesQuery.OrderBy(img => img.CreatedAt);
                    break;
                case "lastUpdatedAt":
                    allImagesQuery = allImagesQuery.OrderBy(img => img.LastUpdatedAt);
                    break;
                default:
                    break;
            }

            List<Image> allImages = allImagesQuery.ToList();

            var allCategories = AdminManager.GetAllCategories();

            ImageIndexViewModel viewModel = new ImageIndexViewModel
            {
                AllImages = allImages,
                AllCategories = allCategories
            };

            return Ok(allImages);
        }
    }
}
