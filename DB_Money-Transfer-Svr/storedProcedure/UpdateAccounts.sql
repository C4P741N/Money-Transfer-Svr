CREATE PROCEDURE [dbo].[UpdateAccounts]
	@userId nvarchar(255),
    @deposit int,
    @transfer int,
    @withdraw int
AS
BEGIN

    DECLARE @userDocEntry nvarchar(255);

    SET @userDocEntry = (SELECT [DocEntry] FROM [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId)

    UPDATE [dbo].[Accounts]
       SET 
       [Balance] = (SELECT ISNULL(SUM([Amount]),0.00) FROM [Money_Transfer].[dbo].[Transactions] WHERE [UserDocEntry] = @userDocEntry)
       ,[AmountSent] = (SELECT ISNULL(SUM([Amount]),0.00) FROM [Money_Transfer].[dbo].[Transactions] WHERE [TransactionType] IN (@withdraw, @transfer))
        ,[AmountReceived] = (SELECT ISNULL(SUM([Amount]),0.00) FROM [Money_Transfer].[dbo].[Transactions] WHERE [TransactionType] IN (@deposit))
      
     WHERE [UserDocEntry] = @userDocEntry
END;
