using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Library.Servicos;
using Gradual.Servicos.Contratos.TesteWCF.Mensagens;
using Gradual.Servicos.Contratos.TesteWCF.Dados;
using Gradual.Servicos.Contratos.TesteWCF;

namespace WindowsFormsApplicationTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServicoHostColecao.Default.CarregarConfig("Default");
            IServicoMensageria _servicoMensageria = Ativador.Get<IServicoMensageria>();

            SalvarMensagemDeTexto(_servicoMensageria);

            ListarMensagensDeTextoResponse lRes = _servicoMensageria.ProcessarMensagem(new ListarMensagensDeTextoRequest()) as ListarMensagensDeTextoResponse;

            //IServicoTesteWcf sunda = Ativador.Get<IServicoTesteWcf>();

            //SalvarMensagemDeTextoResponse resp = sunda.SalvarMensagemDeTexto(new SalvarMensagemDeTextoRequest());


            Application.Run(new Form1());
        }

        private static void SalvarMensagemDeTexto(IServicoMensageria lServicoMensageria)
        {
            SalvarMensagemDeTextoResponse lRes = lServicoMensageria.ProcessarMensagem(new SalvarMensagemDeTextoRequest()
            {
                MensagemDeTexto = new MensagemTextoInfo()
                {
                    CodigoMensagemTexto = Guid.NewGuid().ToString(),
                    TextoDaMensagem = "TEXTO [" + Guid.NewGuid().ToString() + "]"
                }
            }) as SalvarMensagemDeTextoResponse;

            object oRes = lRes;
        }
    }
}
