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
    public class FluxoAlteracaoRegulamentoGrupoEtapaDb
    {
        #region Propriedades
        public static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        /// <summary>
        /// Método que seleciona etapas de aprovação no banco de dados de acordo com os parâmetros informados
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public FluxoAlteracaoRegulamentoGrupoEtapaResponse Buscar(FluxoAlteracaoRegulamentoGrupoEtapaRequest pRequest)
        {
            var lRetorno = new FluxoAlteracaoRegulamentoGrupoEtapaResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_FluxoAlteracaoRegulamentoGrupoEtapa_sel"))
                {
                    #region Adicionar Parâmetros
                    //tratamento dos parâmetros de entrada
                    if (pRequest.IdFluxoAlteracaoRegulamentoGrupo > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFluxoAlteracaoRegulamentoGrupo", DbType.Int32, pRequest.IdFluxoAlteracaoRegulamentoGrupo);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaEtapas = new List<FluxoAlteracaoRegulamentoGrupoEtapaInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista =
                            new FluxoAlteracaoRegulamentoGrupoEtapaInfo()
                            {
                                IdFluxoAlteracaoRegulamentoGrupo = dr["IdFluxoAlteracaoRegulamentoGrupo"].DBToInt32(),
                                IdFluxoAlteracaoRegulamentoGrupoEtapa = dr["IdFluxoAlteracaoRegulamentoGrupoEtapa"].DBToInt32(),
                                DsFluxoAlteracaoRegulamentoGrupoEtapa = dr["DsFluxoAlteracaoRegulamentoGrupoEtapa"].DBToString()
                            };

                        lRetorno.ListaEtapas.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro encontrado no método FluxoAlteracaoRegulamentoGrupoEtapaDb.Buscar", ex);
            }

            return lRetorno;
        }
    }
}
