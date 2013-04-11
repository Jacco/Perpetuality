CREATE PROCEDURE [dbo].[RegisterNewUser]
		@Email nvarchar(256)
	,	@Password nvarchar(256)
	,	@ConfirmHash varchar(32)
	,	@PartnerMail bit
	,	@UserID bigint output
AS
BEGIN
	SET NOCOUNT ON;
	
	set @UserID = null

	set @Password = RTRIM(LTRIM(@Password))
	set @Email = RTRIM(LTRIM(@Email))

	if not exists(select * from tblPlayerEmail ue join tblEmailAddress ea on ue.intEmailAddressID = ea.autID where ea.strEmailAddress = @Email and ue.bitActive = 1)
	begin
		declare @emailaddressid bigint = null
		
		select @emailaddressid = autID from tblEmailAddress where strEmailAddress = @Email
		
		if @emailaddressid is null
		begin
			insert into tblEmailAddress(strEmailAddress)
			select @Email
			
			set @emailaddressid = SCOPE_IDENTITY()
		end
	
		insert into tblPlayer(
				binPassword
		) values (
				dbo.GeneratePasswordHash(@Password)
		)
	  
		set @UserID = scope_identity()
	  
		insert into tblPlayerEmail(
				intPlayerID
			,	intEmailAddressID
			,	strConfirmHash
		) values (
				@userid
			,	@emailaddressid
			,	@ConfirmHash
		)
		
		declare @emailid bigint = scope_identity()
		
		update tblPLayer
		set
				intPrimaryPlayerEmailID = @emailid
		where
				autID = @UserID
				
		--if @PartnerMail = 1
		--begin
		--	insert into tblUserSubscription(intUserID, intSubscriptionTypeID, intMediaFlags) 
		--	select @UserID, 3, 1
		--end
				
		return 1
	end else begin
		if 5 = (select CONVERT(int, binPassword) from tblPlayerEmail ue join tblEmailAddress ea on ue.intEmailAddressID = ea.autID join tblPlayer u on u.intPrimaryPlayerEmailID = ue.autID
			where ea.strEmailAddress = @Email and ue.bitActive = 1)
		begin
			raiserror ('60006 The email address was externally confirmed. You can you password forgotten to use the account.', 16, 1, @Email)
			return 0		
		end			
		raiserror ('60004 The email address "%s" is already registered by an active user.', 16, 1, @Email)
		return 0
	end
END
