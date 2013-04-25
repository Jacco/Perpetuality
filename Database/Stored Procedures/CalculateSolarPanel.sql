CREATE PROCEDURE CalculateSolarPanel
		@InstallationSize int -- [Wp]
	,	@Longitude numeric(18, 6)
	,	@Latitude numeric(18, 6)
	,	@GameDate datetime
	,	@SolarPower numeric(18, 2) -- W/m2/year
	
	,	@PowerOutput numeric(18,6) output 
	,	@Cost numeric(18,2) output
	,	@Revenue numeric(18, 6) output
AS
BEGIN
	SET NOCOUNT ON;
	-- find continent
	declare @CountryID int
	declare @Continent varchar(200)
	
	exec CountryByLonLat @Longitude, @Latitude, @CountryID output
	
	if (@CountryID = 122)
	begin
		set @Continent = 'Japan'
	end else begin
		select @Continent = CONTINENT from tblCountryBoundaries where autID = @CountryID
	end
	
	-- find costs [credits/Wp] for continent
	declare @costPerWp numeric(18,3) = 0.0                                                         -- [credit]
	set @costPerWp = case @Continent
						when 'Africa' then 0.7
						when 'Antartica' then 5.0
						when 'Asia' then 0.7
						when 'Europe' then 1.5
						when 'North America' then 1.4
						when 'Oceania' then 0.7 -- this value is missing
						when 'South America' then 0.7
						when 'Japan' then 1.6
						else 5.0
					 end
					 
	declare @revenuePer_kWh numeric(18,3) = 0.0                                                    -- [credit]
	set @revenuePer_kWh = case @Continent
						when 'Africa' then 0.10
						when 'Antartica' then 0.0
						when 'Asia' then 0.10
						when 'Europe' then 0.20
						when 'North America' then 0.12
						when 'Oceania' then 0.22 
						when 'South America' then 0.10
						when 'Japan' then 0.25
						else 0.0
					 end

	-- calculate the year in to game 
	declare @year int = DateDiff(year, convert(date, '20130101', 112), @GameDate) + 1              -- [year]
	
	declare @installationCost numeric(18, 2) = @InstallationSize * @costPerWp * Power(0.93, @year) -- [credits]
	
	declare @installationSurface numeric(18, 6) = (@InstallationSize + 0.0) / 135                  -- [m2]
	
	declare @energyProduced numeric(18, 6) = @SolarPower * @installationSurface * 0.18             -- [kWh/year]
	
	declare @powerOfPlant numeric(18, 6) = @energyProduced * 0.1141552
	
	set @PowerOutput = @powerOfPlant                                                               -- [W]
	set @Cost = @installationCost                                                                  -- [credits]
	set @Revenue = @revenuePer_kWh * @energyProduced                                               -- [credits/year]

	return 0
END
GO
