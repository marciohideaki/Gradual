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
    public class FundoCategoriaDB
    {
        #region Propriedades
        public static readonly ILog GLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Busca categorias cadastradas a partir dos parâmetros informados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public FundoCategoriaResponse Buscar(FundoCategoriaRequest request)
        {
            var lRetorno = new FundoCategoriaResponse();

            try
            {
                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = "GradualFundosAdm";

                using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundos_categoria_sel"))
                {
                    #region Adicionar Parâmetros
                    if(request.IdFundoCategoria > 0)
                        lAcessaDados.AddInParameter(cmd, "@IdFundoCategoria", DbType.String, request.IdFundoCategoria);
                    #endregion

                    var table = lAcessaDados.ExecuteDbDataTable(cmd);

                    lRetorno.ListaCategorias = new List<FundoCategoriaInfo>();

                    #region Preenchimento Retorno

                    //preenche lista de retorno
                    foreach (DataRow dr in table.Rows)
                    {
                        var itemLista = new FundoCategoriaInfo();

                        itemLista.IdFundoCategoria = dr["IdFundoCategoria"].DBToInt32();
                        itemLista.DsFundoCategoria = dr["DsFundoCategoria"].DBToString();

                        lRetorno.ListaCategorias.Add(itemLista);
                    }
                    #endregion

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
                }
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.StackTrace;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                GLogger.Error("Erro encontrado no método FundoCategoriaDB.Buscar", ex);
            }

            return lRetorno;
        }
    }
}
