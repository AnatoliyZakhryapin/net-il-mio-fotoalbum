using Microsoft.EntityFrameworkCore;
using Pizzeria.Data.CustomValidationeRules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public long CategoryId {  get; set; }
   
        [Required(ErrorMessage = "The name is required")]
        [MinLength(3, ErrorMessage = "The name must have at least 3 letters")]
        [Column("Name")]
        public string Name { get; set; }
        public List<Image>? Images { get; set; }
        public Category() { }
    }
}
