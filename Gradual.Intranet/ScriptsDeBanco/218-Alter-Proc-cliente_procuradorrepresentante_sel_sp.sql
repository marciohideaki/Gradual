set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/*
DESCRIÇÃO:
	Seleciona os dados da tabela tb_cliente_procuradorrepresentante de acordo com o filtro especificado.
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
ALTER PROCEDURE [dbo].[cliente_procuradorrepresentante_sel_sp]
	@id_procuradorrepresentante int
AS
SET NOCOUNT ON
SELECT [id_procuradorrepresentante]
,      [id_cliente]
,      [ds_nome]
,      [ds_cpfcnpj]
,      [dt_nascimento]
,      [ds_numerodocumento]
,      [cd_orgaoemissor]
,      [cd_uforgaoemissordocumento]
,      [tp_documento]
,	   [tp_situacaolegal]
,      [dt_validade]
FROM   [dbo].[tb_cliente_procuradorrepresentante]
WHERE  [id_procuradorrepresentante] = @id_procuradorrepresentante

