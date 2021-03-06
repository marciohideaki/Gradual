set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_tb_avaliacao_palestra_ins]
( @id_avaliacaoPalestra     INT OUTPUT = NULL
, @id_cursopalestra         INT
, @ds_avaliacaoPalestrante  VARCHAR(50)
, @ds_material              VARCHAR(50)
, @ds_infraestrutura        VARCHAR(50)
, @ds_expectativa           VARCHAR(50)
, @dt_avaliacao             DATETIME)
AS
BEGIN

    INSERT INTO [educacional].[dbo].[tb_avaliacao_palestra]
			(	ds_avaliacaoPalestrante
			,   ds_material
			,   ds_infraestrutura
			,   ds_expectativa
			,   dt_avaliacao
			,   id_cursopalestra)
    VALUES  (   @ds_avaliacaoPalestrante
			,   @ds_material
			,   @ds_infraestrutura
			,   @ds_expectativa
			,   GETDATE()
			,   @id_cursopalestra)

	SELECT @id_avaliacaoPalestra = SCOPE_IDENTITY()

END;

