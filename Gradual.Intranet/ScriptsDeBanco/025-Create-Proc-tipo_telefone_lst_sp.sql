-- Descri��o:       Realiza a sele��o de todos dos dados de tipo de telefone na base
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28
CREATE PROCEDURE tipo_telefone_lst_sp
AS
     SELECT [tb_tipo_telefone].[id_tipo_telefone]
     ,      [tb_tipo_telefone].[ds_telefone]
     FROM   [dbo].[tb_tipo_telefone]