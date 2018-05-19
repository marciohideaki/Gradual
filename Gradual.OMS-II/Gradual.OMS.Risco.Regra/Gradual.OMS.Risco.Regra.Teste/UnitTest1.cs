using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Gradual.OMS.Risco.Regra;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.OMS.Risco.Regra.Teste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //this.ListarGrupoIntens();
            ServicoRegrasRisco lServico = new ServicoRegrasRisco();
            
            //ListarPermissoesRiscoClienteResponse lResponse = lServico.ListarPermissoesRiscoClienteSpider(new ListarPermissoesRiscoClienteRequest(){CodigoCliente = 24905 });

            this.TirarFFracionario("OIBR4F");
        }

        private string TirarFFracionario(string pPapel)
        {
            string lRetorno = string.Empty;

            if (pPapel.Substring(pPapel.Length - 1, 1).ToLower() == "f")
            {
                lRetorno = pPapel.Substring(0, pPapel.Length - 1);
            }

            return lRetorno;
        }

        private void SalvarParametroAlavancagem()
        {
            var lRetorno = new ServicoRegrasRisco().SalvarParametroAlavancagem(new SalvarParametroAlavancagemRequest()
            {
                Objeto = new ParametroAlavancagemInfo() 
                {
                     IdGrupo = 83,
                      PercentualContaCorrente = 2
                      , StCarteiraOpcao = 'S'
                      , StCarteiraGarantiaPrazo = 'N'
                      , PercentualCustodia = 1
                      , PercentualAlavancagemVendaOpcao = 3
                      , PercentualAlavancagemVendaAVista =4
                      , PercentualAlavancagemCompraOpcao =5
                      , PercentualAlavancagemCompraAVista = 6
                }
            });
        }

        private void SalvarGrupoItem()
        {
            var lGrupoItemLista = new List<GrupoItemInfo>();

            lGrupoItemLista.Add(new GrupoItemInfo() { CodigoGrupo = 80, NomeGrupoItem = "Mercearia" });
            lGrupoItemLista.Add(new GrupoItemInfo() { CodigoGrupo = 80, NomeGrupoItem = "Maçonaria" });
            lGrupoItemLista.Add(new GrupoItemInfo() { CodigoGrupo = 80, NomeGrupoItem = "Gelateria" });
            lGrupoItemLista.Add(new GrupoItemInfo() { CodigoGrupo = 80, NomeGrupoItem = "Maltoeria" });


            var lRetorno = new ServicoRegrasRisco().SalvarGrupoItem(new SalvarGrupoItemRequest() { GrupoItemLista = lGrupoItemLista });
        }

        private void SalvarGrupo()
        {
            var lRetorno = new ServicoRegrasRisco().SalvarGrupo(new SalvarGrupoRequest() 
            { 
                Grupo = new GrupoInfo() 
                { 
                    NomeDoGrupo = "Teste Dev 001", TipoGrupo = EnumRiscoRegra.TipoGrupo.GrupoAlavancagem 
                }});
        }

        private void ListarGrupoIntens()
        {
            var lRetorno = new ServicoRegrasRisco().ListarMonitoramentoDeRisco(
                new ListarMonitoramentoRiscoRequest());

            if (lRetorno.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {

            }
        }
    }
}
