ALTER PROCEDURE [dbo].[prc_cliente_parametro_valor_salvar]
( @id_cliente_parametro_valor INT
, @id_cliente_parametro       INT
, @vl_alocado                 MONEY
, @vl_disponivel              MONEY
, @dt_movimento               DATETIME
, @ds_historico               VARCHAR(500))
AS
BEGIN
	IF Exists(SELECT [id_cliente_parametro_valor] FROM [dbo].[tb_cliente_parametro_valor] WHERE [id_cliente_parametro_valor] = @id_cliente_parametro_valor)
	BEGIN
		UPDATE  [dbo].[tb_cliente_parametro_valor]
		SET     [vl_alocado]    = @vl_alocado
        ,       [vl_disponivel] = @vl_disponivel
        ,       [dt_movimento]  = @dt_movimento
        ,       [ds_historico]  = @ds_historico
		WHERE   [id_cliente_parametro_valor] = @id_cliente_parametro_valor
	END
	BEGIN
		INSERT INTO [dbo].[tb_cliente_parametro_valor]
               (     [id_cliente_parametro]
               ,     [vl_alocado]
               ,     [vl_disponivel]
               ,     [dt_movimento]
               ,     [ds_historico])
		VALUES (     @id_cliente_parametro
               ,     @vl_alocado
               ,     @vl_disponivel
               ,     @dt_movimento
               ,     @ds_historico)

		SET @id_cliente_parametro_valor = SCOPE_IDENTITY()

	END

	EXEC prc_cliente_parametro_valor_sel @id_cliente_parametro_valor

END