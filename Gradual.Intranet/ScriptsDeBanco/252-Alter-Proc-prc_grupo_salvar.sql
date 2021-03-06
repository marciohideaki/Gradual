ALTER PROCEDURE [dbo].[prc_grupo_salvar]
( @id_grupo INT
, @ds_grupo VARCHAR(100))
AS
BEGIN
	IF Exists(SELECT [id_grupo] FROM [tb_grupo] WHERE [id_grupo] = @id_grupo)
	BEGIN
		UPDATE [tb_grupo]
		SET    [ds_grupo] = @ds_grupo
		WHERE  [id_grupo] = @id_grupo
	END
	ELSE
	BEGIN
		INSERT INTO [tb_grupo] (ds_grupo)
		VALUES (@ds_grupo)

        SET @id_grupo = SCOPE_IDENTITY()
	END
	EXEC prc_grupo_sel @id_grupo
END