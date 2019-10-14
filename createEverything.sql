CREATE TABLE [Component] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [name] nvarchar(255),
  [cost] money
)
GO

CREATE TABLE [Product] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [name] nvarchar(255)
)
GO

CREATE TABLE [ProductComponent] (
  [productId] int,
  [componentId] int,
  [quantity] int,
  PRIMARY KEY (productId, componentId)
)
GO

CREATE TABLE [Customer] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [firstName] nvarchar(255),
  [middleName] nvarchar(255),
  [lastName] nvarchar(255),
  [phoneNumber] nvarchar(255)
)
GO

CREATE TABLE [Location] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [address] nvarchar(255)
)
GO

CREATE TABLE [Inventory] (
  [locationId] int,
  [componentId] int,
  [quantity] int,
  PRIMARY KEY (locationId, componentId)
)
GO

CREATE TABLE [Order] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [orderDate] datetime DEFAULT 'GETDATE()',
  [locationId] int,
  [customerId] int
)
GO

CREATE TABLE [OrderDetails] (
  [orderId] int,
  [productId] int,
  [quantity] int,
  PRIMARY KEY (orderId, productId) 
)
GO

ALTER TABLE [ProductComponent] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [ProductComponent] ADD FOREIGN KEY ([componentId]) REFERENCES [Component] ([id])
GO

ALTER TABLE [Inventory] ADD FOREIGN KEY ([locationId]) REFERENCES [Location] ([id])
GO

ALTER TABLE [Inventory] ADD FOREIGN KEY ([componentId]) REFERENCES [Component] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([locationId]) REFERENCES [Location] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([customerId]) REFERENCES [Customer] ([id])
GO

ALTER TABLE [OrderDetails] ADD FOREIGN KEY ([orderId]) REFERENCES [Order] ([id])
GO

ALTER TABLE [OrderDetails] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO
