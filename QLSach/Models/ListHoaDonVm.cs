namespace QLSach.Models
{
    public class ListHoaDonVm
    {
        public string MAHD { get; set; } = string.Empty;
        public string MANV { get; set; } = string.Empty;
        public string TENNV { get; set; } = string.Empty;
        public string MAKH { get; set; } = string.Empty;
        public string TENKH { get; set; } = string.Empty;
        public DateTime NGAYXUAT { get; set; }
        public string TINHTRANG { get; set; } = string.Empty;
        public decimal TONGTIEN { get; set; }
    }
}
