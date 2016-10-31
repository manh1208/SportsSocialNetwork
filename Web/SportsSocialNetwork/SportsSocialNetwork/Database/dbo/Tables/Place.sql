CREATE TABLE [dbo].[Place] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [UserId]              NVARCHAR (128) NOT NULL,
    [Name]                NVARCHAR (50)  NOT NULL,
    [Email]               VARCHAR (255)  NOT NULL,
    [City]                NVARCHAR (255) NOT NULL,
    [District]            NVARCHAR (255) NOT NULL,
    [Ward]                NVARCHAR (255) NOT NULL,
    [Address]             NVARCHAR (255) NOT NULL,
    [PhoneNumber]         VARCHAR (20)   NOT NULL,
    [Description]         NVARCHAR (MAX) NOT NULL,
    [Latitude]            FLOAT (53)     NULL,
    [Longitude]           FLOAT (53)     NULL,
    [Avatar]              NVARCHAR (255) NOT NULL,
    [AcceptPaymentOnline] BIT            NULL,
    [Status]              INT            NOT NULL,
    [Active]              BIT            NOT NULL,
    [StartTime]           TIME (7)       NOT NULL,
    [EndTime]             TIME (7)       NOT NULL,
    [Approve]             BIT            NOT NULL,
    CONSTRAINT [PK_Place] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Place_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);











