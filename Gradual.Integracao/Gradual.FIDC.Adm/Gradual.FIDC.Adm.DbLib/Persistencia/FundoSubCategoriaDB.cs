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
    public class FundoSubCategoriaDB
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public FundoSubCategoriaResponse Buscar(FundoSubCategoriaRequest request)
        {
            var lRetorno = new FundoSubCategoriaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_sub_categoria_sel"))
                {
                    #region Adicionar Parâmetros
                    if(request.IdFundoSubCategoria > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoSubCategoria", DbType.String, request.IdFundoSubCategoria);                    
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaSubCategorias = new List<FundoSubCategoriaInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new FundoSubCategoriaInfo();

                        itemLista.IdFundoSubCategoria = dr["IdFundoSubCategoria"].DBToInt32();
                        itemLista.DsFundoSubCategoria = dr["DsFundoSubCategoria"].DBToString();

                        lRetorno.ListaSubCategorias.Add(itemLista);
                    }
                    #endregion

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                gLogger.Error("Erro encontrado no método FundoCategoriaDB.Buscar", ex);
            }

            return lRetorno;
        }
    }
}
