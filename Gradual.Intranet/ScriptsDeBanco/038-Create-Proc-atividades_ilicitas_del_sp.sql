CREATE PROCEDURE [dbo].[atividades_ilicitas_del_sp]
	@id_atividadeilicita bigint
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela tb_atividades_ilicitas.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
DELETE FROM [dbo].[tb_atividades_ilicitas]
WHERE       [id_atividadeilicita] = @id_atividadeilicita