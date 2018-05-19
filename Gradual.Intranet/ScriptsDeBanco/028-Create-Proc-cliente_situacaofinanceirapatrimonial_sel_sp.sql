-- Descrição:       Realiza a seleção dos dados de situação financeira patrimonial de um cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/05/2010
-- Motivo                   : Inclusão dos parametros
							-- @vl_capitalsocial			   numeric(17, 2)
							-- @vl_patrimonioliquido         numeric(17, 2)
							-- @dt_capitalsocial             datetime
							-- @dt_patrimonioliquido         datetime
							
CREATE PROCEDURE cliente_situacaofinanceirapatrimonial_sel_sp
                 @id_sfp bigint
AS
    SELECT [dbo].[tb_cliente_situacaofinanceirapatrimonial].[id_sfp]
    ,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[id_cliente]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[vl_totalbensimoveis]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[vl_totalbensmoveis]
	,	   [dbo].[tb_cliente_situacaofinanceirapatrimonial].[vl_totalaplicacaofinanceira]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[vl_totalsalarioprolabore]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[vl_totaloutrosrendimentos]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[ds_outrosrendimentos]  
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[dt_dataatualizacao]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[vl_capitalsocial]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[vl_patrimonioliquido]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[dt_capitalsocial]
	,      [dbo].[tb_cliente_situacaofinanceirapatrimonial].[dt_patrimonioliquido]
    FROM   [dbo].[tb_cliente_situacaofinanceirapatrimonial]
    WHERE  [dbo].[tb_cliente_situacaofinanceirapatrimonial].[id_sfp] = @id_sfp