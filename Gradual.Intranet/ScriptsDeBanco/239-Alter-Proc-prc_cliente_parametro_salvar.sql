USE [DirectTradeRisco]
GO
/****** Object:  StoredProcedure [dbo].[prc_cliente_parametro_salvar]    Script Date: 01/05/2011 15:36:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--    select dscr_parametro from tb_parametro_risco where id_parametro = 31940

/*

sp_helptext prc_cliente_parametro_sel
	Criada por: André Cristono Miguel
	Data: 27/07/2010
*/

ALTER PROCEDURE [dbo].[prc_cliente_parametro_salvar]
(
	@id_cliente_parametro int
	,@id_cliente int
    ,@id_grupo int = null
    ,@id_parametro int
	,@vl_parametro numeric(19,2) = null
    ,@dt_validade datetime = null
)
AS
BEGIN
	IF exists(SELECT id_cliente_parametro FROM tb_cliente_parametro WHERE id_cliente_parametro = @id_cliente_parametro)
	BEGIN
		UPDATE [tb_cliente_parametro]
        SET    [id_grupo]             = @id_grupo
        ,      [id_parametro]         = @id_parametro
        ,      [vl_parametro]         = @vl_parametro
        ,      [dt_validade]          = @dt_validade
		WHERE  [id_cliente_parametro] = @id_cliente_parametro
	END
	ELSE BEGIN
		DECLARE @dt_movimento DATETIME
        SET     @dt_movimento = GETDATE()

		INSERT INTO [tb_cliente_parametro]
               (    [id_cliente]
               ,    [id_grupo]
               ,    [id_parametro]
               ,    [vl_parametro]
               ,    [vl_alocado]
               ,    [dt_movimento]
               ,    [dt_validade])
		VALUES (    @id_cliente
               ,    @id_grupo
               ,    @id_parametro
               ,    @vl_parametro
               ,    0
               ,    @dt_movimento
               ,    @dt_validade)

		SET @id_cliente_parametro = SCOPE_IDENTITY()

		DECLARE @descr_parametro VARCHAR(200)
		SELECT  @descr_parametro = ds_parametro FROM [tb_parametro_risco] WHERE [id_parametro] = @id_parametro
		
		INSERT INTO [tb_cliente_parametro_valor]
               (    [id_cliente_parametro]
               ,    [vl_alocado]
               ,    [vl_disponivel]
               ,    [dt_movimento]
               ,    [ds_historico])
		VALUES (    @id_cliente_parametro
               ,    0
               ,    @vl_parametro
               ,    @dt_movimento
               ,    @descr_parametro)
	END

	EXEC prc_cliente_parametro_sel @id_cliente_parametro
END




