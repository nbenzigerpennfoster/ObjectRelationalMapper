USE [master]
GO
/****** Object:  Database [tinkering]    Script Date: 12/2/2022 9:59:44 AM ******/
CREATE DATABASE [tinkering]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'tinkering', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\tinkering.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'tinkering_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\tinkering_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [tinkering] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [tinkering].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [tinkering] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [tinkering] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [tinkering] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [tinkering] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [tinkering] SET ARITHABORT OFF 
GO
ALTER DATABASE [tinkering] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [tinkering] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [tinkering] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [tinkering] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [tinkering] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [tinkering] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [tinkering] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [tinkering] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [tinkering] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [tinkering] SET  DISABLE_BROKER 
GO
ALTER DATABASE [tinkering] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [tinkering] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [tinkering] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [tinkering] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [tinkering] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [tinkering] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [tinkering] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [tinkering] SET RECOVERY FULL 
GO
ALTER DATABASE [tinkering] SET  MULTI_USER 
GO
ALTER DATABASE [tinkering] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [tinkering] SET DB_CHAINING OFF 
GO
ALTER DATABASE [tinkering] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [tinkering] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [tinkering] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [tinkering] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'tinkering', N'ON'
GO
ALTER DATABASE [tinkering] SET QUERY_STORE = OFF
GO
USE [tinkering]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 12/2/2022 9:59:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[PersonId] [uniqueidentifier] NOT NULL,
	[Address1] [nvarchar](max) NOT NULL,
	[Address2] [nvarchar](max) NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](2) NOT NULL,
	[Zip] [nvarchar](5) NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Child]    Script Date: 12/2/2022 9:59:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Child](
	[Id] [uniqueidentifier] NOT NULL,
	[PersonId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[RelationshipType] [int] NOT NULL,
 CONSTRAINT [PK_Child] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 12/2/2022 9:59:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Age] [int] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Address] ([PersonId], [Address1], [Address2], [City], [State], [Zip]) VALUES (N'dea386a0-e72a-4859-99d6-29a750b44ef8', N'123 Fake Street', NULL, N'Fakeville', N'NJ', N'12345')
GO
INSERT [dbo].[Address] ([PersonId], [Address1], [Address2], [City], [State], [Zip]) VALUES (N'0b03429c-6afe-4e06-831b-ba0a74400135', N'108 Lost Drive', NULL, N'Happy Place', N'NJ', N'86753')
GO
INSERT [dbo].[Child] ([Id], [PersonId], [FirstName], [LastName], [RelationshipType]) VALUES (N'e9055dbb-f68f-4cfa-b070-61721e73e8cb', N'0b03429c-6afe-4e06-831b-ba0a74400135', N'Daughter', N'Benziger', 2)
GO
INSERT [dbo].[Child] ([Id], [PersonId], [FirstName], [LastName], [RelationshipType]) VALUES (N'67f79014-3838-4d27-b850-6f3f47f094d0', N'0b03429c-6afe-4e06-831b-ba0a74400135', N'Son', N'Benziger', 3)
GO
INSERT [dbo].[Person] ([Id], [FirstName], [LastName], [Age]) VALUES (N'dea386a0-e72a-4859-99d6-29a750b44ef8', N'Blug', N'Glub', 25)
GO
INSERT [dbo].[Person] ([Id], [FirstName], [LastName], [Age]) VALUES (N'0b03429c-6afe-4e06-831b-ba0a74400135', N'Neil', N'Benziger', 37)
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Person]
GO
ALTER TABLE [dbo].[Child]  WITH CHECK ADD  CONSTRAINT [FK_Child_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[Child] CHECK CONSTRAINT [FK_Child_Person]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [FK_Person_Person] FOREIGN KEY([Id])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [FK_Person_Person]
GO
USE [master]
GO
ALTER DATABASE [tinkering] SET  READ_WRITE 
GO
