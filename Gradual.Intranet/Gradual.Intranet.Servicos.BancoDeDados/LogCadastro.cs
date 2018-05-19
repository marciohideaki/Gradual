using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using log4net;

namespace Gradual.Intranet.Servicos.BancoDeDados
{
    public static class LogCadastro
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum eAcao
        {
            Inserir,
            Consultar,
            Receber,
            Alterar,
            Excluir,
            Exportar,
            Importar,
            FalhaLogin,
            EfetuarIPO
        }

        private static string GetCabacalho(object Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao)
        {
            return string.Format("[Entidade] {0} ; [Ação] {1} ; [IdUsuarioLogado (ID_LOGIN)] {2} ; [DescricaoUsuarioLogado] {3}"
                                , Entidade.GetType().Name, Acao.ToString(), IdUsuarioLogado.DBToString(), DescricaoUsuarioLogado);
        }

        public static void Logar(object Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao, Exception Ex = null)
        {
            string lCabecalho = GetCabacalho(Entidade, IdUsuarioLogado, DescricaoUsuarioLogado, Acao);

            string lConteudo = "";

            try
            {

                System.Reflection.PropertyInfo[] propriedades = Entidade.GetType().GetProperties();

                foreach (System.Reflection.PropertyInfo item in propriedades)
                {
                    lConteudo += " ; [" + item.Name + "] ";
                    lConteudo += item.GetValue(Entidade, null).DBToString();
                }
                if (null == Ex)
                    logger.Info(lCabecalho + lConteudo);
                else
                    logger.Error(lCabecalho + lConteudo, Ex);
            }
            catch (Exception ex)
            {
                logger.Error("[Erro ao Salvar Log] ", ex);
            }
        }

        public static void Logar(ClienteAtivarInativarInfo Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao, Exception Ex = null)
        {
            string lCabecalho = GetCabacalho(Entidade, IdUsuarioLogado, DescricaoUsuarioLogado, Acao);
            string lConteudo = "";

            try
            {

                lConteudo += " ; [IdCliente] " + Entidade.IdCliente.DBToString();
                lConteudo += " ; [StClienteGeralAtivo] " + Entidade.StClienteGeralAtivo.DBToString();
                lConteudo += " ; [StLoginAtivo] " + Entidade.StLoginAtivo.DBToString();
                lConteudo += " ; [DtUltimaAtualizacao] " + Entidade.DtUltimaAtualizacao.DBToString();


                if (null != Entidade.Contas)
                {
                    foreach (ClienteAtivarInativarContasInfo item in Entidade.Contas)
                    {
                        lConteudo += " ; [CdCodigo] " + item.CdCodigo.DBToString();
                        if (null != item.Bmf)
                        {
                            lConteudo += " ; [Bmf.CdCodigo] " + item.Bmf.CdCodigo.DBToString();
                            lConteudo += " ; [Bmf.IdClienteConta] " + item.Bmf.IdClienteConta.DBToString();
                            lConteudo += " ; [Bmf.StAtiva] " + item.Bmf.StAtiva.DBToString();
                        }
                        if (null != item.Bovespa)
                        {
                            lConteudo += " ; [Bovespa.CdCodigo] " + item.Bovespa.CdCodigo.DBToString();
                            lConteudo += " ; [Bovespa.IdClienteConta] " + item.Bovespa.IdClienteConta.DBToString();
                            lConteudo += " ; [Bovespa.StAtiva] " + item.Bovespa.StAtiva.DBToString();
                        }
                        if (null != item.Custodia)
                        {
                            lConteudo += " ; [Custodia.CdCodigo] " + item.Custodia.CdCodigo.DBToString();
                            lConteudo += " ; [Custodia.IdClienteConta] " + item.Custodia.IdClienteConta.DBToString();
                            lConteudo += " ; [Custodia.StAtiva] " + item.Custodia.StAtiva.DBToString();
                        }
                        if (null != item.CC)
                        {
                            lConteudo += " ; [CC.CdCodigo] " + item.CC.CdCodigo.DBToString();
                            lConteudo += " ; [CC.IdClienteConta] " + item.CC.IdClienteConta.DBToString();
                            lConteudo += " ; [CC.StAtiva] " + item.CC.StAtiva.DBToString();
                        }
                    }
                }
                if (null == Ex)
                    logger.Info(lCabecalho + lConteudo);
                else
                    logger.Error(lCabecalho + lConteudo, Ex);
            }
            catch (Exception ex)
            {
                logger.Error("[Erro ao Salvar Log] ", ex);
            }

        }


        public static void Logar(SinacorExportarInfo Entidade, int IdUsuarioLogado, string DescricaoUsuarioLogado, eAcao Acao, Exception Ex = null)
        {
            string lCabecalho = GetCabacalho(Entidade, IdUsuarioLogado, DescricaoUsuarioLogado, Acao);

            string lConteudo = "";

            try
            {
                lConteudo += " ; [Entrada.IdCliente] " + Entidade.Entrada.IdCliente.DBToString();
                lConteudo += " ; [Entrada.CdCodigo] " + Entidade.Entrada.CdCodigo.DBToString();
                lConteudo += " ; [Entrada.PrimeiraExportacao] " + Entidade.Entrada.PrimeiraExportacao.DBToString();

                if (null != Entidade.Retorno)
                {


                    lConteudo += " ; [Retorno.DadosClienteOk] " + Entidade.Retorno.DadosClienteOk.DBToString();
                    if (!Entidade.Retorno.DadosClienteOk)
                    {
                        if (null != Entidade.Retorno.DadosClienteMensagens)
                            foreach (var item in Entidade.Retorno.DadosClienteMensagens)
                            {
                                lConteudo += " ; [Mensagem] " + item.Mensagem.DBToString();
                                lConteudo += " ; [Tabela] " + item.Tabela.DBToString();
                            }
                    }

                    lConteudo += " ; [Retorno.ExportacaoSinacorOk] " + Entidade.Retorno.ExportacaoSinacorOk.DBToString();
                    if (!Entidade.Retorno.ExportacaoSinacorOk)
                    {
                        if (null != Entidade.Retorno.ExportacaoSinacorMensagens)
                            foreach (var item in Entidade.Retorno.ExportacaoSinacorMensagens)
                            {
                                lConteudo += " ; [CD_CLIENTE] " + item.CD_CLIENTE.DBToString();
                                lConteudo += " ; [CD_CPFCGC] " + item.CD_CPFCGC.DBToString();
                                lConteudo += " ; [CD_EXTERNO] " + item.CD_EXTERNO.DBToString();
                                lConteudo += " ; [DS_AUX] " + item.DS_AUX.DBToString();
                                lConteudo += " ; [DS_OBS] " + item.DS_OBS.DBToString();
                                lConteudo += " ; [DT_IMPORTA] " + item.DT_IMPORTA.DBToString();
                                lConteudo += " ; [DT_NASC_FUND] " + item.DT_NASC_FUND.DBToString();
                                lConteudo += " ; [NM_CLIENTE] " + item.NM_CLIENTE.DBToString();
                            }
                    }

                    lConteudo += " ; [Retorno.ExportacaoAtualizarCadastroOk] " + Entidade.Retorno.ExportacaoAtualizarCadastroOk.DBToString();
                    if (!Entidade.Retorno.ExportacaoAtualizarCadastroOk)
                    {
                        if (null != Entidade.Retorno.ExportacaoAtualizarCadastroMensagens)
                            foreach (var item in Entidade.Retorno.ExportacaoAtualizarCadastroMensagens)
                            {
                                lConteudo += " ; [Mensagem] " + item.Mensagem.DBToString();
                                lConteudo += " ; [Tabela] " + item.Tabela.DBToString();
                            }
                    }

                    lConteudo += " ; [Retorno.ExportacaoComplementosOk] " + Entidade.Retorno.ExportacaoComplementosOk.DBToString();
                    if (!Entidade.Retorno.ExportacaoComplementosOk)
                    {
                        if (null != Entidade.Retorno.ExportacaoComplementosMensagens)
                            foreach (var item in Entidade.Retorno.ExportacaoComplementosMensagens)
                            {
                                lConteudo += " ; [Mensagem] " + item.Mensagem.DBToString();
                                lConteudo += " ; [Tabela] " + item.Tabela.DBToString();
                                lConteudo += " ; [Procedure] " + item.Procedure.DBToString();
                            }
                    }

                    lConteudo += " ; [Retorno.ExportacaoRiscoOk] " + Entidade.Retorno.ExportacaoRiscoOk.DBToString();
                    if (!Entidade.Retorno.ExportacaoRiscoOk)
                    {
                        if (null != Entidade.Retorno.ExportacaoRiscoMensagens)
                            foreach (var item in Entidade.Retorno.ExportacaoRiscoMensagens)
                            {
                                lConteudo += " ; [Mensagem] " + item.Mensagem.DBToString();
                                lConteudo += " ; [Tabela] " + item.Tabela.DBToString();
                                lConteudo += " ; [Procedure] " + item.Procedure.DBToString();
                            }

                    }

                }
                if (null == Ex)
                    logger.Info(lCabecalho + lConteudo);
                else
                    logger.Error(lCabecalho + lConteudo, Ex);
            }
            catch (Exception ex)
            {
                logger.Error("[Erro ao Salvar Log] ", ex);
            }


        }



    }
}
