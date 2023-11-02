-- Tạo cơ sở dữ liệu
CREATE DATABASE s4d;


USE s4d;
GO

-- Tạo bảng Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    Status bit,
    Address NVARCHAR(255),
    PhoneNumber NVARCHAR(20)
);

-- Tạo bảng Categories
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(255) NOT NULL
);

-- Tạo bảng Artists
CREATE TABLE Artists (
    ArtistID INT PRIMARY KEY IDENTITY(1,1),
    ArtistName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX)
);

-- Tạo bảng Albums
CREATE TABLE Albums (
    AlbumID INT PRIMARY KEY IDENTITY(1,1),
    AlbumTitle NVARCHAR(255) NOT NULL,
    ReleaseDate DATE,
    ArtistID INT,
);

-- Tạo bảng Products
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(255) NOT NULL,
    ReleaseDate DATE,
    CategoryID INT,
    Price DECIMAL(10, 2),
    AlbumID INT,
    ProviderName NVARCHAR(255),
    Duration INT,
    Image NVARCHAR(255),
);

-- Tạo bảng Orders
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2),
);

-- Tạo bảng OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    ProductID INT,
    Quantity INT,
    Subtotal DECIMAL(10, 2),
);

-- Tạo bảng Reviews
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    ProductID INT,
    Rating INT,
    Comment NVARCHAR(MAX),
    Date DATE,
);

-- Tạo bảng Promotions
CREATE TABLE Promotions (
    PromotionID INT PRIMARY KEY IDENTITY(1,1),
    PromotionName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    StartDate DATE,
    EndDate DATE,
	DiscountPercentage bit
);

-- Tạo bảng PromotionProducts
CREATE TABLE PromotionProducts (
    PromotionProductID INT PRIMARY KEY IDENTITY(1,1),
    PromotionID INT,
    ProductID INT,
);

-- Thêm ràng buộc khóa ngoại cho bảng Albums
ALTER TABLE Albums
ADD CONSTRAINT FK_Albums_Artists
FOREIGN KEY (ArtistID)
REFERENCES Artists (ArtistID);

-- Thêm ràng buộc khóa ngoại cho bảng Products
ALTER TABLE Products
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (CategoryID)
REFERENCES Categories (CategoryID);

ALTER TABLE Products
ADD CONSTRAINT FK_Products_Albums
FOREIGN KEY (AlbumID)
REFERENCES Albums (AlbumID);

-- Thêm ràng buộc khóa ngoại cho bảng Orders
ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_Users
FOREIGN KEY (UserID)
REFERENCES Users (UserID);

-- Thêm ràng buộc khóa ngoại cho bảng OrderDetails
ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_Orders
FOREIGN KEY (OrderID)
REFERENCES Orders (OrderID);

ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_Products
FOREIGN KEY (ProductID)
REFERENCES Products (ProductID);

-- Thêm ràng buộc khóa ngoại cho bảng Reviews
ALTER TABLE Reviews
ADD CONSTRAINT FK_Reviews_Users
FOREIGN KEY (UserID)
REFERENCES Users (UserID);

ALTER TABLE Reviews
ADD CONSTRAINT FK_Reviews_Products
FOREIGN KEY (ProductID)
REFERENCES Products (ProductID);

-- Thêm ràng buộc khóa ngoại cho bảng PromotionProducts
ALTER TABLE PromotionProducts
ADD CONSTRAINT FK_PromotionProducts_Promotions
FOREIGN KEY (PromotionID)
REFERENCES Promotions (PromotionID);

ALTER TABLE PromotionProducts
ADD CONSTRAINT FK_PromotionProducts_Products
FOREIGN KEY (ProductID)
REFERENCES Products (ProductID);
