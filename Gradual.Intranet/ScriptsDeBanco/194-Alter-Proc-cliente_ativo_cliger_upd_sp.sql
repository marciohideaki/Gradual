set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
--Descrição: Salva a informação relação à ativação na cliger
--Autor: Gustavo Malta Guimarães
--Data de criação: 28/09/2010

--Descrição: Ativar/Inativar acesso ao HB
--Autor: Gustavo Malta Guimarães
--Data de Alteração: 08/10/2010

ALTER PROCEDURE [dbo].[cliente_ativo_cliger_upd_sp]
	@id_cliente	int,
	@st_ativo_cliger bit,
	@st_login_ativo bit,
	@st_ativo_hb bit
AS
	UPDATE tb_cliente
	SET st_ativo_cliger = @st_ativo_cliger,
		st_ativo = @st_login_ativo,
		st_ativo_hb = @st_ativo_hb,
		dt_ativacaoinativacao = getdate()
	where id_cliente = @id_cliente;

