CREATE TABLE [dbo].[tblCountryBoundaries](
	[autID] [int] IDENTITY(1,1) NOT NULL,
	[strName] [nvarchar](80) NULL,
	[strISO3] [nvarchar](80) NULL,
	[strISO2] [nvarchar](4) NULL,
	[FIPS] [nvarchar](6) NULL,
	[strCountry] [nvarchar](75) NULL,
	[strEnglish] [nvarchar](50) NULL,
	[strFrench] [nvarchar](50) NULL,
	[strSpanish] [nvarchar](50) NULL,
	[strLocal] [nvarchar](54) NULL,
	[FAO] [nvarchar](55) NULL,
	[WAS_ISO] [nvarchar](4) NULL,
	[SOVEREIGN] [nvarchar](40) NULL,
	[CONTINENT] [nvarchar](55) NULL,
	[UNREG1] [nvarchar](75) NULL,
	[UNREG2] [nvarchar](75) NULL,
	[EU] [smallint] NULL,
	[SQKM] [numeric](38, 8) NULL,
	[geoPolygon] [geography] NULL,
 CONSTRAINT [PK_tblCountryBoundaries] PRIMARY KEY CLUSTERED 
(
	[autID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE SPATIAL INDEX [SPATIAL_tblCountryBoundaries] ON [dbo].[tblCountryBoundaries] ([geoPolygon])
