CREATE TABLE [dbo].[FieldImage] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [FieldId] INT            NOT NULL,
    [Image]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_FieldImage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FieldImage_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[Field] ([Id])
);



