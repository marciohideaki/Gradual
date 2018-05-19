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

        public static SalvarEntidadeResponse<ClienteInvestidorNaoResidenteInfo> SalvarClienteInvestidorNaoResidente(SalvarObjetoRequest<ClienteInvestidorNaoResidenteInfo> pParametros)
        {
            if (pParametros.Objeto.IdInvestidorNaoResidente > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ClienteInvestidorNaoResidenteInfo> Salvar(SalvarObjetoRequest<ClienteInvestidorNaoResidenteInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_investidor_naoresidente_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_representantelegal", DbType.String, pParametros.Objeto.DsRepresentanteLegal);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_paisorigem", DbType.String, pParametros.Objeto.CdPaisOrigem);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_custodiante", DbType.String, pParametros.Objeto.DsCustodiante);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_rde", DbType.String, pParametros.Objeto.DsRde);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_codigocvm", DbType.String, pParametros.Objeto.DsCodigoCvm);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomeadiministradorcarteira", DbType.String, pParametros.Objeto.DsNomeAdiministradorCarteira);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_investidor_naoresidente", DbType.Int32, 16);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<ClienteInvestidorNaoResidenteInfo> response = new SalvarEntidadeResponse<ClienteInvestidorNaoResidenteInfo>()
                    {
                        Codigo = (lDbCommand.Parameters["@id_investidor_naoresidente"].Value).DBToInt32()
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

        private static SalvarEntidadeResponse<ClienteInvestidorNaoResidenteInfo> Atualizar(SalvarObjetoRequest<ClienteInvestidorNaoResidenteInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_investidor_naoresidente_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_investidor_naoresidente", DbType.Int32, pParametros.Objeto.IdInvestidorNaoResidente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_representantelegal", DbType.String, pParametros.Objeto.DsRepresentanteLegal);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_paisorigem", DbType.String, pParametros.Objeto.CdPaisOrigem);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_custodiante", DbType.String, pParametros.Objeto.DsCustodiante);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_rde", DbType.String, pParametros.Objeto.DsRde);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_codigocvm", DbType.String, pParametros.Objeto.DsCodigoCvm);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nomeadiministradorcarteira", DbType.String, pParametros.Objeto.DsNomeAdiministradorCarteira);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteInvestidorNaoResidenteInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }

        public static RemoverObjetoResponse<ClienteInvestidorNaoResidenteInfo> RemoverClienteInvestidorNaoResidente(RemoverEntidadeRequest<ClienteInvestidorNaoResidenteInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_investidor_naoresidente_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_investidor_naoresidente", DbType.Int32, pParametros.Objeto.IdInvestidorNaoResidente);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ClienteInvestidorNaoResidenteInfo> response = new RemoverEntidadeResponse<ClienteInvestidorNaoResidenteInfo>()
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

        public static ReceberObjetoResponse<ClienteInvestidorNaoResidenteInfo> ReceberClienteNaoResidente(ReceberEntidadeRequest<ClienteInvestidorNaoResidenteInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteInvestidorNaoResidenteInfo> resposta =
                    new ReceberObjetoResponse<ClienteInvestidorNaoResidenteInfo>();

                resposta.Objeto = new ClienteInvestidorNaoResidenteInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_investidor_naoresidente_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_investidor_naoresidente", DbType.Int32, pParametros.Objeto.IdInvestidorNaoResidente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto.IdInvestidorNaoResidente = (lDataTable.Rows[0]["id_investidor_naoresidente"]).DBToInt32();
                        resposta.Objeto.IdCliente = (lDataTable.Rows[0]["id_cliente"]).DBToInt32();
                        resposta.Objeto.DsRepresentanteLegal = lDataTable.Rows[0]["ds_representantelegal"].DBToString();
                        resposta.Objeto.DsNomeAdiministradorCarteira = (lDataTable.Rows[0]["ds_nomeadiministradorcarteira"]).DBToString();
                        resposta.Objeto.CdPaisOrigem = (lDataTable.Rows[0]["cd_paisorigem"]).DBToString();
                        resposta.Objeto.DsCustodiante = (lDataTable.Rows[0]["ds_custodiante"]).DBToString();
                        resposta.Objeto.DsRde = (lDataTable.Rows[0]["ds_rde"]).DBToInt32();
                        resposta.Objeto.DsCodigoCvm = (lDataTable.Rows[0]["ds_codigocvm"]).DBToInt32();
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

        public static ConsultarObjetosResponse<ClienteInvestidorNaoResidenteInfo> ConsultarClienteNaoResidente(ConsultarEntidadeRequest<ClienteInvestidorNaoResidenteInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteInvestidorNaoResidenteInfo> resposta = new ConsultarObjetosResponse<ClienteInvestidorNaoResidenteInfo>();
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_investidor_naoresidente_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente == 0 ? (object)System.DBNull.Value : pParametros.Objeto.IdCliente);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (null != lDataTable && lDataTable.Rows.Count.CompareTo(0).Equals(1))
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            resposta.Resultado.Add(CriarRegistroNaoResidente(lDataTable.Rows[i]));
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteInvestidorNaoResidenteInfo CriarRegistroNaoResidente(DataRow dr)
        {

            ClienteInvestidorNaoResidenteInfo lRetorno = new ClienteInvestidorNaoResidenteInfo();
            lRetorno.IdInvestidorNaoResidente = (dr["id_investidor_naoresidente"]).DBToInt32();
            lRetorno.IdCliente = (dr["id_cliente"]).DBToInt32();
            lRetorno.DsRepresentanteLegal = dr["ds_representantelegal"].DBToString();
            lRetorno.DsNomeAdiministradorCarteira = (dr["ds_nomeadiministradorcarteira"]).DBToString();
            lRetorno.CdPaisOrigem = (dr["cd_paisorigem"]).DBToString();
            lRetorno.DsCustodiante = (dr["ds_custodiante"]).DBToString();
            lRetorno.DsRde = (dr["ds_rde"]).DBToInt32();
            lRetorno.DsCodigoCvm = (dr["ds_codigocvm"]).DBToInt32();
            return lRetorno;
        }


        private static void LogarModificacao(ClienteInvestidorNaoResidenteInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteInvestidorNaoResidenteInfo> lEntrada = new ReceberEntidadeRequest<ClienteInvestidorNaoResidenteInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteInvestidorNaoResidenteInfo> lRetorno = ReceberClienteNaoResidente(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
