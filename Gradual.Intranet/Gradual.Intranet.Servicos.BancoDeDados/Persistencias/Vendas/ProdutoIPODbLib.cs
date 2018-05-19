using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Vendas;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    /// <summary>
    /// Classe de acesso a dados para gerenciado do IPO 
    /// </summary>
    public class ProdutoIPODbLib
    {
        #region Propriedades
        /// <summary>
        /// Nome da conexão do Produto de IPO
        /// </summary>
        public static string NomeDaConexaoDeProdutoIPO
        {
            get
            {
                return "PlataformaInviXX"; //os dados de vendas ficam no  DirectTradeControleAcesso
            }
        }
        #endregion


        #region Métodos Públicos
        /// <summary>
        /// Seleciona os produtos de IPO que irão aaparece no site como compra para os
        /// clientes e na intranet para os assessores aderirem para o cliente
        /// </summary>
        /// <param name="pParametros"></param>
        /// <returns></returns>
        public static ReceberObjetoResponse<IPOInfo> SelecionarProdutosIPOSite(ReceberEntidadeRequest<IPOInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<IPOInfo>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

            lRetorno.Objeto = new IPOInfo();

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_ipo_site_sel"))
            {
                if (pParametros.Objeto != null)
                {
                    //lAcessaDados.AddInParameter(lCommand, "@CodigoCliente", DbType.Int32, pParametros.Objeto.CodigoCliente);
                    lAcessaDados.AddInParameter(lCommand, "@CodigoISIN", DbType.String, pParametros.Objeto.CodigoISIN);
                    lAcessaDados.AddInParameter(lCommand, "@DataDe", DbType.DateTime, pParametros.Objeto.DataInicial);
                    lAcessaDados.AddInParameter(lCommand, "@DataAte", DbType.DateTime, pParametros.Objeto.DataFinal);
                    //lAcessaDados.AddInParameter(lCommand, "@CodigoAssessor", DbType.Int32, pParametros.Objeto.CodigoAssessor);
                    //lAcessaDados.AddInParameter(lCommand, "@Status", DbType.String, pParametros.Objeto.Status);
                }

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetorno.Objeto = InstanciarIPO(lRow);
                }
            }

            return lRetorno;

        }
        /// <summary>
        /// Método que recebe os dados de filtro, ou não, e efetua a consulta no bancos de dados 
        /// </summary>
        /// <param name="pParametros"></param>
        /// <returns>Retorna uma lista de objeto de gerenciamento de IPO do banco de dados</returns>
        public static ConsultarObjetosResponse<IPOInfo> ConsultarProdutosIPO(ConsultarEntidadeRequest<IPOInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<IPOInfo>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

            lRetorno.Resultado = new List<IPOInfo>();

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_ipo_sel"))
            {
                if (pParametros.Objeto != null && !string.IsNullOrEmpty( pParametros.Objeto.CodigoISIN ))
                {
                    lAcessaDados.AddInParameter(lCommand, "@CodigoISIN" , DbType.String,    pParametros.Objeto.CodigoISIN);
                }

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetorno.Resultado.Add(InstanciarIPO(lRow));
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que recebe os dados de gerenciamento de IPO e monta um objeto do Tipo de gerenciamento de IPO
        /// </summary>
        /// <param name="pRow">Datarow com os dados de gerenciamento de IPO</param>
        /// <returns>Retorno  de objeto IPO</returns>
        private static IPOInfo InstanciarIPO(DataRow pRow)
        {
            var lIpo = new IPOInfo();

            lIpo.Ativo                = pRow["DsAtivo"].DBToString();
            lIpo.CodigoIPO            = pRow["CodigoIPO"].DBToInt32();
            lIpo.CodigoISIN           = pRow["CodigoISIN"].DBToString();
            lIpo.DataFinal            = pRow["DataFinal"].DBToDateTime();
            lIpo.DataInicial          = pRow["DataInicio"].DBToDateTime();
            lIpo.DsEmpresa            = pRow["NomeEmpresa"].DBToString();
            lIpo.HoraMaxima           = pRow["HoraMaxima"].DBToString();
            lIpo.Modalidade           = pRow["Modalidade"].DBToString();
            lIpo.Observacoes          = pRow["DsObservacoes"].DBToString();
            lIpo.StAtivo              = pRow["StAtivo"].DBToBoolean();
            lIpo.VlMaximo             = pRow["VlMaximo"].DBToDecimal();
            lIpo.VlMinimo             = pRow["VlMinimo"].DBToDecimal();
            lIpo.VlPercentualGarantia = pRow["VlPercentualGarantia"].DBToDecimal();
            
            return lIpo;
        }

        /// <summary>
        /// Método que verifiac se vai atualizar ou se vai incluir os dados de cadastro
        /// de IPO no banco de dados
        /// </summary>
        /// <param name="pParametros">Objeto emcapsulado de dados de cadastro do IPO</param>
        /// <returns></returns>
        public static SalvarEntidadeResponse<IPOInfo> Salvar(SalvarObjetoRequest<IPOInfo> pParametros)
        {
            if (pParametros.Objeto.CodigoIPO.HasValue && pParametros.Objeto.CodigoIPO != 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Incluir(pParametros);
            }
        }

        /// <summary>
        /// Método que chama a procedure de atualização de cadastro de IPO no banco de dados 
        /// procedure - prc_produto_ipo_upd
        /// </summary>
        /// <param name="pParametros">Objeto emcapsulado de dados de cadastro do IPO</param>
        /// <returns></returns>
        private static SalvarEntidadeResponse<IPOInfo> Atualizar(SalvarObjetoRequest<IPOInfo> pParametros)
        {
            SalvarEntidadeResponse<IPOInfo> lResponse = new SalvarEntidadeResponse<IPOInfo>();

            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_ipo_upd"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoIPO",           DbType.String,      pParametros.Objeto.CodigoIPO);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoISIN",          DbType.String,      pParametros.Objeto.CodigoISIN);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsAtivo",				DbType.String,      pParametros.Objeto.Ativo);
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeEmpresa",			DbType.String,      pParametros.Objeto.DsEmpresa);
                    lAcessaDados.AddInParameter(lDbCommand, "@Modalidade",			DbType.String,      pParametros.Objeto.Modalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@DataInicio",		    DbType.DateTime,    pParametros.Objeto.DataInicial);
                    lAcessaDados.AddInParameter(lDbCommand, "@DataFinal",			DbType.DateTime,    pParametros.Objeto.DataFinal);
                    lAcessaDados.AddInParameter(lDbCommand, "@HoraMaxima",			DbType.String,      pParametros.Objeto.HoraMaxima);
                    lAcessaDados.AddInParameter(lDbCommand, "@VlMinimo",			DbType.Decimal,     pParametros.Objeto.VlMinimo);
                    lAcessaDados.AddInParameter(lDbCommand, "@VlMaximo",			DbType.Decimal,     pParametros.Objeto.VlMaximo);
                    lAcessaDados.AddInParameter(lDbCommand, "@VlPercentualGarantia",DbType.Decimal,     pParametros.Objeto.VlPercentualGarantia);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsObservacoes",		DbType.String,      pParametros.Objeto.Observacoes);
                    lAcessaDados.AddInParameter(lDbCommand, "@stAtivo",				DbType.Boolean,     pParametros.Objeto.StAtivo);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.Objeto = pParametros.Objeto;

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);

                throw ex;
            }

            return lResponse;
        }
        /// <summary>
        /// Método que que chama a procedure de inclusão de IPO no banco de dados 
        /// procedure - prc_produto_ipo_ins
        /// </summary>
        /// <param name="pParametros">Objeto emcapsulado de dados de cadastro do IPO</param>
        /// <returns>Retorna um objeto de cadastro de IPO</returns>
        private static SalvarEntidadeResponse<IPOInfo> Incluir(SalvarObjetoRequest<IPOInfo> pParametros)
        {
            var lResponse = new SalvarEntidadeResponse<IPOInfo>();

            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_ipo_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoISIN",              DbType.String,      pParametros.Objeto.CodigoISIN);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsAtivo",                 DbType.String,      pParametros.Objeto.Ativo);
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeEmpresa",             DbType.String,      pParametros.Objeto.DsEmpresa);
                    lAcessaDados.AddInParameter(lDbCommand, "@Modalidade",              DbType.String,      pParametros.Objeto.Modalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@DataInicio",              DbType.DateTime,    pParametros.Objeto.DataInicial);
                    lAcessaDados.AddInParameter(lDbCommand, "@DataFinal",               DbType.DateTime,    pParametros.Objeto.DataFinal);
                    lAcessaDados.AddInParameter(lDbCommand, "@HoraMaxima",              DbType.String,      pParametros.Objeto.HoraMaxima);
                    lAcessaDados.AddInParameter(lDbCommand, "@VlMinimo",                DbType.Decimal,     pParametros.Objeto.VlMinimo);
                    lAcessaDados.AddInParameter(lDbCommand, "@VlMaximo",                DbType.Decimal,     pParametros.Objeto.VlMaximo);
                    lAcessaDados.AddInParameter(lDbCommand, "@VlPercentualGarantia",    DbType.Decimal,     pParametros.Objeto.VlPercentualGarantia);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsObservacoes",           DbType.String,      pParametros.Objeto.Observacoes);
                    lAcessaDados.AddInParameter(lDbCommand, "@stAtivo",                 DbType.Boolean,     pParametros.Objeto.StAtivo);
                    lAcessaDados.AddOutParameter(lDbCommand, "@CodigoIPO",              DbType.Int32,       4);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.Objeto = pParametros.Objeto;

                    lResponse.Objeto.CodigoIPO = Convert.ToInt32(lDbCommand.Parameters["@CodigoIPO"].Value);

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);

                throw ex;
            }

            return lResponse;
        }
        #endregion
    }
}
