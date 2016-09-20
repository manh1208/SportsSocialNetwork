CREATE TABLE [dbo].[Participation] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [EventId]  INT            NOT NULL,
    [UserId]   NVARCHAR (128) NOT NULL,
    [Type]     INT            NOT NULL,
    [TeamName] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Participation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Participation_Event] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]),
    CONSTRAINT [FK_Participation_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

