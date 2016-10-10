﻿CREATE TABLE [dbo].[Order] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [UserId]              NVARCHAR (128) NOT NULL,
    [FieldId]             INT            NOT NULL,
    [CreateDate]          DATETIME       NOT NULL,
    [StartTime]           DATETIME       NOT NULL,
    [EndTime]             DATETIME       NOT NULL,
    [Note]                NVARCHAR (MAX) NOT NULL,
    [Price]               FLOAT (53)     NOT NULL,
    [Status]              INT            NOT NULL,
    [PaidType]            INT            NULL,
    [Token]               NVARCHAR (MAX) NULL,
    [OrderCode]           NVARCHAR (MAX) NULL,
    [OnlinePaymentMethod] NVARCHAR (MAX) NULL,
    [BankCode]            NVARCHAR (MAX) NULL,
    [TransactionTime]     DATETIME       NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Order_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[Field] ([Id])
);







