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
    public class FluxoAlteracaoRegulamentoStatusDb
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Método que seleciona fundos no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pIdFluxoAlteracaoRegulamentoStatus"></param>
        /// <returns></returns>
        public FluxoAlteracaoRegulamentoStatusResponse Buscar(int pIdFluxoAlteracaoRegulamentoStatus)
        {
            var lRetorno = new FluxoAlteracaoRegulamentoStatusResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_TbFluxoAlteracaoRegulamentoStatus_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pIdFluxoAlteracaoRegulamentoStatus > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoFluxoStatus", DbType.Int32, pIdFluxoAlteracaoRegulamentoStatus);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaStatus = new List<FluxoAlteracaoRegulamentoStatusInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista =
                            new FluxoAlteracaoRegulamentoStatusInfo
                            {
                                IdFluxoAlteracaoRegulamentoStatus = dr["IdFluxoAlteracaoRegulamentoStatus"].DBToInt32(),
                                DsFluxoAlteracaoRegulamentoStatus = dr["DsFluxoAlteracaoRegulamentoStatus"].DBToString()
                            };
                        
                        lRetorno.ListaStatus.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método FluxoAlteracaoRegulamentoStatusDb.Buscar", ex);
            }

            return lRetorno;
        }
    }
}
