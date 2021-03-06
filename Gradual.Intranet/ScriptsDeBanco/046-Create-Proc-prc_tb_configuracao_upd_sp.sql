CREATE PROCEDURE [dbo].[configuracao_upd_sp]
	  @id_configuracao bigint
	, @ds_configuracao varchar(50)
	, @ds_valor varchar(300)
AS
/*
DESCRIÇÃO:
	Atualiza registro(s) na tabela tb_configuracao.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
UPDATE [dbo].[tb_configuracao]
SET    [ds_configuracao] = @ds_configuracao
,      [ds_valor]        = @ds_valor
WHERE  [id_configuracao] = @id_configuracao