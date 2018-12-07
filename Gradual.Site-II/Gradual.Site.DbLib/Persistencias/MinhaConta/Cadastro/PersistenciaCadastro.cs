using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;
using Gradual.Site.DbLib.Mensagens;
using System.Globalization;
using log4net;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta.Cadastro
{
    /// <summary>
    /// Classe de acesso ao banco de dados para informações 
    /// de cadastro de clientes e assessores
    /// </summary>
    public class PersistenciaCadastro
    {
        #region Propriedades
        #region Atributos
        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CultureInfo gCultura = new CultureInfo("pt-BR");
        #endregion

        #endregion
        #region Métodos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public ClienteSinacorResponse BuscaInformacoesClienteSinacor(ClienteSinacorRequest pRequest)
        {
            ClienteSinacorResponse lRetorno = new ClienteSinacorResponse();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();

                    lAcessaDados.ConnectionStringName = "SINACOR";

                    string lProcedure = "PRC_CLIENTE_SPIDER_SEL2";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProcedure))
                    {
                        lAcessaDados.AddInParameter(lCommand, "pCD_BOVESPA", DbType.Int32, pRequest.ClienteSinacor.CodigoCliente);

                        DataTable dt = lAcessaDados.ExecuteOracleDataTable(lCommand);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow lRow = dt.Rows[i];

                                ClienteSinacorInfo lInfo = new ClienteSinacorInfo();

                                lInfo.CodigoCliente = lRow["cd_cliente"].DBToInt32();
                                lInfo.EmailCliente  = lRow["emailcliente"].DBToString();
                                lInfo.EmailAssessor = lRow["emailassessor"].DBToString();
                                lInfo.CodigoAssessor = lRow["cd_assessor"].DBToInt32();

                                lRetorno.ListaClienteSinacor.Add(lInfo);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método BuscaInformacoesClienteSinacor - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
            return lRetorno;
        }
        #endregion
    }
}
