create or replace
procedure prc_ListaComboSinacor
( Informacao IN  INT
, Retorno    OUT GLOBALPKG.Retorno
)
/*
 Nacionalidade               = 1,
 EstadoCivil                 = 2,
 TipoDocumento               = 3,
 OrgaoEmissor                = 4,
 Profissao                   = 5,
 Banco                       = 6,
 Estado                      = 7,
 Pais                        = 8,
 Assessor                    = 9,
 SituacaoLegalRepresentante  =10,
 TipoCliente                 =11,
 AtividadePJ                 =12,
 EmailAssessor               =13,
 TipoConta                   =14,
 Escolaridade                =15,
 AtividadePF                 =16,
 AtividadePFePJ              =17,
 AssessorPadronizado         =18
*/
AS
BEGIN
  if Informacao = 1 then -- Nacionalidade = 1
    OPEN Retorno FOR
      select CD_NACION as id, DS_NACION as value from TSCNACION order by value asc;

  elsif  Informacao = 2 then -- EstadoCivil = 2
    OPEN Retorno FOR
      select CD_EST_CIVIL as id, DS_EST_CIVIL as value from tscestciv order by value asc;

  elsif  Informacao = 3 then -- TipoDocumento = 3
    OPEN Retorno FOR
      select CD_TIPO_DOC as id, DS_TIPO_DOC as value from TSCTIPDOC order by value asc;

  elsif  Informacao = 4 then -- OrgaoEmissor = 4
    OPEN Retorno FOR
      select CD_ORG_EMIT as id, DS_ORG_EMIT as value from TSCOREMI order by value asc;

  elsif  Informacao = 5 then -- Profissao = 5
    OPEN Retorno FOR
      select CD_ATIV as id, DS_ATIV as value from tscativ where CD_ATIV > 100 order by value asc;

  elsif  Informacao = 6 then -- Banco = 6
    OPEN Retorno FOR
      select CD_BANCO as id, NM_BANCO as value from tscbanco order by value asc;

  elsif  Informacao = 7 then -- Estado = 7
    OPEN Retorno FOR
      select SG_ESTADO as id, NM_ESTADO as value from TSCESTADO order by value asc;

  elsif  Informacao = 8 then -- Pais = 8
    OPEN Retorno FOR
      select SG_PAIS as id, NM_PAIS as value from tscpais order by value asc;

  elsif  Informacao = 9 then -- Assessor = 9
    OPEN Retorno FOR
      select cd_assessor as id , cd_assessor || ' - ' || nm_assessor as value from tscasses order by id;

  elsif  Informacao = 10 then -- SituacaoLegalRepresentante = 10
    OPEN Retorno FOR
      select CD_TIPO_FILI as id, DS_TIPO_FILI as value from TSCTIPFIL order by value asc;

  elsif  Informacao = 11 then -- TipoCliente = 11
    OPEN Retorno FOR
      select TP_CLIENTE as id,  TP_CLIENTE||' - '|| DS_TIPO_CLIENTE as value from TSCTIPCLI order by id;

  elsif  Informacao = 12 then -- AtividadePJ = 12
    OPEN Retorno FOR
      select CD_ATIV as id, DS_ATIV as value from tscativ where CD_ATIV<=100 order by value asc;

  elsif  Informacao = 13 then -- EmailAssessor = 13
    OPEN Retorno FOR
      select CD_ASSESSOR as id, NM_E_MAIL as value from tscasses order by id asc;

  elsif  Informacao = 14 then -- TipoConta = 14
    OPEN Retorno FOR
     select TP_CONTA as id, DS_TP_CONTA as value from tsctpconta order by value asc;

  elsif  Informacao = 15 then -- Escolaridade = 15
    OPEN Retorno FOR
      select CD_ESCOLARIDADE as id, DS_ESCOLARIDADE as value from tscescolaridade order by value asc;

  elsif  Informacao = 16 then -- AtividadePF = 16
    OPEN Retorno FOR
      select CD_ATIV as id, DS_ATIV as value from tscativ where CD_ATIV >= 100 order by value asc;

  elsif  Informacao = 17 then -- AtividadePFePJ = 17
    OPEN Retorno FOR
      select CD_ATIV as id, DS_ATIV as value from tscativ order by value asc;
  
  elsif  Informacao = 18 then -- AssessorPadronizado = 18
    OPEN Retorno FOR
     select cd_assessor as id , nm_assessor as value from tscasses order by id;
 end if;
End;
