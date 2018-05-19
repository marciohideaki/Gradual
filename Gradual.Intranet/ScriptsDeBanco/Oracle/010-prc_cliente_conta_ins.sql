create or replace PROCEDURE prc_cliente_conta_ins
( pCD_CLIENTE    NUMBER
, pCD_BANCO      VARCHAR2
, pCD_AGENCIA    VARCHAR2
, pDV_AGENCIA    CHAR
, pNR_CONTA      VARCHAR2
, pDV_CONTA      CHAR
, pIN_PRINCIPAL  CHAR
, pIN_INATIVA    CHAR
, pCD_EMPRESA    NUMBER
, pCD_USUARIO    NUMBER
, pTP_OCORRENCIA CHAR
, pTP_CONTA      VARCHAR2
, pIN_CONJUNTA   VARCHAR2)
IS
BEGIN
    INSERT INTO tscclicta
           (    cd_cliente
           ,    cd_banco
           ,    cd_agencia
           ,    dv_agencia
           ,    nr_conta
           ,    dv_conta
           ,    in_principal
           ,    in_inativa
           ,    cd_empresa
           ,    cd_usuario
           ,    tp_ocorrencia
           ,    tp_conta
           ,    in_conjunta)
    VALUES (    pCD_CLIENTE
            ,   pCD_BANCO
            ,   pCD_AGENCIA
            ,   pDV_AGENCIA
            ,   pNR_CONTA
            ,   pDV_CONTA
            ,   pIN_PRINCIPAL
            ,   pIN_INATIVA
            ,   pCD_EMPRESA
            ,   pCD_USUARIO
            ,   pTP_OCORRENCIA
            ,   pTP_CONTA
            ,   pIN_CONJUNTA);
END;