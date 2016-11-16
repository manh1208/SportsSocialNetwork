CREATE TABLE [dbo].[Post] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [UserId]                NVARCHAR (128) NOT NULL,
    [CreateDate]            DATETIME       NOT NULL,
    [PostContent]           NVARCHAR (MAX) NULL,
    [ContentType]           INT            NULL,
    [PostId]                INT            NULL,
    [EventId]               INT            NULL,
    [NewsId]                INT            NULL,
    [OrderId]               INT            NULL,
    [EditDate]              DATETIME       NULL,
    [LatestInteractionTime] DATETIME       NULL,
    [Active]                BIT            NOT NULL,
    [ProfileId]             NVARCHAR (128) NULL,
    [GroupId]               INT            NULL,
    CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event_Post] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event] ([Id]),
    CONSTRAINT [FK_News_Post] FOREIGN KEY ([NewsId]) REFERENCES [dbo].[News] ([Id]),
    CONSTRAINT [FK_Order_Post] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]),
    CONSTRAINT [FK_Post_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Post_AspNetUsers1] FOREIGN KEY ([ProfileId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Post_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_Post_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([Id])
);



















