CREATE TABLE [dbo].[UserInvitation] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [InvitationId] INT            NOT NULL,
    [ReceiverId]   NVARCHAR (128) NOT NULL,
    [Accepted]     BIT            NULL,
    [Active]       BIT            NOT NULL,
    CONSTRAINT [PK_UserInvitation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserInvitation_AspNetUsers] FOREIGN KEY ([ReceiverId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserInvitation_Invitation] FOREIGN KEY ([InvitationId]) REFERENCES [dbo].[Invitation] ([Id])
);







