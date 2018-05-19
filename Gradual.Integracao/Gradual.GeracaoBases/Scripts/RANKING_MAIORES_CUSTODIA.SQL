SET SERVEROUT ON pages 50000 lines 300 feed off
exec DBMS_OUTPUT.ENABLE(NULL);
-- declaracao cursor c1: 
exec DBMS_OUTPUT.PUT_LINE('==[ Inicio ]================================================');
exec DBMS_OUTPUT.PUT_LINE('COD - NOME_CLIENTE - COD_CART - CUSTODIA - COD_NEG - VAL_POSI');
exec DBMS_OUTPUT.PUT_LINE('--------------------------------------------------------------------');
DECLARE
   CURSOR c1 IS
          SELECT A.CD_CLIENTE 
            FROM DBMFINAN33 A, VCFPOSI_VALO B, TSCCLIBOL C
           WHERE A.CD_CLIENTE = B.COD_CLI AND
                 A.CD_CLIENTE = C.CD_CLIENTE AND 
                 C.TP_CLIENTE IN(01,02,03,18)
             AND A.DATA_POSI = TRUNC(SYSDATE)
             AND B.VAL_POSI != 0
          group by A.CD_CLIENTE 
           ORDER by a.cd_cliente ;
    -- variavel do tipo c1
    CONTA          	NUMBER       := 0  ;
    V_NOME_CLIENTE 	VARCHAR2(60) := '' ; 
    V_NOME_ASSE 	VARCHAR2(20) := '' ; 
    V_CD_ASSESSOR   	NUMBER       := 0  ;
    V_COD_CPF_CNPJ  	NUMBER       := 0  ;
    V_CD_CLIENTE   	NUMBER       := 0  ;
    V_COD_CART     	NUMBER       := 0  ;
    V_CUSTODIA     	NUMBER       := 0  ; 
    V_COD_NEG      	VARCHAR2(10) := '' ; 
    V_VAL_POSI     	NUMBER       := 0  ;
    r1 c1%ROWTYPE;
   BEGIN
        OPEN c1;
             FETCH c1 INTO r1;
             LOOP
                 EXIT WHEN c1%NOTFOUND;
                       -- comentario: Pega PAPEL com MAIOR VALOR de cada CLIENTE
                       select CD_CLIENTE, NOME_CLIENTE, COD_CART, CUSTODIA, COD_NEG, VAL_POSI,COD_ASSE,NOME_ASSE,COD_CPF_CNPJ
                              into  V_CD_CLIENTE, V_NOME_CLIENTE, V_COD_CART, V_CUSTODIA, V_COD_NEG, V_VAL_POSI,V_CD_ASSESSOR,V_NOME_ASSE,V_COD_CPF_CNPJ from
                        (
                                      SELECT A.CD_CLIENTE CD_CLIENTE, B.NOME_CLI NOME_CLIENTE, B.COD_CART, A.CUSTODIA CUSTODIA, B.COD_NEG COD_NEG, B.VAL_POSI VAL_POSI,
                                             B.COD_ASSE,B.NOME_ASSE,B.COD_CPF_CNPJ  
                                        FROM DBMFINAN33 A, VCFPOSI_VALO B, TSCCLIBOL C
                                       WHERE A.CD_CLIENTE = r1.CD_CLIENTE AND
                                             A.CD_CLIENTE = B.COD_CLI     AND
                                             A.CD_CLIENTE = C.CD_CLIENTE AND
                                             C.TP_CLIENTE IN(01,02,03,18) AND
                                             A.DATA_POSI = TRUNC(SYSDATE)
                                       ORDER by b.val_posi desc
                        ) where rownum =1 ;
                        CONTA:= CONTA + 1 ;
                        DBMS_OUTPUT.PUT_LINE(V_CD_CLIENTE||';'||V_NOME_CLIENTE||';'||V_COD_CART||';'||V_CUSTODIA||';'||V_COD_NEG||';'||V_VAL_POSI||';'||
                        V_CD_ASSESSOR||';'||V_NOME_ASSE||';'||V_COD_CPF_CNPJ||';');
                 FETCH c1 INTO r1;
             END LOOP;
             DBMS_OUTPUT.PUT_LINE('==[ Total: '||CONTA||' - Fim ]=========================================');
        CLOSE c1;
END;
/

