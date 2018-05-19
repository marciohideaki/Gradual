CREATE PROCEDURE [dbo].[atividades_ilicitas_sel_sp]
	@id_atividadeilicita bigint
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela tb_atividades_ilicitas de acordo com o filtro especificado.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SELECT   [id_atividadeilicita]
,        [cd_atividade]
FROM     [dbo].[tb_atividades_ilicitas]
WHERE    [id_atividadeilicita] = @id_atividadeilicita