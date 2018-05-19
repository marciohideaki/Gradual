SELECT A.CD_CLIENTE AS Cliente,
	B.NM_CLIENTE AS Nome,
	TO_CHAR(C.DT_ULT_OPER,'DD/MM/YYYY') AS "Ultima Operacao",
	TO_CHAR(A.VL_TOTAL,'FM999999999999D00') AS SaldoD3,
	TO_CHAR(C.CD_CPFCGC,'FM999999999999999') AS CPFCNPJ,
	c.cd_ASSESSOR AS Assessor,
	D.NM_ASSESSOR AS "Nome Assessor"
 FROM DBMFINAN33 A, TSCCLIGER B, TSCCLIBOL C, TSCASSES D
 WHERE A.CD_CLIENTE = C.CD_CLIENTE AND
	C.TP_CLIENTE IN(01,02,03,18) AND
	C.CD_ASSESSOR = D.CD_ASSESSOR AND
	C.CD_CPFCGC = B.CD_CPFCGC AND
	C.CD_CON_DEP = B.CD_CON_DEP AND
	C.DT_NASC_FUND = B.DT_NASC_FUND AND
	A.DATA_posi = trunc(sysdate) AND
	A.VL_TOTAL > 30000.00
	/
SELECT A.CD_CLIENTE AS "Cliente",
 B.NM_CLIENTE AS "Nome",
 SUM(A.VL_LANCAMENTO) AS "Valor Deposito",
 TO_CHAR(A.DT_lancamento,'dd/mm/yyyy') AS "Data",
 C.VL_DISPONIVEL AS "Disponivel CC",
 TO_CHAR(E.CD_CPFCGC,'FM999999999999999') AS "CPF/CNPJ",
 E.CD_ASSESSOR AS "Assessor",
 D.NM_ASSESSOR AS "Nome Assessor"
FROM 
tccmovto a, tsccliger b, tccsaldo c, dbm2 d, TSCCLICC E, TSCASSES D
WHERE A.CD_HISTORICO IN(8,77) AND
A.DT_LANCAMENTO = D.DATA_FINAL AND
A.VL_LANCAMENTO > 10000.00 AND
A.CD_CLIENTE = E.CD_CLIENTE AND
E.CD_ASSESSOR = D.CD_ASSESSOR AND 
A.CD_CLIENTE = C.CD_CLIENTE AND
E.CD_CPFCGC = B.CD_CPFCGC AND
E.CD_CON_DEP = B.CD_CON_DEP AND
E.DT_NASC_FUND = B.DT_NASC_FUND AND
B.TP_CLIENTE IN(01,02,03,18) 
GROUP BY A.CD_CLIENTE,B.NM_CLIENTE,A.DT_LANCAMENTO,C.VL_DISPONIVEL,E.CD_CPFCGC,E.CD_ASSESSOR,D.NM_ASSESSOR
/
SELECT a.cd_cliente AS "Cliente",
       b.nm_cliente AS "Nome",
	   SUM(a.vl_cortot) AS "Receita DT",
	   MAX(to_char(a.dt_negocio,'dd/mm/yyyy')) AS "Ultimo DT",
       to_char(c.cd_cpfcgc,'fm999999999999999') AS "CPF/CNPJ",
	   c.cd_assessor AS "Assessor",
	   d.nm_assessor AS "Nome Assessor"
FROM v_tbomovcl a, tsccliger b, tscclibol c, tscasses d
WHERE A.DT_NEGOCIO BETWEEN trunc(sysdate) - interval '8' day AND trunc(sysdate) - interval '1' day AND
  INSTR(a.in_negocio,'D') > 0  AND 
  A.IN_BROKER = 'F' AND a.cd_cliente = c.cd_cliente and
  c.tp_cliente in(01,02,03,18) and
  c.cd_assessor = d.cd_assessor and
  c.cd_cpfcgc = b.cd_cpfcgc and
  c.cd_con_dep = b.cd_con_dep and
  c.dt_nasc_fund = b.dt_nasc_fund
GROUP BY a.cd_cliente,b.nm_cliente, c.cd_cpfcgc,c.cd_assessor, d.nm_assessor
ORDER BY a.cd_cliente

