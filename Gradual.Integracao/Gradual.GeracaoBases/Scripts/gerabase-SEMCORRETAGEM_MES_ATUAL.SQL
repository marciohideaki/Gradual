select a.cd_cliente as "Cliente",
  b.nm_cliente as "Nome",
  to_char(Max(a.DT_NEGOCIO),'mm/dd/yyyy') as "Data Ultima Receita",
  SUM(A.VL_CORLIQ_TOT) as "Receita",
  to_char(c.cd_cpfcgc,'fm99999999999999999') AS "CPF/CNPJ",
  c.cd_assessor AS "Assessor",
  d.nm_assessor AS "Nome Assessor"
from DBMFINAN1 a
JOIN tscclibol c ON a.cd_cliente = c.cd_cliente AND C.TP_CLIENTE IN(01,02,03,18)
JOIN TSCASSES D ON c.cd_assessor = d.cd_assessor
JOIN tsccliger b ON c.cd_cpfcgc = b.cd_cpfcgc and
  c.cd_con_dep = b.cd_con_dep and
  c.dt_nasc_fund = b.dt_nasc_fund
WHERE TO_CHAR(A.DT_NEGOCIO,'MMYY') = TO_CHAR(add_months(sysdate,-1),'MMYY')
AND A.CD_CLIENTE NOT IN ( SELECT DISTINCT CD_CLIENTE
  FROM DBMFINAN1
  WHERE TO_CHAR(DT_NEGOCIO,'MM/YYYY') =  TO_CHAR(SYSDATE,'MM/YYYY') AND VL_CORLIQ_TOT <> 0)
HAVING SUM(VL_CORLIQ_TOT) <> 0
GROUP BY A.CD_CLIENTE, b.Nm_cliente,c.cd_cpfcgc,c.cd_assessor, d.nm_assessor
ORDER BY A.CD_CLIENTE
