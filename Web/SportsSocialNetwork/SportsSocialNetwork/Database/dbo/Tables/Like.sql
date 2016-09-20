CREATE TABLE [dbo].[Like] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [PostId]     INT            NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_Like] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Like_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([Id]),
    CONSTRAINT [FK_Like_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

