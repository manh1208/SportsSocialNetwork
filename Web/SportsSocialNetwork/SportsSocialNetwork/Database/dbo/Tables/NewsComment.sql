CREATE TABLE [dbo].[NewsComment] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [NewsId]     INT            NOT NULL,
    [Comment]    NVARCHAR (MAX) NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [Active]     BIT            NOT NULL,
    CONSTRAINT [PK_NewsComment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NewsComment_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_NewsComment_News] FOREIGN KEY ([NewsId]) REFERENCES [dbo].[News] ([Id])
);





