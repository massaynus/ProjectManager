CREATE TABLE [dbo].[ActionLog] (
    [ActionID]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [UserName]     VARCHAR (35) COLLATE French_CI_AS NOT NULL,
    [UserFullName] VARCHAR (60) COLLATE French_CI_AS NOT NULL,
    [ActionName]   VARCHAR (35) COLLATE French_CI_AS NOT NULL,
    [ActionDATA]   TEXT         NOT NULL,
    [RequestDate]  DATETIME     CONSTRAINT [DF__ActionLog__Reque__619B8048] DEFAULT (getdate()) NULL,
    [ActionMethod] CHAR (8)     NULL,
    CONSTRAINT [PK_ActionLog] PRIMARY KEY CLUSTERED ([ActionID] ASC)
);

