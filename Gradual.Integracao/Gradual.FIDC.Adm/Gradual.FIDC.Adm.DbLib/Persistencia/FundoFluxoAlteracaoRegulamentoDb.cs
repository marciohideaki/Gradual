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
    public class FundoFluxoAlteracaoRegulamentoDb
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        public FundoFluxoAlteracaoRegulamentoResponse BuscarEtapasPorFundo(FundoFluxoAlteracaoRegulamentoRequest pRequest)
        {
            var lRetorno = new FundoFluxoAlteracaoRegulamentoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_TbFundoFluxoAprovacao_selEtapasGravadas"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaFluxoAlteracaoRegulamento = new List<FundoFluxoAlteracaoRegulamentoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista =
                            new FundoFluxoAlteracaoRegulamentoInfo
                            {
                                IdFundoCadastro = dr["IdFundoCadastro"].DBToInt32(),
                                IdFundoFluxoAlteracaoRegulamento = dr["IdFundoFluxoAlteracaoRegulamento"].DBToInt32(),
                                IdFluxoAlteracaoRegulamentoGrupoEtapa =
                                    dr["IdFluxoAlteracaoRegulamentoGrupoEtapa"].DBToInt32(),
                                IdFluxoAlteracaoRegulamentoStatus = dr["IdFluxoAlteracaoRegulamentoStatus"].DBToInt32(),
                                DtInicio = ((dr["DtInicio"].ToString() == "")
                                    ? ""
                                    : dr["DtInicio"].DBToDateTime().ToString("dd/MM/yyyy")),
                                DtConclusao = dr["DtConclusao"].ToString() == ""
                                    ? ""
                                    : dr["DtConclusao"].DBToDateTime().ToString("dd/MM/yyyy"),
                                UsuarioResponsavel = dr["UsuarioResponsavel"].DBToString()
                            };
                        
                        lRetorno.ListaFluxoAlteracaoRegulamento.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método FundoFluxoAlteracaoRegulamentoDb.BuscarEtapasPorFundo", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que insere uma nova etapa de aprovação regulamento de um fundo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoFluxoAlteracaoRegulamentoResponse Inserir(FundoFluxoAlteracaoRegulamentoRequest request)
        {
            var lRetorno = new FundoFluxoAlteracaoRegulamentoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_TbFundoFluxoAlteracaoRegulamento_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@IdFluxoAlteracaoRegulamentoGrupoEtapa", DbType.Int32, request.IdFluxoAlteracaoRegulamentoGrupoEtapa);
                    lAcessaDados.AddInParameter(cmd, "@IdFluxoAlteracaoRegulamentoStatus", DbType.Int32, request.IdFluxoAlteracaoRegulamentoStatus);
                    lAcessaDados.AddInParameter(cmd, "@DtInicio", DbType.DateTime, request.DtInicio);
                    lAcessaDados.AddInParameter(cmd, "@DtConclusao", DbType.DateTime, request.DtConclusao);
                    lAcessaDados.AddInParameter(cmd, "@UsuarioResponsavel", DbType.String, request.UsuarioResponsavel);
                    #endregion

                    request.IdFundoFluxoAlteracaoRegulamento = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método FundoFluxoAlteracaoRegulamentoDb.Inserir", ex);
            }

            return lRetorno;
        }
    }
}
