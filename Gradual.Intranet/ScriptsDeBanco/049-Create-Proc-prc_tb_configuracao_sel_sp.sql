CREATE PROCEDURE [dbo].[configuracao_sel_sp]
	@id_configuracao bigint
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela tb_configuracao de acordo com o filtro especificado.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
SELECT   [id_configuracao]
,        [ds_configuracao]
,        [ds_valor]
FROM     [dbo].[tb_configuracao]
WHERE    [id_configuracao] = @id_configuracao