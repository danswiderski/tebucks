USE master
GO

IF DB_ID('tebucks') IS NOT NULL
BEGIN
	ALTER DATABASE tebucks SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE tebucks;
END

CREATE DATABASE tebucks
GO

USE tebucks
GO

CREATE TABLE tebucks_user (
	user_id int IDENTITY(1001,1) NOT NULL,
	firstname varchar(100) NULL,
	lastname varchar(MAX) NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	CONSTRAINT PK_user PRIMARY KEY (user_id),
	CONSTRAINT UQ_username UNIQUE (username)
)

