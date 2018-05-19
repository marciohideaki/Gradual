CREATE PROCEDURE prc_tb_palestrante_upd
( @id_palestrante INT
, @nm_palestrante VARCHAR(100)
, @ds_curriculo   VARCHAR(MAX))
AS
   UPDATE [dbo].[tb_palestrante]
   SET    [tb_palestrante].[nm_palestrante] = @nm_palestrante
   ,      [tb_palestrante].[ds_curriculo]   = @ds_curriculo
   WHERE  [tb_palestrante].[id_palestrante] = @id_palestrante