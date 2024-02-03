CREATE PROCEDURE [dbo].[GetUserName]
	@email nvarchar(250)
AS
BEGIN
	SELECT [UserId] FROM [UserCreds] WHERE [UserEmail] = @email;
END;
