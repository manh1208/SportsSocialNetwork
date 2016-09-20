CREATE TABLE [dbo].[Order] (
    [Id]         INT            NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [FieldId]    INT            NOT NULL,
    [CreateDate] DATETIME       NOT NULL,
    [StartTime]  DATETIME       NOT NULL,
    [EndTime]    DATETIME       NOT NULL,
    [Note]       NVARCHAR (255) NULL,
    [Price]      FLOAT (53)     NOT NULL,
    [Status]     INT            NOT NULL,
    [PaidType]   INT            NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[Field] ([Id]),
    CONSTRAINT [FK_Order_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

