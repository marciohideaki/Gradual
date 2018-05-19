CREATE PROCEDURE PendenciaClienteAssessor_lst_sp AS
/*
DESCRIÇÃO:
	Realiza a pesquisa de todos os clientes com pendências cadastrais e os relaciona com seus respectivo assessores.
CRIAÇÃO:
	Desenvolvedor: Antônio Rodrigues
	Data: 06/05/2010
*/
SELECT     [cli].[ds_nome],
           [tpc].[ds_pendencia],
CASE WHEN  [clc].[cd_assessor]  IS NULL THEN [cli].[id_assessorinicial] ELSE [clc].[cd_assessor] END AS [cd_assessor]
FROM       [dbo].[tb_cliente_pendenciacadastral] AS [pen]
INNER JOIN [dbo].[tb_tipo_pendenciacadastral]    AS [tpc] ON [tpc].[id_tipo_pendencia] = [pen].[id_tipo_pendencia]
INNER JOIN [dbo].[tb_cliente]                    AS [cli] ON [cli].[id_cliente]        = [pen].[id_cliente]
LEFT  JOIN [dbo].[tb_cliente_conta]              AS [clc] ON [clc].[id_cliente]        = [pen].[id_cliente]
WHERE      [pen].[dt_resolucao] IS NULL
ORDER BY   [cli].[ds_nome], [cd_assessor]