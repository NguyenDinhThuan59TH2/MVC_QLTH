create database QLTapHoa
go 
use QLTapHoa
go


create table NguoiDung(
	MaND varchar(10) primary key not null,
	TaiKhoan varchar(100) not null,
	MatKhau varchar(100) not null,
	ChucVu nvarchar(100) not null,
	HoTen nvarchar(300) not null, 
	SDT varchar(20) ,
	Anh nvarchar(100) not null,
	DiaChi nvarchar(max) ,
	GioiTinh bit not null,

)


create table MauHang(
	MaMH varchar(10) primary key not null,
	TenMH nvarchar(300) not null,
	DonVi nvarchar(100) not null,
	Anh nvarchar(100) not null,
	ChuThich nvarchar(max) ,
)

create table NhomHang(
	MaNH varchar(10) primary key not null,
	TenNH nvarchar(300) not null,
)

create table MauHangNhomHang(
	MaMH VARCHAR(10) NOT NULL, 
	MaNH VARCHAR(10) NOT NULL,
	PRIMARY KEY(MaMH, MaNH)
)

CREATE TABLE NhaCungCap(
	MaNCC VARCHAR(10) PRIMARY KEY not null,
	TenNCC NVARCHAR(100) NOT NULL,
	QuoGia NVARCHAR(100) NOT NULL,
	DiaChi NVARCHAR(max) NOT NULL,
	SDT VARCHAR(20) NOT NULL,
	Email VARCHAR(20) NOT NULL,
)

create table KhachHang(
	MaKH varchar(10) primary key not null,
	HoTen nvarchar(100) not null,
	Anh nvarchar(100) not null,
	DiaChi nvarchar(max) not null,
	SDT VARCHAR(20) NOT NULL,
	Email VARCHAR(20) NOT NULL,
	NgaySinh DATE not null,
	GioiTinh bit not null,
)

create table Hang(
	MaH VARCHAR(10) PRIMARY KEY not null,
	MaMH VARCHAR(10) NOT NULL,
	MaNCC VARCHAR(10) not null,
	HanSuDung DATETIME NOT NULL,
	NgayNhap DATETIME NOT NULL,
	GiaNhap money NOT NULL,
	SoLuong int NOT NULL,
	GiaBan money not null,
	--DonViTinh NVARCHAR(20) NOT NULL,

)

create table DonHangNhap(
	MaDHN VARCHAR(10) PRIMARY KEY not null,
	MaNCC VARCHAR(10) not null,
	NgayNhap DATETIME not null,
	GiamGia varchar(10),
	KieuGiamGia Nvarchar(100),
)


 
create table DonHangXuat(
	MaDHX VARCHAR(10) primary key not null,
	MaKH VARCHAR(10)  not null,
	NgayXuat DATETIME not null,
	GiamGia varchar(10),
	KieuGiamGia Nvarchar(100),
)


create table HangDonHangNhap(
	MaDHN VARCHAR(10) not null,
	MaH VARCHAR(10) not null,
	SoLuong int NOT NULL,
	PRIMARY KEY (MaDHN,MaH)
)

create table HangDonHangXuat(
	MaDHX VARCHAR(10)  not null,
	MaH VARCHAR(10) not null,
	SoLuong int NOT NULL,
	PRIMARY KEY (MaDHX,MaH)
)

--ADD CONSTRANIT FOREIGN KEY

ALTER TABLE MauHangNhomHang ADD CONSTRAINT FK_MauHang_MauHangNhomHang FOREIGN KEY (MaMH) REFERENCES MauHang(MaMH)
ALTER TABLE MauHangNhomHang ADD CONSTRAINT FK_NhomHang_MauHangNhomHang FOREIGN KEY (MaNH) REFERENCES NhomHang(MaNH)

ALTER TABLE Hang ADD CONSTRAINT FK_MauHang_Hang FOREIGN KEY (MaMH) REFERENCES MauHang(MaMH)
ALTER TABLE Hang ADD CONSTRAINT FK_NhaCungCap_Hang FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)

ALTER TABLE DonHangNhap ADD CONSTRAINT FK_NhaCungCap_DonHangNhap FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)
ALTER TABLE DonHangXuat ADD CONSTRAINT FK_KhachHang_DonHangXuat FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)

ALTER TABLE HangDonHangNhap ADD CONSTRAINT FK_DonHangXuat_HangDonHangNhap FOREIGN KEY (MaDHN) REFERENCES DonHangNhap(MaDHN)
ALTER TABLE HangDonHangNhap ADD CONSTRAINT FK_Hang_DonHangNhap FOREIGN KEY (MaH) REFERENCES Hang(MaH)

ALTER TABLE HangDonHangXuat ADD CONSTRAINT FK_DonHangXuat_HangDonHangXuat FOREIGN KEY (MaDHX) REFERENCES DonHangXuat(MaDHX)
ALTER TABLE HangDonHangXuat ADD CONSTRAINT FK_Hang_DonHangXuat FOREIGN KEY (MaH) REFERENCES Hang(MaH)

INSERT INTO NguoiDung VALUES
	('QL1','admin','$MYHASH$V1$10000$mb6sZl2WmoJcfPNeJ/PVKEhtLAKHUK/DppE1mhKkpfxCW7G/',N'Quản lý',N'Admin','999999','MacDinh.png',N'Dia chi','1')

INSERT INTO MauHang VALUES
	('MH1',N'Oishi Snack Phô Mai 1',N'Gói',N'Default.jpg',N'Khong chu thich'),
	('MH2',N'bánh quy Cosy Marie',N'Gói',N'banhquyCosyMarie.jpg',N'Khong chu thich'),
	('MH3',N'Bia Heineken 24 Lon',N'Thùng',N'bia-heineken-thung-24-lon-330ml.jpg',N'Khong chu thich'),
	('MH4',N'Đường trắng biên hòa 1KG',N'Gói',N'Duong-trang-BIen-Hoa-1kg.jpg',N'Khong chu thich'),
	('MH5',N'Nước mắm Nam Ngư 270ml',N'Chai',N'nuoc-mam-nam-ngu.jpg',N'Khong chu thich'),
	('MH6',N'Rượu cúng',N'Chai',N'Default.jpg',N'Khong chu thich')


Insert into NhomHang Values
	('NH1',N'Bánh Kẹo'),
	('NH2',N'Bia Rượu'),
	('NH3',N'Gia Dụng'),
	('NH4',N'Vàng Mã'),
	('NH5',N'Nước Giải Khát')



Insert into MauHangNhomHang values
	('MH1','NH1'),
	('MH2','NH2'),
	('MH3','NH3'),
	('MH4','NH4'),
	('MH5','NH5')


insert into NhaCungCap values
	('NCC1',N'Thúy Hương',N'Việt Nam',N'Nha Trang,Khánh Hòa','0972111640','thuy@gmail.com'),
	('NCC2',N'Lâm Bân',N'Thái Lan',N'Bang koc','0888186566','Ncc2@gmail.com'),
	('NCC3',N'Bình Đường',N'Mexico',N'Urafe','0921219990','Ncc3@gmail.com'),
	('NCC4',N'Cảnh Hùng',N'Lào',N'LeLeLe','0375323640','Ncc3@gmail.com'),
	('NCC5',N'Đình Thuận',N'Việt Nam',N'Nha Trang,Khánh Hòa','0364321274','Ncc5@gmail.com')


insert into KhachHang values
	('KH1',N'Ngô Nguyễn Tường Nghi',N'MacDinh.png',N'Diên Khánh, Khánh Hòa','0972111640','Nghi@gmail.com','1999/3/2','1'),
	('KH2',N'Ngô Bá Khá',N'MacDinh.png',N'Nha Trang, Khánh Hòa','0888186566','Kha@gmail.com','1998/3/12','1'),
	('KH3',N'Ngô Uyên Ương',N'MacDinh.png',N'Đà lạt','0921219990','Uong@gmail.com','1997/12/2','0'),
	('KH4',N'Ngô Văn Giàu',N'MacDinh.png',N'Cà Mau','0375323640','Giau@gmail.com','1999/6/6','0'),
	('KH5',N'Ngô Nguyễn Cát Tường',N'MacDinh.png',N'Hà Nội','0364321274','Tuong@gmail.com','1999/3/2','1')


insert into Hang values
	('H1','MH1','NCC1','2021/12/31','2010/5/8','5000','10','10000'),
	('H2','MH2','NCC2','2021/01/12','2020/5/8','20000','10','30000'),
	('H3','MH3','NCC3','2021/04/20','2020/5/8','10000','10','15000'),
	('H4','MH2','NCC4','2021/12/12','2020/5/8','120000','10','130000'),
	('H5','MH5','NCC5','2021/05/21','2020/5/8','300000','10','300000'),
	('H6','MH4','NCC5','2021/09/12','2020/5/8','300000','10','330000')


insert into DonHangNhap values 
	('DHN1','NCC1','2020/8/12','10',N'%'),
	('DHN2','NCC2','2020/8/12','10000',N'VNĐ'),
	('DHN3','NCC3','2020/8/12','20000',N'VNĐ'),
	('DHN4','NCC4','2020/8/12','15',N'%'),
	('DHN5','NCC5','2020/8/12','25',N'%')


insert into DonHangXuat values 
	('DHX1','KH1','2020/11/11','10',N'%'),
	('DHX2','KH2','2020/11/11','10000',N'VNĐ'),
	('DHX3','KH3','2020/11/11','20000',N'VNĐ'),
	('DHX4','KH4','2020/11/11','15',N'%'),
	('DHX5','KH5','2020/11/11','25',N'%')

insert into HangDonHangNhap values
	('DHN1','H1','10'),
	('DHN2','H2','10'),
	('DHN3','H3','10'),
	('DHN4','H4','10'),
	('DHN5','H5','10')

insert into HangDonHangXuat values
	('DHX1','H1','5'),
	('DHX2','H2','6'),
	('DHX3','H3','7'),
	('DHX4','H4','8'),
	('DHX5','H5','9')

select * from KhachHang
select * from NhaCungCap


CREATE PROCEDURE Hang_TimKiem
    @HanSuDung DATETIME='',
	@NgayNhap DATETIME='',
	@GiaNhap money= NULL,
	@SoLuong int=NULL,
	@GiaBan money=NULL,

AS
BEGIN
DECLARE @SqlStr NVARCHAR(4000),
		@ParamList nvarchar(2000)
SELECT @SqlStr = '
       SELECT * 
       FROM Hang
       WHERE  (1=1)
       '
IF @HanSuDung IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
              AND (HanSuDung LIKE ''%'+@HanSuDung+'%'')
              '
IF @HoTen IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
              AND (HoNV+'' ''+TenNV LIKE ''%'+@HoTen+'%'')
              '
IF @GioiTinh IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
             AND (GioiTinh LIKE ''%'+@GioiTinh+'%'')
             '
IF @LuongMin IS NOT NULL and @LuongMax IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
             AND (Luong Between Convert(int,'''+@LuongMin+''') AND Convert(int, '''+@LuongMax+'''))
             '
IF @DiaChi IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
              AND (DiaChi LIKE ''%'+@DiaChi+'%'')
              '
IF @MaPB IS NOT NULL
       SELECT @SqlStr = @SqlStr + '
              AND (MaPB LIKE ''%'+@MaPB+'%'')
              '
	EXEC SP_EXECUTESQL @SqlStr
END