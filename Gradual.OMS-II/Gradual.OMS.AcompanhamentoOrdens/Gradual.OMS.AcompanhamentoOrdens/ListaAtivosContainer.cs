using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using log4net;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.OMS.Library.Servicos;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Gradual.OMS.AcompanhamentoOrdens
{
    public class ListaAtivosContainer
    {
        public const string SEGMENTOMERCADO_BMF_FUTURO = "FUT";
        public const string SEGMENTOMERCADO_BMF_VISTA = "SPOT";
        public const string SEGMENTOMERCADO_BMF_OPCAO_VISTA = "SOPT";
        public const string SEGMENTOMERCADO_BMF_OPCAO_FUTURO = "FOPT";
        public const string SEGMENTOMERCADO_BMF_TERMO = "DTERM";

        public const string SEGMENTOMERCADO_BOVESPA_VISTA = "01";
        public const string SEGMENTOMERCADO_BOVESPA_VISTA_OUTROS = "91";
        public const string SEGMENTOMERCADO_BOVESPA_TERMO = "02";
        public const string SEGMENTOMERCADO_BOVESPA_FRACIONARIO = "03";
        public const string SEGMENTOMERCADO_BOVESPA_OPCOES = "04";
        public const string SEGMENTOMERCADO_BOVESPA_EXERCICIO_OPCOES = "09";
        public const string SEGMENTOMERCADO_BOVESPA_INDICES = "90";

        public const string INDICADOROPCAO_COMPRA = "C";
        public const string INDICADOROPCAO_VENDA = "P";

        public const string DESCRICAO_TIPO_OPCAO_BMF_FUTURO = "BM&F Futuro";
        public const string DESCRICAO_TIPO_OPCAO_BMF_VISTA = "BM&F Opções";
        public const string DESCRICAO_TIPO_OPCAO_BOVESPA_COMPRA = "Bovespa Compra";
        public const string DESCRICAO_TIPO_OPCAO_BOVESPA_VENDA = "Bovespa Venda";


        private static readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DataTable _ListaAtivos = null;

        public static DataTable ListaAtivos
        {
            get
            {
                if (_ListaAtivos == null)
                {
                    gLog4Net.Info("Iniciando carga da lista de ativos.");

                    _ListaAtivos = new DataTable();

                    lock (_ListaAtivos)
                    {
                        try
                        {
                            _ListaAtivos.Columns.Add("Instrumento", typeof(string));
                            _ListaAtivos.Columns.Add("RazaoSocial", typeof(string));
                            _ListaAtivos.Columns.Add("CodigoISIN", typeof(string));
                            _ListaAtivos.Columns.Add("LotePadrao", typeof(string));
                            _ListaAtivos.Columns.Add("CodigoPapelObjeto", typeof(string));
                            _ListaAtivos.Columns.Add("SegmentoMercado", typeof(string));
                            _ListaAtivos.Columns.Add("ComposicaoIndice", typeof(string));
                            _ListaAtivos.Columns.Add("PrecoExercicio", typeof(string));
                            _ListaAtivos.Columns.Add("DataVencimento", typeof(string));
                            _ListaAtivos.Columns.Add("IndicadorOpcao", typeof(string));
                            _ListaAtivos.Columns.Add("TipoOpcao", typeof(string));

                            for (int bloco = 1; ; bloco++)
                            {
                                IServicoCadastroPapeis lServico = Ativador.Get<IServicoCadastroPapeis>();

                                ListarCadastroPapeisMdsRequest lRequest = new ListarCadastroPapeisMdsRequest();
                                lRequest.NumBlocoDados = bloco;

                                ListarCadastroPapeisMdsResponse lResponse = lServico.ConsultarCadastroPapeisMds(lRequest);

                                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                                {
                                    foreach (string item in lResponse.ListaCadastroPapeisMds)
                                    {
                                        string tipoOpcao = "";
                                        string[] campos = Regex.Split(item, @"\|\|");

                                        if (campos[5] != null)
                                        {
                                            if (campos[5].Equals(SEGMENTOMERCADO_BMF_OPCAO_FUTURO))
                                                tipoOpcao = DESCRICAO_TIPO_OPCAO_BMF_FUTURO;

                                            else if (campos[5].Equals(SEGMENTOMERCADO_BMF_OPCAO_VISTA))
                                                tipoOpcao = DESCRICAO_TIPO_OPCAO_BMF_VISTA;

                                            else
                                            {
                                                if (campos[3] != null)
                                                {
                                                    if (campos[3].Equals(INDICADOROPCAO_COMPRA))
                                                        tipoOpcao = DESCRICAO_TIPO_OPCAO_BOVESPA_COMPRA;
                                                    else
                                                        tipoOpcao = DESCRICAO_TIPO_OPCAO_BOVESPA_VENDA;
                                                }
                                            }
                                        }

                                        _ListaAtivos.Rows.Add(
                                            campos[0], // Instrumento
                                            campos[1], // RazaoSocial
                                            campos[6], // CodigoISIN
                                            campos[2], // LotePadrao
                                            campos[4], // CodigoPapelObjeto
                                            campos[5], // SegmentoMercado
                                            campos[7], // ComposicaoIndice
                                            Convert.ToDouble(campos[8].Replace('.', ',')).ToString("N2", CultureInfo.CreateSpecificCulture("pt-Br")), // PrecoExercicio //HACK: o preço Exercício do cadastro do MDS está vindo com '.' no separador invés da ','
                                            campos[9].Substring(6, 2) + "/" + campos[9].Substring(4, 2) + "/" + campos[9].Substring(0, 4), // DataVencimento
                                            campos[3], // IndicadorOpcao
                                            tipoOpcao
                                            );
                                    }
                                }
                                if (!lResponse.ContinuaDados)
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            gLog4Net.Error("Aplicacao.ListaAtivos.get()", ex);
                        }
                    }

                    gLog4Net.Info("Aplicacao.ListaAtivos.get(): Lista de ativos carregada com sucesso.");
                }
                else
                {
                    lock (_ListaAtivos)
                    { }
                }
                return _ListaAtivos;
            }
        }

    }
}
