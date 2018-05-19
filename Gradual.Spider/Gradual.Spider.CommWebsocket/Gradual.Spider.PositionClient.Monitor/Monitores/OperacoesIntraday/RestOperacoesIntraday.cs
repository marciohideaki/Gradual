using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using Gradual.Spider.PositionClient.Monitor.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.WebSocket.Lib;
using Newtonsoft.Json;
using Gradual.Spider.SupervisorRisco.Lib.Dados;
using System.Collections.Concurrent;
using Gradual.Spider.PositionClient.Monitor.Transporte;
using System.Web;



namespace Gradual.Spider.PositionClient.Monitor.Monitores.OperacoesIntraday
{
    /// <summary>
    /// Serviço de REST para Buscar operações Intraday na memória e retornar um json
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestOperacoesIntraday : IServicoRestOperacoesIntraday
    {
        /// <summary>
        /// para Buscar operações Intraday na memória e retornar um json
        /// </summary>
        /// <returns>Retorna um json da</returns>
        public string BuscarOperacoesIntradayJSON(
            int CodigoCliente,
            string CodigoInstrumento,
            bool OpcaoMarketTodosMercados,
            bool OpcaoMarketAVista,
            bool OpcaoMarketFuturos,
            bool OpcaoMarketOpcao,
            bool OpcaoParametroIntradayOfertasPedra,
            bool OpcaoParametroIntradayNetNegativo,
            bool OpcaoParametroIntradayPLNegativo
            
            )
        {
            string lRetorno = string.Empty;

            try
            {
                var lList = new List<PosClientSymbolInfo>();

                var lListTrans = new List<TransporteOperacoesIntraday>();

                var lDic = new ConcurrentDictionary<int, List<PosClientSymbolInfo>>();

                var lPos = PositionClientPackageSocket.Instance.DicPositionClient;

                lock (lPos)
                {
                    foreach (KeyValuePair<int, List<PosClientSymbolInfo>> pos in lPos)
                    {
                        lList.AddRange(pos.Value);
                    }
                }

                var lFiltrado = from a in lList 
                                where ((a.QtdExecC != 0 || a.QtdExecV != 0) || (a.QtdAbC != 0 || a.QtdAbC != 0))
                                select a;

                ///Filtrando Cliente
                if ( CodigoCliente != 0 )
                {
                    lFiltrado = from a in lFiltrado where a.Account == CodigoCliente select a;
                }

                ///Filtrando Papel
                if (!string.IsNullOrEmpty(CodigoInstrumento))
                {
                    lFiltrado = from a in lFiltrado where a.Ativo == CodigoInstrumento select a;
                }
                
                if (!OpcaoMarketTodosMercados)
                {
                    var lTiposMercado = new List<string> { "vis", "fut", "opf", "dis", "opc", "opv" };
                    
                    if (!OpcaoMarketAVista)
                    {
                        lTiposMercado.Remove("vis");
                    }

                    if (!OpcaoMarketFuturos)
                    {
                        lTiposMercado.Remove("fut");
                        lTiposMercado.Remove("opf");
                        lTiposMercado.Remove("dis");
                    }

                    if (!OpcaoMarketOpcao)
                    {
                        lTiposMercado.Remove("opc");
                        lTiposMercado.Remove("opv");
                    }

                    if (OpcaoMarketAVista || OpcaoMarketFuturos || OpcaoMarketOpcao)
                    {
                        lFiltrado = from a in lFiltrado where lTiposMercado.Contains(a.TipoMercado.ToLower()) select a;
                    }
                }
                
                //Opção de Parametros Intraday
                if (OpcaoParametroIntradayNetNegativo)
                {
                    lFiltrado = from a in lFiltrado where a.NetExec < 0 select a;
                }

                if (OpcaoParametroIntradayOfertasPedra)
                {
                    lFiltrado = from a in lFiltrado where a.NetAb > 0 select a;
                }

                if (OpcaoParametroIntradayPLNegativo)
                {
                    lFiltrado = from a in lFiltrado where a.LucroPrej < 0 select a;
                }
                
                var lTrans = new TransporteOperacoesIntraday(lFiltrado.ToList());

                
                lRetorno = JsonConvert.SerializeObject(lTrans.ListaTransporte);
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return lRetorno;
        }

        
    }
}
