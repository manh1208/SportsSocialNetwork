CREATE TABLE [dbo].[Invitation] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [SenderId]          NVARCHAR (128) NOT NULL,
    [InvitationContent] NVARCHAR (MAX) NOT NULL,
    [CreateDate]        INT            NOT NULL,
    [Active]            BIT            NOT NULL,
    CONSTRAINT [PK_Invitation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Invitation_AspNetUsers] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);



