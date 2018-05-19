create procedure cliente_banco_pri_sp
	@id_cliente bigint
AS
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 20/05/2010
-- Description:	Pegar o registro principal de cada cliente
-- =============================================
SELECT
	[id_banco],
	[id_cliente],
	[ds_agencia],
	[ds_conta],
	[ds_digito],
	[st_principal],
	[cd_banco],
	[tp_conta]
FROM [tb_cliente_banco]
WHERE id_cliente = @id_cliente
	and st_principal = 1;

GO



create procedure cliente_telefone_pri_sp
	@id_cliente bigint
AS
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 20/05/2010
-- Description:	Pegar o registro principal de cada cliente
-- =============================================
     SELECT [id_telefone]
     ,      [id_cliente]
     ,      [id_tipo_telefone]
     ,      [st_principal]
     ,      [ds_ddd]
     ,      [ds_ramal]
     ,      [ds_numero]
     FROM   [tb_cliente_telefone]
WHERE id_cliente = @id_cliente
	and st_principal = 1;

GO



create procedure cliente_endereco_pri_sp
	@id_cliente bigint
AS
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 20/05/2010
-- Description:	Pegar o registro principal de cada cliente
-- =============================================
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
WHERE id_cliente = @id_cliente
	and st_principal = 1;

GO



create procedure cliente_conta_pri_sp
	@id_cliente bigint
AS
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 20/05/2010
-- Description:	Pegar o registro principal de cada cliente
-- =============================================
SELECT
	[id_cliente_conta],
	[id_cliente],
	[cd_assessor],
	[cd_codigo],
	[cd_sistema],
	[st_containvestimento],
	[st_ativa],
	[st_principal]
FROM 
	[tb_cliente_conta]
WHERE id_cliente = @id_cliente
	and st_principal = 1;

GO

