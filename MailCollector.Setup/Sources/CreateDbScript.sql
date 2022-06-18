USE [master]

/****** Object:  Database [MailCollectorStorage]    Script Date: 15.05.2022 17:21:42 ******/
CREATE DATABASE [MailCollectorStorage]
 CONTAINMENT = NONE
 WITH CATALOG_COLLATION = DATABASE_DEFAULT

ALTER DATABASE [MailCollectorStorage] SET COMPATIBILITY_LEVEL = 150

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MailCollectorStorage].[dbo].[sp_fulltext_database] @action = 'enable'
end 

ALTER DATABASE [MailCollectorStorage] SET ANSI_NULL_DEFAULT OFF 

ALTER DATABASE [MailCollectorStorage] SET ANSI_NULLS OFF 

ALTER DATABASE [MailCollectorStorage] SET ANSI_PADDING OFF 

ALTER DATABASE [MailCollectorStorage] SET ANSI_WARNINGS OFF 

ALTER DATABASE [MailCollectorStorage] SET ARITHABORT OFF 

ALTER DATABASE [MailCollectorStorage] SET AUTO_CLOSE OFF 

ALTER DATABASE [MailCollectorStorage] SET AUTO_SHRINK OFF 

ALTER DATABASE [MailCollectorStorage] SET AUTO_UPDATE_STATISTICS ON 

ALTER DATABASE [MailCollectorStorage] SET CURSOR_CLOSE_ON_COMMIT OFF 

ALTER DATABASE [MailCollectorStorage] SET CURSOR_DEFAULT  GLOBAL 

ALTER DATABASE [MailCollectorStorage] SET CONCAT_NULL_YIELDS_NULL OFF 

ALTER DATABASE [MailCollectorStorage] SET NUMERIC_ROUNDABORT OFF 

ALTER DATABASE [MailCollectorStorage] SET QUOTED_IDENTIFIER OFF 

ALTER DATABASE [MailCollectorStorage] SET RECURSIVE_TRIGGERS OFF 

ALTER DATABASE [MailCollectorStorage] SET  DISABLE_BROKER 

ALTER DATABASE [MailCollectorStorage] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 

ALTER DATABASE [MailCollectorStorage] SET DATE_CORRELATION_OPTIMIZATION OFF 

ALTER DATABASE [MailCollectorStorage] SET TRUSTWORTHY OFF 

ALTER DATABASE [MailCollectorStorage] SET ALLOW_SNAPSHOT_ISOLATION OFF 

ALTER DATABASE [MailCollectorStorage] SET PARAMETERIZATION SIMPLE 

ALTER DATABASE [MailCollectorStorage] SET READ_COMMITTED_SNAPSHOT OFF 

ALTER DATABASE [MailCollectorStorage] SET HONOR_BROKER_PRIORITY OFF 

ALTER DATABASE [MailCollectorStorage] SET RECOVERY SIMPLE 

ALTER DATABASE [MailCollectorStorage] SET  MULTI_USER 

ALTER DATABASE [MailCollectorStorage] SET PAGE_VERIFY CHECKSUM  

ALTER DATABASE [MailCollectorStorage] SET DB_CHAINING OFF 

ALTER DATABASE [MailCollectorStorage] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 

ALTER DATABASE [MailCollectorStorage] SET TARGET_RECOVERY_TIME = 60 SECONDS 

ALTER DATABASE [MailCollectorStorage] SET DELAYED_DURABILITY = DISABLED 

ALTER DATABASE [MailCollectorStorage] SET ACCELERATED_DATABASE_RECOVERY = OFF  

EXEC sys.sp_db_vardecimal_storage_format N'MailCollectorStorage', N'ON'

ALTER DATABASE [MailCollectorStorage] SET QUERY_STORE = OFF

ALTER SERVER ROLE [sysadmin] ADD MEMBER [NT SERVICE\Winmgmt]

ALTER SERVER ROLE [sysadmin] ADD MEMBER [NT SERVICE\SQLWriter]

ALTER SERVER ROLE [sysadmin] ADD MEMBER [NT SERVICE\SQLSERVERAGENT]

ALTER SERVER ROLE [sysadmin] ADD MEMBER [NT Service\MSSQLSERVER]

ALTER SERVER ROLE [sysadmin] ADD MEMBER [NT AUTHORITY\СИСТЕМА]

GO
USE [MailCollectorStorage]

GRANT VIEW ANY COLUMN ENCRYPTION KEY DEFINITION TO [public] AS [dbo]

GRANT VIEW ANY COLUMN MASTER KEY DEFINITION TO [public] AS [dbo]

/****** Object:  Table [dbo].[Folders]    Script Date: 15.05.2022 17:21:43 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Folders](
	[Uid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[FullName] [nvarchar](256) NOT NULL,
	[ImapClientUid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Folders] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[ImapClients]    Script Date: 15.05.2022 17:21:43 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ImapClients](
	[Uid] [uniqueidentifier] NOT NULL,
	[Login] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[ImapServerUid] [uniqueidentifier] NOT NULL,
	[IsWorking] [bit] NULL,
 CONSTRAINT [PK_ImapClients] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[ImapServers]    Script Date: 15.05.2022 17:21:43 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[ImapServers](
	[Uid] [uniqueidentifier] NOT NULL,
	[Uri] [nvarchar](128) NOT NULL,
	[Port] [int] NOT NULL,
	[UseSsl] [bit] NOT NULL,
 CONSTRAINT [PK_ImapServers] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[Mails]    Script Date: 15.05.2022 17:21:43 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Mails](
	[Uid] [uniqueidentifier] NOT NULL,
	[FolderUid] [uniqueidentifier] NOT NULL,
	[IndexInFolder] [int] NOT NULL,
	[Subject] [nvarchar](MAX) NULL,
	[Date] [datetimeoffset](7) NOT NULL,
	[MFrom] [nvarchar](MAX) NOT NULL,
	[MTo] [nvarchar](MAX) NOT NULL,
	[MCc] [nvarchar](MAX) NULL,
	[HtmlBody] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_Mails] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[TelegramBotSubscribers]    Script Date: 15.05.2022 17:21:43 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[TelegramBotSubscribers](
	[ChatId] [bigint] NULL,
	[Username] [nvarchar](128) NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[Folders] ADD  CONSTRAINT [DF_Folders_Uid]  DEFAULT (newid()) FOR [Uid]

ALTER TABLE [dbo].[ImapClients] ADD  CONSTRAINT [DF_ImapClients_Uid]  DEFAULT (newid()) FOR [Uid]

ALTER TABLE [dbo].[ImapServers] ADD  CONSTRAINT [DF_ImapServers_Uid]  DEFAULT (newid()) FOR [Uid]

ALTER TABLE [dbo].[Mails] ADD  CONSTRAINT [DF_Mails_Uid]  DEFAULT (newid()) FOR [Uid]

ALTER TABLE [dbo].[Folders]  WITH CHECK ADD  CONSTRAINT [FK_Folders_ImapClients] FOREIGN KEY([ImapClientUid])
REFERENCES [dbo].[ImapClients] ([Uid])

ALTER TABLE [dbo].[Folders] CHECK CONSTRAINT [FK_Folders_ImapClients]

ALTER TABLE [dbo].[ImapClients]  WITH CHECK ADD  CONSTRAINT [FK_ImapClients_ImapServers] FOREIGN KEY([ImapServerUid])
REFERENCES [dbo].[ImapServers] ([Uid])

ALTER TABLE [dbo].[ImapClients] CHECK CONSTRAINT [FK_ImapClients_ImapServers]

ALTER TABLE [dbo].[Mails]  WITH CHECK ADD  CONSTRAINT [FK_Mails_Folders] FOREIGN KEY([FolderUid])
REFERENCES [dbo].[Folders] ([Uid])

ALTER TABLE [dbo].[Mails] CHECK CONSTRAINT [FK_Mails_Folders]

USE [master]

ALTER DATABASE [MailCollectorStorage] SET  READ_WRITE 

