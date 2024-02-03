CREATE PROCEDURE [dbo].[AddTransaction]
	@userId nvarchar(250),
	@amount float(53),
	@timeStamp datetime,
	@type nvarchar(250),
	@recepient nvarchar(250)
AS
BEGIN
	DECLARE @sumAmount FLOAT;
	DECLARE @field nvarchar(200)

	SET @sumAmount = (SELECT [Balance] FROM [Money_Transfer].[dbo].[Accounts] 
						WHERE [UserDocEntry] = (SELECT [DocEntry] FROM [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId))

	IF @sumAmount + @amount >= 0
		BEGIN 
			INSERT INTO [dbo].[Transactions] ([UserDocEntry], [Amount], [TimeStamp], [TransactionType], [Recepient]) 
			VALUES (
            (SELECT [DocEntry] FROM [Money_Transfer].[dbo].[UserCreds] WHERE [UserId] = @userId), 
			@amount,
			@timeStamp,
			@type,
			@recepient)
		END

	--IF @amount > 0
 --   BEGIN 
 --       SET @field = '[AmountReceived]'
 --   END;
 --   ELSE 
 --   BEGIN
 --       SET @field = '[AmountSent]'
 --   END; 

	--DECLARE @sql nvarchar(max)
 --   SET @sql = N'
 --       UPDATE [Money_Transfer].[dbo].[Accounts]
 --      SET 
 --      [Balance] = [Balance] + '+@amount+'
 --      , '+ @field +' + '+@amount+'
      
 --    WHERE [UserDocEntry] = @userDocEntry'
    
 --   EXEC sp_executesql @sql, N'@userDocEntry', userDocEntry
END;
