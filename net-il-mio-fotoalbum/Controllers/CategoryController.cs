using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        [Route("/Admin/Categories")]
        public IActionResult Index()
        {
            List<Category> categories = AdminManager.GetAllCategories();
            return View("/Views/Admin/Categories/Index.cshtml", categories);
        }

        [HttpGet]
        [Route("/Admin/Categories/Create")]
        public IActionResult Create()
        {
            Category category = new Category();
            return View("/Views/Admin/Categories/Create.cshtml", category);
        }

        [HttpPost]
        [Route("/Admin/Categories/Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Category data)
        {
            if (!ModelState.IsValid)
            {
                return View("/Views/Admin/Categories/Create.cshtml", data);
            }

            AdminManager.AddNewCategory(data);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long id)
        {
            AdminManager.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}
