CREATE PROCEDURE [dbo].[AddUser]
	@userId nvarchar(250),
	@pwd nvarchar(250),
	@email nvarchar(250)
AS
BEGIN
	DECLARE @userIdentity INT;

	INSERT INTO [dbo].[UserCreds] ([UserId], [UserPwd], [UserEmail]	) 
		VALUES (@userId, @pwd, @email)

		--Get the identity value of the newly inserted row
	SET @userIdentity = SCOPE_IDENTITY();

	INSERT INTO [dbo].[Accounts] ([UserDocEntry], [Balance], [AmountSent], [AmountReceived])
		VALUES ('U' + CAST(@userIdentity AS NVARCHAR(10)), 0.00, 0.00, 0.00);
END;
