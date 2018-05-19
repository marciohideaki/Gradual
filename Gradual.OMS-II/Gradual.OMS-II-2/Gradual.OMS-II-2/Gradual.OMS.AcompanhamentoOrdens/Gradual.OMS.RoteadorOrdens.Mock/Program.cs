using System;
using System.ServiceModel;

namespace Gradual.OMS.RoteadorOrdens.Mock
{
    class Program
    {
        #region Métodos Private
        
        private static void AlterarOrdem(string pInput)
        {
            string[] lInputs = pInput.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (lInputs.Length >= 6)
            {
                string lOrdem, lPapel;
                int lConta, lQuantidade;

                decimal lPreco;

                try
                {
                    lOrdem = lInputs[1];
                    lConta = Convert.ToInt32(lInputs[2]);
                    lPapel = lInputs[3];
                    lPreco = Convert.ToDecimal(lInputs[4]);
                    lQuantidade = Convert.ToInt32(lInputs[5]);

                    ServicoRoteadorOrdens.AdicionarAcompanhamentoDeOrdem(lOrdem, lConta, lPapel, lPreco, lQuantidade);
                }
                catch
                {
                    Console.WriteLine("Erro de conversão! >>  a  [ordem] [conta] [papel] [preço] [quantidade]");
                }
            }
        }

        #endregion

        static void Main(string[] args)
        {
            ServiceHost lServico;

            lServico = new ServiceHost(typeof(ServicoRoteadorOrdens));

            //ServicoOrdens.MensagemInterna += new MensagemInternaDelegate(Servico_MensagemInterna);

            lServico.Open();

            Console.WriteLine("Hoster de teste para Roteador de Ordens.\r\nUtilização:");

            Console.WriteLine("  a  [ordem] [conta] [papel] [preço] [quantidade]     Para alterar uma ordem ");
            Console.WriteLine("  q                                                   Para sair");

            string lInput = "";

            while (lInput != "q")
            {
                lInput = Console.ReadLine();

                if (lInput.StartsWith("a "))
                {
                    AlterarOrdem(lInput);
                }
                else
                {
                    Console.WriteLine("Comando [{0}] não reconhecido; utilize q para sair, a para enviar ordem", lInput);
                }
            }
        }
    }
}
