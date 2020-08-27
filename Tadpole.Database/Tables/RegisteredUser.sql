﻿CREATE TABLE [dbo].[RegisteredUser]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT DF_RegisteredUser_Id DEFAULT NEWID(),
	[Email] VARCHAR(256) NOT NULL,
	[PasswordHash] CHAR(44) NOT NULL,
	[PasswordSalt] CHAR(24) NOT NULL,
	CONSTRAINT PK_RegisteredUser PRIMARY KEY (Id)
)