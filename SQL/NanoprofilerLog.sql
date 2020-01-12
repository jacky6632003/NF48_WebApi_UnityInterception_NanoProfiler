USE [Northwind]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NanoprofilerLog](
	[idx] [bigint] IDENTITY(1,1) NOT NULL,
	[mainId] [nvarchar](70) NULL,
	[sessionId] [nvarchar](70) NULL,
	[parentId] [nvarchar](70) NULL,
	[machine] [nvarchar](100) NULL,
	[type] [nvarchar](50) NULL,
	[name] [nvarchar](3000) NULL,
	[started] [datetime] NULL,
	[dbdruation] [nvarchar](100) NULL,
	[druation] [bigint] NULL,
	[requesttype] [nvarchar](50) NULL,
	[clientip] [nvarchar](50) NULL,
	[dbcount] [nvarchar](100) NULL,
	[executetype] [nvarchar](100) NULL,
	[parameters] [nvarchar](1000) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_NanoprofilerTable] PRIMARY KEY CLUSTERED 
(
	[idx] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[NanoprofilerLog] ADD  CONSTRAINT [DF_NanoprofilerLog_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'idx'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主id，不重覆' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'mainId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每次api共同的id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'sessionId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'對應SaveMainSession的mainid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'parentId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呼叫的電腦名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'machine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總共分session和setp和db，這個是session' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'api的名字 或 紀錄root 或 紀錄執行的sql或sp名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'開始時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'started'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'db耗時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'dbdruation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'api的總耗時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'druation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'request的方式，以我的例子是web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'requesttype'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'這次api呼叫了多少個連線，如果有兩個sp就會有兩個連線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'dbcount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查詢或非查詢' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'executetype'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'參數包含型別和丟進去的數值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'parameters'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料建立時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nanoprofiler紀錄Log的表單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'NanoprofilerLog'
GO


