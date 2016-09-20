CREATE TABLE [dbo].[GroupMember] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [GroupId] INT            NOT NULL,
    [UserId]  NVARCHAR (128) NOT NULL,
    [Admin]   BIT            NOT NULL,
    [Status]  INT            NOT NULL,
    CONSTRAINT [PK_GroupMember] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GroupMember_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_GroupMember_User1] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

