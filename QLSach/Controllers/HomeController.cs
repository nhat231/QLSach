using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSach.Models;
using QLSach.Models.Entities;
using System.Diagnostics;

namespace QLSach.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        private readonly AppDbContext _appdbcontext;
        public HomeController(AppDbContext appdbcontext)
        {
            _appdbcontext = appdbcontext;
        }

        public async Task<IActionResult> Index()
        {
            var r = await _appdbcontext.SACHS.Select(r => new ListSachVm()
            {
                MASACH = r.MASACH,
                TENSACH = r.TENSACH,
                TENTACGIA = r.TENTACGIA,
                NHAXB = r.NHAXB,
                CHUDE = r.CHUDE,
                SOLUONG = r.SOLUONG,
                DONGIA = r.DONGIA,
                NAMXB = r.NAMXB,
                GHICHU = r.GHICHU,
            }).ToListAsync();
            return View(r);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SACHS s)
        {
            if (string.IsNullOrEmpty(s.MASACH))
            {
                TempData["Message"] = "Mã sách không được trống.";
                return RedirectToAction("Create");
            }
            if (string.IsNullOrEmpty(s.TENSACH))
            {
                TempData["Message"] = "Tên sách không được trống.";
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
        public async Task<IActionResult> Edit(string MASACH)
        {
            if (MASACH == null)
            {
                return NotFound();
            }

            var v = await _appdbcontext.SACHS.FindAsync(MASACH);
            if (v == null)
            {
                return NotFound();
            }
            return View(v);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SACHS s)
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
        public async Task<JsonResult> Delete(string _maSach)
        {
            try
            {
                var rowtable = await _appdbcontext.SACHS.FirstOrDefaultAsync(x => x.MASACH == _maSach);
                if (rowtable == null) return Json(new { Code = 500 }); ;
                _appdbcontext.SACHS.Remove(rowtable);
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