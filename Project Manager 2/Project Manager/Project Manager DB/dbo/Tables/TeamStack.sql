CREATE TABLE [dbo].[TeamStack] (
    [Num]   INT IDENTITY (1, 1) NOT NULL,
    [Team]  INT NOT NULL,
    [Stack] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Num] ASC),
    FOREIGN KEY ([Stack]) REFERENCES [dbo].[Stack] ([StackID]),
    FOREIGN KEY ([Team]) REFERENCES [dbo].[Team] ([TeamID])
);

