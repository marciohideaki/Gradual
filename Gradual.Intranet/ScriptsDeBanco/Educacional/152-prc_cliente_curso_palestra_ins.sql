set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_cliente_curso_palestra_ins]
( @id_cursopalestra     INT
, @id_cliente           INT
, @st_presenca          CHAR
, @st_confirmainscricao CHAR)
AS
   DECLARE @statuslista      CHAR
   DECLARE @vagasrestantes   INT
   DECLARE @totalinscritos   INT
   DECLARE @existealuno      INT

BEGIN

   SELECT @totalinscritos = [nr_vagainscritos] FROM [dbo].[tb_curso_palestra] WHERE [id_cursopalestra] = @id_cursopalestra;
  
   SET @totalinscritos = @totalinscritos + 1;

   SELECT @vagasrestantes = ([nr_vagalimite] - [nr_vagainscritos]) FROM [dbo].[tb_curso_palestra] WHERE [id_cursopalestra] = @id_cursopalestra;

   IF @vagasrestantes > 0  
      SET @statuslista = 'N';
   ELSE
      SET @statuslista = 'S';

   UPDATE [dbo].[tb_curso_palestra] SET [nr_vagainscritos] = @totalinscritos WHERE [id_cursopalestra] = @id_cursopalestra;

   INSERT INTO [dbo].[tb_cliente_curso_palestra]
          (    [id_cursopalestra]
          ,    [id_cliente]
          ,    [dt_cadastro]
          ,    [st_presenca]
          ,    [st_confirmainscricao]
          ,    [st_listaespera])
   VALUES (    @id_cursopalestra
          ,    @id_cliente
          ,    GETDATE()
          ,    @st_presenca
          ,    @st_confirmainscricao
          ,    @statuslista);
END;

