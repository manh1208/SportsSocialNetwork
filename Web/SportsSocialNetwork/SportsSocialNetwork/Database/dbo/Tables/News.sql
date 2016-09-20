CREATE TABLE [dbo].[News] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [CreateDate]  DATETIME       NOT NULL,
    [Title]       NVARCHAR (255) NOT NULL,
    [NewsContent] NVARCHAR (MAX) NOT NULL,
    [Image]       NVARCHAR (255) NOT NULL,
    [CategoryId]  INT            NOT NULL,
    [Active]      BIT            NULL,
    CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_News_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_News_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

