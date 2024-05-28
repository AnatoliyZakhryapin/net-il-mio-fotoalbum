using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Image
    {
        public long ImageId { get; set; }

        [Required(ErrorMessage = "The title is required")]
        [MinLength(3, ErrorMessage = "The title must have at least 3 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The description is required")]
        public string Description { get; set; }
        public byte[]? ImageFile {  get; set; }
        public string ImgSrc => ImageFile != null ? $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}" : "";
        [Required(ErrorMessage = "The IsVisibile is required")]
        public bool IsVisibile { get; set; }
        [Required(ErrorMessage = "The HasPermitVisibility is required")]
        public bool HasPermitVisibility { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public List<Category>? Categories { get; set; }
        public long CreatedByProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
