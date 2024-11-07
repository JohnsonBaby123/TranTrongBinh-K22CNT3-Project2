CREATE DATABASE BanHangDB;
USE BanHangDB;

CREATE TABLE KhachHang (
    MaKhachHang INT PRIMARY KEY,
    Ho VARCHAR(50) NOT NULL,
    Ten VARCHAR(50) NOT NULL,
    Email VARCHAR(100),
    SoDienThoai VARCHAR(20),
    DiaChi TEXT
);

CREATE TABLE SanPham (
    MaSanPham INT PRIMARY KEY,
    TenSanPham VARCHAR(100) NOT NULL,
    DanhMuc VARCHAR(50),
    Gia DECIMAL(10, 2) NOT NULL,
    TonKho INT DEFAULT 0
);

CREATE TABLE DonHang (
    MaDonHang INT PRIMARY KEY,
    MaKhachHang INT NOT NULL,
    NgayDat DATETIME DEFAULT CURRENT_TIMESTAMP,
    TongTien DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang)
);

CREATE TABLE ChiTietDonHang (
    MaChiTietDonHang INT PRIMARY KEY,
    MaDonHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL,
    Gia DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

CREATE TABLE ThanhToan (
    MaThanhToan INT PRIMARY KEY,
    MaDonHang INT NOT NULL,
    PhuongThucThanhToan VARCHAR(50) NOT NULL,
    SoTienThanhToan DECIMAL(10, 2) NOT NULL,
    NgayThanhToan DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)
);
Select * from KhachHang
Select * from SanPham
Select * from DonHang
Select * from ChiTietDonHang
Select * from ThanhToan


INSERT INTO KhachHang (MaKhachHang, Ho, Ten, Email, SoDienThoai, DiaChi) VALUES
(1,'Nguyễn', 'Hoài Nam', 'tuanh@example.com', '0987654321', 'Hải Dương, Việt Nam'),
(2,'Trần', 'Trọng Bình', 'mailan@example.com', '0912345678', 'Hà Nội, Việt Nam');

INSERT INTO SanPham (MaSanPham, TenSanPham, DanhMuc, Gia, TonKho) VALUES
(1,'GSX R150', 'Suzuki', 15000.00, 10),
(2,'R15M', 'Yamaha', 12000.00, 7),
(3,'CBR 150R', 'Honda', 17000.00, 8),
(4,'Ninja 125', 'Kawasaki', 19000.00, 9),
(5,'Rebel 150', 'Honda', 14000.00, 8);

-- Bảng DonHang
INSERT INTO DonHang (MaDonHang, MaKhachHang, TongTien) VALUES
(1120,1, 15000.00),
(1121,2, 12000.00);

-- Bảng ChiTietDonHang
INSERT INTO ChiTietDonHang (MaChiTietDonHang,MaDonHang, MaSanPham, SoLuong, Gia) VALUES
(0010,1120, 1, 1, 15000.00),
(0011,1121, 2, 1, 12000.00);

-- Bảng ThanhToan
INSERT INTO ThanhToan (MaThanhToan,MaDonHang, PhuongThucThanhToan, SoTienThanhToan) VALUES
(0010,1120, 'Chuyển khoản ngân hàng', 15000.00),
(0011,1121, 'Thanh toán khi nhận hàng', 12000.00);
