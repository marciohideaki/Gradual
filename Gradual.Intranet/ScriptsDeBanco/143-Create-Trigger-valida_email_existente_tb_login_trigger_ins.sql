CREATE TRIGGER valida_email_existente_tb_login_trigger ON [dbo].[tb_login]
FOR INSERT, UPDATE AS

DECLARE @count INT
DECLARE @emailInserido varchar(30)

SELECT @emailinserido = ds_email FROM INSERTED

SELECT @count = COUNT(*) FROM [dbo].[tb_login] AS lgn WHERE lgn.ds_email = @emailinserido

IF(@count > 1 AND LOWER(@emailInserido) <> 'a@a.a') BEGIN
	--RAISERROR('Email já existente', 11, 1)
	ROLLBACK
END


