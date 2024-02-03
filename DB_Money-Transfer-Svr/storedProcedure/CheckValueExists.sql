CREATE PROCEDURE [dbo].[CheckValueExists]
	@email nvarchar(250),
	@pwd nvarchar(250)
AS
BEGIN
	SELECT COUNT(*) FROM [dbo].[UserCreds] 
		WHERE [UserEmail] = @email 
		AND [UserPwd] = @pwd
END;
