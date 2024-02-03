CREATE PROCEDURE [dbo].[GetStatements]
	@userId nvarchar(255)
AS
BEGIN
	SELECT  
		[ID]			AS Id
		,[DocEntry]			AS TransactionId
      ,	[Recepient]			AS Recepient
      ,	[Amount]			AS Amount
      ,	[TimeStamp]			AS TimeStamp
      ,	[TransactionType]	AS TransactionStatus

	FROM [Money_Transfer].[dbo].[Transactions]
		WHERE [UserDocEntry] = (SELECT [DocEntry] FROM  [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId)
END;
