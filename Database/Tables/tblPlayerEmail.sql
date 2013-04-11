CREATE TABLE [dbo].[tblPlayerEmail] (
    [autID]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [datCreated]        DATETIME     CONSTRAINT [DF_tblPlayerEmail_datCreated] DEFAULT (getdate()) NULL,
    [intPlayerID]       BIGINT       NOT NULL,
    [intEmailAddressID] BIGINT       NOT NULL,
    [bitActive]         BIT          CONSTRAINT [DF_tblPlayerEmail_bitActive] DEFAULT ((1)) NOT NULL,
    [datDeactivated]    DATETIME     NULL,
    [strConfirmHash]    VARCHAR (32) NOT NULL,
    [bitConfirmed]      BIT          CONSTRAINT [DF_tblPlayerEmail_bitConfirmed] DEFAULT ((0)) NOT NULL,
    [datConfirmed]      DATETIME     NULL,
    CONSTRAINT [PK_tblPlayerEmail] PRIMARY KEY CLUSTERED ([autID] ASC)
);

