using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Enum;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using Newtonsoft.Json;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Posicao;
using System.Web.UI.WebControls;
using System.Web.UI;
using Gradual.OMS.RelatoriosFinanc.Lib;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class NotaDeCorretagemImpressaoEmLote : PaginaBase
    {
        #region | Atributos

        private List<DateTime> gDataDasNotas = null;

        #endregion

        #region | Propriedades

        private int GetCodCorretora
        {
            get
            {
                return 2271;
            }
        }

        private string GetTipoMercado
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request["TipoMercado"]))
                    return string.Empty;

                return this.Request["TipoMercado"];
            }
        }

        private int GetCodCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBovespaCliente"], out lRetorno);

                if (lRetorno == default(int))
                    int.TryParse(this.Request["CdBmfCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetNomeCliente
        {
            get
            {
                return this.Request["NomeCliente"];
            }
        }

        private List<DateTime> GetDataNota
        {
            get
            {
                if (null == this.gDataDasNotas && !string.IsNullOrWhiteSpace(this.Request["DatasNC"]))
                {
                    var lArrayDatas = this.Request["DatasNC"].Trim('"').Split(';');

                    this.gDataDasNotas = new List<DateTime>();

                    if (null != lArrayDatas && lArrayDatas.Length > 0)
                        for (int i = 0; i < lArrayDatas.Length; i++)
                            this.gDataDasNotas.Add(lArrayDatas[i].DBToDateTime());
                }

                return this.gDataDasNotas;
            }
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.MontarNotasNaTela();
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Ocorreu um erro ao processar a requisição.", ex);
            }
        }

        protected void rptNotaDeCorretagemImpressaoPorLote_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                Repeater lRepeaterDetalhesDaNota = this.BuscarRepeater(e.Item.Controls);

                lRepeaterDetalhesDaNota.DataSource = ((TransporteNotaDeCorretagem)(e.Item.DataItem)).DetalhesDaNota;
                lRepeaterDetalhesDaNota.DataBind();
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Ocorreu um erro ao processar a requisição.", ex);
            }
        }

        #endregion

        #region | Métodos

        private void MontarNotasNaTela()
        {
            var lListaTraducao = new TransporteNotaDeCorretagem().TraduzirLista(this.CarregarRelatorio(), this.GetNomeCliente, this.GetTipoMercado);

            lListaTraducao.Sort((nc1, nc2) => Comparer<DateTime>.Default.Compare(nc1.DataEmissaoNota.DBToDateTime(), nc2.DataEmissaoNota.DBToDateTime()));

            this.rptNotaDeCorretagemImpressaoPorLote.DataSource = lListaTraducao;
            this.rptNotaDeCorretagemImpressaoPorLote.DataBind();
        }

        private Repeater BuscarRepeater(ControlCollection pParametro)
        {
            if (null != pParametro && pParametro.Count > 0)
                for (int i = 0; i < pParametro.Count; i++)
                    if (pParametro[i] is Repeater && ((Repeater)pParametro[i]).ID == "rptDetalhesDaNota")
                        return (Repeater)pParametro[i];

            return new Repeater();
        }

        private List<Gradual.OMS.RelatoriosFinanc.Lib.Dados.NotaDeCorretagemExtratoInfo> CarregarRelatorio()
        {
            var lRetorno = new List<Gradual.OMS.RelatoriosFinanc.Lib.Dados.NotaDeCorretagemExtratoInfo>();

            if (null != this.GetDataNota && this.GetDataNota.Count > 0)
            {
                try
                {
                    var lServicoAtivador = Ativador.Get<IServicoRelatoriosFinanceiros>();
                    var lResponse = new Gradual.OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoResponse();

                    this.GetDataNota.ForEach(lDataDaNota =>
                    {   //--> Fazendo a consulta das datas do relatório.
                        lResponse = lServicoAtivador.ConsultarNotaDeCorretagem(
                            new OMS.RelatoriosFinanc.Lib.Mensagens.NotaDeCorretagemExtratoRequest()
                            {
                                ConsultaCodigoCliente = this.GetCodCliente,
                                ConsultaCodigoCorretora = this.GetCodCorretora, //ConfiguracoesValidadas.CodigoCorretora,
                                ConsultaDataMovimento = lDataDaNota,
                                ConsultaTipoDeMercado = this.GetTipoMercado,
                                ConsultaProvisorio = false, // this.GetProvisorio,
                            });

                        if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK) //--> Registrando o log.
                        {
                            lRetorno.Add(lResponse.Relatorio); //--> Adicionando o relatório à lista.
                            base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });
                        }
                        else
                            throw new Exception(string.Format("{0}-{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                    });
                }
                catch (Exception ex)
                {
                    base.RetornarErroAjax("Houve um erro ao tentar gerar a nota de corretagem", ex);
                    return new List<Gradual.OMS.RelatoriosFinanc.Lib.Dados.NotaDeCorretagemExtratoInfo>();
                }
            }

            return lRetorno;
        }

        #endregion
    }
}