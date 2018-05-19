using Gradual.Spider.PositionClient.DbLib;
using Gradual.Spider.PostTradingClientEngine.App_Codigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gradual.Spider.PostTradingClientEngine.Backend
{
    public partial class Acompanhamento : PaginaBase
    {
        private bool gNova = false;

        private static List<Objects.Ordem> gListaOrdens;

        private List<Objects.Ordem> SessionUltimoResultadoDeBuscaAcompanhamento
        {
            get
            {
                return gListaOrdens != null ? gListaOrdens : null;
            }
            set
            {
                gListaOrdens = value;
            }
        }

        private static List<Objects.Ordem> gListaOrdensOrdenas;

        private List<Objects.Ordem> SessionUltimoResultadoDeBuscaAcompanhamentoOrdenado
        {
            get
            {
                return gListaOrdensOrdenas != null ? gListaOrdensOrdenas : null;
            }
            set
            {
                gListaOrdensOrdenas = value;
            }
        }

        /// <summary>
        /// Código de cliente para filtro
        /// </summary>
        public int CodigoCliente
        {
            get
            {
                int lRetorno = default(int);

                int.TryParse(this.Request["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }

        /// <summary>
        /// Código de Instrumento para filtro
        /// </summary>
        public string CodigoInstrumento
        {
            get
            {
                var lRetorno = default(string);

                if (this.Request["CodigoInstrumento"] != null)
                {
                    lRetorno = this.Request["CodigoInstrumento"].ToString();
                }

                return lRetorno;
            }
        }

        public string Sentido
        {
            get
            {
                String lRetorno = String.Empty;

                if (this.Request["Sentido"] != null)
                {
                    lRetorno = this.Request["Sentido"];

                    if (!String.IsNullOrEmpty(lRetorno))
                    {
                        if (lRetorno.Substring(lRetorno.Length - 1).Equals(";"))
                        {
                            lRetorno = lRetorno.Remove(lRetorno.Length - 1);
                        }
                    }

                }

                return lRetorno;
            }
        }

        public string Status
        {
            get
            {
                System.String lRetorno = String.Empty;

                if (this.Request["Status"] != null)
                {
                    lRetorno = this.Request["Status"].ToString();

                    if (!String.IsNullOrEmpty(lRetorno))
                    {
                        if (lRetorno.Substring(lRetorno.Length - 1).Equals(";"))
                        {
                            lRetorno = lRetorno.Remove(lRetorno.Length - 1);
                        }
                    }
                }

                return lRetorno;
            }
        }

        public System.String Exchange
        {
            get
            {
                System.String lRetorno = null;

                if (this.Request["Exchange"] != null)
                {
                    lRetorno = this.Request["Exchange"].ToString();
                }

                return lRetorno;
            }
        }

        public System.String HoraInicio
        {
            get
            {
                System.String lRetorno = null;

                if (this.Request["HoraInicio"] != null)
                {
                    lRetorno = this.Request["HoraInicio"].ToString();
                }

                return lRetorno;
            }
        }

        public System.String Horafim
        {
            get
            {
                System.String lRetorno = null;

                if (this.Request["HoraFim"] != null)
                {
                    lRetorno = this.Request["HoraFim"].ToString();
                }

                return lRetorno;
            }
        }

        public int CodigoOrdem
        {
            get
            {
                int lRetorno = 0;

                if (this.Request["CodigoOrdem"] != null)
                {
                    lRetorno = Convert.ToInt32(this.Request["CodigoOrdem"]);
                }

                return lRetorno;
            }
        }

        public string Mercados
        {
            get
            {
                System.String lRetorno = String.Empty;

                if (this.Request["Mercados"] != null)
                {
                    lRetorno = this.Request["Mercados"].ToString();

                    if (!String.IsNullOrEmpty(lRetorno))
                    {
                        if (lRetorno.Substring(lRetorno.Length - 1).Equals(";"))
                        {
                            lRetorno = lRetorno.Remove(lRetorno.Length - 1);
                        }
                    }
                }

                return lRetorno;
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                RegistrarRespostasAjax
                    (
                        new string[] 
                        {
                            "BuscarAcompanhamento",
                            "BuscarAcompanhamentoPaginado",
                            "BuscarDetalhe"
                }, new ResponderAcaoAjaxDelegate[]
                {
                    BuscarAcompanhamento,
                    BuscarAcompanhamentoPaginado,
                    BuscarDetalhe
                });


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string BuscarAcompanhamento()
        {
            string lRetorno = string.Empty;

            Gradual.Generico.Dados.AcessaDados lDados = null;
            System.Data.DataTable lTable = null;
            System.Data.Common.DbCommand lCommand = null;

            try
            {
                int lRegistrosPorPagina = default(int);
                int? lCodigoCliente                 = this.CodigoCliente;
                System.String lCodigoInstrumento    = this.CodigoInstrumento;
                System.String lSentido              = this.Sentido;
                System.String lStatus               = this.Status;
                System.String lExchange             = this.Exchange;
                System.String lHoraInicio           = this.HoraInicio;
                System.String lHoraFim              = this.Horafim;
                int.TryParse(Request["rows"], out lRegistrosPorPagina);

                lDados = new Generico.Dados.AcessaDados();
                lDados.ConnectionStringName = "GradualSpider";

                lCommand = lDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_acompanhamento_buscar_ordens");

                if (lCodigoCliente.Equals(0))
                {
                    lDados.AddInParameter(lCommand, "@CodigoCliente", System.Data.DbType.Int32, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@CodigoCliente", System.Data.DbType.Int32, lCodigoCliente);
                }

                if (String.IsNullOrEmpty(lCodigoInstrumento))
                {
                    lDados.AddInParameter(lCommand, "@CodigoInstrumento", System.Data.DbType.String, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@CodigoInstrumento", System.Data.DbType.String, lCodigoInstrumento);
                }

                if (String.IsNullOrEmpty(lSentido) || lSentido.Equals("0"))
                {
                    lDados.AddInParameter(lCommand, "@Sentido", System.Data.DbType.String, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@Sentido", System.Data.DbType.String, lSentido);
                }

                if (String.IsNullOrEmpty(lStatus))
                {
                    lDados.AddInParameter(lCommand, "@Status", System.Data.DbType.String, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@Status", System.Data.DbType.String, lStatus);
                }

                if (String.IsNullOrEmpty(lExchange) || lExchange.Equals("Ambos"))
                {
                    lDados.AddInParameter(lCommand, "@Exchange", System.Data.DbType.String, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@Exchange", System.Data.DbType.String, lExchange);
                }


                if (String.IsNullOrEmpty(lHoraInicio))
                {
                    lDados.AddInParameter(lCommand, "@HoraInicio", System.Data.DbType.String, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@HoraInicio", System.Data.DbType.String, lHoraInicio);
                }

                if (String.IsNullOrEmpty(lHoraFim))
                {
                    lDados.AddInParameter(lCommand, "@HoraFim", System.Data.DbType.String, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@HoraFim", System.Data.DbType.String, lHoraFim);
                }

                lTable = lDados.ExecuteDbDataTable(lCommand);

                List<Objects.Ordem> lListaOrdens = new List<Objects.Ordem>();

                foreach (System.Data.DataRow dr in lTable.Rows)
                {
                    lListaOrdens.Add
                        (
                            new Objects.Ordem
                            {
                                Cliente             = dr["Cliente"].DBToInt32(),
                                Exchange            = dr["Exchange"].DBToString(),
                                CodigoOrdem         = dr["CodigoOrdem"].DBToInt32(),
                                Ativo               = dr["Ativo"].DBToString(),
                                Sentido             = dr["Sentido"].DBToInt32().Equals(1) ? "Compra" : "Venda",
                                Status              = dr["Status"].DBToString().ToUpper().Equals("NOVA") ? "ABERTA" : dr["Status"].DBToString().ToUpper(),
                                QuantidadeOrdem     = dr["QuantidadeOrdem"].DBToInt32(),
                                QuantidadeExecutada = dr["QuantidadeExecutada"].DBToInt32(),
                                QuantidadeAberta    = dr["QuantidadeAberta"].DBToInt32(),
                                Preco               = dr["Preco"].DBToDecimal(),
                                Horario             = dr["DataHoraRegistro"].DBToDateTime().ToString("HH:mm:ss"),
                                PrecoStop           = dr["PrecoStop"].DBToDecimal(),
                                TipoOrdem           = dr["TipoOrdem"].DBToString(),
                                DataValidade        = dr["DataValidade"].DBToDateTime().ToString("dd/MM/yyyy"),
                                Plataforma          = dr["Plataforma"].DBToString(),
                                ExecBroker          = dr["Execbroker"].DBToString()
                            }
                        );
                }

                this.SessionUltimoResultadoDeBuscaAcompanhamento = lListaOrdens;

                TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

                lRetornoLista = new TransporteDeListaPaginada(SessionUltimoResultadoDeBuscaAcompanhamento);
                
                lRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = lListaOrdens.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = (int)Math.Ceiling((double)(lListaOrdens.Count() / lRegistrosPorPagina));

                lRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(lRetornoLista);
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", ex);
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lRetorno;

        }

        private string BuscarAcompanhamentoPaginado()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            bool.TryParse(Request["Nova"], out gNova);

            if (this.SessionUltimoResultadoDeBuscaAcompanhamento != null && !gNova)
            {
                int lPagina;
                int lRegistrosPorPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    if (int.TryParse(Request["rows"], out lRegistrosPorPagina))
                    {
                        lLista = BuscarPaginaDeResultadosAcompanhamento(lPagina, lRegistrosPorPagina, this.Request["sidx"], this.Request["sord"]);
                    }

                }
            }
            else
            {
                return BuscarAcompanhamento();
            }

            lRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultadosAcompanhamento(int pPagina, int pRows, string pCampo, string pOrdenacao)
        {
            var lRetorno = new TransporteDeListaPaginada();

            var lLista = new List<Objects.Ordem>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * pRows);
            lIndiceFinal = (pPagina) * pRows;

            if (!String.IsNullOrEmpty(pCampo) && !String.IsNullOrEmpty(pOrdenacao))
            {
                this.SessionUltimoResultadoDeBuscaAcompanhamentoOrdenado = this.SessionUltimoResultadoDeBuscaAcompanhamento;
                OrdenarAcompanhamento(pCampo, pOrdenacao);

                for (int a = lIndiceInicial; a < lIndiceFinal; a++)
                {
                    if (a < this.SessionUltimoResultadoDeBuscaAcompanhamentoOrdenado.Count)
                    {
                        lLista.Add(this.SessionUltimoResultadoDeBuscaAcompanhamentoOrdenado[a]);
                    }
                }
            }
            else
            {
                for (int a = lIndiceInicial; a < lIndiceFinal; a++)
                {
                    if (a < this.SessionUltimoResultadoDeBuscaAcompanhamento.Count)
                    {
                        lLista.Add(this.SessionUltimoResultadoDeBuscaAcompanhamento[a]);
                    }
                }
            }
            

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBuscaAcompanhamento.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / pRows));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        //private void OrdenarAcompanhamento(ref List<Objects.Ordem> pObjeto, string pCampo, string pDirecao)
        private void OrdenarAcompanhamento(string pCampo, string pDirecao)
        {
            List<Objects.Ordem> lObjeto = this.SessionUltimoResultadoDeBuscaAcompanhamentoOrdenado;

            switch (pCampo)
            {
                case "CodigoOrdem":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.CodigoOrdem - b.CodigoOrdem));
                    else
                        lObjeto.Sort((a, b) => (b.CodigoOrdem - a.CodigoOrdem));
                    break;
                case "Cliente":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.Cliente - b.Cliente));
                    else
                        lObjeto.Sort((a, b) => (b.Cliente - a.Cliente));
                    break;
                case "Exchange":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => string.Compare(a.Exchange, b.Exchange));
                    else
                        lObjeto.Sort((a, b) => string.Compare(b.Exchange, a.Exchange));
                    break;
                case "Ativo":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => string.Compare(a.Ativo, b.Ativo));
                    else
                        lObjeto.Sort((a, b) => string.Compare(b.Ativo, a.Ativo));
                    break;
                case "Sentido":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => string.Compare(a.Sentido, b.Sentido));
                    else
                        lObjeto.Sort((a, b) => string.Compare(b.Sentido, a.Sentido));
                    break;
                case "Status":
                    if ("asc".Equals(pDirecao))
                        lObjeto = lObjeto.OrderBy(x => x.Status).ToList();
                    else
                        lObjeto = lObjeto.OrderByDescending(x => x.Status).ToList();
                    break;
                case "QuantidadeOrdem":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.QuantidadeOrdem - b.QuantidadeOrdem));
                    else
                        lObjeto.Sort((a, b) => (b.QuantidadeOrdem - a.QuantidadeOrdem));
                    break;
                case "QuantidadeExecutada":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.QuantidadeExecutada - b.QuantidadeExecutada));
                    else
                        lObjeto.Sort((a, b) => (b.QuantidadeExecutada - a.QuantidadeExecutada));
                    break;
                case "QuantidadeAberta":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.QuantidadeAberta - b.QuantidadeAberta));
                    else
                        lObjeto.Sort((a, b) => (b.QuantidadeAberta - a.QuantidadeAberta));
                    break;
                case "Preco":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => Decimal.Compare(a.Preco, b.Preco));
                    else
                        lObjeto.Sort((a, b) => Decimal.Compare(b.Preco, a.Preco));
                    break;
                case "Horario":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => string.Compare(a.Horario, b.Horario));
                    else
                        lObjeto.Sort((a, b) => string.Compare(b.Horario, a.Horario));
                    break;
                case "PrecoStop":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => Decimal.Compare(a.PrecoStop, b.PrecoStop));
                    else
                        lObjeto.Sort((a, b) => Decimal.Compare(b.PrecoStop, a.PrecoStop));
                    break;
                case "TipoOrdem":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => System.String.Compare(a.TipoOrdem, b.TipoOrdem));
                    else
                        lObjeto.Sort((a, b) => System.String.Compare(b.TipoOrdem, a.TipoOrdem));
                    break;
                case "DataValidade":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => DateTime.Compare(DateTime.ParseExact(a.DataValidade, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture), DateTime.ParseExact(b.DataValidade, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)));
                    else
                        lObjeto.Sort((a, b) => DateTime.Compare(DateTime.ParseExact(b.DataValidade, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture), DateTime.ParseExact(a.DataValidade, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)));
                    break;
                case "Plataforma":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => System.String.Compare(a.Plataforma, b.Plataforma));
                    else
                        lObjeto.Sort((a, b) => System.String.Compare(b.Plataforma, a.Plataforma));
                    break;
                case "ExecBroker":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => System.String.Compare(a.ExecBroker, b.ExecBroker));
                    else
                        lObjeto.Sort((a, b) => System.String.Compare(b.ExecBroker, a.ExecBroker));
                    break;
            }

            this.SessionUltimoResultadoDeBuscaAcompanhamentoOrdenado = lObjeto;
        }

        public string BuscarDetalhe()
        {
            string lRetorno = string.Empty;

            Gradual.Generico.Dados.AcessaDados lDados = null;
            System.Data.DataTable lTable = null;
            System.Data.Common.DbCommand lCommand = null;

            try
            {
                int lCodigoOrdem = this.CodigoOrdem;

                lDados = new Generico.Dados.AcessaDados();
                lDados.ConnectionStringName = "GradualSpider";

                lCommand = lDados.CreateCommand(System.Data.CommandType.StoredProcedure, "[prc_acompanhamento_buscar_detalhes]");

                if (lCodigoOrdem.Equals(0))
                {
                    lDados.AddInParameter(lCommand, "@OrderID", System.Data.DbType.Int32, null);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "@OrderID", System.Data.DbType.Int32, lCodigoOrdem);
                }

                lTable = lDados.ExecuteDbDataTable(lCommand);

                List<Objects.Detalhe> lListaOrdens = new List<Objects.Detalhe>();

                foreach (System.Data.DataRow dr in lTable.Rows)
                {
                    int lQuantidade = 0;
                    int lQuantidadeExecutada = 0;
                    int lSaldo = 0;
                    lQuantidade = dr["OrderQty"].DBToInt32();
                    lQuantidadeExecutada = dr["CumQty"].DBToInt32();
                    lSaldo = lQuantidade - lQuantidadeExecutada;

                    lListaOrdens.Add
                        (
                            new Objects.Detalhe
                            {
                                OrderDetailID = dr["OrderDetailID"].DBToString(),
                                TransactID = dr["TransactID"].DBToString(),
                                OrderID = dr["OrderID"].DBToString(),
                                OrderQty = dr["OrderQty"].DBToString(),
                                OrdQtyRemaining = dr["OrdQtyRemaining"].DBToString(),
                                Price = String.Format("{0:C}", dr["Price"].DBToDecimal()),
                                StopPx = dr["StopPx"].DBToString(),
                                OrderStatusID = dr["OrderStatusID"].DBToString(),
                                Status = dr["Status"].DBToString(),
                                TransactTime = dr["TransactTime"].DBToString(),
                                Description = dr["Descricao"].DBToString(),
                                TradeQty = dr["TradeQty"].DBToString(),
                                CumQty = dr["CumQty"].DBToString(),
                                FixMsgSeqNum = dr["FixMsgSeqNum"].DBToString(),
                                CxlRejResponseTo = dr["CxlRejResponseTo"].DBToString(),
                                CxlRejReason = dr["CxlRejReason"].DBToString(),
                                MsgFixDetail = dr["MsgFixDetail"].DBToString(),
                                Contrabroker = dr["Contrabroker"].DBToString(),
                                Saldo = lSaldo.ToString()
                            }
                        );
                }

                TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

                lRetornoLista = new TransporteDeListaPaginada(lListaOrdens);

                lRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(lRetornoLista);

                //lRetornoLista.TotalDeItens = lResponse.Resultado.Count;
                lRetornoLista.TotalDeItens = lListaOrdens.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = (int)Math.Ceiling((double)(lListaOrdens.Count() / 30));

                lRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(lRetornoLista);

                //lRetorno = RetornarSucessoAjax(lListaOrdens, "Foram encontrados [{0}] operacoes" + lListaOrdens.Count, lListaOrdens.Count);
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro ao deserializar objeto JSON [{0}]", ex);
            }
            finally
            {
                lDados = null;
                lTable = null;
                lCommand.Dispose();
                lCommand = null;
            }

            return lRetorno;

        }



    }
}