CREATE DATABASE IF NOT EXISTS s4d;

USE s4d;

-- Tạo bảng Users
CREATE TABLE IF NOT EXISTS Users (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    Status bit
);

-- Tạo bảng Categories
CREATE TABLE IF NOT EXISTS Categories (
    CategoryID INT PRIMARY KEY AUTO_INCREMENT,
    CategoryName VARCHAR(255) NOT NULL
);

-- Tạo bảng Artists
CREATE TABLE IF NOT EXISTS Artists (
    ArtistID INT PRIMARY KEY AUTO_INCREMENT,
    ArtistName VARCHAR(255) NOT NULL,
    Description TEXT
);

-- Tạo bảng Albums
CREATE TABLE IF NOT EXISTS Albums (
    AlbumID INT PRIMARY KEY AUTO_INCREMENT,
    AlbumTitle VARCHAR(255) NOT NULL,
    ReleaseDate DATE,
    ArtistID INT
);

-- Tạo bảng Products
CREATE TABLE IF NOT EXISTS Products (
    ProductID INT PRIMARY KEY AUTO_INCREMENT,
    ProductName VARCHAR(255) NOT NULL,
    ReleaseDate DATE,
    CategoryID INT,
    Price DECIMAL(10, 2),
    AlbumID INT,
    ProviderName VARCHAR(255),
    Duration INT,
    Image VARCHAR(255)
);

-- Tạo bảng Orders
CREATE TABLE IF NOT EXISTS Orders (
    OrderID INT PRIMARY KEY AUTO_INCREMENT,
    UserID INT,
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2)
);

-- Tạo bảng OrderDetails
CREATE TABLE IF NOT EXISTS OrderDetails (
    OrderDetailID INT PRIMARY KEY AUTO_INCREMENT,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    Subtotal DECIMAL(10, 2)
);

-- Tạo bảng Reviews
CREATE TABLE IF NOT EXISTS Reviews (
    ReviewID INT PRIMARY KEY AUTO_INCREMENT,
    UserID INT,
    ProductID INT,
    Rating INT,
    Comment TEXT,
    Date DATE
);

-- Tạo bảng Promotions
CREATE TABLE IF NOT EXISTS Promotions (
    PromotionID INT PRIMARY KEY AUTO_INCREMENT,
    PromotionName VARCHAR(255) NOT NULL,
    Description TEXT,
    StartDate DATE,
    EndDate DATE
);

-- Tạo bảng PromotionProducts
CREATE TABLE IF NOT EXISTS PromotionProducts (
    PromotionProductID INT PRIMARY KEY AUTO_INCREMENT,
    PromotionID INT,
    ProductID INT
);

-- Thêm ràng buộc khóa ngoại
ALTER TABLE Albums
ADD FOREIGN KEY (ArtistID) REFERENCES Artists(ArtistID);

ALTER TABLE Products
ADD FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
ADD FOREIGN KEY (AlbumID) REFERENCES Albums(AlbumID);

ALTER TABLE Orders
ADD FOREIGN KEY (UserID) REFERENCES Users(UserID);

ALTER TABLE OrderDetails
ADD FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
ADD FOREIGN KEY (ProductID) REFERENCES Products(ProductID);

ALTER TABLE Reviews
ADD FOREIGN KEY (UserID) REFERENCES Users(UserID),
ADD FOREIGN KEY (ProductID) REFERENCES Products(ProductID);

ALTER TABLE PromotionProducts
ADD FOREIGN KEY (PromotionID) REFERENCES Promotions(PromotionID),
ADD FOREIGN KEY (ProductID) REFERENCES Products(ProductID);
