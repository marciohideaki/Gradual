using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.Threading;
using System.Configuration;
using Gradual.OMS.InvXX.Fundos.Lib.FINANCIAL;
using Gradual.OMS.InvXX.Fundos.DbLib.FINANCIAL;
using System.ServiceModel;

namespace Gradual.OMS.InvXX.Fundos.FINANCIAL
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ImportacaoClientePosicaoServico : IServicoControlavel
    {
        #region Atributos
        private static readonly ILog gLogger             = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private WaitOrTimerCallback ThreadResetFinancial = null;
        private AutoResetEvent lThreadEvent              = new AutoResetEvent(false);
        #endregion

        #region Prorpiedades
        private ServicoStatus _ServicoStatus 
        { 
            set; get; 
        }
        private int IntervaloImportacaoCliente
        {
            get { return int.Parse(ConfigurationManager.AppSettings["intervaloImportacaoCliente"]); }
        }
        private string HorariosImportacaoCliente
        {
            get { return ConfigurationManager.AppSettings["HorariosImportacaoClienteFINANCIAL"].ToString(); }
        }
        private string UsuarioFinancial
        {
            get { return ConfigurationManager.AppSettings["UsuarioFinancial"].ToString(); }
        }
        private string SenhaFinancial
        {
            get { return ConfigurationManager.AppSettings["SenhaFinancial"].ToString(); }
        }
        #endregion

        #region Construtores
        public ImportacaoClientePosicaoServico()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Métodos

        public static string FormatarCpf(string cpf)
        {
            cpf = cpf.Replace("-", "").Replace("/", "").Replace(".", "");
            return Formatar(cpf, "###.###.###-##");
        }

        public static string FormatarCnpj(string cnpj)
        {
            cnpj = cnpj.Replace("-", "").Replace("/", "").Replace(".", "");
            return Formatar(cnpj, "##.###.###/####-##");
        }
        
        public static string Formatar( string valor, string mascara )
        {
            StringBuilder dado = new StringBuilder();
            // remove caracteres nao numericos
            foreach ( char c in valor )
            {
                if ( Char.IsNumber(c) )
                dado.Append(c);
            }
            int indMascara = mascara.Length;
            int indCampo = dado.Length;
            for (; indCampo > 0 && indMascara > 0; )
            {
                if ( mascara[--indMascara] == '#' )
                    indCampo--;
            }
            StringBuilder saida = new StringBuilder();

            for (; indMascara < mascara.Length; indMascara++)
            {
                saida.Append( ( mascara[indMascara] == '#' ) ? dado[indCampo++] : mascara[indMascara] );
            }
            return saida.ToString();
        }

        public void ThreadImportacaoCliente(object sender, bool signed)
        {
            try
            {
                DateTime lNow = DateTime.Now;

                List<string> listHorarios = ListaHorarios(this.HorariosImportacaoCliente);

                if ( listHorarios.Contains(lNow.ToString("HH:mm")))
                {
                    gLogger.Debug("Obtendo relacao de Clientes para serem atualizados na financial ");

                    List<ClienteFinancialInfo> lClienteFinancial = new List<ClienteFinancialInfo>();

                    ImportacaoFinancialDbLib lFinancial = new ImportacaoFinancialDbLib();

                    lClienteFinancial = lFinancial.SelecionaClienteImportacaoFinancial();

                    gLogger.InfoFormat("Foram encontrados [{0}] Clientes para serem atualizados na Financial", lClienteFinancial.Count);

                    CadastroCotista.CadastroCotistaWSSoapClient lServico = new CadastroCotista.CadastroCotistaWSSoapClient();

                    CadastroCotista.ValidateLogin login = new CadastroCotista.ValidateLogin() { Username = UsuarioFinancial, Password = SenhaFinancial };

                    DateTime lDiferencaLoadImportacao = DateTime.MinValue;

                    foreach (ClienteFinancialInfo info in lClienteFinancial)
                    {
                        CadastroCotista.Cotista lResponse = lServico.ExportaPorCpfcnpj(login, info.DsCpfCnpj);

                        if (lResponse != null && lResponse.IdCotista != null)
                        {
                            gLogger.InfoFormat("Importando cliente [{0}] ", info.CodigoCliente);

                            lFinancial.ImportacaoClienteFinancial(info);
                        }
                    }

                    TimeSpan lIntervaloLoadImportacao = DateTime.Now - lDiferencaLoadImportacao;

                    gLogger.InfoFormat("importação concluída! Tempo de importação de Clientes [{0}] ",  lIntervaloLoadImportacao);
                }
                
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadImportacaoCliente:" + ex.Message, ex);
            }
        }

        public void StartImportacaoClienteFinancial(object state)
        {
            try
            {
                gLogger.Info("StartImportacaoClientePosicao - Iniciando Serviço de Aplicacao e Resgate");

                ThreadResetFinancial = new WaitOrTimerCallback(ThreadImportacaoCliente);

                ThreadPool.RegisterWaitForSingleObject(lThreadEvent, ThreadResetFinancial, null, this.IntervaloImportacaoCliente, false);

                //thThreadImportacaoCliente = new Thread(new ThreadStart(ThreadImportacaoCliente));
                //thThreadImportacaoCliente.Name = "ThreadAplicacaoResgate";
                //thThreadImportacaoCliente.Start();

                //thThreadImportacaoPosicao = new Thread(new ThreadStart(ThreadImportacaoPosicao));
                //thThreadImportacaoPosicao.Name = "ThreadImportacaoPosicao";
                //thThreadImportacaoPosicao.Start();

                //thThreadImportacaoCarteira = new Thread(new ThreadStart(ThreadImportacaoCarteiraPosicao));
                //thThreadImportacaoCarteira.Name = "ThreadImportacaoCarteira";
                //thThreadImportacaoCarteira.Start();

                gLogger.Info("*****************************************************************");
                gLogger.Info("***********Processo de inicialização finalizado******************");
                gLogger.Info("*****************************************************************");
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o metodo StartImportacaoClientePosicao.", ex);
            }
        }

        private List<string> ListaHorarios(string pHorarios)
        {
            List<string> lretorno = new List<string>();

            try
            {
                if (pHorarios.Contains(";"))
                {
                    char[] lchars = { ';' };
                    string[] lhr = pHorarios.Split(lchars, StringSplitOptions.RemoveEmptyEntries);

                     foreach ( string hr in lhr) lretorno.Add(hr);
                }
                else if (!string.IsNullOrEmpty(pHorarios))
                {
                    lretorno.Add(pHorarios);
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("ListaHorarios() - ", ex);
            }

            return lretorno;
        }

        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                _ServicoStatus = ServicoStatus.EmExecucao;
                this.StartImportacaoClienteFinancial(null);
            }
            catch (Exception ex)
            {
                gLogger.Error("IniciarServico()  StackTrace - ", ex);
            }
             
        }

        public void PararServico()
        {
            try
            {
                _ServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.Error("PararServico()  StackTrace - ", ex);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }
        #endregion
    }
}
