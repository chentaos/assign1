IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201208193746_Initial')
BEGIN
    CREATE TABLE [Advertisements] (
        [Id] int NOT NULL IDENTITY,
        [CommunityID] nvarchar(max) NULL,
        [FileName] nvarchar(max) NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Advertisements] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201208193746_Initial')
BEGIN
    CREATE TABLE [Community] (
        [ID] nvarchar(450) NOT NULL,
        [Title] nvarchar(50) NOT NULL,
        [Budget] money NOT NULL,
        CONSTRAINT [PK_Community] PRIMARY KEY ([ID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201208193746_Initial')
BEGIN
    CREATE TABLE [Student] (
        [ID] int NOT NULL IDENTITY,
        [LastName] nvarchar(50) NOT NULL,
        [FirstName] nvarchar(50) NOT NULL,
        [EnrollmentDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Student] PRIMARY KEY ([ID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201208193746_Initial')
BEGIN
    CREATE TABLE [CommunityMembership] (
        [StudentID] int NOT NULL,
        [CommunityID] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_CommunityMembership] PRIMARY KEY ([StudentID], [CommunityID]),
        CONSTRAINT [FK_CommunityMembership_Community_CommunityID] FOREIGN KEY ([CommunityID]) REFERENCES [Community] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_CommunityMembership_Student_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [Student] ([ID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201208193746_Initial')
BEGIN
    CREATE INDEX [IX_CommunityMembership_CommunityID] ON [CommunityMembership] ([CommunityID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201208193746_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201208193746_Initial', N'3.1.9');
END;

GO

