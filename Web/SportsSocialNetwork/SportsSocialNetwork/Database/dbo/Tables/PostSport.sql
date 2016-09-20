CREATE TABLE [dbo].[PostSport] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [PostId]  INT NOT NULL,
    [SportId] INT NOT NULL,
    CONSTRAINT [PK_PostSport] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PostSport_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([Id]),
    CONSTRAINT [FK_PostSport_Sport] FOREIGN KEY ([SportId]) REFERENCES [dbo].[Sport] ([Id])
);

