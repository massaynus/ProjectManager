CREATE TABLE [dbo].[Task] (
    [TaskID]      INT          IDENTITY (1, 1) NOT NULL,
    [Project]     INT          NULL,
    [Name]        VARCHAR (30) NULL,
    [Description] TEXT         NULL,
    [Priority]    INT          NULL,
    [Difficulty]  INT          NULL,
    [DeadLine]    DATE         NULL,
    [Stack]       INT          NULL,
    [isComplete]  BIT          NULL,
    [DoneBy]      INT          DEFAULT (NULL) NULL,
    [IsBooked]    BIT          NULL,
    PRIMARY KEY CLUSTERED ([TaskID] ASC),
    FOREIGN KEY ([DoneBy]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Project]) REFERENCES [dbo].[Project] ([ProjectID]),
    FOREIGN KEY ([Stack]) REFERENCES [dbo].[Stack] ([StackID])
);

