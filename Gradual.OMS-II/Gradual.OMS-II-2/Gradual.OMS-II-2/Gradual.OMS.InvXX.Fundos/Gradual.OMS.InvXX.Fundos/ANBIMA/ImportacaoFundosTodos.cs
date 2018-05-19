using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.OMS.InvXX.Fundos.DbLib;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos;
using Gradual.OMS.InvXX.Fundos.Lib.ANBIMA;

namespace Gradual.OMS.InvXX.Fundos.ANBIMA
{
    public class ImportacaoFundosTodos : ImportacaoFundosServico
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Métodos
        public void ThreadImportacaoAnbima(object sender, bool signed)
        {
            DateTime lNow = DateTime.Now;

            List<string> listHorarios = ListaHorarios(this.HorariosImportacaoRentabilidade);
            
            gLogger.Info("********************************************************************");
            gLogger.Info("**Entrou na verificação de Importação de TODOS os fundos da Anbima**");
            gLogger.Info("********************************************************************");

            try
            {
                if (listHorarios.Contains(lNow.ToString("HH:mm")))
                {
                    

                    gLogger.Debug("Obtendo relacao de arquivos para serem importados");

                    SharpSSH lSftp = new SharpSSH();

                    bool lTransferiu = lSftp.TranferirArquivo();

                    if (lTransferiu)
                    {
                        var lImpDB = new ImportacaoArquivoDbLib();

                        
                        LeituraArquivos lLeitura = new LeituraArquivos();
                        List<FundosDiaInfo> lFundosDia = lLeitura.LeArquivoFundosDia();
                        List<FundosMesInfo> lFundosMes = lLeitura.LeArquivoFundosMes();
                        //List<FundosMovCotaInfo> lFundosMovCota         = lLeitura.LeArquivoFundosMovCota();
                        List<TaxaAdministracaoInfo> lTaxaAdministracao = lLeitura.LeArquivoFundosTaxaAdm();
                        List<ANBIMAIndicadoresMesInfo> lIndicadoresMes = lLeitura.LerArquivoIndcadoresMes();
                        List<ANBIMAFundosInfo> lCadFundos = lLeitura.LeArquivoCadastroFundos();

                        //lIndicadoresMes.ForEach(
                        //    indicador =>
                        //    {
                        //        lImpDB.SalvarIndicadoresMes(indicador);
                        //        gLogger.InfoFormat("Indicador  -> [{0}] importado com sucesso", indicador.CodigoIndicador);

                        //    });

                        //lTaxaAdministracao.ForEach(
                        //    taxa =>
                        //    {
                        //        //if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(taxa.CodigoFundo))
                        //        //{
                        //            lImpDB.SalvarTaxaAdministrador(taxa);

                        //            gLogger.InfoFormat("Taxa administracao  -> [{0}] importada com sucesso", taxa.CodigoFundo);
                        //        //}
                        //    }
                        //    );

                        lFundosDia.ForEach(
                            fundo =>
                            {
                                lImpDB.ImportarRentabilidadeDia(fundo);

                                gLogger.InfoFormat("Rentabilidade Dia -> [{0}] importado com sucesso", fundo.CodigoFundo);
                                
                            });

                        lFundosMes.ForEach(
                            fundo =>
                            {
                                lImpDB.ImportarRentabilidadeMes(fundo);

                                gLogger.InfoFormat("Rentabiliadde Mês -> [{0}] importado com sucesso", fundo.CodigoFundo);
                                
                            });
                        lCadFundos.ForEach(
                            fundo =>
                            {
                                lImpDB.ImportarFundos(fundo);

                                gLogger.InfoFormat("Cadastro do fundo -> [{0}] importado com sucesso", fundo.CodigoFundo);
                            });
                        //lFundosMovCota.ForEach(
                        //    fundo =>
                        //    {
                        //        if (gListaCodigoAnbima.Count > 0 && gListaCodigoAnbima.Contains(fundo.CodigoFundo))
                        //       // if (gListaCodigoAnbima.Count > 0)
                        //        {
                        //            if (ListaIdProdutoAnbima.ContainsKey(fundo.CodigoFundo))
                        //            {
                        //                fundo.IdProduto = ListaIdProdutoAnbima[fundo.CodigoFundo];

                        //                lImpDB.SalvarProdutoMovimento(fundo);

                        //                gLogger.InfoFormat("Fundos Mov Cota -> [{0}] importado com sucesso", fundo.CodigoFundo);
                        //            }
                        //        }
                        //    });

                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Ocorreu um erro ao acessar o método ThreadImportacaoAnbima:" + ex.Message, ex);
            }

            //Thread.Sleep(250);
        }
        #endregion

    }
}
