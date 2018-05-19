
CREATE PROCEDURE [dbo].mensagem_sel_sp
	@id_mensagem int
AS
/*
DESCRIÇÃO:
	Seleciona a mensagem de ajuda para o Portal.
CRIAÇÃO:
	Desenvolvedor: Gustavo Malta Guimarães
	Data: 06/07/2010
*/
SELECT   id_mensagem
,        ds_titulo
,		 ds_mensagem
FROM     tb_mensagem_portal
WHERE    id_mensagem = @id_mensagem
