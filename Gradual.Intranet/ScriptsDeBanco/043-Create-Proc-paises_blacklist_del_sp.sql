CREATE PROCEDURE [dbo].[prc_tb_paises_blacklist_del]
	@id_pais_blacklist bigint
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela tb_paises_blacklist.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
*/
SET NOCOUNT ON
DELETE FROM [dbo].[tb_paises_blacklist]
WHERE       [id_pais_blacklist] = @id_pais_blacklist