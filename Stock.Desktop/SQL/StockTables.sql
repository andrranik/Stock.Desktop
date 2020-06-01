USE [Stock]
GO

/****** Object:  Table [dbo].[di_doc_types]    Script Date: 23.05.2020 20:24:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[di_doc_types](
	[id] [int] NOT NULL identity,
	[name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_di_doc_types] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [Stock]
GO

/****** Object:  Table [dbo].[di_docs]    Script Date: 23.05.2020 20:25:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[di_material]    Script Date: 23.05.2020 20:26:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[di_material](
	[id] [int] NOT NULL IDENTITY,
	[name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_di_material] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [Stock]
GO

/****** Object:  Table [dbo].[di_metric_units]    Script Date: 23.05.2020 20:26:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[di_metric_units](
	[id] [int] NOT NULL IDENTITY,
	[name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_di_metric_units] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [Stock]
GO

/****** Object:  Table [dbo].[di_stocks]    Script Date: 23.05.2020 20:27:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[di_stocks](
	[id] [int] NOT NULL IDENTITY,
	[name] [nvarchar](255) NOT NULL,
	[volume] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_di_stocks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [Stock]
GO

/****** Object:  Table [dbo].[di_users]    Script Date: 23.05.2020 20:27:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[di_users](
	[id] [int] NOT NULL IDENTITY,
	[username] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_di_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[di_docs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[date] [datetime] NOT NULL,
	[quantity] [decimal](18, 2) NOT NULL,
	[doc_type_id] [int] NOT NULL,
	[stock_id] [int] NOT NULL,
	[metric_unit_id] [int] NOT NULL,
	[material_id] [int] NOT NULL,
	[user_id] [int] NULL,
 CONSTRAINT [PK_di_docs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[di_docs]  WITH CHECK ADD  CONSTRAINT [FK_di_docs_di_doc_types] FOREIGN KEY([doc_type_id])
REFERENCES [dbo].[di_doc_types] ([id])
GO

ALTER TABLE [dbo].[di_docs] CHECK CONSTRAINT [FK_di_docs_di_doc_types]
GO

ALTER TABLE [dbo].[di_docs]  WITH CHECK ADD  CONSTRAINT [FK_di_docs_di_material] FOREIGN KEY([material_id])
REFERENCES [dbo].[di_material] ([id])
GO

ALTER TABLE [dbo].[di_docs] CHECK CONSTRAINT [FK_di_docs_di_material]
GO

ALTER TABLE [dbo].[di_docs]  WITH CHECK ADD  CONSTRAINT [FK_di_docs_di_metric_units] FOREIGN KEY([metric_unit_id])
REFERENCES [dbo].[di_metric_units] ([id])
GO

ALTER TABLE [dbo].[di_docs] CHECK CONSTRAINT [FK_di_docs_di_metric_units]
GO

ALTER TABLE [dbo].[di_docs]  WITH CHECK ADD  CONSTRAINT [FK_di_docs_di_stocks] FOREIGN KEY([stock_id])
REFERENCES [dbo].[di_stocks] ([id])
GO

ALTER TABLE [dbo].[di_docs] CHECK CONSTRAINT [FK_di_docs_di_stocks]
GO

ALTER TABLE [dbo].[di_docs]  WITH CHECK ADD  CONSTRAINT [FK_di_docs_di_users] FOREIGN KEY([user_id])
REFERENCES [dbo].[di_users] ([id])
GO

ALTER TABLE [dbo].[di_docs] CHECK CONSTRAINT [FK_di_docs_di_users]
GO

USE [Stock]
GO