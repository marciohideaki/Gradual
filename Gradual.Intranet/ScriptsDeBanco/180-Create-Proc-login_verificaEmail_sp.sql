set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Desenvolvida por: Gustavo Malta Guimarães
--Data: 11/08/2010
--Motivo: Validar Se email já existe na tabela de Login 

ALTER PROCEDURE [dbo].[login_verificaEmail_sp]
	@id_login             int,
    @ds_email				varchar(80),
    @count					int output
AS
	
	if (@ds_email = 'a@a.a')
	Begin --verificação para ver se não está verificando e-mail de importação do Sinacor
		select @count = 0;
	End
	else
	Begin
		if (@id_login is null or @id_login=0 )
		Begin --Inclusão
			SELECT @count = count(*) from tb_login where RTRIM(LTRIM(lower(ds_email))) =  RTRIM(LTRIM(lower(@ds_email)))
		End
		Else
		Begin --Alteração
			SELECT 
				@count = count(*) 
			FROM  
				tb_login 
			WHERE
				RTRIM(LTRIM(lower(ds_email))) =  RTRIM(LTRIM(lower(@ds_email))) 
			AND id_login <> @id_login
		End
	End
 
