/*
DESCRI��O:
	Insere registro na tabela tb_cliente_procuradorrepresentante.
CRIA��O:
	Desenvolvedor: Ant�nio Rodrigues
	Data: 28/04/2010
	
-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 11/05/2010
-- Motivo                   : Inclus�o dos campos:  
							-- @tp_situacaoLegal
*/
CREATE PROCEDURE [dbo].[cliente_procuradorrepresentante_ins_sp]
   	  @id_cliente                 bigint
	, @ds_nome                    varchar(60)
	, @ds_cpfcnpj                 numeric(15, 0)
	, @dt_nascimento              datetime
	, @ds_numerodocumento         varchar(16)
	, @cd_orgaoemissor            varchar(4)
	, @cd_uforgaoemissordocumento varchar(4)
	, @tp_documento               varchar(2)
	, @tp_situacaoLegal			  numeric(1, 0)
	, @id_procuradorrepresentante bigint OUTPUT
AS
INSERT INTO [dbo].[tb_cliente_procuradorrepresentante]
       (    [id_cliente]
	   ,    [ds_nome]
	   ,    [ds_cpfcnpj]
	   ,    [dt_nascimento]
	   ,    [ds_numerodocumento]
	   ,    [cd_orgaoemissor]
	   ,    [cd_uforgaoemissordocumento]
	   ,    [tp_documento]
	   ,	[tp_situacaoLegal]
       )
VALUES (    @id_cliente
	   ,    @ds_nome
	   ,    @ds_cpfcnpj
	   ,    @dt_nascimento
	   ,    @ds_numerodocumento
	   ,    @cd_orgaoemissor
	   ,    @cd_uforgaoemissordocumento
	   ,    @tp_documento
	   ,	@tp_situacaoLegal
       )
SELECT @id_procuradorrepresentante = SCOPE_IDENTITY()