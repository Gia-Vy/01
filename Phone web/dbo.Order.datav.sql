SET IDENTITY_INSERT [dbo].[Order] ON
INSERT INTO [dbo].[Order] ([OrderID], [CusPhone], [AddressDelivery], [TotalValue], [PaymentMethod], [PaymentStatus], [OrderStatus], [OrderDate]) VALUES (33, N'0393110888', N'134 tân sơn nhì, , ', CAST(27950.00 AS Decimal(18, 2)), N'COD', N'Chưa thanh toán', N'Đang xử lý', N'2025-11-30 11:11:12')
INSERT INTO [dbo].[Order] ([OrderID], [CusPhone], [AddressDelivery], [TotalValue], [PaymentMethod], [PaymentStatus], [OrderStatus], [OrderDate]) VALUES (34, N'0948172056', N'134 tân sơn nhì, , ', CAST(2345.00 AS Decimal(18, 2)), N'COD', N'Chưa thanh toán', N'Đang xử lý', N'2025-11-30 11:24:41')
SET IDENTITY_INSERT [dbo].[Order] OFF
