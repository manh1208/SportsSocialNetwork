CREATE TABLE [dbo].[Post] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [CreateDate]  DATETIME       NOT NULL,
    [PostContent] NVARCHAR (MAX) NULL,
    [ContentType] INT            NULL,
    [EditDate]    DATETIME       NULL,
    [Active]      BIT            NOT NULL,
    [GroupId]     INT            NULL,
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Post_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Post_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id])
);











