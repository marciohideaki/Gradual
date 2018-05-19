CREATE PROCEDURE [dbo].[cliente_procuradorrepresentante_del_sp]
	@id_procuradorrepresentante bigint
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela tb_cliente_procuradorrepresentante.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 28/04/2010
*/
DELETE FROM [dbo].[tb_cliente_procuradorrepresentante]
WHERE       [id_procuradorrepresentante] = @id_procuradorrepresentante