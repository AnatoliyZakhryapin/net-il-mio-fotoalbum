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
    }
}
