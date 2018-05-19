CREATE PROCEDURE [dbo].[atividades_ilicitas_upd_sp]
	  @id_atividadeilicita bigint
	, @cd_atividade numeric(3, 0)
AS
/*
DESCRIÇÃO:
	Atualiza registro(s) na tabela tb_atividades_ilicitas.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
UPDATE [dbo].[tb_atividades_ilicitas]
SET    [cd_atividade]        = @cd_atividade
WHERE  [id_atividadeilicita] = @id_atividadeilicita