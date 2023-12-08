using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSach.Models;
using QLSach.Models.Entities;

namespace QLSach.Controllers
{
    [Authorize]
    public class NhanVienController : Controller
    {

        private readonly AppDbContext _appdbcontext;
        public NhanVienController(AppDbContext appdbcontext)
        {
            _appdbcontext = appdbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var r = await _appdbcontext.NHANVIENS.Select(r => new ListNhanVienVm()
            {
                MANV = r.MANV,
                TENNV = r.TENNV,
                MATKHAU = r.MATKHAU,
                SDT = r.SDT,
            }).ToListAsync();
            return View(r);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NHANVIENS s)
        {
            if (string.IsNullOrEmpty(s.MANV))
            {
                TempData["Message"] = "Mã nhân viên không được trống.";
                return RedirectToAction("Create");
            }
            if (string.IsNullOrEmpty(s.TENNV))
            {
                TempData["Message"] = "Tên nhân viên không được trống.";
                return RedirectToAction("Create");
            }
            if (string.IsNullOrEmpty(s.MATKHAU))
            {
                TempData["Message"] = "Mật khẩu không được trống.";
                return RedirectToAction("Create");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _appdbcontext.Add(s);
                    await _appdbcontext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch
            {

            }
            return View(s);
        }
        public async Task<IActionResult> Edit(string MANV)
        {
            if (MANV == null)
            {
                return NotFound();
            }

            var v = await _appdbcontext.NHANVIENS.FindAsync(MANV);
            if (v == null)
            {
                return NotFound();
            }
            return View(v);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(NHANVIENS s)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _appdbcontext.Update(s);
                    await _appdbcontext.SaveChangesAsync();
                }
                catch
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(s);
        }
        public async Task<JsonResult> Delete(string _maNV)
        {
            try
            {
                var rowtable = await _appdbcontext.NHANVIENS.FirstOrDefaultAsync(x => x.MANV == _maNV);
                if (rowtable == null) return Json(new { Code = 500 }); ;
                _appdbcontext.NHANVIENS.Remove(rowtable);
                await _appdbcontext.SaveChangesAsync();
                return Json(new { Code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, ex.Message });
            }
        }
    }
}
