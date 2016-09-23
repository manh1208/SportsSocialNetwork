CREATE TABLE [dbo].[Sport] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (255) NOT NULL,
    [Active] BIT            NOT NULL,
    CONSTRAINT [PK_Sport] PRIMARY KEY CLUSTERED ([Id] ASC)
);



