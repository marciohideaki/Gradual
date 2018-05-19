SELECT to_char(c.cd_cliente) AS "Cliente",
       b.nm_cliente AS "Nome",
       to_char(c.cd_cpfcgc,'fm999999999999999') AS "CPF/CNPJ",
	   to_char(c.cd_assessor) AS "Assessor",
	   d.nm_assessor AS "Nome Assessor"
FROM tsccliger b, tscclibol c, tscasses d
WHERE c.tp_cliente in(01,02,03,18) and
  c.cd_assessor = d.cd_assessor and
  c.cd_cpfcgc = b.cd_cpfcgc and
  c.cd_con_dep = b.cd_con_dep and
  c.dt_nasc_fund = b.dt_nasc_fund
GROUP BY c.cd_cliente,b.nm_cliente, c.cd_cpfcgc,c.cd_assessor, d.nm_assessor