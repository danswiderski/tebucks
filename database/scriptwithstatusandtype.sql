USE [master]
GO
/****** Object:  Database [tebucks]    Script Date: 7/7/2023 4:10:16 PM ******/
CREATE DATABASE [tebucks]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'tebucks', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\tebucks.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'tebucks_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\tebucks_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [tebucks] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [tebucks].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [tebucks] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [tebucks] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [tebucks] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [tebucks] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [tebucks] SET ARITHABORT OFF 
GO
ALTER DATABASE [tebucks] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [tebucks] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [tebucks] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [tebucks] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [tebucks] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [tebucks] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [tebucks] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [tebucks] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [tebucks] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [tebucks] SET  ENABLE_BROKER 
GO
ALTER DATABASE [tebucks] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [tebucks] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [tebucks] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [tebucks] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [tebucks] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [tebucks] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [tebucks] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [tebucks] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [tebucks] SET  MULTI_USER 
GO
ALTER DATABASE [tebucks] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [tebucks] SET DB_CHAINING OFF 
GO
ALTER DATABASE [tebucks] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [tebucks] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [tebucks] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [tebucks] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [tebucks] SET QUERY_STORE = OFF
GO
USE [tebucks]
GO
/****** Object:  Table [dbo].[account]    Script Date: 7/7/2023 4:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account](
	[accountId] [int] IDENTITY(1000,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[balance] [money] NOT NULL,
 CONSTRAINT [PK_accounttable] PRIMARY KEY CLUSTERED 
(
	[accountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[status]    Script Date: 7/7/2023 4:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[status](
	[status_id] [int] IDENTITY(1,1) NOT NULL,
	[status_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_status] PRIMARY KEY CLUSTERED 
(
	[status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tebucks_user]    Script Date: 7/7/2023 4:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tebucks_user](
	[user_id] [int] IDENTITY(1001,1) NOT NULL,
	[firstname] [varchar](100) NULL,
	[lastname] [varchar](max) NULL,
	[username] [varchar](50) NOT NULL,
	[password_hash] [varchar](200) NOT NULL,
	[salt] [varchar](200) NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transfer]    Script Date: 7/7/2023 4:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transfer](
	[transfer_id] [int] NOT NULL,
	[to_user_id] [int] NOT NULL,
	[from_user_id] [int] NOT NULL,
	[transfer_type] [int] NOT NULL,
	[transfer_status] [int] NOT NULL,
	[amount] [money] NOT NULL,
 CONSTRAINT [PK_transfer] PRIMARY KEY CLUSTERED 
(
	[transfer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transfer_type]    Script Date: 7/7/2023 4:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transfer_type](
	[type_id] [int] IDENTITY(1,1) NOT NULL,
	[transfer_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_transfer_type] PRIMARY KEY CLUSTERED 
(
	[type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[status] ON 
GO
INSERT [dbo].[status] ([status_id], [status_name]) VALUES (1, N'Approved')
GO
INSERT [dbo].[status] ([status_id], [status_name]) VALUES (2, N'Rejected')
GO
INSERT [dbo].[status] ([status_id], [status_name]) VALUES (3, N'Pending')
GO
SET IDENTITY_INSERT [dbo].[status] OFF
GO
SET IDENTITY_INSERT [dbo].[transfer_type] ON 
GO
INSERT [dbo].[transfer_type] ([type_id], [transfer_name]) VALUES (1, N'Send')
GO
INSERT [dbo].[transfer_type] ([type_id], [transfer_name]) VALUES (2, N'Request')
GO
SET IDENTITY_INSERT [dbo].[transfer_type] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_username]    Script Date: 7/7/2023 4:10:16 PM ******/
ALTER TABLE [dbo].[tebucks_user] ADD  CONSTRAINT [UQ_username] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[account]  WITH CHECK ADD  CONSTRAINT [FK_accounttable_tebucks_user] FOREIGN KEY([user_id])
REFERENCES [dbo].[tebucks_user] ([user_id])
GO
ALTER TABLE [dbo].[account] CHECK CONSTRAINT [FK_accounttable_tebucks_user]
GO
ALTER TABLE [dbo].[transfer]  WITH CHECK ADD  CONSTRAINT [FK_transfer_account] FOREIGN KEY([from_user_id])
REFERENCES [dbo].[account] ([accountId])
GO
ALTER TABLE [dbo].[transfer] CHECK CONSTRAINT [FK_transfer_account]
GO
ALTER TABLE [dbo].[transfer]  WITH CHECK ADD  CONSTRAINT [FK_transfer_account2] FOREIGN KEY([to_user_id])
REFERENCES [dbo].[account] ([accountId])
GO
ALTER TABLE [dbo].[transfer] CHECK CONSTRAINT [FK_transfer_account2]
GO
ALTER TABLE [dbo].[transfer]  WITH CHECK ADD  CONSTRAINT [FK_transfer_status] FOREIGN KEY([transfer_status])
REFERENCES [dbo].[status] ([status_id])
GO
ALTER TABLE [dbo].[transfer] CHECK CONSTRAINT [FK_transfer_status]
GO
ALTER TABLE [dbo].[transfer]  WITH CHECK ADD  CONSTRAINT [FK_transfer_transfer_type] FOREIGN KEY([transfer_type])
REFERENCES [dbo].[transfer_type] ([type_id])
GO
ALTER TABLE [dbo].[transfer] CHECK CONSTRAINT [FK_transfer_transfer_type]
GO
ALTER TABLE [dbo].[account]  WITH CHECK ADD  CONSTRAINT [CHK_balance] CHECK  (([balance]>=(0)))
GO
ALTER TABLE [dbo].[account] CHECK CONSTRAINT [CHK_balance]
GO
ALTER TABLE [dbo].[transfer]  WITH CHECK ADD  CONSTRAINT [CHK_amount] CHECK  (([amount]>(0)))
GO
ALTER TABLE [dbo].[transfer] CHECK CONSTRAINT [CHK_amount]
GO
USE [master]
GO
ALTER DATABASE [tebucks] SET  READ_WRITE 
GO
