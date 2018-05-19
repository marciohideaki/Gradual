/*
DESCRI��O:
	Atualiza registro(s) na tabela tb_cliente_procuradorrepresentante.
CRIA��O:
	Desenvolvedor: Ant�nio Rodrigues
	Data: 28/04/2010
	
-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 11/05/2010
-- Motivo                   : Inclus�o dos campos:  
							-- @tp_situacaoLegal		
*/
CREATE PROCEDURE [dbo].[cliente_procuradorrepresentante_upd_sp]
	  @id_procuradorrepresentante bigint
	, @id_cliente                 bigint
	, @ds_nome                    varchar(60)
	, @ds_cpfcnpj                 numeric(15, 0)
	, @dt_nascimento              datetime
	, @ds_numerodocumento         varchar(16)
	, @cd_orgaoemissor            varchar(4)
	, @cd_uforgaoemissordocumento varchar(4)
	, @tp_documento               varchar(2)
	, @tp_situacaoLegal			  numeric(1, 0)
AS
SET NOCOUNT ON
UPDATE [dbo].[tb_cliente_procuradorrepresentante]
SET    [id_cliente]                 = @id_cliente
,      [ds_nome]                    = @ds_nome
,      [ds_cpfcnpj]                 = @ds_cpfcnpj
,      [dt_nascimento]              = @dt_nascimento
,      [ds_numerodocumento]         = @ds_numerodocumento
,      [cd_orgaoemissor]            = @cd_orgaoemissor
,      [cd_uforgaoemissordocumento] = @cd_uforgaoemissordocumento
,      [tp_documento]               = @tp_documento
,      [tp_situacaoLegal]           = @tp_situacaoLegal
WHERE  [id_procuradorrepresentante] = @id_procuradorrepresentante