
CREATE PROCEDURE [dbo].[cliente_diretor_lst_porcliente_sp]
	@id_cliente int
AS
-- Descrição:       Retorna os diretores de um cliente
-- Autor:           Gustavo Malta Guimaraes
-- Data de criação: 21/05/2010
SELECT
	id_cliente_diretor,
	id_cliente,
	ds_identidade,
	ds_cpfcnpj,
	ds_nome
FROM tb_cliente_diretor
WHERE
	@id_cliente = @id_cliente
ORDER BY 
	id_cliente_diretor ASC

