using net_il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;

namespace net_il_mio_fotoalbum.Data
{
    public static class AdminManager
    {
        public static bool AddNewCategory(Category category)
        {
            try
            {
                using FotoAlbumContext db = new FotoAlbumContext();
                db.Add(category);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public static bool AddNewImage(Image image, List<string> selectedCategories) {
            try
            {
                using FotoAlbumContext db = new FotoAlbumContext();

                if(selectedCategories != null)
                {
                    image.Categories = new List<Category>();
                    foreach (var item in selectedCategories)
                    {
                        long categoryId = long.Parse(item);
                        Category category = db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                        image.Categories.Add(category);
                    }
                }

                db.Images.Add(image);
                db.SaveChanges ();
                return true;
            }
            catch { return false; }
        }

        public static Profile GetProfileByUserId(string id)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            return db.Profiles.FirstOrDefault(p => p.UserId == id);
        }
        public static List<Category> GetAllCategories(bool includeReferences = false)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            if (includeReferences)
                return db.Categories.Include(c => c.Images).ToList();
            return db.Categories.ToList();
        }
        public static List<Image> GetAllImages(bool includeReferences = false)
        {
            using FotoAlbumContext db = new FotoAlbumContext();

            if(includeReferences)
                return db.Images.Include(i => i.Categories).Include(i => i.Profile).ToList();
            return db.Images.ToList();
        }

        public static Image GetImageById(long id, bool includeReferences = false)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            if (includeReferences)
                return db.Images.Include(i => i.Categories).Include(i => i.Profile).FirstOrDefault(i => i.ImageId == id);
            return db.Images.FirstOrDefault(i => i.ImageId == id);
        }

        public static FormModel CreateFormModel(Image image = null)
        {
            FormModel formModel = new FormModel();

            if(image != null)
            {
                formModel.Image = image;
                formModel.CreateCategories();
            }

            formModel.Image = new Image ();
            formModel.CreateCategories();

            return formModel;
        }
    }
}
