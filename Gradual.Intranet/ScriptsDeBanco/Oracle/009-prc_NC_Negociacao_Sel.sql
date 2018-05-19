create or replace
PROCEDURE prc_NC_Negociacao_Sel
( cd_cliente IN NUMBER
, dt_negocio IN DATE
, dt_datmov IN DATE
, cd_sistema IN VARCHAR2 default 'BOL'
, tp_negocio IN VARCHAR2 default 'NOR'
, cd_bolfat IN NUMBER default 1
--, in_broker IN VARCHAR2 default 'F'
, Retorno out globalpkg.Retorno)
AS
BEGIN
    OPEN Retorno FOR
    Select
        d.nm_resu_bolsa
        , a.cd_natope
        , c.ds_mercado
        , decode(a.tp_vcoter,null,to_char(a.dt_opcoes,'MM/YY'), to_char(a.tp_vcoter,'9999')) tp_vcoter
        , a.nm_nomres
        , a.cd_especif
        , sum(a.qt_qtdesp) qt_qtdesp
        , a.vl_negocio
        , sum(abs(a.vl_total)) vl_total
        , a.cd_negocio
        , a.cd_bolsamov
        , a.in_liquida
        , a.cd_codisi
        , decode(a.nr_negocio,0,a.vl_total,0) nr_negocio_vl_total
        , decode(a.nr_negocio,0,0,1) nr_negocio
        , sum(nvl(a.vl_corresp,0)) vl_corresp
        , sum(nvl(a.vl_iss_corresp,0)) vl_iss_corresp
        , a.tp_mercado
        , replace(a.in_negocio,'N',null) in_negocio
        , sum(a.vl_total)  vl_totneg
/*      , sum(a.vl_liqoper) vl_liqoper
        , sum(a.vl_emolum_bv) vl_emolum_bv
        , sum(a.vl_emolum_cb) vl_emolum_cb
        , sum(a.vl_taxreg_bv) vl_taxreg_bv
        , sum(a.vl_taxreg_cb) vl_taxreg_cb
        , sum(a.vl_taxana) vl_taxana
        , sum(a.vl_corret) vl_corret
        , sum(a.vl_corresp) vl_corresp
        , sum(a.vl_valdes) vl_valdes
        , sum(a.vl_irretido) vl_irretido
        , sum(a.vl_iroper) vl_iroper
        , sum(a.vl_iss) vl_iss*/
        --, d.cd_sistema
        --, a.tp_negocio
    from
        V_TORDETNOTA a
        , tbomercado c
        , tgebolsa d
    where
        a.cd_cliente_fin = prc_NC_Negociacao_Sel.cd_cliente --39382
        and a.dt_negocio = prc_NC_Negociacao_Sel.dt_negocio --'31-MAR-2010'
        and c.cd_almerc = a.tp_mercado
        and a.cd_bolfat = prc_NC_Negociacao_Sel.cd_bolfat -- 1
        and d.cd_bolsa = a.cd_bolsamov
        and d.cd_sistema = prc_NC_Negociacao_Sel.cd_sistema -- 'BOL'
        and a.tp_negocio = prc_NC_Negociacao_Sel.tp_negocio -- 'NOR'
        and a.dt_datmov  = prc_NC_Negociacao_Sel.dt_negocio -- '31-MAR-2010'
        --and a.in_broker = prc_NC_Negociacao_Sel.in_broker
    group by
        decode(sign(instr(a.in_negocio,'A',1)),1,0,1)
        , a.in_negocio
        , a.nr_negocio
        , d.nm_resu_bolsa
        , a.cd_natope
        , c.ds_mercado
        , a.tp_mercado
        , a.dt_opcoes
        , a.nm_nomres
        , a.cd_especif
        , a.cd_negocio
        , a.cd_codisi
        , a.tp_vcoter
        , a.cd_bolsamov
        , a.in_liquida
        , decode(a.nr_negocio,0,a.vl_total,0)
        , decode(a.nr_negocio,0,0,1)
        , a.vl_negocio
        --, d.cd_sistema
        --, a.tp_negocio
    order by
        decode(sign(instr(a.in_negocio,'A',1)),1,0,1)
        ,d.nm_resu_bolsa
        , c.ds_mercado
        , a.cd_natope
        , a.nm_nomres
        , a.cd_especif
        , a.nr_negocio;
END;