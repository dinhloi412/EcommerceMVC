using EcommerceMVC.Data;
using EcommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;

        public HangHoaController(Hshop2023Context context )
        {
            db = context;
        }
        public IActionResult Index(int? loai)
        {
            var hangHoa = db.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoa = hangHoa.Where(p => p.MaLoai == loai.Value);
            }
            var result = hangHoa.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                Hinh = p.Hinh ?? "",
                TenHh = p.TenHh,
                MoTaNgan = p.MoTa ?? "",
                DonGia = p.DonGia ?? 0,
                TenLoai = p.MaLoaiNavigation.TenLoai

            });
            return View(result);
        }

        public IActionResult Search(string? query)
        {
            var hangHoa = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoa = hangHoa.Where(p => p.TenHh.Contains(query));
            }
            var result = hangHoa.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                Hinh = p.Hinh ?? "",
                TenHh = p.TenHh,
                MoTaNgan = p.MoTa ?? "",
                DonGia = p.DonGia ?? 0,
                TenLoai = p.MaLoaiNavigation.TenLoai

            });
            return View(result);
        }

        public IActionResult Detail(int id)
        {
            var data = db.HangHoas.Include(p => p.MaLoaiNavigation.HangHoas)
                .SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var result = new ChitietHangHoaVM
            {
                MaHh= data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                TenLoai =  data.MaLoaiNavigation.TenLoai,
                DiemDanhGia =  5,
                SoLuongTon = 10,
            };
            return View(result);  
        }
    }
}
