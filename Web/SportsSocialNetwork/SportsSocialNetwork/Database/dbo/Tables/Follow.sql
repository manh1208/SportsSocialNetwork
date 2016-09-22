CREATE TABLE [dbo].[Follow] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [FollowedId] NVARCHAR (128) NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_Follow] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Follow_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);



