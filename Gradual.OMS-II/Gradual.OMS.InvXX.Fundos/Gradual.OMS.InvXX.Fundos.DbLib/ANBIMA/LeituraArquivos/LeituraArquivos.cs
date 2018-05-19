using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info;
using System.Globalization;
using log4net;
using System.IO;
using System.Configuration;
using Gradual.OMS.InvXX.Fundos.Lib.ANBIMA;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos
{
    public class LeituraArquivos
    {
        #region Atributos
        private Dictionary<string, string> ListaArquivos = new Dictionary<string, string>();
        private CultureInfo gCultura = new CultureInfo("pt-BR");
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string gPath = ConfigurationManager.AppSettings["ArquivosAnbima"];
        #endregion

        #region Construtores
        public LeituraArquivos()
        {
            ListaArquivos.Add("FundosMovCota", "anmovcot");
            ListaArquivos.Add("FundosDia"    , "anvldia");
            ListaArquivos.Add("FundosMes"    , "anvlmes");
            //ListaArquivos.Add("Instituicoes", "cbinstit.txt");
            //ListaArquivos.Add("FundosTipo", "");
            ListaArquivos.Add("Fundos", "cbfundo");
            ListaArquivos.Add("TaxaAdm", "antaxadm");
            //ListaArquivos.Add("FundosStatus", "anstatus.txt");
            //ListaArquivos.Add("NotaExpl", "");
            //ListaArquivos.Add("FundosNE", "");
            //ListaArquivos.Add("FundosDia1", "");
            //ListaArquivos.Add("FundosConsDia", "ancondia.txt");
            //ListaArquivos.Add("FundosConsMes", "anconmes.txt");
            //ListaArquivos.Add("PLInst", "");
            //ListaArquivos.Add("AplicAgregada", "");
            //ListaArquivos.Add("Especie", "");
            //ListaArquivos.Add("Indicadores", "");
            ListaArquivos.Add("IndicMes", "anvalind");
            //ListaArquivos.Add("Queries", "");
            //ListaArquivos.Add("ControleTransf", "");
            //ListaArquivos.Add("Xanvaldia", "");
            //ListaArquivos.Add("XFundosDia", "");
        }
        #endregion

        #region Métodos
        private string GetNomeCorretoArquivo(string pInicialArquivo)
        {
            string lRetorno = string.Empty;

            try
            {
                DateTime lDtUil = new ImportacaoDbLib().SelecionaUltimoPregao();

                string[] filePaths = Directory.GetFiles(gPath, pInicialArquivo+"*");

                foreach (string arquivo in filePaths)
                {
                    string[] lNomeSplitado = arquivo.Split('_');

                    int lAno = Convert.ToInt32(lNomeSplitado[2].Substring(0, 4));

                    int lMes = Convert.ToInt32(lNomeSplitado[2].Substring(4, 2));

                    int lDia = Convert.ToInt32(lNomeSplitado[2].Substring(6, 2));

                    DateTime lDtArquivo = new DateTime(lAno, lMes, lDia);

                    if (lDtArquivo == lDtUil || arquivo.Contains("anvalind"))
                    {
                        lRetorno = arquivo;
                    }
                }

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método GetNomeCorretoArquivo - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }
        public List<FundosDiaInfo> LeArquivoFundosDia()
        {
            List<FundosDiaInfo> lRetorno = new List<FundosDiaInfo>();
            try
            {
                string lArquivo = GetNomeCorretoArquivo(ListaArquivos["FundosDia"]);
                System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo );
                string lContent              = lFile.ReadToEnd();
                lFile.Close();

                string[] Linhas = lContent.Split('\r');

                foreach (string linha in Linhas)
                {
                    gLogger.InfoFormat("Linha de Fundos Dia encontrada [{0}].", linha);
                    lRetorno.Add(RetornaFundosDia(linha));
                }

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LeArquivoFundosDia - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<FundosMesInfo> LeArquivoFundosMes()
        {
            List<FundosMesInfo> lRetorno = new List<FundosMesInfo>();
            try
            {
                string lArquivo =GetNomeCorretoArquivo(ListaArquivos["FundosMes"]);
                System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo);
                string lContent              = lFile.ReadToEnd();
                lFile.Close();

                string[] Linhas = lContent.Split('\r');

                foreach (string linha in Linhas)
                {
                    gLogger.InfoFormat("Linha de Fundos Mes encontrada [{0}].", linha);
                    lRetorno.Add(RetornaFundosMes(linha));
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LeArquivoFundosMes - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<FundosMovCotaInfo> LeArquivoFundosMovCota()
        {
            List<FundosMovCotaInfo> lRetorno = new List<FundosMovCotaInfo>();

            try
            {
                string lArquivo =GetNomeCorretoArquivo(ListaArquivos["FundosMovCota"]);
                System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo);
                string lContent              = lFile.ReadToEnd();
                lFile.Close();

                string[] Linhas = lContent.Split('\r');

                foreach (string linha in Linhas)
                {
                    gLogger.InfoFormat("Linha de Mov Cota encontrada [{0}].", linha);

                    lRetorno.Add(RetornaFundosMovCotaInfo(linha));
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LeArquivoFundosMovCota - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<TaxaAdministracaoInfo> LeArquivoFundosTaxaAdm()
        {
            List<TaxaAdministracaoInfo> lRetorno = new List<TaxaAdministracaoInfo>();

            try
            {
                string lArquivo = GetNomeCorretoArquivo(ListaArquivos["TaxaAdm"]);
                System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo);
                string lContent = lFile.ReadToEnd();
                lFile.Close();

                string[] Linhas = lContent.Split('\r');

                foreach (string linha in Linhas)
                {
                    gLogger.InfoFormat("Linha de Taxa de Administracao encontrada [{0}].", linha);

                    lRetorno.Add(RetornaFundosTaxaAdm(linha));
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LeArquivoFundosTaxaAdm - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<ANBIMAIndicadoresMesInfo> LerArquivoIndcadoresMes()
        {
            List<ANBIMAIndicadoresMesInfo> lRetorno = new List<ANBIMAIndicadoresMesInfo>();

            try
            {
                string[] filePaths = Directory.GetFiles(gPath, "anvalind" + "*");

                foreach (string arquivo in filePaths)
                {

                    string lArquivo              = arquivo;
                    System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo);
                    string lContent              = lFile.ReadToEnd();
                    lFile.Close();

                    string[] Linhas = lContent.Split('\r');

                    foreach (string linha in Linhas)
                    {
                        gLogger.InfoFormat("Linha de Indicadores Mês encontrada [{0}].", linha);

                        lRetorno.Add(RetornaIndcadoresMes(linha));
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LerArquivoIndcadoresMes - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        private ANBIMAIndicadoresMesInfo RetornaIndcadoresMes(string linha)
        {
            ANBIMAIndicadoresMesInfo lIndicadoresMes = new ANBIMAIndicadoresMesInfo();
            try
            {
                string[] lIndMesString = linha.Split('\t');

                lIndicadoresMes.CodigoIndicador      = Convert.ToInt32( lIndMesString[0].Replace("\n", ""));
                lIndicadoresMes.Mes                  = Convert.ToInt32(lIndMesString[1], gCultura);
                lIndicadoresMes.Ano                  = Convert.ToInt32(lIndMesString[2],gCultura);
                lIndicadoresMes.Volume               = Convert.ToDecimal(lIndMesString[3] == "" ? "0" : lIndMesString[3], gCultura);
                lIndicadoresMes.Taxa                 = Convert.ToDecimal(lIndMesString[4] == "" ? "0" : lIndMesString[4], gCultura);
                lIndicadoresMes.Indice               = Convert.ToDecimal(lIndMesString[5] == "" ? "0" : lIndMesString[5], gCultura);
                lIndicadoresMes.Quantidade           = Convert.ToDecimal(lIndMesString[6] == "" ? "0" : lIndMesString[6], gCultura);
                lIndicadoresMes.DataHora             = DateTime.Now;

            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método RetornaIndcadoresMes - ", ex);
            }

            return lIndicadoresMes;
        }

        private TaxaAdministracaoInfo RetornaFundosTaxaAdm(string linha)
        {
            TaxaAdministracaoInfo lTaxaAdm = new TaxaAdministracaoInfo();
            try
            {
                string[] lTaxaAdmString = linha.Split('\t');

                lTaxaAdm.CodigoFundo                   = lTaxaAdmString[0].Replace("\n", "");
                lTaxaAdm.DataInicio                    = Convert.ToDateTime(lTaxaAdmString[1], gCultura);
                lTaxaAdm.TaxaFixa                      = Convert.ToDouble(lTaxaAdmString[2] == "" ? "0" : lTaxaAdmString[2], gCultura);
                lTaxaAdm.CobraTaxaPerfomance           = Convert.ToChar(lTaxaAdmString[3]);
                lTaxaAdm.TaxaPerfomance                = lTaxaAdmString[4];
                lTaxaAdm.RegraTaxaPerformance          = lTaxaAdmString[5];
                lTaxaAdm.TaxaEntrada                   = lTaxaAdmString[6];
                lTaxaAdm.TaxaSaida                     = lTaxaAdmString[7];
                lTaxaAdm.PeriodoCobTxPerf              = Convert.ToInt32(lTaxaAdmString[8] == "" ? "0" : lTaxaAdmString[8]);
                lTaxaAdm.Unidade                       = Convert.ToChar(lTaxaAdmString[9]);
                lTaxaAdm.TaxaComposta                  = Convert.ToChar(lTaxaAdmString[10]);
                lTaxaAdm.DataHora = DateTime.Now;

            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método RetornaFundosTaxaAdm - ", ex);
            }

            return lTaxaAdm;
        }

        private FundosMovCotaInfo RetornaFundosMovCotaInfo(string linha)
        {
            FundosMovCotaInfo lMovCota = new FundosMovCotaInfo();
            try
            {
                string [] lMovCotaString = linha.Split('\t');

                lMovCota.CodigoFundo                   = lMovCotaString[0].Replace("\n","");
                lMovCota.Data                          = Convert.ToDateTime(lMovCotaString[1] ,gCultura);
                lMovCota.DataHora                      = DateTime.Now;
                lMovCota.Identificador                 = lMovCotaString[6];
                lMovCota.ValorMinimoAplicacao          = Convert.ToDouble(lMovCotaString[5]==""?"0":lMovCotaString[5], gCultura);
                lMovCota.ValorMinimoAplicacaoAdicional = Convert.ToDouble(lMovCotaString[3]==""?"0":lMovCotaString[3], gCultura);
                lMovCota.ValorMinimoAplicacaoInicial   = Convert.ToDouble(lMovCotaString[2]==""?"0":lMovCotaString[2], gCultura);
                lMovCota.ValorMinimoResgate            = Convert.ToDouble(lMovCotaString[4]==""?"0":lMovCotaString[4], gCultura);

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método RetornaFundosMovCotaInfo - [{0}]", ex.StackTrace);
            }

            return lMovCota;
        }
        
        private FundosDiaInfo RetornaFundosDia(string linha)
        {
            FundosDiaInfo lFundoDia = new FundosDiaInfo();

            try
            {
                string[] lFundoDiaString = linha.Split('\t');

                lFundoDia.CodigoFundo      = lFundoDiaString[0].Replace("\n","");
                lFundoDia.Data             = Convert.ToDateTime(lFundoDiaString[1], gCultura);
                lFundoDia.Pl               = Convert.ToDouble(lFundoDiaString[2]==""?"0":lFundoDiaString[2], gCultura);
                lFundoDia.ValorCota        = Convert.ToDouble(lFundoDiaString[3]==""?"0":lFundoDiaString[3], gCultura);
                lFundoDia.RentabilidadeDia = Convert.ToDouble(lFundoDiaString[4]==""?"0":lFundoDiaString[4], gCultura);
                lFundoDia.RentabilidadeMes = Convert.ToDouble(lFundoDiaString[5]==""?"0":lFundoDiaString[5], gCultura);
                lFundoDia.RentabilidadeAno = Convert.ToDouble(lFundoDiaString[6]==""?"0":lFundoDiaString[6], gCultura);
                
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método RetornaFundosDia - [{0}]", ex.StackTrace);
            }

            return lFundoDia;
        }

        private FundosMesInfo RetornaFundosMes(string linha)
        {
            FundosMesInfo lFundoMes = new FundosMesInfo();

            try
            {
                string [] lFundoMesString = linha.Split('\t');

                lFundoMes.CodigoFundo      = lFundoMesString[0].Replace("\n","");
                lFundoMes.DataMes          = Convert.ToInt32(lFundoMesString[1]);
                lFundoMes.DataAno          = Convert.ToInt32(lFundoMesString[2]);
                lFundoMes.ValorPL          = Convert.ToDouble(lFundoMesString[3]==""?"0":lFundoMesString[3], gCultura);
                lFundoMes.ValorCota        = Convert.ToDouble(lFundoMesString[4]==""?"0":lFundoMesString[4], gCultura);
                lFundoMes.RentabilidadeMes = Convert.ToDouble(lFundoMesString[5] == "" ? "0" : lFundoMesString[5], gCultura);
                lFundoMes.RentabilidadeAno = Convert.ToDouble(lFundoMesString[6]==""?"0":lFundoMesString[6], gCultura);
                lFundoMes.CodigoTipo       = lFundoMesString[7];
                lFundoMes.DataHora         = DateTime.Now;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método RetornaFundosMes - [{0}] ", ex.StackTrace);
            }

            return lFundoMes;
        }
        #endregion

        #region Leitura avulsa
        public List<FundosDiaInfo> LeArquivoFundosDiaAvulsa()
        {
            List<FundosDiaInfo> lRetorno = new List<FundosDiaInfo>();
            try
            {
                string lArquivo = @"C:\ANBIMA\Arquivos\anvldia.txt";
                System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo);
                string lContent = lFile.ReadToEnd();
                lFile.Close();

                string[] Linhas = lContent.Split('\r');

                foreach (string linha in Linhas)
                {
                    gLogger.InfoFormat("Linha de Fundos Dia encontrada [{0}].", linha);
                    lRetorno.Add(RetornaFundosDia(linha));
                }

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LeArquivoFundosDia - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<FundosMesInfo> LeArquivoFundosMesAvulsa()
        {
            List<FundosMesInfo> lRetorno = new List<FundosMesInfo>();
            try
            {
                string lArquivo = @"C:\ANBIMA\Arquivos\anvlmes.txt"; //GetNomeCorretoArquivo(ListaArquivos["FundosMes"]);
                System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo);
                string lContent = lFile.ReadToEnd();
                lFile.Close();

                string[] Linhas = lContent.Split('\r');

                foreach (string linha in Linhas)
                {
                    gLogger.InfoFormat("Linha de Fundos Mes encontrada [{0}].", linha);
                    lRetorno.Add(RetornaFundosMes(linha));
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LeArquivoFundosMes - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<FundosMovCotaInfo> LeArquivoFundosMovCotaAvulsa()
        {
            List<FundosMovCotaInfo> lRetorno = new List<FundosMovCotaInfo>();

            try
            {
                string lArquivo = @"C:\ANBIMA\Arquivos\anmovcot.txt";// GetNomeCorretoArquivo(ListaArquivos["FundosMovCota"]);
                System.IO.StreamReader lFile = new System.IO.StreamReader(lArquivo);
                string lContent = lFile.ReadToEnd();
                lFile.Close();

                string[] Linhas = lContent.Split('\r');

                foreach (string linha in Linhas)
                {
                    gLogger.InfoFormat("Linha de Mov Cota encontrada [{0}].", linha);

                    lRetorno.Add(RetornaFundosMovCotaInfo(linha));
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método LeArquivoFundosMovCota - [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }
        #endregion
    }
}
