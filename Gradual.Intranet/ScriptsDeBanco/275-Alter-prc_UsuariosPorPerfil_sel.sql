--Descrição: Lista registro na tabela tb_cliente_investidor_naoresidente.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 06/05/2010
ALTER PROCEDURE [dbo].[cliente_investidor_naoresidente_lst_sp]
	@id_cliente int
AS
SELECT
	[id_investidor_naoresidente],
	[id_cliente],
	[ds_representantelegal],
	[cd_paisorigem],
	[ds_custodiante],
	[ds_rde],
	[ds_codigocvm],
	[ds_nomeadiministradorcarteira]
FROM 
	[tb_cliente_investidor_naoresidente]
WHERE
	[id_cliente] = @id_cliente
ORDER BY 
	[id_investidor_naoresidente] ASC
