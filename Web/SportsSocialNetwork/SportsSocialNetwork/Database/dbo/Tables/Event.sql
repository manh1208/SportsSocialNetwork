CREATE TABLE [dbo].[Event] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [CreatorId]   NVARCHAR (128) NOT NULL,
    [StartDate]   DATETIME       NOT NULL,
    [EndDate]     DATETIME       NOT NULL,
    [PlaceId]     INT            NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Image]       NVARCHAR (255) NOT NULL,
    [Status]      INT            NOT NULL,
    [Active]      BIT            NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_AspNetUsers] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Event_Place] FOREIGN KEY ([PlaceId]) REFERENCES [dbo].[Place] ([Id])
);







