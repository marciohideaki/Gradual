
create or replace procedure prc_exp_iss(
 pCD_CLIENTE in int
)
  --Lilia solicitou que fosse alterado de S para L em 11/11/09
AS
Begin
	update 
		tscclibol 
	set 
		CD_USUARIO=151, 
		IN_ISS='L' 
	where 
		cd_cliente = pCD_CLIENTE;
End;
/





create or replace procedure prc_exp_sfp(
 pCD_CPFCGC     in NUMBER,
 pDT_NASC_FUND  in DATE,
 pCD_CON_DEP    in NUMBER,
 pCD_SFPGRUPO   in NUMBER,
 pVL_BEN        in NUMBER,
 pTM_STAMP      in DATE,
 pDS_BEN        in VARCHAR2,
 pCD_SFPSUBGRUPO  in NUMBER default 1,
 pIN_ONUS       in CHAR default 'N',
 pVL_DEVEDOR    in NUMBER default 0,
 pDT_VENCIMENTO   in DATE default null,
 pSG_ESTADO     in VARCHAR2  default null
)
AS
  pCount number;
  pNR_SEQ_SFP number;
  pPC_LIMITE  number;
Begin

 --Calculando o percentual de limite
        select
   TSCSFPCLA.PC_LIMITE_PF
  into
   pPC_LIMITE
         from
           TSCSFPCLA,
        TSCSFPSUBGRU
          where
   TSCSFPSUBGRU.CD_GRUPO  = pCD_SFPGRUPO and
         TSCSFPCLA.CD_CLASSIFICACAO = TSCSFPSUBGRU.CD_CLASSIFICACAO ;
 --Calculando o Numero sequencial da SFP
      select count(*) into   pCount
      from   tscsfp
      where
            CD_CPFCGC = pCD_CPFCGC and
            DT_NASC_FUND = pDT_NASC_FUND and
            CD_CON_DEP =  pCD_CON_DEP;
      if pCount = 0 then
            pNR_SEQ_SFP := 1;
      else
            select  max(NR_SEQ_SFP) + 1
            into    pNR_SEQ_SFP
            from    tscsfp
            where
                  CD_CPFCGC = pCD_CPFCGC and
                  DT_NASC_FUND = pDT_NASC_FUND and
                  CD_CON_DEP =  pCD_CON_DEP;
      End if;
 insert into tscsfp (
   CD_CPFCGC ,
   DT_NASC_FUND ,
   CD_CON_DEP ,
   NR_SEQ_SFP ,
   CD_SFPGRUPO ,
   CD_SFPSUBGRUPO ,
   PC_LIMITE   ,
   DS_BEN   ,
   VL_BEN  ,
   IN_ONUS  ,
   VL_DEVEDOR   ,
   DT_VENCIMENTO  ,
   SG_ESTADO   ,
   TM_STAMP
 )values(
   pCD_CPFCGC   ,
   pDT_NASC_FUND ,
   pCD_CON_DEP  ,
   pNR_SEQ_SFP  ,
   pCD_SFPGRUPO  ,
   pCD_SFPSUBGRUPO ,
   pPC_LIMITE  ,
   pDS_BEN    ,
   pVL_BEN   ,
   pIN_ONUS  ,
   pVL_DEVEDOR  ,
   pDT_VENCIMENTO ,
   pSG_ESTADO   ,
   pTM_STAMP
  );
End;
/





create or replace procedure prc_exp_bol(
 pCD_CLIENTE in int,
 pTP_INVESTIDOR int
)
AS
Begin
	update 
		tscclibol 
	set 
		CD_USUARIO=151, 
		nr_seq_e_mail = 1, 
		cd_relatorio =3, 
		tp_investidor = pTP_INVESTIDOR , 
		cd_liquidante = null, 
		dv_usua_comp = null , 
		cd_usua_comp = null,
		DT_ATUALIZ = sysdate
    where 
		CD_CLIENTE = pCD_CLIENTE;
End;
/





create or replace procedure prc_exp_basag(
 pCD_CLIENTE in int,
 pCD_ASSESSOR in int
)
AS
Begin
----Ver CD_USUARIO
--	select 
--		cd_usuario 
--	from 
--		TSCCBASAG 
--	where 
--		CD_CLIENTE = pCD_CLIENTE ;
--Excluir agentes do cliente
	delete 
	from 
		TSCCBASAG 
	where 
		CD_CLIENTE = pCD_CLIENTE;
--Alterar Assessor
	update 
		TSCCLIBOL 
	set  
		CD_USUARIO=151,  
		CD_ASSESSOR = pCD_ASSESSOR
	where 
		CD_CLIENTE = pCD_CLIENTE;
--Voltar Agentes
	insert into 
		TSCCBASAG 
		( CD_CLIENTE , CD_ASSESSOR  , CD_AGENTE  , IN_PRINCIPAL  ,  CD_EMPRESA , CD_USUARIO , TP_OCORRENCIA  )
	Values 
		( pCD_CLIENTE , pCD_ASSESSOR , 0 , 'S'  , 227 , 151 , 'D' );
End;
/






create or replace procedure prc_exp_cta(
  pCD_CLIENTE in int
)
AS
Begin
	update 
		tscclicta 
	set 
		CD_USUARIO=151, 
		in_inativa = 'S' 
	where 
		cd_cliente = pCD_CLIENTE;
End;
/






create or replace procedure prc_exp_cc(
 pCD_CLIENTE in int,
 pCD_ASSESSOR in int
)
AS
Begin
	update 
		tscclicc 
	set 
		CD_USUARIO=151,
		cd_assessor = pCD_ASSESSOR ,
		DT_ATUALIZ = sysdate
	where 
		CD_CLIENTE = pCD_CLIENTE;
End;
/





create or replace procedure prc_exp_sis(
 pCD_CLIENTE in int
)
AS
Begin
	insert into 
		tscclisis 
		(CD_CLIENTE,CD_SISTEMA,CD_CLISIS) 
	values
		( pCD_CLIENTE,'OUT', pCD_CLIENTE);              
End;
/





create or replace procedure prc_exp_ende(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 pNM_E_MAIL in varchar2
) 
AS
Begin
	update 
		tscende 
	set 
		CD_USUARIO=151,
		nm_e_mail = pNM_E_MAIL,
        in_ende_oficial = 'S' ,
		IN_ENDE_CORR = 'S' ,
		DT_ATUALIZ_CCLI = sysdate
    where 
		CD_CON_DEP = pCD_CON_DEP 
		and CD_CPFCGC = pCD_CPFCGC
		and  DT_NASC_FUND =pDT_NASC_FUND
        and  NR_SEQ_ENDE = 1 ;
End;
/





create or replace procedure prc_exp_cus(
 pCD_CLIENTE in int,
 pCD_ASSESSOR in int
)
AS
Begin
	update 
		tscclicus 
	set
		CD_USUARIO=151, 
        cd_assessor =  pCD_ASSESSOR ,
		DT_ATUALIZ = sysdate
    where 
        CD_CLIENTE = pCD_CLIENTE;
End;
/





create or replace procedure prc_exp_ger(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 pCD_ASSESSOR in int
)
AS
Begin
	update 
		tsccliger 
	set 
        CD_USUARIO=151, 
        cd_assessor = pCD_ASSESSOR,
        PC_SFPPESO = 100, -- 100% para SPF
		DT_ATUALIZ = sysdate
    where 
		CD_CON_DEP = pCD_CON_DEP
		and CD_CPFCGC = pCD_CPFCGC
		and  DT_NASC_FUND = pDT_NASC_FUND;
End;
/





create or replace procedure prc_exp_docs(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 pCHEK_COMPROVANTES in char,
 pCHEK_BALANCO in char,
 pDT_BAL_PATRIMONIAL in date,
 pIN_PROCUR in char,
 pDT_VAL_PROCUR in date
)
AS
Begin
	insert into 
		TSCDOCS (
		in_comp_res ,
        in_cpfcgc ,
        in_doc_ident ,
        in_situac_financ ,
        in_contr_bolsa ,
        in_fich_cad ,
        dt_contr_bolsa ,
        dt_fich_cad ,
        dt_validade ,
        CD_CON_DEP ,
        CD_CPFCGC ,
        DT_NASC_FUND,
        IN_PROCUR,
        IN_CONTR_BMF,
        IN_CONTR_SOCIAL,
        IN_CONTR_OPC,
        IN_CONTR_TER,
        IN_ATA_CONTRAT,
        IN_FICHA_BMF,
        IN_CGC,
        IN_RG,
        IN_PROPOSTA,
        IN_AUT_OPER,
        IN_DOC_ADM,
        IN_AUTENTIC,
        IN_CONTR_INTERNET,
        IN_BAL_PATRIMONIAL, 
        IN_REGULAMENTO,
        IN_CONTR_TST,
        IN_CONTR_BTC,
        IN_WEB_TRADING ,
        IN_CONTA_MARGEM,
		DT_BAL_PATRIMONIAL,
		DT_VAL_PROCUR )
	values (
        pCHEK_COMPROVANTES,pCHEK_COMPROVANTES,pCHEK_COMPROVANTES,'S','S','S', 
        null, 
        null, 
        add_months(sysdate,24), --Adicionando 2 anos
        pCD_CON_DEP , 
        pCD_CPFCGC , 
        pDT_NASC_FUND,
        pIN_PROCUR,'N','N','S','S','N','N','N','N','N','N','N','N','S',pCHEK_BALANCO,'N','S','S','N','S',pDT_BAL_PATRIMONIAL,pDT_VAL_PROCUR);
End;
/





create or replace procedure prc_exp_ver_dia_feriado(
 pDATA in date,
 pFERIADO out int
)
AS
Begin
	Select 
		count(*) 
	into 
		pFERIADO
	from 
		tgeferia
	where	
		TP_FERIADO = 'N' -- Nascional
		and CD_PRACA = 1 --Sao Paulo
		and CD_SISTEMA = 'CCO' --Conta corrente
		and trunc(DT_FERIADO) = trunc(pDATA);
End;
/





create or replace procedure prc_exp_cobrcus(
 pCD_CLIENTE in int,
 pDT_ULT_COBRANCA in date
)
AS
Begin
	update 
		tscclicus 
	set
		CD_USUARIO=151, 
        DT_ULT_COBRANCA =  trunc(pDT_ULT_COBRANCA) ,
		DT_ATUALIZ = sysdate
    where 
        CD_CLIENTE = pCD_CLIENTE;
End;
/





create or replace procedure prc_exp_del_sfp(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int     
)
AS
Begin
		delete
		from 
			tscsfp
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP ;
End;
/




create or replace procedure prc_exp_del_diretor_emitente(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int     
)
AS
	pCountBMF int;
Begin
--1 - Ao Excluir Emitente, não excluir Emitentes BMF, pois os mesmos não serão Re-exportados, evitando cobrança pela conta BMF. 
--2 - Para PF é exportado registro na tabela de Diretor, pois a PK de Diretor é FK em Emitente.
		-- Pelo motivo 1 - Não excluir mais o relacionamento entre o emitente BMF e a Conta BMF do Cliente
		--delete
		--from 
		--	tsccliemitordem
		--where 
		--	CD_CPFCGC = pCD_CPFCGC
		--	and DT_NASC_FUND = pDT_NASC_FUND
		--	and CD_CON_DEP = pCD_CON_DEP ;		
		delete
		from 
			tscemitordem
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP 
			-- Pelo motivo 1 - Não Excluir Emitentes BMF
			and CD_SISTEMA <> 'BMF';
		-- Pelo motivo 1 - Excluir apenas Diretor, se não houver registro em emitente (emitente BMF)
		select count(*) into pCountBMF from tscemitordem
		where CD_CPFCGC = pCD_CPFCGC and DT_NASC_FUND = pDT_NASC_FUND and CD_CON_DEP = pCD_CON_DEP; 
		if (pCountBMF = 0) then
			delete
			from 
				tsccvm220
			where 
				CD_CPFCGC = pCD_CPFCGC
				and DT_NASC_FUND = pDT_NASC_FUND
				and CD_CON_DEP = pCD_CON_DEP ;
		End If;
End;
/




create or replace procedure prc_exp_diretor(
pCD_CPFCGC       in NUMBER,
pDT_NASC_FUND    in DATE,
pCD_CON_DEP      in NUMBER,
pNM_DIRETOR_1    in VARCHAR2 default null,
pCD_CPFCGC_DIR1  in NUMBER default null,
pCD_DOC_IDENT_DIR1   in VARCHAR2 default null,
pNM_EMIT_ORDEM_1     in VARCHAR2 default null,
pCD_CPFCGC_EMIT1     in NUMBER default null,
pCD_DOC_IDENT_EMIT1  in VARCHAR2 default null,
pNM_DIRETOR_2        in VARCHAR2 default null,
pCD_CPFCGC_DIR2      in NUMBER default null,
pCD_DOC_IDENT_DIR2   in VARCHAR2 default null,
pNM_EMIT_ORDEM_2     in VARCHAR2 default null,
pCD_CPFCGC_EMIT2     in NUMBER default null,
pCD_DOC_IDENT_EMIT2  in VARCHAR2 default null,
pNM_DIRETOR_3        in VARCHAR2 default null,
pCD_CPFCGC_DIR3      in NUMBER default null,
pCD_DOC_IDENT_DIR3   in VARCHAR2 default null,
pDS_FORMACAO         in VARCHAR2 default null,
pNR_INSCRICAO        in VARCHAR2 default null
)
AS
	 pCountDiretor int;
Begin
--1 - Ao Excluir Emitente, não excluir Emitentes BMF, pois os mesmos não serão Re-exportados, evitando cobrança pela conta BMF. 
--2 - Para PF é exportado registro na tabela de Diretor, pois a PK de Diretor é FK em Emitente.
--3 - Verificar se Diretor existe e alterar se existir
 --pNM_FONT_REF_1    null
 --pNM_FONT_REF_2     null
 --pCD_CLIE_CART_ADM   null 
 -- pCD_EMPRESA  227
 -- pCD_USUARIO  1
 -- pTP_OCORRENCIA I
 -- pTM_STAMP sysdate
	-- Motivo 3 - ver se existe Diretor, pois se ficou algum emitente BMF, o Diretor não foi excluído e deve ser alterado
	select count(*) into pCountDiretor from tsccvm220 
	where CD_CPFCGC = pCD_CPFCGC and DT_NASC_FUND = pDT_NASC_FUND and CD_CON_DEP = pCD_CON_DEP ; 
	if (pCountDiretor=0) then
		insert into tsccvm220(
			CD_CPFCGC  ,DT_NASC_FUND  ,
			CD_CON_DEP  ,NM_DIRETOR_1   ,
			CD_CPFCGC_DIR1 ,CD_DOC_IDENT_DIR1  ,
			NM_DIRETOR_2 ,CD_CPFCGC_DIR2 ,
			CD_DOC_IDENT_DIR2 ,	NM_DIRETOR_3  ,
			CD_CPFCGC_DIR3 ,CD_DOC_IDENT_DIR3 ,
			NM_EMIT_ORDEM_1 ,CD_CPFCGC_EMIT1 ,
			CD_DOC_IDENT_EMIT1 ,NM_EMIT_ORDEM_2   ,
			CD_CPFCGC_EMIT2   ,	CD_DOC_IDENT_EMIT2 ,
			DS_FORMACAO  ,	NM_FONT_REF_1  ,
			NM_FONT_REF_2  ,CD_CLIE_CART_ADM  ,
			NR_INSCRICAO  ,	CD_EMPRESA ,
			CD_USUARIO  ,	TP_OCORRENCIA ,
			TM_STAMP )
		values(
			pCD_CPFCGC   ,pDT_NASC_FUND  ,
			pCD_CON_DEP ,pNM_DIRETOR_1 ,
			pCD_CPFCGC_DIR1 ,pCD_DOC_IDENT_DIR1 ,
			pNM_DIRETOR_2 ,	pCD_CPFCGC_DIR2 ,
			pCD_DOC_IDENT_DIR2 ,pNM_DIRETOR_3   ,
			pCD_CPFCGC_DIR3  ,pCD_DOC_IDENT_DIR3  ,
			pNM_EMIT_ORDEM_1  ,	pCD_CPFCGC_EMIT1 ,
			pCD_DOC_IDENT_EMIT1 ,pNM_EMIT_ORDEM_2 ,
			pCD_CPFCGC_EMIT2 ,	pCD_DOC_IDENT_EMIT2 ,
			pDS_FORMACAO  ,	null ,
			null ,	null ,
			pNR_INSCRICAO ,	227,
			151,'I',
			sysdate);
	else
		update tsccvm220 
		set 
			NM_DIRETOR_1		=pNM_DIRETOR_1,
			CD_CPFCGC_DIR1		=pCD_CPFCGC_DIR1,
			CD_DOC_IDENT_DIR1	=pCD_DOC_IDENT_DIR1,
			NM_DIRETOR_2 		=pNM_DIRETOR_2,
			CD_CPFCGC_DIR2 		=pCD_CPFCGC_DIR2,
			CD_DOC_IDENT_DIR2 	=pCD_DOC_IDENT_DIR2,	
			NM_DIRETOR_3  		=pNM_DIRETOR_3,
			CD_CPFCGC_DIR3 		=pCD_CPFCGC_DIR3,
			CD_DOC_IDENT_DIR3 	=pCD_DOC_IDENT_DIR3,
			NM_EMIT_ORDEM_1 	=pNM_EMIT_ORDEM_1,
			CD_CPFCGC_EMIT1 	=pCD_CPFCGC_EMIT1,
			CD_DOC_IDENT_EMIT1 	=pCD_DOC_IDENT_EMIT1,
			NM_EMIT_ORDEM_2   	=pNM_EMIT_ORDEM_2,
			CD_CPFCGC_EMIT2   	=pCD_CPFCGC_EMIT2,	
			CD_DOC_IDENT_EMIT2 	=pCD_DOC_IDENT_EMIT2,
			DS_FORMACAO  		=pDS_FORMACAO,	
			NM_FONT_REF_1  		=null,
			NM_FONT_REF_2  		=null,
			CD_CLIE_CART_ADM  	=null, 
			NR_INSCRICAO  		=pNR_INSCRICAO,	
			CD_EMPRESA 			=227,
			CD_USUARIO  		=151,	
			TP_OCORRENCIA 		='I',
			TM_STAMP 		=sysdate	
		where CD_CPFCGC = pCD_CPFCGC and DT_NASC_FUND = pDT_NASC_FUND and CD_CON_DEP = pCD_CON_DEP ;
	end if;
End;
/



create or replace procedure prc_exp_emitente
(pCD_CPFCGC          in    NUMBER,
 pDT_NASC_FUND       in    DATE,
 pCD_CON_DEP         in    NUMBER,
 pNM_EMIT_ORDEM      in    VARCHAR2,
 pCD_CPFCGC_EMIT     in    NUMBER,
 pCD_DOC_IDENT_EMIT  in    VARCHAR2,
 pIN_PRINCIPAL       in    int,
 pCD_SISTEMA         in    VARCHAR2,
 pNM_E_MAIL          in    VARCHAR2,
 pTM_STAMP           in	   DATE
)
as
  pCount number;
  pNR_SEQ_EMIT number;
  pPrincipal char;
Begin
--1 - Ao Excluir Emitente, não excluir Emitentes BMF, pois os mesmos não serão Re-exportados, evitando cobrança pela conta BMF. 
--2 - Para PF é exportado registro na tabela de Diretor, pois a PK de Diretor é FK em Emitente.
--3 - Não exportar emitente BMF
-- pNR_SEQ_EMIT calcular  
-- pCD_EMPRESA 227  
-- pCD_USUARIO 1 
-- pTP_OCORRENCIA I 
-- pTM_STAMP sysdate  
	--Motivo 3 - Não exportar BMF
	if (pCD_SISTEMA <> 'BMF') then
		if pIN_PRINCIPAL = 1 then
			pPrincipal := 'S';
		else
			pPrincipal := 'N';
		end if;
		  --Calculando o Numero sequencial do Emitente
		  select count(*) into   pCount 
		  from   tscemitordem
		  where
				CD_CPFCGC = pCD_CPFCGC and
				DT_NASC_FUND = pDT_NASC_FUND and
				CD_CON_DEP =  pCD_CON_DEP;
		  if pCount = 0 then
				pNR_SEQ_EMIT := 1; --select 1 into pNR_SEQ_EMIT from dual;
		  else
				select  max(NR_SEQ_EMIT) + 1
				into    pNR_SEQ_EMIT 
				from    tscemitordem
				where
					  CD_CPFCGC = pCD_CPFCGC and
					  DT_NASC_FUND = pDT_NASC_FUND and
					  CD_CON_DEP =  pCD_CON_DEP;
		  End if;
		  insert 
		  into 
				tscemitordem(
				CD_CPFCGC,
				DT_NASC_FUND,
				CD_CON_DEP,
				NM_EMIT_ORDEM,
				CD_CPFCGC_EMIT,
				CD_DOC_IDENT_EMIT,
				IN_PRINCIPAL,
				CD_SISTEMA,
				NM_E_MAIL,        
				NR_SEQ_EMIT,
				CD_EMPRESA,
				CD_USUARIO,
				TP_OCORRENCIA,
				TM_STAMP)
		  values(
				pCD_CPFCGC,
				pDT_NASC_FUND,
				pCD_CON_DEP,
				pNM_EMIT_ORDEM,
				pCD_CPFCGC_EMIT,
				pCD_DOC_IDENT_EMIT,
				pPrincipal,
				pCD_SISTEMA,
				pNM_E_MAIL,         
				pNR_SEQ_EMIT,
				227,
				151,
				'I',
				pTM_STAMP);
	end if;
-- Motivo 3 - Não vincular à conta BMF, pois a mesma não será cadastrada
   --insert into tsccliemitordem
end;
/





create or replace procedure prc_exp_sfp(
 pCD_CPFCGC     in NUMBER,
 pDT_NASC_FUND  in DATE,
 pCD_CON_DEP    in NUMBER,
 pCD_SFPGRUPO   in NUMBER,
 pVL_BEN        in NUMBER,
 pTM_STAMP      in DATE,
 pDS_BEN        in VARCHAR2,
 pCD_SFPSUBGRUPO  in NUMBER default 1,
 pIN_ONUS       in CHAR default 'N',
 pVL_DEVEDOR    in NUMBER default 0,
 pDT_VENCIMENTO   in DATE default null,
 pSG_ESTADO     in VARCHAR2  default null
)
AS
  pCount number;
  pNR_SEQ_SFP number;
  pPC_LIMITE  number;
Begin
    
	--Calculando o percentual de limite
        select 
			TSCSFPCLA.PC_LIMITE_PF 
		into
			pPC_LIMITE
         from 
          	TSCSFPCLA,
      		TSCSFPSUBGRU 
          where
			TSCSFPSUBGRU.CD_GRUPO  = pCD_SFPGRUPO and
       		TSCSFPCLA.CD_CLASSIFICACAO = TSCSFPSUBGRU.CD_CLASSIFICACAO ;
	--Calculando o Numero sequencial da SFP
      select count(*) into   pCount 
      from   tscsfp
      where
            CD_CPFCGC = pCD_CPFCGC and
            DT_NASC_FUND = pDT_NASC_FUND and
            CD_CON_DEP =  pCD_CON_DEP;
      if pCount = 0 then
            pNR_SEQ_SFP := 1;
      else
            select  max(NR_SEQ_SFP) + 1
            into    pNR_SEQ_SFP 
            from    tscsfp
            where
                  CD_CPFCGC = pCD_CPFCGC and
                  DT_NASC_FUND = pDT_NASC_FUND and
                  CD_CON_DEP =  pCD_CON_DEP;
      End if;
	insert into tscsfp	(
		 CD_CPFCGC ,
		 DT_NASC_FUND ,
		 CD_CON_DEP ,
		 NR_SEQ_SFP ,
		 CD_SFPGRUPO ,
		 CD_SFPSUBGRUPO ,
		 PC_LIMITE   ,
		 DS_BEN   ,
		 VL_BEN  ,
		 IN_ONUS  ,
		 VL_DEVEDOR   ,
		 DT_VENCIMENTO  ,
		 SG_ESTADO   ,
		 TM_STAMP  
	)values(
		 pCD_CPFCGC   ,
		 pDT_NASC_FUND ,
		 pCD_CON_DEP  ,
		 pNR_SEQ_SFP  ,
		 pCD_SFPGRUPO  ,
		 pCD_SFPSUBGRUPO ,
		 pPC_LIMITE  ,
		 pDS_BEN    ,
		 pVL_BEN   ,
		 pIN_ONUS  ,
		 pVL_DEVEDOR  ,
		 pDT_VENCIMENTO ,
		 pSG_ESTADO   ,
		 pTM_STAMP   
		);
End;
/




create or replace procedure prc_sel_endereco_cc(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
 OPEN Retorno FOR
     select 
		e.CD_CEP,
		e.CD_CEP_EXT,
		e.CD_DDD_CELULAR1,
		e.CD_DDD_CELULAR2,
		e.CD_DDD_FAX,
		e.CD_DDD_TEL,
		e.IN_TIPO_ENDE,
		e.NM_BAIRRO,
		e.NR_CELULAR1,
		e.NR_CELULAR2,
		e.NM_CIDADE,
		e.NM_COMP_ENDE,
		e.NR_FAX,
		e.NM_CONTATO1,
		e.NM_CONTATO2,
		e.NM_LOGRADOURO,
		e.NR_PREDIO,
		e.NR_RAMAL,
		e.NR_TELEFONE,
		e.SG_ESTADO,
		e.SG_PAIS 
	from
		tscende e
		inner join tscclicc a on (e.CD_CPFCGC = a.CD_CPFCGC and e.cd_con_dep = a.cd_con_dep and e.DT_NASC_FUND=a.DT_NASC_FUND)
	where
		e.CD_CPFCGC = pCD_CPFCGC
		and e.DT_NASC_FUND = pDT_NASC_FUND
		and e.CD_CON_DEP = pCD_CON_DEP;
End;
/
 



create or replace procedure prc_sel_endereco_custodia(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
 OPEN Retorno FOR
     select 
		e.CD_CEP,
		e.CD_CEP_EXT,
		e.CD_DDD_CELULAR1,
		e.CD_DDD_CELULAR2,
		e.CD_DDD_FAX,
		e.CD_DDD_TEL,
		e.IN_TIPO_ENDE,
		e.NM_BAIRRO,
		e.NR_CELULAR1,
		e.NR_CELULAR2,
		e.NM_CIDADE,
		e.NM_COMP_ENDE,
		e.NR_FAX,
		e.NM_CONTATO1,
		e.NM_CONTATO2,
		e.NM_LOGRADOURO,
		e.NR_PREDIO,
		e.NR_RAMAL,
		e.NR_TELEFONE,
		e.SG_ESTADO,
		e.SG_PAIS 
	from
		tscende e
		inner join tscclicus a on (e.CD_CPFCGC = a.CD_CPFCGC and e.cd_con_dep = a.cd_con_dep and e.DT_NASC_FUND=a.DT_NASC_FUND)
	where
		e.CD_CPFCGC = pCD_CPFCGC
		and e.DT_NASC_FUND = pDT_NASC_FUND
		and e.CD_CON_DEP = pCD_CON_DEP;
End;
/




create or replace procedure prc_sel_endereco_bol(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
 OPEN Retorno FOR
     select 
		e.CD_CEP,
		e.CD_CEP_EXT,
		e.CD_DDD_CELULAR1,
		e.CD_DDD_CELULAR2,
		e.CD_DDD_FAX,
		e.CD_DDD_TEL,
		e.IN_TIPO_ENDE,
		e.NM_BAIRRO,
		e.NR_CELULAR1,
		e.NR_CELULAR2,
		e.NM_CIDADE,
		e.NM_COMP_ENDE,
		e.NR_FAX,
		e.NM_CONTATO1,
		e.NM_CONTATO2,
		e.NM_LOGRADOURO,
		e.NR_PREDIO,
		e.NR_RAMAL,
		e.NR_TELEFONE,
		e.SG_ESTADO,
		e.SG_PAIS 
	from
		tscende e
		inner join tscclibol a on (e.CD_CPFCGC = a.CD_CPFCGC and e.cd_con_dep = a.cd_con_dep and e.DT_NASC_FUND=a.DT_NASC_FUND)
	where
		e.CD_CPFCGC = pCD_CPFCGC
		and e.DT_NASC_FUND = pDT_NASC_FUND
		and e.CD_CON_DEP = pCD_CON_DEP;
End;
/
  



create or replace procedure prc_sel_endereco_bmf(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
 OPEN Retorno FOR
     select 
		e.CD_CEP,
		e.CD_CEP_EXT,
		e.CD_DDD_CELULAR1,
		e.CD_DDD_CELULAR2,
		e.CD_DDD_FAX,
		e.CD_DDD_TEL,
		e.IN_TIPO_ENDE,
		e.NM_BAIRRO,
		e.NR_CELULAR1,
		e.NR_CELULAR2,
		e.NM_CIDADE,
		e.NM_COMP_ENDE,
		e.NR_FAX,
		e.NM_CONTATO1,
		e.NM_CONTATO2,
		e.NM_LOGRADOURO,
		e.NR_PREDIO,
		e.NR_RAMAL,
		e.NR_TELEFONE,
		e.SG_ESTADO,
		e.SG_PAIS 
	from
		tscende e
		inner join tscclibmf a on (e.CD_CPFCGC = a.CD_CPFCGC and e.cd_con_dep = a.cd_con_dep and e.DT_NASC_FUND=a.DT_NASC_FUND)
	where
		e.CD_CPFCGC = pCD_CPFCGC
		and e.DT_NASC_FUND = pDT_NASC_FUND
		and e.CD_CON_DEP = pCD_CON_DEP;
End;
/



 
create or replace procedure prc_existe_endereco1(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 pCOUNT out int)
As
Begin
	select 
		count (*)
	into
		 pCOUNT
	from 
		tscende 
	where nr_seq_ende =1 
		and	CD_CPFCGC = pCD_CPFCGC
		and DT_NASC_FUND = pDT_NASC_FUND
		and CD_CON_DEP = pCD_CON_DEP;
End;
/





create or replace procedure prc_exp_endereco_temp(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int)
As
Begin
	insert into tscende (
		CD_CPFCGC, 
		DT_NASC_FUND, 
		CD_CON_DEP, 
		NR_SEQ_ENDE, 
		NM_LOGRADOURO, 
		NR_PREDIO, 
		NM_BAIRRO, 
		NM_CIDADE, 
		SG_ESTADO, 
		SG_PAIS, 
		CD_CEP, 
		CD_CEP_EXT, 
		IN_ENDE_OFICIAL, 
		CD_USUARIO,
		TP_OCORRENCIA, 
		DT_ATUALIZ_CCLI
	) values ( 
		pCD_CPFCGC ,
		pDT_NASC_FUND, 
		pCD_CON_DEP,
		1, 
		'Endereco Temp. p/ atualizacao', 
		1, 
		'Bairro Temporario',
		'Cidade Temporaria', 
		'SP', 
		'BRA', 
		04543 , 
		000 , 
		'S', 
		151, 
		'U', 
		sysdate
	);
End;
/




create or replace procedure prc_move1_endAtiv_del_outros(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int)
As
Begin
	update 
		tscclicus 
	set 
		NR_SEQ_ENDE_CORRESP = 1, 
		CD_USUARIO =151 , 
		TP_OCORRENCIA='U' , 
		DT_ATUALIZ = sysdate 
	where 
		CD_CON_DEP = pCD_CON_DEP and 
		CD_CPFCGC = pCD_CPFCGC and  
		DT_NASC_FUND = pDT_NASC_FUND ;
      
	update 
		tscclicc 
	set 
		NR_SEQ_ENDE_CORRESP = 1, 
		CD_USUARIO =151 , 
		TP_OCORRENCIA='U' , 
		DT_ATUALIZ = sysdate 
	where 
		CD_CON_DEP = pCD_CON_DEP and 
		CD_CPFCGC = pCD_CPFCGC and  
		DT_NASC_FUND = pDT_NASC_FUND ;

	update 
		tscclibol 
	set 
		NR_SEQ_ENDE_CORRESP = 1, 
		CD_USUARIO =151 , 
		TP_OCORRENCIA='U' , 
		DT_ATUALIZ = sysdate 
	where 
		CD_CON_DEP = pCD_CON_DEP and 
		CD_CPFCGC = pCD_CPFCGC and  
		DT_NASC_FUND = pDT_NASC_FUND ; 

	update 
		tscclibmf 
	set 
		NR_SEQ_ENDE_CORRESP = 1, 
		CD_USUARIO =151 , 
		TP_OCORRENCIA='U' , 
		DT_ATUALIZ = sysdate 
	where 
		CD_CON_DEP = pCD_CON_DEP and 
		CD_CPFCGC = pCD_CPFCGC and  
		DT_NASC_FUND = pDT_NASC_FUND ; 

	delete 
		tscende 
	where 
		CD_CON_DEP = pCD_CON_DEP and 
		CD_CPFCGC = pCD_CPFCGC and  
		DT_NASC_FUND = pDT_NASC_FUND
        and NR_SEQ_ENDE > 1; 
           
End;
/




create or replace procedure prc_exp_endereco(
	pCD_CEP in number,
	pCD_CEP_EXT number,
	pCD_DDD_CELULAR1 in number default null,
	pCD_DDD_CELULAR2 in number default null,
	pCD_DDD_FAX in number default null,
	pCD_DDD_TEL in number default null,
	pIN_ENDE in varchar2,
	pNM_BAIRRO in varchar2,
	pNR_CELULAR1 in number default null,
	pNR_CELULAR2 in number default null,
	pNM_CIDADE in varchar2,
	pNM_COMP_ENDE in varchar2 default null,
	pNR_FAX in number default null,
	pNM_CONTATO1 in varchar2 default null,
	pNM_CONTATO2 in varchar2 default null,
	pNM_LOGRADOURO in varchar2 default null,
	pNR_PREDIO in varchar2,
	pNR_RAMAL in number default null,
	pNR_TELEFONE in number default null,
	pSG_ESTADO in varchar2,
	pSG_PAIS_ENDE1 in varchar2,
	pSEQENDERECO in number,
	pCD_CPFCGC in number,
	pCD_CON_DEP in number,
	pDT_NASC_FUND in date
	)
As
Begin
	INSERT INTO 
		TSCENDE ( 
			CD_CEP 
            ,CD_CEP_EXT 
            ,CD_DDD_CELULAR1 
            ,CD_DDD_CELULAR2 
            ,CD_DDD_FAX
            ,CD_DDD_TEL 
            ,IN_TIPO_ENDE 
            ,NM_BAIRRO 
            ,NR_CELULAR1 
            ,NR_CELULAR2 
            ,NM_CIDADE 
            ,NM_COMP_ENDE 
            ,NR_FAX 
            ,NM_CONTATO1 
            ,NM_CONTATO2 
            ,NM_LOGRADOURO 
            ,NR_PREDIO 
            ,NR_RAMAL 
            ,NR_TELEFONE 
            ,SG_ESTADO 
            ,SG_PAIS 
            ,CD_USUARIO 
            ,TP_OCORRENCIA 
            ,DT_ATUALIZ_CCLI 
            ,NR_SEQ_ENDE 
            ,CD_CPFCGC 
            ,CD_CON_DEP 
            ,DT_NASC_FUND  
            ,IN_ENDE_OFICIAL 
		) values ( 
			pCD_CEP,
			pCD_CEP_EXT,
			pCD_DDD_CELULAR1,
			pCD_DDD_CELULAR2,
			pCD_DDD_FAX,
			pCD_DDD_TEL,
			pIN_ENDE,
			pNM_BAIRRO,
			pNR_CELULAR1,
			pNR_CELULAR2,
			pNM_CIDADE,
			pNM_COMP_ENDE,
			pNR_FAX,
			pNM_CONTATO1,
			pNM_CONTATO2,
			pNM_LOGRADOURO,
			pNR_PREDIO,
			pNR_RAMAL,
			pNR_TELEFONE,
			pSG_ESTADO,
			pSG_PAIS_ENDE1,
			151,
			'U',
			sysdate,
			pseqendereco,
			pCD_CPFCGC,
			pCD_CON_DEP,
			pDT_NASC_FUND,
			'N'
         ) ;
End;
/




create or replace procedure prc_nr_endereco_atividades(
	pCD_CEP in number,
	pCD_CEP_EXT number,
	pNM_LOGRADOURO in varchar2 default null,
	pNR_PREDIO in varchar2,
	pCD_CPFCGC in number,
	pCD_CON_DEP in number,
	pDT_NASC_FUND in date,
	pNR_SEQ_ENDE out int
)
As
Begin
	select 
		max(NR_SEQ_ENDE) 
	into
		pNR_SEQ_ENDE
	from  
		TSCENDE
    WHERE 
		CD_CPFCGC = pCD_CPFCGC
        and CD_CON_DEP =pCD_CON_DEP
        and DT_NASC_FUND =pDT_NASC_FUND
        and NM_LOGRADOURO =pNM_LOGRADOURO
		and CD_CEP = pCD_CEP
		and CD_CEP_EXT = pCD_CEP_EXT
        and NR_PREDIO = pNR_PREDIO;
End;
/



create or replace procedure prc_relaciona_endereco_cus(
	pCD_CPFCGC in number,
	pCD_CON_DEP in number,
	pDT_NASC_FUND in date,
	pNR_SEQ_ENDE_CORRESP in int
)
As
Begin

	UPDATE 
		TSCCLICUS 
	SET 
		NR_SEQ_ENDE_CORRESP = pNR_SEQ_ENDE_CORRESP,
		CD_USUARIO = 151
    WHERE 
		CD_CPFCGC =pCD_CPFCGC
        and CD_CON_DEP =pCD_CON_DEP
        and DT_NASC_FUND = pDT_NASC_FUND;

End;
/



create or replace procedure prc_relaciona_endereco_cc(
	pCD_CPFCGC in number,
	pCD_CON_DEP in number,
	pDT_NASC_FUND in date,
	pNR_SEQ_ENDE_CORRESP in int
)
As
Begin

	UPDATE 
		TSCCLICC 
	SET 
		NR_SEQ_ENDE_CORRESP = pNR_SEQ_ENDE_CORRESP,
		CD_USUARIO = 151
    WHERE 
		CD_CPFCGC =pCD_CPFCGC
        and CD_CON_DEP =pCD_CON_DEP
        and DT_NASC_FUND = pDT_NASC_FUND;

End;
/




create or replace procedure prc_relaciona_endereco_bol(
	pCD_CPFCGC in number,
	pCD_CON_DEP in number,
	pDT_NASC_FUND in date,
	pNR_SEQ_ENDE_CORRESP in int
)
As
Begin

	UPDATE 
		TSCCLIBOL 
	SET 
		NR_SEQ_ENDE_CORRESP = pNR_SEQ_ENDE_CORRESP,
		CD_USUARIO = 151
    WHERE 
		CD_CPFCGC =pCD_CPFCGC
        and CD_CON_DEP =pCD_CON_DEP
        and DT_NASC_FUND = pDT_NASC_FUND;
End;
/ 



create or replace procedure prc_relaciona_endereco_bmf(
	pCD_CPFCGC in number,
	pCD_CON_DEP in number,
	pDT_NASC_FUND in date,
	pNR_SEQ_ENDE_CORRESP in int
)
As
Begin

	UPDATE 
		TSCCLIBMF 
	SET 
		NR_SEQ_ENDE_CORRESP = pNR_SEQ_ENDE_CORRESP,
		CD_USUARIO = 151
    WHERE 
		CD_CPFCGC =pCD_CPFCGC
        and CD_CON_DEP =pCD_CON_DEP
        and DT_NASC_FUND = pDT_NASC_FUND;

End;
/
 





grant execute  on prc_sel_endereco_cc to corrwin ;
grant execute  on prc_sel_endereco_custodia to corrwin ;
grant execute  on prc_sel_endereco_bol to corrwin ;
grant execute  on prc_sel_endereco_bmf to corrwin ;
grant execute  on prc_existe_endereco1 to corrwin ;
grant execute  on prc_exp_endereco_temp to corrwin ;
grant execute  on prc_move1_endAtiv_del_outros to corrwin ;
grant execute  on prc_exp_endereco to corrwin ;
grant execute  on prc_nr_endereco_atividades to corrwin ;
grant execute  on prc_relaciona_endereco_cus to corrwin ;
grant execute  on prc_relaciona_endereco_cc  to corrwin ;
grant execute  on prc_relaciona_endereco_bol  to corrwin ;
grant execute  on prc_relaciona_endereco_bmf to corrwin ;


create public synonym prc_sel_endereco_cc for cadastrosinacor.prc_sel_endereco_cc;
create public synonym prc_sel_endereco_custodia  for cadastrosinacor.prc_sel_endereco_custodia;
create public synonym prc_sel_endereco_bol  for cadastrosinacor.prc_sel_endereco_bol;
create public synonym prc_sel_endereco_bmf  for cadastrosinacor. prc_sel_endereco_bmf;
create public synonym prc_existe_endereco1  for cadastrosinacor. prc_existe_endereco1;
create public synonym prc_exp_endereco_temp  for cadastrosinacor. prc_exp_endereco_temp;
create public synonym prc_move1_endAtiv_del_outros  for cadastrosinacor. prc_move1_endAtiv_del_outros;
create public synonym prc_exp_endereco  for cadastrosinacor. prc_exp_endereco;
create public synonym prc_nr_endereco_atividades  for cadastrosinacor. prc_nr_endereco_atividades;
create public synonym prc_relaciona_endereco_cus  for cadastrosinacor. prc_relaciona_endereco_cus;
create public synonym prc_relaciona_endereco_cc   for cadastrosinacor. prc_relaciona_endereco_cc  ;
create public synonym prc_relaciona_endereco_bol   for cadastrosinacor. prc_relaciona_endereco_bol  ;
create public synonym prc_relaciona_endereco_bmf  for cadastrosinacor. prc_relaciona_endereco_bmf;



grant select on corrwin.TSCNACION to cadastrosinacor;
grant select on corrwin.tscestciv to cadastrosinacor;
grant select on corrwin.TSCTIPDOC to cadastrosinacor;
grant select on corrwin.TSCOREMI to cadastrosinacor;
grant select on corrwin.tscativ to cadastrosinacor;
grant select on corrwin.tscbanco to cadastrosinacor;
grant select on corrwin.TSCESTADO to cadastrosinacor;
grant select on corrwin.tscpais to cadastrosinacor;
grant select on corrwin.tscasses to cadastrosinacor;
grant select on corrwin.TSCTIPFIL to cadastrosinacor;
grant select on corrwin.TSCTIPCLI to cadastrosinacor;
grant select on corrwin.tsctpconta to cadastrosinacor;
grant select on corrwin.tscescolaridade to cadastrosinacor;
grant select on corrwin.tscclicomp to cadastrosinacor;
grant select on corrwin.tgeferia to cadastrosinacor;



grant select, insert, update, delete on corrwin.TSCDOCS to cadastrosinacor;
grant select, insert, update, delete on corrwin.tsccliger to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscclibmf to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscclibol to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscclicc to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscclicus to cadastrosinacor;
grant select, insert, update, delete on corrwin.TSCSFPCLA to cadastrosinacor;
grant select, insert, update, delete on corrwin.TSCSFPSUBGRU to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscsfp to cadastrosinacor;
grant select, insert, update, delete on corrwin.TSCCBASAG to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscclicta to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscclisis to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscende to cadastrosinacor;
grant select, insert, update, delete on corrwin.tscemitordem to cadastrosinacor;
grant select, insert, update, delete on corrwin.tsccvm220 to cadastrosinacor;









