-- Tạo cơ sở dữ liệu
-- Create the database if it doesn't exist and use it
CREATE DATABASE IF NOT EXISTS s4d;

USE s4d;

-- Sử dụng cơ sở dữ liệu
-- Tạo bảng Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    Username VARCHAR(255),
    Password VARCHAR(255),
    Email VARCHAR(255),
    Role VARCHAR(50)
);

-- Tạo bảng Categories
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY,
    CategoryName VARCHAR(255)
);

-- Tạo bảng Artists
CREATE TABLE Artists (
    ArtistID INT PRIMARY KEY,
    ArtistName VARCHAR(255),
    Description TEXT
);

-- Tạo bảng Products
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductTitle VARCHAR(255),
    ReleaseDate DATE,
    CategoryID INT,
    Price DECIMAL(10, 2),
    ProductType VARCHAR(50),
	AlbumID int
);

-- Tạo bảng ProductDetails
CREATE TABLE ProductDetails (
    ProductDetailID INT PRIMARY KEY,
    ProductID INT,
    Duration INT,
    GameDetails TEXT,
    MovieDetails TEXT
);

-- Tạo bảng Providers
CREATE TABLE Providers (
    ProviderID INT PRIMARY KEY,
    ProviderName VARCHAR(255),
    ProviderType VARCHAR(50),
    ContactInfo TEXT
);

-- Tạo bảng Orders
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    UserID INT,
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2)
);

-- Tạo bảng OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY,
    OrderID INT,
    ProductDetailID INT,
    Quantity INT,
    Subtotal DECIMAL(10, 2)
);

-- Tạo bảng Reviews
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY,
    UserID INT,
    ProductDetailID INT,
    Rating INT,
    Comment TEXT,
    ReviewType VARCHAR(50),
    Date DATE
);

-- Tạo bảng UserCart
CREATE TABLE UserCart (
    CartID INT PRIMARY KEY,
    UserID INT,
    ProductDetailID INT
);

-- Tạo bảng Promotions
CREATE TABLE Promotions (
    PromotionID INT PRIMARY KEY,
    PromotionName VARCHAR(255),
    Description TEXT,
    StartDate DATE,
    EndDate DATE
);

-- Tạo bảng PromotionProducts
CREATE TABLE PromotionProducts (
    PromotionProductID INT PRIMARY KEY,
    PromotionID INT,
    ProductID INT
);

-- Tạo bảng PurchasingInvoices
CREATE TABLE PurchasingInvoices (
    InvoiceID INT PRIMARY KEY,
    ProviderID INT,
    InvoiceDate DATE,
    TotalAmount DECIMAL(10, 2)
);

-- Tạo bảng InvoiceDetails
CREATE TABLE InvoiceDetails (
    InvoiceDetailID INT PRIMARY KEY,
    InvoiceID INT,
    ProductDetailID INT,
    Quantity INT,
    UnitPrice DECIMAL(10, 2)
);

-- Tạo bảng OrdersHistory
CREATE TABLE OrdersHistory (
    HistoryID INT PRIMARY KEY,
    UserID INT,
    OrderID INT,
    OrderDate DATE
);

-- Tạo bảng Albums
CREATE TABLE Albums (
    AlbumID INT PRIMARY KEY,
    AlbumTitle VARCHAR(255),
    ReleaseDate DATE,
    ArtistID INT
);
-- Thêm các khóa ngoại

-- Users --> Orders
ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);

-- Products --> Categories
ALTER TABLE Products
ADD CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID);

-- ProductDetails --> Products
ALTER TABLE ProductDetails
ADD CONSTRAINT FK_ProductDetails_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID);

-- OrderDetails --> Orders, ProductDetails
ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID);

ALTER TABLE OrderDetails
ADD CONSTRAINT FK_OrderDetails_ProductDetails FOREIGN KEY (ProductDetailID) REFERENCES ProductDetails(ProductDetailID);

-- Reviews --> Users, ProductDetails
ALTER TABLE Reviews
ADD CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);

ALTER TABLE Reviews
ADD CONSTRAINT FK_Reviews_ProductDetails FOREIGN KEY (ProductDetailID) REFERENCES ProductDetails(ProductDetailID);

-- UserCart --> Users, ProductDetails
ALTER TABLE UserCart
ADD CONSTRAINT FK_UserCart_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);

ALTER TABLE UserCart
ADD CONSTRAINT FK_UserCart_ProductDetails FOREIGN KEY (ProductDetailID) REFERENCES ProductDetails(ProductDetailID);

-- PromotionProducts --> Promotions, Products
ALTER TABLE PromotionProducts
ADD CONSTRAINT FK_PromotionProducts_Promotions FOREIGN KEY (PromotionID) REFERENCES Promotions(PromotionID);

ALTER TABLE PromotionProducts
ADD CONSTRAINT FK_PromotionProducts_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID);

-- PurchasingInvoices --> Providers
ALTER TABLE PurchasingInvoices
ADD CONSTRAINT FK_PurchasingInvoices_Providers FOREIGN KEY (ProviderID) REFERENCES Providers(ProviderID);

-- InvoiceDetails --> PurchasingInvoices, ProductDetails
ALTER TABLE InvoiceDetails
ADD CONSTRAINT FK_InvoiceDetails_PurchasingInvoices FOREIGN KEY (InvoiceID) REFERENCES PurchasingInvoices(InvoiceID);

ALTER TABLE InvoiceDetails
ADD CONSTRAINT FK_InvoiceDetails_ProductDetails FOREIGN KEY (ProductDetailID) REFERENCES ProductDetails(ProductDetailID);

-- OrdersHistory --> Users, Orders
ALTER TABLE OrdersHistory
ADD CONSTRAINT FK_OrdersHistory_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);

ALTER TABLE OrdersHistory
ADD CONSTRAINT FK_OrdersHistory_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID);

ALTER TABLE Albums
ADD CONSTRAINT FK_Albums_Artists FOREIGN KEY (ArtistID) REFERENCES Artists(ArtistID);

ALTER TABLE Products
ADD CONSTRAINT FK_Products_Albums FOREIGN KEY (AlbumID) REFERENCES Albums(AlbumID);
