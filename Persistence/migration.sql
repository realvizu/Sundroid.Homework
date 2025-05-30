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
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    CREATE TABLE [DataLoggers] (
        [Id] int NOT NULL IDENTITY,
        [SerialNumber] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_DataLoggers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    CREATE TABLE [Inverters] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [SerialNumber] nvarchar(100) NOT NULL,
        [DataLoggerId] int NOT NULL,
        CONSTRAINT [PK_Inverters] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Inverters_DataLoggers_DataLoggerId] FOREIGN KEY ([DataLoggerId]) REFERENCES [DataLoggers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    CREATE TABLE [LogItems] (
        [Id] int NOT NULL IDENTITY,
        [InverterId] int NOT NULL,
        [Time] datetimeoffset NOT NULL,
        [Upv1] decimal(18,4) NOT NULL,
        [Upv2] decimal(18,4) NOT NULL,
        [Upv3] decimal(18,4) NOT NULL,
        [Upv4] decimal(18,4) NOT NULL,
        [Upv5] decimal(18,4) NOT NULL,
        [Upv6] decimal(18,4) NOT NULL,
        [Upv7] decimal(18,4) NOT NULL,
        [Upv8] decimal(18,4) NOT NULL,
        [Ipv1] decimal(18,4) NOT NULL,
        [Ipv2] decimal(18,4) NOT NULL,
        [Ipv3] decimal(18,4) NOT NULL,
        [Ipv4] decimal(18,4) NOT NULL,
        [Ipv5] decimal(18,4) NOT NULL,
        [Ipv6] decimal(18,4) NOT NULL,
        [Ipv7] decimal(18,4) NOT NULL,
        [Ipv8] decimal(18,4) NOT NULL,
        [Uac1] decimal(18,4) NOT NULL,
        [Uac2] decimal(18,4) NOT NULL,
        [Uac3] decimal(18,4) NOT NULL,
        [Iac1] decimal(18,4) NOT NULL,
        [Iac2] decimal(18,4) NOT NULL,
        [Iac3] decimal(18,4) NOT NULL,
        [Status] int NOT NULL,
        [Error] int NOT NULL,
        [Temp] decimal(18,4) NOT NULL,
        [Cos] decimal(18,4) NOT NULL,
        [Fac] decimal(18,4) NOT NULL,
        [Pac] decimal(18,4) NOT NULL,
        [Qac] decimal(18,4) NOT NULL,
        [Eac] decimal(18,4) NOT NULL,
        [EDay] decimal(18,4) NOT NULL,
        [ETotal] decimal(18,4) NOT NULL,
        [CycleTime] int NOT NULL,
        [UpdatedAtUtc] datetimeoffset NOT NULL,
        CONSTRAINT [PK_LogItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_LogItems_Inverters_InverterId] FOREIGN KEY ([InverterId]) REFERENCES [Inverters] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DataLoggers_SerialNumber] ON [DataLoggers] ([SerialNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    CREATE INDEX [IX_Inverters_DataLoggerId] ON [Inverters] ([DataLoggerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Inverters_SerialNumber] ON [Inverters] ([SerialNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_LogItems_InverterId_Time] ON [LogItems] ([InverterId], [Time]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250530123451_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250530123451_Initial', N'8.0.16');
END;
GO

COMMIT;
GO

