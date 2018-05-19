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
    public partial class VendasDescobertas : PaginaBase
    {

        private bool gNova = false;

        private static List<Objects.VendaDescoberta> gListaVendasDescobertas;

        private List<Objects.VendaDescoberta> SessionUltimoResultadoDeBuscaVendasDescobertas
        {
            get
            {
                return gListaVendasDescobertas != null ? gListaVendasDescobertas : null;
            }
            set
            {
                gListaVendasDescobertas = value;
            }
        }

        private static List<Objects.VendaDescoberta> gListaVendasDescobertasOrdenada;

        private List<Objects.VendaDescoberta> SessionUltimoResultadoDeBuscaVendasDescobertasOrdenada
        {
            get
            {
                return gListaVendasDescobertasOrdenada != null ? gListaVendasDescobertasOrdenada : null;
            }
            set
            {
                gListaVendasDescobertasOrdenada = value;
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
                                "BuscarVendasDescobertas",
                                "BuscarVendasDescobertasPaginado"

                            }
                            , new ResponderAcaoAjaxDelegate[]
                            {
                                BuscarVendasDescobertas,
                                BuscarVendasDescobertasPaginado
                            }
                    );
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string BuscarVendasDescobertas()
        {
            string lRetorno = string.Empty;

            Gradual.Generico.Dados.AcessaDados lDados = null;
            System.Data.DataTable lTable = null;
            System.Data.Common.DbCommand lCommand = null;

            try
            {
                int? lCodigoCliente = this.CodigoCliente;
                System.String lCodigoInstrumento = this.CodigoInstrumento;
                List<String> lMercado = new List<string>();

                if (!String.IsNullOrEmpty(this.Mercados))
                {
                    lMercado = this.Mercados.Split(';').ToList();
                }

                lDados = new Generico.Dados.AcessaDados();
                lDados.ConnectionStringName = "GradualSpider";

                lCommand = lDados.CreateCommand(System.Data.CommandType.StoredProcedure, "prc_vendasdescobertas_buscar");

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

                //if (String.IsNullOrEmpty(lMercado) || lMercado.Equals("Todos"))
                //{
                //    lDados.AddInParameter(lCommand, "@Mercado", System.Data.DbType.String, null);
                //}
                //else
                //{
                //    lDados.AddInParameter(lCommand, "@Mercado", System.Data.DbType.String, lMercado);
                //}

                lDados.AddInParameter(lCommand, "@Mercado", System.Data.DbType.String, null);

                lTable = lDados.ExecuteDbDataTable(lCommand);

                List<Objects.VendaDescoberta> lListaVendas = new List<Objects.VendaDescoberta>();
                
                foreach (System.Data.DataRow dr in lTable.Rows)
                {
                    if (lMercado.Count.Equals(0) || lMercado.Contains(dr["TipoMercado"].DBToString()))
                    {
                        lListaVendas.Add
                        (
                            new Objects.VendaDescoberta
                            {
                                Account         = dr["Cliente"].DBToInt32(),
                                Ativo           = dr["Ativo"].DBToString(),
                                SegmentoMercado = dr["SegmentoMercado"].DBToString(),
                                Variacao        = dr["Variacao"].DBToString(),
                                UltimoPreco     = dr["UltimoPreco"].DBToString(),
                                PrecoFechamento = dr["PrecoFechamento"].DBToString(),
                                QtdAbertura     = dr["QtdAbertura"].DBToString(),
                                QtdD1           = dr["QtdD1"].DBToString(),
                                QtdD2           = dr["QtdD2"].DBToString(),
                                QtdD3           = dr["QtdD3"].DBToString(),
                                QtdExecC        = dr["QtdExecC"].DBToString(),
                                QtdExecV        = dr["QtdExecV"].DBToString(),
                                NetExec         = dr["NetExec"].DBToInt32(),
                                QtdAbC          = dr["QtdAbC"].DBToString(),
                                QtdAbV          = dr["QtdAbV"].DBToString(),
                                NetAb           = dr["NetAb"].DBToString(),
                                PcMedC          = dr["PcMedC"].DBToString(),
                                PcMedV          = dr["PcMedV"].DBToString(),
                                FinancNet       = dr["FinancNet"].DBToString(),
                                LucroPrej       = dr["LucroPrejuizo"].DBToDecimal(),
                                DtPosicao       = dr["DtPosicao"].DBToString(),
                                DtMovimento     = dr["DtMovimento"].DBToString(),
                                Bolsa           = dr["Bolsa"].DBToString(),
                                TipoMercado     = dr["TipoMercado"].DBToString(),
                                DtVencimento    = dr["DtVencimento"].DBToString(),
                                ExecBroker      = dr["ExecBroker"].DBToString(),
                                QtdTotal        = dr["QtdTotal"].DBToInt32(),
                                QtdDisponivel   = dr["QtdDisponivel"].DBToInt32(),
                                VolCompra       = dr["VolCompra"].DBToString(),
                                VolVenda        = dr["VolVenda"].DBToString(),
                                VolTotal        = dr["VolTotal"].DBToString()
                            }
                        );
                    }
                }

                this.SessionUltimoResultadoDeBuscaVendasDescobertas = lListaVendas;

                TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada();

                lRetornoLista = new TransporteDeListaPaginada(this.SessionUltimoResultadoDeBuscaVendasDescobertas);

                lRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(lRetornoLista);

                lRetornoLista.TotalDeItens = lListaVendas.Count;

                lRetornoLista.PaginaAtual = 1;

                lRetornoLista.TotalDePaginas = (int)Math.Ceiling((double)(lListaVendas.Count() / 30));

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

        private string BuscarVendasDescobertasPaginado()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            bool.TryParse(Request["Nova"], out gNova);

            if (this.SessionUltimoResultadoDeBuscaVendasDescobertas != null && !gNova)
            {
                int lPagina;
                int lRegistrosPorPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    if (int.TryParse(Request["rows"], out lRegistrosPorPagina))
                    {
                        lLista = BuscarPaginaDeResultadosVendasDescobertas(lPagina, lRegistrosPorPagina, this.Request["sidx"], this.Request["sord"]);
                    }

                }
            }
            else
            {
                return BuscarVendasDescobertas();
            }

            lRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        private TransporteDeListaPaginada BuscarPaginaDeResultadosVendasDescobertas(int pPagina, int pRows, string pCampo, string pOrdenacao)
        {
            var lRetorno = new TransporteDeListaPaginada();

            var lLista = new List<Objects.VendaDescoberta>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * pRows);
            lIndiceFinal = (pPagina) * pRows;


            if (!String.IsNullOrEmpty(pCampo) && !String.IsNullOrEmpty(pOrdenacao))
            {
                this.SessionUltimoResultadoDeBuscaVendasDescobertasOrdenada = this.SessionUltimoResultadoDeBuscaVendasDescobertas;
                OrdenarVendasDescobertas(pCampo, pOrdenacao);

                for (int a = lIndiceInicial; a < lIndiceFinal; a++)
                {
                    if (a < this.SessionUltimoResultadoDeBuscaVendasDescobertasOrdenada.Count)
                    {
                        lLista.Add(this.SessionUltimoResultadoDeBuscaVendasDescobertasOrdenada[a]);
                    }
                }
            }
            else
            {
                for (int a = lIndiceInicial; a < lIndiceFinal; a++)
                {
                    if (a < this.SessionUltimoResultadoDeBuscaVendasDescobertas.Count)
                    {
                        lLista.Add(this.SessionUltimoResultadoDeBuscaVendasDescobertas[a]);
                    }
                }
            }



            if (!String.IsNullOrEmpty(pCampo) && !String.IsNullOrEmpty(pOrdenacao))
            {
                
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBuscaVendasDescobertas.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / pRows));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        private void OrdenarVendasDescobertas(string pCampo, string pDirecao)
        {
            List<Objects.VendaDescoberta> lObjeto = this.SessionUltimoResultadoDeBuscaVendasDescobertasOrdenada;

            switch (pCampo)
            {
                case "Account":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.Account - b.Account));
                    else
                        lObjeto.Sort((a, b) => (b.Account - a.Account));
                    break;
                case "Ativo":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => System.String.Compare(a.Ativo, b.Ativo));
                    else
                        lObjeto.Sort((a, b) => System.String.Compare(b.Ativo, a.Ativo));
                    break;
                case "QtdTotal":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.QtdTotal - b.QtdTotal));
                    else
                        lObjeto.Sort((a, b) => (b.QtdTotal - a.QtdTotal));
                    //if ("asc".Equals(pDirecao))
                    //    pObjeto.Sort((a, b) => System.String.Compare(a.Ativo, b.Ativo));
                    //else
                    //    pObjeto.Sort((a, b) => System.String.Compare(b.Ativo, a.Ativo));
                    break;
                case "LucroPrej":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => Decimal.Compare(a.LucroPrej, b.LucroPrej));
                    else
                        lObjeto.Sort((a, b) => Decimal.Compare(b.LucroPrej, a.LucroPrej));
                    break;
                case "QtdDisponivel":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.QtdDisponivel - b.QtdDisponivel));
                    else
                        lObjeto.Sort((a, b) => (b.QtdDisponivel - a.QtdDisponivel));
                    break;
                case "NetExec":
                    if ("asc".Equals(pDirecao))
                        lObjeto.Sort((a, b) => (a.NetExec - b.NetExec));
                    else
                        lObjeto.Sort((a, b) => (b.NetExec - a.NetExec));
                    break;
            }

            this.SessionUltimoResultadoDeBuscaVendasDescobertasOrdenada = lObjeto;
        }
    }
}