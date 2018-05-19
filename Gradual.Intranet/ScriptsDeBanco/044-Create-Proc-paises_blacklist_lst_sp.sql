CREATE PROCEDURE [dbo].[paises_blacklist_lst_sp]
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela tb_paises_blacklist de acordo com o filtro especificado.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
SELECT [id_pais_blacklist]
,      [cd_pais]
FROM   [dbo].[tb_paises_blacklist]