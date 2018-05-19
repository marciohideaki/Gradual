using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        #region ConsultarClienteAtivoInativo
        /// <summary>
        /// Consulta o cliente pelo ID
        /// </summary>
        /// <param name="pParametros">Parametros do tipo ClienteAtivoInativoInfo</param>
        /// <returns></returns>
        public static ReceberObjetoResponse<ClienteAtivoInativoInfo> ReceberClienteAtivoInativo(ReceberEntidadeRequest<ClienteAtivoInativoInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteAtivoInativoInfo> resposta =
                    new ReceberObjetoResponse<ClienteAtivoInativoInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_ativar_inativar_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        resposta.Objeto = CriarRegistroClienteAtivoInativo(lDataTable.Rows[0]);
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }
        #endregion

        #region Métodos de Apoio
        /// <summary>
        /// Cria registro de Cliente Ativo Inativo
        /// </summary>
        /// <param name="linha">Linha datarow</param>
        /// <returns>Retoan ua entidade de ClienteAtivoInativoInfo</returns>
        private static ClienteAtivoInativoInfo CriarRegistroClienteAtivoInativo(DataRow linha)
        {
            ClienteAtivoInativoInfo lClienteAtivoInativo = new ClienteAtivoInativoInfo()
            {
                DsNomeCliente = linha["ds_nome"].DBToString(),
                IdCliente = linha["id_cliente"].DBToInt32(),
                St_Ativo = Convert.ToBoolean(linha["st_ativo"]),
                DtAtivacaoInativacao = linha["dt_ativacaoinativacao"].DBToDateTime()
            };

            return lClienteAtivoInativo;
        }
        #endregion

        #region SalvarClienteAtivoInativo
        /// <summary>
        /// Ativa e inativa o cliente em questão
        /// </summary>
        /// <param name="pParametros">Parametros do tipo ClienteAtivoInativoInfo</param>
        /// <returns>Retorna uma entidade do tipo SalvarEntidadeResponse(ClienteAtivoInativoInfo) vazia</returns>
        public static SalvarEntidadeResponse<ClienteAtivoInativoInfo> SalvarClienteAtivoInativo(SalvarObjetoRequest<ClienteAtivoInativoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_ativar_inativar_upd_sp"))
                {
                    DateTime dtAgora = DateTime.Now;
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativo", DbType.Int32, pParametros.Objeto.St_Ativo);
                    lAcessaDados.AddInParameter(lDbCommand, "@dt_ativacaoinativacao", DbType.DateTime, dtAgora);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteAtivoInativoInfo>()
                    {
                        Objeto = new ClienteAtivoInativoInfo()
                        {
                            DtAtivacaoInativacao = dtAgora,
                            St_Ativo = pParametros.Objeto.St_Ativo,
                            IdCliente = pParametros.Objeto.IdCliente
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }
        #endregion

        private static void LogarModificacao(ClienteAtivoInativoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteAtivoInativoInfo> lEntrada = new ReceberEntidadeRequest<ClienteAtivoInativoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteAtivoInativoInfo> lRetorno = ReceberClienteAtivoInativo(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

    }
}
