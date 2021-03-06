﻿CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [FullName]             NVARCHAR (255) NULL,
    [Address]              NVARCHAR (255) NULL,
    [City]                 NVARCHAR (255) NULL,
    [District]             NVARCHAR (255) NULL,
    [Ward]                 NVARCHAR (255) NULL,
    [Latitude]             FLOAT (53)     NULL,
    [Longitude]            FLOAT (53)     NULL,
    [AvatarImage]          NVARCHAR (255) NULL,
    [CoverImage]           NVARCHAR (255) NULL,
    [Birthday]             DATE           NULL,
    [Gender]               INT            NULL,
    [NganLuongAccount]     NVARCHAR (MAX) NULL,
    [CreateDate]           DATETIME       NULL,
    [Status]               INT            NULL,
    [Active]               BIT            NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);
















GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);

