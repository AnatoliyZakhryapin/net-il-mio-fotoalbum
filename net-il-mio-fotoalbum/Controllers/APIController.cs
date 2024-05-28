using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Globalization;

namespace net_il_mio_fotoalbum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index([FromQuery] APIQueryParameters queryParameters)
        {
            //var context = HttpContext.Request.Query["CategoryFilter"].ToArray();
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

            return Ok(allImages);
        }

        [HttpGet("{id}")]
        public IActionResult GetImageById(int id)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            var image = db.Images
                          .Include(img => img.Categories)
                          .Include(img => img.Profile)
                          .FirstOrDefault(img => img.ImageId == id);

            if (image == null)
            {
                return NotFound(); // Immagine non trovata
            }

            return Ok(image); // Restituisce l'immagine trovata
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message message)
        {
            try
            {
                using FotoAlbumContext db = new FotoAlbumContext();

                message.SendAt = DateTime.Now;

                db.Messages.Add(message);

                db.SaveChanges();

                return StatusCode(201);
            }
            catch (Exception ex)
            {
      
                return StatusCode(500, ex.Message);
            }
        }
    }
}
