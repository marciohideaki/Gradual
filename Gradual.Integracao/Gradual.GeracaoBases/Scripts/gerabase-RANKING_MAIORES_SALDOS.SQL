SELECT a.cd_cliente AS "Cliente",
   b.nm_cliente AS "Nome Cliente",
   A.VL_DISPONIVEL AS "Valor Disponivel",
   SUM(b.vl_corliq_tot) AS "Valor Corretagem Total",
   SUM(b.vl_corneg) AS "Valor Corretagem Negocio",
   TO_CHAR(c.cd_cpfcgc,'fm999999999999999') AS "CPF/CNPJ",
   c.cd_assessor AS "Assessor",
   d.nm_assessor AS "Nome Assessor"
FROM tccsaldo a, dbmfinan1 b, tscclibol c, tscasses d
WHERE a.cd_cliente = b.cd_cliente AND 
a.cd_cliente = c.cd_cliente AND
c.tp_cliente IN(01,02,03,18) AND
c.cd_assessor = d.cd_assessor AND
b.dt_negocio BETWEEN '01-JAN-2015' AND TRUNC(sysdate)
GROUP BY a.cd_cliente, b.nm_cliente, A.VL_DISPONIVEL, C.CD_CPFCGC,C.CD_ASSESSOR, D.NM_ASSESSOR
ORDER BY A.VL_DISPONIVEL DESC 
