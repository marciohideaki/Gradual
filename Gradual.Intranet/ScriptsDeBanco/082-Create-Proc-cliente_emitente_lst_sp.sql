--Descri��o: Seleciona os dados da tabela tb_cliente_emitente de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de cria��o: 28/04/2010
CREATE PROCEDURE cliente_emitente_lst_sp
	@id_cliente bigint
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
FROM [dbo].[tb_cliente_emitente]
WHERE [id_cliente] = @id_cliente
ORDER BY 
	[id_pessoaautorizada] ASC