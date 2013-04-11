CREATE PROCEDURE [dbo].[FindUser]
		@Email nvarchar(256)
	,	@Password nvarchar(256)
	,	@UserID bigint = null output
	,	@confirmed bit = null output
AS
BEGIN
	SET NOCOUNT ON;

	declare @ByPass bit = 0
	declare @ByPassPrefix varchar(256) = (select strValue from tblSetting where strName = 'ByPassPrefix')

	if @Email like @ByPassPrefix + '_%'
	begin
		set @Email = SUBSTRING(@Email, CHARINDEX('_', @Email)+1, 256)
		set @ByPass = 1
	end
	
	declare @hash varbinary(16) = dbo.GeneratePasswordHash(@Password)
	
	declare @actualhash varbinary(16)
	
	select top 1 
			@userid = u.autID
		,	@confirmed = e.bitConfirmed
		,	@actualhash = u.binPassword
	from
		tblPlayerEmail e join tblPlayer u on e.intPlayerID = u.autID
			join tblEmailAddress a on e.intEmailAddressID = a.autID
	where
			a.strEmailAddress = @Email 
		and e.bitActive = 1

	if (@UserID is null)
	begin
		return 0
	end
	if (@hash <> @actualhash and IsNull(@ByPass,0) = 0)
	begin
		return 0
	end
	return 1
END
