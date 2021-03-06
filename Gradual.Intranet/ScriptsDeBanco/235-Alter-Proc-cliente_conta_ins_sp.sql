set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Insere registro na tabela tb_cliente_conta.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Distigue conta depósito de investimento.
--Autor: Gustavo Malta Guimarães
--Data de criação: 06/12/2010

ALTER PROCEDURE [dbo].[cliente_conta_ins_sp]
	@id_cliente int,
	@cd_assessor numeric(5, 0),
	@cd_codigo numeric(7, 0),
	@cd_sistema varchar(4),
	@st_containvestimento bit,
	@st_principal bit,
	@st_ativa bit,
	@id_cliente_conta int OUTPUT
AS
INSERT tb_cliente_conta
(
	[id_cliente],
	[cd_assessor],
	[cd_codigo],
	[cd_sistema],
	[st_containvestimento],
	[st_ativa],
	[st_principal]
)
VALUES
(
	@id_cliente,
	@cd_assessor,
	@cd_codigo,
	@cd_sistema,
	@st_containvestimento,
	@st_ativa,
	@st_principal
)
-- Get the IDENTITY value for the row just inserted.
SELECT @id_cliente_conta=SCOPE_IDENTITY()


