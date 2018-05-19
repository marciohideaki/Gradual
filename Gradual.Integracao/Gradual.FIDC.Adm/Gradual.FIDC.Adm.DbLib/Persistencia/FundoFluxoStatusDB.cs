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
    public class FundoFluxoStatusDB
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        /// <summary>
        /// Método que seleciona fundos no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoFluxoStatusResponse Buscar(FundoFluxoStatusRequest pRequest)
        {
            FundoFluxoStatusResponse lRetorno = new FundoFluxoStatusResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_fluxo_status_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdFundoFluxoStatus > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoFluxoStatus", DbType.Int32, pRequest.IdFundoFluxoStatus);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaStatus = new List<FundoFluxoStatusInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new FundoFluxoStatusInfo();

                        itemLista.IdFundoFluxoStatus = dr["IdFundoFluxoStatus"].DBToInt32();
                        itemLista.DsFundoFluxoStatus = dr["DsFundoFluxoStatus"].DBToString();

                        lRetorno.ListaStatus.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método FundoFluxoStatusDB.Buscar", ex);
            }

            return lRetorno;
        }
    }
}
