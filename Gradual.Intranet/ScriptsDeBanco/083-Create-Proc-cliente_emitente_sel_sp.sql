--Descri��o: Seleciona os dados da tabela tb_cliente_emitente de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de cria��o: 28/04/2010
CREATE PROCEDURE cliente_emitente_sel_sp
@id_pessoaautorizada bigint
AS
SELECT
	[id_pessoaautorizada],
	[id_cliente],
	[ds_nome],
	[ds_cpfcnpj],
	[dt_nascimento],
	[ds_numerodocumento],
	[cd_sistema],
	[st_principal],
	[ds_email],
	[ds_data],
	[dt_nascimento]
FROM tb_cliente_emitente
WHERE
	[id_pessoaautorizada] = @id_pessoaautorizada
ORDER BY 
	[id_pessoaautorizada] ASC