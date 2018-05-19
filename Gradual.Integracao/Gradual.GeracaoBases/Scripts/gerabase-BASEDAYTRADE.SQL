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