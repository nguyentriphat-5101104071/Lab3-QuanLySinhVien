using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab03_QuanLySinhVienOOP
{
    public class QuanLySinhVien
    {
        private readonly List<SinhVien> _danhSach = new List<SinhVien>();

        public bool Them(SinhVien sv)
        {
            if (sv == null || _danhSach.Any(s => s.MaSinhVien.Equals(sv.MaSinhVien, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            _danhSach.Add(sv);
            return true;
        }

        public List<SinhVien> LayDanhSach()
        {
            return _danhSach.ToList();
        }

        public SinhVien TimTheoMa(string maSV)
        {
            return _danhSach.FirstOrDefault(s => s.MaSinhVien.Equals(maSV, StringComparison.OrdinalIgnoreCase));
        }

        public List<SinhVien> TimTheoTen(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa)) return new List<SinhVien>();
            return _danhSach
                .Where(s => s.HoTen != null && s.HoTen.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public bool Sua(string maSV, double diemMoi)
        {
            var sv = TimTheoMa(maSV);
            if (sv == null) return false;

            sv.DiemTrungBinh = diemMoi;
            return true;
        }

        public bool Xoa(string maSV)
        {
            var sv = TimTheoMa(maSV);
            if (sv == null) return false;
            return _danhSach.Remove(sv);
        }

        public List<SinhVien> SapXepTheoDiem()
        {
            return _danhSach.OrderByDescending(s => s.DiemTrungBinh).ToList();
        }

        public List<SinhVien> LocSinhVienDat()
        {
            return _danhSach.Where(s => s.DiemTrungBinh >= 5.0).ToList();
        }
    }
}