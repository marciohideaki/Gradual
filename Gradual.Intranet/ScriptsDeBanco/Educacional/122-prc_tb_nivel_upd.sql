CREATE PROCEDURE prc_tb_nivel_upd
( @id_nivel int
, @ds_nivel varchar(50)
, @nr_order int)
AS
   UPDATE [dbo].[tb_nivel]
   SET    [tb_nivel].[ds_nivel] = @ds_nivel
   ,      [tb_nivel].[nr_order] = @nr_order
   WHERE  [tb_nivel].[id_nivel] = @id_nivel