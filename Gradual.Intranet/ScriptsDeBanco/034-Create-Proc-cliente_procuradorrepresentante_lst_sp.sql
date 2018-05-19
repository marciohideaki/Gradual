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
*/
CREATE PROCEDURE [dbo].[cliente_procuradorrepresentante_lst_sp]
	@id_cliente bigint
AS
SET NOCOUNT ON
SELECT   [id_procuradorrepresentante]
,        [id_cliente]
,        [ds_nome]
,        [ds_cpfcnpj]
,        [dt_nascimento]
,        [ds_numerodocumento]
,        [cd_orgaoemissor]
,        [cd_uforgaoemissordocumento]
,        [tp_documento]
,        [tp_situacaoLegal]
FROM     [dbo].[tb_cliente_procuradorrepresentante]
WHERE    [id_cliente] = @id_cliente
ORDER BY [ds_nome] ASC