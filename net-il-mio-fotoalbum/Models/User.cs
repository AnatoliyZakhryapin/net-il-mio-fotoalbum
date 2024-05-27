using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class User : IdentityUser
    {
        //[Key]
        //public override string Id {  get; set; }
        public Profile Profile { get; set; }
    }
}
