INSERT INTO RAFA
(CD_CLIENTE, RECEITA,DATA)
SELECT CD_CLIENTE,SUM(VL_CORLIQ_TOT),MAX(DT_NEGOCIO)
FROM DBMFINAN1
WHERE TO_CHAR(DT_NEGOCIO,'MMYY') = TO_CHAR(add_months(sysdate,-1),'MMYY')
HAVING SUM(VL_CORLIQ_TOT) <> 0 
GROUP BY CD_CLIENTE
/
COMMIT;
INSERT INTO RAFA2
(CD_CLIENTE)
 SELECT DISTINCT CD_CLIENTE
 FROM DBMFINAN1
 WHERE TO_CHAR(DT_NEGOCIO,'MM/YYYY') =  TO_CHAR(SYSDATE,'MM/YYYY') AND VL_CORLIQ_TOT <> 0
 /
COMMIT;

SET ECHO OFF
SET TERMOUT OFF
SET FEEDBACK OFF
SET UNDERLINE OFF
SET WRAP OFF
SET LINESIZE 1300
SET PAGESIZE 0
SET HEADING OFF
SET SPACE 0
SET VERIFY OFF

SET MARKUP HTML ON ENTMAP ON SPOOL ON PREFORMAT OFF 

SPOOL c:\SEMCORRETAGEM_MES_ATUAL.XLS


select 'Cliente','Nome','Ultima receita','Receita','CPF/CNPJ', 'Assessor','Nome'
from dual
/
select a.cd_cliente,b.nm_cliente,to_char(a.data,'mm/dd/yyyy'),
to_char(a.receita,'fm9999999999d00'),
to_char(c.cd_cpfcgc,'fm99999999999999999'), c.cd_assessor,d.nm_assessor
from rafa a, tsccliger b , tscclibol c, TSCASSES D
where a.cd_cliente not in(select cd_cliente FROM rafa2) and
a.cd_cliente = c.cd_cliente and
C.TP_CLIENTE IN(01,02,03,18) AND
c.cd_assessor = d.cd_assessor and
c.cd_cpfcgc = b.cd_cpfcgc and
c.cd_con_dep = b.cd_con_dep and
c.dt_nasc_fund = b.dt_nasc_fund
/
spool off
delete from rafa2;
commit;
delete from rafa;
commit;
EXIT

