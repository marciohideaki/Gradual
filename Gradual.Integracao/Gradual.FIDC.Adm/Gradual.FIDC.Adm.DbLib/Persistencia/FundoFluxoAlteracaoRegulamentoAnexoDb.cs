using System;
using log4net;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.FIDC.Adm.DbLib.Mensagem;

namespace Gradual.FIDC.Adm.DbLib.Persistencia
{
    /// <summary>
    /// Classe de gerenciamento de acesso ao banco de dados das informações de fundos
    /// </summary>
    public class FundoFluxoAlteracaoRegulamentoAnexoDb
    {
        #region Propriedades
        public static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        /// <summary>
        /// Método que insere uma nova etapa de aprovação de um fundo de investimento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoFluxoAlteracaoRegulamentoAnexoResponse Inserir(FundoFluxoAlteracaoRegulamentoAnexoRequest request)
        {
            var lRetorno = new FundoFluxoAlteracaoRegulamentoAnexoResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_TbFundoFluxoAlteracaoRegulamentoAnexo_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdFundoFluxoAlteracaoRegulamento", DbType.Int32, request.IdFundoFluxoAlteracaoRegulamento);
                    lAcessaDados.AddInParameter(cmd, "@CaminhoAnexo", DbType.String, request.CaminhoAnexo);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                GLogger.Error("Erro encontrado no método FundoFluxoAprovacaoAnexoResponse.Inserir", ex);

                throw;
            }

            return lRetorno;
        }
    }
}
