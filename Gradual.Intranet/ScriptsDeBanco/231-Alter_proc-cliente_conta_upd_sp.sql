set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Atualiza registro(s) na tabela tb_cliente_conta.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Distigue conta depósito de investimento.
--Autor: Gustavo Malta Guimarães
--Data de criação: 06/12/2010

ALTER PROCEDURE [dbo].[cliente_conta_upd_sp]
	@id_cliente_conta int,
	@id_cliente int,
	@cd_assessor numeric(5, 0),
	@cd_codigo numeric(7, 0),
	@cd_sistema varchar(4),
	@st_containvestimento bit,
	@st_ativa bit,
	@st_principal bit
AS
UPDATE tb_cliente_conta
SET 
	[id_cliente] = @id_cliente,
	[cd_assessor] = @cd_assessor,
	[cd_codigo] = @cd_codigo,
	[cd_sistema] = @cd_sistema,
	[st_containvestimento] = @st_containvestimento,
	[st_ativa] = @st_ativa,
	[st_principal] = @st_principal
WHERE
	[id_cliente_conta] = @id_cliente_conta


