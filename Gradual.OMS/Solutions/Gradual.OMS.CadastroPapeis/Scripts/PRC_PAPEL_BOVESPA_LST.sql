create or replace
PROCEDURE PRC_PAPEL_BOVESPA_LST (
Retorno              OUT GLOBALPKG.Retorno
)
AS
BEGIN
  OPEN Retorno FOR 
  SELECT
    DISTINCT(A.CD_CODNEG) as CD_CODNEG,
    A.CD_CODISI     , -- CODISIN
    A.NM_NOMPRE     , -- NOMPRE
    A.CD_TPMERC     , -- NOMERC
    A.NM_NOMERC     , -- TipoMercado
    A.NR_DISMEX     , -- DISMEX
    TO_DATE(A.DT_DATVEN,'YYYY/MM/DD')  as  DT_DATVEN  , -- DATVEN
    A.VL_PREFEC  , -- PREFEC
    TO_DATE(A.DT_DATFCH,'YYYY/MM/DD') as DT_DATFCH,     -- DATFCH
    A.VL_PRMDAN     , -- PRMDAN
    A.NR_FATCOT, -- FATOR_COTACAO
    A.NR_LOTNEG  , -- LOTE MINIMO
    A.VL_PREEXE  ,     -- PRE�O DO EXERC�CIO
    A.DT_ININEG   , 
    A.CD_CODSET   , 
    A.NM_NOMSET
  FROM
    TBOTITULOS_T      A
  --WHERE 
  --TO_CHAR(SYSDATE, 'YYYYMMDD') BETWEEN DT_ININEG AND DT_LIMNEG
  ORDER BY A.DT_ININEG DESC;
  --WHERE
    --A.CD_CODNEG             = B.CD_NEGOCIO
    --AND C.COD_NEG           = A.CD_CODNEG
    --AND SYSDATE  BETWEEN C.DATA_INI_LOTE_NEG  AND C.DATA_FIM_LOTE_NEG
END PRC_PAPEL_BOVESPA_LST;
