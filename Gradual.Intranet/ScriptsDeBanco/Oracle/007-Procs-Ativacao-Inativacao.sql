create or replace procedure prc_AtivarInativar_CliGer_upd(
 pCD_CPFCGC in int,
 pDT_NASC_FUND in date,
 pCD_CON_DEP in int,
 pIN_SITUAC in varchar2
) 
AS
Begin
	update 
		tsccliger 
	set 
		DT_ATUALIZ = sysdate,
		CD_USUARIO=151,
		IN_SITUAC = pIN_SITUAC
    where 
		CD_CON_DEP = pCD_CON_DEP 
		and CD_CPFCGC = pCD_CPFCGC
		and  DT_NASC_FUND =pDT_NASC_FUND;
End;
/

 

create or replace procedure prc_AtivarInativar_Ativ_upd(
 pCD_CLIENTE in int,
 pATividade in varchar2,
 pIN_SITUAC in varchar2
)
AS
Begin
	if (pATividade = 'BMF') then
		update 
				tscclibmf 
			set
				status=CAST(pIN_SITUAC as char(1)),
				CD_USUARIO=151, 
				DT_ATUALIZ = sysdate
			where 
				CODCLI = pCD_CLIENTE;
	elsif (pATividade = 'BOVESPA') then
		update 
			tscclibol 
		set
			IN_SITUAC=CAST(pIN_SITUAC as char(1)),
			CD_USUARIO=151, 
			DT_ATUALIZ = sysdate
		where 
			CD_CLIENTE = pCD_CLIENTE;
	elsif (pATividade = 'CC') then
		update 
			tscclicc 
		set
			IN_SITUAC=CAST(pIN_SITUAC as char(1)),
			CD_USUARIO=151, 
			DT_ATUALIZ = sysdate
		where 
			CD_CLIENTE = pCD_CLIENTE;
	elsif (pATividade = 'CUSTODIA') then
		update 
			tscclicus 
		set
			IN_SITUAC=CAST(pIN_SITUAC as char(1)),
			CD_USUARIO=151, 
			DT_ATUALIZ = sysdate
		where 
			CD_CLIENTE = pCD_CLIENTE;
	end if;
End;
/

 
