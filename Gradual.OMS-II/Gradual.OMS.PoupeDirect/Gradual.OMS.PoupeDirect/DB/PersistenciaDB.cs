using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.PoupeDirect.App_Codigo;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using Gradual.OMS.PoupeDirect.Lib.Mensagens;
using Gradual.OMS.PoupeDirect.Lib.Util;
using log4net;
using System.Globalization;

namespace Gradual.OMS.PoupeDirect.DB
{
    public class PersistenciaDB 
    {
        public static string ConexaoSQL
        {
            get { return "DirectRendaFixa"; }
        }

        public static string ConexaoOracle
        {
            get { return "ConexaoSinacor"; }
        }

        public static string ConexaoTrade
        {
            get { return "ConexaoTrade"; }
        }

        public static string ConexaoRisco
        {
            get { return "Risco"; }
        }

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ClienteProdutoResponse InserirAtualizarClienteProduto(ClienteProdutoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();

            lRetorno.ClienteProduto = new ClienteProdutoInfo();

            lRetorno.ClienteProduto = pRequest.ClienteProduto;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_PRODUTO"))
            {
                if (pRequest.ClienteProduto.CodigoClienteProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.String, pRequest.ClienteProduto.CodigoClienteProduto);
                else
                    _AcessaDados.AddOutParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.Int32, 0);

                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"     , DbType.Int32      , pRequest.ClienteProduto.CodigoCliente                             );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ATIVO"       , DbType.String     , pRequest.ClienteProduto.CodigoAtivo                               );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_SOLICITACAO" , DbType.Int32      , pRequest.ClienteProduto.CodigoSolicitacao                         );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO"     , DbType.Int32      , pRequest.ClienteProduto.CodigoProduto                             );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO" , DbType.DateTime   , pRequest.ClienteProduto.DtSolicitacao.Value);

                if (pRequest.ClienteProduto.DtExportacao != null && pRequest.ClienteProduto.DtExportacao != DateTime.MinValue)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_EXPORTACAO", DbType.DateTime, pRequest.ClienteProduto.DtExportacao.Value);

                _AcessaDados.ExecuteNonQuery(_DbCommand);

                pRequest.ClienteProduto.CodigoClienteProduto = _DbCommand.Parameters["@P_ID_CLIENTE_PRODUTO"].Value.DBToInt32();
            }
            return lRetorno;
        }

        public ClienteProdutoResponse InserirClienteProdutoCompleto(ClienteProdutoRequest pRequest)
        {

            logger.Info("Inicio chamada do método InserirClienteProdutoCompleto eeeeeeeeee");

            AcessaDados _AcessaDados = new AcessaDados();
            _AcessaDados.ConnectionStringName = ConexaoSQL;
            _AcessaDados.Conexao._ConnectionStringName = ConexaoSQL;

            DbConnection ConnectionSql;
                        
            ConnectionSql = _AcessaDados.Conexao.CreateIConnection();
            ConnectionSql.Open();

            DbTransaction transacao = ConnectionSql.BeginTransaction();

                                 

            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();

            lRetorno.ClienteProduto = new ClienteProdutoInfo();

            lRetorno.ClienteProduto = pRequest.ClienteProduto;


            try
            {
                using (DbCommand _DbCommand = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_PRODUTO"))// insere na tabela de cliente produto
                {
                    if (pRequest.ClienteProduto.CodigoClienteProduto > 0)
                        _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.String, pRequest.ClienteProduto.CodigoClienteProduto);
                    else
                        _AcessaDados.AddOutParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.Int32, 0);

                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.ClienteProduto.CodigoCliente);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ATIVO", DbType.String, pRequest.ClienteProduto.CodigoAtivo);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_SOLICITACAO", DbType.Int32, pRequest.ClienteProduto.CodigoSolicitacao);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.ClienteProduto.CodigoProduto);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO", DbType.DateTime, pRequest.ClienteProduto.DtSolicitacao.Value);

                    _AcessaDados.ExecuteNonQuery(_DbCommand, transacao);
                    
                    

                    pRequest.ClienteProduto.CodigoClienteProduto = _DbCommand.Parameters["@P_ID_CLIENTE_PRODUTO"].Value.DBToInt32();
                }



                using (DbCommand _DbCommand = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_SOLICITACAO_HISTORICO")) //insere na tabela de historico de solicitação
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.Int32, pRequest.ClienteProduto.CodigoClienteProduto);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_SOLICITACAO", DbType.Int32, pRequest.ClienteProduto.CodigoSolicitacao);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.ClienteProduto.CodigoProduto);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.ClienteProduto.CodigoCliente);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ATIVO", DbType.String, pRequest.ClienteProduto.CodigoAtivo);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DS_HISTORICO", DbType.String, "");
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO", DbType.DateTime, DateTime.Now);


                    _AcessaDados.ExecuteNonQuery(_DbCommand, transacao);

                    
                }


                ProdutoRequest Produto = new ProdutoRequest();
                Produto.Produto = new ProdutoInfo();
                Produto.Produto.CodigoProduto = pRequest.ClienteProduto.CodigoProduto;
                DateTime dtVencimento;
                DateTime dtInicioTrocaPlano;

                ProdutoResponse ProdutoResposta = this.SelecionarProduto(Produto); //seleciona as inforamções do produto

                if (ProdutoResposta.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                {
                    dtVencimento = pRequest.ClienteProduto.DtSolicitacao.Value.AddDays(ProdutoResposta.ListaProduto[0].QuantidadeDiasVencimento);
                    dtInicioTrocaPlano = dtVencimento.AddDays(-ProdutoResposta.ListaProduto[0].QuantidadeDiasRetroTrocaAtivo);
                }
                else
                {
                    throw new Exception(ProdutoResposta.DescricaoResposta);
                }


                using (DbCommand _DbCommand = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_VENCIMENTO"))// insere na tabela de vencimento de produtos 
                {

                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.Int32, pRequest.ClienteProduto.CodigoClienteProduto);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.ClienteProduto.CodigoCliente);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_VENCIMENTO", DbType.DateTime, dtVencimento);
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIO_TROCA_PLANO", DbType.DateTime, dtInicioTrocaPlano);

                    _AcessaDados.ExecuteNonQuery(_DbCommand, transacao);

                    
                }

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao.Rollback();

                logger.Info("ERRO: " + ex.Message);
                logger.Info("STACKTRACE: " + ex.StackTrace);
               
                throw ex;
            }
            finally
            {
                ConnectionSql.Close();
                ConnectionSql.Dispose();
                ConnectionSql = null;
                transacao = null;
            }

            return lRetorno;
        }

        public ClienteSolicitacaoHistoricoResponse InserirClienteSolicitacaoHistorico(ClienteSolicitacaoHistoricoRequest pRequest)
        {
            ClienteSolicitacaoHistoricoResponse lRetorno = new ClienteSolicitacaoHistoricoResponse();

            lRetorno.ClienteSolicitacaoHistorico = new ClienteSolicitacaoHistoricoInfo();

            lRetorno.ClienteSolicitacaoHistorico = pRequest.ClienteSolicitacaoHistorico;

            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_SOLICITACAO_HISTORICO"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO" , DbType.Int32      , pRequest.ClienteSolicitacaoHistorico.CodigoClienteProduto );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_SOLICITACAO"     , DbType.Int32      , pRequest.ClienteSolicitacaoHistorico.CodigoSolicitacao    );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO"         , DbType.Int32      , pRequest.ClienteSolicitacaoHistorico.CodigoProduto        );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"         , DbType.Int32      , pRequest.ClienteSolicitacaoHistorico.CodigoCliente        );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ATIVO"           , DbType.String     , pRequest.ClienteSolicitacaoHistorico.CodigoAtivo          );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_HISTORICO"       , DbType.String     , pRequest.ClienteSolicitacaoHistorico.DsHistórico          );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO"     , DbType.DateTime   , pRequest.ClienteSolicitacaoHistorico.DtSolicitacao.Value        );


                if (pRequest.ClienteSolicitacaoHistorico.DtEfetivacao != null && pRequest.ClienteSolicitacaoHistorico.DtEfetivacao != DateTime.MinValue)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_EFETIVACAO", DbType.DateTime, pRequest.ClienteSolicitacaoHistorico.DtEfetivacao.Value);

                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            return lRetorno; 
        }

        public ResgateResponse InserirAtualizarResgate(ResgateRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ResgateResponse lRetorno = new ResgateResponse();

            lRetorno.Resgate = new ResgateInfo();

            lRetorno.Resgate = pRequest.Resgate;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_INSERIR_REGASTE"))
            {
                if (pRequest.Resgate.CodigoResgate > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_RESGATE", DbType.Int32, pRequest.Resgate.CodigoResgate);

                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"     , DbType.Int32      , pRequest.Resgate.CodigoCliente                        );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO"     , DbType.Int32      , pRequest.Resgate.CodigoProduto                        );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO" , DbType.DateTime   , pRequest.Resgate.DtSolicitacao.Value );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_SOLICITADO"  , DbType.Decimal    , pRequest.Resgate.ValorSolicitado                      );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_STATUS"      , DbType.Int32      , pRequest.Resgate.CodigoStatus                         );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_RESGATE"     , DbType.String     , pRequest.Resgate.DescricaoResgate                     );




                if (pRequest.Resgate.DtEfetivacao != null && pRequest.Resgate.DtEfetivacao != DateTime.MinValue)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_EFETIVACAO", DbType.DateTime, pRequest.Resgate.DtEfetivacao.Value);

                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            return lRetorno;
        }

        public AplicacaoResponse InserirAtualizarAplicacao(AplicacaoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            AplicacaoResponse lRetorno = new AplicacaoResponse();

            lRetorno.Aplicacao = new AplicacaoInfo();

            lRetorno.Aplicacao = pRequest.Aplicacao;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_INSERIR_APLICACAO"))
            {
                if (pRequest.Aplicacao.CodigoAplicacao > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_Aplicacao", DbType.Int32, pRequest.Aplicacao.CodigoAplicacao);

                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"     , DbType.Int32, pRequest.Aplicacao.CodigoCliente                            );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO"     , DbType.Int32, pRequest.Aplicacao.CodigoProduto                            );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO" , DbType.DateTime, pRequest.Aplicacao.DtSolicitacao.Value  );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_SOLICITADO"  , DbType.Decimal, pRequest.Aplicacao.ValorSolicitado                        );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_STATUS"      , DbType.Int32, pRequest.Aplicacao.CodigoStatus                             );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_Aplicacao"   , DbType.String, pRequest.Aplicacao.DescricaoAplicacao                      );




                if (pRequest.Aplicacao.DtEfetivacao != null && pRequest.Aplicacao.DtEfetivacao != DateTime.MaxValue)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_EFETIVACAO", DbType.DateTime, pRequest.Aplicacao.DtEfetivacao.Value);

                    _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            return lRetorno;
        }

        public ClienteVencimentoResponse InserirClienteVencimento(ClienteVencimentoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteVencimentoResponse lRetorno = new ClienteVencimentoResponse();

            lRetorno.ClienteVencimento = new ClienteVencimentoInfo();

            lRetorno.ClienteVencimento = pRequest.ClienteVencimento;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_VENCIMENTO"))
            {
                if (pRequest.ClienteVencimento.IdClienteVencimento > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_VENCIMENTO", DbType.Int32, pRequest.ClienteVencimento.IdClienteVencimento);

                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO"     , DbType.Int32, pRequest.ClienteVencimento.IdClienteProduto                                     );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"             , DbType.Int32, pRequest.ClienteVencimento.IdCliente                                            );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_VENCIMENTO"          , DbType.DateTime, pRequest.ClienteVencimento.DtVencimento.Value         );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIO_TROCA_PLANO"  , DbType.DateTime, pRequest.ClienteVencimento.DtInicio_troca_plano.Value );

                if(pRequest.ClienteVencimento.DtCompra != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_COMPRA", DbType.DateTime, pRequest.ClienteVencimento.DtCompra.Value);    
                
                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            return lRetorno;
        }

        public ClienteMovCCResponse InserirAtualizarClienteMovCC(ClienteMovCCRequest pRequest, DbTransaction transacao, AcessaDados AcessoDados)
        {
            ClienteMovCCResponse lRetorno = new ClienteMovCCResponse();

            lRetorno.ClienteMovCC = new ClienteMovCCInfo();

            lRetorno.ClienteMovCC = pRequest.ClienteMovCC;

            using (DbCommand _DbCommand = AcessoDados.CreateCommand(transacao, CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_MOV_CC"))
            {
                if (pRequest.ClienteMovCC.CodigoClienteMovCC > 0)
                    AcessoDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_MOV_CC", DbType.Int32, pRequest.ClienteMovCC.CodigoClienteMovCC);

                AcessoDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO" , DbType.Int32      , pRequest.ClienteMovCC.CodigoClienteProduto                    );
                AcessoDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"         , DbType.Int32      , pRequest.ClienteMovCC.CodigoCliente                           );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_APLICACAO"       , DbType.Decimal    , pRequest.ClienteMovCC.ValorAplicacao                          );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_CONSUMIDO"       , DbType.Decimal    , pRequest.ClienteMovCC.ValorConsumido                          );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_RESQUICIO"       , DbType.Decimal    , pRequest.ClienteMovCC.ValorResquicio                          );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_CORRETAGEM"      , DbType.Decimal    , pRequest.ClienteMovCC.ValorCorretagem                         );
                AcessoDados.AddInParameter(_DbCommand, "@P_DT_SISTEMA"         , DbType.DateTime   , pRequest.ClienteMovCC.DtSistema.Value  );
                AcessoDados.AddInParameter(_DbCommand, "@P_DT_TRANSACAO"       , DbType.DateTime   , pRequest.ClienteMovCC.DtTransacao.Value);
                AcessoDados.AddInParameter(_DbCommand, "@P_DS_MOV_CC"          , DbType.String     , pRequest.ClienteMovCC.DescMovCC                               );



                AcessoDados.ExecuteNonQuery(_DbCommand);
            }
            return lRetorno;
        }

        public CustodiaValorizadaResponse InserirAtualizarCustodiaValorizada(CustodiaValorizadaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            CustodiaValorizadaResponse lRetorno = new CustodiaValorizadaResponse();

            lRetorno.CustodiaValorizada = new CustodiaValorizadaInfo();

            lRetorno.CustodiaValorizada = pRequest.CustodiaValorizada;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_INSERIR_CUSTODIA_VALORIZADA"))
            {
                if (pRequest.CustodiaValorizada.CodigoCustodiaValorizada > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CUSTODIA_VALORIZADA", DbType.Int32, pRequest.CustodiaValorizada.CodigoCustodiaValorizada);

                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO" , DbType.Int32      , pRequest.CustodiaValorizada.CodigoClienteProduto                          );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"         , DbType.Int32      , pRequest.CustodiaValorizada.CodigoCliente                                 );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO"         , DbType.Int32      , pRequest.CustodiaValorizada.CodigoProduto                                 );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_APLICACAO"       , DbType.Decimal    , pRequest.CustodiaValorizada.ValorAplicacao                                );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_CONSUMIDO"       , DbType.Decimal    , pRequest.CustodiaValorizada.ValorConsumido                                );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_RESQUICIO"       , DbType.Decimal    , pRequest.CustodiaValorizada.ValorResquicio                                );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_CARTEIRA"        , DbType.Decimal    , pRequest.CustodiaValorizada.ValorCarteira                                 );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_CUSTO_MEDIO"     , DbType.Decimal    , pRequest.CustodiaValorizada.ValorCustoMedio                               );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_RENTABILIZACAO"  , DbType.DateTime   , pRequest.CustodiaValorizada.DtRentabilizacao.Value );
                _AcessaDados.AddInParameter(_DbCommand, "@P_QTDE_TITULOS"       , DbType.Int32      , pRequest.CustodiaValorizada.QtdTitulos                                    );
                _AcessaDados.AddInParameter(_DbCommand, "@P_PERC_VARIACAO"      , DbType.Int32      , pRequest.CustodiaValorizada.PercentVariacao                               );
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ATIVO"            , DbType.String     , pRequest.CustodiaValorizada.CodigoAtivo                                  );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_ATIVO"            , DbType.Decimal    , pRequest.CustodiaValorizada.ValorAtivo                                   );
                
                
                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            return lRetorno;
        }


        public CustodiaValorizadaResponse InserirAtualizarCustodiaValorizada(CustodiaValorizadaRequest pRequest, DbTransaction transacao, AcessaDados AcessoDados)
        {
            CustodiaValorizadaResponse lRetorno = new CustodiaValorizadaResponse();

            lRetorno.CustodiaValorizada = new CustodiaValorizadaInfo();

            lRetorno.CustodiaValorizada = pRequest.CustodiaValorizada;

            using (DbCommand _DbCommand = AcessoDados.CreateCommand(transacao, CommandType.StoredProcedure, "PR_INSERIR_CUSTODIA_VALORIZADA"))
            {
                if (pRequest.CustodiaValorizada.CodigoCustodiaValorizada > 0)
                    AcessoDados.AddInParameter(_DbCommand, "@P_ID_CUSTODIA_VALORIZADA", DbType.Int32, pRequest.CustodiaValorizada.CodigoCustodiaValorizada);

                AcessoDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO"  , DbType.Int32      , pRequest.CustodiaValorizada.CodigoClienteProduto                          );
                AcessoDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"          , DbType.Int32      , pRequest.CustodiaValorizada.CodigoCliente                                 );
                AcessoDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO"          , DbType.Int32      , pRequest.CustodiaValorizada.CodigoProduto                                 );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_APLICACAO"        , DbType.Decimal    , pRequest.CustodiaValorizada.ValorAplicacao                                );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_CONSUMIDO"        , DbType.Decimal    , pRequest.CustodiaValorizada.ValorConsumido                                );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_RESQUICIO"        , DbType.Decimal    , pRequest.CustodiaValorizada.ValorResquicio                                );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_CARTEIRA"         , DbType.Decimal    , pRequest.CustodiaValorizada.ValorCarteira                                 );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_CUSTO_MEDIO"      , DbType.Decimal    , pRequest.CustodiaValorizada.ValorCustoMedio                               );
                AcessoDados.AddInParameter(_DbCommand, "@P_DT_RENTABILIZACAO"   , DbType.DateTime   , pRequest.CustodiaValorizada.DtRentabilizacao.Value );
                AcessoDados.AddInParameter(_DbCommand, "@P_QTDE_TITULOS"        , DbType.Int32      , pRequest.CustodiaValorizada.QtdTitulos                                    );
                AcessoDados.AddInParameter(_DbCommand, "@P_PERC_VARIACAO"       , DbType.Decimal    , pRequest.CustodiaValorizada.PercentVariacao                               );
                AcessoDados.AddInParameter(_DbCommand, "@P_ID_ATIVO"            , DbType.String     , pRequest.CustodiaValorizada.CodigoAtivo                                   );
                AcessoDados.AddInParameter(_DbCommand, "@P_VL_ATIVO"            , DbType.Decimal    , pRequest.CustodiaValorizada.ValorAtivo                                    );


                AcessoDados.ExecuteNonQuery(_DbCommand);
            }
            return lRetorno;
        }

        public void InserirClienteVencimentoHistorico()
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            _AcessaDados.Conexao._ConnectionStringName = ConexaoSQL;
            
            DbTransaction transacao;
            
            DbConnection ConnectionSql;
                     
            ConnectionSql = _AcessaDados.Conexao.CreateIConnection();

            ConnectionSql.Open();

            transacao = ConnectionSql.BeginTransaction();

            ClienteVencimentoInfo clienteVencimento;

            try
            {

                ClienteVencimentoResponse ClienteVencido = this.SelecionarClienteVencimentoVencido();//seleciona os clientes com data de vencimento menor que hoje

                if (ClienteVencido.StatusResposta == Library.MensagemResponseStatusEnum.OK)
                {
                    foreach (ClienteVencimentoInfo item in ClienteVencido.ListaClienteVencimento)
                    {
                        using (DbCommand _DbCommand = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_VENCIMENTO_HISTORICO"))
                        {
                            _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_VENCIMENTO"  , DbType.Int32      , item.IdClienteVencimento          );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO"     , DbType.Int32      , item.IdClienteProduto             );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"             , DbType.Int32      , item.IdCliente                    );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_DT_VENCIMENTO"          , DbType.DateTime   , item.DtVencimento.Value           );
                            if (item.DtCompra != null)
                                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_COMPRA"              , DbType.DateTime   , item.DtCompra.Value               );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIO_TROCA_PLANO"  , DbType.DateTime   , item.DtInicio_troca_plano.Value   );

                            _AcessaDados.ExecuteNonQuery(_DbCommand, transacao);
                        }

                        clienteVencimento = this.SelecionarProximoVencimento(item);//Calcula as próximas data de vencimento

                        // atualiza na tabela de vencimento de produtos com o novo vencimento
                        using (DbCommand _DbCommand = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "PR_INSERIR_CLIENTE_VENCIMENTO"))
                        {

                            _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_VENCIMENTO"  , DbType.Int32      , clienteVencimento.IdClienteVencimento         );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO"     , DbType.Int32      , clienteVencimento.IdClienteProduto            );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"             , DbType.Int32      , clienteVencimento.IdCliente                   );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_DT_VENCIMENTO"          , DbType.DateTime   , clienteVencimento.DtVencimento.Value          );
                            _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIO_TROCA_PLANO"  , DbType.DateTime   , clienteVencimento.DtInicio_troca_plano.Value  );

                            _AcessaDados.ExecuteNonQuery(_DbCommand, transacao);
                        }

                    }

                }

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw ex;
            }
            finally
            {
                ConnectionSql.Close();
                ConnectionSql.Dispose();
                ConnectionSql = null;
                transacao = null;
            }
            
        }

        public AplicacaoResponse SelecionarAplicacao(AplicacaoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            AplicacaoResponse lRetorno = new AplicacaoResponse();

            lRetorno.ListaAplicacao = new List<AplicacaoInfo>();

            lRetorno.Aplicacao = pRequest.Aplicacao;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SELECIONAR_APLICACAO"))
            {
                if (pRequest.Aplicacao.CodigoAplicacao > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_Aplicacao", DbType.Int32, pRequest.Aplicacao.CodigoAplicacao);

                if (pRequest.Aplicacao.CodigoCliente > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.Aplicacao.CodigoCliente);

                if (pRequest.Aplicacao.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.Aplicacao.CodigoProduto);

                if (pRequest.Aplicacao.DtSolicitacao != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO", DbType.DateTime, pRequest.Aplicacao.DtSolicitacao.Value);

                if (pRequest.Aplicacao.ValorSolicitado > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VL_SOLICITADO", DbType.Decimal, pRequest.Aplicacao.ValorSolicitado);

                if (pRequest.Aplicacao.CodigoStatus != null && pRequest.Aplicacao.CodigoStatus > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_STATUS", DbType.Int32, pRequest.Aplicacao.CodigoStatus);

                if (pRequest.Aplicacao.DescricaoAplicacao != null && pRequest.Aplicacao.DescricaoAplicacao  != string.Empty)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DS_Aplicacao", DbType.String, pRequest.Aplicacao.DescricaoAplicacao);

                if (pRequest.Aplicacao.DtEfetivacao != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_EFETIVACAO", DbType.DateTime, pRequest.Aplicacao.DtEfetivacao.Value);

                if (pRequest.Aplicacao.DtInicialSolicitacao != null && pRequest.Aplicacao.DtFinalSolicitacao != null)
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIAL_SOLICITACAO" , DbType.DateTime, pRequest.Aplicacao.DtInicialSolicitacao.Value );
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_FINAL_SOLICITACAO"   , DbType.DateTime, pRequest.Aplicacao.DtFinalSolicitacao.Value   );
                }

                DataTable tabela =  _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaAplicacao.Add(this.MontaObjetoAplicacaoInfo(linha));
            }
            return lRetorno;
        }

        public ResgateResponse SelecionarResgate(ResgateRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ResgateResponse lRetorno = new ResgateResponse();

            lRetorno.ListaResgate = new List<ResgateInfo>();

            lRetorno.Resgate = pRequest.Resgate;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SELECIONAR_RESGATE"))
            {
                if (pRequest.Resgate.CodigoResgate > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_RESGATE", DbType.Int32, pRequest.Resgate.CodigoResgate);

                if (pRequest.Resgate.CodigoCliente > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.Resgate.CodigoCliente);

                if (pRequest.Resgate.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.Resgate.CodigoProduto);

                if (pRequest.Resgate.DtSolicitacao != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO", DbType.DateTime, pRequest.Resgate.DtSolicitacao.Value);

                if (pRequest.Resgate.ValorSolicitado > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VL_SOLICITADO", DbType.Decimal, pRequest.Resgate.ValorSolicitado);

                if (pRequest.Resgate.CodigoStatus != null && pRequest.Resgate.CodigoStatus > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_STATUS", DbType.Int32, pRequest.Resgate.CodigoStatus);

                if (pRequest.Resgate.DescricaoResgate != null && pRequest.Resgate.DescricaoResgate  != string.Empty)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DS_Resgate", DbType.String, pRequest.Resgate.DescricaoResgate);

                if (pRequest.Resgate.DtSolicitacao != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_EFETIVACAO", DbType.DateTime, pRequest.Resgate.DtEfetivacao.Value);

                if (pRequest.Resgate.DtInicialSolicitacao != null && pRequest.Resgate.DtFinalSolicitacao != null)
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIAL_SOLICITACAO" , DbType.DateTime, pRequest.Resgate.DtInicialSolicitacao.Value   );
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_FINAL_SOLICITACAO"   , DbType.DateTime, pRequest.Resgate.DtFinalSolicitacao.Value     );
                }

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaResgate.Add(this.MontaObjetoResgateInfo(linha));
            }
            return lRetorno;
        }

        public ClienteProdutoResponse SelecionarClienteProduto(ClienteProdutoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();

            lRetorno.ListaClienteProduto = new List<ClienteProdutoInfo>();

            lRetorno.ClienteProduto = pRequest.ClienteProduto;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SCLIENTE_PRODUTO"))
            {
                if (pRequest.ClienteProduto.CodigoClienteProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.Int32, pRequest.ClienteProduto.CodigoClienteProduto);

                if (pRequest.ClienteProduto.CodigoCliente > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.ClienteProduto.CodigoCliente);

                if (pRequest.ClienteProduto.CodigoAtivo != null && pRequest.ClienteProduto.CodigoAtivo != String.Empty)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ATIVO", DbType.Int32, pRequest.ClienteProduto.CodigoAtivo);

                if (pRequest.ClienteProduto.CodigoSolicitacao > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_SOLICITACAO", DbType.Int32, pRequest.ClienteProduto.CodigoSolicitacao);

                if (pRequest.ClienteProduto.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.ClienteProduto.CodigoProduto);

                if (pRequest.ClienteProduto.DtExportacao != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_EXPORTACAO", DbType.DateTime, pRequest.ClienteProduto.DtExportacao.Value);
                
                if (pRequest.ClienteProduto.DtSolicitacao != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_SOLICITACAO", DbType.DateTime, pRequest.ClienteProduto.DtSolicitacao.Value);
                
                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaClienteProduto.Add(this.MontaObjetoClienteProduto(linha));
            }
            return lRetorno;
        }

        public CustodiaValorizadaResponse SelecionarCustodiaValorizada(CustodiaValorizadaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            CustodiaValorizadaResponse lRetorno = new CustodiaValorizadaResponse();

            lRetorno.ListaCustodiaValorizada = new List<CustodiaValorizadaInfo>();

            lRetorno.CustodiaValorizada = pRequest.CustodiaValorizada;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SELECIONAR_CUSTODIA_VALORIZADA"))
            {
                if (pRequest.CustodiaValorizada.CodigoCustodiaValorizada > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CUSTODIA_VALORIZADA", DbType.Int32, pRequest.CustodiaValorizada.CodigoCustodiaValorizada);

                if (pRequest.CustodiaValorizada.CodigoClienteProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.Int32, pRequest.CustodiaValorizada.CodigoClienteProduto);

                if (pRequest.CustodiaValorizada.CodigoCliente > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.CustodiaValorizada.CodigoCliente);

                if (pRequest.CustodiaValorizada.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.CustodiaValorizada.CodigoProduto);

                if (pRequest.CustodiaValorizada.ValorAplicacao > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VL_APLICACAO", DbType.Decimal, pRequest.CustodiaValorizada.ValorAplicacao);

                if (pRequest.CustodiaValorizada.ValorConsumido > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VL_CONSUMIDO", DbType.Decimal, pRequest.CustodiaValorizada.ValorConsumido);

                if (pRequest.CustodiaValorizada.ValorResquicio > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VL_RESQUICIO", DbType.Decimal, pRequest.CustodiaValorizada.ValorResquicio);

                if (pRequest.CustodiaValorizada.ValorCarteira > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VL_CARTEIRA", DbType.Decimal, pRequest.CustodiaValorizada.ValorCarteira);

                if (pRequest.CustodiaValorizada.ValorCustoMedio > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VL_CUSTO_MEDIO", DbType.Decimal, pRequest.CustodiaValorizada.ValorCustoMedio);
                
                if (pRequest.CustodiaValorizada.DtInicioRentabilizacao != null && pRequest.CustodiaValorizada.DtFimRentabilizacao != null)
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIAL" , DbType.DateTime, pRequest.CustodiaValorizada.DtInicioRentabilizacao.Value) ;
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_FINAL"   , DbType.DateTime, pRequest.CustodiaValorizada.DtFimRentabilizacao.Value)    ;
                }

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaCustodiaValorizada.Add(this.MontaObjetoCustodiaValorizada(linha));
            }
            return lRetorno;
        }

        public ClienteProdutoResponse SelecionarClienteComVencimentoHoje()
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();

            lRetorno.ListaClienteProduto = new List<ClienteProdutoInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SCLIENTE_PRODUTO_MOV"))
            {
                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaClienteProduto.Add(this.MontaObjetoClienteProduto(linha));
            }

            return lRetorno;
        }


        public ClienteProdutoResponse SelecionarClienteRentabilidadeRetroativa(int CodigoCliente, DateTime dataRentabilidade)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteProdutoResponse lRetorno = new ClienteProdutoResponse();

            lRetorno.ListaClienteProduto = new List<ClienteProdutoInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SCLIENTE_PRODUTO_MOV_HISTORICO"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE"     , DbType.Int32      , CodigoCliente     );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_VENCIMENTO"  , DbType.DateTime   , dataRentabilidade );

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaClienteProduto.Add(this.MontaObjetoClienteProduto(linha));
            }

            return lRetorno;
        }

        public MovimentoCCSinacorResponse SelecionarMovimentoCliente(int CodigoCliente, DateTime dataInicio, DateTime dataFim)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoTrade;

            MovimentoCCSinacorResponse lRetorno = new MovimentoCCSinacorResponse();

            lRetorno.ListaMovimentoCCSinacor = new List<MovimentoCCSinacorInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SMOVIMENTO_POUPE_MES"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CD_CLIENTE", DbType.Int32      , CodigoCliente );
                _AcessaDados.AddInParameter(_DbCommand, "DT_INICIO" , DbType.DateTime   , dataInicio    );
                _AcessaDados.AddInParameter(_DbCommand, "DT_FIM"    , DbType.DateTime   , dataFim       );
                

                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaMovimentoCCSinacor.Add(this.MontaObjetoMovimentoCCSinacor(linha));

            }

            return lRetorno;
        }

        public CompraETFResponse SelecionarMovimentoCompraETF(int CodigoCliente, DateTime dataInicio, DateTime dataFim, string CodigoAtivo)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoTrade;

            CompraETFResponse lRetorno = new CompraETFResponse();

            lRetorno.ListaCompraETF = new List<CompraETFInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SMOVIMENTO_COMPRA_ETF"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CD_CLIENTE"    , DbType.Int32      , CodigoCliente );
                _AcessaDados.AddInParameter(_DbCommand, "DT_INICIO"     , DbType.DateTime   , dataInicio    );
                _AcessaDados.AddInParameter(_DbCommand, "DT_FIM"        , DbType.DateTime   , dataFim       );
                _AcessaDados.AddInParameter(_DbCommand, "CD_ATIVO"      , DbType.String     , CodigoAtivo   );


                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaCompraETF.Add(this.MontaObjetoCompraETFInfo(linha));

            }

            return lRetorno;
        }

        public decimal ObterPosicaoFechamentoCotacao(string Instrumento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal ValorCotacao = 0;
            try
            {
                lAcessaDados.ConnectionStringName = ConexaoRisco;
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_fechamento_instrumento"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ativo", DbType.AnsiString, Instrumento);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorCotacao = (lDataTable.Rows[0]["vl_fechamento"]).DBToDecimal();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ValorCotacao;
        }

        public decimal ObterPrecoMedio(int CodigoCliente, DateTime dataInicio, DateTime dataFim, string CodigoAtivo)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ConexaoTrade;

            decimal ValorPrecoMedio = 0;
            try
            {
                
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SBUSCAR_PRECO_MEDIO"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE"    , DbType.Int32      , CodigoCliente );
                    lAcessaDados.AddInParameter(lDbCommand, "DT_INICIO"     , DbType.DateTime   , dataInicio    );
                    lAcessaDados.AddInParameter(lDbCommand, "DT_FIM"        , DbType.DateTime   , dataFim       );
                    lAcessaDados.AddInParameter(lDbCommand, "CD_ATIVO"      , DbType.String     , CodigoAtivo   );

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorPrecoMedio = (lDataTable.Rows[0]["PREC_MED"]).DBToDecimal();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ValorPrecoMedio;
        }

        public int ObterQuantidadeAtivo(int CoidgoCliente, string CodigoPapel)
        {
            int lRetorno = 0;



            return lRetorno;
        }

        public bool VerificarDataBuscaMovimento()
        {
            bool lRetorno = false;



            return lRetorno;
        }

        public RentabilidadeInfo RentabilizarCarteira(int CodigoCliente, int CodigoProduto, Decimal ValorAtivoRentabilizado, Decimal PercentualRentabilizadoPapel)
        {
            Decimal ValorTotalCarteira = 0;
            Decimal ValorPapelHoje = 0;
            Decimal ValorRentabilizado = 0;
            Decimal ValorPercentualRentabilizado = 0;
            Decimal ValorRepresentativo = 0;

            RentabilidadeInfo RentabilidadeTotal = new RentabilidadeInfo();

            List<CustodiaInfo> ListaCustodia = new List<CustodiaInfo>();
            CustodiaInfo custodiaInfo;

            CustodiaValorizadaRequest CustodiaRequest = new CustodiaValorizadaRequest();
            CustodiaRequest.CustodiaValorizada = new CustodiaValorizadaInfo();
            CustodiaRequest.CustodiaValorizada.CodigoCliente = CodigoCliente;
            CustodiaRequest.CustodiaValorizada.CodigoProduto = CodigoProduto;

            CustodiaValorizadaResponse CustodiaResponse = this.SelecionarCustodiaValorizada(CustodiaRequest);

            
            foreach (CustodiaValorizadaInfo Custodia in CustodiaResponse.ListaCustodiaValorizada)
            {
                custodiaInfo = new CustodiaInfo();
                ValorPapelHoje = this.ObterPosicaoFechamentoCotacao(Custodia.CodigoAtivo);
                ValorRentabilizado = ValorPapelHoje * Custodia.QtdTitulos;
                
                custodiaInfo.CodigoAtivo = Custodia.CodigoAtivo;
                custodiaInfo.ValorRentabilizadoAtivo = ValorRentabilizado;
                custodiaInfo.ValorAtivoAnterior = Custodia.ValorAtivo;
                custodiaInfo.PercentVariacaoRentabilizado = ((ValorPapelHoje - custodiaInfo.ValorAtivoAnterior) * 100) / custodiaInfo.ValorAtivoAnterior;  // Valor rentablizado de cada ativo

                ValorTotalCarteira += (ValorRentabilizado + Custodia.ValorResquicio); // Somatório dos valores rentabilizados

                ListaCustodia.Add(custodiaInfo);
                
            }

            ValorTotalCarteira += ValorAtivoRentabilizado; //valor rentabilizado até o momento + o valor rentabilizado deste processamento

            foreach (CustodiaInfo item in ListaCustodia)
            {
                ValorRepresentativo = (item.ValorRentabilizadoAtivo * 100) / ValorTotalCarteira;

                ValorPercentualRentabilizado += (ValorRepresentativo * item.PercentVariacaoRentabilizado) / 100;
            }


            ValorRepresentativo = (ValorAtivoRentabilizado * 100) / ValorTotalCarteira;
            ValorPercentualRentabilizado += (ValorRepresentativo * PercentualRentabilizadoPapel) / 100;

            RentabilidadeTotal.PercentVariacaoTotal = ValorPercentualRentabilizado  ;
            RentabilidadeTotal.ValorTotalCarteira   = ValorTotalCarteira            ;
            
            return RentabilidadeTotal;

        }


        public ProdutoResponse SelecionarProduto(ProdutoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ProdutoResponse lRetorno = new ProdutoResponse();

            lRetorno.ListaProduto = new List<ProdutoInfo>();

            lRetorno.Produto = pRequest.Produto;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SPRODUTO"))
            {
                if (pRequest.Produto.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.Produto.CodigoProduto);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                ProdutoInfo produto;
                foreach (DataRow linha in tabela.Rows)
                {
                    produto = new ProdutoInfo();

                    produto.CodigoProduto                   = linha["ID_PRODUTO"].DBToInt32()                   ;
                    produto.DescricaProduto                 = linha["DS_PRODUTO"].ToString()                    ;
                    produto.PermanenciaMinima               = linha["PERMANENCIA_MINIMA"].DBToInt32()           ;
                    produto.PercentualMulta                 = linha["PERCENT_MULTA"].DBToInt32()                ;
                    produto.QuantidadeDiasRetroTrocaAtivo   = linha["QTDE_DIAS_RETRO_TROCA_PLANO"].DBToInt32()  ;
                    produto.QuantidadeDiasVencimento        = linha["QTDE_DIAS_PARA_VENCIMENTO"].DBToInt32()    ;
                    produto.TermoAdesao                     = linha["DS_TERMO_ADESAO"].ToString()               ;

                    lRetorno.ListaProduto.Add(produto);
                }
            }
            return lRetorno;
        }

        public ProdutoClienteResponse SelecionarProdutoCliente(ProdutoClienteRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ProdutoClienteResponse lRetorno = new ProdutoClienteResponse();

            lRetorno.ListaProdutoCliente = new List<ProdutoClienteInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SPRODUTO_CLIENTE"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.ProdutoCliente.CodigoCliente);

                if(pRequest.ProdutoCliente.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.ProdutoCliente.CodigoProduto);

                if (pRequest.ProdutoCliente.CodigoAtivo != string.Empty)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ATIVO", DbType.String, pRequest.ProdutoCliente.CodigoAtivo);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    lRetorno.ListaProdutoCliente.Add(this.MontaObjetoProdutoClienteInfo(linha));
                }
            }

            return lRetorno;
        }

        public ProdutoClienteResponse SelecionarProdutoClienteOperador(ProdutoClienteRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ProdutoClienteResponse lRetorno = new ProdutoClienteResponse();

            lRetorno.ListaProdutoCliente = new List<ProdutoClienteInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SPRODUTO_CLIENTE_OPERADOR"))
            {
                if (pRequest.ProdutoCliente.CodigoCliente > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.ProdutoCliente.CodigoCliente);

                if (pRequest.ProdutoCliente.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.Int32, pRequest.ProdutoCliente.CodigoProduto);

                if (pRequest.ProdutoCliente.DataInicio != null && pRequest.ProdutoCliente.DataFim != null)
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DATA_INICIO", DbType.DateTime   , pRequest.ProdutoCliente.DataInicio.Value   );
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DATA_FIM"   , DbType.DateTime   , pRequest.ProdutoCliente.DataFim.Value      );
                }

                if (pRequest.ProdutoCliente.CheckOperador != null)
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_CHECK_OPERADOR", DbType.Boolean, pRequest.ProdutoCliente.CheckOperador);
                }

                

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    lRetorno.ListaProdutoCliente.Add(this.MontaObjetoProdutoClienteInfo(linha));
                }
            }

            return lRetorno;
        }

        public StatusResponse SelecionarStatusAplicacaoResgate(StatusRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            StatusInfo statusInfo;

            StatusResponse lRetorno = new StatusResponse();

            lRetorno.ListaStatus = new List<StatusInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_TB_STATUS"))
            {
                if (pRequest.Status.CodigoStatus > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_STATUS", DbType.Int32, pRequest.Status.CodigoStatus);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    statusInfo = new StatusInfo();

                    statusInfo.CodigoStatus = linha["ID_STATUS"].DBToInt32();
                    statusInfo.DescStatus   = linha["DS_STATUS"].ToString() ;


                    lRetorno.ListaStatus.Add(statusInfo);
                }
            }


            return lRetorno;
        }

        public ClienteVencimentoResponse SelecionarClienteVencimento(ClienteVencimentoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteVencimentoResponse lRetorno = new ClienteVencimentoResponse();

            lRetorno.ListaClienteVencimento = new List<ClienteVencimentoInfo>();

            lRetorno.ClienteVencimento = pRequest.ClienteVencimento;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SCLIENTE_VENCIMENTO"))
            {
                if (pRequest.ClienteVencimento.IdClienteVencimento > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_VENCIMENTO", DbType.Int32, pRequest.ClienteVencimento.IdClienteVencimento);

                if (pRequest.ClienteVencimento.IdClienteProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE_PRODUTO", DbType.Int32, pRequest.ClienteVencimento.IdClienteProduto);

                if (pRequest.ClienteVencimento.IdCliente > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.ClienteVencimento.IdCliente);

                if (pRequest.ClienteVencimento.DtVencimento != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_VENCIMENTO", DbType.DateTime, pRequest.ClienteVencimento.DtVencimento.Value);

                if (pRequest.ClienteVencimento.DtInicio_troca_plano != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIO_TROCA_PLANO", DbType.DateTime, pRequest.ClienteVencimento.DtInicio_troca_plano.Value);


                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaClienteVencimento.Add(this.MontaObjetoClienteVencimento(linha));
            }

            return lRetorno;
        }

        public ClienteVencimentoResponse SelecionarClienteVencimentoVencido()
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteVencimentoResponse lRetorno = new ClienteVencimentoResponse();

            lRetorno.ListaClienteVencimento = new List<ClienteVencimentoInfo>();

            ClienteVencimentoInfo ClienteVencimento;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SCLIENTE_VENCIMENTO_VENCIDO"))
            {
                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {

                    ClienteVencimento = new ClienteVencimentoInfo();

                    ClienteVencimento.IdClienteVencimento    = linha["ID_CLIENTE_VENCIMENTO"].DBToInt32()   ;
                    ClienteVencimento.IdClienteProduto       = linha["ID_CLIENTE_PRODUTO"].DBToInt32()      ;
                    ClienteVencimento.IdCliente              = linha["ID_CLIENTE"].DBToInt32()              ;
                    ClienteVencimento.DtVencimento           = linha["DT_VENCIMENTO"].DBToDateTime()        ;
                    ClienteVencimento.DtInicio_troca_plano   = linha["DT_INICIO_TROCA_PLANO"].DBToDateTime();
                    if (linha["DT_COMPRA"].ToString() != "")
                        ClienteVencimento.DtCompra               = linha["DT_COMPRA"].DBToDateTime()            ;


                    lRetorno.ListaClienteVencimento.Add(ClienteVencimento);
                }
            }

            return lRetorno;
        }

        private ClienteVencimentoInfo SelecionarProximoVencimento(ClienteVencimentoInfo pClienteVencimento)
        {
            ClienteVencimentoInfo lRetorno = new ClienteVencimentoInfo();

            lRetorno = pClienteVencimento;

            ClienteProdutoRequest clienteProduto = new ClienteProdutoRequest();
            clienteProduto.ClienteProduto = new ClienteProdutoInfo();
            clienteProduto.ClienteProduto.CodigoClienteProduto = pClienteVencimento.IdClienteProduto;

            ClienteProdutoResponse ClienteProdutoResposta = this.SelecionarClienteProduto(clienteProduto); //seleciona o cliente_produto

            if (ClienteProdutoResposta.StatusResposta != Library.MensagemResponseStatusEnum.OK)
            {
                throw new Exception(ClienteProdutoResposta.DescricaoResposta);
            }
            
            ProdutoRequest Produto = new ProdutoRequest();
            Produto.Produto = new ProdutoInfo();
            Produto.Produto.CodigoProduto = ClienteProdutoResposta.ListaClienteProduto[0].CodigoProduto;
            
            ProdutoResponse ProdutoResposta = this.SelecionarProduto(Produto); //seleciona as inforamções do produto

            if (ProdutoResposta.StatusResposta == Library.MensagemResponseStatusEnum.OK)
            {
                lRetorno.DtVencimento           = pClienteVencimento.DtVencimento.Value.AddDays(ProdutoResposta.ListaProduto[0].QuantidadeDiasVencimento)       ;
                lRetorno.DtInicio_troca_plano   = pClienteVencimento.DtVencimento.Value.AddDays(-ProdutoResposta.ListaProduto[0].QuantidadeDiasRetroTrocaAtivo) ;
            }
            else
            {
                throw new Exception(ProdutoResposta.DescricaoResposta);
            }

            
            return lRetorno;
        }

        public ClienteCCOResponse SelecionarClientesArquivoCCOU()
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoSQL;

            ClienteCCOUInfo Cliente;

            ClienteCCOResponse lRetorno = new ClienteCCOResponse();

            lRetorno.ListaClienteCCOUInfo = new List<ClienteCCOUInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SCLIENTE_GERAR_CCOU"))
            {
                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    Cliente = new ClienteCCOUInfo();

                    Cliente.CodigoClientePoupe      = linha["ID_CLIENTE_POUPE"].DBToInt32();
                    Cliente.CodigoClientePrincipal  = linha["ID_CLIENTE"].DBToInt32()   ;
                    Cliente.ValorProduto            = linha["VL_PRODUTO"].DBToDecimal()   ;

                    lRetorno.ListaClienteCCOUInfo.Add(Cliente);
                }
                    
            }

            return lRetorno;
        }

        public Decimal SelecionaValorDisponivel(int CodigoBolsa)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ConexaoTrade;

            decimal ValorDisponivel = 0;
            try
            {

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDO_DISPONIVEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "cd_cliente", DbType.Int32, CodigoBolsa);
                    
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorDisponivel = (lDataTable.Rows[0]["Saldo"]).DBToDecimal();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ValorDisponivel;
        }

        private AplicacaoInfo MontaObjetoAplicacaoInfo(DataRow linha)
        {
            AplicacaoInfo aplicacao = new AplicacaoInfo();

            aplicacao.CodigoAplicacao       = linha["ID_APLICACAO"].DBToInt32()                         ;
            aplicacao.CodigoCliente         = linha["ID_CLIENTE"].DBToInt32()                           ;
            aplicacao.CodigoProduto         = linha["ID_PRODUTO"].DBToInt32()                           ;
            aplicacao.DtSolicitacao         = linha["DT_SOLICITACAO"].DBToDateTime()                    ;
            aplicacao.DtEfetivacao          = linha["DT_EFETIVACAO"].DBToDateTime()                     ;
            aplicacao.ValorSolicitado       = linha["VL_SOLICITADO"].DBToDecimal()                      ;
            aplicacao.CodigoStatus          = this.SelecionarEnumStatus(linha["ID_STATUS"].DBToInt32()) ;
            aplicacao.DescricaoAplicacao    = linha["DS_APLICACAO"].ToString()                          ;
            aplicacao.DescricaoProduto      = linha["DS_PRODUTO"].ToString()                            ;
            aplicacao.DescricaoStatus       = linha["DS_STATUS"].ToString()                             ;

            return aplicacao;
        }

        private ResgateInfo MontaObjetoResgateInfo(DataRow linha)
        {
            ResgateInfo Resgate = new ResgateInfo();

            Resgate.CodigoResgate       = linha["ID_RESGATE"].DBToInt32()                           ;
            Resgate.CodigoCliente       = linha["ID_CLIENTE"].DBToInt32()                           ;
            Resgate.CodigoProduto       = linha["ID_PRODUTO"].DBToInt32()                           ;
            Resgate.DtSolicitacao       = linha["DT_SOLICITACAO"].DBToDateTime()                    ;
            Resgate.DtEfetivacao        = linha["DT_EFETIVACAO"].DBToDateTime()                     ;
            Resgate.ValorSolicitado     = linha["VL_SOLICITADO"].DBToDecimal()                      ;
            Resgate.CodigoStatus        = this.SelecionarEnumStatus(linha["ID_STATUS"].DBToInt32()) ;
            Resgate.DescricaoResgate    = linha["DS_RESGATE"].ToString()                            ;
            Resgate.DescricaoProduto    = linha["DS_PRODUTO"].ToString()                            ;
            Resgate.DescricaoStatus     = linha["DS_STATUS"].ToString()                             ;


            return Resgate;
        }

        private ClienteProdutoInfo MontaObjetoClienteProduto(DataRow linha)
        {
            ClienteProdutoInfo lRetorno = new ClienteProdutoInfo();

            lRetorno.CodigoClienteProduto   = linha["ID_CLIENTE_PRODUTO"].DBToInt32()                               ;
            lRetorno.CodigoCliente          = linha["ID_CLIENTE"].DBToInt32()                                       ;
            lRetorno.CodigoAtivo            = linha["ID_ATIVO"].ToString()                                          ;
            lRetorno.CodigoSolicitacao      =  this.SelecionarEnumSolicitacao(linha["ID_SOLICITACAO"].DBToInt32())  ;
            lRetorno.CodigoProduto          = linha["ID_PRODUTO"].DBToInt32()                                       ;
            lRetorno.DtSolicitacao          = linha["DT_SOLICITACAO"].DBToDateTime()                                ;
            lRetorno.DtExportacao           = linha["DT_EXPORTACAO"].DBToDateTime()                                 ;
            lRetorno.CodigoClientePoupe     = linha["ID_CLIENTE_POUPE"].DBToInt32()                                 ;
            lRetorno.DtVencimento           = linha["DT_VENCIMENTO"].DBToDateTime()                                 ;
            lRetorno.DtInicioTrocaPlano     = linha["DT_INICIO_TROCA_PLANO"].DBToDateTime()                         ;
            lRetorno.QtdDiasVencimento      = linha["QTDE_DIAS_PARA_VENCIMENTO"].DBToInt32()                        ;
            
            return lRetorno;
        }

        private MovimentoCCSinacorInfo MontaObjetoMovimentoCCSinacor(DataRow linha)
        {
            MovimentoCCSinacorInfo lRetorno = new MovimentoCCSinacorInfo();

            lRetorno.CodigoCliente          = linha["CD_CLIENTE"].DBToInt32()       ;
            lRetorno.NumeroLancamento       = linha["NR_LANCAMENTO"].DBToInt32()    ;
            lRetorno.DtLancamento           = linha["DT_LANCAMENTO"].DBToDateTime() ;
            lRetorno.DtReferenca            = linha["DT_REFERENCIA"].DBToDateTime() ;
            lRetorno.DtLiquidacao           = linha["DT_LIQUIDACAO"].DBToDateTime() ;
            lRetorno.CodigoAtividade        = linha["CD_ATIVIDADE"].DBToString()    ;
            lRetorno.CodigoHistorico        = linha["CD_HISTORICO"].DBToInt32()     ;
            lRetorno.DescricaoLancamento    = linha["DS_LANCAMENTO"].DBToString()   ;
            lRetorno.ValorLancamento         = linha["VL_LANCAMENTO"].DBToDecimal()  ;    

            return lRetorno;
        }

        private CustodiaValorizadaInfo MontaObjetoCustodiaValorizada(DataRow linha)
        {
            CustodiaValorizadaInfo lRetorno = new CustodiaValorizadaInfo();

            lRetorno.CodigoCustodiaValorizada   = linha["ID_CUSTODIA_VALORIZADA"].DBToInt32()   ;
            lRetorno.CodigoClienteProduto       = linha["ID_CLIENTE_PRODUTO"].DBToInt32()       ;
            lRetorno.CodigoCliente              = linha["ID_CLIENTE"].DBToInt32()               ;
            lRetorno.CodigoProduto              = linha["ID_PRODUTO"].DBToInt32()               ;
            lRetorno.ValorAplicacao             = linha["VL_APLICACAO"].DBToDecimal()           ;
            lRetorno.ValorConsumido             = linha["VL_CONSUMIDO"].DBToDecimal()           ;
            lRetorno.ValorResquicio             = linha["VL_RESQUICIO"].DBToDecimal()           ;
            lRetorno.ValorCarteira              = linha["VL_CARTEIRA"].DBToDecimal()            ;
            lRetorno.ValorCustoMedio            = linha["VL_CUSTO_MEDIO"].DBToDecimal()         ;
            lRetorno.DtRentabilizacao           = linha["DT_RENTABILIZACAO"].DBToDateTime()     ;
            lRetorno.QtdTitulos                 = linha["QTDE_TITULOS"].DBToInt32()             ;
            lRetorno.PercentVariacao            = linha["PERC_VARIACAO"].DBToDecimal()          ; 
            lRetorno.DescricaProduto            = linha["DS_PRODUTO"].DBToString()              ;
            lRetorno.CodigoAtivo                = linha["ID_ATIVO"].DBToString()                ;
            lRetorno.ValorAtivo                 = linha["VL_ATIVO"].DBToDecimal()               ; 

            return lRetorno;
        }

        private CompraETFInfo MontaObjetoCompraETFInfo(DataRow linha)
        {
            CompraETFInfo lRetorno = new CompraETFInfo();

            lRetorno.CodigoCliente      = linha["CD_CLIENTE"].DBToInt32()       ;
            lRetorno.ValorLiquido       = linha["VL_LIQUIDO"].DBToDecimal()     ;
            lRetorno.ValorCorretagem    = linha["VL_CORTOT"].DBToDecimal()      ;
            lRetorno.CodigoAtivo        = linha["CD_ATIVO"].DBToString()        ;
            lRetorno.DataNegocio        = linha["DT_NEGOCIO"].DBToDateTime()    ;
            lRetorno.ValorPapel         = linha["VL_NEGOCIO"].DBToDecimal()     ;
            lRetorno.QuantidadePapel    = linha["QT_QTDESP"].DBToInt32()        ;
            lRetorno.ValorTotalNegocio  = linha["VL_TOTNEG"].DBToDecimal()      ;
            

            return lRetorno;
        }

        private ProdutoClienteInfo MontaObjetoProdutoClienteInfo(DataRow linha)
        {
            ProdutoClienteInfo produtoCliente = new ProdutoClienteInfo();

            
            produtoCliente.CodigoProduto                = linha["ID_PRODUTO"].DBToInt32()                               ;
            produtoCliente.DescricaProduto              = linha["DS_PRODUTO"].ToString()                                ;
            produtoCliente.CodigoAtivo                  = linha["ID_ATIVO"].ToString()                                  ;
            produtoCliente.CodigoCliente                = linha["ID_CLIENTE"].DBToInt32()                               ;
            produtoCliente.PermanenciaMinima            = linha["PERMANENCIA_MINIMA"].DBToInt32()                       ;
            produtoCliente.PercentualMulta              = linha["PERCENT_MULTA"].DBToInt32()                            ;
            produtoCliente.QuantidadeRetroTrocaAtivo    = linha["QTDE_DIAS_RETRO_TROCA_PLANO"].DBToInt32()              ;
            produtoCliente.QuantidadeDiaVencimento      = linha["QTDE_DIAS_PARA_VENCIMENTO"].DBToInt32()                ;
            produtoCliente.ValorTotalProduto            = linha["VALOR_TOTAL"].DBToDecimal()                            ;
            produtoCliente.DataSolicitacao              = linha["DT_SOLICITACAO"].DBToDateTime()                        ;
            produtoCliente.DataVencimento               = linha["DT_VENCIMENTO"].DBToDateTime()                         ;
            produtoCliente.DataRetroTrocaPlano          = linha["DT_INICIO_TROCA_PLANO"].DBToDateTime()                 ;
            produtoCliente.CodigoClienteProduto         = linha["ID_CLIENTE_PRODUTO"].DBToInt32()                       ;
            produtoCliente.CodigoVencimento             = linha["ID_CLIENTE_VENCIMENTO"].DBToInt32()                    ;
            produtoCliente.ValorDisponivel              = this.SelecionaValorDisponivel(produtoCliente.CodigoCliente)   ; //saldo em conta corrente.
            produtoCliente.ValorPapel                   = this.ObterPosicaoFechamentoCotacao(produtoCliente.CodigoAtivo); //ultima cotaçao do papel.
            produtoCliente.QuantidadeCompra             =  produtoCliente.ValorDisponivel / produtoCliente.ValorPapel   ; //Quantidade deste papel que pode-se comprar com o valor disponível.

            
                         
            if(linha["DT_COMPRA"].ToString() != string.Empty)
                    produtoCliente.DataCompra       = linha["DT_COMPRA"].DBToDateTime();
            
            if (linha["DT_EXPORTACAO"].ToString() != string.Empty)
                produtoCliente.DataExportacao = linha["DT_EXPORTACAO"].DBToDateTime();

                    
           return produtoCliente;
        }

        private ClienteVencimentoInfo MontaObjetoClienteVencimento(DataRow linha)
        {
            ClienteVencimentoInfo lRetorno = new ClienteVencimentoInfo();

            lRetorno.IdClienteVencimento    = linha["ID_CLIENTE_VENCIMENTO"].DBToInt32();
            lRetorno.IdClienteProduto       = linha["ID_CLIENTE_PRODUTO"].DBToInt32()   ;
            lRetorno.IdCliente              = linha["ID_CLIENTE"].DBToInt32()           ;
            lRetorno.DtVencimento           = linha["DT_VENCIMENTO"].DBToDateTime()     ;
            lRetorno.DtInicio_troca_plano   = linha["DT_INICIO_TROCA_PLANO"].DBToDateTime();
                                             
            
            return lRetorno;
        }

        private EnumPoupeDirect.EnumSolicitacao SelecionarEnumSolicitacao(int CodigoSolicitacao)
        {
            switch (CodigoSolicitacao)
            {
                case 1: return EnumPoupeDirect.EnumSolicitacao.PEDIDO_ADESAO;
                case 2: return EnumPoupeDirect.EnumSolicitacao.EXPORTACAO_SINACOR;
                case 3: return EnumPoupeDirect.EnumSolicitacao.ALTERACAO_ATIVO;
                case 4: return EnumPoupeDirect.EnumSolicitacao.CANCELAMENTO_PLANO;
                default: return EnumPoupeDirect.EnumSolicitacao.CANCELAMENTO_PLANO;
            }
        }

        private EnumPoupeDirect.EnumStatus SelecionarEnumStatus(int CodigoStatus)
        {
            switch (CodigoStatus)
            {
                case 1: return EnumPoupeDirect.EnumStatus.AGUADANDO_APROVACAO;
                case 2: return EnumPoupeDirect.EnumStatus.EFETIVADO;
                default: return EnumPoupeDirect.EnumStatus.AGUADANDO_APROVACAO;
            }
        }


        /// <summary>
        /// Retorna o dia util mais próximo, incluindo a data corrente(hoje).
        /// </summary>
        /// <returns></returns>
        public DateTime RetornarDiaUtil()
        {

            DateTime retorno = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ConexaoTrade;

            try
            {

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SELECIONAR_DIA_UTIL_SEL"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        retorno = (lDataTable.Rows[0][0]).DBToDateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }



            return retorno;
        }

        
    }
}
