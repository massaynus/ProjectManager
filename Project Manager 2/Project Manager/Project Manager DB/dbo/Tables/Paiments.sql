CREATE TABLE [dbo].[Paiments] (
    [PaymentID] INT            IDENTITY (1, 1) NOT NULL,
    [Project]   INT            NULL,
    [Amount]    DECIMAL (9, 2) NULL,
    [Date]      DATE           NULL,
    PRIMARY KEY CLUSTERED ([PaymentID] ASC),
    FOREIGN KEY ([Project]) REFERENCES [dbo].[Project] ([ProjectID])
);

