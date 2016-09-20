CREATE TABLE [dbo].[Notification] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [UserId]       NVARCHAR (128) NOT NULL,
    [Title]        NVARCHAR (255) NOT NULL,
    [Message]      NVARCHAR (255) NOT NULL,
    [Type]         INT            NOT NULL,
    [PostId]       INT            NOT NULL,
    [InvitationId] INT            NOT NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notification_Invitation] FOREIGN KEY ([InvitationId]) REFERENCES [dbo].[Invitation] ([Id]),
    CONSTRAINT [FK_Notification_Post] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([Id]),
    CONSTRAINT [FK_Notification_User1] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

