set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
--Descrição: Efetua a migração de clientes selecionados específico entre assessores
--Autor: Bruno Varandas Ribeiro
--Data de criação: 12/05/2010
CREATE PROCEDURE [dbo].[cliente_parcial_assessor_migracao_sp]
	@id_assessor_origem bigint,
	@id_assessor_destino bigint,
	@ds_clientes varchar(2000)
AS
DECLARE @update_cliente nvarchar(4000)

EXEC('UPDATE 
	tb_cliente
SET 
	id_assessorinicial =' + @id_assessor_destino +'
WHERE
	id_assessorinicial =' + @id_assessor_origem +'
	and id_cliente     IN  ('  + @ds_clientes + ')') 
	
EXEC('UPDATE 
	tb_cliente_conta 
SET 
	tb_cliente_conta.cd_assessor     =' + @id_assessor_destino +'
FROM 
	tb_cliente as cliente
WHERE
	tb_cliente_conta.id_cliente      = cliente.id_cliente
	and tb_cliente_conta.cd_assessor = '+ @id_assessor_origem +'
	and cliente.id_cliente           IN (' + @ds_clientes + ')
	and cliente.st_passo             = 4')
	
	
	
	



