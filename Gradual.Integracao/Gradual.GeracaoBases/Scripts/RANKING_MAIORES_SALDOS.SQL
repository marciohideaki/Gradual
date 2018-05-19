select a.cd_cliente, b.nm_cliente, A.VL_DISPONIVEL, sum(b.vl_corliq_tot), sum(b.vl_corneg),
to_char(c.cd_cpfcgc,'fm999999999999999'),c.cd_assessor, d.nm_assessor
from tccsaldo a, dbmfinan1 b, tscclibol c, tscasses d
where a.cd_cliente = b.cd_cliente and 
a.cd_cliente = c.cd_cliente and
c.tp_cliente in(01,02,03,18) and
c.cd_assessor = d.cd_assessor and
b.dt_negocio between '01-JAN-2015' AND '26-MAY-2015' 
GROUP BY a.cd_cliente, b.nm_cliente, A.VL_DISPONIVEL, C.CD_CPFCGC,C.CD_ASSESSOR, D.NM_ASSESSOR
ORDER BY A.VL_DISPONIVEL DESC 
/