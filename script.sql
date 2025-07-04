IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Customers] (
    [CustomerID] int NOT NULL IDENTITY,
    [CustomerFullName] nvarchar(50) NOT NULL,
    [Telephone] nvarchar(12) NOT NULL,
    [EmailAddress] nvarchar(50) NOT NULL,
    [CustomerBirthday] date NULL,
    [CustomerStatus] tinyint NULL,
    [Password] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerID])
);

CREATE TABLE [roomTypes] (
    [RoomTypeID] int NOT NULL IDENTITY,
    [RoomTypeName] nvarchar(50) NOT NULL,
    [TypeDescription] nvarchar(250) NULL,
    [TypeNote] nvarchar(max) NULL,
    CONSTRAINT [PK_roomTypes] PRIMARY KEY ([RoomTypeID])
);

CREATE TABLE [BookingReservations] (
    [BookingReservationID] int NOT NULL IDENTITY,
    [BookingDate] date NULL,
    [TotalPrice] decimal(18,2) NULL,
    [BookingStatus] tinyint NULL,
    [CustomerID] int NOT NULL,
    CONSTRAINT [PK_BookingReservations] PRIMARY KEY ([BookingReservationID]),
    CONSTRAINT [FK_BookingReservations_Customers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [Customers] ([CustomerID]) ON DELETE CASCADE
);

CREATE TABLE [RoomInformations] (
    [RoomID] int NOT NULL IDENTITY,
    [RoomNumber] nvarchar(50) NOT NULL,
    [RoomDetailDescription] nvarchar(max) NULL,
    [RoomMaxCapacity] int NULL,
    [RoomStatus] tinyint NULL,
    [RoomPricePerDay] decimal(18,2) NULL,
    [RoomTypeID] int NOT NULL,
    CONSTRAINT [PK_RoomInformations] PRIMARY KEY ([RoomID]),
    CONSTRAINT [FK_RoomInformations_roomTypes_RoomTypeID] FOREIGN KEY ([RoomTypeID]) REFERENCES [roomTypes] ([RoomTypeID]) ON DELETE CASCADE
);

CREATE TABLE [BookingDetails] (
    [BookingReservationID] int NOT NULL,
    [RoomID] int NOT NULL,
    [StartDate] date NOT NULL,
    [EndDate] date NOT NULL,
    [ActualPrice] decimal(18,2) NULL,
    CONSTRAINT [PK_BookingDetails] PRIMARY KEY ([BookingReservationID], [RoomID]),
    CONSTRAINT [FK_BookingDetails_BookingReservations_BookingReservationID] FOREIGN KEY ([BookingReservationID]) REFERENCES [BookingReservations] ([BookingReservationID]) ON DELETE CASCADE,
    CONSTRAINT [FK_BookingDetails_RoomInformations_RoomID] FOREIGN KEY ([RoomID]) REFERENCES [RoomInformations] ([RoomID]) ON DELETE CASCADE
);

CREATE INDEX [IX_BookingDetails_RoomID] ON [BookingDetails] ([RoomID]);

CREATE INDEX [IX_BookingReservations_CustomerID] ON [BookingReservations] ([CustomerID]);

CREATE INDEX [IX_RoomInformations_RoomTypeID] ON [RoomInformations] ([RoomTypeID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250704094602_InitialCreate', N'9.0.6');

COMMIT;
GO

