CREATE PROCEDURE cliente_contrato_ins_sp
	@id_cliente bigint,
	@id_contrato bigint,
	@dt_assinatura datetime
AS
--Descri��o: Inclui registro na tabela tb_cliente_contrato.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 05/05/2010
SET NOCOUNT ON
INSERT tb_cliente_contrato
(
	id_cliente,
	id_contrato,
	dt_assinatura
)
VALUES
(
	@id_cliente,
	@id_contrato,
	@dt_assinatura
)
GO
