CREATE TABLE [dbo].Promotions
(
	Promotion_ID  char(13) primary key,
	Promotion_Name varchar(Max) not null,
	Promotion_Description varchar(max),
	Promotion_StartDate date  not null,
	Promotion_EndDate date not null,


);
