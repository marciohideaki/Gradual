set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/*
DESCRIÇÃO:
	Insere registro na tabela tb_cliente_procuradorrepresentante.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 28/04/2010
	
-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/05/2010
-- Motivo                   : Inclusão dos campos:  
							-- @tp_situacaoLegal

-- Autor Ultima Alteração   : Gustavo Malta Guimarães
-- Data da ultima alteração : 12/11/2010
-- Motivo                   : Inclusão da Data de Validade

*/
ALTER PROCEDURE [dbo].[cliente_procuradorrepresentante_ins_sp]
   	  @id_cliente                 int
	, @ds_nome                    varchar(60)
	, @ds_cpfcnpj                 varchar(15)
	, @dt_nascimento              datetime
	, @ds_numerodocumento         varchar(16)
	, @cd_orgaoemissor            varchar(4)
	, @cd_uforgaoemissordocumento varchar(4)
	, @tp_documento               varchar(2)
	, @tp_situacaoLegal			  numeric(1, 0)
	, @dt_validade                datetime
	, @id_procuradorrepresentante int OUTPUT
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
	   ,    [dt_validade]
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
	   ,    @dt_validade
       )
SELECT @id_procuradorrepresentante = SCOPE_IDENTITY()

