using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class SinacorExportarDadosClienteInfo : ICodigoEntidade
    {

        private string _TP_Registro;//1
        public string TP_Registro
        {
            set { _TP_Registro = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _TP_Registro; }
        }

        public string CD_CPFCGC { set; get; }

        public string DT_CRIACAO { set; get; }

        public string DT_ATUALIZ { set; get; }

        public string DT_NASC_FUND { set; get; }

        public string CD_CON_DEP { set; get; }

        private string _TP_PESSOA;//1
        public string TP_PESSOA
        {
            set { _TP_PESSOA = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _TP_PESSOA; }
        }

        private string _NM_CLIENTE;//60
        public string NM_CLIENTE
        {
            set { _NM_CLIENTE = value.Substring(0, value.Length > 60 ? 60 : value.Length); }
            get { return _NM_CLIENTE; }
        }

        private string _CD_DOC_IDENT;//16
        public string CD_DOC_IDENT
        {
            set { _CD_DOC_IDENT = value.Substring(0, value.Length > 16 ? 16 : value.Length); }
            get { return _CD_DOC_IDENT; }
        }

        private string _CD_TIPO_DOC;//2
        public string CD_TIPO_DOC
        {
            set { _CD_TIPO_DOC = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _CD_TIPO_DOC; }
        }

        private string _CD_ORG_EMIT;//4
        public string CD_ORG_EMIT
        {
            set { _CD_ORG_EMIT = value.Substring(0, value.Length > 4 ? 4 : value.Length); }
            get { return _CD_ORG_EMIT; }
        }

        private string _ID_SEXO;//1
        public string ID_SEXO
        {
            set { _ID_SEXO = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _ID_SEXO; }
        }

        public string CD_EST_CIVIL { set; get; }

        private string _IN_PESS_VINC;//1
        public string IN_PESS_VINC
        {
            set { _IN_PESS_VINC = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_PESS_VINC; }
        }

        private string _NM_CONJUGE;//60
        public string NM_CONJUGE
        {
            set { _NM_CONJUGE = value.Substring(0, value.Length > 60 ? 60 : value.Length); }
            get { return _NM_CONJUGE; }
        }

        public string CD_CAPAC { set; get; }

        private string _NM_PAI;//60
        public string NM_PAI
        {
            set { _NM_PAI = value.Substring(0, value.Length > 60 ? 60 : value.Length); }
            get { return _NM_PAI; }
        }

        public string CD_CODQUA { set; get; }

        public string TP_CLIENTE { set; get; }

        public string CD_ATIV { set; get; }

        private string _IN_IRMARG;//1
        public string IN_IRMARG
        {
            set { _IN_IRMARG = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_IRMARG; }
        }

        private string _IN_IOFMAR;//1
        public string IN_IOFMAR
        {
            set { _IN_IOFMAR = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_IOFMAR; }
        }

        public string IN_IRSDIV { set; get; }

        public string CD_NACION { set; get; }

        private string _SG_PAIS;//3
        public string SG_PAIS
        {
            set { _SG_PAIS = value.Substring(0, value.Length > 3 ? 3 : value.Length); }
            get { return _SG_PAIS; }
        }

        private string _CD_CLIENTE;//7
        public string CD_CLIENTE
        {
            set { _CD_CLIENTE = value.Substring(0, value.Length > 7 ? 7 : value.Length); }
            get { return _CD_CLIENTE; }
        }

        private string _CD_ADMIN_CVM;//13
        public string CD_ADMIN_CVM
        {
            set { _CD_ADMIN_CVM = value.Substring(0, value.Length > 13 ? 13 : value.Length); }
            get { return _CD_ADMIN_CVM; }
        }

        private string _CD_BOLSA;//2
        public string CD_BOLSA
        {
            set { _CD_BOLSA = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _CD_BOLSA; }
        }

        private string _CD_CORR_OUTR_BOLSA;//5
        public string CD_CORR_OUTR_BOLSA
        {
            set { _CD_CORR_OUTR_BOLSA = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _CD_CORR_OUTR_BOLSA; }
        }

        private string _CD_CLIE_OUTR_BOLSA;//10
        public string CD_CLIE_OUTR_BOLSA
        {
            set { _CD_CLIE_OUTR_BOLSA = value.Substring(0, value.Length > 10 ? 10 : value.Length); }
            get { return _CD_CLIE_OUTR_BOLSA; }
        }

        public string CD_BANCO { set; get; }

        public string CD_AGENCIA { set; get; }

        private string _NM_CONTA;//13
        public string NM_CONTA
        {
            set { _NM_CONTA = value.Substring(0, value.Length > 13 ? 13 : value.Length); }
            get { return _NM_CONTA; }
        }

        public string DV_CLIENTE { set; get; }
        public string CD_ASSESSOR { set; get; }

        private string _DS_COMNOM;//24
        public string DS_COMNOM
        {
            set { _DS_COMNOM = value.Substring(0, value.Length > 24 ? 24 : value.Length); }
            get { return _DS_COMNOM; }
        }

        private string _IN_REC_DIVI;//1
        public string IN_REC_DIVI
        {
            set { _IN_REC_DIVI = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_REC_DIVI; }
        }

        private string _NM_LOGRADOURO;//30
        public string NM_LOGRADOURO
        {
            set { _NM_LOGRADOURO = value.Substring(0, value.Length > 30 ? 30 : value.Length); }
            get { return _NM_LOGRADOURO; }
        }

        private string _NR_PREDIO;//5
        public string NR_PREDIO
        {
            set { _NR_PREDIO = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _NR_PREDIO; }
        }

        private string _NM_COMP_ENDE;//10
        public string NM_COMP_ENDE
        {
            set { _NM_COMP_ENDE = value.Substring(0, value.Length > 10 ? 10 : value.Length); }
            get { return _NM_COMP_ENDE; }
        }

        private string _NM_BAIRRO;//18
        public string NM_BAIRRO
        {
            set { _NM_BAIRRO = value.Substring(0, value.Length > 18 ? 18 : value.Length); }
            get { return _NM_BAIRRO; }
        }

        private string _NM_CIDADE;//28
        public string NM_CIDADE
        {
            set { _NM_CIDADE = value.Substring(0, value.Length > 28 ? 28 : value.Length); }
            get { return _NM_CIDADE; }
        }

        private string _SG_ESTADO;//4
        public string SG_ESTADO
        {
            set { _SG_ESTADO = value.Substring(0, value.Length > 4 ? 4 : value.Length); }
            get { return _SG_ESTADO; }
        }

        public string CD_CEP { set; get; }

        public string CD_DDD_TEL { set; get; }

        public string NR_TELEFONE { set; get; }

        public string NR_RAMAL { set; get; }

        public string CD_DDD_FAX { set; get; }

        public string NR_FAX { set; get; }

        private string _PC_CORCOR_PRIN;//12
        public string PC_CORCOR_PRIN
        {
            set { _PC_CORCOR_PRIN = value.Substring(0, value.Length > 12 ? 12 : value.Length); }
            get { return _PC_CORCOR_PRIN; }
        }

        public string COD_AGCT { set; get; }
        public string COD_CLI_AGCT { set; get; }
        public string CD_AGENTE_COMP { set; get; }
        public string CD_CLIENTE_COMP { set; get; }



        private string _IN_CART_PROP;//1
        public string IN_CART_PROP
        {
            set { _IN_CART_PROP = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_CART_PROP; }
        }

        public string IN_EMITE_NOTA { set; get; }

        public string CD_USUA_INST { set; get; }

        public string DV_USUA_INST { set; get; }

        public string CD_CLIE_INST { set; get; }

        public string DV_CLIE_INST { set; get; }

        private string _DT_DATINA;//8
        public string DT_DATINA
        {
            set { _DT_DATINA = value.Substring(0, value.Length > 8 ? 8 : value.Length); }
            get { return _DT_DATINA; }
        }

        public string TP_CLIENTE_BOL { set; get; }

        private string _IN_SITUAC;//1
        public string IN_SITUAC
        {
            set { _IN_SITUAC = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_SITUAC; }
        }

        private string _NM_MAE;//60
        public string NM_MAE
        {
            set { _NM_MAE = value.Substring(0, value.Length > 60 ? 60 : value.Length); }
            get { return _NM_MAE; }
        }

        private string _NM_E_MAIL;//80
        public string NM_E_MAIL
        {
            set { _NM_E_MAIL = value.Substring(0, value.Length > 80 ? 80 : value.Length); }
            get { return _NM_E_MAIL; }
        }

        private string _NM_APELIDO;//18
        public string NM_APELIDO
        {
            set { _NM_APELIDO = value.Substring(0, value.Length > 18 ? 18 : value.Length); }
            get { return _NM_APELIDO; }
        }

        private string _IN_ENDE;//1
        public string IN_ENDE
        {
            set { _IN_ENDE = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_ENDE; }
        }

        public string NR_CELULAR1 { set; get; }

        public string NR_CELULAR2 { set; get; }

        public string CD_DDD_CELULAR1 { set; get; }

        public string CD_DDD_CELULAR2 { set; get; }

        private string _NR_CONTA_COMPL;//11
        public string NR_CONTA_COMPL
        {
            set { _NR_CONTA_COMPL = value.Substring(0, value.Length > 11 ? 11 : value.Length); }
            get { return _NR_CONTA_COMPL; }
        }

        private string _IN_CORRENTISTA;//1
        public string IN_CORRENTISTA
        {
            set { _IN_CORRENTISTA = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_CORRENTISTA; }
        }

        public string DT_DOC_IDENT { set; get; }

        private string _SG_ESTADO_EMIS;//4
        public string SG_ESTADO_EMIS
        {
            set { _SG_ESTADO_EMIS = value.Substring(0, value.Length > 4 ? 4 : value.Length); }
            get { return _SG_ESTADO_EMIS; }
        }

        private string _NM_LOC_NASC;//20
        public string NM_LOC_NASC
        {
            set { _NM_LOC_NASC = value.Substring(0, value.Length > 20 ? 20 : value.Length); }
            get { return _NM_LOC_NASC; }
        }

        private string _NM_EMPRESA;//60
        public string NM_EMPRESA
        {
            set { _NM_EMPRESA = value.Substring(0, value.Length > 60 ? 60 : value.Length); }
            get { return _NM_EMPRESA; }
        }

        private string _DS_CARGO;//40
        public string DS_CARGO
        {
            set { _DS_CARGO = value.Substring(0, value.Length > 40 ? 40 : value.Length); }
            get { return _DS_CARGO; }
        }

        public string CD_COSIF { set; get; }

        public string TP_REGCAS { set; get; }

        public string CD_ORIGEM { set; get; }

        public string CD_SITUAC { set; get; }

        private string _NM_LOGRADOURO2;//30
        public string NM_LOGRADOURO2
        {
            set { _NM_LOGRADOURO2 = value.Substring(0, value.Length > 30 ? 30 : value.Length); }
            get { return _NM_LOGRADOURO2; }
        }

        private string _NR_PREDIO2;//5
        public string NR_PREDIO2
        {
            set { _NR_PREDIO2 = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _NR_PREDIO2; }
        }

        private string _NM_COMP_ENDE2;//10
        public string NM_COMP_ENDE2
        {
            set { _NM_COMP_ENDE2 = value.Substring(0, value.Length > 10 ? 10 : value.Length); }
            get { return _NM_COMP_ENDE2; }
        }

        private string _NM_BAIRRO2;//18
        public string NM_BAIRRO2
        {
            set { _NM_BAIRRO2 = value.Substring(0, value.Length > 18 ? 18 : value.Length); }
            get { return _NM_BAIRRO2; }
        }

        private string _NM_CIDADE2;//28
        public string NM_CIDADE2
        {
            set { _NM_CIDADE2 = value.Substring(0, value.Length > 28 ? 28 : value.Length); }
            get { return _NM_CIDADE2; }
        }

        private string _SG_ESTADO2;//4
        public string SG_ESTADO2
        {
            set { _SG_ESTADO2 = value.Substring(0, value.Length > 4 ? 4 : value.Length); }
            get { return _SG_ESTADO2; }
        }

        public string CD_CEP2 { set; get; }

        public string CD_CEP_EXT2 { set; get; }//*não tem na tabela

        public string CD_DDD_TEL2 { set; get; }

        public string NR_TELEFONE2 { set; get; }

        public string NR_RAMAL2 { set; get; }

        public string CD_DDD_FAX2 { set; get; }

        public string NR_FAX2 { set; get; }

        private string _IN_TIPO_ENDE2;//1
        public string IN_TIPO_ENDE2
        {
            set { _IN_TIPO_ENDE2 = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_TIPO_ENDE2; }
        }

        public string CD_DDD_FAX22 { set; get; }
        public string NR_FAX22 { set; get; }

        private string _NM_CONTATO12;//50
        public string NM_CONTATO12
        {
            set { _NM_CONTATO12 = value.Substring(0, value.Length > 50 ? 50 : value.Length); }
            get { return _NM_CONTATO12; }
        }

        private string _NM_CONTATO22;//50
        public string NM_CONTATO22
        {
            set { _NM_CONTATO22 = value.Substring(0, value.Length > 50 ? 50 : value.Length); }
            get { return _NM_CONTATO22; }
        }

        public string NR_CELULAR12 { set; get; }
        public string NR_CELULAR22 { set; get; }
        public string CD_DDD_CELULAR12 { set; get; }
        public string CD_DDD_CELULAR22 { set; get; }

        private string _IN_SITUAC_QUALI;//1
        public string IN_SITUAC_QUALI
        {
            set { _IN_SITUAC_QUALI = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_SITUAC_QUALI; }
        }

        public string CD_CNPJ_CVM { set; get; }

        private string _CD_CVM;//19
        public string CD_CVM
        {
            set { _CD_CVM = value.Substring(0, value.Length > 19 ? 19 : value.Length); }
            get { return _CD_CVM; }
        }

        private string _SG_ESTADO_NASC;//4
        public string SG_ESTADO_NASC
        {
            set { _SG_ESTADO_NASC = value.Substring(0, value.Length > 4 ? 4 : value.Length); }
            get { return _SG_ESTADO_NASC; }
        }

        private string _TP_CONTA;//1
        public string TP_CONTA
        {
            set { _TP_CONTA = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _TP_CONTA; }
        }

        public string CD_VINCULO { set; get; }
        public string CD_CLIENTE_BMF { set; get; }

        private string _SOCEFE;//1
        public string SOCEFE
        {
            set { _SOCEFE = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _SOCEFE; }
        }

        private string _CLINST;//1
        public string CLINST
        {
            set { _CLINST = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _CLINST; }
        }

        public string PERTAX { set; get; }
        public string CORMAX { set; get; }

        private string _IN_NAO_RESIDE;//1
        public string IN_NAO_RESIDE
        {
            set { _IN_NAO_RESIDE = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_NAO_RESIDE; }
        }

        private string _IN_INTEGRA_CC;//1
        public string IN_INTEGRA_CC
        {
            set { _IN_INTEGRA_CC = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_INTEGRA_CC; }
        }

        private string _INDPLD;//1
        public string INDPLD
        {
            set { _INDPLD = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _INDPLD; }
        }

        private string _INDBRO;//1
        public string INDBRO
        {
            set { _INDBRO = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _INDBRO; }
        }

        private string _IN_INTEGRA_CORR;//1
        public string IN_INTEGRA_CORR
        {
            set { _IN_INTEGRA_CORR = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_INTEGRA_CORR; }
        }

        private string _IN_LIMINAR_IR_OPER;//1
        public string IN_LIMINAR_IR_OPER
        {
            set { _IN_LIMINAR_IR_OPER = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_LIMINAR_IR_OPER; }
        }

        private string _IN_TRIBUT_ESPECIAL;//1
        public string IN_TRIBUT_ESPECIAL
        {
            set { _IN_TRIBUT_ESPECIAL = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_TRIBUT_ESPECIAL; }
        }

        private string _IN_COBRA_MC;//1
        public string IN_COBRA_MC
        {
            set { _IN_COBRA_MC = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _IN_COBRA_MC; }
        }

        private string _INTLIQ;//1
        public string INTLIQ
        {
            set { _INTLIQ = value.Substring(0, value.Length > 1 ? 1 : value.Length); }
            get { return _INTLIQ; }
        }

        public string TP_CLIENTE_BMF { set; get; }

        private string _CD_OPERAC_CVM;//19
        public string CD_OPERAC_CVM
        {
            set { _CD_OPERAC_CVM = value.Substring(0, value.Length > 19 ? 19 : value.Length); }
            get { return _CD_OPERAC_CVM; }
        }

        public string CD_BANCO_CLICTA_PRINCIPAL { set; get; }

        private string _DV_AGENCIA_CLICTA_PRINCIPAL;
        public string DV_AGENCIA_CLICTA_PRINCIPAL
        {
            set { _DV_AGENCIA_CLICTA_PRINCIPAL = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _DV_AGENCIA_CLICTA_PRINCIPAL; }
        }

        private string _CD_AGENCIA_CLICTA_PRINCIPAL;//5
        public string CD_AGENCIA_CLICTA_PRINCIPAL
        {
            set { _CD_AGENCIA_CLICTA_PRINCIPAL = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _CD_AGENCIA_CLICTA_PRINCIPAL; }
        }

        private string _NR_CONTA_CLICTA_PRINCIPAL;//13
        public string NR_CONTA_CLICTA_PRINCIPAL
        {
            set { _NR_CONTA_CLICTA_PRINCIPAL = value.Substring(0, value.Length > 13 ? 13 : value.Length); }
            get { return _NR_CONTA_CLICTA_PRINCIPAL; }
        }

        private string _DV_CONTA_CLICTA_PRINCIPAL;//2
        public string DV_CONTA_CLICTA_PRINCIPAL
        {
            set { _DV_CONTA_CLICTA_PRINCIPAL = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _DV_CONTA_CLICTA_PRINCIPAL; }
        }

        public string CD_BANCO_CLICTA_A { set; get; }

        private string _CD_AGENCIA_CLICTA_A;//5
        public string CD_AGENCIA_CLICTA_A
        {
            set { _CD_AGENCIA_CLICTA_A = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _CD_AGENCIA_CLICTA_A; }
        }

        private string _DV_AGENCIA_CLICTA_A;//1
        public string DV_AGENCIA_CLICTA_A
        {
            set { _DV_AGENCIA_CLICTA_A = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _DV_AGENCIA_CLICTA_A; }
        }

        private string _NR_CONTA_CLICTA_A1;//13
        public string NR_CONTA_CLICTA_A1
        {
            set { _NR_CONTA_CLICTA_A1 = value.Substring(0, value.Length > 13 ? 13 : value.Length); }
            get { return _NR_CONTA_CLICTA_A1; }
        }

        private string _DV_CONTA_CLICTA_A1;//2
        public string DV_CONTA_CLICTA_A1
        {
            set { _DV_CONTA_CLICTA_A1 = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _DV_CONTA_CLICTA_A1; }
        }

        private string _NR_CONTA_CLICTA_A2;//13
        public string NR_CONTA_CLICTA_A2
        {
            set { _NR_CONTA_CLICTA_A2 = value.Substring(0, value.Length > 13 ? 13 : value.Length); }
            get { return _NR_CONTA_CLICTA_A2; }
        }

        private string _DV_CONTA_CLICTA_A2;//2
        public string DV_CONTA_CLICTA_A2
        {
            set { _DV_CONTA_CLICTA_A2 = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _DV_CONTA_CLICTA_A2; }
        }

        public string CD_BANCO_CLICTA_B { set; get; }

        private string _CD_AGENCIA_CLICTA_B;//5
        public string CD_AGENCIA_CLICTA_B
        {
            set { _CD_AGENCIA_CLICTA_B = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _CD_AGENCIA_CLICTA_B; }
        }

        private string _DV_AGENCIA_CLICTA_B;//5
        public string DV_AGENCIA_CLICTA_B
        {
            set { _DV_AGENCIA_CLICTA_B = value.Substring(0, value.Length > 5 ? 5 : value.Length); }
            get { return _DV_AGENCIA_CLICTA_B; }
        }

        private string _NR_CONTA_CLICTA_B1;//13
        public string NR_CONTA_CLICTA_B1
        {
            set { _NR_CONTA_CLICTA_B1 = value.Substring(0, value.Length > 13 ? 13 : value.Length); }
            get { return _NR_CONTA_CLICTA_B1; }
        }

        private string _DV_CONTA_CLICTA_B1;//2
        public string DV_CONTA_CLICTA_B1
        {
            set { _DV_CONTA_CLICTA_B1 = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _DV_CONTA_CLICTA_B1; }
        }

        private string _NR_CONTA_CLICTA_B2;//13
        public string NR_CONTA_CLICTA_B2
        {
            set { _NR_CONTA_CLICTA_B2 = value.Substring(0, value.Length > 13 ? 13 : value.Length); }
            get { return _NR_CONTA_CLICTA_B2; }
        }

        private string _DV_CONTA_CLICTA_B2;//2
        public string DV_CONTA_CLICTA_B2
        {
            set { _DV_CONTA_CLICTA_B2 = value.Substring(0, value.Length > 2 ? 2 : value.Length); }
            get { return _DV_CONTA_CLICTA_B2; }
        }

        public string CD_COSIF_CI { set; get; }
        public string CD_AGENTE { set; get; }


        private string _SG_PAIS_ENDE1;//3
        public string SG_PAIS_ENDE1
        {
            set { _SG_PAIS_ENDE1 = value.Substring(0, value.Length > 3 ? 3 : value.Length); }
            get { return _SG_PAIS_ENDE1; }
        }

        private string _SG_PAIS_ENDE2;//3
        public string SG_PAIS_ENDE2
        {
            set { _SG_PAIS_ENDE2 = value.Substring(0, value.Length > 3 ? 3 : value.Length); }
            get { return _SG_PAIS_ENDE2; }
        }

        private string _NM_E_MAIL2; //80
        public string NM_E_MAIL2
        {
            set { _NM_E_MAIL2 = value.Substring(0, value.Length > 80 ? 80 : value.Length); }
            get { return _NM_E_MAIL2; }
        }

        public string TP_INVESTIDOR { set; get; }


        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
