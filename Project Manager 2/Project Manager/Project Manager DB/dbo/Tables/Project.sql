CREATE TABLE [dbo].[Project] (
    [ProjectID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (30)   NULL,
    [Description] TEXT           NULL,
    [Owner]       INT            NULL,
    [Team]        INT            NULL,
    [Budget]      DECIMAL (9, 2) NULL,
    [Priority]    INT            NULL,
    [Complexity]  INT            NULL,
    [StartinDate] DATE           NULL,
    [DueDate]     DATE           NULL,
    PRIMARY KEY CLUSTERED ([ProjectID] ASC),
    FOREIGN KEY ([Owner]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Team]) REFERENCES [dbo].[Team] ([TeamID])
);

