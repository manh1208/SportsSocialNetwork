CREATE TABLE [dbo].[TimeBlock] (
    [Id]        INT        IDENTITY (1, 1) NOT NULL,
    [StartTime] TIME (7)   NOT NULL,
    [EndTime]   TIME (7)   NOT NULL,
    [Price]     FLOAT (53) NOT NULL,
    [Active]    BIT        NOT NULL,
    CONSTRAINT [PK_TimeBlock] PRIMARY KEY CLUSTERED ([Id] ASC)
);

