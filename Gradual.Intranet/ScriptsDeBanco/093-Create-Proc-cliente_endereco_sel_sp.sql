--Descrição: Seleciona registro da tabela tb_cliente_endereco.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 06/05/2010
CREATE PROCEDURE cliente_endereco_sel_sp
	@id_endereco bigint
AS

SELECT
	[id_endereco],
	[id_tipo_endereco],
	[id_cliente],
	[st_principal],
	[cd_cep],
	[cd_cep_ext],
	[ds_logradouro],
	[ds_complemento],
	[ds_bairro],
	[ds_cidade],
	[cd_uf],
	[cd_pais],
	[ds_numero]
FROM 
	tb_cliente_endereco
WHERE
	[id_endereco] = @id_endereco
ORDER BY 
	[id_endereco] ASC