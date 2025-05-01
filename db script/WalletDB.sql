Create database [WalletDB]
USE [WalletDB]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 1/5/2025 7:46:36 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[idrol] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](30) NOT NULL,
	[descripcion] [varchar](100) NULL,
	[condicion] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[idrol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransfertHistory]    Script Date: 1/5/2025 7:46:36 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransfertHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WalletId] [int] NOT NULL,
	[amount] [decimal](18, 2) NOT NULL,
	[TransferType] [smallint] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/5/2025 7:46:36 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[idusuario] [int] IDENTITY(1,1) NOT NULL,
	[idrol] [int] NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[password_hash] [varbinary](max) NOT NULL,
	[password_salt] [varbinary](max) NOT NULL,
	[condicion] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[idusuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallets]    Script Date: 1/5/2025 7:46:36 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [varchar](50) NOT NULL,
	[WalletName] [varchar](100) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Rol] ADD  DEFAULT ((1)) FOR [condicion]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((1)) FOR [condicion]
GO
ALTER TABLE [dbo].[TransfertHistory]  WITH CHECK ADD  CONSTRAINT [fk_wallet_id] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallets] ([Id])
GO
ALTER TABLE [dbo].[TransfertHistory] CHECK CONSTRAINT [fk_wallet_id]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([idrol])
REFERENCES [dbo].[Rol] ([idrol])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([idrol])
REFERENCES [dbo].[Rol] ([idrol])
GO
