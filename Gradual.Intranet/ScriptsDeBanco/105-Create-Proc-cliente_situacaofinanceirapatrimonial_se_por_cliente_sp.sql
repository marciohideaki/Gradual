						
CREATE PROCEDURE [dbo].[cliente_situacaofinanceirapatrimonial_sel_porcliente_sp]
                 @id_cliente int
AS
-- Descrição:       Retorna a situação financeira patrimonial de um cliente
-- Autor:           Gustavo Malta Guimaraes
-- Data de criação: 21/05/2010
    SELECT id_sfp
    ,      id_cliente
	,      vl_totalbensimoveis
	,      vl_totalbensmoveis
	,	   vl_totalaplicacaofinanceira
	,      vl_totalsalarioprolabore
	,      vl_totaloutrosrendimentos
	,      ds_outrosrendimentos
	,      dt_dataatualizacao
	,      vl_capitalsocial
	,      vl_patrimonioliquido
	,      dt_capitalsocial
	,      dt_patrimonioliquido
    FROM   tb_cliente_situacaofinanceirapatrimonial
    WHERE  id_cliente = @id_cliente

