-- Descrição:       Realiza a atualização dos dados de situação financeira patrimonial do cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/05/2010
-- Motivo                   : Inclusão dos parametros
							-- @vl_capitalsocial			   numeric(17, 2)
							-- @vl_patrimonioliquido         numeric(17, 2)
							-- @dt_capitalsocial             datetime
							-- @dt_patrimonioliquido         datetime
							
CREATE PROCEDURE cliente_situacaofinanceirapatrimonial_upd_sp
                 @id_sfp                       bigint
               , @id_cliente                   bigint
               , @vl_totalbensimoveis          numeric(17, 2)
               , @vl_totalbensmoveis           numeric(17, 2)
               , @vl_totalaplicacaofinanceira  numeric(17, 2)
               , @vl_totalsalarioprolabore     numeric(17, 2)
               , @vl_totaloutrosrendimentos    numeric(17, 2)
               , @vl_capitalsocial			   numeric(17, 2)
               , @vl_patrimonioliquido         numeric(17, 2)
               , @dt_capitalsocial             datetime
               , @dt_patrimonioliquido         datetime
               , @ds_outrosrendimentos         varchar(2000)
               , @dt_dataatualizacao           datetime
AS
	UPDATE [dbo].[tb_cliente_situacaofinanceirapatrimonial]
	SET    [id_cliente]                  = @id_cliente
	,      [vl_totalbensimoveis]         = @vl_totalbensimoveis
	,      [vl_totalbensmoveis]          = @vl_totalbensmoveis
	,	   [vl_totalaplicacaofinanceira] = @vl_totalaplicacaofinanceira
	,      [vl_totalsalarioprolabore]    = @vl_totalsalarioprolabore
	,      [vl_totaloutrosrendimentos]   = @vl_totaloutrosrendimentos
	,      [vl_capitalsocial]			 = @vl_capitalsocial
	,      [vl_patrimonioliquido]		 = @vl_patrimonioliquido
	,      [dt_capitalsocial]			 = @dt_capitalsocial
	,      [dt_patrimonioliquido]		 = @dt_patrimonioliquido
	,      [ds_outrosrendimentos]        = @ds_outrosrendimentos
	,      [dt_dataatualizacao]          = @dt_dataatualizacao
	WHERE  [id_sfp]                      = @id_sfp