CREATE TABLE [dbo].[Participation] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EventId]     INT            NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [Type]        INT            NOT NULL,
    [TeamName]    NVARCHAR (255) NULL,
    [Slogan]      NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Participation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Participation_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Participation_Event] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id])
);







