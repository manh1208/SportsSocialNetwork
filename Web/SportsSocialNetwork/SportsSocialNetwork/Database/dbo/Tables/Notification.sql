CREATE TABLE [dbo].[Notification] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [UserId]       NVARCHAR (128) NOT NULL,
    [FromUserId]   NVARCHAR (128) NULL,
    [Title]        NVARCHAR (MAX) NOT NULL,
    [Message]      NVARCHAR (MAX) NOT NULL,
    [Type]         INT            NOT NULL,
    [PostId]       INT            NULL,
    [InvitationId] INT            NULL,
    [OrderId]      INT            NULL,
    [GroupId]      INT            NULL,
    [CreateDate]   DATETIME       NULL,
    [MarkRead]     BIT            NULL,
    [Active]       BIT            NOT NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notification_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Notification_AspNetUsers1] FOREIGN KEY ([FromUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Notification_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_Notification_Invitation] FOREIGN KEY ([InvitationId]) REFERENCES [dbo].[Invitation] ([Id]),
    CONSTRAINT [FK_Notification_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]),
    CONSTRAINT [FK_Notification_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([Id])
);



















