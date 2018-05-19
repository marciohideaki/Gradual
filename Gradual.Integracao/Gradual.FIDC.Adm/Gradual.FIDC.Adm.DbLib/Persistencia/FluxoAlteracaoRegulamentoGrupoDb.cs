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
    /// Classe de gerenciamento de acesso ao banco de dados das informações de grupos de aprovação
    /// </summary>
    public class FluxoAlteracaoRegulamentoGrupoDb
    {
        #region Propriedades
        public static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        
        public AlteracaoRegulamentacaoConsultaFundosCarregarListaGruposResponse ObterLista()
        {
            var lRetorno = new AlteracaoRegulamentacaoConsultaFundosCarregarListaGruposResponse();

            try
            {
                var lAcessaDados = new AcessaDados {ConnectionStringName = "GradualFundosAdm"};
                
                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_FluxoAlteracaoRegulamentoGrupo_sel"))
                {
                    #region Adicionar Parâmetros
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaGrupos = new List<FluxoAlteracaoRegulamentoGrupoInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista =
                            new FluxoAlteracaoRegulamentoGrupoInfo
                            {
                                IdFluxoAlteracaoRegulamentoGrupo = dr["IdFluxoAlteracaoRegulamentoGrupo"].DBToInt32(),
                                DsFluxoAlteracaoRegulamentoGrupo = dr["DsFluxoAlteracaoRegulamentoGrupo"].DBToString()
                            };
                        
                        lRetorno.ListaGrupos.Add(itemLista);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro encontrado no método FundoFluxoGrupoDB.Buscar", ex);

                throw;
            }

            return lRetorno;
        }
    }
}
