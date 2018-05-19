using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Servico.ExportacaoGoldmanSachs.Teste
{
    public static class SqlDasViews
    {
        public static string V_GSFUTURO_NOVA
        {
            get
            {
                return @" SELECT * FROM GOLDMAN_BMF_LUCIANO WHERE DT_OPCOES >= TRUNC(to_date('{0}', 'DD/MM/YYYY'))";
            }
        }
        
        public static string V_GSFUTURO_COMPLEXA
        {
            get
            {
                return @"exec prc_Goldman_BMF_Anterior(to_date('{0}', 'DD/MM/YYYY'));";
            }
        }
        
        public static string V_GSOPCAO
        {
            get
            {
                return @"SELECT  
        a.nr_negocio as NR_NEGOCIO, 
        a.dt_datord as DT_DATORD, 
        b.hh_negocio as HH_NEGOCIO, 
        a.cd_negocio as  CD_NEGOCIO,
        b.ft_valorizacao as LOTE, 
        a.cd_natope as CD_NATOPE, 
        b.dt_opcoes as DT_OPCOES, 
        a.qt_qtdesp as QT_QTDESP,
        b.vl_negocio as VL_NEGOCIO, 
        c.vl_preexe as PRECO_EXEC, 
        d.pz_liq_fut as PAGTO_PREMIO, 
        d.pz_liq_fut as DATA_LIQUID,
        DECODE (b.tp_mercado, 'OPV', 'EUROPEAN', 'AMERICAN') as  ESTILO, 
        c.vl_prefec as  PRECO_FECHAMENTO,
        c.dt_limneg as DATAEXPIRACAO, 
        a.nr_seqord as NR_ORDEM,
        B.CD_CODISI as ISIN,
        DECODE(B.CD_OPERA_MEGA, 816,'S', 842,'S', 858,'S', 'N') as ROBO,
        a.cd_cliente as CD_CLIENTE
     FROM torcomi a, tornegd b, tbotitulos c, tbocalbol d
    WHERE d.dt_mov = TRUNC (to_date('{0}', 'DD/MM/YYYY'))
        AND d.cd_praca = 1
        AND a.dt_negocio = TRUNC (to_date('{0}', 'DD/MM/YYYY'))
        AND a.cd_cliente IN ({1})
        AND a.nr_negocio = b.nr_negocio
        AND a.dt_negocio = b.dt_pregao
        AND a.cd_natope = b.cd_natope
        AND a.cd_negocio = b.cd_negocio
        AND a.cd_negocio = c.cd_codneg
        AND c.dt_limneg >= TRUNC (to_date('{0}', 'DD/MM/YYYY'))
        AND c.dt_inineg <= TRUNC (to_date('{0}', 'DD/MM/YYYY'))
        AND b.tp_mercado IN ('OPC', 'OPV')";
            }
        }

        
        public static string V_GSVISTA
        {
            get
            {
                return @"  SELECT 
    a.nr_negocio as NR_NEGOCIO, 
    a.dt_datord as DT_DATORD, 
    b.hh_negocio as HH_NEGOCIO, 
    a.cd_negocio as CD_NEGOCIO,
    b.ft_valorizacao as LOTE, 
    a.cd_natope as CD_NATOPE, 
    b.dt_opcoes as DT_OPCOES, 
    a.qt_qtdesp as QT_QTDESP,
    b.vl_negocio as VL_NEGOCIO, 
    c.vl_prefec as PRECO_FECHAMENTO, 
    c.dt_limneg as DATAEXPIRACAO,
    A.NR_SEQORD as NR_ORDEM,
    B.CD_CODISI as ISIN,
    DECODE(B.CD_OPERA_MEGA, 816,'S', 842,'S', 858,'S', 'N') as ROBO,
    a.CD_CLIENTE as CD_CLIENTE
 FROM torcomi a, tornegd b, tbotitulos c
WHERE a.dt_negocio = TRUNC (to_date('{0}', 'DD/MM/YYYY'))
  AND a.cd_cliente IN ({1})
  AND a.nr_negocio = b.nr_negocio
  AND a.dt_negocio = b.dt_pregao
  AND a.cd_natope = b.cd_natope
  AND a.cd_negocio = b.cd_negocio
  AND b.tp_mercado = 'VIS'
  AND a.cd_negocio = c.cd_codneg
  AND c.dt_limneg >= TRUNC (to_date('{0}', 'DD/MM/YYYY'))
  AND c.dt_inineg <= TRUNC (to_date('{0}', 'DD/MM/YYYY'))";
            }
        }
    }
}
