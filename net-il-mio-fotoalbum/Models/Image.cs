using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("image")]
    public class Image
    {
        public long ImageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url {  get; set; }
        public bool IsVisibile { get; set; }
        public bool HasPermitVisibility { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public List<Category> Categories { get; set; }
        public long CreatedByProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
