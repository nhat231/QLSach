using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSach.Models;
using QLSach.Models.Entities;

namespace QLSach.Controllers
{
    [Authorize]
    public class KhachHangController : Controller
    {
        private readonly AppDbContext _appdbcontext;
        public KhachHangController(AppDbContext appdbcontext)
        {
            _appdbcontext = appdbcontext;
        }
        public async Task<IActionResult> Index()
        {
            var r = await _appdbcontext.KHACHHANGS.Select(r => new ListKhachHangVm()
            {
                MAKH = r.MAKH,
                TENKH = r.TENKH,
                DIACHI = r.DIACHI,
                SDT = r.SDT,
            }).ToListAsync();
            return View(r);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(KHACHHANGS s)
        {
            if (string.IsNullOrEmpty(s.MAKH))
            {
                TempData["Message"] = "Mã khách hàng không được trống.";
                return RedirectToAction("Create");
            }
            if (string.IsNullOrEmpty(s.TENKH))
            {
                TempData["Message"] = "Tên khách hàng không được trống.";
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
        public async Task<IActionResult> Edit(string MAKH)
        {
            if (MAKH == null)
            {
                return NotFound();
            }

            var v = await _appdbcontext.KHACHHANGS.FindAsync(MAKH);
            if (v == null)
            {
                return NotFound();
            }
            return View(v);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(KHACHHANGS s)
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
        public async Task<JsonResult> Delete(string _maKH)
        {
            try
            {
                var rowtable = await _appdbcontext.KHACHHANGS.FirstOrDefaultAsync(x => x.MAKH == _maKH);
                if (rowtable == null) return Json(new { Code = 500 }); ;
                _appdbcontext.KHACHHANGS.Remove(rowtable);
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
