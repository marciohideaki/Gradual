--Descrição: Seleciona registro na tabela tb_alteracao.
--Autor: Gustavo Malta Guimarães
--Data de criação: 19/07/2010
CREATE PROCEDURE [dbo].[alteracao_sel_sp]
	@id_alteracao	int
AS
SELECT id_alteracao
      ,id_cliente
      ,cd_tipo
      ,ds_informacao
      ,ds_descricao
      ,dt_solicitacao
      ,dt_realizacao
      ,id_login
  FROM tb_alteracao
where id_alteracao = @id_alteracao;