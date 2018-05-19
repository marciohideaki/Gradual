USE [GRADUAL_CLUBES_FUNDOS]
GO

/****** Object:  StoredProcedure [dbo].[prc_sel_cotistas]    Script Date: 10/08/2012 20:21:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_sel_cotistas] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM tbCadastroCotistas
END

GO

