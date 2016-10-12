CREATE TABLE [dbo].[News] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [CreateDate]  DATETIME       NOT NULL,
    [Title]       NVARCHAR (255) NOT NULL,
    [NewsContent] NVARCHAR (MAX) NOT NULL,
    [Image]       NVARCHAR (MAX) NOT NULL,
    [CategoryId]  INT            NOT NULL,
    [Active]      BIT            NOT NULL,
    CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_News_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_News_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id])
);







