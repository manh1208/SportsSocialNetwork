CREATE TABLE [dbo].[FieldSchedule] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [FieldId]     INT            NOT NULL,
    [StartTime]   DATETIME       NOT NULL,
    [EndTime]     DATETIME       NOT NULL,
    [Type]        INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Active]      BIT            NOT NULL,
    CONSTRAINT [PK_FieldSchedule] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FieldSchedule_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[Field] ([Id])
);



