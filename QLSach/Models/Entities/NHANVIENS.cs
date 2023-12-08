using System.ComponentModel.DataAnnotations;

namespace QLSach.Models.Entities
{
    public class NHANVIENS
    {
        [Key]
        public string? MANV { get; set; } = string.Empty;
        public string TENNV { get; set; } = string.Empty;
        public string? MATKHAU { get; set; } = string.Empty;
        public string? SDT { get; set; }
    }
}
