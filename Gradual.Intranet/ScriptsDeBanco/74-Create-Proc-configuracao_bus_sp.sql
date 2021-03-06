set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


CREATE PROCEDURE [dbo].[configuracao_bus_sp]
	@ds_configuracao varchar(50)
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela tb_configuracao de acordo com a descrição da configuração.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 04/05/2010
*/
SELECT   [id_configuracao]
,        [ds_configuracao]
,        [ds_valor]
FROM     [dbo].[tb_configuracao]
WHERE    [ds_configuracao] like @ds_configuracao

