CREATE PROCEDURE prc_tb_tema_upd
( @id_tema     int
, @id_nivel    int
, @ds_titulo   varchar(200)
, @ds_chamada  varchar(500)
, @st_situacao char(1))
AS
   UPDATE [dbo].[tb_tema]
   SET    [tb_tema].[id_nivel]    = @id_nivel
   ,      [tb_tema].[ds_titulo]   = @ds_titulo
   ,      [tb_tema].[ds_chamada]  = @ds_chamada
   ,      [tb_tema].[st_situacao] = @st_situacao
   WHERE  [tb_tema].[id_tema]     = @id_tema