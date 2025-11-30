
-- 1️ Bảng User
CREATE TABLE [User] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED ,
    [username] NVARCHAR(50) UNIQUE NOT NULL,
    [fullname] NVARCHAR(100),
    [email] NVARCHAR(100),
    [password] NVARCHAR(100) NOT NULL,
    [phone] NVARCHAR(20),
    [role] NVARCHAR(20) CHECK ([role] IN ('user', 'admin')) DEFAULT 'user',
    [created_at] DATETIME DEFAULT GETDATE()--hàm lưu thời gian ngay lúc tạo
);


-- 2️ Bảng Customer
CREATE TABLE [Customer] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [userID] INT,
    [name] NVARCHAR(100),
    [address] NVARCHAR(255),
    [phone_number] NVARCHAR(20),
    FOREIGN KEY ([userID]) REFERENCES [User]([id])
);


-- 3️ Bảng Category
CREATE TABLE [Category] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [name] NVARCHAR(100) NOT NULL
);


-- 4️ Bảng SubCategory
CREATE TABLE [SubCategory] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [name] NVARCHAR(100) NOT NULL,
    [catID] INT,
    FOREIGN KEY ([catID]) REFERENCES [Category]([id])
);


-- 5️ Bảng Color
CREATE TABLE [Color] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [name] NVARCHAR(50)
);


-- 6️ Bảng DungLuong
CREATE TABLE [DungLuong] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [name] NVARCHAR(50)
);


-- 7️ Bảng Product
CREATE TABLE [Product] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [name] NVARCHAR(150) NOT NULL,
    [subCatID] INT,
    FOREIGN KEY ([subCatID]) REFERENCES [SubCategory]([id])
);


-- 8️ Bảng ProductDetails
CREATE TABLE [ProductDetails] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [productID] INT,
    [colorID] INT NULL,
    [dungLuongID] INT NULL,
    [soldQuantity] INT DEFAULT 0,
    [remainQuantity] INT DEFAULT 0,
    [price] DECIMAL(18,2),
    [discount] DECIMAL(5,2) DEFAULT 0,
    FOREIGN KEY ([productID]) REFERENCES [Product]([id]),
    FOREIGN KEY ([colorID]) REFERENCES [Color]([id]),
    FOREIGN KEY ([dungLuongID]) REFERENCES [DungLuong]([id])
);


-- 9️  Image
CREATE TABLE [Image] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [productDetailID] INT,
    [url] NVARCHAR(255),
    FOREIGN KEY ([productDetailID]) REFERENCES [ProductDetails]([id])
);


-- 🔟 Bảng Order
CREATE TABLE [Order] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [customerID] INT,
    [orderDate] DATETIME DEFAULT GETDATE(),
    [total] DECIMAL(18,2),
    [deliveryAddress] NVARCHAR(255),
    [paymentMethod] NVARCHAR(50),
    [status] NVARCHAR(50) DEFAULT N'Đang xử lý',
    FOREIGN KEY ([customerID]) REFERENCES [Customer]([id])
);


-- 1️1 Bảng OrderDetails
CREATE TABLE [OrderDetails] (
    [id] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    [orderID] INT,
    [productDetailsID] INT,
    [quantity] INT,
    [unitPrice] DECIMAL(18,2),
    [discount] DECIMAL(5,2) DEFAULT 0,
    FOREIGN KEY ([orderID]) REFERENCES [Order]([id]),
    FOREIGN KEY ([productDetailsID]) REFERENCES [ProductDetails]([id])
);
GO
ALTER AUTHORIZATION ON DATABASE::Phoneweb TO sa;
