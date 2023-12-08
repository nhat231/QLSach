using System.ComponentModel.DataAnnotations;

namespace QLSach.Models.Entities
{
    public class CHITIETHOADONS
    {
        [Key]
        public string MAHD { get; set; } = string.Empty;
        public string MASACH { get; set; } = string.Empty;
        public decimal SOLUONG { get; set; }
        public decimal DONGIA { get; set; }
    }
}
