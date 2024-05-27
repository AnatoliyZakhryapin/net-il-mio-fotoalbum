using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("user")]
    public class User
    {
        public long UserID {  get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public List<Image> Images { get; set; }
        public List<Message> Messages { get; set; }
        public User() { }
    }
}
