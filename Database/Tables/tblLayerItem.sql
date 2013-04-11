CREATE TABLE [dbo].[tblLayerItem] (
    [ObjectID]   INT               IDENTITY (1, 1) NOT NULL,
    [intLayerID] BIGINT            NOT NULL,
    [geoPolygon] [sys].[geography] NULL,
    CONSTRAINT [PK_tblLayerItem] PRIMARY KEY CLUSTERED ([ObjectID] ASC)
);

