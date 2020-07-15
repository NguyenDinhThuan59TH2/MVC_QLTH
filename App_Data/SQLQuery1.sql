drop database QLTapHoa
create database QLTapHoa
use QLTapHoa
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
	DaXoa bit not null,
)


create table MauHang(
	MaMH varchar(10) primary key not null,
	TenMH nvarchar(300) not null,
	DonVi nvarchar(100) not null,
	Anh nvarchar(100) not null,
	ChuThich nvarchar(max),
	DaXoa bit not null,
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
	DaXoa bit not null,
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
	DaXoa bit not null,
)

create table TaiKhoanKhachHang(
	MaKH varchar(10) not null,
	TaiKhoan varchar(100) not null,
	MatKhau varchar(100) not null,
	PRIMARY KEY (MaKH, TaiKhoan)
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

create table DonHangDat(
	MaDHD VARCHAR(10) primary key not null,
	MaKH VARCHAR(10)  not null,
	NgayDat DATETIME not null,
	TrangThai NVARCHAR(30) not null
)

create table HangDonHangDat(
	MaDHD VARCHAR(10) not null,
	MaH VARCHAR(10) not null,
	SoLuong int NOT NULL,
	PRIMARY KEY (MaDHD,MaH)
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
ALTER TABLE DonHangDat ADD CONSTRAINT FK_KhachHang_DonHangDat FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)

ALTER TABLE HangDonHangNhap ADD CONSTRAINT FK_DonHangXuat_HangDonHangNhap FOREIGN KEY (MaDHN) REFERENCES DonHangNhap(MaDHN)
ALTER TABLE HangDonHangNhap ADD CONSTRAINT FK_Hang_DonHangNhap FOREIGN KEY (MaH) REFERENCES Hang(MaH)

ALTER TABLE HangDonHangXuat ADD CONSTRAINT FK_DonHangXuat_HangDonHangXuat FOREIGN KEY (MaDHX) REFERENCES DonHangXuat(MaDHX)
ALTER TABLE HangDonHangXuat ADD CONSTRAINT FK_Hang_DonHangXuat FOREIGN KEY (MaH) REFERENCES Hang(MaH)

ALTER TABLE HangDonHangDat ADD CONSTRAINT FK_DonHangDat_HangDonHangDat FOREIGN KEY (MaDHD) REFERENCES DonHangDat(MaDHD)
ALTER TABLE HangDonHangDat ADD CONSTRAINT FK_Hang_DonHangDat FOREIGN KEY (MaH) REFERENCES Hang(MaH)

ALTER TABLE TaiKhoanKhachHang ADD CONSTRAINT FK_KhachHang_TaiKhoanKhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)

INSERT INTO NguoiDung VALUES
	('QL1','admin','$MYHASH$V1$10000$mb6sZl2WmoJcfPNeJ/PVKEhtLAKHUK/DppE1mhKkpfxCW7G/',N'Quản lý',N'Admin','999999','MacDinh.png',N'Dia chi','1', '0')

INSERT INTO MauHang VALUES
	('MH1',N'Oishi Snack Phô Mai 1',N'Gói',N'Default.jpg',N'Giai, giòn, ngon', '0'),
	('MH2',N'bánh quy Cosy Marie',N'Gói',N'banhquyCosyMarie.jpg',N'Có vị hơi ngọt và mặn', '0'),
	('MH3',N'Bia Heineken 24 Lon',N'Thùng',N'bia-heineken-thung-24-lon-330ml.jpg',N'Cồn 5.6 độ', '0'),
	('MH4',N'Đường trắng biên hòa 1KG',N'Gói',N'Duong-trang-BIen-Hoa-1kg.jpg',N'Đường tinh nguyên', '0'),
	('MH5',N'Nước mắm Nam Ngư 270ml',N'Chai',N'nuoc-mam-nam-ngu.jpg',N'Nước mắm mặn', '0'),
	('MH6',N'Xì dầu tam thái tử',N'Chai',N'nuoctuongtamthaitu.jpg',N'Rượu 39 độ', '0'),
	('MH7',N'Khuôn làm đá',N'cái',N'khuondaMini',N'khuôn đá nhựa', '0'),
	('MH8',N'Tiêu xay sẵn 100g',N'gói',N'tieuXay.jpg',N'hạt tiêu xay nhuyễn', '0'),
	('MH9',N'Kẹo oishi hương ổi',N'gói',N'Default.jpg',N'Kẹo thơm rẻ', '0'),
	('MH10',N'Bánh gạo One One',N'gói',N'banhgao.jpg',N'Bánh sản xuất trong nước', '0'),
	('MH11',N'Muỗng Inox',N'chục',N'muongInox.jpg',N'Muỗng Trung Quốc - rẻ', '0'),
	('MH12',N'Mì hảo hảo',N'thùng',N'haoHao.jpg',N'bán lẻ 3.5k 1 gói', '0'),
	('MH13',N'Mì Omachi sốt Spaghetti',N'thùng',N'omachiSpaghetti.jpg',N'Ngon', '0'),
	('MH14',N'Mì ký Vifon 1kg',N'Gói',N'miVifon1kg.png',N'Rẻ, hương vị ổn', '0'),
	('MH15',N'Rượu Vodka',N'Chai',N'vodka.png',N'Rượu 39 độ', '0'),
	('MH16',N'Bánh tráng nướng (chưa nướng)',N'banhDa.jpg',N'Default.jpg',N'Bánh tráng địa phương', '0'),
	('MH17',N'Nui Safoco 500g',N'Chai',N'nuiSafoco.jpg',N'ít bán được', '0'),
	('MH18',N'Bột ngọt Ajino-moto 200g',N'gói',N'botngotAjonomoto.jpg',N'Công nghệ nhật bản', '0'),
	('MH19',N'Hạt nêm Knorr gói 200g',N'',N'knorr.jpg',N'Dùng ít thôi, có hại', '0'),
	('MH20',N'Vở 200 trang',N'cái',N'vo200.jpg',N'Rẻ', '0'),
	('MH21',N'Ớt bột 100g',N'hũ',N'otBot.jpg',N'Cay gớm', '0'),
	('MH22',N'Muối iot 500g',N'gói',N'MuoiIot.jpg',N'Ủng hộ bà con làm muối, bán rẻ', '0'),
	('MH23',N'Sườn non chay 1kg',N'gói',N'suonNonChay.jpg',N'Đồ chay giá rẻ', '0'),
	('MH24',N'Gạo ST25 bao 5kg',N'Bao',N'gaoST25.jpg',N'Gạo ngon nhất thế giới ST25', '0'),
	('MH25',N'Tôm khô 200g',N'gói',N'tomKho200g.jpg',N'Hàng trôi nổi ngoài chợ', '0'),
	('MH26',N'Dầu đậu nành Simply 1L',N'Chai',N'dauDauNanhSimply.jpg',N'Đắt nhưng tốt', '0'),
	('MH27',N'Dầu thực vật cái lân 1L',N'Chai',N'dauCaiLan.jpg',N'Rẻ - dùng để chiên rồi đổ', '0'),
	('MH28',N'Dép tổ ong nhiều màu',N'Đôi 2 chiếc',N'depToOng.jpg',N'rẻ đẹp kém bền', '0'),
	('MH29',N'Chổi Đót',N'cái',N'choiDot.jpg',N'Độ bền trung bình', '0'),
	('MH30',N'Vá inox',N'cái',N'vaInox.jpg',N'Hàng Trung Quốc - rẻ', '0'),
	('MH31',N'Sữa cô gái hà lan - lốc 4 hộp',N'lốc',N'suahopCoGaiHaLan.jpg',N'Sữa Hộp nhập theo thùng, bán thùng và lẻ', '0')

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
	('NCC1',N'Thúy Hương',N'Việt Nam',N'Nha Trang,Khánh Hòa','0972111640','thuy@gmail.com', '0'),
	('NCC2',N'Lâm Bân',N'Thái Lan',N'Bangkok','0888186566','lamban@gmail.com', '0'),
	('NCC3',N'Bình Đường',N'Mexico',N'Urafe','0921219990','binhduong@gmail.com', '0'),
	('NCC4',N'Cảnh Hùng',N'Lào',N'Viêng Chăn','0375323640','canhhung@gmail.com', '0'),
	('NCC5',N'Đình Thuận',N'Việt Nam',N'Quảng Nam','0364321274','dinhthuan@gmail.com', '0')


insert into KhachHang values
	('KH1',N'Khách vãng lai',N'MacDinh.png',N'Nha Trang','9999999999','vanglai@gmail.com','1999/9/9','1', '0'),
	('KH2',N'Ngô Đường Quyền',N'person2.png',N'Nha Trang, Khánh Hòa','0888186566','duongquyen@gmail.com','1998/3/12','1', '0'),
	('KH3',N'Ngô Uyên Ương',N'person3.png',N'Đà lạt','0921219990','uongbi@gmail.com','1997/12/2','0', '0'),
	('KH4',N'Ngô Văn Giàu',N'person4.png',N'Cà Mau','0375323640','khagiau@gmail.com','1999/6/6','0', '0'),
	('KH5',N'Ngô Nguyễn Cát Tường',N'person5.png',N'Hà Nội','0364321274','cattuong@gmail.com','1999/3/2','1', '0')

insert into TaiKhoanKhachHang values
	('KH2', 'quyen.ngo', '$MYHASH$V1$10000$oqgMYomAysw1sF5JiXq9IXv7e2+0evXP6NFeTM+18Lfbepfh'),
	('KH3', 'uong.ngo', '$MYHASH$V1$10000$oqgMYomAysw1sF5JiXq9IXv7e2+0evXP6NFeTM+18Lfbepfh'),
	('KH4', 'giau.ngo', '$MYHASH$V1$10000$oqgMYomAysw1sF5JiXq9IXv7e2+0evXP6NFeTM+18Lfbepfh'),
	('KH5', 'tuong.ngo', '$MYHASH$V1$10000$oqgMYomAysw1sF5JiXq9IXv7e2+0evXP6NFeTM+18Lfbepfh')

insert into Hang values
	('H1','MH1','NCC1','2021/12/31','2010/5/8','5000','5','10000'),
	('H2','MH2','NCC2','2021/01/12','2020/5/8','20000','3','30000'),
	('H3','MH3','NCC3','2021/04/20','2020/5/8','10000','2','15000'),
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

insert into DonHangDat values
	('DHD1','KH2','2020/11/11',N'Đang đặt'),
	('DHD2','KH3','2020/11/11',N'Đang giao'),
	('DHD3','KH4','2020/11/11',N'Đã thanh toán')

insert into HangDonHangDat values
	('DHD1','H1','1'),
	('DHD2','H2','1'),
	('DHD3','H3','1')

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