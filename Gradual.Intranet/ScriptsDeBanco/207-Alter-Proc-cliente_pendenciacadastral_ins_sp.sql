set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Insere registro na tabela tb_cliente_pendenciacadastral.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Inserindo o id_login do usuário que resolveu.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

ALTER PROCEDURE [dbo].[cliente_pendenciacadastral_ins_sp]
	@id_tipo_pendencia int,
	@id_cliente int,
	@ds_pendencia varchar(1000),
	@dt_resolucao datetime,
	@ds_resolucao varchar(200),
	@id_login int,
	@id_pendencia_cadastral int OUTPUT
AS
INSERT tb_cliente_pendenciacadastral
(
	[id_tipo_pendencia],
	[id_cliente],
	[ds_pendencia],
	[dt_cadastropendencia],
	[dt_resolucao],
	[ds_resolucao],
	[id_login]
)
VALUES
(
	@id_tipo_pendencia,
	@id_cliente,
	@ds_pendencia,
	getdate(),
	@dt_resolucao,
	@ds_resolucao,
	@id_login
)

select @id_pendencia_cadastral = @@IDENTITY

