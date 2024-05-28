namespace net_il_mio_fotoalbum.Models
{
    public class APIQueryParameters
    {
        public string? TitleFilter {  get; set; }
        public List<string>? CategoryFilter { get; set; }
        public string? SortBy { get; set; }

    }
}
