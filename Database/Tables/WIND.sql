CREATE TABLE [dbo].[WIND] (
    [OBJECTID]   INT               IDENTITY (1, 1) NOT NULL,
    [Shape]      [sys].[geometry]  NULL,
    [geoPolygon] [sys].[geography] NULL,
    PRIMARY KEY CLUSTERED ([OBJECTID] ASC)
);


GO
CREATE SPATIAL INDEX [FDO_Shape]
    ON [dbo].[WIND] ([Shape])
    USING GEOMETRY_GRID
    WITH  (
            BOUNDING_BOX = (XMAX = 400, XMIN = -400, YMAX = 90, YMIN = -90),
            GRIDS = (LEVEL_1 = MEDIUM, LEVEL_2 = MEDIUM, LEVEL_3 = MEDIUM, LEVEL_4 = MEDIUM)
          );

