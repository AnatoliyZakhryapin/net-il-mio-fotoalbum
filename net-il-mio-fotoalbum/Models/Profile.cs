using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Profile
    {
        public long ProfileId {  get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Image> Images { get; set; }
        public List<Message> Messages { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Profile() { }
    }
}
