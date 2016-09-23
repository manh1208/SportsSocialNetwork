CREATE TABLE [dbo].[UserInvitation] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [InvitationId] INT            NOT NULL,
    [ReciverId]    NVARCHAR (128) NOT NULL,
    [Accepted]     BIT            NOT NULL,
    [Active]       BIT            NOT NULL,
    CONSTRAINT [PK_UserInvitation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserInvitation_AspNetUsers] FOREIGN KEY ([ReciverId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserInvitation_Invitation] FOREIGN KEY ([InvitationId]) REFERENCES [dbo].[Invitation] ([Id])
);





