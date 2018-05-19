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
using Gradual.Spider.PositionClient.Monitor.Lib.Message;

namespace Gradual.Spider.PositionClient.Monitor.Monitores.RiscoResumido
{
    /// <summary>
    /// Serviço de REST para Buscar operações Intraday na memória e retornar um json
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestRiscoResumido : IServicoRestRiscoResumido
    {
        /// <summary>
        /// para Buscar Risco Resumido na memória e retornar um json
        /// </summary>
        /// <param name="CodigoCliente"></param>
        /// <param name="OpcaoPLSomenteComLucro"></param>
        /// <param name="OpcaoPLSomentePLnegativo"></param>
        /// <param name="OpcaoSFPAtingidoAte25"></param>
        /// <param name="OpcaoSFPAtingidoEntre25e50"></param>
        /// <param name="OpcaoSFPAtingidoEntre50e75"></param>
        /// <param name="OpcaoSFPAtingidoAcima75"></param>
        /// <param name="OpcaoPrejuizoAtingidoAte2K"></param>
        /// <param name="OpcaoPrejuizoAtingidoEntre2Ke5K"></param>
        /// <param name="OpcaoPrejuizoAtingidoEntre5Ke10K"></param>
        /// <param name="OpcaoPrejuizoAtingidoEntre10Ke20K"></param>
        /// <param name="OpcaoPrejuizoAtingidoEntre20Ke50K"></param>
        /// <param name="OpcaoPrejuizoAtingidoAcima50K"></param>
        /// <returns></returns>
        public string BuscarRiscoResumidoJSON(string pRequestJson)
        {
            string lRetorno = string.Empty;

            try
            {
                
                var lList = new List<ConsolidatedRiskInfo>();

                var lDic = new ConcurrentDictionary<int, ConsolidatedRiskInfo>();

                lList.AddRange(PositionClientSocketRiscoResumido.Instance.DicConsolidatedRisk.Values);

                var pRequest = JsonConvert.DeserializeObject(pRequestJson, typeof(BuscarRiscoResumidoRESTRequest)) as BuscarRiscoResumidoRESTRequest;

                var lFiltrado = from a in lList select a;

                if (pRequest.ListaClientes.Count() > 0)
                {
                    lFiltrado = from a in lFiltrado where pRequest.ListaClientes.Contains(a.Account) select a;
                }

                //Opção de PL negativo ou com lucro
                if (pRequest.OpcaoPLSomenteComLucro)
                {
                    lFiltrado = from a in lFiltrado where a.PLTotal > 0 select a;
                }
                else if (pRequest.OpcaoPLSomentePLnegativo)
                {
                    lFiltrado = from a in lFiltrado where a.PLTotal < 0 select a;
                }

                //Opção de SFP Atingido
                if (pRequest.OpcaoSFPAtingidoAte25)
                {
                    lFiltrado = from a in lFiltrado where (a.TotalPercentualAtingido > (-25) && a.TotalPercentualAtingido < 0) select a;
                }

                if (pRequest.OpcaoSFPAtingidoEntre25e50)
                {
                    lFiltrado = from a in lFiltrado where (a.TotalPercentualAtingido < (-25) && a.TotalPercentualAtingido > (-50)) select a;
                }

                if (pRequest.OpcaoSFPAtingidoEntre50e75)
                {
                    lFiltrado = from a in lFiltrado where (a.TotalPercentualAtingido < (-50) && a.TotalPercentualAtingido > (-75)) select a;
                }

                if (pRequest.OpcaoSFPAtingidoAcima75)
                {
                    lFiltrado = from a in lFiltrado where (a.TotalPercentualAtingido < (-75)) select a;
                }

                //Prejuízo Atingido
                if (pRequest.OpcaoPrejuizoAtingidoAte2K)
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal >= (-2000) && a.PLTotal < 0) select a;
                }

                if (pRequest.OpcaoPrejuizoAtingidoEntre2Ke5K)
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal <= (-2000) && a.PLTotal >= (-5000)) select a;
                }

                if (pRequest.OpcaoPrejuizoAtingidoEntre5Ke10K)
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal <= (-5000) && a.PLTotal >= (-10000)) select a;
                }

                if (pRequest.OpcaoPrejuizoAtingidoEntre10Ke20K)
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal <= (-10000) && a.PLTotal >= (-20000)) select a;
                }

                if (pRequest.OpcaoPrejuizoAtingidoEntre20Ke50K)
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal <= (-20000) && a.PLTotal >= (-50000)) select a;
                }

                if (pRequest.OpcaoPrejuizoAtingidoAcima50K)
                {
                    lFiltrado = from a in lFiltrado where (a.PLTotal <= (-50000)) select a;
                }

                var lTrans = new TransporteRiscoResumido(lFiltrado.ToList());

                lRetorno = JsonConvert.SerializeObject(lTrans.ListaTransporte);

                //_Logger.InfoFormat("Foram Encontrados {0} itens de Risco Resumido", lRetorno.ListRiscoResumido.Count);
            }
            catch (Exception ex)
            {
                //gLogger.
            }

            return lRetorno;
        }
    }
}
