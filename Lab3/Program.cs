using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Lab03_QuanLySinhVienOOP
{
    internal class Program
    {
        private static readonly QuanLySinhVien qlsv = new QuanLySinhVien();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            KhoiTaoDuLieuMau();

            while (true)
            {
                HienThiMenu();
                Console.Write("Chon chuc nang: ");
                string luaChon = Console.ReadLine()?.Trim();

                Console.WriteLine();
                switch (luaChon)
                {
                    case "1":
                        XuLyThem();
                        break;
                    case "2":
                        XuLyXuatDanhSach(qlsv.LayDanhSach(), "DANH SÁCH TOÀN BỘ SINH VIÊN");
                        break;
                    case "3":
                        XuLyTimTheoMa();
                        break;
                    case "4":
                        XuLyTimTheoTen();
                        break;
                    case "5":
                        XuLySuaDiem();
                        break;
                    case "6":
                        XuLyXoa();
                        break;
                    case "7":
                        XuLyXuatDanhSach(qlsv.SapXepTheoDiem(), "DANH SÁCH XẾP THEO ĐIỂM GIẢM DẦN");
                        break;
                    case "8":
                        XuLyXuatDanhSach(qlsv.LocSinhVienDat(), "DANH SÁCH SINH VIÊN ĐẠT (ĐTB >= 5.0)");
                        break;
                    case "0":
                        Console.WriteLine("Đã thoát chương trình. Tạm biệt!");
                        return;
                    default:
                        Console.WriteLine("⚠️ Lựa chọn không hợp lệ, vui lòng chọn lại!");
                        break;
                }

                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void HienThiMenu()
        {
            Console.WriteLine("===== QUAN LY SINH VIEN =====");
            Console.WriteLine("1. Them sinh vien");
            Console.WriteLine("2. Xuat danh sach");
            Console.WriteLine("3. Tim sinh vien theo ma");
            Console.WriteLine("4. Tim sinh vien theo ten");
            Console.WriteLine("5. Sua diem trung binh");
            Console.WriteLine("6. Xoa sinh vien");
            Console.WriteLine("7. Sap xep theo diem giam dan");
            Console.WriteLine("8. Loc sinh vien dat");
            Console.WriteLine("0. Thoat");
        }

        static void XuLyThem()
        {
            Console.WriteLine("--- THÊM SINH VIÊN ---");
            string maSV;
            while (true)
            {
                Console.Write("Nhập mã sinh viên: ");
                maSV = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(maSV))
                {
                    Console.WriteLine("Mã sinh viên không được để trống!");
                    continue;
                }
                if (qlsv.TimTheoMa(maSV) != null)
                {
                    Console.WriteLine("⚠️ Mã sinh viên đã tồn tại! Vui lòng nhập mã khác.");
                    continue;
                }
                break;
            }

            string hoTen;
            while (true)
            {
                Console.Write("Nhập họ tên: ");
                hoTen = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(hoTen)) break;
                Console.WriteLine("Họ tên không được để trống!");
            }

            DateTime ngaySinh = NhapNgaySinh();

            Console.Write("Nhập mã lớp: ");
            string maLop = Console.ReadLine()?.Trim();

            double diemTB = NhapDiemTrungBinh();

            try
            {
                SinhVien sv = new SinhVien(maSV, hoTen, ngaySinh, maLop, diemTB);
                if (qlsv.Them(sv))
                {
                    Console.WriteLine(" Thêm sinh viên thành công!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
            }
        }

        static void XuLyXuatDanhSach(List<SinhVien> danhSach, string tieuDe)
        {
            Console.WriteLine($"--- {tieuDe} ---");
            if (!danhSach.Any())
            {
                Console.WriteLine("Không có sinh viên nào trong danh sách.");
                return;
            }

            Console.WriteLine($"{"Mã SV",-8} | {"Họ tên",-20} | {"Ngày sinh",-10} | {"Lớp",-8} | {"ĐTB",-4} | {"Xếp loại",-10}");
            Console.WriteLine(new string('-', 72));

            foreach (var sv in danhSach)
            {
                Console.WriteLine($"{sv.MaSinhVien,-8} | {sv.HoTen,-20} | {sv.NgaySinh:dd/MM/yyyy} | {sv.MaLop,-8} | {sv.DiemTrungBinh,4:F1} | {sv.XepLoai(),-10}");
            }
        }

        static void XuLyTimTheoMa()
        {
            Console.Write("Nhập mã sinh viên cần tìm: ");
            string maSV = Console.ReadLine()?.Trim();

            var sv = qlsv.TimTheoMa(maSV);
            if (sv != null)
            {
                Console.WriteLine("\n Thông tin sinh viên tìm thấy:");
                Console.WriteLine(sv.LayThongTin());
            }
            else
            {
                Console.WriteLine($"❌ Không tìm thấy sinh viên có mã '{maSV}'.");
            }
        }

        static void XuLyTimTheoTen()
        {
            Console.Write("Nhập từ khóa họ tên cần tìm: ");
            string tuKhoa = Console.ReadLine()?.Trim();

            var ketQua = qlsv.TimTheoTen(tuKhoa);
            XuLyXuatDanhSach(ketQua, $"KẾT QUẢ TÌM THEO TỪ KHÓA \"{tuKhoa}\"");
        }

        static void XuLySuaDiem()
        {
            Console.Write("Nhập mã sinh viên cần sửa điểm: ");
            string maSV = Console.ReadLine()?.Trim();

            var sv = qlsv.TimTheoMa(maSV);
            if (sv == null)
            {
                Console.WriteLine($"❌ Không tìm thấy sinh viên có mã '{maSV}'.");
                return;
            }

            Console.WriteLine($"Đang sửa điểm cho: {sv.HoTen} (Điểm hiện tại: {sv.DiemTrungBinh:F1})");
            double diemMoi = NhapDiemTrungBinh();

            if (qlsv.Sua(maSV, diemMoi))
            {
                Console.WriteLine(" Cập nhật điểm trung bình thành công!");
            }
        }

        static void XuLyXoa()
        {
            Console.Write("Nhập mã sinh viên muốn xóa: ");
            string maSV = Console.ReadLine()?.Trim();

            if (qlsv.Xoa(maSV))
            {
                Console.WriteLine($" Đã xóa thành công sinh viên có mã '{maSV}'.");
            }
            else
            {
                Console.WriteLine($"❌ Không tìm thấy sinh viên có mã '{maSV}' để xóa.");
            }
        }

        static DateTime NhapNgaySinh()
        {
            DateTime ngaySinh;
            while (true)
            {
                Console.Write("Nhập ngày sinh (dd/MM/yyyy): ");
                string input = Console.ReadLine()?.Trim();
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngaySinh))
                {
                    return ngaySinh;
                }
                Console.WriteLine("⚠️ Ngày sinh sai định dạng (ví dụ đúng: 20/11/2004), vui lòng nhập lại!");
            }
        }

        static double NhapDiemTrungBinh()
        {
            double diem;
            while (true)
            {
                Console.Write("Nhập điểm trung bình (0 - 10): ");
                string input = Console.ReadLine()?.Trim();
                if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out diem) ||
                    double.TryParse(input, out diem))
                {
                    if (diem >= 0 && diem <= 10) return diem;
                }
                Console.WriteLine("⚠️ Điểm không hợp lệ! Điểm phải là số thực từ 0 đến 10.");
            }
        }

        static void KhoiTaoDuLieuMau()
        {
            qlsv.Them(new SinhVien("SV001", "Nguyễn Văn A", new DateTime(2004, 3, 12), "CNTT01", 8.2));
            qlsv.Them(new SinhVien("SV002", "Trần Thị B", new DateTime(2004, 7, 25), "CNTT02", 4.5));
            qlsv.Them(new SinhVien("SV003", "Lê Hoàng C", new DateTime(2003, 11, 5), "CNTT01", 6.8));
            qlsv.Them(new SinhVien("SV004", "Nguyễn Thị D", new DateTime(2004, 1, 18), "CNTT02", 9.0));
        }
    }
}