CREATE TABLE [dbo].[Field] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [PlaceId]     INT            NOT NULL,
    [Name]        INT            NOT NULL,
    [Status]      INT            NOT NULL,
    [FieldTypeId] INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Active]      BIT            NOT NULL,
    CONSTRAINT [PK_Field] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Field_FieldType] FOREIGN KEY ([FieldTypeId]) REFERENCES [dbo].[FieldType] ([Id]),
    CONSTRAINT [FK_Field_Place] FOREIGN KEY ([PlaceId]) REFERENCES [dbo].[Place] ([Id])
);

