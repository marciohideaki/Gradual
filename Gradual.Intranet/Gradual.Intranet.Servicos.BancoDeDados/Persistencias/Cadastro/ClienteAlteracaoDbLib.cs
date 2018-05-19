using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using System.Data;
using System.Data.Common;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        public static ReceberObjetoResponse<ClienteAlteracaoInfo> ReceberClienteAlteracao(ReceberEntidadeRequest<ClienteAlteracaoInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteAlteracaoInfo> resposta = new ReceberObjetoResponse<ClienteAlteracaoInfo>();

                resposta.Objeto = new ClienteAlteracaoInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "alteracao_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_alteracao", DbType.Int32, pParametros.Objeto.IdAlteracao);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto = MontarLinha(lDataTable.Rows[0]);

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

        public static ConsultarObjetosResponse<ClienteAlteracaoInfo> ConsultarClienteAlteracao(ConsultarEntidadeRequest<ClienteAlteracaoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteAlteracaoInfo> resposta = new ConsultarObjetosResponse<ClienteAlteracaoInfo>();

                resposta.Resultado = new List<ClienteAlteracaoInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "alteracao_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    foreach (DataRow item in lDataTable.Rows)
                    {
                        resposta.Resultado.Add(MontarLinha(item));
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

        private static ClienteAlteracaoInfo MontarLinha(DataRow pDr)
        {

            ClienteAlteracaoInfo resposta = new ClienteAlteracaoInfo();

            resposta.CdTipo = pDr["cd_tipo"].DBToChar();
            resposta.DsDescricao = pDr["Ds_Descricao"].DBToString();
            resposta.DsInformacao = pDr["Ds_Informacao"].DBToString();
            resposta.DtRealizacao = pDr["Dt_Realizacao"].DBToDateTime(Contratos.Dados.Enumeradores.eDateNull.Permite);
            resposta.DtSolicitacao = pDr["Dt_Solicitacao"].DBToDateTime();
            resposta.IdAlteracao = pDr["Id_Alteracao"].DBToInt32();
            resposta.IdCliente = pDr["Id_Cliente"].DBToInt32();
            resposta.IdLoginRealizacao = pDr["Id_Login"].DBToInt32();
            resposta.IdLoginSolicitante = pDr["id_login_solicitante"].DBToInt32();

            //Tratar aqui, pois para assessor é no Sinacor
            if (0 != resposta.IdLoginRealizacao)
            {
                var lLoginRealizacao = ReceberLogin(new ReceberEntidadeRequest<LoginInfo>()
                {
                    DescricaoUsuarioLogado = "Consulta ao Montar Linha de Solicitação de Alteração Cadastral",
                    Objeto = new LoginInfo()
                    {
                        IdLogin = resposta.IdLoginRealizacao
                    }
                });
                if (lLoginRealizacao.Objeto.TpAcesso == Contratos.Dados.Enumeradores.eTipoAcesso.Assessor)
                {
                    //Pegar do Sinacor
                    var lLoginAssessor = ConsultarListaComboSinacor(new ConsultarEntidadeRequest<SinacorListaComboInfo>()
                    {
                        DescricaoUsuarioLogado = "Consulta ao Montar Linha de Solicitação de Alteração Cadastral",
                        Objeto = new SinacorListaComboInfo()
                        {
                            Informacao = Contratos.Dados.Enumeradores.eInformacao.Assessor,
                            Filtro = lLoginRealizacao.Objeto.CdAssessor.ToString()
                        }
                    });
                    resposta.DsLoginRealizacao = lLoginAssessor.Resultado[0].Value;
                }
                else
                    resposta.DsLoginRealizacao = lLoginRealizacao.Objeto.DsNome;
            }

            if (0 != resposta.IdLoginSolicitante)
            {
                var lLoginSolicitante = ReceberLogin(new ReceberEntidadeRequest<LoginInfo>()
                {
                    DescricaoUsuarioLogado = "Consulta ao Montar Linha de Solicitação de Alteração Cadastral",
                    Objeto = new LoginInfo()
                    {
                        IdLogin = resposta.IdLoginSolicitante
                    }
                } );
                if (lLoginSolicitante.Objeto.TpAcesso == Contratos.Dados.Enumeradores.eTipoAcesso.Assessor)
                {
                    //Pegar do Sinacor
                    var lLoginAssessor = ConsultarListaComboSinacor(new ConsultarEntidadeRequest<SinacorListaComboInfo>()
                    {
                        DescricaoUsuarioLogado = "Consulta ao Montar Linha de Solicitação de Alteração Cadastral",
                        Objeto = new SinacorListaComboInfo()
                        {
                            Informacao = Contratos.Dados.Enumeradores.eInformacao.Assessor,
                            Filtro = lLoginSolicitante.Objeto.CdAssessor.ToString()
                        }
                    });
                    resposta.DsLoginSolicitante = lLoginAssessor.Resultado[0].Value;
                }
                else
                    resposta.DsLoginSolicitante = lLoginSolicitante.Objeto.DsNome;
            }
            else
            {
                resposta.DsLoginSolicitante = "Solicitado pelo cliente via Portal";
            }

            return resposta;

        }

        public static SalvarEntidadeResponse<ClienteAlteracaoInfo> SalvarClienteAlteracao(SalvarObjetoRequest<ClienteAlteracaoInfo> pParametros)
        {
            if (pParametros.Objeto.IdAlteracao > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        /// <summary>
        /// Método utilizado apenas para a importação de clientes do DUC.
        /// Nos outros casos, foi montado apenas 1 método com a Transação, mas neste caso, a importação exige um diferêncial, que a alteração pode vir resolvida
        /// </summary>
        /// <param name="pTrans"></param>
        /// <param name="pParametros"></param>
        /// <returns></returns>
        public static SalvarEntidadeResponse<ClienteAlteracaoInfo> SalvarClienteAlteracaoImportacao(DbTransaction pTrans, SalvarObjetoRequest<ClienteAlteracaoInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pTrans, CommandType.StoredProcedure, "alteracao_ins_import_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_tipo", DbType.AnsiString, pParametros.Objeto.CdTipo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_informacao", DbType.AnsiString, pParametros.Objeto.DsInformacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_descricao", DbType.AnsiString, pParametros.Objeto.DsDescricao);
                    lAcessaDados.AddInParameter(lDbCommand, "@Dt_Solicitacao", DbType.DateTime, pParametros.Objeto.DtSolicitacao);
                    if (null != pParametros.Objeto.DtRealizacao)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@Dt_Realizacao", DbType.DateTime, pParametros.Objeto.DtRealizacao);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLoginRealizacao);
                    }

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_alteracao", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand, pTrans);

                    SalvarEntidadeResponse<ClienteAlteracaoInfo> response = new SalvarEntidadeResponse<ClienteAlteracaoInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_alteracao"].Value)
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


        private static SalvarEntidadeResponse<ClienteAlteracaoInfo> Salvar(SalvarObjetoRequest<ClienteAlteracaoInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "alteracao_ins_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_tipo", DbType.AnsiString, pParametros.Objeto.CdTipo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_informacao", DbType.AnsiString, pParametros.Objeto.DsInformacao);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_descricao", DbType.AnsiString, pParametros.Objeto.DsDescricao);
                    	lAcessaDados.AddInParameter(lDbCommand, "@id_login_solicitante", DbType.Int32, pParametros.Objeto.IdLoginSolicitante);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_login_resolucao", DbType.Int32, pParametros.Objeto.IdLoginRealizacao);
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_alteracao", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<ClienteAlteracaoInfo> response = new SalvarEntidadeResponse<ClienteAlteracaoInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_alteracao"].Value)
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

        /// <summary>
        /// Baixa na solicitação de alteração
        /// </summary>
        /// <param name="pParametros">IdAlteraçao e IdLogin</param>
        /// <returns></returns>
        private static SalvarEntidadeResponse<ClienteAlteracaoInfo> Atualizar(SalvarObjetoRequest<ClienteAlteracaoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "alteracao_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_alteracao", DbType.Int32, pParametros.Objeto.IdAlteracao);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_login", DbType.Int32, pParametros.Objeto.IdLoginRealizacao);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ClienteAlteracaoInfo>();

                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(ClienteAlteracaoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteAlteracaoInfo> lEntrada = new ReceberEntidadeRequest<ClienteAlteracaoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteAlteracaoInfo> lRetorno = ReceberClienteAlteracao(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
