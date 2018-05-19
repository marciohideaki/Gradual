CREATE PROCEDURE [dbo].[configuracao_del_sp]
	@id_configuracao bigint
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela tb_configuracao.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
DELETE FROM [dbo].[tb_configuracao]
WHERE       [dbo].[tb_configuracao].[id_configuracao] = @id_configuracao