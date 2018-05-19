using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados.Vendas;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Vendas
{
    /// <summary>
    /// Classe de acesso a dados para gerenciado do IPO com o cliente
    /// </summary>
    public class ProdutoIPOClienteDbLib
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
        /// Método que recebe os dados de filtro, ou não, e efetua a consulta no bancos de dados 
        /// </summary>
        /// <param name="pParametros"></param>
        /// <returns>Retorna uma lista de objeto de gerenciamento de IPO do banco de dados</returns>
        public static ConsultarObjetosResponse<IPOClienteInfo> ConsultarProdutosIPOCliente(ConsultarEntidadeRequest<IPOClienteInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<IPOClienteInfo>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

            lRetorno.Resultado = new List<IPOClienteInfo>();

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_cliente_ipo_sel"))
            {
                if (pParametros.Objeto != null )
                {
                    lAcessaDados.AddInParameter(lCommand, "@CodigoCliente", DbType.Int32,       pParametros.Objeto.CodigoCliente);
                    lAcessaDados.AddInParameter(lCommand, "@CodigoISIN",    DbType.String ,     pParametros.Objeto.CodigoISIN);
                    lAcessaDados.AddInParameter(lCommand, "@DataDe",        DbType.DateTime,    pParametros.Objeto.DataDe);
                    lAcessaDados.AddInParameter(lCommand, "@DataAte",       DbType.DateTime,    pParametros.Objeto.DataAte);
                    lAcessaDados.AddInParameter(lCommand, "@CodigoAssessor",DbType.Int32,       pParametros.Objeto.CodigoAssessor);

                    if (pParametros.Objeto.Status  != eStatusIPO.Nenhum)
                    {
                        lAcessaDados.AddInParameter(lCommand, "@Status", DbType.Int32, pParametros.Objeto.Status);
                    }

                    lAcessaDados.AddInParameter(lCommand, "@CpfCnpj",       DbType.String,      pParametros.Objeto.CpfCnpj);

                }

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetorno.Resultado.Add(InstanciarIPOCliente(lRow));
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que recebe os dados de gerenciamento de IPO e monta um objeto do Tipo de gerenciamento de IPO
        /// </summary>
        /// <param name="pRow">Datarow com os dados de gerenciamento de IPO</param>
        /// <returns>Retorno  de objeto IPO</returns>
        private static IPOClienteInfo InstanciarIPOCliente(DataRow pRow)
        {
            var lIpo = new IPOClienteInfo();

            lIpo.CodigoCliente        = pRow["CodigoCliente"].DBToInt32();
            lIpo.NomeCliente          = pRow["NomeCliente"].DBToString();
            lIpo.CodigoAssessor       = pRow["CodigoAssessor"].DBToInt32();
            lIpo.CodigoISIN           = pRow["CodigoISIN"].DBToString();
            lIpo.CpfCnpj              = pRow["DsCpfCnpj"].DBToString();
            lIpo.Empresa              = pRow["DsEmpresa"].DBToString();
            lIpo.Modalidade           = pRow["DsModalidade"].DBToString();
            lIpo.Data                 = pRow["dtSolicitacao"].DBToDateTime();
            lIpo.ValorReserva         = pRow["vlReserva"].DBToDecimal();
            lIpo.ValorMaximo          = pRow["vlMaximo"].DBToDecimal();
            lIpo.TaxaMaxima           = pRow["vlTaxaMaxima"].DBToDecimal();
            lIpo.Limite               = pRow["vlLimite"].DBToDecimal();
            lIpo.PessoaVinculada      = pRow["PessoaVinculada"].DBToBoolean();
            lIpo.VlPercentualGarantia = pRow["VlPercentualGarantia"].DBToDecimal();
            lIpo.Status               = (eStatusIPO)Enum.Parse(typeof(eStatusIPO), pRow["dsStatus"].DBToString());
            lIpo.CodigoIPOCliente     = pRow["CodigoClienteIPO"].DBToInt32();
            lIpo.NumeroProtocolo      = pRow["NumeroProtocolo"].DBToString();
            lIpo.Observacoes          = pRow["Observacoes"].ToString();
            return lIpo;
        }

        /// <summary>
        /// Método que verifiac se vai atualizar ou se vai incluir os dados de cadastro
        /// de IPO no banco de dados
        /// </summary>
        /// <param name="pParametros">Objeto emcapsulado de dados de cadastro do IPO</param>
        /// <returns></returns>
        public static SalvarEntidadeResponse<IPOClienteInfo> Salvar(SalvarObjetoRequest<IPOClienteInfo> pParametros)
        {
            if (pParametros.Objeto.CodigoIPOCliente.HasValue)
            {
                return AtualizarStatus(pParametros);
            }
            else
            {
                return Incluir(pParametros);
            }
        }

        /// <summary>
        /// Método que chama a procedure de atualização de Status de  IPO no banco de dados 
        /// procedure - prc_produto_ipo_upd
        /// </summary>
        /// <param name="pParametros">Objeto emcapsulado de dados de cadastro do IPO</param>
        /// <returns>Retorna um objeto Encapsulado do tipo SalvarEntidade SalvarEntidadeResponse<IPOClienteInfo></returns>
        private static SalvarEntidadeResponse<IPOClienteInfo> AtualizarStatus(SalvarObjetoRequest<IPOClienteInfo> pParametros)
        {
            var lResponse = new SalvarEntidadeResponse<IPOClienteInfo>();

            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_cliente_ipo_status_upd"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoIPOCliente",    DbType.Int32,   pParametros.Objeto.CodigoIPOCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente",       DbType.Int32,   pParametros.Objeto.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeCliente",         DbType.String,  pParametros.Objeto.NomeCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor",      DbType.Int32,   pParametros.Objeto.CodigoAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsCpfCnpj",           DbType.String,  pParametros.Objeto.CpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsEmpresa",           DbType.String,  pParametros.Objeto.Empresa);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsModalidade",        DbType.String,  pParametros.Objeto.Modalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtSolicitacao",       DbType.DateTime, pParametros.Objeto.Data);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlReserva",           DbType.Decimal, pParametros.Objeto.ValorReserva);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlMaximo",            DbType.Decimal, pParametros.Objeto.ValorMaximo);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlTaxaMaxima",        DbType.Decimal, pParametros.Objeto.TaxaMaxima);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlLimite",            DbType.Decimal, pParametros.Objeto.Limite);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsStatus",            DbType.Int32,   (int)pParametros.Objeto.Status);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoISIN",          DbType.String,  pParametros.Objeto.CodigoISIN);
                    lAcessaDados.AddInParameter(lDbCommand, "@NumeroProtocolo",     DbType.String,  pParametros.Objeto.NumeroProtocolo);
                    lAcessaDados.AddInParameter(lDbCommand, "@Observacoes",         DbType.String, pParametros.Objeto.Observacoes);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.Objeto = pParametros.Objeto;

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);

                throw ex;
            }

            return lResponse;
        }

        /// <summary>
        /// Método que chama a procedure de atualização de Solicitação de reserva de IPO no banco de dados 
        /// procedure - prc_produto_cliente_ipo_upd
        /// </summary>
        /// <param name="pParametros">Objeto emcapsulado de dados de solicitação de reserva de IPO</param>
        /// <returns>Retorna um objeto Encapsulado do tipo SalvarEntidade SalvarEntidadeResponse<IPOClienteInfo></returns>
        private static SalvarEntidadeResponse<IPOClienteInfo> Atualizar(SalvarObjetoRequest<IPOClienteInfo> pParametros)
        {
            var lResponse = new SalvarEntidadeResponse<IPOClienteInfo>();

            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_cliente_ipo_upd"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente",   DbType.Int32,   pParametros.Objeto.CodigoISIN);
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeCliente",     DbType.String,  pParametros.Objeto.NomeCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor",  DbType.Int32,   pParametros.Objeto.CodigoAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsCpfCnpj",       DbType.String,  pParametros.Objeto.Modalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsEmpresa",       DbType.String,  pParametros.Objeto.Empresa);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsModalidade",    DbType.String,  pParametros.Objeto.Modalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtSolicitacao",   DbType.DateTime,pParametros.Objeto.Data);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlReserva",       DbType.Decimal, pParametros.Objeto.ValorReserva);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlMaximo",        DbType.Decimal, pParametros.Objeto.ValorMaximo);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlTaxaMaxima",    DbType.Decimal, pParametros.Objeto.TaxaMaxima);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlLimite",        DbType.Decimal, pParametros.Objeto.Limite);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsStatus",        DbType.Boolean, pParametros.Objeto.Status);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoISIN",      DbType.String,  pParametros.Objeto.CodigoISIN);
                    lAcessaDados.AddInParameter(lDbCommand, "@NumeroProtocolo", DbType.String,  pParametros.Objeto.NumeroProtocolo);
                    lAcessaDados.AddInParameter(lDbCommand, "@Observacoes",     DbType.String,  pParametros.Objeto.Observacoes);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.Objeto = pParametros.Objeto;

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);

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
        private static SalvarEntidadeResponse<IPOClienteInfo> Incluir(SalvarObjetoRequest<IPOClienteInfo> pParametros)
        {
            var lResponse = new SalvarEntidadeResponse<IPOClienteInfo>();

            try
            {
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = NomeDaConexaoDeProdutoIPO;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produto_cliente_ipo_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoISIN", DbType.String,               pParametros.Objeto.CodigoISIN);                     
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente",  DbType.Int32,            pParametros.Objeto.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeCliente",    DbType.String,           pParametros.Objeto.NomeCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor", DbType.Int32,            pParametros.Objeto.CodigoAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsCpfCnpj",      DbType.String,           pParametros.Objeto.CpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsEmpresa",      DbType.String,           pParametros.Objeto.Empresa);
                    lAcessaDados.AddInParameter(lDbCommand, "@DsModalidade",   DbType.String,           pParametros.Objeto.Modalidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtSolicitacao",  DbType.DateTime,         pParametros.Objeto.Data);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlReserva",      DbType.Decimal,          pParametros.Objeto.ValorReserva);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlMaximo",       DbType.Decimal,          pParametros.Objeto.ValorMaximo);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlTaxaMaxima",   DbType.Decimal,          pParametros.Objeto.TaxaMaxima);
                    lAcessaDados.AddInParameter(lDbCommand, "@vlLimite",       DbType.Decimal,          pParametros.Objeto.Limite);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsStatus",       DbType.Boolean,          pParametros.Objeto.Status);
                    lAcessaDados.AddInParameter(lDbCommand, "@PessoaVinculada", DbType.Boolean,         pParametros.Objeto.PessoaVinculada);
                    lAcessaDados.AddInParameter(lDbCommand, "@VlPercentualGarantia", DbType.Decimal,    pParametros.Objeto.VlPercentualGarantia);
                    lAcessaDados.AddInParameter(lDbCommand, "@NumeroProtocolo", DbType.String,          pParametros.Objeto.NumeroProtocolo);
                    lAcessaDados.AddInParameter(lDbCommand, "@Observacoes",     DbType.String,          pParametros.Objeto.Observacoes);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.Objeto = pParametros.Objeto;

                    //lResponse.Objeto.IdProduto = Convert.ToInt32(lDbCommand.Parameters["@id_produto"].Value);

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
