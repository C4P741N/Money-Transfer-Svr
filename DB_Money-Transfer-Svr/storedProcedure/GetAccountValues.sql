CREATE PROCEDURE [dbo].[GetAccountValues]
	@userId nvarchar(255)
AS
BEGIN
	SELECT  
		[Balance]			AS Sum_Balance
      ,	[AmountSent]		AS Sum_Sent
      ,	[AmountReceived]	AS Sum_Received

	FROM [Money_Transfer].[dbo].[Accounts]
		WHERE [UserDocEntry] = (SELECT [DocEntry] FROM  [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId)
END;
