create or replace PROCEDURE prc_cliente_upd_dt_validade
(
  pDataAtualizada IN DATE
, pCpjCgc         IN LONG
)
AS
/*
	Autor: Ant�nio Rodrigues
	Data : 18.05.2010
	Descricao: Atualiza a data de validade de um cliente que realizou a renova��o cadastral
*/
BEGIN
  UPDATE TSCDOCS
  SET    dt_validade = pDataAtualizada
  WHERE  cd_cpfcgc   = pcpjcgc;
END;
