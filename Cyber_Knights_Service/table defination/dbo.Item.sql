CREATE TABLE [dbo].[Item] (
    [ItemCode]        CHAR (6)      NOT NULL,
    [ItemName]        VARCHAR (50)  NOT NULL,
    [ItemPrice]       DECIMAL (18)  NOT NULL,
    Id int not null,
    [ItemDescription] VARCHAR (MAX) NOT NULL,
    [ItemQuantity]    INT           NOT NULL,
    [ItemImageUrl]    VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([ItemCode] ASC),
	Foreign key(Id) references dbo.CategoryTbl(Id)
);

