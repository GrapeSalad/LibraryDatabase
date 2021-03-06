USE [library]
GO
/****** Object:  Table [dbo].[authors]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[books]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[books](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[books_authors]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[books_authors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[book_id] [int] NULL,
	[author_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[books_genres]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[books_genres](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[book_id] [int] NULL,
	[genre_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[checkouts]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[checkouts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[copy_id] [int] NULL,
	[patron_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[copies]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[copies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[status] [bit] NULL,
	[book_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[genres]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[genres](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[genres_authors]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[genres_authors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[genre_id] [int] NULL,
	[author_id] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[patrons]    Script Date: 6/15/2017 4:50:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[patrons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL
) ON [PRIMARY]

GO
