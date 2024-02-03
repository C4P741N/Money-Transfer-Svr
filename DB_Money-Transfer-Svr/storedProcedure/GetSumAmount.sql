CREATE PROCEDURE [dbo].[GetSumAmount]
	@userId nvarchar(250)
AS
BEGIN
	SELECT ISNULL([Balance], 0.00) 
	FROM [Money_Transfer].[dbo].[Accounts] 
		WHERE [UserDocEntry] =
                (SELECT [DocEntry] FROM [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId)
END;
