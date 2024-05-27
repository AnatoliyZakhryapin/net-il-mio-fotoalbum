using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("message")]
    public class Message
    {
        public long MessageId { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }
        public string SenderEmail {  get; set; }
        public string MessageText { get; set; }
        public DateTime SendAt { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
        public Message() { }
    }
}
