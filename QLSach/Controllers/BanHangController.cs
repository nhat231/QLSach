using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QLSach.Models;
using QLSach.Models.Entities;
using System.Runtime.CompilerServices;

namespace QLSach.Controllers
{
    [Authorize]
    public class BanHangController : Controller
    {
        private readonly AppDbContext _appdbcontext;
        public BanHangController(AppDbContext appdbcontext)
        {
            _appdbcontext = appdbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> ListHOADONS()
        {
            try
            {
                var query = "DELETE CHITIETHOADONS WHERE MAHD NOT IN (SELECT MAHD FROM HOADONS)";
                using (SqlConnection conn = new SqlConnection(_appdbcontext.Database.GetConnectionString()))
                {
                    var x = await conn.ExecuteAsync(query);
                }

                var r = await (from a in _appdbcontext.HOADONS
                               join b in _appdbcontext.NHANVIENS on a.MANV equals b.MANV into tmp1
                               from ab in tmp1.DefaultIfEmpty()
                               join c in _appdbcontext.KHACHHANGS on a.MAKH equals c.MAKH into tmp2
                               from bc in tmp2.DefaultIfEmpty()
                               select new ListHoaDonVm()
                               {
                                   MAHD = a.MAHD,
                                   MANV = a.MANV,
                                   TENNV = ab.TENNV,
                                   MAKH = a.MAKH,
                                   TENKH = bc.TENKH,
                                   NGAYXUAT = a.NGAYXUAT,
                                   TINHTRANG = a.TINHTRANG,
                                   TONGTIEN = a.TONGTIEN
                               }).OrderBy(x => x.NGAYXUAT).ToListAsync();
                return Json(new { Code = 200, ListHOADONS = r });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> ListCHITIETHOADONS(string mahd)
        {
            try
            {
                var r = await (from a in _appdbcontext.CHITIETHOADONS.Where(x=>x.MAHD == mahd)
                               join b in _appdbcontext.HOADONS on a.MAHD equals b.MAHD into tmp1
                               from ab in tmp1.DefaultIfEmpty()
                               join c in _appdbcontext.SACHS on a.MASACH equals c.MASACH into tmp2
                               from bc in tmp2.DefaultIfEmpty()
                               select new ListCTHDVm()
                               {
                                   MAHD = a.MAHD,
                                   MASACH = a.MASACH,
                                   TENSACH = bc.TENSACH,
                                   SOLUONG = a.SOLUONG,
                                   DONGIA = bc.DONGIA
                               }).OrderBy(x => x.MASACH).ToListAsync();
                return Json(new { Code = 200, ListCHITIETHOADONS = r });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        public string IdNumber()
        {
            string sTemp = DateTime.Now.ToString("yyMMddhhmmss");
            string sBarcode = sTemp;
            int iSum = 0;
            int iDigit = 0;
            for (int i = sTemp.Length; i >= 1; i += -1)
            {
                iDigit = Convert.ToInt32(sTemp.Substring(i - 1, 1));
                if (i % 2 == 0)
                    iSum += iDigit * 3;
                else
                    iSum += iDigit * 1;
            }
            int iCheckSum = (10 - (iSum % 10)) % 10;
            sBarcode = sBarcode + iCheckSum.ToString();
            return sBarcode;
        }
        public JsonResult GenIdNumber()
        {
            try
            {
                string sBarcode = "PBH" + IdNumber();
                return Json(new { Code = 200, sBarcode = sBarcode });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        [HttpGet]
        public async Task<JsonResult> LoadDanhSachNhanVien()
        {
            try
            {
                var r = await _appdbcontext.NHANVIENS.Select(r => new ListNhanVienVm()
                {
                    MANV = r.MANV,
                    TENNV = r.TENNV
                }).OrderBy(x => x.MANV)
                .ToListAsync();
                return Json(new { Code = 200, danhSachNhanVien = r });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        [HttpGet]
        public async Task<JsonResult> LoadDanhSachKhachHang()
        {
            try
            {
                var r = await _appdbcontext.KHACHHANGS.Select(r => new ListKhachHangVm()
                {
                    MAKH = r.MAKH,
                    TENKH = r.TENKH
                }).OrderBy(x => x.MAKH)
                .ToListAsync();
                return Json(new { Code = 200, danhSachKhachHang = r });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        [HttpGet]
        public async Task<JsonResult> LoadDanhSachSanPham()
        {
            try
            {
                var r = await _appdbcontext.SACHS.Select(r => new ListSachVm()
                {
                    MASACH = r.MASACH,
                    TENSACH = r.TENSACH
                }).OrderBy(x => x.MASACH)
                .ToListAsync();
                return Json(new { Code = 200, danhSachSanPham = r });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddCHITIETHOADONS(string mahd,string masach, decimal soluong)
        {
            try
            {
                var rowtable = await _appdbcontext.CHITIETHOADONS.FirstOrDefaultAsync(x => x.MASACH == masach && x.MAHD == mahd);
                if (rowtable != null)
                {
                    rowtable.SOLUONG = rowtable.SOLUONG + soluong;
                    await _appdbcontext.SaveChangesAsync();
                    return Json(new { Code = 200 });
                }

                var rowSach = await _appdbcontext.SACHS.FirstOrDefaultAsync(x => x.MASACH == masach);

                var table = new CHITIETHOADONS()
                {
                    MAHD = mahd,
                    MASACH = masach,
                    SOLUONG = soluong,
                    DONGIA = rowSach.DONGIA,
                };
                await _appdbcontext.CHITIETHOADONS.AddAsync(table);
                await _appdbcontext.SaveChangesAsync();
                return Json(new { Code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });

            }
        }
        public async Task<JsonResult> AddHOADONS(string mahd,string manv,string makh,string ngayxuat)
        {
            try
            {
                var table = new HOADONS()
                {
                    MAHD = mahd,
                    MANV = manv,
                    MAKH = makh,
                    NGAYXUAT = DateTime.Parse(ngayxuat),
                    TINHTRANG = "Chưa thanh toán",
                    TONGTIEN = 0,
                };
                await _appdbcontext.HOADONS.AddAsync(table);
                await _appdbcontext.SaveChangesAsync();

                var query = "UPDATE HOADONS SET TONGTIEN =  (SELECT SUM(DONGIA*SOLUONG) FROM CHITIETHOADONS WHERE MAHD = '" + mahd + "') WHERE MAHD = '"+mahd+"'";
                using (SqlConnection conn = new SqlConnection(_appdbcontext.Database.GetConnectionString()))
                {
                    var x = await conn.ExecuteAsync(query);
                }

                return Json(new { Code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });

            }
        }
        [HttpGet]
        public async Task<JsonResult> UpdateHOADONS(string mahd)
        {
            try
            {
                var rowtable = await _appdbcontext.HOADONS.FindAsync(mahd);
                return Json(new { Code = 200, r = rowtable });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
                throw;
            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteItemCTHD(string macthd,string mactsach)
        {
            try
            {
                var query = "DELETE CHITIETHOADONS WHERE MAHD ='"+macthd+"' AND MASACH = '"+mactsach+"'";
                using (SqlConnection conn = new SqlConnection(_appdbcontext.Database.GetConnectionString()))
                {
                    var x = await conn.ExecuteAsync(query);
                }
                return Json(new { Code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteHOADONS(string mahd)
        {
            try
            {
                var query = " DELETE CHITIETHOADONS WHERE MAHD ='" + mahd + "'";
                query += " DELETE HOADONS WHERE MAHD ='" + mahd + "'";
                using (SqlConnection conn = new SqlConnection(_appdbcontext.Database.GetConnectionString()))
                {
                    var x = await conn.ExecuteAsync(query);
                }
                return Json(new { Code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> TinhTienHOADONS(string mahd)
        {
            try
            {
                var query = " UPDATE HOADONS SET TINHTRANG = N'Đã thanh toán' WHERE MAHD ='" + mahd + "'";
                using (SqlConnection conn = new SqlConnection(_appdbcontext.Database.GetConnectionString()))
                {
                    var x = await conn.ExecuteAsync(query);
                }
                return Json(new { Code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateHOADONS(string mahd, string manv, string makh, string ngayxuat)
        {
            try
            {
                var rowtable = await _appdbcontext.HOADONS.FindAsync(mahd);
                if (rowtable == null) return Json(new { Code = 500 });
                rowtable.MANV = manv;
                rowtable.MAKH = makh;
                rowtable.NGAYXUAT = DateTime.Parse(ngayxuat);
                await _appdbcontext.SaveChangesAsync();
                var query = "UPDATE HOADONS SET TONGTIEN =  (SELECT SUM(DONGIA*SOLUONG) FROM CHITIETHOADONS WHERE MAHD = '" + mahd + "') WHERE MAHD = '" + mahd + "'";
                using (SqlConnection conn = new SqlConnection(_appdbcontext.Database.GetConnectionString()))
                {
                    var x = await conn.ExecuteAsync(query);
                }
                return Json(new { Code = 200, r = rowtable });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }

    }
}
