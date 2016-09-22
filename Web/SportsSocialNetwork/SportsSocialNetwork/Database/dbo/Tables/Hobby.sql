CREATE TABLE [dbo].[Hobby] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [UserId]  NVARCHAR (128) NOT NULL,
    [SportId] INT            NOT NULL,
    CONSTRAINT [PK_Hobby] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Hobby_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Hobby_Sport] FOREIGN KEY ([SportId]) REFERENCES [dbo].[Sport] ([Id])
);



