set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Atualiza registro(s) na tabela tb_cliente_conta.
--Autor: Gustavo Malta Guimarães
--Data de criação: 29/09/2010
Create PROCEDURE [dbo].cliente_conta_ativarinativar_upd_sp
	@cd_codigo numeric(7, 0),
	@cd_sistema varchar(4),
	@st_ativa bit
AS
UPDATE tb_cliente_conta
SET 
	[st_ativa] = @st_ativa
WHERE
	[cd_codigo] = @cd_codigo and
	[cd_sistema] = @cd_sistema


