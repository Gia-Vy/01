CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,     -- ID t? t?ng
    Username NVARCHAR(50) NOT NULL,           -- Tên ??ng nh?p
    PasswordHash NVARCHAR(255) NOT NULL,      -- M?t kh?u ?ã hash
    Email NVARCHAR(100) NULL,                 -- Email
    CreatedDate DATETIME DEFAULT GETDATE(),   -- Ngày t?o
    Role NVARCHAR(50) NULL,                   -- Vai trò: Admin/User
    LastLogin DATETIME NULL,                  -- L?n ??ng nh?p cu?i
    CreatedBy NVARCHAR(50) DEFAULT 'admin'   -- Ng??i t?o
);
