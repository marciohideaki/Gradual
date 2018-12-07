using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos;
using Gradual.OMS.Library.Servicos;
using log4net;

namespace Gradual.Site.Www
{
    public static class DadosDeAplicacao
    {
        #region Globais

        public const string ANALISESEMERCADO_ANALISESECONOMICAS       = "AnalisesEMercado/AnalisesEconomicas.aspx";
        public const string ANALISESEMERCADO_ANALISESFUNDAMENTALISTAS = "AnalisesEMercado/AnalisesFundamentalistas.aspx";
        public const string ANALISESEMERCADO_ANALISESGRAFICAS         = "AnalisesEMercado/AnalisesGraficas.aspx";
        public const string ANALISESEMERCADO_CARTEIRASRECOMENDADAS    = "AnalisesEMercado/CarteirasRecomendadas.aspx";
        public const string INSTITUCIONAL_NIKKEIDESK                  = "Institucional/NikkeiDesk.aspx";
        public const string INSTITUCIONAL_NIKKEIDESK_JP               = "Institucional/NikkeiDesk-Jp.aspx";
        public const string INSTITUCIONAL_GRADIUSGESTAO               = "Institucional/GradiusGestao.aspx";
        
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Propriedades

        private static Dictionary<int, string> _NomesDasCorretoras = new Dictionary<int,string>();

        public static Dictionary<int, string> NomesDasCorretoras
        {
            get
            {
                if (_NomesDasCorretoras.Count == 0)
                {
                    AcessaDados lDados = null;
                    DataTable lTable = null;
                    DbCommand lCommand = null;

                    int lID;

                    try
                    {
                        lDados = new AcessaDados();
                        lDados.ConnectionStringName = "Risco";

                        lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_corretoras_lst");

                        lTable = lDados.ExecuteDbDataTable(lCommand);

                        foreach (DataRow dr in lTable.Rows)
                        {
                            if(dr["cdCorretora"] != DBNull.Value)
                            {
                                lID = Convert.ToInt32(dr["cdCorretora"]);

                                if (!_NomesDasCorretoras.ContainsKey(lID))
                                {
                                    _NomesDasCorretoras.Add(lID, Convert.ToString(dr["dsCorretora"]));
                                }
                            }
                        }
                    }
                    finally
                    {
                        lDados = null;
                        lTable = null;
                        lCommand.Dispose();
                        lCommand = null;
                    }
                }

                return _NomesDasCorretoras;
            }
        }

        private static Dictionary<string, int> _IDsDasPaginas = new Dictionary<string, int>();

        public static Dictionary<string, int> IDsDasPaginas
        {
            get
            {
                if (_IDsDasPaginas.Count == 0)
                {
                    _IDsDasPaginas.Add(ANALISESEMERCADO_ANALISESECONOMICAS,       72);
                    _IDsDasPaginas.Add(ANALISESEMERCADO_ANALISESFUNDAMENTALISTAS, 71);
                    _IDsDasPaginas.Add(ANALISESEMERCADO_ANALISESGRAFICAS,         70);
                    _IDsDasPaginas.Add(ANALISESEMERCADO_CARTEIRASRECOMENDADAS,    69);
                    _IDsDasPaginas.Add(INSTITUCIONAL_GRADIUSGESTAO,               83);
                    _IDsDasPaginas.Add(INSTITUCIONAL_NIKKEIDESK,                  58);
                    _IDsDasPaginas.Add(INSTITUCIONAL_NIKKEIDESK_JP,               86);
                }

                return _IDsDasPaginas;
            }
        }

        private static List<SinacorListaInfo> _Nacionalidades = null;

        public static List<SinacorListaInfo> Nacionalidades
        {
            get
            {
                if (_Nacionalidades == null)
                {
                    CarregarNacionalidades();
                }

                return _Nacionalidades;
            }

            set
            {
                _Nacionalidades = value;
            }
        }

        private static List<SinacorListaInfo> _Paises = null;

        public static List<SinacorListaInfo> Paises
        {
            get
            {
                if (_Paises == null)
                {
                    CarregarPaises();
                }

                return _Paises;
            }

            set
            {
                _Paises = value;
            }
        }

        private static List<SinacorListaInfo> _Estados = null;

        public static List<SinacorListaInfo> Estados
        {
            get
            {
                if (_Estados == null)
                {
                    CarregarEstados();
                }

                return _Estados;
            }

            set
            {
                _Estados = value;
            }
        }

        private static List<SinacorListaInfo> _EstadosCivis = null;

        public static List<SinacorListaInfo> EstadosCivis
        {
            get
            {
                if (_EstadosCivis == null)
                {
                    CarregarEstadosCivis();
                }

                return _EstadosCivis;
            }

            set
            {
                _EstadosCivis = value;
            }
        }

        private static List<SinacorListaInfo> _Escolaridades = null;

        public static List<SinacorListaInfo> Escolaridades
        {
            get
            {
                if (_Escolaridades == null)
                {
                    CarregarEscolaridades();
                }

                return _Escolaridades;
            }

            set
            {
                _Escolaridades = value;
            }
        }

        private static List<SinacorListaInfo> _Profissoes = null;

        public static List<SinacorListaInfo> Profissoes
        {
            get
            {
                if (_Profissoes == null)
                {
                    CarregarProfissoes();
                }

                return _Profissoes;
            }

            set
            {
                _Profissoes = value;
            }
        }

        private static List<SinacorListaInfo> _TiposDeDocumento = null;

        public static List<SinacorListaInfo> TiposDeDocumento
        {
            get
            {
                if (_TiposDeDocumento == null)
                {
                    CarregarTipoSDeDocumento();
                }

                return _TiposDeDocumento;
            }

            set
            {
                _TiposDeDocumento = value;
            }
        }

        private static List<SinacorListaInfo> _OrgaosEmissores = null;

        public static List<SinacorListaInfo> OrgaosEmissores
        {
            get
            {
                if (_OrgaosEmissores == null)
                {
                    CarregarOrgaosEmissores();
                }

                return _OrgaosEmissores;
            }

            set
            {
                _OrgaosEmissores = value;
            }
        }

        private static List<SinacorListaInfo> _Bancos = null;

        public static List<SinacorListaInfo> Bancos
        {
            get
            {
                if (_Bancos == null)
                {
                    CarregarBancos();
                }

                return _Bancos;
            }

            set
            {
                _Bancos = value;
            }
        }
        
        private static List<SinacorListaInfo> _Assessores = null;

        public static List<SinacorListaInfo> Assessores
        {
            get
            {
                if (_Assessores == null)
                {
                    CarregarAssessores();
                }


                return _Assessores;
            }

            set
            {
                _Assessores = value;
            }
        }
        
        private static List<SinacorListaInfo> _SituacoesLegais = null;

        public static List<SinacorListaInfo> SituacoesLegais
        {
            get
            {
                if (_SituacoesLegais == null)
                {
                    CarregarSituacoesLegais();
                }

                return _SituacoesLegais;
            }

            set
            {
                _SituacoesLegais = value;
            }
        }
                
        private static List<SinacorListaInfo> _TiposDeContaBancaria = null;

        public static List<SinacorListaInfo> TiposDeContaBancaria
        {
            get
            {
                if (_TiposDeContaBancaria == null)
                {
                    CarregarTiposDeContaBancaria();
                }

                return _TiposDeContaBancaria;
            }

            set
            {
                _TiposDeContaBancaria = value;
            }
        }

        #endregion

        #region Métodos Private

        private static List<SinacorListaInfo> CarregarListaDoSinacor(eInformacao pTipo)
        {
            List<SinacorListaInfo> lRetorno = new List<SinacorListaInfo>();

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ConsultarEntidadeCadastroRequest<SinacorListaInfo>  lRequest = new ConsultarEntidadeCadastroRequest<SinacorListaInfo>();
            ConsultarEntidadeCadastroResponse<SinacorListaInfo> lResponse;

            lRequest.EntidadeCadastro = new SinacorListaInfo(pTipo);
            lResponse = lServico.ConsultarEntidadeCadastro<SinacorListaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = lResponse.Resultado;
            }
            else
            {
                gLogger.ErrorFormat("Resposta com erro em CarregarListaDoSinacor(), ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SinacorListaInfo>({0}): [{1}]\r\n[{2}]"
                                    , pTipo.ToString()
                                    , lResponse.StatusResposta
                                    , lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        private static void CarregarNacionalidades()
        {
            _Nacionalidades = CarregarListaDoSinacor(eInformacao.Nacionalidade);
        }

        private static void CarregarPaises()
        {
            _Paises = CarregarListaDoSinacor(eInformacao.Pais);
        }

        private static void CarregarEstados()
        {
            _Estados = CarregarListaDoSinacor(eInformacao.Estado);
        }

        private static void CarregarEstadosCivis()
        {
            _EstadosCivis = CarregarListaDoSinacor(eInformacao.EstadoCivil);
        }

        private static void CarregarEscolaridades()
        {
            _Escolaridades = CarregarListaDoSinacor(eInformacao.Escolaridade);
        }

        private static void CarregarProfissoes()
        {
            _Profissoes = CarregarListaDoSinacor(eInformacao.ProfissaoPF);
        }

        private static void CarregarTipoSDeDocumento()
        {
            _TiposDeDocumento = CarregarListaDoSinacor(eInformacao.TipoDocumento);
        }

        private static void CarregarOrgaosEmissores()
        {
            _OrgaosEmissores = CarregarListaDoSinacor(eInformacao.OrgaoEmissor);
        }

        private static void CarregarBancos()
        {
            _Bancos = CarregarListaDoSinacor(eInformacao.Banco);
        }
        
        private static void CarregarAssessores()
        {
            _Assessores = CarregarListaDoSinacor(eInformacao.AssessorPadronizado);
        }

        private static void CarregarSituacoesLegais()
        {
            _SituacoesLegais = CarregarListaDoSinacor(eInformacao.SituacaoLegalRepresentante);
        }

        private static void CarregarTiposDeContaBancaria()
        {
            _TiposDeContaBancaria = new List<SinacorListaInfo>();

            _TiposDeContaBancaria.Add(new SinacorListaInfo() { Id="CC", Value="CONTA CORRENTE" });
            _TiposDeContaBancaria.Add(new SinacorListaInfo() { Id="CI", Value="CONTA INVESTIMENTO" });
            _TiposDeContaBancaria.Add(new SinacorListaInfo() { Id="CC", Value="CONTA POUPANÇA" });
        }

        #endregion

        #region Métodos Públicos

        public static string BuscarDadoDoSinacor(eInformacao pTipoInformacao, string pId)
        {
            List<SinacorListaInfo> lLista = null;

            switch (pTipoInformacao)
            {
                case eInformacao.Assessor: lLista = DadosDeAplicacao.Assessores;
                    break;
                case eInformacao.AssessorPadronizado: lLista = DadosDeAplicacao.Assessores;
                    break;
                case eInformacao.AtividadePF: lLista = DadosDeAplicacao.Profissoes;
                    break;
                case eInformacao.AtividadePFePJ: lLista = DadosDeAplicacao.Profissoes;
                    break;
                case eInformacao.AtividadePJ: lLista = DadosDeAplicacao.Profissoes;
                    break;
                case eInformacao.Banco: lLista = DadosDeAplicacao.Bancos;
                    break;
                case eInformacao.Escolaridade: lLista = DadosDeAplicacao.Escolaridades;
                    break;
                case eInformacao.Estado: lLista = DadosDeAplicacao.Estados;
                    break;
                case eInformacao.EstadoCivil: lLista = DadosDeAplicacao.EstadosCivis;
                    break;
                case eInformacao.Nacionalidade: lLista = DadosDeAplicacao.Nacionalidades;
                    break;
                case eInformacao.OrgaoEmissor: lLista = DadosDeAplicacao.OrgaosEmissores;
                    break;
                case eInformacao.Pais: lLista = DadosDeAplicacao.Paises;
                    break;
                case eInformacao.ProfissaoPF: lLista = DadosDeAplicacao.Profissoes;
                    break;
                case eInformacao.SituacaoLegalRepresentante: lLista = DadosDeAplicacao.SituacoesLegais;
                    break;
                case eInformacao.TipoCliente: lLista = null;
                    break;
                case eInformacao.TipoConta: lLista = DadosDeAplicacao.TiposDeContaBancaria;
                    break;
                case eInformacao.TipoDocumento: lLista = DadosDeAplicacao.TiposDeDocumento;
                    break;
                case eInformacao.TipoInvestidorPJ: lLista = null;
                    break;
                default:
                    break;
            }

            if (lLista != null)
            {
                foreach (SinacorListaInfo lItem in lLista)
                {
                    if (lItem.Id == pId)
                    {
                        return lItem.Value;
                    }
                }
            }

            return string.Empty;
        }
 
        #endregion
    }
}