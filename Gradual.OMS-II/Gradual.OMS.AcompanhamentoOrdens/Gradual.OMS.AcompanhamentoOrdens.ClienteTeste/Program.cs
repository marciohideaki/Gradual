using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Gradual.OMS.AcompanhamentoOrdensLib.Info;
using Gradual.OMS.AcompanhamentoOrdensLib.Mensageria;

namespace Gradual.OMS.AcompanhamentoOrdens.ClienteTeste
{
    class Program
    {
        #region Métodos Private

        private static void ConsoleWriteOrdens(OrdemInfo[] pOrdens)
        {
            if (pOrdens.Length > 0)
            {
                foreach (OrdemInfo lInfo in pOrdens)
                {
                    Console.WriteLine("Ordem: [{0}], [{2}] [{1}] [{3}] [{4}]"
                                        , lInfo.ClOrdID
                                        , lInfo.Symbol
                                        , lInfo.OrderQty
                                        , lInfo.Price
                                        , lInfo.OrdStatus);

                    foreach (AcompanhamentoOrdemInfo lAcompanhamento in lInfo.Acompanhamentos)
                    {
                        Console.WriteLine("\tAcompanhamento: [{0}], Qtd Exec: [{1}] [{2}]"
                                            , lAcompanhamento.CodigoTransacao
                                            , lAcompanhamento.QuantidadeExecutada
                                            , lAcompanhamento.StatusOrdem);
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhuma ordem encontrada");
            }
        }

        private static void RealizarLogin(string pInput)
        {
            string[] lInputs = pInput.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (lInputs.Length >= 2)
            {
                InformarLoginDoClienteRequest  lRequest = new InformarLoginDoClienteRequest();
                InformarLoginDoClienteResponse lResponse;

                ServicoAcompanhamentoOrdensClient lClient = new ServicoAcompanhamentoOrdensClient();

                lRequest.ContaDoCliente = Convert.ToInt32( lInputs[1]);

                Console.WriteLine("Enviando requisição...");

                lResponse = lClient.InformarLoginDoCliente(lRequest);

                Console.WriteLine("Resposta do Serviço: [{0}]", lResponse.StatusResposta);
            }
        }
        
        private static void RealizarLogout(string pInput)
        {
            string[] lInputs = pInput.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (lInputs.Length >= 2)
            {
                InformarLogoutDoClienteRequest  lRequest = new InformarLogoutDoClienteRequest();
                InformarLogoutDoClienteResponse lResponse;

                ServicoAcompanhamentoOrdensClient lClient = new ServicoAcompanhamentoOrdensClient();

                lRequest.ContaDoCliente = Convert.ToInt32(lInputs[1]);
                
                Console.WriteLine("Enviando requisição...");

                lResponse = lClient.InformarLogoutDoCliente(lRequest);

                Console.WriteLine("Resposta do Serviço: [{0}]", lResponse.StatusResposta);
            }
        }
        
        private static void VerificarAcompanhamento(string pInput)
        {
            string[] lInputs = pInput.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (lInputs.Length >= 2)
            {
                VerificarStatusDasOrdensRequest  lRequest = new VerificarStatusDasOrdensRequest();
                VerificarStatusDasOrdensResponse lResponse;

                ServicoAcompanhamentoOrdensClient lClient = new ServicoAcompanhamentoOrdensClient();

                lRequest.ContaDoCliente = Convert.ToInt32(lInputs[1]);

                Console.WriteLine("Enviando requisição...");

                lResponse = lClient.VerificarStatusDasOrdens(lRequest);

                Console.WriteLine("Resposta do Serviço: [{0}]", lResponse.StatusResposta);

                ConsoleWriteOrdens(lResponse.Ordens);
            }
        }
        
        private static void BuscarOrdens(string pInput)
        {
            string[] lInputs = pInput.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            int lConta = -1;
            DateTime lDataDe = DateTime.MinValue, lDataAte = DateTime.MinValue;

            string lParamA = null, lParamB = null, lParamC = null;

            CultureInfo lProv = new CultureInfo("pt-BR");

            if (lInputs.Length == 2)
            {
                lParamA = lInputs[1];
            }

            if (lInputs.Length == 3)
            {
                //passador dois parametros, vê se o primeiro é int
                if (int.TryParse(lInputs[1], out lConta))
                {
                    lParamA = lInputs[1];
                    lParamB = lInputs[2];
                }
                else
                {
                    lParamA = null;
                    lParamB = lInputs[1];
                    lParamC = lInputs[2];
                }
            }

            if (!string.IsNullOrEmpty(lParamA))
            {
                if (!int.TryParse(lParamA, out lConta))
                {
                    lConta = -1;
                }
            }
            
            if (!string.IsNullOrEmpty(lParamB))
            {

                if(!DateTime.TryParseExact(lParamB, "dd/MM/yyyy", lProv, DateTimeStyles.None, out lDataDe))
                {
                    if(!DateTime.TryParseExact(lParamB, "dd/MM", lProv, DateTimeStyles.None, out lDataDe))
                    {
                        if(!DateTime.TryParseExact(lParamB, "dd/MM/yyyy HH:mm", lProv, DateTimeStyles.None, out lDataDe))
                        {
                            if(!DateTime.TryParseExact(lParamB, "dd/MM/yyyy HH:mm:ss", lProv, DateTimeStyles.None, out lDataDe))
                            {
                                //não entendeu a data...
                            }
                        }
                    }
                }
            }
            
            if (string.IsNullOrEmpty(lParamC))
            {
                if(lDataDe != DateTime.MinValue)
                    lDataAte = DateTime.Now;
            }
            else
            {
                if(!DateTime.TryParseExact(lParamC, "dd/MM/yyyy", lProv, DateTimeStyles.None, out lDataAte))
                {
                    if(!DateTime.TryParseExact(lParamC, "dd/MM", lProv, DateTimeStyles.None, out lDataAte))
                    {
                        if(!DateTime.TryParseExact(lParamC, "dd/MM/yyyy HH:mm", lProv, DateTimeStyles.None, out lDataAte))
                        {
                            if(!DateTime.TryParseExact(lParamC, "dd/MM/yyyy HH:mm:ss", lProv, DateTimeStyles.None, out lDataAte))
                            {
                                    //não entendeu a data...
                            }
                        }
                    }
                }
            }

            
            BuscarOrdensRequest  lRequest = new BuscarOrdensRequest();
            BuscarOrdensResponse lResponse;

            ServicoAcompanhamentoOrdensClient lClient = new ServicoAcompanhamentoOrdensClient();

            if(lConta > 0)
                lRequest.ContaDoCliente = lConta;

            if(lDataDe != DateTime.MinValue)
            {
                lRequest.DataDe = lDataDe;
                lRequest.DataAte = lDataAte;
            }
            else
            {
                lRequest.DataDe  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1,  0,  0,  0);
                lRequest.DataAte = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 23, 59, 59);
            }

            Console.WriteLine("Enviando busca: conta [{0}] De [{1}] Até [{2}]...", lRequest.ContaDoCliente, lRequest.DataDe, lRequest.DataAte);

            lResponse = lClient.BuscarOrdens(lRequest);

            if (lResponse.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {
                ConsoleWriteOrdens(lResponse.Ordens);
            }
            else
            {
                Console.WriteLine("Erro na consulta: [{0}] [{1}]", lResponse.StatusResposta, lResponse.DescricaoResposta);
            }
        }

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Cliente de teste para Acompanhamento de Ordens.\r\nUtilização:");

            Console.WriteLine("  in  [conta]                  Para efetuar login do usuário");
            Console.WriteLine("  out [conta]                  Para efetuar logout do usuário");
            Console.WriteLine("  a   [conta]                  Para ver o acompanhamento das ordens do usuário");
            Console.WriteLine("  b   [conta] [data de] [até]  Para buscar ordens do usuário");
            Console.WriteLine("  q                            Para sair");

            string lInput = "";

            while (lInput != "q")
            {
                lInput = Console.ReadLine();

                if (lInput.StartsWith("in "))
                {
                    RealizarLogin(lInput);
                }
                else if (lInput.StartsWith("out "))
                {
                    RealizarLogout(lInput);
                }
                else if (lInput.StartsWith("a "))
                {
                    VerificarAcompanhamento(lInput);
                }
                else if (lInput.StartsWith("b"))
                {
                    BuscarOrdens(lInput);
                }
                else
                {
                    Console.WriteLine("Comando [{0}] não reconhecido", lInput);
                }
            }
        }
    }
}
