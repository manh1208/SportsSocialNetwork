CREATE TABLE [dbo].[CategorySport] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [CategoryId] INT NOT NULL,
    [SportId]    INT NOT NULL,
    CONSTRAINT [PK_CategorySport] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CategorySport_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_CategorySport_Sport] FOREIGN KEY ([SportId]) REFERENCES [dbo].[Sport] ([Id])
);

