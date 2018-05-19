USE [Cadastro]
GO
/****** Object:  StoredProcedure [dbo].[cliente_suitability_lst_sp]    Script Date: 06/07/2010 15:30:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Descrição: Lista os resultados de suitability do cliente.
--Autor: Luciano De Maria Leal
--Data de criação: 07/06/2010
CREATE PROCEDURE [dbo].[cliente_suitability_lst_sp]
	@id_cliente int
AS

SELECT
	*
FROM 
	[dbo].[tb_cliente_suitability]
WHERE
	[id_cliente] = @id_cliente
ORDER BY 
	[dt_realizacao]  desc





