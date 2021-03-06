set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 19/05/2010
-- Description:	Recupera uma lista de emails disparados em um certo período
-- =============================================
CREATE PROCEDURE [dbo].[rel_email_disparado_periodo_lst_sp]
	@DtDe datetime  ,
	@DtAte datetime ,
	@IdTipoEmail int,
	@DsEmailDestinatario varchar(200)
AS
BEGIN
	SELECT 
		[id_email],
		[id_tipoemail],
		[ds_corpoemail],
		[ds_emaildestinatario],
		[dt_envioemail],
		[ds_assuntoemail]
	FROM
		[tb_emaildisparado] as email
	WHERE 
		[dt_envioemail] between @DtDe and @DtAte 
		AND ((@IdTipoEmail = 0) OR ([id_tipoemail] = @IdTipoEmail) )
		AND ((@DsEmailDestinatario = '') OR (ds_emaildestinatario LIKE '%' + @DsEmailDestinatario + '%'))
END



