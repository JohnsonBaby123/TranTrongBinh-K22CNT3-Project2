CREATE DATABASE BanHangDB;
USE BanHangDB;

CREATE TABLE KhachHang (
    MaKhachHang INT PRIMARY KEY IDENTITY(1,1),
    Ho VARCHAR(50) NOT NULL,
    Ten VARCHAR(50) NOT NULL,
    Email VARCHAR(100),
    SoDienThoai VARCHAR(20),
    DiaChi TEXT
);


CREATE TABLE SanPham (
    MaSanPham INT PRIMARY KEY IDENTITY(1,1),
    TenSanPham VARCHAR(100) NOT NULL,
    DanhMuc VARCHAR(50),
    Gia DECIMAL(10, 2) NOT NULL,
    TonKho INT DEFAULT 0
);

CREATE TABLE DonHang (
    MaDonHang INT PRIMARY KEY IDENTITY(1,1),
    MaKhachHang INT NOT NULL,
    NgayDat DATETIME DEFAULT CURRENT_TIMESTAMP,
    TongTien DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang)
);

CREATE TABLE ChiTietDonHang (
    MaChiTietDonHang INT PRIMARY KEY IDENTITY(1,1),
    MaDonHang INT NOT NULL,
    MaSanPham INT NOT NULL,
    SoLuong INT NOT NULL,
    Gia DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
    FOREIGN KEY (MaSanPham) REFERENCES SanPham(MaSanPham)
);

CREATE TABLE ThanhToan (
    MaThanhToan INT PRIMARY KEY IDENTITY(1,1),
    MaDonHang INT NOT NULL,
    PhuongThucThanhToan VARCHAR(50) NOT NULL,
    SoTienThanhToan DECIMAL(10, 2) NOT NULL,
    NgayThanhToan DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)
);

Select*from SanPham
Drop table SanPham
Drop table DonHang
Drop table ChiTietDonHang
Drop table ThanhToan



INSERT INTO SanPham (TenSanPham, DanhMuc, Gia, TonKho) VALUES
('GSX R150', 'Suzuki', 15000.00, 10),
('R15M', 'Yamaha', 12000.00, 7),
('CBR 150R', 'Honda', 17000.00, 8),
('Ninja 125', 'Kawasaki', 19000.00, 9),
('Rebel 150', 'Honda', 14000.00, 8),
('TNT 150i', 'Benelli', 13000.00, 10);


