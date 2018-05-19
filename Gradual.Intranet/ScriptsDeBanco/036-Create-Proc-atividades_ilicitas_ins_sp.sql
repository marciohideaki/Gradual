CREATE PROCEDURE [dbo].[atividades_ilicitas_ins_sp]
	@cd_atividade numeric(3, 0)
AS
/*
DESCRIÇÃO:
	Insere registro na tabela tb_atividades_ilicitas.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 28/04/2010
*/
INSERT [dbo].[tb_atividades_ilicitas]
     (
       [cd_atividade]
     )
VALUES
     ( 
       @cd_atividade
     )
-- Recupera o valor IDENTITY da linha inserida
SELECT SCOPE_IDENTITY()