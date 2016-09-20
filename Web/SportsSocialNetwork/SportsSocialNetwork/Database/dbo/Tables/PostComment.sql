CREATE TABLE [dbo].[PostComment] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [PostId]     INT            NOT NULL,
    [Comment]    NVARCHAR (MAX) NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [Image]      VARCHAR (255)  NOT NULL,
    CONSTRAINT [PK_PostComment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PostComment_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([Id]),
    CONSTRAINT [FK_PostComment_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

