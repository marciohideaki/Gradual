using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static SalvarEntidadeResponse<ClienteControladoraInfo> SalvarClienteControladora(SalvarObjetoRequest<ClienteControladoraInfo> pParametros)
        {
            if (pParametros.Objeto.IdClienteControladora > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ClienteControladoraInfo> Salvar(SalvarObjetoRequest<ClienteControladoraInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_controladora_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomerazaosocial", DbType.String, pParametros.Objeto.DsNomeRazaoSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_cliente_controlada", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<ClienteControladoraInfo> response = new SalvarEntidadeResponse<ClienteControladoraInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_cliente_controlada"].Value)
                    };
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ClienteControladoraInfo> Atualizar(SalvarObjetoRequest<ClienteControladoraInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_controladora_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomerazaosocial", DbType.String, pParametros.Objeto.DsNomeRazaoSocial);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_controlada", DbType.Int32, pParametros.Objeto.IdClienteControladora);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteControladoraInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClienteControladoraInfo> RemoverClienteControladora(RemoverEntidadeRequest<ClienteControladoraInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_controladora_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_controlada", DbType.Int32, pParametros.Objeto.IdClienteControladora);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteControladoraInfo> response = new RemoverEntidadeResponse<ClienteControladoraInfo>()
                {
                    lStatus = true
                };
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);

                return response;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir, ex);
                throw ex;
            }
        }

        private static ClienteControladoraInfo CriarClienteControladora(DataRow linha)
        {
            ClienteControladoraInfo lClienteControladoraInfo = new ClienteControladoraInfo();

            lClienteControladoraInfo.DsCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteControladoraInfo.DsNomeRazaoSocial = linha["ds_nomerazaosocial"].DBToString();
            lClienteControladoraInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteControladoraInfo.IdClienteControladora = linha["id_cliente_controlada"].DBToInt32();

            return lClienteControladoraInfo;

        }

        public static ConsultarObjetosResponse<ClienteControladoraInfo> ConsultarClienteControladora(ConsultarEntidadeRequest<ClienteControladoraInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteControladoraInfo> resposta =
                    new ConsultarObjetosResponse<ClienteControladoraInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdClienteControladora);
                pParametros.Condicoes.Add(info);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_controladora_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_cliente_controlada"] = (lDataTable.Rows[i]["id_cliente_controlada"]).DBToInt32();
                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["ds_nomerazaosocial"] = (lDataTable.Rows[i]["ds_nomerazaosocial"]).DBToString();
                            linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();

                            resposta.Resultado.Add(CriarClienteControladora(linha));
                        }

                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }
        public static ReceberObjetoResponse<ClienteControladoraInfo> ReceberClienteControladora(ReceberEntidadeRequest<ClienteControladoraInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteControladoraInfo> resposta =
                    new ReceberObjetoResponse<ClienteControladoraInfo>();

                resposta.Objeto = new ClienteControladoraInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_controladora_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_controlada", DbType.Int32, pParametros.Objeto.IdClienteControladora);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdClienteControladora = (lDataTable.Rows[0]["id_cliente_controlada"]).DBToInt32();
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.DsNomeRazaoSocial = (lDataTable.Rows[0]["ds_nomerazaosocial"]).DBToString();
                        resposta.Objeto.DsCpfCnpj = (lDataTable.Rows[0]["ds_cpfcnpj"]).DBToString();
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(ClienteControladoraInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteControladoraInfo> lEntrada = new ReceberEntidadeRequest<ClienteControladoraInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteControladoraInfo> lRetorno = ReceberClienteControladora(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
