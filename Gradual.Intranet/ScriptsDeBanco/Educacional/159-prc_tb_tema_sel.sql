CREATE PROCEDURE prc_tb_tema_sel (@id_tema INT)
AS
   SELECT [tb_tema].[id_tema]
   ,      [tb_tema].[id_nivel]
   ,      [tb_tema].[ds_titulo]
   ,      [tb_tema].[ds_chamada]
   ,      [tb_tema].[st_situacao]
   ,      [tb_tema].[dt_criacao]
   FROM   [dbo].[tb_tema]
   WHERE  [tb_tema].[id_tema] = @id_tema