using System.ComponentModel.DataAnnotations;

namespace QLSach.Models.Entities
{
    public class SACHS
    {
        [Key]
        public string MASACH { get; set; } = string.Empty;
        public string TENSACH { get; set; } = string.Empty;
        public string? TENTACGIA { get; set; }
        public string? NHAXB { get; set; }
        public string? CHUDE { get; set; } 
        public int SOLUONG { get; set; } 
        public int DONGIA { get; set; }
        public int NAMXB { get; set; }
        public string? GHICHU { get; set; }
    }
}
