CREATE PROCEDURE [dbo].[CountryByLonLat]
		@Longitude numeric(18,6)
	,	@Latitude numeric(18,6)
	,	@CountryID int output
AS
BEGIN
	declare @geo geography = geography::STGeomFromText('POINT(' + convert(varchar, @Longitude) + ' ' + convert(varchar, @Latitude) + ')', 4326)

	select top 1 
		@CountryID = autID
	from 
		tblCountryBoundaries with(index(SPATIAL_tblCountryBoundaries))
	where
		geoPolygon.STIntersects(@geo) = 1
END