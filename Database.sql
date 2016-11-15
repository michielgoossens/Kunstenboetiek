USE [master]
GO
/****** Object:  Database [Kunstenboetiek]    Script Date: 27/10/2016 15:42:41 ******/
CREATE DATABASE [Kunstenboetiek]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Kunstenboetiek', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Kunstenboetiek.mdf' , SIZE = 4288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Kunstenboetiek_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Kunstenboetiek_log.ldf' , SIZE = 1088KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Kunstenboetiek] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Kunstenboetiek].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Kunstenboetiek] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET ARITHABORT OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Kunstenboetiek] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Kunstenboetiek] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Kunstenboetiek] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Kunstenboetiek] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Kunstenboetiek] SET  MULTI_USER 
GO
ALTER DATABASE [Kunstenboetiek] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Kunstenboetiek] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Kunstenboetiek] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Kunstenboetiek] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Kunstenboetiek] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Kunstenboetiek', N'ON'
GO
USE [Kunstenboetiek]
GO
/****** Object:  Table [dbo].[Artikels]    Script Date: 27/10/2016 15:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artikels](
	[ArtikelNr] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nchar](100) NOT NULL,
	[Prijs] [float] NULL,
	[Soort] [nchar](10) NULL,
 CONSTRAINT [PK_Artikels] PRIMARY KEY CLUSTERED 
(
	[ArtikelNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArtikelsAfbeeldingen]    Script Date: 27/10/2016 15:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtikelsAfbeeldingen](
	[AfbeeldingNr] [int] IDENTITY(1,1) NOT NULL,
	[ArtikelNr] [int] NOT NULL,
	[AfbeeldingLink] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ArtikelsAfbeeldingen] PRIMARY KEY CLUSTERED 
(
	[AfbeeldingNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Facturen]    Script Date: 27/10/2016 15:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facturen](
	[FactuurNr] [int] IDENTITY(1,1) NOT NULL,
	[KlantNr] [int] NOT NULL,
	[Datum] [date] NOT NULL,
 CONSTRAINT [PK_Facturen] PRIMARY KEY CLUSTERED 
(
	[FactuurNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FactuurRegels]    Script Date: 27/10/2016 15:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FactuurRegels](
	[RegelNr] [int] IDENTITY(1,1) NOT NULL,
	[FactuurNr] [int] NOT NULL,
	[ArtikelNr] [int] NOT NULL,
	[Aantal] [int] NOT NULL,
	[Korting] [float] NULL,
 CONSTRAINT [PK_FactuurRegels] PRIMARY KEY CLUSTERED 
(
	[RegelNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Klanten]    Script Date: 27/10/2016 15:42:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Klanten](
	[KlantNr] [int] IDENTITY(1,1) NOT NULL,
	[Voornaam] [nchar](50) NOT NULL,
	[Familienaam] [nchar](50) NULL,
	[Straat] [nchar](50) NULL,
	[HuisNr] [nchar](10) NULL,
	[Postcode] [nchar](15) NULL,
	[Gemeente] [nchar](50) NULL,
	[Land] [nchar](20) NULL,
	[Email] [nchar](254) NULL,
	[Telefoon] [nchar](50) NULL,
	[BtwNr] [nchar](15) NULL,
 CONSTRAINT [PK_Klanten] PRIMARY KEY CLUSTERED 
(
	[KlantNr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ArtikelsAfbeeldingen]  WITH CHECK ADD  CONSTRAINT [FK_ArtikelsAfbeeldingen_Artikels] FOREIGN KEY([ArtikelNr])
REFERENCES [dbo].[Artikels] ([ArtikelNr])
GO
ALTER TABLE [dbo].[ArtikelsAfbeeldingen] CHECK CONSTRAINT [FK_ArtikelsAfbeeldingen_Artikels]
GO
ALTER TABLE [dbo].[Facturen]  WITH CHECK ADD  CONSTRAINT [FK_Facturen_Klanten] FOREIGN KEY([KlantNr])
REFERENCES [dbo].[Klanten] ([KlantNr])
GO
ALTER TABLE [dbo].[Facturen] CHECK CONSTRAINT [FK_Facturen_Klanten]
GO
ALTER TABLE [dbo].[FactuurRegels]  WITH CHECK ADD  CONSTRAINT [FK_FactuurRegels_Artikels] FOREIGN KEY([ArtikelNr])
REFERENCES [dbo].[Artikels] ([ArtikelNr])
GO
ALTER TABLE [dbo].[FactuurRegels] CHECK CONSTRAINT [FK_FactuurRegels_Artikels]
GO
ALTER TABLE [dbo].[FactuurRegels]  WITH CHECK ADD  CONSTRAINT [FK_FactuurRegels_Facturen] FOREIGN KEY([FactuurNr])
REFERENCES [dbo].[Facturen] ([FactuurNr])
GO
ALTER TABLE [dbo].[FactuurRegels] CHECK CONSTRAINT [FK_FactuurRegels_Facturen]
GO
USE [master]
GO
ALTER DATABASE [Kunstenboetiek] SET  READ_WRITE 
GO
