using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        public long MessageId { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }
        public string SenderEmail {  get; set; }
        public string MessageText { get; set; }
        public DateTime SendAt { get; set; }
        public long ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public Message() { }
        public Message(string senderName, string senderSurname, string senderEmail, string messageText, long profileId)
        {
            SenderName = senderName;
            SenderSurname = senderSurname;
            SenderEmail = senderEmail;
            MessageText = messageText;
            ProfileId = profileId;
        }       
    }
}
