CREATE TABLE [dbo].[FieldPrice] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [FieldId]     INT NOT NULL,
    [TimeBlockId] INT NOT NULL,
    CONSTRAINT [PK_FieldPrice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FieldPrice_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[Field] ([Id]),
    CONSTRAINT [FK_FieldPrice_TimeBlock] FOREIGN KEY ([TimeBlockId]) REFERENCES [dbo].[TimeBlock] ([Id])
);

