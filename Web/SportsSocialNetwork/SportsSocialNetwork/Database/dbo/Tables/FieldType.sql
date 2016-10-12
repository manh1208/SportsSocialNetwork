CREATE TABLE [dbo].[FieldType] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (255) NULL,
    [SportId]     INT            NOT NULL,
    [Active]      BIT            NOT NULL,
    CONSTRAINT [PK_FieldType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FieldType_Sport] FOREIGN KEY ([SportId]) REFERENCES [dbo].[Sport] ([Id])
);



