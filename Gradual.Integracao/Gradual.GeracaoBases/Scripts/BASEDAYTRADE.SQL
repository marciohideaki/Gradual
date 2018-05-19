accept  arquivo1  char prompt 'Data inicial pregão (dd/mm/yyyy) : ' 
accept  arquivo2  char prompt 'Data final   pregão (dd/mm/yyyy) : ' 

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

SPOOL C:\BASEDAYTRADE.XLS

SELECT 'Cliente','Nome','Receita DT','Ultimo DT','CPF/CNPJ', 'Assessor','Nome'
from dual
/
select a.cd_cliente,b.nm_cliente,sum(TO_CHAR(a.vl_cortot,'FM99999999D00')),max(to_char(a.dt_negocio,'dd/mm/yyyy')),
to_char(c.cd_cpfcgc,'fm999999999999999'), c.cd_assessor, d.nm_assessor
from v_tbomovcl a, tsccliger b, tscclibol c, tscasses d
where A.DT_NEGOCIO BETWEEN TO_DATE('&arquivo1','dd/mm/yy') AND TO_DATE('&arquivo2','dd/mm/yy') AND
instr(a.in_negocio,'D') > 0  AND 
A.IN_BROKER = 'F' AND a.cd_cliente = c.cd_cliente and
c.tp_cliente in(01,02,03,18) and
c.cd_assessor = d.cd_assessor and
c.cd_cpfcgc = b.cd_cpfcgc and
c.cd_con_dep = b.cd_con_dep and
c.dt_nasc_fund = b.dt_nasc_fund
group by a.cd_cliente,b.nm_cliente, c.cd_cpfcgc,c.cd_assessor, d.nm_assessor
order by a.cd_cliente
/
SPOOL OFF
EXIT
