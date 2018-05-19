using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using System.Configuration;
using Gradual.OMS.CadastroPapeis.Lib;
using Gradual.OMS.Library;

namespace Gradual.OMS.CadastroPapeis.Teste
{
    class Program
    {
        static void Main(string[] args)
        {


            //ServicoHostColecao.Default.CarregarConfig("Default");
            //IServicoMensageria _servicoMensageria = Ativador.Get<IServicoMensageria>();

            //ConsultarPapelNegociadoResponse lReponse = _servicoMensageria.ProcessarMensagem(new ConsultarPapelNegociadoRequest()
            //{
            //    //LstAtivos = new List<string>() { "PETR4", "ASSIM" }
            //    DataVencimento = new DateTime(2010, 4, 19)
            //}) as ConsultarPapelNegociadoResponse;


            var lPapel = new ServicoCadastroPapeis();
            try
            {
                IServicoCadastroPapeis lServico = Ativador.Get<IServicoCadastroPapeis>();//new ServicoCadastroPapeis();
                //Ativador.Get<IServicoCadastroPapeis>();

                
                //ConsultarPapelNegociadoResponse lResponse = lServico.ConsultarPapelNegociado(new ConsultarPapelNegociadoRequest()
                //{
                //    LstAtivos = new List<string>() { "PETR4F" },
                //    DataVencimento = null,
                //    TipoMercado = 0
                //}) as ConsultarPapelNegociadoResponse;

                ListarOpcoesDoPapelResponse lResponse = lServico.ListarOpcoesDoPapel(new ListarOpcoesDoPapelRequest() 
                {
                    Ativo = "PETR4",
                    TipoMercado = "OPC"
                });
                
                /*
                ListarOpcoesDoPapelResponse lResponse;
                ListarOpcoesDoPapelRequest lRequest = new ListarOpcoesDoPapelRequest() { Ativo = "PETR4" };



                lResponse = lServico.ConsultarPapelNegociado(lRequest);
                */

                //ConsultarSePapelExisteRequest r = new ConsultarSePapelExisteRequest() { Ativo = "petr4" };

                //ConsultarSePapelExisteResponse p = lServico.ConsultarPapelNegociado(r);
                

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
