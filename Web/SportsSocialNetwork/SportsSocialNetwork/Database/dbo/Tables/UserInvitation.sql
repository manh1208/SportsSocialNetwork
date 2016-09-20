CREATE TABLE [dbo].[UserInvitation] (
    [Id]           INT            NOT NULL,
    [InvitationId] INT            NOT NULL,
    [ReciverId]    NVARCHAR (128) NOT NULL,
    [Accept]       BIT            NULL,
    CONSTRAINT [PK_UserInvitation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserInvitation_Invitation] FOREIGN KEY ([InvitationId]) REFERENCES [dbo].[Invitation] ([Id]),
    CONSTRAINT [FK_UserInvitation_User] FOREIGN KEY ([ReciverId]) REFERENCES [dbo].[User] ([Id])
);

