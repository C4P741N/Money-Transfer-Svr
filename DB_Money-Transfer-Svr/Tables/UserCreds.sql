﻿CREATE TABLE [dbo].[UserCreds]
(
	ID				INT NOT NULL IDENTITY (1, 1),
	[DocEntry]		AS 'U' + CAST(ID AS NVARCHAR(10)) PERSISTED,
	[UserId]		VARCHAR(255) NOT NULL  PRIMARY KEY,
	[UserEmail]		VARCHAR(255) NOT NULL,
	[UserPwd]		VARCHAR(255) NOT NULL,
)
