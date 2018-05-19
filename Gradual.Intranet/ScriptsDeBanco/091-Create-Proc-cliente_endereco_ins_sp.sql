--Descrição: Inclui registro na tabela tb_cliente_endereco.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 06/05/2010
CREATE PROCEDURE cliente_endereco_ins_sp
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
	@ds_numero varchar(5),
	@id_endereco bigint OUTPUT
AS
INSERT tb_cliente_endereco
(
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
)
VALUES
(
	@id_tipo_endereco,
	@id_cliente,
	@st_principal,
	@cd_cep,
	@cd_cep_ext,
	@ds_logradouro,
	@ds_complemento,
	@ds_bairro,
	@ds_cidade,
	@cd_uf,
	@cd_pais,
	@ds_numero
)
-- Get the IDENTITY value for the row just inserted.
SELECT @id_endereco=SCOPE_IDENTITY()