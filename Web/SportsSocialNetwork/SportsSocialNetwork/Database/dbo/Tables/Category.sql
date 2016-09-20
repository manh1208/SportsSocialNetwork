CREATE TABLE [dbo].[Category] (
    [Id]     INT            NOT NULL,
    [Name]   NVARCHAR (255) NOT NULL,
    [Active] BIT            NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

