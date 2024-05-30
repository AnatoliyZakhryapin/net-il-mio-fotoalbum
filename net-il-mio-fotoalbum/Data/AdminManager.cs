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

        public static bool AddNewProfile(Profile profile)
        {
            try
            {
                using FotoAlbumContext db = new FotoAlbumContext();
                db.Add(profile);
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
        public static List<Message> GetProfilesMessages(long id, bool includeReferences = false)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            if (includeReferences)
                return db.Messages.Where(m => m.ProfileId == id).Include(m => m.Profile).ToList();
            return db.Messages.Where(m => m.ProfileId == id).ToList();
        }

        public static Message GetMessageById(long id, bool includeReferences = false)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            if (includeReferences)
                return db.Messages.Include(m => m.Profile).FirstOrDefault(m => m.MessageId == id);
            return db.Messages.FirstOrDefault(m => m.MessageId == id);
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
                return formModel;
            }

            formModel.Image = new Image ();
            formModel.CreateCategories();
            return formModel;
        }

        public static bool DeleteImage(long id)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            var imageToDelete = db.Images.Find(id);

            if (imageToDelete == null)
                return false;

            db.Images.Remove(imageToDelete);
            db.SaveChanges();
            return true;
        }

        public static bool DeleteCategory(long id)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            var categoryToDelete = db.Categories.Find(id);

            if (categoryToDelete == null)
                return false;

            db.Categories.Remove(categoryToDelete);
            db.SaveChanges();
            return true;
        }

        public static bool DeleteMessage(long id)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            var messageToDelete = db.Messages.Find(id);

            if (messageToDelete == null)
                return false;

            db.Messages.Remove(messageToDelete);
            db.SaveChanges();
            return true;
        }

        public static bool UpdateImage(long id, Image imageUpdated, List<string> SelectedCategories)
        {
            using FotoAlbumContext db = new FotoAlbumContext();
            var imageToUpdate = db.Images.Where(i => i.ImageId == id).Include(i => i.Categories).FirstOrDefault();

            if (imageToUpdate != null)
            {
                imageToUpdate.Title = imageUpdated.Title;
                imageToUpdate.Description = imageUpdated.Description;
                imageToUpdate.IsVisibile = imageUpdated.IsVisibile;
                imageToUpdate.HasPermitVisibility = imageUpdated.HasPermitVisibility;
                imageToUpdate.ProfileId = imageUpdated.ProfileId;
                imageToUpdate.CreatedAt = imageUpdated.CreatedAt;
                imageToUpdate.LastUpdatedAt = DateTime.UtcNow;

                if (imageUpdated.ImageFile != null && imageUpdated.ImageFile.Length > 0)
                {
                    imageToUpdate.ImageFile = imageUpdated.ImageFile;
                }

                imageToUpdate.Categories.Clear();
                if (SelectedCategories != null)
                {
                    foreach (string i in SelectedCategories)
                    {
                        int idCategory = int.Parse(i);
                        Category category = db.Categories.FirstOrDefault(c => c.CategoryId == idCategory);
                        imageToUpdate.Categories.Add(category);
                    }
                }

                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
