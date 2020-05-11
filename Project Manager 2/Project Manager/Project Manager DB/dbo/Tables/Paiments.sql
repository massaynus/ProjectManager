CREATE TABLE [dbo].[Paiments] (
    [PaymentID]          INT            IDENTITY (1, 1) NOT NULL,
    [PaymentDescription] TEXT           COLLATE French_CI_AS NULL,
    [SenderFullName]     VARCHAR (70)   COLLATE French_CI_AS NOT NULL,
    [SenderID]           INT            NULL,
    [RecieverFullName]   VARCHAR (70)   COLLATE French_CI_AS NOT NULL,
    [RecieverID]         INT            NULL,
    [Amount]             DECIMAL (9, 2) NOT NULL,
    [isSalary]           BIT            DEFAULT ((0)) NULL,
    [isProjectPaiement]  BIT            DEFAULT ((0)) NULL,
    [ProjectID]          INT            NULL,
    [Date]               DATE           NOT NULL,
    PRIMARY KEY CLUSTERED ([PaymentID] ASC),
    FOREIGN KEY ([ProjectID]) REFERENCES [dbo].[Project] ([ProjectID]),
    FOREIGN KEY ([RecieverID]) REFERENCES [dbo].[User] ([UserID]),
    FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([UserID])
);

