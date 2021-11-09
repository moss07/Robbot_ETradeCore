
SET IDENTITY_INSERT [dbo].[Categories] ON 
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Computer')
INSERT [dbo].[Categories] ([Id], [Name], [Description]) VALUES (2, N'Home Entertainment', 'Home theater systems and TVs.')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [ExpirationDate]) VALUES (1, N'Laptop', 3000.0000, 10, 1, '2020-12-13')
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [ExpirationDate], [Description]) VALUES (2, N'Mouse', 20.0000, 20, 1, '2021-11-23', 'Computer peripherals.')
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [Description]) VALUES (3, N'Keyboard', 40.0000, 21, 1, 'Computer peripherals.')
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId]) VALUES (18, N'Speaker', 2500.0000, 5, 2)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId]) VALUES (19, N'Receiver', 5000.0000, 9, 2)
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [Description]) VALUES (23, N'Monitor', 2500.0000, 27, 1, 'Computer peripherals.')
INSERT [dbo].[Products] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId]) VALUES (24, N'Equalizer', 1000.0000, 11, 2)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
