create or replace PROCEDURE prc_cliente_renov_cadas_sel
(
     Retorno              OUT GLOBALPKG.Retorno
,    pdt_pesquisaInicio   IN date
,    pdt_pesquisaFim      IN date
,    pqt_meses_progressos IN  int               DEFAULT 0
)
AS
BEGIN
        /*  Autor: Ant�nio Rodrigues
            Data : 18.05.2010
            Descri��o: Seleciona o cpfcnpj dos clientes que est�o em per�odo de renova��o cadastral.
       */
        OPEN Retorno FOR
        SELECT      doc.cd_cpfcgc, doc.dt_validade
        FROM        TSCDOCS doc
        WHERE       doc.dt_validade 
					BETWEEN  ADD_MONTHS(pdt_pesquisaInicio,pqt_meses_progressos)  
					AND      ADD_MONTHS(pdt_pesquisaFim, pqt_meses_progressos)
        ORDER BY doc.dt_validade ASC;
END;
/

