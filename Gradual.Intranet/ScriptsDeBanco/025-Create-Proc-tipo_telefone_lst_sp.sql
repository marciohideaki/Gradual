-- Descrição:       Realiza a seleção de todos dos dados de tipo de telefone na base
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE tipo_telefone_lst_sp
AS
     SELECT [tb_tipo_telefone].[id_tipo_telefone]
     ,      [tb_tipo_telefone].[ds_telefone]
     FROM   [dbo].[tb_tipo_telefone]