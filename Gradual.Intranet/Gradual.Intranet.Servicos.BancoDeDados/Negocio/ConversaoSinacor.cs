using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Servicos.BancoDeDados.Negocios
{
    public static class ConversaoSinacor
    {
        public static ClienteInfo ConverterCliente(SinacorChaveClienteInfo pClienteChave, SinacorClienteGeralInfo pClienteGeral, SinacorClienteComplementoInfo pClienteComplemento, SinacorCcInfo pCc, SinacorDiretorInfo pDiretor)
        {
            ClienteInfo lRetorno = new ClienteInfo();

            DateTime pHoje = DateTime.Now;

            lRetorno.IdCliente = null;
            lRetorno.DsNome = pClienteGeral.NM_CLIENTE;
            lRetorno.IdLogin = null;
            lRetorno.DtUltimaAtualizacao = pHoje;
            lRetorno.DtPasso1 = pClienteGeral.DT_CRIACAO.Value;
            lRetorno.DtPasso2 = pClienteGeral.DT_CRIACAO.Value;
            lRetorno.DtPasso3 = pClienteGeral.DT_CRIACAO.Value;
            lRetorno.DtPrimeiraExportacao = pHoje;
            lRetorno.DtUltimaExportacao = pHoje;
            lRetorno.DsOrigemCadastro = "Importação do Sinacor";
            lRetorno.TpPessoa = pClienteGeral.TP_PESSOA.Value;
            lRetorno.TpCliente = pClienteGeral.TP_CLIENTE.Value;
            lRetorno.StPasso = 4;
            lRetorno.CdSexo = pClienteComplemento.ID_SEXO;
            lRetorno.CdNacionalidade = pClienteComplemento.CD_NACION;
            lRetorno.CdPaisNascimento = pClienteComplemento.SG_PAIS;
            lRetorno.CdUfNascimento = pClienteComplemento.SG_ESTADO_NASC;
            lRetorno.DsUfnascimentoEstrangeiro = "";
            lRetorno.CdEstadoCivil = pClienteComplemento.CD_EST_CIVIL;
            lRetorno.DsConjugue = pClienteComplemento.NM_CONJUGE;
            lRetorno.DtNascimentoFundacao = pClienteChave.DT_NASC_FUND;
            lRetorno.CdProfissaoAtividade = pClienteComplemento.CD_ATIV;
            lRetorno.DsCargo = pClienteComplemento.DS_CARGO;
            lRetorno.DsEmpresa = pClienteComplemento.NM_EMPRESA;
            lRetorno.StPPE = ToBool(pClienteGeral.IN_POLITICO_EXP);
            lRetorno.DsNomeMae = pClienteComplemento.NM_MAE;
            lRetorno.DsNomePai = pClienteComplemento.NM_PAI;
            lRetorno.DsCargo = pClienteComplemento.DS_CARGO;
            lRetorno.DsNaturalidade = pClienteComplemento.NM_LOC_NASC;
            lRetorno.StPessoaVinculada = pClienteGeral.IN_PESS_VINC == 'S' ? 1 : 0;

            if (lRetorno.TpPessoa == 'J')
            {
                lRetorno.StCarteiraPropria = true;
                lRetorno.IdAssessorInicial = null;
                lRetorno.DsCpfCnpj = pClienteChave.CD_CPFCGC.ToString().PadLeft(14, '0');
            }
            else
            {
                lRetorno.StCarteiraPropria = pClienteGeral.OPERA_CONTA_PROPRIA;// ToBool(pCc.IN_CARPRO);
                lRetorno.IdAssessorInicial = pCc.CD_ASSESSOR;
                lRetorno.DsCpfCnpj = pClienteChave.CD_CPFCGC.ToString().PadLeft(11, '0');
            }

            //Documentos pegar sempre de outros
            lRetorno.TpDocumento = pClienteComplemento.CD_TIPO_DOC;
            lRetorno.DtEmissaoDocumento = pClienteComplemento.DT_DOC_IDENT;
            lRetorno.CdOrgaoEmissorDocumento = pClienteComplemento.CD_ORG_EMIT;
            lRetorno.CdUfEmissaoDocumento = pClienteComplemento.SG_ESTADO_EMIS;
            lRetorno.DsNumeroDocumento = pClienteComplemento.CD_DOC_IDENT;

            //if (null == lRetorno.DtEmissaoDocumento)
            //{ 

            //}
            //System.Text.StringBuilder l = new System.Text.StringBuilder();
            //l.Append("CPF: " + lRetorno.DsCpfCnpj + Environment.NewLine);
            //l.Append("Tipo de Documento: " + lRetorno.TpDocumento + Environment.NewLine);
            //l.Append("Emissão do Documento: " + lRetorno.DtEmissaoDocumento.ToString() + Environment.NewLine);
            //l.Append("Órgão Emissor do Documento: " + lRetorno.CdOrgaoEmissorDocumento + Environment.NewLine);
            //l.Append("UF do Documento: " + lRetorno.CdUfEmissaoDocumento + Environment.NewLine);
            //l.Append("Número do Documento: " + lRetorno.DsNumeroDocumento + Environment.NewLine);

            //string z = l.ToString();


            //       //* Outros
            //public string CD_DOC_IDENT { get; set; }// - Número - VARCHAR2(16)   
            //public string CD_TIPO_DOC { get; set; }// - Tipo - VARCHAR2(2)   
            //public string CD_ORG_EMIT { get; set; }// - Órgão - VARCHAR2(4)    
            //public string SG_ESTADO_EMIS { get; set; }// - UF - VARCHAR2(4)
            //public Nullable<DateTime> DT_DOC_IDENT { get; set; }// - Data - DATE

            ////* RG 
            //public string NR_RG { get; set; }// - Número - VARCHAR2(16)                 
            //public string SG_ESTADO_EMISS_RG { get; set; }// - UF - VARCHAR2(4)
            //public Nullable<DateTime> DT_EMISS_RG { get; set; }// - Data - DATE            
            //public string CD_ORG_EMIT_RG { get; set; }// - Órgão - VARCHAR2(4)  

            lRetorno.DsAutorizadoOperar = "";
            lRetorno.StCVM387 = true;
            lRetorno.StEmancipado = null;

            lRetorno.CdEscolaridade = pClienteComplemento.CD_ESCOLARIDADE;
            lRetorno.StCadastroPortal = false;


            lRetorno.DsNomeFantasia = pClienteGeral.NM_CLIENTE;
            lRetorno.CdNire = pClienteComplemento.CD_NIRE; ;
            lRetorno.DsFormaConstituicao = null;
            lRetorno.NrInscricaoEstadual = null;
            lRetorno.StInterdito = null;
            lRetorno.StSituacaoLegalOutros = null;
            

            //CVM220
            if (null != pDiretor.NR_INSCRICAO && pDiretor.NR_INSCRICAO.Trim().Length > 0)
                lRetorno.NrInscricaoEstadual = pDiretor.NR_INSCRICAO;

            if (null != pDiretor.DS_FORMACAO && pDiretor.DS_FORMACAO.Trim().Length > 0)
                lRetorno.DsFormaConstituicao = pDiretor.DS_FORMACAO;

            lRetorno.StAtivo = true;
            lRetorno.DsEmail = "a@a.a";

            lRetorno.StAtivo = true;

            return lRetorno;
        }

        private static Boolean ToBool(char? pValue)
        {
            if (null == pValue)
            {
                return false;
            }
            else
            {
                if (pValue == 'S')
                    return true;
                else
                    return false;
            }
        }

        public static List<ClienteBancoInfo> ConverterContaBancaria(List<SinacorContaBancariaInfo> pContas)
        {
            List<ClienteBancoInfo> lRetorno = new List<ClienteBancoInfo>();
            ClienteBancoInfo lBanco;

            foreach (SinacorContaBancariaInfo item in pContas)
            {
                lBanco = new ClienteBancoInfo();
                lBanco.CdBanco = item.CD_BANCO;
                lBanco.DsAgencia = item.CD_AGENCIA.DBToString();
                lBanco.DsAgenciaDigito = item.DV_AGENCIA.DBToString();
                lBanco.DsConta = item.NR_CONTA;
                lBanco.DsContaDigito = item.DV_CONTA;
                lBanco.IdCliente = 0;
                lBanco.StPrincipal = ToBool(item.IN_PRINCIPAL);
                lBanco.TpConta = item.TP_CONTA;
                lRetorno.Add(lBanco);
            }
            return lRetorno;
        }
        private static Int64 GetTipoEndereco(char pTipoEndereco)
        {
            if (pTipoEndereco == 'C')
                return 1;
            else
                return 2;
        }
        private static Int64 GetTipoTelefoneEndereco(char pTipoTelefoneEndereco)
        {
            if (pTipoTelefoneEndereco == 'C')
                return 2;
            else
                return 1;
        }

        public static List<ClienteTelefoneInfo> ConverterTelefone(List<SinacorTelefoneInfo> pTelefones)
        {
            List<ClienteTelefoneInfo> lRetorno = new List<ClienteTelefoneInfo>();
            ClienteTelefoneInfo lTelefone;

            foreach (SinacorTelefoneInfo item in pTelefones)
            {
                if (item.NR_TELEFONE.HasValue && null != item.NR_TELEFONE && item.NR_TELEFONE != 0)
                {
                    lTelefone                = new ClienteTelefoneInfo();
                    lTelefone.DsDdd          = item.CD_DDD_TEL.Value.ToString();
                    lTelefone.IdCliente      = 0;
                    lTelefone.StPrincipal    = ToBool(item.IN_ENDE_OFICIAL);
                    lTelefone.DsRamal        = item.NR_RAMAL.ToString() == "0" ? null : item.NR_RAMAL.ToString();
                    lTelefone.DsNumero       = item.NR_TELEFONE.Value.ToString();
                    lTelefone.IdTipoTelefone = int.Parse(GetTipoTelefoneEndereco(item.IN_TIPO_ENDE.Value).ToString());
                    
                    ClienteTelefoneInfo lTelefoneExiste = lRetorno.Find(tel => tel.DsNumero == item.NR_TELEFONE.Value.ToString());

                    if (lTelefoneExiste == null)
                    {
                        lRetorno.Add(lTelefone);
                    }
                }

                if (item.NR_CELULAR1.HasValue && null != item.NR_CELULAR1 && item.NR_CELULAR1 != 0)
                {
                    lTelefone                = new ClienteTelefoneInfo();
                    lTelefone.DsDdd          = item.CD_DDD_CELULAR1.Value.ToString();
                    lTelefone.IdCliente      = 0;
                    lTelefone.StPrincipal    = ToBool(item.IN_ENDE_OFICIAL);
                    lTelefone.DsNumero       = item.NR_CELULAR1.Value.ToString();
                    lTelefone.DsRamal        = null;
                    lTelefone.IdTipoTelefone = 3; //Celular

                    ClienteTelefoneInfo lTelefoneExiste = lRetorno.Find(tel => tel.DsNumero == item.NR_CELULAR1.Value.ToString());

                    if (lTelefoneExiste == null)
                    {
                        lRetorno.Add(lTelefone);
                    }
                    
                }

                if (item.NR_CELULAR2.HasValue && null != item.NR_CELULAR2 && item.NR_CELULAR2 != 0)
                {
                    lTelefone                = new ClienteTelefoneInfo();
                    lTelefone.DsDdd          = item.CD_DDD_CELULAR2.Value.ToString();
                    lTelefone.IdCliente      = 0;
                    lTelefone.StPrincipal    = false;
                    lTelefone.DsNumero       = item.NR_CELULAR2.Value.ToString();
                    lTelefone.DsRamal        = null;
                    lTelefone.IdTipoTelefone = 3; //Celular
                    ClienteTelefoneInfo lTelefoneExiste = lRetorno.Find(tel => tel.DsNumero == item.NR_FAX.Value.ToString());
                    
                    if (lTelefoneExiste == null)
                    {
                        lRetorno.Add(lTelefone);
                    }
                    
                }
                if (item.NR_FAX.HasValue && null != item.NR_FAX && item.NR_FAX != 0)
                {
                    lTelefone                = new ClienteTelefoneInfo();
                    lTelefone.DsDdd          = item.CD_DDD_FAX.Value.ToString();
                    lTelefone.IdCliente      = 0;
                    lTelefone.StPrincipal    = false;
                    lTelefone.DsNumero       = item.NR_FAX.Value.ToString();
                    lTelefone.DsRamal        = null;
                    lTelefone.IdTipoTelefone = 4; //Fax
                    ClienteTelefoneInfo lTelefoneExiste = lRetorno.Find(tel => tel.DsNumero == item.NR_FAX.Value.ToString());

                    if (lTelefoneExiste == null)
                    {
                        lRetorno.Add(lTelefone);
                    }

                    
                    
                }
                if (item.NR_FAX2.HasValue && null != item.NR_FAX2 && item.NR_FAX2 != 0)
                {
                    lTelefone                = new ClienteTelefoneInfo();
                    lTelefone.DsDdd          = item.CD_DDD_FAX2.Value.ToString();
                    lTelefone.IdCliente      = 0;
                    lTelefone.StPrincipal    = false;
                    lTelefone.DsNumero       = item.NR_FAX2.Value.ToString();
                    lTelefone.DsRamal        = null;
                    lTelefone.IdTipoTelefone = 4; //Fax
                    ClienteTelefoneInfo lTelefoneExiste = lRetorno.Find(tel => tel.DsNumero == item.NR_FAX2.Value.ToString());

                    if (lTelefoneExiste == null)
                    {
                        lRetorno.Add(lTelefone);
                    }
                    
                }
            }
            return lRetorno;
        }

        public static ClienteSituacaoFinanceiraPatrimonialInfo ConverterSFP(List<SinacorSituacaoFinanceiraPatrimonialInfo> pSfp)
        {
            ClienteSituacaoFinanceiraPatrimonialInfo lRetorno = new ClienteSituacaoFinanceiraPatrimonialInfo();

            lRetorno.DtAtualizacao = DateTime.Now;

            lRetorno.DtCapitalSocial = null;
            lRetorno.DtPatrimonioLiquido = null;
            lRetorno.IdCliente = 0;
            lRetorno.IdSituacaoFinanceiraPatrimonial = null;
            lRetorno.VTotalCapitalSocial = 0;

            lRetorno.VlTotalPatrimonioLiquido = 0;
            lRetorno.VlTotalAplicacaoFinanceira = 0;
            lRetorno.VlTotalBensImoveis = 0;
            lRetorno.VlTotalBensMoveis = 0;
            lRetorno.VlTotalOutrosRendimentos = 0;
            lRetorno.VlTotalSalarioProLabore = 0;
            lRetorno.DsOutrosRendimentos = "";
            foreach (SinacorSituacaoFinanceiraPatrimonialInfo item in pSfp)
            {
                /*  
                 select * from TSCSFPGRUPO;
                  
                 CD_GRUPO DS_GRUPO        T
                --------- --------------- -
                        -1 CASA            I
                        -2 APARTAMENTO     I
                        -3 TERRENO         I
                        -4 LOTE            I
                        -5 IMOVEL RURAL    I
                        -6 FUNDOS          O
                        -7 PREV PRIVADA    O
                        -8 POUPANÇA        O
                        -9 SALARIO         R
                       -10 PRO LABORE      R
                       -11 OUTROS          R
                       -12 AUTOMOVEL       O
                       -13 PARTICIPACOES   O
                       -14 CJ COMERCIAL    I
                       -15 APLICACOES      O
                       -16 LOJA            I
                       -17 COTAS           O
                       -18 PATRIMONIO LIQ  O
                       -19 APOSENTADORIA   R
                       -20 MESADA          R
                       -21 FAIXA DE PL     O
                 * 
                 Novos.
                      -23 TOT BENS IMOV.  I
                      -24 TOT.OUTROS REND R
                      -25 TOT.BENS MOVEIS O
                      -26 TOT. APLIC. FIN O
                      -27 SAL. OU PROLAB. R
                      28 CAPITAL SOCIAL  O
                 
                   */

                int SFPTotOutrosRendimentos = -1;
                int SFPTotBensImoveis = -1;
                int SFPTotBensMoveis = -1;
                int SFPTotAplicacoesFinanceiras = -1;
                int SFPTotSalarioProLabore = -1;
                int SFPTotCapitalSocial = -1;
                int SFPTotPatrimonioLiquido = -1;

                try
                {
                    SFPTotOutrosRendimentos = System.Configuration.ConfigurationManager.AppSettings["SFPTotOutrosRendimentos"].DBToInt32();
                }
                catch { }
                try
                {
                    SFPTotBensImoveis = System.Configuration.ConfigurationManager.AppSettings["SFPTotBensImoveis"].DBToInt32();
                }
                catch { }
                try
                {
                    SFPTotBensMoveis = System.Configuration.ConfigurationManager.AppSettings["SFPTotBensMoveis"].DBToInt32();
                }
                catch { }
                try
                {
                    SFPTotAplicacoesFinanceiras = System.Configuration.ConfigurationManager.AppSettings["SFPTotAplicacoesFinanceiras"].DBToInt32();
                }
                catch { }
                try
                {
                    SFPTotSalarioProLabore = System.Configuration.ConfigurationManager.AppSettings["SFPTotSalarioProLabore"].DBToInt32();
                }
                catch { }
                try
                {
                    SFPTotCapitalSocial = System.Configuration.ConfigurationManager.AppSettings["SFPTotCapitalSocial"].DBToInt32();
                }
                catch { }
                try
                {
                    SFPTotPatrimonioLiquido = System.Configuration.ConfigurationManager.AppSettings["SFPTotPatrimonioLiquido"].DBToInt32();
                }
                catch { }

                //Imoveis
                if (item.CD_SFPGRUPO == 1
                || (item.CD_SFPGRUPO == 2)
                || (item.CD_SFPGRUPO == 3)
                || (item.CD_SFPGRUPO == 4)
                || (item.CD_SFPGRUPO == 5)
                || (item.CD_SFPGRUPO == 14)
                || (item.CD_SFPGRUPO == 16)
                || (item.CD_SFPGRUPO == SFPTotBensImoveis))
                {
                    lRetorno.VlTotalBensImoveis += item.VL_BEN;
                }
                else
                {
                    //SalarioProlabore
                    if (item.CD_SFPGRUPO == 9
                    || (item.CD_SFPGRUPO == 10)
                    || (item.CD_SFPGRUPO == 19)
                    || (item.CD_SFPGRUPO == SFPTotSalarioProLabore))
                    {
                        lRetorno.VlTotalSalarioProLabore += item.VL_BEN;
                    }
                    else
                    {
                        //Aplicação Financeira
                        if (item.CD_SFPGRUPO == 6
                        || (item.CD_SFPGRUPO == 13)
                        || (item.CD_SFPGRUPO == 15)
                        || (item.CD_SFPGRUPO == SFPTotAplicacoesFinanceiras))
                        {
                            lRetorno.VlTotalAplicacaoFinanceira += item.VL_BEN;
                        }
                        else
                        {
                            //Bens Moveis
                            if (item.CD_SFPGRUPO == 12
                            || (item.CD_SFPGRUPO == SFPTotBensMoveis))
                            {
                                lRetorno.VlTotalBensMoveis += item.VL_BEN;
                            }
                            else
                            {
                                //Outros Rendimentos
                                if (item.CD_SFPGRUPO == 7
                                || (item.CD_SFPGRUPO == 8)
                                || (item.CD_SFPGRUPO == 11)
                                || (item.CD_SFPGRUPO == 17)
                                || (item.CD_SFPGRUPO == 20)
                                || (item.CD_SFPGRUPO == 21)
                                || (item.CD_SFPGRUPO == SFPTotOutrosRendimentos))
                                {
                                    lRetorno.VlTotalOutrosRendimentos += item.VL_BEN;
                                }
                                else
                                {
                                    //Patrimônio Liquido
                                    if (item.CD_SFPGRUPO == 18
                                    || (item.CD_SFPGRUPO == SFPTotPatrimonioLiquido))
                                    {
                                        lRetorno.VlTotalPatrimonioLiquido += item.VL_BEN;
                                    }
                                    else
                                    {
                                        //Patrimônio Liquido
                                        if (item.CD_SFPGRUPO == SFPTotCapitalSocial)
                                        {
                                            lRetorno.VTotalCapitalSocial += item.VL_BEN;
                                        }
                                        else
                                        {
                                            lRetorno.VlTotalOutrosRendimentos += item.VL_BEN;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return lRetorno;
        }

        public static List<ClienteEnderecoInfo> ConverterEndereco(List<SinacorEnderecoInfo> pEnderecos)
        {
            List<ClienteEnderecoInfo> lRetorno = new List<ClienteEnderecoInfo>();
            ClienteEnderecoInfo lEndereco;

            foreach (SinacorEnderecoInfo item in pEnderecos)
            {
                lEndereco = new ClienteEnderecoInfo();
                lEndereco.DsBairro = item.NM_BAIRRO;
                lEndereco.NrCep = item.CD_CEP.Value;
                lEndereco.NrCepExt = item.CD_CEP_EXT.Value;
                lEndereco.DsCidade = item.NM_CIDADE;
                lEndereco.DsComplemento = item.NM_COMP_ENDE;
                lEndereco.StPrincipal = ToBool(item.IN_ENDE_CORR);
                lEndereco.IdCliente = 0;
                lEndereco.DsLogradouro = item.NM_LOGRADOURO;
                lEndereco.DsNumero = item.NR_PREDIO;
                lEndereco.CdPais = item.SG_PAIS;
                lEndereco.IdTipoEndereco = int.Parse(GetTipoEndereco(item.IN_TIPO_ENDE.Value).ToString());
                lEndereco.CdUf = item.SG_ESTADO;

                lRetorno.Add(lEndereco);
            }

            return lRetorno;
        }

        public static List<ClienteDiretorInfo> ConverterDiretor(SinacorDiretorInfo pDiretor)
        {
            List<ClienteDiretorInfo> lRetorno = new List<ClienteDiretorInfo>();

            if (null != pDiretor.NM_DIRETOR_1 && pDiretor.NM_DIRETOR_1.Trim().Length > 0)
            {
                ClienteDiretorInfo lDiretor1 = new ClienteDiretorInfo();
                lDiretor1.DsNome = pDiretor.NM_DIRETOR_1;
                lDiretor1.DsIdentidade = pDiretor.CD_DOC_IDENT_DIR1;
                lDiretor1.NrCpfCnpj = pDiretor.CD_CPFCGC_DIR1.Value.ToString();
                lDiretor1.IdCliente = 0;
                lRetorno.Add(lDiretor1);
            }
            if (null != pDiretor.NM_DIRETOR_2 && pDiretor.NM_DIRETOR_2.Trim().Length > 0)
            {
                ClienteDiretorInfo lDiretor2 = new ClienteDiretorInfo();
                lDiretor2.DsNome = pDiretor.NM_DIRETOR_2;
                lDiretor2.DsIdentidade = pDiretor.CD_DOC_IDENT_DIR2;
                lDiretor2.NrCpfCnpj = pDiretor.CD_CPFCGC_DIR2.Value.ToString();
                lDiretor2.IdCliente = 0;
                lRetorno.Add(lDiretor2);
            }
            if (null != pDiretor.NM_DIRETOR_3 && pDiretor.NM_DIRETOR_3.Trim().Length > 0)
            {
                ClienteDiretorInfo lDiretor3 = new ClienteDiretorInfo();
                lDiretor3.DsNome = pDiretor.NM_DIRETOR_3;
                lDiretor3.DsIdentidade = pDiretor.CD_DOC_IDENT_DIR3;
                lDiretor3.NrCpfCnpj = pDiretor.CD_CPFCGC_DIR3.Value.ToString();
                lDiretor3.IdCliente = 0;
                lRetorno.Add(lDiretor3);
            }

            return lRetorno;
        }

        public static List<ClienteEmitenteInfo> ConverterEmitente(List<SinacorEmitenteInfo> pEmitentes)
        {
            List<ClienteEmitenteInfo> lRetorno = new List<ClienteEmitenteInfo>();
            ClienteEmitenteInfo lEmitente;

            foreach (SinacorEmitenteInfo item in pEmitentes)
            {
                lEmitente = new ClienteEmitenteInfo();
                lEmitente.DsEmail = item.NM_E_MAIL;
                lEmitente.DsNome = item.NM_EMIT_ORDEM;
                lEmitente.DsNumeroDocumento = item.CD_DOC_IDENT_EMIT;
                lEmitente.CdSistema = item.CD_SISTEMA;
                lEmitente.DsData = item.TM_STAMP;
                lEmitente.IdCliente = 0;
                lEmitente.NrCpfCnpj = item.CD_CPFCGC_EMIT.Value.ToString();
                lEmitente.StPrincipal = ToBool(item.IN_PRINCIPAL);
                lEmitente.DtNascimento = null;

                lRetorno.Add(lEmitente);
            }
            return lRetorno;
        }

        public static List<ClienteContaInfo> ConverterConta(List<SinacorBovespaInfo> pBovespas, List<SinacorBmfInfo> pBmfs, List<SinacorAtividadeCcInfo> pCcs, List<SinacorAtividadeCustodiaInfo> pCus)
        {
            List<ClienteContaInfo> lRetorno = new List<ClienteContaInfo>();
            ClienteContaInfo lConta;

            foreach (SinacorBovespaInfo item in pBovespas)
            {
                lConta = new ClienteContaInfo();
                lConta.CdAssessor = item.CD_ASSESSOR.Value;
                lConta.CdCodigo = item.CD_CLIENTE.Value;
                lConta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BOL;
                lConta.IdCliente = 0;
                lConta.StContaInvestimento = ToBool(item.IN_CONTA_INV);
                lConta.StPrincipal = false;
                lConta.StAtiva = item.IN_SITUAC == 'A';
                lRetorno.Add(lConta);
            }
            foreach (SinacorAtividadeCcInfo item in pCcs)
            {
                lConta = new ClienteContaInfo();
                lConta.CdAssessor = item.CD_ASSESSOR.Value;
                lConta.CdCodigo = item.CD_CLIENTE.Value;
                lConta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CC;
                lConta.IdCliente = 0;
                lConta.StContaInvestimento = ToBool(item.IN_CONTA_INV);
                lConta.StPrincipal = false;
                lConta.StAtiva = item.IN_SITUAC == 'A';
                lRetorno.Add(lConta);
            }
            foreach (SinacorAtividadeCustodiaInfo item in pCus)
            {
                lConta = new ClienteContaInfo();
                lConta.CdAssessor = item.CD_ASSESSOR.Value;
                lConta.CdCodigo = item.CD_CLIENTE.Value;
                lConta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CUS;
                lConta.IdCliente = 0;
                lConta.StContaInvestimento = ToBool(item.IN_CONTA_INV);
                lConta.StPrincipal = false;
                lConta.StAtiva = item.IN_SITUAC == 'A';
                lRetorno.Add(lConta);
            }
            foreach (SinacorBmfInfo item in pBmfs)
            {
                lConta = new ClienteContaInfo();
                lConta.CdAssessor = item.CODASS.Value;
                lConta.CdCodigo = item.CODCLI.Value;
                lConta.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BMF;
                lConta.IdCliente = 0;
                lConta.StContaInvestimento = ToBool(item.IN_CONTA_INV);
                lConta.StPrincipal = false;
                lConta.StAtiva = item.STATUS == 'A';
                lRetorno.Add(lConta);
            }
            return lRetorno;
        }
    }
}
