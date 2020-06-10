IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        [Discriminator] nvarchar(max) NOT NULL,
        [FullName] nvarchar(max) NULL,
        [AvatarLink] nvarchar(300) NULL,
        [Status] bit NULL,
        [IsDeleted] bit NULL,
        [Salt] nvarchar(50) NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [Category] (
        [CategoryID] int NOT NULL IDENTITY,
        [Category] nvarchar(max) NULL,
        CONSTRAINT [PK_Category] PRIMARY KEY ([CategoryID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [City] (
        [CityID] nvarchar(450) NOT NULL,
        [city] nvarchar(max) NULL,
        [type] nvarchar(30) NULL,
        CONSTRAINT [PK_City] PRIMARY KEY ([CityID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [Coupon] (
        [CouponID] nvarchar(450) NOT NULL,
        [Discount] int NULL,
        [Quantity] int NULL,
        [QuantityUsed] int NULL,
        [Status] nvarchar(max) NULL,
        CONSTRAINT [PK_Coupon] PRIMARY KEY ([CouponID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [Publisher] (
        [PublisherID] int NOT NULL IDENTITY,
        [publisher] nvarchar(max) NULL,
        CONSTRAINT [PK_Publisher] PRIMARY KEY ([PublisherID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [District] (
        [DistrictID] nvarchar(450) NOT NULL,
        [district] nvarchar(max) NULL,
        [type] nvarchar(30) NULL,
        [CityID] nvarchar(450) NULL,
        [Fee] int NULL,
        CONSTRAINT [PK_District] PRIMARY KEY ([DistrictID]),
        CONSTRAINT [FK_District_City_CityID] FOREIGN KEY ([CityID]) REFERENCES [City] ([CityID]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [Book] (
        [BookID] int NOT NULL IDENTITY,
        [NameBook] nvarchar(200) NOT NULL,
        [CategoryID] int NOT NULL,
        [PublisherID] int NOT NULL,
        [Author] nvarchar(max) NULL,
        [Dimensions] nvarchar(max) NULL,
        [Format] nvarchar(max) NULL,
        [Date] datetime2 NOT NULL,
        [NumberOfPage] int NULL,
        [Infomation] nvarchar(max) NULL,
        [OriginalPrice] decimal(18,2) NULL,
        [Price] decimal(18,2) NULL,
        [ImageLink] nvarchar(300) NULL,
        [QuantityIn] int NULL,
        [QuantityOut] int NULL,
        [Status] bit NULL,
        [Weight] real NULL,
        CONSTRAINT [PK_Book] PRIMARY KEY ([BookID]),
        CONSTRAINT [FK_Book_Category_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [Category] ([CategoryID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Book_Publisher_PublisherID] FOREIGN KEY ([PublisherID]) REFERENCES [Publisher] ([PublisherID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [Recipient] (
        [RecipientID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [Phone] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [CityID] nvarchar(450) NULL,
        [DistrictID] nvarchar(450) NULL,
        CONSTRAINT [PK_Recipient] PRIMARY KEY ([RecipientID]),
        CONSTRAINT [FK_Recipient_City_CityID] FOREIGN KEY ([CityID]) REFERENCES [City] ([CityID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Recipient_District_DistrictID] FOREIGN KEY ([DistrictID]) REFERENCES [District] ([DistrictID]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [CartItems] (
        [ApplicationUserId] nvarchar(450) NOT NULL,
        [BookID] int NOT NULL,
        [Quantity] int NULL,
        [CreatedDate] datetime2 NULL,
        CONSTRAINT [PK_CartItems] PRIMARY KEY ([ApplicationUserId], [BookID]),
        CONSTRAINT [FK_CartItems_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CartItems_Book_BookID] FOREIGN KEY ([BookID]) REFERENCES [Book] ([BookID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [Reviews] (
        [BookID] int NOT NULL,
        [ApplicationUserId] nvarchar(450) NOT NULL,
        [Rating] int NULL,
        [Comment] nvarchar(max) NULL,
        [Date] datetime2 NOT NULL,
        CONSTRAINT [PK_Reviews] PRIMARY KEY ([ApplicationUserId], [BookID]),
        CONSTRAINT [FK_Reviews_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Reviews_Book_BookID] FOREIGN KEY ([BookID]) REFERENCES [Book] ([BookID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [WishList] (
        [ApplicationUserId] nvarchar(450) NOT NULL,
        [BookID] int NOT NULL,
        CONSTRAINT [PK_WishList] PRIMARY KEY ([ApplicationUserId], [BookID]),
        CONSTRAINT [FK_WishList_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_WishList_Book_BookID] FOREIGN KEY ([BookID]) REFERENCES [Book] ([BookID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [Orders] (
        [OrderID] int NOT NULL IDENTITY,
        [ApplicationUserID] nvarchar(450) NULL,
        [RecipientID] int NOT NULL,
        [Date] datetime2 NULL,
        [CouponID] nvarchar(450) NULL,
        [ShippingFee] decimal(18,2) NULL,
        [Status] nvarchar(max) NULL,
        [Note] nvarchar(max) NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderID]),
        CONSTRAINT [FK_Orders_AspNetUsers_ApplicationUserID] FOREIGN KEY ([ApplicationUserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Orders_Coupon_CouponID] FOREIGN KEY ([CouponID]) REFERENCES [Coupon] ([CouponID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Orders_Recipient_RecipientID] FOREIGN KEY ([RecipientID]) REFERENCES [Recipient] ([RecipientID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE TABLE [OrderItems] (
        [OrderID] int NOT NULL,
        [BookID] int NOT NULL,
        [Quantity] int NULL,
        [Price] decimal(18,2) NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([OrderID], [BookID]),
        CONSTRAINT [FK_OrderItems_Book_BookID] FOREIGN KEY ([BookID]) REFERENCES [Book] ([BookID]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderItems_Orders_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Orders] ([OrderID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Book_CategoryID] ON [Book] ([CategoryID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Book_PublisherID] ON [Book] ([PublisherID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_CartItems_BookID] ON [CartItems] ([BookID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_District_CityID] ON [District] ([CityID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_OrderItems_BookID] ON [OrderItems] ([BookID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Orders_ApplicationUserID] ON [Orders] ([ApplicationUserID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Orders_CouponID] ON [Orders] ([CouponID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Orders_RecipientID] ON [Orders] ([RecipientID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Recipient_CityID] ON [Recipient] ([CityID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Recipient_DistrictID] ON [Recipient] ([DistrictID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_Reviews_BookID] ON [Reviews] ([BookID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    CREATE INDEX [IX_WishList_BookID] ON [WishList] ([BookID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200603141027_InitialAzureDb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200603141027_InitialAzureDb', N'3.1.4');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200607160827_ChangeInformationInBookTable')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Book]') AND [c].[name] = N'Infomation');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Book] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Book] DROP COLUMN [Infomation];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200607160827_ChangeInformationInBookTable')
BEGIN
    ALTER TABLE [Book] ADD [Information] nvarchar(max) NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200607160827_ChangeInformationInBookTable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200607160827_ChangeInformationInBookTable', N'3.1.4');
END;

GO

