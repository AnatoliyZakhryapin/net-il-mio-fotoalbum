using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using net_il_mio_fotoalbum.Data;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class FormModel
    {
        public Image Image { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        [Required(ErrorMessage = "The Category is required")]
        public List<string> SelectedCategories { get; set; }
        [Required(ErrorMessage = "The ImageFile is required")]
        public IFormFile? ImageFormFile { get; set; }
        public FormModel() { }

        public void CreateCategories()
        {
            this.Categories = new List<SelectListItem>();
            this.SelectedCategories = new List<string>();

            List<Category> categoriesFromDb = AdminManager.GetAllCategories();

            foreach (Category category in categoriesFromDb)
            {
                bool idSelected = this.Image.Categories?.Any(i => i.CategoryId == category.CategoryId) == true;
                this.Categories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.CategoryId.ToString(),
                    Selected = idSelected
                });
                if (idSelected)
                    this.SelectedCategories.Add(category.CategoryId.ToString());
            }
        }

        public void SetImageFileFromFormFile()
        {
            if (ImageFormFile == null)
                return;

            using var stream = new MemoryStream();
            ImageFormFile.CopyTo(stream);
            Image.ImageFile = stream.ToArray();
        }
    }
}
