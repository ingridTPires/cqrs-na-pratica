CREATE DATABASE [CqrsNaPratica]
go
USE [CqrsNaPratica]
GO
/****** Object:  Table [dbo].[Curso]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Curso](
	[CursoID] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NOT NULL,
	[Creditos] [int] NOT NULL,
 CONSTRAINT [PK_Curso] PRIMARY KEY CLUSTERED 
(
	[CursoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Desinscricao]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Desinscricao](
	[DesincricaoID] [bigint] IDENTITY(1,1) NOT NULL,
	[CursoID] [bigint] NOT NULL,
	[AlunoID] [bigint] NOT NULL,
	[Data] [DateTime] NOT NULL,
	[Comentario] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Desincricao] PRIMARY KEY CLUSTERED 
(
	[DesincricaoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Inscricao]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inscricao](
	[InscricaoID] [bigint] IDENTITY(1,1) NOT NULL,
	[AlunoID] [bigint] NOT NULL,
	[CursoID] [bigint] NOT NULL,
	[Grade] [int] NOT NULL,
 CONSTRAINT [PK_Inscricao] PRIMARY KEY CLUSTERED 
(
	[InscricaoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Aluno]    Script Date: 6/27/2018 9:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aluno](
	[AlunoID] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Aluno] PRIMARY KEY CLUSTERED 
(
	[AlunoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Curso] ON 

GO
INSERT [dbo].[Curso] ([CursoID], [Nome], [Creditos]) VALUES (1, N'Matemática', 3)
GO
INSERT [dbo].[Curso] ([CursoID], [Nome], [Creditos]) VALUES (2, N'Português', 3)
GO
INSERT [dbo].[Curso] ([CursoID], [Nome], [Creditos]) VALUES (3, N'Química', 3)
GO
INSERT [dbo].[Curso] ([CursoID], [Nome], [Creditos]) VALUES (4, N'Ciências', 4)
GO
INSERT [dbo].[Curso] ([CursoID], [Nome], [Creditos]) VALUES (5, N'Física', 4)
GO
INSERT [dbo].[Curso] ([CursoID], [Nome], [Creditos]) VALUES (6, N'Biologia', 3)
GO
INSERT [dbo].[Curso] ([CursoID], [Nome], [Creditos]) VALUES (7, N'Inglês', 3)
GO
SET IDENTITY_INSERT [dbo].[Curso] OFF
GO
SET IDENTITY_INSERT [dbo].[Inscricao] ON 

GO
INSERT [dbo].[Inscricao] ([InscricaoID], [AlunoID], [CursoID], [Grade]) VALUES (5, 2, 2, 1)
GO
INSERT [dbo].[Inscricao] ([InscricaoID], [AlunoID], [CursoID], [Grade]) VALUES (13, 2, 3, 3)
GO
INSERT [dbo].[Inscricao] ([InscricaoID], [AlunoID], [CursoID], [Grade]) VALUES (20, 1, 1, 1)
GO
INSERT [dbo].[Inscricao] ([InscricaoID], [AlunoID], [CursoID], [Grade]) VALUES (38, 1, 2, 3)
GO
SET IDENTITY_INSERT [dbo].[Inscricao] OFF
GO
SET IDENTITY_INSERT [dbo].[Aluno] ON 

GO
INSERT [dbo].[Aluno] ([AlunoID], [Nome], [Email]) VALUES (1, N'Alice', N'alice@gmail.com')
GO
INSERT [dbo].[Aluno] ([AlunoID], [Nome], [Email]) VALUES (2, N'Bob', N'bob@outlook.com')
GO
SET IDENTITY_INSERT [dbo].[Aluno] OFF
GO
ALTER TABLE [dbo].[Desinscricao]  WITH CHECK ADD  CONSTRAINT [FK_Desincricao_Curso] FOREIGN KEY([CursoID])
REFERENCES [dbo].[Curso] ([CursoID])
GO
ALTER TABLE [dbo].[Desinscricao] CHECK CONSTRAINT [FK_Desincricao_Curso]
GO
ALTER TABLE [dbo].[Desinscricao]  WITH CHECK ADD  CONSTRAINT [FK_Desincricao_Aluno] FOREIGN KEY([AlunoID])
REFERENCES [dbo].[Aluno] ([AlunoID])
GO
ALTER TABLE [dbo].[Desinscricao] CHECK CONSTRAINT [FK_Desincricao_Aluno]
GO
ALTER TABLE [dbo].[Inscricao]  WITH CHECK ADD  CONSTRAINT [FK_Inscricao_Curso] FOREIGN KEY([CursoID])
REFERENCES [dbo].[Curso] ([CursoID])
GO
ALTER TABLE [dbo].[Inscricao] CHECK CONSTRAINT [FK_Inscricao_Curso]
GO
ALTER TABLE [dbo].[Inscricao]  WITH CHECK ADD  CONSTRAINT [FK_Inscricao_Aluno] FOREIGN KEY([AlunoID])
REFERENCES [dbo].[Aluno] ([AlunoID])
GO
ALTER TABLE [dbo].[Inscricao] CHECK CONSTRAINT [FK_Inscricao_Aluno]
GO
