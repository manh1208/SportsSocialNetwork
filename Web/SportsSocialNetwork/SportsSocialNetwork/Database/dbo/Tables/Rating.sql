CREATE TABLE [dbo].[Rating] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [UserId]  NVARCHAR (128) NOT NULL,
    [PlaceId] INT            NULL,
    [Point]   INT            NOT NULL,
    CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Rating_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Rating_Place] FOREIGN KEY ([PlaceId]) REFERENCES [dbo].[Place] ([Id])
);



