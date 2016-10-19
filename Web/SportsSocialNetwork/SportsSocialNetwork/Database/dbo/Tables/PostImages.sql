CREATE TABLE [dbo].[PostImages] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [PostId] INT            NOT NULL,
    [Image]  NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PostImages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PostImages_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([Id])
);

