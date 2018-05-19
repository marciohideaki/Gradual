using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Sistemas.CadastroPapeis;
using Gradual.OMS.Contratos.CadastroPapeis.Mensagens;
using Gradual.OMS.Contratos.CadastroPapeis.Dados;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.CadastroPapeis;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.Configuration;

namespace Gradual.OMS.Teste.CadastroPapeis
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

                ConsultarPapelNegociadoResponse lReponse = lPapel.ConsultarPapelNegociado(new ConsultarPapelNegociadoRequest()
                    {
                        LstAtivos = new List<string>() { "PETR4" },
                        //DataVencimento = Convert.ToDateTime("15/12/2010"),
                        //TipoMercado = 70

                    });

                if (lReponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lReponse.LstPapelBovespaInfo.ForEach(delegate(PapelNegociadoBovespaInfo item)
                    {
                        Console.WriteLine(item.GetType());
                    });

                    lReponse.LstPapelBmfInfo.ForEach(delegate(PapelNegociadoBmfInfo item)
                    {
                        Console.WriteLine(item.GetType());
                    });
                }

                //var lPapel2 = new ServicoCadastroPapeis();

                //ConsultarPapelNegociadoResponse lReponse2 = lPapel2.ConsultarPapelNegociado(new ConsultarPapelNegociadoRequest()
                //{
                //    LstAtivos = new List<string>() { "PETR4", "ASSIM" }
                //});

                //if (lReponse2.StatusResposta == MensagemResponseStatusEnum.OK)
                //{ 
                //}

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        
    }
}
