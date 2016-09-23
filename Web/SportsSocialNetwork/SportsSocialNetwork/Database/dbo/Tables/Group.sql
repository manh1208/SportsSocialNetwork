﻿CREATE TABLE [dbo].[Group] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [SportId]     INT            NOT NULL,
    [CoverImage]  NVARCHAR (255) NULL,
    [Active]      BIT            NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Group_Sport] FOREIGN KEY ([SportId]) REFERENCES [dbo].[Sport] ([Id])
);


