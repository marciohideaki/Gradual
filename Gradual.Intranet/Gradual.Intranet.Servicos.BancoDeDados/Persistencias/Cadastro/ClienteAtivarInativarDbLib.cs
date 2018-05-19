using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Contratos.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {

        private static void LogarModificacao(ClienteAtivarInativarInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ClienteAtivarInativarInfo> lEntrada = new ReceberEntidadeRequest<ClienteAtivarInativarInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ClienteAtivarInativarInfo> lRetorno = ReceberClienteAtivarInativar(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }

        public static ReceberObjetoResponse<ClienteAtivarInativarInfo> ReceberClienteAtivarInativar(ReceberEntidadeRequest<ClienteAtivarInativarInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ClienteAtivarInativarInfo> lResposta = new ReceberObjetoResponse<ClienteAtivarInativarInfo>();

                lResposta.Objeto = new ClienteAtivarInativarInfo();
                lResposta.Objeto.IdCliente = pParametros.Objeto.IdCliente;

                //pegar do novo campo de Cliente
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_ativo_cliger_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                    if (lDataTable.Rows.Count == 1)
                    {
                        lResposta.Objeto.StClienteGeralAtivo = lDataTable.Rows[0]["st_ativo_cliger"].DBToBoolean();
                        lResposta.Objeto.StLoginAtivo = lDataTable.Rows[0]["st_ativo"].DBToBoolean();
                        lResposta.Objeto.StHbAtivo = lDataTable.Rows[0]["st_ativo_hb"].DBToBoolean();
                        lResposta.Objeto.DtUltimaAtualizacao = lDataTable.Rows[0]["dt_ativacaoinativacao"].DBToDateTime();
                    }
                    else
                    {
                        lResposta.Objeto.StClienteGeralAtivo = false;
                        lResposta.Objeto.StLoginAtivo = false;
                        lResposta.Objeto.DtUltimaAtualizacao = DateTime.MinValue;
                    }
                }

                lResposta.Objeto.Contas = new List<ClienteAtivarInativarContasInfo>();


                //Listando todas as contas do Cliente
                ConsultarObjetosResponse<ClienteContaInfo> lContas =
                                                            ConsultarClienteConta(
                                                            new ConsultarEntidadeRequest<ClienteContaInfo>()
                                                            {
                                                                Objeto = new ClienteContaInfo()
                                                                {
                                                                    IdCliente = pParametros.Objeto.IdCliente
                                                                }
                                                                ,
                                                                DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado
                                                                ,
                                                                IdUsuarioLogado = pParametros.IdUsuarioLogado
                                                            }
                                                            );


                //Montando Retorno

                //passando por todas as contas
                foreach (ClienteContaInfo item in lContas.Resultado)
                {

                    //Filtrando para ver se já existe alguma linha para a conta atual
                    var Records = (from a in lResposta.Objeto.Contas
                                   where a.CdCodigo == item.CdCodigo
                                   select a).ToList();

                    //Se já existe uma linha com o Código, atribuir a atividade atual
                    if (Records.Count() == 1)
                    {
                        switch (item.CdSistema)
                        {
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.BOL:
                                Records[0].Bovespa = item;
                                break;
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.BMF:
                                Records[0].Bmf = item;
                                break;
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.CUS:
                                Records[0].Custodia = item;
                                break;
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.CC:
                                Records[0].CC = item;
                                break;
                            default:
                                break;
                        }
                    }
                    else //Se não existe uma linha com o Código, criar a linha e atribuir a atividade atual
                    {
                        switch (item.CdSistema)
                        {
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.BOL:
                                lResposta.Objeto.Contas.Add(new ClienteAtivarInativarContasInfo()
                                {
                                    CdCodigo = item.CdCodigo.DBToInt32(),
                                    Bovespa = item
                                });
                                break;
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.BMF:
                                lResposta.Objeto.Contas.Add(new ClienteAtivarInativarContasInfo()
                                {
                                    CdCodigo = item.CdCodigo.DBToInt32(),
                                    Bmf = item
                                });
                                break;
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.CUS:
                                lResposta.Objeto.Contas.Add(new ClienteAtivarInativarContasInfo()
                                {
                                    CdCodigo = item.CdCodigo.DBToInt32(),
                                    Custodia = item
                                });
                                break;
                            case Gradual.Intranet.Contratos.Dados.Enumeradores.eAtividade.CC:
                                lResposta.Objeto.Contas.Add(new ClienteAtivarInativarContasInfo()
                                {
                                    CdCodigo = item.CdCodigo.DBToInt32(),
                                    CC = item
                                });
                                break;
                            default:
                                break;
                        }
                    }
                }

                return lResposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ClienteAtivarInativarInfo> SalvarClienteAtivarInativar(SalvarObjetoRequest<ClienteAtivarInativarInfo> pParametros)
        {
            try
            {

                ReceberObjetoResponse<ClienteInfo> lCliente = ReceberCliente(new ReceberEntidadeRequest<ClienteInfo>()
                                                               {
                                                                   Objeto = new ClienteInfo()
                                                                   {
                                                                       IdCliente = pParametros.Objeto.IdCliente
                                                                   },
                                                                   IdUsuarioLogado = pParametros.IdUsuarioLogado,
                                                                   DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado
                                                               });

                DbConnection conn;
                DbTransaction trans;
                Conexao._ConnectionStringName = gNomeConexaoCadastro;
                conn = Conexao.CreateIConnection();
                conn.Open();
                trans = conn.BeginTransaction();
                try
                {

                    //Salvar referente à cliger
                    //pegar do novo campo de Cliente
                    ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                    lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(trans, CommandType.StoredProcedure, "cliente_ativo_cliger_upd_sp"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@st_ativo_cliger", DbType.Boolean, pParametros.Objeto.StClienteGeralAtivo);
                        lAcessaDados.AddInParameter(lDbCommand, "@st_login_ativo", DbType.Boolean, pParametros.Objeto.StLoginAtivo);
                        lAcessaDados.AddInParameter(lDbCommand, "@st_ativo_hb", DbType.Boolean, pParametros.Objeto.StHbAtivo);
                        lAcessaDados.ExecuteNonQuery(lDbCommand, trans);
                    }


                    //Salvando cada uma das contas
                    if (null != pParametros.Objeto.Contas)
                        foreach (ClienteAtivarInativarContasInfo item in pParametros.Objeto.Contas)
                        {

                            SalvarObjetoRequest<ClienteContaInfo> lSalvarConta;
                            if (null != item.Bmf)
                            {
                                lSalvarConta = new SalvarObjetoRequest<ClienteContaInfo>();
                                lSalvarConta.IdUsuarioLogado = pParametros.IdUsuarioLogado;
                                lSalvarConta.DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado;
                                lSalvarConta.Objeto = new ClienteContaInfo();
                                lSalvarConta.Objeto.CdCodigo = item.CdCodigo;
                                lSalvarConta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BMF;
                                lSalvarConta.Objeto.StAtiva = item.Bmf.StAtiva;
                                AtualizarAtivaInativa(trans, lSalvarConta);
                            }
                            if (null != item.Bovespa)
                            {
                                lSalvarConta = new SalvarObjetoRequest<ClienteContaInfo>();
                                lSalvarConta.IdUsuarioLogado = pParametros.IdUsuarioLogado;
                                lSalvarConta.DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado;
                                lSalvarConta.Objeto = new ClienteContaInfo();
                                lSalvarConta.Objeto.CdCodigo = item.CdCodigo;
                                lSalvarConta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.BOL;
                                lSalvarConta.Objeto.StAtiva = item.Bovespa.StAtiva;
                                AtualizarAtivaInativa(trans, lSalvarConta);
                            }
                            if (null != item.CC)
                            {
                                lSalvarConta = new SalvarObjetoRequest<ClienteContaInfo>();
                                lSalvarConta.IdUsuarioLogado = pParametros.IdUsuarioLogado;
                                lSalvarConta.DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado;
                                lSalvarConta.Objeto = new ClienteContaInfo();
                                lSalvarConta.Objeto.CdCodigo = item.CdCodigo;
                                lSalvarConta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CC;
                                lSalvarConta.Objeto.StAtiva = item.CC.StAtiva;
                                AtualizarAtivaInativa(trans, lSalvarConta);
                            }
                            if (null != item.Custodia)
                            {
                                lSalvarConta = new SalvarObjetoRequest<ClienteContaInfo>();
                                lSalvarConta.IdUsuarioLogado = pParametros.IdUsuarioLogado;
                                lSalvarConta.DescricaoUsuarioLogado = pParametros.DescricaoUsuarioLogado;
                                lSalvarConta.Objeto = new ClienteContaInfo();
                                lSalvarConta.Objeto.CdCodigo = item.CdCodigo;
                                lSalvarConta.Objeto.CdSistema = Contratos.Dados.Enumeradores.eAtividade.CUS;
                                lSalvarConta.Objeto.StAtiva = item.Custodia.StAtiva;
                                AtualizarAtivaInativa(trans, lSalvarConta);
                            }

                        }

                    //Atualizando no Sinacor
                    SalvarClienteAtivarInativarSinacor(pParametros, lCliente);


                    trans.Commit();

                    LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);


                    return new SalvarEntidadeResponse<ClienteAtivarInativarInfo>();
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

            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }

        }

        public static SalvarEntidadeResponse<ClienteAtivarInativarInfo> SalvarClienteAtivarInativarSinacor(SalvarObjetoRequest<ClienteAtivarInativarInfo> pParametros, ReceberObjetoResponse<ClienteInfo> pCliente)
        {
            DbConnection conn;
            DbTransaction trans;
            Conexao._ConnectionStringName = gNomeConexaoSinacor;
            conn = Conexao.CreateIConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            try
            {

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;


                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(trans, CommandType.StoredProcedure, "prc_AtivarInativar_CliGer_upd"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CPFCGC", DbType.Int64, pCliente.Objeto.DsCpfCnpj.DBToInt64());
                    lAcessaDados.AddInParameter(lDbCommand, "pDT_NASC_FUND", DbType.Date, pCliente.Objeto.DtNascimentoFundacao);
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CON_DEP", DbType.Int32, 1);
                    lAcessaDados.AddInParameter(lDbCommand, "pIN_SITUAC", DbType.AnsiString, pParametros.Objeto.StClienteGeralAtivo ? "A" : "D");
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                if (null != pParametros.Objeto.Contas)
                    foreach (ClienteAtivarInativarContasInfo item in pParametros.Objeto.Contas)
                    {
                        if (null != item.Bmf)
                        {
                            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(trans, CommandType.StoredProcedure, "prc_AtivarInativar_Ativ_upd"))
                            {
                                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, item.Bmf.CdCodigo);
                                lAcessaDados.AddInParameter(lDbCommand, "pAtividade", DbType.AnsiString, "BMF");
                                lAcessaDados.AddInParameter(lDbCommand, "pIN_SITUAC", DbType.AnsiString, item.Bmf.StAtiva ? "A" : "D");
                                lAcessaDados.ExecuteNonQuery(lDbCommand);
                            }
                        }
                        if (null != item.Bovespa)
                        {
                            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(trans, CommandType.StoredProcedure, "prc_AtivarInativar_Ativ_upd"))
                            {
                                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, item.Bovespa.CdCodigo);
                                lAcessaDados.AddInParameter(lDbCommand, "pAtividade", DbType.AnsiString, "BOVESPA");
                                lAcessaDados.AddInParameter(lDbCommand, "pIN_SITUAC", DbType.AnsiString, item.Bovespa.StAtiva ? "A" : "D");
                                lAcessaDados.ExecuteNonQuery(lDbCommand);
                            }
                        }
                        if (null != item.CC)
                        {
                            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(trans, CommandType.StoredProcedure, "prc_AtivarInativar_Ativ_upd"))
                            {
                                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, item.CC.CdCodigo);
                                lAcessaDados.AddInParameter(lDbCommand, "pAtividade", DbType.AnsiString, "CC");
                                lAcessaDados.AddInParameter(lDbCommand, "pIN_SITUAC", DbType.AnsiString, item.CC.StAtiva ? "A" : "D");
                                lAcessaDados.ExecuteNonQuery(lDbCommand);
                            }
                        }
                        if (null != item.Custodia)
                        {
                            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(trans, CommandType.StoredProcedure, "prc_AtivarInativar_Ativ_upd"))
                            {
                                lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, item.Custodia.CdCodigo);
                                lAcessaDados.AddInParameter(lDbCommand, "pAtividade", DbType.AnsiString, "CUSTODIA");
                                lAcessaDados.AddInParameter(lDbCommand, "pIN_SITUAC", DbType.AnsiString, item.Custodia.StAtiva ? "A" : "D");
                                lAcessaDados.ExecuteNonQuery(lDbCommand);
                            }
                        }
                    }

                trans.Commit();
                return new SalvarEntidadeResponse<ClienteAtivarInativarInfo>();

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


        }

    }
}
