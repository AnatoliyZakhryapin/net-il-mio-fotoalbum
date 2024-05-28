using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        [Route("/Admin/Categories")]
        public IActionResult Index()
        {
            return View("/Views/Admin/Categories/Index.cshtml");
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
    }
}
