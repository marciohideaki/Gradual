
CREATE PROCEDURE [dbo].[cliente_emitente_lst_porcliente_sp]
@id_cliente int
AS
-- Descrição:       Retorna os emitentes de um cliente
-- Autor:           Gustavo Malta Guimaraes
-- Data de criação: 21/05/2010
SELECT
	id_pessoaautorizada,
	id_cliente,
	ds_nome,
	ds_cpfcnpj,
	ds_numerodocumento,
	cd_sistema,
	st_principal,
	ds_email,
	ds_data
FROM tb_cliente_emitente
WHERE
	id_cliente = @id_cliente
ORDER BY 
	id_pessoaautorizada ASC


