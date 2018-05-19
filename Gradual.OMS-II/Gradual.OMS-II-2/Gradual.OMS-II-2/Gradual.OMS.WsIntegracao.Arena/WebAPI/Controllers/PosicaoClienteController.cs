using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gradual.OMS.Monitor.Custodia.Lib.Info;
using Gradual.OMS.WsIntegracao.Arena.Models;
using Gradual.OMS.WsIntegracao.Arena.Services;
using Gradual.OMS.WsIntegracao.Arena.Transporte;
using WebAPI_Test.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Controllers
{
    public class PosicaoClienteController : ApiController
    {
        private MonitorCustodiaServico gServicoCustodia = new MonitorCustodiaServico();
        private ContaCorrenteServico gServicoContaCorrente = new ContaCorrenteServico();
        private ClienteContaServico gSericoClienteConta = new ClienteContaServico();  

        // GET api_teste/posicao
        
        [ApiDocumentation("Retorna todas as posições")]
        public IEnumerable<PosicaoCliente> Get()
        {
            List<PosicaoCliente> lListaRetornoPosicao = new List<PosicaoCliente>();

            try
            {
                var lListaClientes = MonitorCustodiaServico.ListarClientesComCustodiaCC();


                for(int i= 0; i < lListaClientes.Count; i++ )
                {
                    int lCodigoBovespa = lListaClientes[i];

                    var lContaCorrente = gServicoContaCorrente.GetSaldoContaCorrenteCliente(lCodigoBovespa);

                    var lCustodia = new MonitorCustodiaInfo();
                    
                    lCustodia = gServicoCustodia.GetPosicaoCustodiaCliente(lCodigoBovespa);

                    var lPosicao = new PosicaoCliente();
                    lPosicao.CodigoBovespaCliente        = lCustodia.CodigoClienteBov.HasValue ? lCustodia.CodigoClienteBov.Value : 0;
                    lPosicao.CodigoBmfCliente            = lCustodia.CodigoClienteBmf.HasValue ? lCustodia.CodigoClienteBmf.Value : 0;
                    lPosicao.SaldoFinanceiro             = TransporteFinanceiro.TraduzirCustodiaInfo(lContaCorrente);
                    lPosicao.SaldoCustodiaBovespaCliente = TransporteCustodia.TraduzirCustodiaInfo(lCustodia.ListaCustodia);
                    lPosicao.SaldoCustodiaBmfCliente     = TransporteCustodia.TraduzirCustodiaInfo(lCustodia.ListaPosicaoDiaBMF,lCustodia.ListaCustodia);

                    lListaRetornoPosicao.Add(lPosicao);
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lListaRetornoPosicao; // new string[] { "value1", "value2" };
        }
       
        // GET api_teste/posicao/5
        public PosicaoCliente Get(int id)
        {
            var lRetornoPosicao = new PosicaoCliente();

            try
            {
                Cliente lCliente = ClienteContaServico.ConsultarClienteConta(id);

                int lCodigoBovespa = lCliente.CodigoBovespa;
            
                var lCustodia  = gServicoCustodia.GetPosicaoCustodiaCliente(lCodigoBovespa);

                if (lCustodia.CodigoClienteBov.HasValue)
                {
                    lRetornoPosicao.CodigoBovespaCliente = lCustodia.CodigoClienteBov.Value;
                    lCodigoBovespa                       = lCustodia.CodigoClienteBov.Value;
                }

                if (lCustodia.CodigoClienteBmf.HasValue)
                {
                    lRetornoPosicao.CodigoBmfCliente = lCustodia.CodigoClienteBmf.Value;
                }

                var lContaCorrente = gServicoContaCorrente.GetSaldoContaCorrenteCliente(lCodigoBovespa);

                lRetornoPosicao.SaldoFinanceiro = TransporteFinanceiro.TraduzirCustodiaInfo(lContaCorrente);

                lRetornoPosicao.SaldoCustodiaBovespaCliente = TransporteCustodia.TraduzirCustodiaInfo(lCustodia.ListaCustodia);

                lRetornoPosicao.SaldoCustodiaBmfCliente = TransporteCustodia.TraduzirCustodiaInfo(lCustodia.ListaPosicaoDiaBMF, lCustodia.ListaCustodia);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetornoPosicao;
        }

        // POST api_teste/posicao
        public void Post([FromBody]string value)
        {
        }

        // PUT api_teste/posicao/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api_teste/posicao/5
        public void Delete(int id)
        {
        }
    }

    public class ClientesComparer : IEqualityComparer<MonitorCustodiaInfo.CustodiaPosicao>
    {
        public bool Equals(MonitorCustodiaInfo.CustodiaPosicao x, MonitorCustodiaInfo.CustodiaPosicao y)
        {
            if (x.IdCliente == y.IdCliente)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(MonitorCustodiaInfo.CustodiaPosicao obj )
        {
            return obj.IdCliente.GetHashCode();
        }
    }
}
