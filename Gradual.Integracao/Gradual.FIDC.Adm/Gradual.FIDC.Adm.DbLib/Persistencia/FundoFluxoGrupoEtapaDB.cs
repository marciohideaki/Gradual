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
    public class FundoFluxoGrupoEtapaDB
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        /// <summary>
        /// Método que seleciona etapas de aprovação no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FundoFluxoGrupoEtapaResponse Buscar(FundoFluxoGrupoEtapaRequest pRequest)
        {
            FundoFluxoGrupoEtapaResponse lRetorno = new FundoFluxoGrupoEtapaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_fluxo_grupo_etapa_fluxo_aprovacao_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdFundoFluxoGrupo > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoFluxoGrupo", DbType.Int32, pRequest.IdFundoFluxoGrupo);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaEtapas = new List<FundoFluxoGrupoEtapaInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new FundoFluxoGrupoEtapaInfo();

                        itemLista.IdFundoFluxoGrupo = dr["IdFundoFluxoGrupo"].DBToInt32();
                        itemLista.IdFundoFluxoGrupoEtapa = dr["IdFundoFluxoGrupoEtapa"].DBToInt32();
                        itemLista.DsFundoFluxoGrupoEtapa = dr["DsFundoFluxoGrupoEtapa"].DBToString();

                        lRetorno.ListaEtapas.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método FundoFluxoGrupoEtapaDB.Buscar", ex);
            }

            return lRetorno;
        }
    }
}
