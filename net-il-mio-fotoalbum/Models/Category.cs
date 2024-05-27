using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("category")]
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public long CategoryId {  get; set; }
        [Column("category_name")]
        public string Name { get; set; }
        public List<Image> Images { get; set; }
        public Category() { }
    }
}
