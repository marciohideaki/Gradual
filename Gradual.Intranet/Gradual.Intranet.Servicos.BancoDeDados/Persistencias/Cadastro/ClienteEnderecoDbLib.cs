using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        #region | Métodos CRUD

        public static ReceberObjetoResponse<ClienteEnderecoDeCustodiaInfo> ReceberClienteEnderecoDeCustodia(ReceberEntidadeRequest<ClienteEnderecoDeCustodiaInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<ClienteEnderecoDeCustodiaInfo>();
            lRetorno.Objeto = new ClienteEnderecoDeCustodiaInfo();

            var lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sinacor_sel_ende_cus"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "CPF", DbType.Int64, Int64.Parse(pParametros.Objeto.ConsultaCpfCnpj.Replace("-", "").Replace(".", "").Replace("/", "")));
                lAcessaDados.AddInParameter(lDbCommand, "NASCIMENTO", DbType.Date, pParametros.Objeto.ConsultaDataDeNascimento);
                lAcessaDados.AddInParameter(lDbCommand, "DEPENDENTE", DbType.Int16, pParametros.Objeto.ConsultaCondicaoDeDePendente);
                
                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && null != lDataTable.Rows && lDataTable.Rows.Count > 0)
                {
                    lRetorno.Objeto.DsBairro = lDataTable.Rows[0]["nm_bairro"].DBToString();
                    lRetorno.Objeto.NrCep = lDataTable.Rows[0]["cep"].DBToInt32();
                    lRetorno.Objeto.DsCidade = lDataTable.Rows[0]["nm_cidade"].DBToString();
                    lRetorno.Objeto.DsComplemento = lDataTable.Rows[0]["nm_comp_ende"].DBToString();
                    lRetorno.Objeto.DsLogradouro = lDataTable.Rows[0]["nm_logradouro"].DBToString();
                    lRetorno.Objeto.CdUf = lDataTable.Rows[0]["sg_estado"].DBToString();
                    lRetorno.Objeto.DsNumero = lDataTable.Rows[0]["nr_predio"].DBToString();
                }
            }

            return lRetorno;
        }

        public static ReceberObjetoResponse<ClienteEnderecoInfo> ReceberClienteEndereco(ReceberEntidadeRequest<ClienteEnderecoInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteEnderecoInfo> resposta =
                    new ReceberObjetoResponse<ClienteEnderecoInfo>();

                resposta.Objeto = new ClienteEnderecoInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_endereco", DbType.Int32, pParametros.Objeto.IdEndereco);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {

                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.IdEndereco = (lDataTable.Rows[0]["id_endereco"]).DBToInt32();
                        resposta.Objeto.IdTipoEndereco = (lDataTable.Rows[0]["id_tipo_endereco"]).DBToInt32();
                        resposta.Objeto.NrCep = (lDataTable.Rows[0]["cd_cep"]).DBToInt32();
                        resposta.Objeto.NrCepExt = (lDataTable.Rows[0]["cd_cep_ext"]).DBToInt32();
                        resposta.Objeto.StPrincipal = bool.Parse(lDataTable.Rows[0]["st_principal"].ToString());
                        resposta.Objeto.CdPais = (lDataTable.Rows[0]["cd_pais"]).DBToString();
                        resposta.Objeto.CdUf = (lDataTable.Rows[0]["cd_uf"]).DBToString();
                        resposta.Objeto.DsBairro = (lDataTable.Rows[0]["ds_bairro"]).DBToString();
                        resposta.Objeto.DsCidade = (lDataTable.Rows[0]["ds_cidade"]).DBToString();
                        resposta.Objeto.DsComplemento = (lDataTable.Rows[0]["ds_complemento"]).DBToString();
                        resposta.Objeto.DsLogradouro = (lDataTable.Rows[0]["ds_logradouro"]).DBToString();
                        resposta.Objeto.DsNumero = (lDataTable.Rows[0]["ds_numero"]).DBToString();
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

        public static SalvarEntidadeResponse<ClienteEnderecoInfo> SalvarClienteEndereco(DbTransaction pTrans, SalvarObjetoRequest<ClienteEnderecoInfo> pParametros)
        {
            return Gravar(pTrans, pParametros);
        }

        public static SalvarEntidadeResponse<ClienteEnderecoInfo> SalvarClienteEndereco(SalvarObjetoRequest<ClienteEnderecoInfo> pParametros)
        {
            if (pParametros.Objeto.IdEndereco > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Gravar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ClienteEnderecoInfo> Gravar(SalvarObjetoRequest<ClienteEnderecoInfo> pParametros)
        {
            SalvarEntidadeResponse<ClienteEnderecoInfo> lRetorno;

            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoCadastro;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {
                lRetorno = Gravar(trans, pParametros);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                trans = null;
                if (!ConnectionState.Closed.Equals(conn.State)) conn.Close();
                conn.Dispose();
                conn = null;
            }
            return lRetorno;
        }

        private static SalvarEntidadeResponse<ClienteEnderecoInfo> Gravar(DbTransaction pTrans, SalvarObjetoRequest<ClienteEnderecoInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "cliente_endereco_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_endereco", DbType.Int32, pParametros.Objeto.IdTipoEndereco);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cep", DbType.Int32, pParametros.Objeto.NrCep);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cep_ext", DbType.Int32, pParametros.Objeto.NrCepExt);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_logradouro", DbType.String, pParametros.Objeto.DsLogradouro);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_complemento", DbType.String, pParametros.Objeto.DsComplemento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_bairro", DbType.String, pParametros.Objeto.DsBairro);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cidade", DbType.String, pParametros.Objeto.DsCidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_uf", DbType.String, pParametros.Objeto.CdUf);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_pais", DbType.String, pParametros.Objeto.CdPais);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numero", DbType.String, pParametros.Objeto.DsNumero);


                    lAcessaDados.AddOutParameter(lDbCommand, "@id_endereco", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<ClienteEnderecoInfo> response = new SalvarEntidadeResponse<ClienteEnderecoInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_endereco"].Value)
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

        private static SalvarEntidadeResponse<ClienteEnderecoInfo> Atualizar(SalvarObjetoRequest<ClienteEnderecoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_endereco", DbType.Int32, pParametros.Objeto.IdTipoEndereco);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@st_principal", DbType.Boolean, pParametros.Objeto.StPrincipal);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cep", DbType.Int32, pParametros.Objeto.NrCep);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_cep_ext", DbType.Int32, pParametros.Objeto.NrCepExt);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_logradouro", DbType.String, pParametros.Objeto.DsLogradouro);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_complemento", DbType.String, pParametros.Objeto.DsComplemento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_bairro", DbType.String, pParametros.Objeto.DsBairro);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_cidade", DbType.String, pParametros.Objeto.DsCidade);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_uf", DbType.String, pParametros.Objeto.CdUf);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_pais", DbType.String, pParametros.Objeto.CdPais);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_numero", DbType.String, pParametros.Objeto.DsNumero);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_endereco", DbType.Int32, pParametros.Objeto.IdEndereco);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteEnderecoInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClienteEnderecoInfo> RemoverClienteEndereco(RemoverEntidadeRequest<ClienteEnderecoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_endereco", DbType.Int32, pParametros.Objeto.IdEndereco);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                }

                RemoverEntidadeResponse<ClienteEnderecoInfo> response = new RemoverEntidadeResponse<ClienteEnderecoInfo>()
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

        public static ConsultarObjetosResponse<ClienteEnderecoInfo> ConsultarClienteEndereco(ConsultarEntidadeRequest<ClienteEnderecoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteEnderecoInfo> resposta =
                    new ConsultarObjetosResponse<ClienteEnderecoInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["ds_bairro"] = (lDataTable.Rows[i]["ds_bairro"]).DBToString();
                            linha["ds_cidade"] = (lDataTable.Rows[i]["ds_cidade"]).DBToString();
                            linha["ds_complemento"] = (lDataTable.Rows[i]["ds_complemento"]).DBToString();
                            linha["ds_logradouro"] = (lDataTable.Rows[i]["ds_logradouro"]).DBToString();
                            linha["ds_numero"] = (lDataTable.Rows[i]["ds_numero"]).DBToString();
                            linha["cd_pais"] = (lDataTable.Rows[i]["cd_pais"]).DBToString();
                            linha["cd_uf"] = (lDataTable.Rows[i]["cd_uf"]).DBToString();
                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_endereco"] = (lDataTable.Rows[i]["id_endereco"]).DBToInt32();
                            linha["id_tipo_endereco"] = (lDataTable.Rows[i]["id_tipo_endereco"]).DBToInt32();
                            linha["cd_cep"] = (lDataTable.Rows[i]["cd_cep"]).DBToInt32();
                            linha["cd_cep_ext"] = (lDataTable.Rows[i]["cd_cep_ext"]).DBToInt32();
                            linha["st_principal"] = bool.Parse(lDataTable.Rows[i]["st_principal"].ToString());

                            resposta.Resultado.Add(CriarRegistroClienteEnderecoInfo(linha));
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

        public static ClienteEnderecoInfo GetClienteEnderecoPrincipal(ClienteInfo pCliente)
        {
            ClienteEnderecoInfo resposta = new ClienteEnderecoInfo();


            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_pri_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pCliente.IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {

                    resposta.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                    resposta.IdEndereco = (lDataTable.Rows[0]["id_endereco"]).DBToInt32();
                    resposta.IdTipoEndereco = (lDataTable.Rows[0]["id_tipo_endereco"]).DBToInt32();
                    resposta.NrCep = (lDataTable.Rows[0]["cd_cep"]).DBToInt32();
                    resposta.NrCepExt = (lDataTable.Rows[0]["cd_cep_ext"]).DBToInt32();
                    resposta.StPrincipal = bool.Parse(lDataTable.Rows[0]["st_principal"].ToString());
                    resposta.CdPais = (lDataTable.Rows[0]["cd_pais"]).DBToString();
                    resposta.CdUf = (lDataTable.Rows[0]["cd_uf"]).DBToString();
                    resposta.DsBairro = (lDataTable.Rows[0]["ds_bairro"]).DBToString();
                    resposta.DsCidade = (lDataTable.Rows[0]["ds_cidade"]).DBToString();
                    resposta.DsComplemento = (lDataTable.Rows[0]["ds_complemento"]).DBToString();
                    resposta.DsLogradouro = (lDataTable.Rows[0]["ds_logradouro"]).DBToString();
                    resposta.DsNumero = (lDataTable.Rows[0]["ds_numero"]).DBToString();
                }
            }

            return resposta;
        }

        private static ClienteEnderecoInfo CriarRegistroClienteEnderecoInfo(DataRow linha)
        {
            ClienteEnderecoInfo lClienteEnderecoInfo = new ClienteEnderecoInfo();


            lClienteEnderecoInfo.DsBairro = linha["ds_bairro"].DBToString();
            lClienteEnderecoInfo.DsCidade = linha["ds_cidade"].DBToString();
            lClienteEnderecoInfo.DsComplemento = linha["ds_complemento"].DBToString();
            lClienteEnderecoInfo.DsLogradouro = linha["ds_logradouro"].DBToString();
            lClienteEnderecoInfo.DsNumero = linha["ds_numero"].DBToString();
            lClienteEnderecoInfo.CdPais = linha["cd_pais"].DBToString();
            lClienteEnderecoInfo.CdUf = linha["cd_uf"].DBToString();
            lClienteEnderecoInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteEnderecoInfo.IdEndereco = linha["id_endereco"].DBToInt32();
            lClienteEnderecoInfo.IdTipoEndereco = linha["id_tipo_endereco"].DBToInt32();
            lClienteEnderecoInfo.NrCep = linha["cd_cep"].DBToInt32();
            lClienteEnderecoInfo.NrCepExt = linha["cd_cep_ext"].DBToInt32();
            lClienteEnderecoInfo.StPrincipal = bool.Parse(linha["st_principal"].ToString());

            return lClienteEnderecoInfo;

        }

        #endregion

        private static void LogarModificacao(ClienteEnderecoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteEnderecoInfo> lEntrada = new ReceberEntidadeRequest<ClienteEnderecoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteEnderecoInfo> lRetorno = ReceberClienteEndereco(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
