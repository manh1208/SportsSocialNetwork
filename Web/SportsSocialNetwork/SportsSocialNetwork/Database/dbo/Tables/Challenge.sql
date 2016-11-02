CREATE TABLE [dbo].[Challenge] (
    [Id]          INT            NOT NULL,
    [FromGroup]   INT            NOT NULL,
    [ToGroup]     INT            NOT NULL,
    [Accepted]    BIT            NULL,
    [Status]      INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Active]      BIT            NOT NULL,
    CONSTRAINT [PK_Challenge] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Challenge_Group] FOREIGN KEY ([FromGroup]) REFERENCES [dbo].[Group] ([Id]),
    CONSTRAINT [FK_Challenge_Group1] FOREIGN KEY ([ToGroup]) REFERENCES [dbo].[Group] ([Id])
);



