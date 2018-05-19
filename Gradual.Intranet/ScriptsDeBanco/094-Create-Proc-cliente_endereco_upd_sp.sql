--Descrição: Atualiza registro na tabela tb_cliente_endereco.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 06/05/2010
CREATE PROCEDURE cliente_endereco_upd_sp
	@id_endereco bigint,
	@id_tipo_endereco bigint,
	@id_cliente bigint,
	@st_principal bit,
	@cd_cep numeric(5, 0),
	@cd_cep_ext numeric(3, 0),
	@ds_logradouro varchar(30),
	@ds_complemento varchar(10),
	@ds_bairro varchar(18),
	@ds_cidade varchar(28),
	@cd_uf varchar(4),
	@cd_pais varchar(3),
	@ds_numero varchar(5)
AS
UPDATE tb_cliente_endereco
SET 
	[id_tipo_endereco] = @id_tipo_endereco,
	[id_cliente]       = @id_cliente,
	[st_principal]     = @st_principal,
	[cd_cep]           = @cd_cep,
	[cd_cep_ext]       = @cd_cep_ext,
	[ds_logradouro]    = @ds_logradouro,
	[ds_complemento]   = @ds_complemento,
	[ds_bairro]        = @ds_bairro,
	[ds_cidade]        = @ds_cidade,
	[cd_uf]            = @cd_uf,
	[cd_pais]          = @cd_pais,
	[ds_numero]        = @ds_numero
WHERE
	[id_endereco]      = @id_endereco