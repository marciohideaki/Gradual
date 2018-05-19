using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Globalization;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.DbLib.Dados;
using Gradual.FIDC.Adm.DbLib.App_Codigo;

namespace Gradual.FIDC.Adm.DbLib.Persistencia
{
    /// <summary>
    /// Classe de gerenciamento de acesso ao banco de dados das informações de fundos
    /// </summary>
    public class FundoFluxoAprovacaoDB
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        /// <summary>
        /// Método que seleciona as etapas de aprovacao de um fundo no banco de dados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoFluxoAprovacaoResponse BuscarEtapasPorFundo(FundoFluxoAprovacaoRequest pRequest)
        {
            FundoFluxoAprovacaoResponse lRetorno = new FundoFluxoAprovacaoResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_fluxo_aprovacao_ultimas_etapas_sel"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, pRequest.IdFundoCadastro);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaFluxoAprovacao = new List<FundoFluxoAprovacaoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new FundoFluxoAprovacaoInfo();

                        itemLista.IdFundoCadastro = dr["IdFundoCadastro"].DBToInt32();
                        itemLista.IdFundoFluxoAprovacao = dr["IdFundoFluxoAprovacao"].DBToInt32();
                        itemLista.IdFundoFluxoGrupoEtapa = dr["IdFundoFluxoGrupoEtapa"].DBToInt32();
                        itemLista.IdFundoFluxoStatus = dr["IdFundoFluxoStatus"].DBToInt32();
                        itemLista.DtInicio = ((dr["DtInicio"].ToString() == "") ? "" : dr["DtInicio"].DBToDateTime().ToString("dd/MM/yyyy"));
                        itemLista.DtConclusao = dr["DtConclusao"].ToString() == "" ? "" : dr["DtConclusao"].DBToDateTime().ToString("dd/MM/yyyy");
                        itemLista.UsuarioResponsavel = dr["UsuarioResponsavel"].DBToString();

                        lRetorno.ListaFluxoAprovacao.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método FundoFluxoAprovacaoDB.BuscarEtapasPorFundo", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que insere uma nova etapa de aprovação de um fundo de investimento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoFluxoAprovacaoResponse Inserir(FundoFluxoAprovacaoRequest request)
        {
            var lRetorno = new FundoFluxoAprovacaoResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_fluxo_aprovacao_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoCadastro", DbType.Int32, request.IdFundoCadastro);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoFluxoGrupoEtapa", DbType.Int32, request.IdFundoFluxoGrupoEtapa);
                    lAcessaDados.AddInParameter(cmd, "@IdFundoFluxoStatus", DbType.Int32, request.IdFundoFluxoStatus);
                    lAcessaDados.AddInParameter(cmd, "@DtInicio", DbType.DateTime, request.DtInicio);
                    lAcessaDados.AddInParameter(cmd, "@DtConclusao", DbType.DateTime, request.DtConclusao);
                    lAcessaDados.AddInParameter(cmd, "@UsuarioResponsavel", DbType.String, request.UsuarioResponsavel);
                    #endregion

                    request.IdFundoFluxoAprovacao = Convert.ToInt32(lAcessaDados.ExecuteScalar(cmd));

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método FundoFluxoAprovacaoDB.Inserir", ex);
            }

            return lRetorno;
        }

    }
}
