CREATE TABLE [dbo].[Transactions]
(
	ID					INT NOT NULL IDENTITY (1, 1),
	[DocEntry]			AS 'T' + CAST(ID AS NVARCHAR(10)) PERSISTED PRIMARY KEY,
	[UserDocEntry]		NVARCHAR(255) NOT NULL,
	[Recepient]			NVARCHAR(255) NOT NULL,
	[Amount]			DECIMAL(20,2) NOT NULL, 
	[TimeStamp]			DateTime NOT NULL, 
	[TransactionType]	NVARCHAR(255) NOT NULL, 
)
