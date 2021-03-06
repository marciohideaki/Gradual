set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- Descrição:       Lista dados de telefone de todos clientes
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
ALTER PROCEDURE [dbo].[cliente_telefone_lst_sp]
AS
     SELECT [tb_cliente_telefone].[id_telefone]
     ,      [tb_cliente_telefone].[id_cliente]
     ,      [tb_cliente_telefone].[id_tipo_telefone]
     ,      [tb_cliente_telefone].[st_principal]
     ,      [tb_cliente_telefone].[ds_ddd]
     ,      [tb_cliente_telefone].[ds_ramal]
     ,      [tb_cliente_telefone].[ds_numero]
     FROM   [tb_cliente_telefone]
