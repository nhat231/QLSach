using Microsoft.EntityFrameworkCore;
using QLSach.Models.Entities;

namespace QLSach.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<SACHS> SACHS { get; set; }
        public DbSet<KHACHHANGS> KHACHHANGS { get; set; }
        public DbSet<NHANVIENS> NHANVIENS { get; set; }
        public DbSet<HOADONS> HOADONS { get; set; }
        public DbSet<CHITIETHOADONS> CHITIETHOADONS { get; set; }
    }
}
