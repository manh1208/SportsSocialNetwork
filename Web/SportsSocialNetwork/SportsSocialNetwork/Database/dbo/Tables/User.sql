CREATE TABLE [dbo].[User] (
    [Id]          NVARCHAR (128) NOT NULL,
    [City]        NVARCHAR (255) NOT NULL,
    [District]    NVARCHAR (255) NOT NULL,
    [Ward]        NVARCHAR (255) NOT NULL,
    [Address]     NVARCHAR (255) NOT NULL,
    [AvatarImage] NVARCHAR (255) NULL,
    [CoverImage]  NVARCHAR (255) NOT NULL,
    [Birthday]    DATE           NOT NULL,
    [Gender]      INT            NOT NULL,
    [Active]      BIT            NOT NULL,
    [RoleId]      INT            NOT NULL,
    [CreateDate]  DATETIME       NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);

