using System.ComponentModel.DataAnnotations;

namespace QLSach.Models.Entities
{
    public class HOADONS
    {
        [Key]
        public string MAHD { get; set; } = string.Empty;
        public string MANV { get; set; } = string.Empty;
        public string MAKH { get; set; } = string.Empty;
        public DateTime NGAYXUAT { get; set; }
        public string TINHTRANG { get; set; } = string.Empty;
        public decimal TONGTIEN { get; set; }
    }
}
