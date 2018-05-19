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
    public class CotistaFidcProcuradorAnexoDb
    {
        #region Propriedades
        private static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        public CotistaFidcProcuradorAnexoResponse Inserir(CotistaFidcProcuradorAnexoRequest request)
        {
            var lRetorno = new CotistaFidcProcuradorAnexoResponse();

            try
            {
                var lAcessaDados = new AcessaDados { ConnectionStringName = "GradualFundosAdm" };

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_procurador_anexo_ins"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidcProcurador", DbType.Int32, request.IdCotistaFidcProcurador);
                    lAcessaDados.AddInParameter(cmd, "@CaminhoAnexo", DbType.String, request.CaminhoAnexo);
                    lAcessaDados.AddInParameter(cmd, "@TipoAnexo", DbType.String, request.TipoAnexo);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;

                GLogger.Error("Erro encontrado no método CotistaFidcProcuradorAnexoDb.Inserir", ex);

                throw ex;
            }

            return lRetorno;
        }

        public CotistaFidcProcuradorAnexoResponse ExcluirPorProcurador(CotistaFidcProcuradorAnexoRequest request)
        {
            var lRetorno = new CotistaFidcProcuradorAnexoResponse();

            try
            {
                var lAcessaDados = new AcessaDados { ConnectionStringName = "GradualFundosAdm" };

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotista_fidc_procurador_anexo_removerPorProcurador"))
                {
                    #region Adicionar Parâmetros
                    lAcessaDados.AddInParameter(cmd, "@IdCotistaFidcProcurador", DbType.Int32, request.IdCotistaFidcProcurador);
                    #endregion

                    lAcessaDados.ExecuteNonQuery(cmd);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método CotistaFidcProcuradorAnexoDb.ExcluirPorProcurador", ex);

                throw ex;
            }

            return lRetorno;
        }
    }
}
