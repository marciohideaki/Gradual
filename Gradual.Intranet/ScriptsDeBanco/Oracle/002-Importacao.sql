create or replace procedure prc_sel_todos(
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select distinct
			ger.CD_CPFCGC ,      
			ger.DT_NASC_FUND ,   
			ger.CD_CON_DEP 
		from 
			tsccliger ger
			--inner join tscclibol bol on (ger.CD_CPFCGC = bol.CD_CPFCGC and ger.cd_con_dep = bol.cd_con_dep and ger.DT_NASC_FUND=bol.DT_NASC_FUND)
		where 
			((ger.CD_CPFCGC ,ger.DT_NASC_FUND)  in (select distinct CD_CPFCGC ,DT_NASC_FUND from tscclibol where cd_con_dep = 1 )  
			or
			(ger.CD_CPFCGC ,ger.DT_NASC_FUND)  in (select distinct CD_CPFCGC ,DT_NASC_FUND from tscclibmf where cd_con_dep = 1 ))
			--ger.IN_SITUAC='A'
            --and  bol.IN_SITUAC='A' and  
			--bol.IN_CONTA_INV='N'
			and ger.CD_CON_DEP = 1
			and ger.TP_CLIENTE in (1, 2, 3, 4, 6, 8, 9, 11, 13, 15, 17, 18, 20, 21, 23, 25, 26, 29, 99 );
            --and  ger.TP_CLIENTE  in (1, 2, 3,  9, 13, 15, 26, 27, 28, 29, 30, 99)		
			--and  ger.TP_CLIENTE = 1	
			--and  ger.TP_CLIENTE in (8,18)	
			--and ger.CD_CPFCGC = 4480226800   ;--and ROWNUM < 50; 
End;
/
 



create or replace procedure prc_sel_tsccliger(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
 OPEN Retorno FOR
     select distinct
		ger.TP_PESSOA,
		ger.TP_CLIENTE,
		ger.IN_PESS_VINC,
		ger.IN_POLITICO_EXP,
		ger.DT_CRIACAO,
		ger.NM_CLIENTE,
		ger.IN_SITUAC
	from
		tsccliger ger
		-- agora aceita cliente somente com bmf 
		--inner join tscclibol bol on (ger.CD_CPFCGC = bol.CD_CPFCGC and ger.cd_con_dep = bol.cd_con_dep and ger.DT_NASC_FUND=bol.DT_NASC_FUND)
	where
		ger.CD_CPFCGC = pCD_CPFCGC
		and ger.DT_NASC_FUND = pDT_NASC_FUND
		and ger.CD_CON_DEP = pCD_CON_DEP;
		--and ger.IN_SITUAC='A'
        --and  bol.IN_SITUAC='A'
        --and  bol.IN_CONTA_INV='N'
End;
/



create or replace procedure prc_sel_codigo_principal(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	pCD_CLIENTE OUT int
)
AS
	countBov int;
Begin
	select count(*) into countBov from tscclibol where cd_cpfcgc = pCD_CPFCGC ;
	if countBov>0 then
		select bol.cd_cliente  into pCD_CLIENTE
		from tscclibol bol 
		where --bol.in_situac='A' and 
		bol.in_CONTA_INV='N' 
		and bol.cd_cpfcgc = pCD_CPFCGC
		and bol.cd_con_dep = pCD_CON_DEP
		and bol.dt_nasc_fund = pDT_NASC_FUND
		and bol.dt_ult_oper = 
			(select max(bol.dt_ult_oper) 
			from tscclibol bol 
			where  --bol.in_situac='A' and 
			bol.in_CONTA_INV='N' 
			and bol.cd_cpfcgc = pCD_CPFCGC
			and bol.cd_con_dep = pCD_CON_DEP
			and bol.dt_nasc_fund = pDT_NASC_FUND  )
		and ROWNUM < 2 ; --Existiu um caso de um cliente que operou ao mesmo tempo com duas contas...
	else
		select bmf.codcli  into pCD_CLIENTE
		from tscclibmf bmf 
		where  
		bmf.in_CONTA_INV='N' 
		and bmf.cd_cpfcgc = pCD_CPFCGC
		and bmf.cd_con_dep = pCD_CON_DEP
		and bmf.dt_nasc_fund = pDT_NASC_FUND
		and bmf.DTULTOP = 
			(select max(bmf2.DTULTOP) 
			from tscclibmf bmf2 
			where  --bol.in_situac='A' and 
			bmf2.in_CONTA_INV='N' 
			and bmf2.cd_cpfcgc = pCD_CPFCGC
			and bmf2.cd_con_dep = pCD_CON_DEP
			and bmf2.dt_nasc_fund = pDT_NASC_FUND  )
		and ROWNUM < 2 ;
	end if;
End;
/




create or replace procedure prc_sel_tscclicomp(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			 NM_CONJUGE ,
			NM_EMPRESA ,
			CD_EST_CIVIL,
			SG_ESTADO_EMIS ,
			CD_NACION ,
			NM_LOC_NASC,
			NM_MAE ,
			NM_PAI,
			SG_PAIS ,
			ID_SEXO,
			SG_ESTADO_NASC,
			CD_ATIV,
			CD_ESCOLARIDADE,
			CD_NIRE,
			DS_CARGO,
			CD_DOC_IDENT ,
			CD_TIPO_DOC ,
			CD_ORG_EMIT ,  
			SG_ESTADO_EMIS,
			DT_DOC_IDENT ,
			NR_RG ,         
			SG_ESTADO_EMISS_RG ,
			DT_EMISS_RG ,  
			CD_ORG_EMIT_RG 
		from 
			tscclicomp
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP ;
End;
/

create or replace procedure prc_sel_tscemitordem(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			NM_EMIT_ORDEM,
			CD_DOC_IDENT_EMIT,
			CD_CPFCGC_EMIT,
			IN_PRINCIPAL,
			CD_SISTEMA,
			NM_E_MAIL ,
			TM_STAMP
		from 
			tscemitordem
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP ;
End;
/

create or replace procedure prc_sel_tsccvm220(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			NM_DIRETOR_1,
			CD_DOC_IDENT_DIR1,
			CD_CPFCGC_DIR1,
			NM_DIRETOR_2,
			CD_DOC_IDENT_DIR2,
			CD_CPFCGC_DIR2,
			NM_DIRETOR_3,
			CD_DOC_IDENT_DIR3,
			CD_CPFCGC_DIR3,
			DS_FORMACAO,
			NR_INSCRICAO
		from 
			tsccvm220
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP ;
End;
/

create or replace procedure prc_sel_tscclicc(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			IN_CARPRO,
			CD_ASSESSOR
		from 
			tscclicc
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP 
			and IN_SITUAC='A'
			and IN_CONTA_INV='N';
End;
/


create or replace procedure prc_sel_tscende(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			NM_BAIRRO,
			CD_CEP,
			CD_CEP_EXT ,
			NM_CIDADE ,
			NM_COMP_ENDE ,
			IN_ENDE_CORR ,
			NM_LOGRADOURO ,
			NR_PREDIO,
			SG_PAIS ,
			IN_TIPO_ENDE ,
			SG_ESTADO,
			CD_DDD_TEL ,
			IN_ENDE_OFICIAL,
			NR_RAMAL ,
			NR_TELEFONE,
			IN_TIPO_ENDE ,
			CD_DDD_CELULAR1 ,
			NR_CELULAR1 ,
			CD_DDD_CELULAR2 ,
			NR_CELULAR2,
			CD_DDD_FAX ,   
			NR_FAX  ,
			CD_DDD_FAX2 ,   
			NR_FAX2        
		from 
			tscende
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP ;
End;
/





create or replace procedure prc_sel_tscclicta(
	pCD_CLIENTE in int,      
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			CD_AGENCIA,
			CD_BANCO,
			NR_CONTA,
			DV_CONTA,
			IN_PRINCIPAL,
			TP_CONTA
		from 
			tscclicta
		where 
			CD_CLIENTE = pCD_CLIENTE ;
End;
/



create or replace procedure prc_sel_tscsfp(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			CD_SFPGRUPO,
			CD_SFPSUBGRUPO,
			PC_LIMITE,
			DS_BEN,
			VL_BEN,
			IN_ONUS,
			VL_DEVEDOR,
			DT_VENCIMENTO,
			SG_ESTADO
		from 
			tscsfp
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP ;
End;
/



create or replace procedure prc_sel_tscclibol(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			CD_ASSESSOR,
			IN_SITUAC,
			IN_CONTA_INV,
			CD_CLIENTE
		from 
			tscclibol
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP; 
End;
/

create or replace procedure prc_sel_ativ_tscclicus(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			CD_ASSESSOR,
			IN_SITUAC,
			IN_CONTA_INV,
			CD_CLIENTE
		from 
			tscclicus
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP; 
End;
/

create or replace procedure prc_sel_ativ_tscclicc(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			CD_ASSESSOR,
			IN_SITUAC,
			IN_CONTA_INV,
			CD_CLIENTE
		from 
			tscclicc
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP; 
End;
/

create or replace procedure prc_sel_tscclibmf(
	pCD_CPFCGC in int,      
	pDT_NASC_FUND in date,   
	pCD_CON_DEP in int,     
	Retorno OUT GLOBALPKG.Retorno
)
AS
Begin
	OPEN Retorno FOR 
	    select 
			CODASS,
			STATUS,
			IN_CONTA_INV,
			CODCLI
		from 
			tscclibmf
		where 
			CD_CPFCGC = pCD_CPFCGC
			and DT_NASC_FUND = pDT_NASC_FUND
			and CD_CON_DEP = pCD_CON_DEP ;
End;
/

