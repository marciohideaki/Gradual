CREATE PROCEDURE [dbo].[configuracao_lst_sp]
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
ORDER BY [ds_configuracao] ASC