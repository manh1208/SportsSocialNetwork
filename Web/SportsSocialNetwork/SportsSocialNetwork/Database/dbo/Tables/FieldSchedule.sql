CREATE TABLE [dbo].[FieldSchedule] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [FieldId]      INT            NOT NULL,
    [UserId]       NVARCHAR (128) NULL,
    [StartDate]    DATE           NOT NULL,
    [EndDate]      DATE           NOT NULL,
    [StartTime]    TIME (7)       NOT NULL,
    [EndTime]      TIME (7)       NOT NULL,
    [AvailableDay] INT            NOT NULL,
    [Type]         INT            NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Active]       BIT            NOT NULL,
    CONSTRAINT [PK_FieldSchedule] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FieldSchedule_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_FieldSchedule_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[Field] ([Id])
);









