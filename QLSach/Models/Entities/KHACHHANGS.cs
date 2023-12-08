using System.ComponentModel.DataAnnotations;

namespace QLSach.Models.Entities
{
    public class KHACHHANGS
    {
        [Key]
        public string MAKH { get; set; } = string.Empty;
        public string TENKH { get; set; } = string.Empty;
        public string? DIACHI { get; set; }
        public string? SDT { get; set; }
    }
}
