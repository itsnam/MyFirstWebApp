use master
go
if exists(select * from sysdatabases where name = 'littlemilktea')
	drop database littlemilktea
go
create database littlemilktea
go
use littlemilktea
go
if exists(select * from sysobjects where name = 'nhanvien')
	drop table nhanvien
go
create table nhanvien(
	id int not null primary key identity,
	name nvarchar(50) not null,
	age int not null,
	email nvarchar(100) not null unique,
	phone varchar(10) not null,
	job nvarchar(30) not null,
	create_at datetime not null default current_timestamp
)

if exists(select * from sysobjects where name = 'account')
	drop table account
go
create table account(
	username char(30) primary key,
	password char(30) not null,
	role char(30) not null
)

if exists(select * from sysobjects where name = 'trasua')
	drop table trasua
go
create table trasua(
	id int not null primary key identity,
	img nvarchar(50) not null,
	name nvarchar(50) not null,
	cost int not null,
	create_at datetime not null default current_timestamp
)

if exists(select * from sysobjects where name = 'cart')
	drop table cart
go
create table cart (
    itemId int not null primary key identity,
	itemImage nvarchar(50) not null,
	itemName nvarchar(50) not null,
	itemCost int not null,
	quantity int not null
);

if exists(select * from sysobjects where name = 'salecart')
	drop table salecart
go
create table salecart (
    itemId int not null primary key identity,
	itemImage nvarchar(50) not null,
	itemName nvarchar(50) not null,
	itemCost int not null,
	quantity int not null
);

if exists(select * from sysobjects where name = 'shippercart')
	drop table shippercart
go
create table shippercart (
    itemId int not null primary key identity,
	itemImage nvarchar(50) not null,
	itemName nvarchar(50) not null,
	itemCost int not null,
	quantity int not null
);

if exists(select * from sysobjects where name = 'thongtindonhang')
	drop table thongtindonhang
go
create table thongtindonhang (
	name nvarchar(50),	
	address nvarchar(50),
	phonenumber varchar(10)
);

if exists(select * from sysobjects where name = 'thongtinbanhang')
	drop table thongtinbanhang
go
create table thongtinbanhang (
	name nvarchar(50),	
	address nvarchar(50),
	phonenumber varchar(10)
);

if exists(select * from sysobjects where name = 'thongtingiaohang')
	drop table thongtingiaohang
go
create table thongtingiaohang (
	name nvarchar(50),	
	address nvarchar(50),
	phonenumber varchar(10)
);

insert into nhanvien(name, age, email, phone, job) values
(N'Nguyễn Hoài Nam', 21, 'nam@gmail.com', '0123456789', N'Quản lý trà sữa'),
(N'Nguyễn Thị Thu', 22, 'thu@gmail.com', '0456789123', N'Quản lý nhân sự'),
(N'Nguyễn Thị Hoa', 19, 'hoa@gmail.com', '0456123789', N'Nhân viên bán hàng'),
(N'Nguyễn Văn Thành', 20, 'thanh@gmail.com', '0789123456', N'Nhân viên giao hàng')

insert into account(username, password, role) values
('qltrasua', '123456', 'qltrasua'), ('qlnhansu', '123456', 'qlnhansu'), 
('nhanvien', '123456', 'nhanvien'), ('shipper', '123456', 'shipper'), ('user123', '123456', 'user')


insert into trasua(img, name, cost) values 
('trasuatruyenthong.jpg', N'Trà sữa truyền thống', 25000),
('trasuatraxanh.jpg', N'Trà sữa trà xanh', 30000),
('trasuaoolong.jpg', N'Trà sữa ô lông', 35000),
('trasuasocola.jpg', N'Trà sữa sô cô la', 30000),
('trasuadau.jpg', N'Trà sữa dâu', 35000),
('trasuacheesefoam.jpg', N'Trà sữa cheese foam', 40000)
