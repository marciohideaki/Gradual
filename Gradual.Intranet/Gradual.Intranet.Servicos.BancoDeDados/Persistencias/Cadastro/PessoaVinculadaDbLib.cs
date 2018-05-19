using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        /// <summary>
        /// Lista as pessoas vinculadas
        /// </summary>
        /// <param name="pParametros">Parametros padrão</param>
        /// <returns>Retorna lista de pessoas vinculadas</returns>
        public static ConsultarObjetosResponse<ClientePessoaVinculadaInfo> ConsultarPessoaVinculada(ConsultarEntidadeRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClientePessoaVinculadaInfo> resposta =
                    new ConsultarObjetosResponse<ClientePessoaVinculadaInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_lst_sp"))
                {

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                            linha["ds_nome"] = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                            linha["st_ativo"] = bool.Parse(lDataTable.Rows[i]["st_ativo"].ToString());
                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_pessoa_vinculada"] = (lDataTable.Rows[i]["id_pessoa_vinculada"]).DBToInt32();
                            linha["id_pessoavinculadaresponsavel"] = (lDataTable.Rows[i]["id_pessoavinculadaresponsavel"]).DBToInt32();

                            resposta.Resultado.Add(CriarRegistroPessoaVinculada(linha));
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

        /// <summary>
        /// Seleciona uma pessoa vinculada
        /// </summary>
        /// <param name="pParametros">Parametros padrão</param>
        /// <returns>Retorna pessoa vinculada selecionada</returns>
        public static ConsultarObjetosResponse<ClientePessoaVinculadaPorClienteInfo> ConsultarPessoaVinculadaPorCliente(ConsultarEntidadeRequest<ClientePessoaVinculadaPorClienteInfo> pParametros)
        {
            try
            {
                var lRetorno = new ConsultarObjetosResponse<ClientePessoaVinculadaPorClienteInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);

                pParametros.Condicoes.Add(info);

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_por_cliente_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lRetorno.Resultado.Add(new ClientePessoaVinculadaPorClienteInfo()
                            {
                                DsCpfCnpj = lDataTable.Rows[i]["ds_cpfcnpj"].DBToString(),
                                DsNome = lDataTable.Rows[i]["ds_nome"].DBToString(),
                                IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                StAtivo = lDataTable.Rows[i]["st_ativo"].DBToBoolean(),
                            });
                        }
                    }
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Efetua a seleção apropriada da pessoa vinculada
        /// </summary>
        /// <param name="pParametros">Parametros padrão</param>
        /// <returns>Retorna pessoa vinculada selecionada</returns>
        public static ReceberObjetoResponse<ClientePessoaVinculadaInfo> ReceberPessoaVinculada(ReceberEntidadeRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            ReceberObjetoResponse<ClientePessoaVinculadaInfo> lResposta =
                new ReceberObjetoResponse<ClientePessoaVinculadaInfo>();

            switch (pParametros.Objeto.ReceberPessoaVinculada)
            {
                case Contratos.Dados.Enumeradores.eReceberPessoaVinculada.PorIdPessoaVinculada:
                    lResposta = ReceberPessoaVinculadaPorId(pParametros);
                    break;
                case Contratos.Dados.Enumeradores.eReceberPessoaVinculada.PorIdResponsavel:
                    lResposta = ReceberPessoaVinculadaPorResponsavel(pParametros);
                    break;
            }

            return lResposta;

        }

        /// <summary>
        /// Seleciona a pessoa vinculada filtrada pelo ID
        /// </summary>
        /// <param name="pParametros">Parametros padrão</param>
        /// <returns>Retorna a pessoa vinculada selecionada pelo ID</returns>
        private static ReceberObjetoResponse<ClientePessoaVinculadaInfo> ReceberPessoaVinculadaPorId(ReceberEntidadeRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClientePessoaVinculadaInfo> resposta =
                    new ReceberObjetoResponse<ClientePessoaVinculadaInfo>();

                resposta.Objeto = new ClientePessoaVinculadaInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoa_vinculada", DbType.Int32, pParametros.Objeto.IdPessoaVinculada);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.DsCpfCnpj = (lDataTable.Rows[0]["ds_cpfcnpj"]).DBToString();
                        resposta.Objeto.DsNome = (lDataTable.Rows[0]["ds_nome"]).DBToString();
                        resposta.Objeto.StAtivo = bool.Parse(lDataTable.Rows[0]["st_ativo"].ToString());
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.IdPessoaVinculada = (lDataTable.Rows[0]["id_pessoa_vinculada"]).DBToInt32();
                        resposta.Objeto.IdPessoaVinculadaResponsavel = (lDataTable.Rows[0]["id_pessoavinculadaresponsavel"]).DBToInt32();
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

        /// <summary>
        /// Seleciona a pessoa vinculada filtrando pelo ID da pessoa responsavel
        /// </summary>
        /// <param name="pParametros">Parametros padrão</param>
        /// <returns>Retorna a pessoa seleciona filtrada plo ID da pessoa responsável</returns>
        public static ReceberObjetoResponse<ClientePessoaVinculadaInfo> ReceberPessoaVinculadaPorResponsavel(ReceberEntidadeRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClientePessoaVinculadaInfo> resposta =
                    new ReceberObjetoResponse<ClientePessoaVinculadaInfo>();

                resposta.Objeto = new ClientePessoaVinculadaInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_resp_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoa_vinculada", DbType.Int32, pParametros.Objeto.IdPessoaVinculada);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.DsCpfCnpj = (lDataTable.Rows[0]["ds_cpfcnpj"]).DBToString();
                        resposta.Objeto.DsNome = (lDataTable.Rows[0]["ds_nome"]).DBToString();
                        resposta.Objeto.StAtivo = bool.Parse(lDataTable.Rows[0]["st_ativo"].ToString());
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.IdPessoaVinculada = (lDataTable.Rows[0]["id_pessoa_vinculada"]).DBToInt32();
                        resposta.Objeto.IdPessoaVinculadaResponsavel = (lDataTable.Rows[0]["id_pessoavinculadaresponsavel"]).DBToInt32();
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

        /// <summary>
        /// Cria registro de pessoa vinculada com o datarow passado como parametro
        /// </summary>
        /// <param name="linha">Datarow da entidade a ser criada</param>
        /// <returns>Retorna uma lista de entidade do tipo PessoaVinculada</returns>
        private static ClientePessoaVinculadaPorClienteInfo CriarRegistroPessoaVinculada(DataRow linha)
        {
            ClientePessoaVinculadaPorClienteInfo lPessoaVinculadaInfo = new ClientePessoaVinculadaPorClienteInfo();

            lPessoaVinculadaInfo.DsCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lPessoaVinculadaInfo.DsNome = linha["ds_nome"].DBToString();
            lPessoaVinculadaInfo.StAtivo = bool.Parse(linha["st_ativo"].ToString());
            lPessoaVinculadaInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lPessoaVinculadaInfo.IdPessoaVinculada = linha["id_pessoa_vinculada"].DBToInt32();
            lPessoaVinculadaInfo.IdPessoaVinculadaResponsavel = linha["id_pessoavinculadaresponsavel"].DBToInt32();

            return lPessoaVinculadaInfo;
        }

        /// <summary>
        /// Exclui a pessoa vinculada do banco de dados
        /// </summary>
        /// <param name="pParametros">Parametros padrão</param>
        /// <returns>Retorna true se excluiu co sucesso</returns>
        public static RemoverObjetoResponse<ClientePessoaVinculadaInfo> RemoverPessoaVinculada(RemoverEntidadeRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoa_vinculada", DbType.Int32, pParametros.Objeto.IdPessoaVinculada);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClientePessoaVinculadaInfo> response = new RemoverEntidadeResponse<ClientePessoaVinculadaInfo>()
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

        /// <summary>
        /// Efetua o salvar aprorpiado 
        /// </summary>
        public static SalvarEntidadeResponse<ClientePessoaVinculadaInfo> SalvarPessoaVinculada(SalvarObjetoRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            if (pParametros.Objeto.IdPessoaVinculada > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ClientePessoaVinculadaInfo> Salvar(SalvarObjetoRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativo", DbType.Boolean, pParametros.Objeto.StAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoavinculadaresponsavel", DbType.Int32, pParametros.Objeto.IdPessoaVinculadaResponsavel);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_pessoa_vinculada", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<ClientePessoaVinculadaInfo> response = new SalvarEntidadeResponse<ClientePessoaVinculadaInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_pessoa_vinculada"].Value)
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

        private static SalvarEntidadeResponse<ClientePessoaVinculadaInfo> Atualizar(SalvarObjetoRequest<ClientePessoaVinculadaInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_ativo", DbType.Boolean, pParametros.Objeto.StAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoavinculadaresponsavel", DbType.Int32, pParametros.Objeto.IdPessoaVinculadaResponsavel);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pessoa_vinculada", DbType.Int32, pParametros.Objeto.IdPessoaVinculada);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClientePessoaVinculadaInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(ClientePessoaVinculadaInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClientePessoaVinculadaInfo> lEntrada = new ReceberEntidadeRequest<ClientePessoaVinculadaInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClientePessoaVinculadaInfo> lRetorno = ReceberPessoaVinculadaPorId(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
