CREATE TABLE [dbo].[FirebaseToken] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [UserId] NVARCHAR (128) NOT NULL,
    [Token]  NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_FirebaseToken] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FirebaseToken_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);



