CREATE PROCEDURE [dbo].[AuthenticateUser]
		@Email nvarchar(256)
	,	@Password nvarchar(256)
	,	@IPAddress varchar(22)
	,	@Session uniqueidentifier output
AS
BEGIN
	SET NOCOUNT ON;

	set @Password = RTRIM(LTRIM(@Password))
	set @Email = RTRIM(LTRIM(@Email))
	
	declare @userid bigint = null
	declare @confirmed bit
	declare @result int
	exec @result = FindUser @Email = @Email, @Password = @Password, @UserID = @userid output, @Confirmed = @confirmed output
	
	if (@result = 0)
	begin
		raiserror('60022 This combination of username and password is unknown.', 16, 1)
		return 0
	end
	
	set @Session = null

	if (@confirmed = 1)
	begin
		
		select top 1
			@Session = guidID
		from 
			tblPlayerSession 
		where 
				intUserRID = @userid
			and	strIPAddress = @IPAddress
			and	bitActive = 1
		
		if (@Session is null)
		begin
			set @Session = newid()
			insert into tblPlayerSession(guidID, intUserRID, strIPAddress) values (@Session, @userid, @IPAddress)
		end
		
		exec ValidateSession @Session = @Session, @IPAddress = @IPAddress, @Reason = 'Authenticate' 
				
		return 1
	end else begin
		raiserror('60021 The activation code sent to this email address has not yet been used.', 16, 1)
		return 0
	end
END
