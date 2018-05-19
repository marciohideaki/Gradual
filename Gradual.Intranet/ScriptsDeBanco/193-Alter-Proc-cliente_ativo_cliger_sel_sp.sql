set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
--Descrição: Pega a informação relação à ativação na cliger
--Autor: Gustavo Malta Guimarães
--Data de criação: 28/09/2010

--Descrição: Ativar/Inativar acesso ao HB
--Autor: Gustavo Malta Guimarães
--Data de Alteração: 08/10/2010

ALTER PROCEDURE [dbo].[cliente_ativo_cliger_sel_sp]
	@id_cliente	int
AS
SELECT st_ativo_cliger,st_ativo,dt_ativacaoinativacao,st_ativo_hb
  FROM tb_cliente
where id_cliente = @id_cliente;


