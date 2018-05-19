-- Descri��o:       Realiza a sele��o dos dados de situa��o financeira patrimonial de um cliente
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28

-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 11/05/2010
-- Motivo                   : Inclus�o dos parametros
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