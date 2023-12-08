using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLSach.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QLSach.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly AppDbContext _appdbcontext;
        public ThongKeController(AppDbContext appdbcontext)
        {
            _appdbcontext = appdbcontext;
        }
        public IActionResult Index()
        {
           return View();
        }
        [HttpGet]
        public async Task<JsonResult> LoadDoanhThu(DateTime ngayxuat1, DateTime ngayxuat2)
        {
            try
            {
                var query = "SELECT  a.MAHD,a.NGAYXUAT,SUM(TONGTIEN) as TONGTIEN  FROM HOADONS a WHERE TINHTRANG = N'Đã thanh toán' AND NGAYXUAT BETWEEN '"+ngayxuat1.ToString("yyyyMMdd") + "' AND '"+ngayxuat2.ToString("yyyyMMdd") + "' GROUP BY a.MAHD,a.NGAYXUAT";
                using (SqlConnection conn = new SqlConnection(_appdbcontext.Database.GetConnectionString()))
                {
                    var r = await conn.QueryAsync<ListDoanhThuVm>(query);
                    return Json(new { Code = 200, ListDoanhThu = r });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
    }
}
