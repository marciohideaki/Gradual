set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Lista o passo que o cliente está mais as contas dele para verificar se é primeira exoportação ou não e escolher a conta para exportar caso não seja
--Autor: Luciano De Maria Leal
--Data de criação: 09/06/2010
ALTER PROCEDURE [dbo].[cliente_passo_conta_lst_sp]
	@id_cliente int
AS

SELECT
         [cli].[id_cliente]
       , [cli].[st_passo]
       , [con].[id_cliente_conta]
       , [con].[cd_assessor]
       , [con].[cd_sistema]
       , [con].[st_principal]
 FROM
        [tb_cliente]          cli
 JOIN	[tb_cliente_conta]    con
   ON	[cli].[id_cliente] = [con].[id_cliente]
WHERE
        [con].[st_ativa] = 1
 AND    [cli].[id_cliente] = @id_cliente
ORDER BY 
	[id_cliente_conta] 