CREATE TABLE [dbo].[Task] (
    [TaskID]      INT          IDENTITY (1, 1) NOT NULL,
    [Project]     INT          NOT NULL,
    [Name]        VARCHAR (30) COLLATE French_CI_AS NOT NULL,
    [Description] TEXT         COLLATE French_CI_AS NOT NULL,
    [Priority]    INT          NOT NULL,
    [Difficulty]  INT          NOT NULL,
    [DeadLine]    DATE         NOT NULL,
    [Stack]       INT          NULL,
    [isComplete]  BIT          DEFAULT ((0)) NULL,
    [isBooked]    BIT          DEFAULT ((0)) NULL,
    [DoneBy]      INT          DEFAULT (NULL) NULL,
    PRIMARY KEY CLUSTERED ([TaskID] ASC),
    FOREIGN KEY ([DoneBy]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([Project]) REFERENCES [dbo].[Project] ([ProjectID]),
    FOREIGN KEY ([Stack]) REFERENCES [dbo].[Stack] ([StackID])
);

