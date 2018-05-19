using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.CarteiraRecomendada.lib;
using Gradual.OMS.CarteiraRecomendada.lib.Mensageria;
using Gradual.OMS.CadastroPapeis;
using Gradual.OMS.CadastroPapeis.Lib.Info;
using Gradual.OMS.CadastroPapeis.Lib.Mensageria;
using log4net;

namespace Gradual.OMS.CarteiraRecomendada
{
    public class PersistenciaCarteiraRecomendada
    {
        #region Conexoes
        private const string gNomeConexaoProdutos = "GradualCobrancaProdutos";
        private const string gNomeConexaoCadastro = "Cadastro";
        private const string gNomeConexaoSinacor = "SINACOR";
        private const string gNomeConexao = "Risco";
        private const string gNomeConexaoContaMargem = "CONTAMARGEM";
        #endregion

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ListarResponse Lista()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CarteiraRecomendadaInfo> listaCarteiraRecomendada = new List<CarteiraRecomendadaInfo>();
            ListarResponse response = new ListarResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_carteira_recomendada"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CarteiraRecomendadaInfo carteiraRecomendadaInfo = new CarteiraRecomendadaInfo();
                            carteiraRecomendadaInfo.IdCarteira = (lDataTable.Rows[i]["idCarteiraRecomendada"]).DBToInt32();
                            carteiraRecomendadaInfo.IdProduto = (lDataTable.Rows[i]["idProduto"]).DBToInt32();
                            carteiraRecomendadaInfo.IdTipoCarteira = (lDataTable.Rows[i]["idTipoCarteira"]).DBToInt32();
                            carteiraRecomendadaInfo.DtCarteira = (lDataTable.Rows[i]["dtCarteiraRecomendada"]).DBToDateTime();
                            carteiraRecomendadaInfo.DsCarteira = (lDataTable.Rows[i]["dsCarteira"]).DBToString();
                            carteiraRecomendadaInfo.StAtiva = (lDataTable.Rows[i]["stAtiva"]).DBToChar();
                            listaCarteiraRecomendada.Add(carteiraRecomendadaInfo);
                        }
                        response.Lista = listaCarteiraRecomendada;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ListarComposicaoResponse ListaComposicao(ListarComposicaoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CarteiraRecomendadaComposicaoInfo> listaComposicao = new List<CarteiraRecomendadaComposicaoInfo>();
            ListarComposicaoResponse response = new ListarComposicaoResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_carteira_recomendada_composicao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.idCarteiraRecomendada);
                    lAcessaDados.AddInParameter(lDbCommand, "@idProduto", DbType.Int32, request.idProduto);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CarteiraRecomendadaComposicaoInfo carteiraRecomendadaComposicaoInfo = new CarteiraRecomendadaComposicaoInfo();
                            carteiraRecomendadaComposicaoInfo.IdCarteiraRecomendada = (lDataTable.Rows[i]["idCarteiraRecomendada"]).DBToInt32();
                            carteiraRecomendadaComposicaoInfo.IdAtivo = (lDataTable.Rows[i]["idAtivo"]).DBToString();
                            carteiraRecomendadaComposicaoInfo.Quantidade = (lDataTable.Rows[i]["quantidade"]).DBToInt32();
                            listaComposicao.Add(carteiraRecomendadaComposicaoInfo);
                        }
                        response.listaComposicao = listaComposicao;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool Inclusao(InserirRequest request)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            int idCarteiraRecomendada = 0;
            DbConnection lDbConnection = null;
            DbTransaction lDbTransaction = null;
            string storedProcedure = "";

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;
                lAcessaDados.Conexao._ConnectionStringName = gNomeConexaoProdutos;

                //Cria DBComand atribuindo a storedprocedure 
                storedProcedure = "prc_ins_carteira_recomendada";
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, storedProcedure))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dsCarteira", DbType.AnsiString, request.carteiraRecomendada.DsCarteira);
                    lAcessaDados.AddInParameter(lDbCommand, "@idTipoCarteira", DbType.Int32, request.carteiraRecomendada.IdTipoCarteira);
                    lAcessaDados.AddInParameter(lDbCommand, "@stAtiva", DbType.AnsiString, request.carteiraRecomendada.StAtiva);
                    lAcessaDados.AddOutParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, idCarteiraRecomendada);

                    // Executa Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    idCarteiraRecomendada = lAcessaDados.GetParameterValue(lDbCommand, "@idCarteiraRecomendada").DBToInt32();
                }

                // Cria uma transaction para o loop de inserção dos registros 
                lDbConnection = lAcessaDados.Conexao.CreateIConnection();
                lDbConnection.Open();
                lDbTransaction = lDbConnection.BeginTransaction();

                storedProcedure = "prc_ins_carteira_recomendada_composicao";
                foreach (CarteiraRecomendadaComposicaoInfo composicao in request.listaComposicao)
                {
                    //Cria DBComand atribuindo a storedprocedure 
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, storedProcedure))
                    {
                        lDbCommand.Parameters.Clear();
                        lAcessaDados.AddInParameter(lDbCommand, "@idRenovacao", DbType.Int32, null);
                        lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, idCarteiraRecomendada);
                        lAcessaDados.AddInParameter(lDbCommand, "@idAtivo", DbType.AnsiString, composicao.IdAtivo);
                        lAcessaDados.AddInParameter(lDbCommand, "@quantidade", DbType.Int32, composicao.Quantidade);

                        // Executa Stored procedure
                        lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                    }
                }

                lDbTransaction.Commit();
            }

            catch (Exception ex)
            {
                lDbTransaction.Rollback();

                logger.Info("Ocorreu um erro ao executar " + storedProcedure);
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
            finally
            {
                if (lDbConnection != null)
                {
                    if (lDbConnection.State != ConnectionState.Closed)
                    {
                        lDbConnection.Close();
                        lDbConnection.Dispose();
                    }
                }
            }
            return true;
        }

        public bool Alteracao(AlterarRequest request)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_upd_carteira_recomendada"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.IdCarteiraRecomendada);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsCarteira", DbType.AnsiString, request.DsCarteira);
                    lAcessaDados.AddInParameter(lDbCommand, "@stAtiva", DbType.AnsiString, request.StAtiva);

                    // Executa Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }

            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao executar prc_upd_carteira_recomendada");
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
            return true;
        }

        public int Renovacao(RenovarRequest request)
        {
            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            int idRenovacao = 0;
            DbConnection lDbConnection = null;
            DbTransaction lDbTransaction = null;
            string storedProcedure = "";

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;
                lAcessaDados.Conexao._ConnectionStringName = gNomeConexaoProdutos;

                lDbConnection = lAcessaDados.Conexao.CreateIConnection();
                lDbConnection.Open();
                lDbTransaction = lDbConnection.BeginTransaction();

                // prc_ins_renovacao_carteira_recomendada:
                //  Copiar todos os registros de tbCarteiraRecomendadaComposicao para tbCarteiraRecomendadaComposicaoHistorico,
                //  para o idCarteiraRecomendada informado.
                //  Depois de copiar, gerar novo registro em tbCarteiraRenovacao e retornar o idRenovacao criado.
                //  Se não existir registros, retorna 0 em idRenovacao.
                storedProcedure = "prc_ins_renovacao_carteira_recomendada";
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, storedProcedure))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.idCarteiraRecomendada);
                    lAcessaDados.AddInParameter(lDbCommand, "@dsRenovacao", DbType.AnsiString, request.dsRenovacao);
                    lAcessaDados.AddOutParameter(lDbCommand, "@idRenovacao", DbType.Int32, idRenovacao);

                    // Executa Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);

                    idRenovacao = lAcessaDados.GetParameterValue(lDbCommand, "@idRenovacao").DBToInt32();
                }

                storedProcedure = "prc_ins_carteira_recomendada_composicao";
                foreach (CarteiraRecomendadaComposicaoInfo composicao in request.listaComposicao)
                {
                    // prc_ins_carteira_recomendada_composicao:
                    //  Insere ou atualiza os registros em tbCarteiraRecomendadaComposicao com o idRenovacao criado.
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, storedProcedure))
                    {
                        lDbCommand.Parameters.Clear();
                        if (idRenovacao == 0)
                            lAcessaDados.AddInParameter(lDbCommand, "@idRenovacao", DbType.Int32, null);
                        else
                            lAcessaDados.AddInParameter(lDbCommand, "@idRenovacao", DbType.Int32, idRenovacao);
                        lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.idCarteiraRecomendada);
                        lAcessaDados.AddInParameter(lDbCommand, "@idAtivo", DbType.AnsiString, composicao.IdAtivo);
                        lAcessaDados.AddInParameter(lDbCommand, "@quantidade", DbType.Int32, composicao.Quantidade);

                        // Executa Stored procedure
                        lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                    }
                }

                if (idRenovacao > 0)
                {
                    // prc_del_carteira_recomendada_composicao:
                    //  Os registros em tbCarteiraRecomendadaComposicao com não foram atualizados com o idRenovacao criado, são removidos.
                    storedProcedure = "prc_del_carteira_recomendada_composicao";
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(lDbTransaction, CommandType.StoredProcedure, storedProcedure))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@idRenovacao", DbType.Int32, idRenovacao);
                        lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.idCarteiraRecomendada);

                        // Executa Stored procedure
                        lAcessaDados.ExecuteNonQuery(lDbCommand, lDbTransaction);
                    }
                }

                lDbTransaction.Commit();
            }

            catch (Exception ex)
            {
                lDbTransaction.Rollback();

                logger.Info("Ocorreu um erro ao executar " + storedProcedure);
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
            finally
            {
                if (lDbConnection != null)
                {
                    if (lDbConnection.State != ConnectionState.Closed)
                    {
                        lDbConnection.Close();
                        lDbConnection.Dispose();
                    }
                }
            }
            return idRenovacao;
        }

        public ListarClienteResponse ListaCliente(ListarClienteRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CarteiraRecomendadaClienteInfo> lista = new List<CarteiraRecomendadaClienteInfo>();
            ListarClienteResponse response = new ListarClienteResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_carteira_recomendada_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, request.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CarteiraRecomendadaClienteInfo carteiraRecomendadaClienteInfo = new CarteiraRecomendadaClienteInfo();

                            carteiraRecomendadaClienteInfo.IdCarteira = (lDataTable.Rows[i]["idCarteiraRecomendada"]).DBToInt32();
                            carteiraRecomendadaClienteInfo.IdProduto = (lDataTable.Rows[i]["idProduto"]).DBToInt32();
                            carteiraRecomendadaClienteInfo.DsCarteira = (lDataTable.Rows[i]["dsCarteira"]).DBToString();
                            carteiraRecomendadaClienteInfo.PermiteAdesao = false;
                            carteiraRecomendadaClienteInfo.PermiteRenovacao = false;

                            DateTime dtRenovacao = (lDataTable.Rows[i]["dtRenovacao"]).DBToDateTime();
                            if (dtRenovacao == DateTime.MinValue)
                                carteiraRecomendadaClienteInfo.DtCarteira = (lDataTable.Rows[i]["dtCarteiraRecomendada"]).DBToDateTime();
                            else
                                carteiraRecomendadaClienteInfo.DtCarteira = dtRenovacao;

                            int idRenovacao = (lDataTable.Rows[i]["idRenovacao"]).DBToInt32();
                            int idConfirmacao = (lDataTable.Rows[i]["idConfirmacao"]).DBToInt32();
                            int idRenovacaoCliente = (lDataTable.Rows[i]["idRenovacaoCliente"]).DBToInt32();

                            if (idConfirmacao == 0)
                                carteiraRecomendadaClienteInfo.PermiteAdesao = true;
                            else
                                if (idRenovacao != 0)
                                    if (idRenovacaoCliente == 0 || idRenovacao > idRenovacaoCliente)
                                        carteiraRecomendadaClienteInfo.PermiteRenovacao = true;

                            lista.Add(carteiraRecomendadaClienteInfo);
                        }
                        response.lista = lista;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ListarComposicaoClienteResponse ListaComposicaoCliente(ListarComposicaoClienteRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CarteiraRecomendadaComposicaoInfo> listaComposicaoNova = new List<CarteiraRecomendadaComposicaoInfo>();
            List<CarteiraRecomendadaComposicaoInfo> listaComposicaoAtual = new List<CarteiraRecomendadaComposicaoInfo>();
            ListarComposicaoClienteResponse response = new ListarComposicaoClienteResponse();
            string storedProcedure = null;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                storedProcedure = "prc_lst_carteira_recomendada_composicao";
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, storedProcedure))
                {
                    if (request.idCarteiraRecomendada != 0)
                        lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.idCarteiraRecomendada);
                    if (request.idProduto != 0)
                        lAcessaDados.AddInParameter(lDbCommand, "@idProduto", DbType.Int32, request.idProduto);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CarteiraRecomendadaComposicaoInfo carteiraRecomendadaComposicaoInfo = new CarteiraRecomendadaComposicaoInfo();
                            carteiraRecomendadaComposicaoInfo.IdCarteiraRecomendada = (lDataTable.Rows[i]["idCarteiraRecomendada"]).DBToInt32(); 
                            carteiraRecomendadaComposicaoInfo.IdAtivo = (lDataTable.Rows[i]["idAtivo"]).DBToString();
                            carteiraRecomendadaComposicaoInfo.Quantidade = (lDataTable.Rows[i]["quantidade"]).DBToInt32();
                            listaComposicaoNova.Add(carteiraRecomendadaComposicaoInfo);
                        }
                        response.listaComposicaoNova = listaComposicaoNova;
                    }
                }

                storedProcedure = "prc_lst_carteira_recomendada_composicao_anterior";
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, storedProcedure))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, request.idCliente);
                    if (request.idCarteiraRecomendada != 0)
                        lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.idCarteiraRecomendada);
                    if (request.idProduto != 0)
                        lAcessaDados.AddInParameter(lDbCommand, "@idProduto", DbType.Int32, request.idProduto);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CarteiraRecomendadaComposicaoInfo carteiraRecomendadaComposicaoInfo = new CarteiraRecomendadaComposicaoInfo();
                            carteiraRecomendadaComposicaoInfo.IdCarteiraRecomendada = (lDataTable.Rows[i]["idCarteiraRecomendada"]).DBToInt32();
                            carteiraRecomendadaComposicaoInfo.IdAtivo = (lDataTable.Rows[i]["idAtivo"]).DBToString();
                            carteiraRecomendadaComposicaoInfo.Quantidade = (lDataTable.Rows[i]["quantidade"]).DBToInt32();
                            listaComposicaoAtual.Add(carteiraRecomendadaComposicaoInfo);
                        }
                        response.listaComposicaoAtual = listaComposicaoAtual;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao executar " + storedProcedure);
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
        }

        public ListarComposicaoClienteResponse ListaComposicaoClienteRenovado(ListarComposicaoClienteRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<CarteiraRecomendadaComposicaoInfo> listaComposicaoNova = new List<CarteiraRecomendadaComposicaoInfo>();
            List<CarteiraRecomendadaComposicaoInfo> listaComposicaoAtual = new List<CarteiraRecomendadaComposicaoInfo>();
            ListarComposicaoClienteResponse response = new ListarComposicaoClienteResponse();
            string storedProcedure = null;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                storedProcedure = "prc_lst_carteira_recomendada_composicao_cliente";
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, storedProcedure))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, request.idCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.idCarteiraRecomendada);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        int idRenovacaoNova = (lDataTable.Rows[0]["idRenovacao"]).DBToInt32();
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            CarteiraRecomendadaComposicaoInfo carteiraRecomendadaComposicaoInfo = new CarteiraRecomendadaComposicaoInfo();
                            carteiraRecomendadaComposicaoInfo.IdCarteiraRecomendada = (lDataTable.Rows[i]["idCarteiraRecomendada"]).DBToInt32();
                            carteiraRecomendadaComposicaoInfo.IdAtivo = (lDataTable.Rows[i]["idAtivo"]).DBToString();
                            carteiraRecomendadaComposicaoInfo.Quantidade = (lDataTable.Rows[i]["Quantidade"]).DBToInt32();

                            if (idRenovacaoNova == (lDataTable.Rows[i]["idRenovacao"]).DBToInt32())
                                listaComposicaoNova.Add(carteiraRecomendadaComposicaoInfo);
                            else
                                listaComposicaoAtual.Add(carteiraRecomendadaComposicaoInfo);
                        }
                        response.listaComposicaoNova = listaComposicaoNova;
                        response.listaComposicaoAtual = listaComposicaoAtual;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao executar " + storedProcedure);
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
        }

        public int AdesaoCliente(ClienteProdutoInfo clienteProdutoInfo)
        {
            int idCarteiraRecomendada = 0;

            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ins_adesao_carteira_recomendada"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idProduto", DbType.Int32, clienteProdutoInfo.IdProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "@idPlano", DbType.Int32, clienteProdutoInfo.IdPlano);
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, clienteProdutoInfo.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@stAtivo", DbType.AnsiString, clienteProdutoInfo.StAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ip", DbType.AnsiString, clienteProdutoInfo.IP);
                    lAcessaDados.AddInParameter(lDbCommand, "@descricao", DbType.AnsiString, clienteProdutoInfo.Descricao);
                    lAcessaDados.AddOutParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, idCarteiraRecomendada);

                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    idCarteiraRecomendada = lAcessaDados.GetParameterValue(lDbCommand, "@idCarteiraRecomendada").DBToInt32();
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao executar prc_ins_adesao_carteira_recomendada");
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
            return idCarteiraRecomendada;
        }

        public int RenovacaoCliente(ClienteProdutoInfo clienteProdutoInfo)
        {
            int idCarteiraRecomendada = 0;

            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_upd_adesao_carteira_recomendada"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idProduto", DbType.Int32, clienteProdutoInfo.IdProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, clienteProdutoInfo.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@stAtivo", DbType.AnsiString, clienteProdutoInfo.StAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ip", DbType.AnsiString, clienteProdutoInfo.IP);
                    lAcessaDados.AddOutParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, idCarteiraRecomendada);

                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    idCarteiraRecomendada = lAcessaDados.GetParameterValue(lDbCommand, "@idCarteiraRecomendada").DBToInt32();
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao executar prc_upd_adesao_carteira_recomendada");
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
            return idCarteiraRecomendada;
        }

        public int Cancelamento(ClienteProdutoInfo clienteProdutoInfo)
        {
            int idCarteiraRecomendada = 0;

            //Instancia a biblioteca de acesso a dados
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                //Atribui o nome da conexão para a biblioteca
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                //Cria DBComand atribuindo a storedprocedure 
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_del_adesao_carteira_recomendada"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idProduto", DbType.Int32, clienteProdutoInfo.IdProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, clienteProdutoInfo.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@stAtivo", DbType.AnsiString, clienteProdutoInfo.StAtivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@ip", DbType.AnsiString, clienteProdutoInfo.IP);
                    lAcessaDados.AddInParameter(lDbCommand, "@descricao", DbType.AnsiString, clienteProdutoInfo.Descricao);
                    lAcessaDados.AddOutParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, idCarteiraRecomendada);

                    // Executa s Stored procedure
                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    idCarteiraRecomendada = lAcessaDados.GetParameterValue(lDbCommand, "@idCarteiraRecomendada").DBToInt32();
                }
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao executar prc_del_adesao_carteira_recomendada");
                logger.Info("Descrição do erro:    " + ex.Message);
                throw (ex);
            }
            return idCarteiraRecomendada;
        }

        public ListarAssessoresResponse ListaAssessores()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<AssessorInfo> lista = new List<AssessorInfo>();
            ListarAssessoresResponse response = new ListarAssessoresResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relacao_assessores"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            AssessorInfo assessorInfo = new AssessorInfo();
                            assessorInfo.IdAssessor = (lDataTable.Rows[i]["cd_assessor"]).DBToInt32();
                            assessorInfo.NomeAssessor = (lDataTable.Rows[i]["nm_assessor"]).DBToString();
                            lista.Add(assessorInfo);
                        }
                        response.Lista = lista;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ListarAcompanhamentoResponse ListaAcompanhamento(ListarAcompanhamentoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<AcompanhamentoInfo> lista = new List<AcompanhamentoInfo>();
            ListarAcompanhamentoResponse response = new ListarAcompanhamentoResponse();

            lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

            try
            {
                if (request.IdAssessor != 0)
                {
                    ListarClientesAssessorRequest requestClientesAssessor = new ListarClientesAssessorRequest();

                    requestClientesAssessor.idAssessor = request.IdAssessor;
                    ListarClientesAssessorResponse responseClientesAssessor = ListaClientesAssessor(requestClientesAssessor);

                    foreach (ClienteAssessorInfo clienteAssessor in responseClientesAssessor.Lista)
                    {
                        if (request.IdCliente == 0 || request.IdCliente == clienteAssessor.IdCliente)
                        {
                            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_acompanhamento_carteira_recomendada"))
                            {
                                lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, clienteAssessor.IdCliente);
                                lAcessaDados.AddInParameter(lDbCommand, "@dtAdesaoInicial", DbType.DateTime,
                                    DateTime.Parse(request.DtAdesaoInicial.ToString("yyyy/MM/dd") + " 00:00:00", new CultureInfo("pt-BR", false)));
                                lAcessaDados.AddInParameter(lDbCommand, "@dtAdesaoFinal", DbType.DateTime,
                                    DateTime.Parse(request.DtAdesaoFinal.ToString("yyyy/MM/dd") + " 23:59:59", new CultureInfo("pt-BR", false)));
                                lAcessaDados.AddInParameter(lDbCommand, "@dtRenovacaoInicial", DbType.DateTime, 
                                    (request.DtRenovacaoInicial == null ? 
                                        request.DtRenovacaoInicial :
                                        DateTime.Parse(((DateTime)request.DtRenovacaoInicial).ToString("yyyy/MM/dd") + " 00:00:00", new CultureInfo("pt-BR", false))));
                                lAcessaDados.AddInParameter(lDbCommand, "@dtRenovacaoFinal", DbType.DateTime, 
                                    (request.DtRenovacaoFinal == null ? 
                                        request.DtRenovacaoFinal :
                                        DateTime.Parse(((DateTime)request.DtRenovacaoFinal).ToString("yyyy/MM/dd") + " 23:59:59", new CultureInfo("pt-BR", false))));
                                lAcessaDados.AddInParameter(lDbCommand, "@idCarteira", DbType.Int32, request.IdCarteiraRecomendada);

                                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                                if (null != lDataTable && lDataTable.Rows.Count > 0)
                                {
                                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                                    {
                                        AcompanhamentoInfo acompanhamentoInfo = new AcompanhamentoInfo();
                                        acompanhamentoInfo.IdCliente = clienteAssessor.IdCliente;
                                        acompanhamentoInfo.DsCliente = clienteAssessor.NomeCliente;
                                        acompanhamentoInfo.IdCarteira = (lDataTable.Rows[i]["idCarteira"]).DBToInt32();
                                        acompanhamentoInfo.DsCarteira = (lDataTable.Rows[i]["dsCarteira"]).DBToString();
                                        acompanhamentoInfo.IdAssessor = clienteAssessor.IdAssessor;
                                        acompanhamentoInfo.DtAdesao = (lDataTable.Rows[i]["dtAdesao"]).DBToDateTime();
                                        acompanhamentoInfo.DtRenovacao = (lDataTable.Rows[i]["dtRenovacao"]).DBToDateTime();
                                        if (acompanhamentoInfo.DtRenovacao.Equals(DateTime.MinValue))
                                            acompanhamentoInfo.DtRenovacao = null;
                                        acompanhamentoInfo.IdRenovacao = (lDataTable.Rows[i]["idRenovacao"]).DBToInt32();
                                        acompanhamentoInfo.QtdRenovacoes = (lDataTable.Rows[i]["qtdRenovacoes"]).DBToInt32();

                                        StatusBasketInfo statusBasketInfo = VerificaBasketDisparada(acompanhamentoInfo);

                                        if ((request.StBasketAberto && statusBasketInfo.stBasketAberta) ||
                                            (request.StOrdensExecutadas && statusBasketInfo.stOrdensExecutadas) ||
                                            (!request.StBasketAberto && !request.StOrdensExecutadas))
                                        {
                                            acompanhamentoInfo.StBasketDisparada = statusBasketInfo.stBasketDisparada;
                                            lista.Add(acompanhamentoInfo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_acompanhamento_carteira_recomendada"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, request.IdCliente);

                        lAcessaDados.AddInParameter(lDbCommand, "@dtAdesaoInicial", DbType.DateTime,
                            DateTime.Parse(request.DtAdesaoInicial.ToString("yyyy/MM/dd") + " 00:00:00", new CultureInfo("pt-BR", false)));
                        lAcessaDados.AddInParameter(lDbCommand, "@dtAdesaoFinal", DbType.DateTime,
                            DateTime.Parse(request.DtAdesaoFinal.ToString("yyyy/MM/dd") + " 23:59:59", new CultureInfo("pt-BR", false)));
                        lAcessaDados.AddInParameter(lDbCommand, "@dtRenovacaoInicial", DbType.DateTime,
                            (request.DtRenovacaoInicial == null ?
                                request.DtRenovacaoInicial :
                                DateTime.Parse(((DateTime)request.DtRenovacaoInicial).ToString("yyyy/MM/dd") + " 00:00:00", new CultureInfo("pt-BR", false))));
                        lAcessaDados.AddInParameter(lDbCommand, "@dtRenovacaoFinal", DbType.DateTime,
                            (request.DtRenovacaoFinal == null ?
                                request.DtRenovacaoFinal :
                                DateTime.Parse(((DateTime)request.DtRenovacaoFinal).ToString("yyyy/MM/dd") + " 23:59:59", new CultureInfo("pt-BR", false))));
                        lAcessaDados.AddInParameter(lDbCommand, "@idCarteira", DbType.Int32, request.IdCarteiraRecomendada);

                        DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                ClienteAssessorInfo clienteAssessorRequest = new ClienteAssessorInfo();
                                clienteAssessorRequest.IdCliente = (lDataTable.Rows[i]["idCliente"]).DBToInt32();

                                ClienteAssessorInfo clienteAssessor = ObtemClienteAssessor(clienteAssessorRequest);

                                AcompanhamentoInfo acompanhamentoInfo = new AcompanhamentoInfo();
                                acompanhamentoInfo.IdCliente = (lDataTable.Rows[i]["idCliente"]).DBToInt32();
                                acompanhamentoInfo.DsCliente = clienteAssessor.NomeCliente;
                                acompanhamentoInfo.IdCarteira = (lDataTable.Rows[i]["idCarteira"]).DBToInt32();
                                acompanhamentoInfo.DsCarteira = (lDataTable.Rows[i]["dsCarteira"]).DBToString();
                                acompanhamentoInfo.IdAssessor = clienteAssessor.IdAssessor;
                                acompanhamentoInfo.DtAdesao = (lDataTable.Rows[i]["dtAdesao"]).DBToDateTime();
                                acompanhamentoInfo.DtRenovacao = (lDataTable.Rows[i]["dtRenovacao"]).DBToDateTime();
                                if (acompanhamentoInfo.DtRenovacao.Equals(DateTime.MinValue))
                                    acompanhamentoInfo.DtRenovacao = null;
                                acompanhamentoInfo.IdRenovacao = (lDataTable.Rows[i]["idRenovacao"]).DBToInt32();
                                acompanhamentoInfo.QtdRenovacoes = (lDataTable.Rows[i]["qtdRenovacoes"]).DBToInt32();

                                StatusBasketInfo statusBasketInfo = VerificaBasketDisparada(acompanhamentoInfo);

                                if ((request.StBasketAberto && statusBasketInfo.stBasketAberta) ||
                                    (request.StOrdensExecutadas && statusBasketInfo.stOrdensExecutadas) ||
                                    (!request.StBasketAberto && !request.StOrdensExecutadas))
                                {
                                    acompanhamentoInfo.StBasketDisparada = statusBasketInfo.stBasketDisparada;
                                    lista.Add(acompanhamentoInfo);
                                }
                            }
                        }
                    }

                }
                response.Lista = lista;

                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private StatusBasketInfo VerificaBasketDisparada(AcompanhamentoInfo acompanhamentoInfo)
        {
            StatusBasketInfo statusBasketInfo = new StatusBasketInfo();
            statusBasketInfo.stBasketDisparada = false;
            statusBasketInfo.stBasketAberta = true;
            statusBasketInfo.stOrdensExecutadas = false;

            // Verifica se existem ordens enviadas para a carteira aderida (ou renovada) do cliente
            OrdensEnviadasRequest ordensEnviadasRequest = new OrdensEnviadasRequest();
            ordensEnviadasRequest.IdCliente = acompanhamentoInfo.IdCliente;
            ordensEnviadasRequest.IdCarteiraRecomendada = acompanhamentoInfo.IdCarteira;
            if (acompanhamentoInfo.DtRenovacao == null)
                ordensEnviadasRequest.DtOrdensEnviadas = (DateTime)acompanhamentoInfo.DtAdesao;
            else
                ordensEnviadasRequest.DtOrdensEnviadas = (DateTime)acompanhamentoInfo.DtRenovacao;

            OrdensEnviadasResponse ordensEnviadasResponse = ListaDetalhesAcompanhamento(ordensEnviadasRequest);

            if (ordensEnviadasResponse.Lista.Count > 0)
            {
                Hashtable listaOrdens = new Hashtable();
                foreach (OrdensEnviadasInfo ordens in ordensEnviadasResponse.Lista)
                    if (!listaOrdens.ContainsKey(ordens.IdAtivo))
                        listaOrdens.Add(ordens.IdAtivo, ordens);

                if (acompanhamentoInfo.DtRenovacao == null)
                {
                    ListarComposicaoClienteRequest requestComposicao = new ListarComposicaoClienteRequest();
                    requestComposicao.idCliente = acompanhamentoInfo.IdCliente;
                    requestComposicao.idCarteiraRecomendada = acompanhamentoInfo.IdCarteira;

                    ListarComposicaoClienteResponse listarComposicaoClienteResponse = ListaComposicaoCliente(requestComposicao);

                    List<CarteiraRecomendadaComposicaoInfo> listaComposicao = new List<CarteiraRecomendadaComposicaoInfo>();
                    if (listarComposicaoClienteResponse.listaComposicaoAtual.Count > 0)
                    {
                        foreach (CarteiraRecomendadaComposicaoInfo composicao in listarComposicaoClienteResponse.listaComposicaoAtual)
                            listaComposicao.Add(composicao);
                    }
                    else
                    {
                        foreach (CarteiraRecomendadaComposicaoInfo composicao in listarComposicaoClienteResponse.listaComposicaoNova)
                            listaComposicao.Add(composicao);
                    }

                    int totalOrdens = 0;
                    int totalOrdensExecutadas = 0;
                    foreach (CarteiraRecomendadaComposicaoInfo composicao in listaComposicao)
                    {
                        string msgLog = "";
                        if (listaOrdens.ContainsKey(composicao.IdAtivo))
                        {
                            totalOrdens++;
                            msgLog = "Adesão: Ordem de compra disparada do ativo[" + composicao.IdAtivo +
                                "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                            if (((OrdensEnviadasInfo)listaOrdens[composicao.IdAtivo]).IdOrdemStatus == 2)
                            {
                                totalOrdensExecutadas++;
                                msgLog = "Adesão: Ordem de compra disparada e executada do ativo[" + composicao.IdAtivo +
                                    "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                            }
                        }
                        else
                        {
                            msgLog = "Adesão: Ordem de compra NÃO disparada do ativo[" + composicao.IdAtivo +
                                "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                        }
                        logger.Debug(msgLog);
                    }

                    if (listaComposicao.Count == totalOrdens)
                        statusBasketInfo.stBasketDisparada = true;
                    if (listaComposicao.Count == totalOrdensExecutadas)
                    {
                        statusBasketInfo.stOrdensExecutadas = true;
                        statusBasketInfo.stBasketAberta = false;
                    }
                }
                else
                {
                    // Se for renovação do cliente, verifica se todos os ativos adicionados ou removidos da composição da carteira, 
                    // em relação à composição atual do cliente, teve ordens disparadas
                    ListarComposicaoClienteRequest requestComposicaoCliente = new ListarComposicaoClienteRequest();
                    requestComposicaoCliente.idCarteiraRecomendada = acompanhamentoInfo.IdCarteira;
                    requestComposicaoCliente.idCliente = acompanhamentoInfo.IdCliente;

                    ListarComposicaoClienteResponse listarComposicaoClienteResponse = ListaComposicaoClienteRenovado(requestComposicaoCliente);

                    // Prepara listas de ativos de compra e venda do cliente, referente à renovação
                    List<CarteiraRecomendadaComposicaoInfo> listaCompra = new List<CarteiraRecomendadaComposicaoInfo>();
                    List<CarteiraRecomendadaComposicaoInfo> listaVenda = new List<CarteiraRecomendadaComposicaoInfo>();

                    Hashtable listaAtual = new Hashtable();
                    foreach (CarteiraRecomendadaComposicaoInfo composicao in listarComposicaoClienteResponse.listaComposicaoAtual)
                        listaAtual.Add(composicao.IdAtivo, composicao);

                    Hashtable listaNova = new Hashtable();
                    foreach (CarteiraRecomendadaComposicaoInfo composicao in listarComposicaoClienteResponse.listaComposicaoNova)
                        listaNova.Add(composicao.IdAtivo, composicao);

                    foreach (CarteiraRecomendadaComposicaoInfo composicao in listarComposicaoClienteResponse.listaComposicaoNova)
                        if (!listaAtual.ContainsKey(composicao.IdAtivo))
                            listaCompra.Add(composicao);

                    foreach (CarteiraRecomendadaComposicaoInfo composicao in listarComposicaoClienteResponse.listaComposicaoAtual)
                        if (!listaNova.ContainsKey(composicao.IdAtivo))
                            listaVenda.Add(composicao);

                    int totalOrdens = 0;
                    int totalOrdensExecutadas = 0;
                    foreach (CarteiraRecomendadaComposicaoInfo composicao in listaCompra)
                    {
                        string msgLog = "";
                        if (listaOrdens.ContainsKey(composicao.IdAtivo))
                        {
                            totalOrdens++;
                            msgLog = "Renovação: Ordem de compra disparada do ativo[" + composicao.IdAtivo +
                                "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                            if (((OrdensEnviadasInfo)listaOrdens[composicao.IdAtivo]).IdOrdemStatus == 2)
                            {
                                totalOrdensExecutadas++;
                                msgLog = "Renovação: Ordem de compra disparada e executada do ativo[" + composicao.IdAtivo +
                                    "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                            }
                        }
                        else
                        {
                            msgLog = "Renovação: Ordem de compra NÃO disparada do ativo[" + composicao.IdAtivo +
                                "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                        }
                        logger.Debug(msgLog);
                    }

                    foreach (CarteiraRecomendadaComposicaoInfo composicao in listaVenda)
                    {
                        string msgLog = "";
                        if (listaOrdens.ContainsKey(composicao.IdAtivo))
                        {
                            totalOrdens++;
                            msgLog = "Renovação: Ordem de venda disparada do ativo[" + composicao.IdAtivo +
                                "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                            if (((OrdensEnviadasInfo)listaOrdens[composicao.IdAtivo]).IdOrdemStatus == 2)
                            {
                                totalOrdensExecutadas++;
                                msgLog = "Renovação: Ordem de venda disparada e executada do ativo[" + composicao.IdAtivo +
                                    "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                            }
                        }
                        else
                        {
                            msgLog = "Renovação: Ordem de venda NÃO disparada do ativo[" + composicao.IdAtivo +
                                "] para o cliente[" + acompanhamentoInfo.IdCliente + "] da carteira[" + composicao.IdCarteiraRecomendada + "]";
                        }
                        logger.Debug(msgLog);
                    }

                    if ((listaCompra.Count + listaVenda.Count) == totalOrdens)
                        statusBasketInfo.stBasketDisparada = true;
                    if ((listaCompra.Count + listaVenda.Count) == totalOrdensExecutadas)
                    {
                        statusBasketInfo.stOrdensExecutadas = true;
                        statusBasketInfo.stBasketAberta = false;
                    }
                }
            }
            return statusBasketInfo;
        }

        public OrdensEnviadasResponse ListaDetalhesAcompanhamento(OrdensEnviadasRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<OrdensEnviadasInfo> listaOrdensEnviadas = new List<OrdensEnviadasInfo>();
            OrdensEnviadasResponse response = new OrdensEnviadasResponse();

            DateTime dataInicial = new DateTime(
                request.DtOrdensEnviadas.Year, request.DtOrdensEnviadas.Month, request.DtOrdensEnviadas.Day, 0, 0, 0);
            DateTime dataFinal = new DateTime(
                request.DtOrdensEnviadas.Year, request.DtOrdensEnviadas.Month, request.DtOrdensEnviadas.Day, 23, 59, 59);

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_acompanhamento_carteira_recomendada"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idCliente", DbType.Int32, request.IdCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@idCarteiraRecomendada", DbType.Int32, request.IdCarteiraRecomendada);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtDE", DbType.DateTime, dataInicial);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtAte", DbType.DateTime, dataFinal);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdensEnviadasInfo ordensEnviadasInfo = new OrdensEnviadasInfo();
                            ordensEnviadasInfo.IdOrdem = (lDataTable.Rows[i]["OrderID"]).DBToInt32();
                            ordensEnviadasInfo.IdAtivo = (lDataTable.Rows[i]["Symbol"]).DBToString();
                            ordensEnviadasInfo.Quantidade = (lDataTable.Rows[i]["OrderQty"]).DBToInt32();
                            ordensEnviadasInfo.IdOrdemStatus = (lDataTable.Rows[i]["OrdStatusID"]).DBToInt32();
                            ordensEnviadasInfo.DsOrdemStatus = (lDataTable.Rows[i]["OrderStatusDescription"]).DBToString();
                            ordensEnviadasInfo.DtOrdem = (lDataTable.Rows[i]["registerTime"]).DBToDateTime();
                            listaOrdensEnviadas.Add(ordensEnviadasInfo);
                        }
                        response.Lista = listaOrdensEnviadas;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ClienteAssessorInfo ObtemClienteAssessor(ClienteAssessorInfo clienteAssessor)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            ClienteAssessorInfo clienteAssessorInfo = new ClienteAssessorInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obtem_cliente_assessor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, clienteAssessor.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        clienteAssessorInfo.IdAssessor = (lDataTable.Rows[0]["cd_assessor"]).DBToInt32();
                        clienteAssessorInfo.NomeCliente = (lDataTable.Rows[0]["nm_cliente"]).DBToString();
                    }
                }
                return clienteAssessorInfo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ListarClientesAssessorResponse ListaClientesAssessor(ListarClientesAssessorRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<ClienteAssessorInfo> lista = new List<ClienteAssessorInfo>();
            ListarClientesAssessorResponse response = new ListarClientesAssessorResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_assessor_carteira"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdAssessor", DbType.Int32, request.idAssessor);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ClienteAssessorInfo clienteAssessorInfo = new ClienteAssessorInfo();
                            clienteAssessorInfo.IdAssessor = (lDataTable.Rows[i]["cd_assessor"]).DBToInt32();
                            clienteAssessorInfo.IdCliente = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            clienteAssessorInfo.NomeCliente = (lDataTable.Rows[i]["nm_cliente"]).DBToString();
                            lista.Add(clienteAssessorInfo);
                        }
                        response.Lista = lista;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public CadastroPapeisResponse<CadastroPapelInfo> ObterInformacoesPapeis(CadastroPapeisRequest pParametro)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            CadastroPapeisResponse<CadastroPapelInfo> CadastroPapelResponse = new CadastroPapeisResponse<CadastroPapelInfo>();

            try
            {

                CadastroPapelInfo lRetorno = null;
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_informacao_papel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CODNEG", DbType.String, pParametro.Instrumento.Trim());

                    logger.Info("Solicitação de consulta de papeis enviada para o banco de dados");

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        logger.Info("Instrumento encontrado com sucesso");

                        lRetorno = new CadastroPapelInfo();
                        lRetorno.Instrumento = (lDataTable.Rows[0]["CD_CODNEG"]).DBToString();
                        lRetorno.PapelObjeto = (lDataTable.Rows[0]["CD_TITOBJ"]).DBToString();

                        CadastroPapelInfo _CadLoteNegFatCot = this.ObterLoteFatorCotacao(lRetorno.Instrumento);

                        if (_CadLoteNegFatCot.Instrumento != string.Empty)
                        {
                            //lRetorno.LoteNegociacao = (lDataTable.Rows[0]["NR_LOTNEG"]).DBToString();
                            //lRetorno.FatorCotacao = (lDataTable.Rows[0]["NR_LOTNEG"]).DBToString();

                            lRetorno.LoteNegociacao = _CadLoteNegFatCot.LoteNegociacao;
                            lRetorno.FatorCotacao = _CadLoteNegFatCot.FatorCotacao;
                        }

                        string TipoBolsa = (lDataTable.Rows[0]["BOLSA"]).DBToString();

                        switch (TipoBolsa)
                        {
                            case "BOVESPA":
                                lRetorno.TipoBolsa = CadastroPapeis.Lib.Enum.TipoBolsaEnum.BOVESPA;
                                break;
                            default:
                                lRetorno.TipoBolsa = CadastroPapeis.Lib.Enum.TipoBolsaEnum.BMF;
                                lRetorno.TipoMercado = CadastroPapeis.Lib.Enum.TipoMercadoEnum.FUTURO;
                                break;
                        }

                        string TipoMercado = (lDataTable.Rows[0]["CD_TPMERC"]).DBToString();

                        switch (TipoMercado)
                        {

                            case "FRA":
                                lRetorno.TipoMercado = CadastroPapeis.Lib.Enum.TipoMercadoEnum.FRACIONARIO;
                                break;
                            case "VIS":
                                lRetorno.TipoMercado = CadastroPapeis.Lib.Enum.TipoMercadoEnum.AVISTA;
                                break;
                            case "OPC":
                                lRetorno.TipoMercado = CadastroPapeis.Lib.Enum.TipoMercadoEnum.OPCAO;
                                break;
                            case "OPV":
                                lRetorno.TipoMercado = CadastroPapeis.Lib.Enum.TipoMercadoEnum.OPCAO;
                                break;
                            case "LEI":
                                lRetorno.TipoMercado = CadastroPapeis.Lib.Enum.TipoMercadoEnum.LEILAO;
                                break;

                        }



                        CadastroPapelResponse.Objeto = lRetorno;
                    }
                    else
                    {
                        logger.Info("Instrumento não encontrado");
                    }

                }

                CadastroPapelResponse.DescricaoResposta = "Dados do instrumento <" + pParametro.Instrumento + "> carregados com sucesso !";
                CadastroPapelResponse.StatusResposta = CadastroPapeis.Lib.Enum.CriticaMensagemEnum.OK;

            }
            catch (Exception ex)
            {
                CadastroPapelResponse.DescricaoResposta = ex.Message;
                CadastroPapelResponse.StackTrace = ex.StackTrace;
                CadastroPapelResponse.StatusResposta = CadastroPapeis.Lib.Enum.CriticaMensagemEnum.Exception;
                CadastroPapelResponse.Objeto = null;

                logger.Info("Ocorreu um erro ao efetuar a busca de papeis.");
                logger.Info("Descrição do erro:   " + ex.Message);

            }

            return CadastroPapelResponse;
        }

        public ListarEmailResponse ListaEmails()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<ClienteInfo> listaEmails = new List<ClienteInfo>();
            ListarEmailResponse response = new ListarEmailResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_email_lst_sp"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ClienteInfo clienteEmailInfo = new ClienteInfo();
                            clienteEmailInfo.IdCliente = (lDataTable.Rows[i]["IdCliente"]).DBToInt32();
                            clienteEmailInfo.DsCliente = (lDataTable.Rows[i]["DsCliente"]).DBToString();
                            clienteEmailInfo.DsEmail = (lDataTable.Rows[i]["Email"]).DBToString();
                            listaEmails.Add(clienteEmailInfo);
                        }
                        response.Lista = listaEmails;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public CarteiraRecomendadaInfo ObtemCarteiraRecomendada(int idCarteiraRecomendada)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            CarteiraRecomendadaInfo carteiraRecomendadaInfo = new CarteiraRecomendadaInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;


                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_carteira_recomendada"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "idCarteiraRecomendada", DbType.Int32, idCarteiraRecomendada);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        carteiraRecomendadaInfo.IdCarteira = idCarteiraRecomendada;
                        carteiraRecomendadaInfo.IdProduto = (lDataTable.Rows[0]["idProduto"]).DBToInt32();
                        carteiraRecomendadaInfo.IdTipoCarteira = (lDataTable.Rows[0]["idTipoCarteira"]).DBToInt32();
                        carteiraRecomendadaInfo.DtCarteira = (lDataTable.Rows[0]["dtCarteiraRecomendada"]).DBToDateTime();
                        carteiraRecomendadaInfo.DsCarteira = (lDataTable.Rows[0]["dsCarteira"]).DBToString();
                        carteiraRecomendadaInfo.StAtiva = (lDataTable.Rows[0]["stAtiva"]).DBToChar();
                    }
                }
                return carteiraRecomendadaInfo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ListarClienteRenovacaoResponse ListaClientesRenovacao(ListarClienteRenovacaoRequest request)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            List<ClienteInfo> listaClientes = new List<ClienteInfo>();
            ListarClienteRenovacaoResponse response = new ListarClienteRenovacaoResponse();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoProdutos;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_lst_cliente_renovacao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "idCarteiraRecomendada", DbType.Int32, request.IdCarteiraRecomendada);
                    lAcessaDados.AddInParameter(lDbCommand, "idRenovacao", DbType.Int32, request.IdRenovacao);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            ClienteInfo clienteInfo = new ClienteInfo();
                            clienteInfo.IdCliente = (lDataTable.Rows[i]["idCliente"]).DBToInt32();
                            listaClientes.Add(clienteInfo);
                        }
                        response.lista = listaClientes;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public CadastroPapelInfo ObterLoteFatorCotacao(string Instrumento)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            CadastroPapelInfo lPapelInfo = new CadastroPapelInfo();

            try
            {
                logger.Info("Inicia as chamadas de margem para o dia");

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                logger.Info("Inicia chamada da stored procedure {prc_obter_LoteNegociacao}");
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_LoteNegociacao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodigoInstrumento", DbType.String, Instrumento);

                    logger.Info("Inicia transação com o banco de dados");
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        logger.Info("Foram encontrados: " + lDataTable.Rows.Count.ToString());


                        lPapelInfo.Instrumento = (lDataTable.Rows[0]["cd_codneg"]).DBToString();
                        lPapelInfo.FatorCotacao = (lDataTable.Rows[0]["nr_fatcot"]).DBToString();
                        lPapelInfo.LoteNegociacao = (lDataTable.Rows[0]["nr_lotneg"]).DBToString();
                    }
                    else
                    {
                        logger.Info("Nenhum registro encontrado.");
                    }

                }

                return lPapelInfo;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o banco de dados: " + ex.Message);
                throw (ex);
            }

        }

    }
}



