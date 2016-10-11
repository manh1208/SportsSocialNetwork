CREATE TABLE [dbo].[PlaceImage] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [PlaceId] INT            NOT NULL,
    [Image]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PlaceImage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PlaceImage_Place] FOREIGN KEY ([PlaceId]) REFERENCES [dbo].[Place] ([Id])
);



