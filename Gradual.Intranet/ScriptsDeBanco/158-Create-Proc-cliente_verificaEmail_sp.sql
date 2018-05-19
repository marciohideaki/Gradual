set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cliente_verificaEmail_sp]
	@id_cliente             int,
    @ds_email				varchar(80),
    @count					int output
AS
	if (@ds_email <> 'a@a.a') --verificação para ver se não está verificando e-mail de importação
	begin
		if (@id_cliente is null )
		begin	
			SELECT @count = count(*) from tb_login where RTRIM(LTRIM(lower(ds_email))) =  RTRIM(LTRIM(lower(@ds_email)))
		end
		else
		begin		
			SELECT 
				@count = count(*) 
			FROM  
				tb_login as lg, 
				tb_cliente as cli
			WHERE
				RTRIM(LTRIM(lower(ds_email))) =  RTRIM(LTRIM(lower(@ds_email))) 
			AND cli.id_login = lg.id_login
			AND cli.id_cliente <> @id_cliente 
		end 
    end


