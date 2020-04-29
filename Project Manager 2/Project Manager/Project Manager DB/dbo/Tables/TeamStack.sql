CREATE TABLE [dbo].[TeamStack] (
    [Num]   INT IDENTITY (1, 1) NOT NULL,
    [Team]  INT NULL,
    [Stack] INT NULL,
    PRIMARY KEY CLUSTERED ([Num] ASC),
    FOREIGN KEY ([Stack]) REFERENCES [dbo].[Stack] ([StackID]),
    FOREIGN KEY ([Team]) REFERENCES [dbo].[Team] ([TeamID])
);

