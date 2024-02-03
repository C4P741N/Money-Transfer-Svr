CREATE PROCEDURE [dbo].[GetContacts]
	@userId nvarchar(255),
	@deposit int,
    @transfer int,
    @withdraw int
AS
BEGIN
	
	DECLARE @userDocEntry nvarchar(255);
	SET @userDocEntry = (SELECT [DocEntry] FROM [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId)

	SELECT 
		[ID],
	  [Recepient] AS Recepient,
	  SUM(CASE WHEN TransactionType IN (@transfer, @withdraw) THEN Amount ELSE 0 END) AS TotalSent,
	  SUM(CASE WHEN TransactionType IN (@deposit) THEN Amount ELSE 0 END) AS TotalReceived,
	  Max([TimeStamp]) AS EarliestDate
	FROM [Money_Transfer].[dbo].[Transactions]
	WHERE [Recepient] != 'self' AND [UserDocEntry] = @userDocEntry
	GROUP BY [Recepient], [ID];

END;
