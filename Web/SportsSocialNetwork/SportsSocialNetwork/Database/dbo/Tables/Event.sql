﻿CREATE TABLE [dbo].[Event] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        INT            NOT NULL,
    [CreatorId]   NVARCHAR (128) NOT NULL,
    [StartDate]   DATETIME       NOT NULL,
    [EndDate]     DATETIME       NOT NULL,
    [PlaceId]     INT            NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Image]       NVARCHAR (255) NOT NULL,
    [Status]      INT            NOT NULL,
    [Avtive]      BIT            NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_User] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[User] ([Id])
);

