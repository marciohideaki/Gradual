using System;
using System.Collections.Generic;
using log4net;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.DbLib.Dados;
using Gradual.FIDC.Adm.DbLib.App_Codigo;

namespace Gradual.FIDC.Adm.DbLib.Persistencia
{
    /// <summary>
    /// Classe de gerenciamento de acesso ao banco de dados das informações de consulta de fundos em constituição
    /// </summary>
    public class AlteracaoRegulamentacaoFundosConsultaDb
    {
        #region Propriedades
        public static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        public AlteracaoRegulamentacaoConsultaFundosCarregarGridResponse ObterLista(AlteracaoRegulamentacaoConsultaFundosCarregarGridRequest pRequest)
        {
            var lRetorno = new AlteracaoRegulamentacaoConsultaFundosCarregarGridResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_fluxo_alteracao_regulamento_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada                    
                    lAcessaDados.AddInParameter(cmd, "@SelecionarPendentes", DbType.Int32, pRequest.SelecionarPendentes);
                    lAcessaDados.AddInParameter(cmd, "@SelecionarConcluidos", DbType.Int32, pRequest.SelecionarConcluídos);

                    if (pRequest.DtDe != null)
                        lAcessaDados.AddInParameter(cmd, "@DataDe", DbType.DateTime, pRequest.DtDe.Value);
                    if (pRequest.DtAte != null)
                        lAcessaDados.AddInParameter(cmd, "@DataAte", DbType.DateTime, pRequest.DtAte.Value);

                    if (pRequest.IdFundoCadastro > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);
                    if (pRequest.IdFluxoAlteracaoRegulamentoGrupo > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFluxoAlteracaoRegulamentoGrupo", DbType.Int32, pRequest.IdFluxoAlteracaoRegulamentoGrupo);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaConsultaFundos = new List<ConsultaFundosConstituicaoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista =
                            new ConsultaFundosConstituicaoInfo
                            {
                                IdFundoCadastro = dr["idFundoCadastro"].DBToInt32(),
                                NomeFundo = dr["nomeFundo"].DBToString(),
                                Grupo = dr["DsFundoFluxoGrupo"].DBToString(),
                                Etapa = dr["DsFundoFluxoGrupoEtapa"].DBToString(),
                                StatusEtapa = dr["DsFundoFluxoStatus"].DBToString(),
                                StatusGeral = dr["StatusGeral"].DBToString()
                            };
                        
                        lRetorno.ListaConsultaFundos.Add(itemLista);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro encontrado no método AlteracaoRegulamentacaoFundosConsultaDb.Buscar", ex);
            }

            return lRetorno;
        }
        
        public AlteracaoRegulamentacaoCarregarDadosModalEnvioEmailResponse BuscarDadosGeraisEmailFundo(int pIdFundoCadastro)
        {
            var lRetorno = new AlteracaoRegulamentacaoCarregarDadosModalEnvioEmailResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_TbFundoFluxoAlteracaoRegulamento_selDadosEmail"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, pIdFundoCadastro);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaConsultaFundos = new List<ConsultaFundosConstituicaoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista =
                            new ConsultaFundosConstituicaoInfo
                            {
                                IdFundoCadastro = dr["idFundoCadastro"].DBToInt32(),
                                NomeFundo = dr["nomeFundo"].DBToString(),
                                Grupo = dr["DsFluxoAlteracaoRegulamentoGrupo"].DBToString(),
                                Etapa = dr["DsFluxoAlteracaoRegulamentoGrupoEtapa"].DBToString(),
                                StatusEtapa = dr["DsFluxoAlteracaoRegulamentoStatus"].DBToString(),
                                StatusGeral = dr["StatusGeral"].DBToString()
                            };
                        
                        lRetorno.ListaConsultaFundos.Add(itemLista);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro encontrado no método AlteracaoRegulamentacaoFundosConsultaDb.BuscarDadosGeraisFundo", ex);

                throw;
            }

            return lRetorno;
        }

    }
}
