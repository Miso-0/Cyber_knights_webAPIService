CREATE TABLE [dbo].[Transaction] (
    [TransactionId]       int  identity     NOT NULL,
    [TransactionDate] date  NOT NULL,
    [UserID]              CHAR (13) NOT NULL,
    [ItemCode]            CHAR (6)  NOT NULL,
	TransactionAmount decimal not null, 
    PRIMARY KEY CLUSTERED ([TransactionId] ASC),
    FOREIGN KEY ([ItemCode]) REFERENCES [dbo].[Item] ([ItemCode]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);

