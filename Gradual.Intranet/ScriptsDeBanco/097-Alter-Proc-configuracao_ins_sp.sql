set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[configuracao_ins_sp]
	  @ds_configuracao varchar(50)
	, @ds_valor        varchar(300)
    , @id_configuracao bigint output
AS
/*
DESCRIÇÃO:
	Insere registro na tabela tb_configuracao.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 29/04/2010
ALTERAÇÃO:
    descrição: Inclusão do parâmetro de output
	Desenvolvedor: Antônio Rodrigues
	Data: 11/05/2010
*/
INSERT INTO [dbo].[tb_configuracao]
       (    [ds_configuracao]
       ,    [ds_valor])
VALUES
       (    @ds_configuracao
       ,    @ds_valor)
-- Get the IDENTITY value for the row just inserted.
SELECT @id_configuracao = SCOPE_IDENTITY()
