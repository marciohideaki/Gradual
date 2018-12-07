using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Generico.Dados;
using System.Data.Common;
using Gradual.Site.DbLib.Dados.MinhaConta;
using System.Data;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta
{
    public class PersistenciaIR
    {
        public void InserirIntegracaoIR(IntegracaoIRRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "INS_MYC_INTEGRACAO"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , pRequest.IntegracaoIR.IdBovespa       );
                _AcessaDados.AddInParameter(_DbCommand, "P_EMAILUSUARIO"                , DbType.String     , pRequest.IntegracaoIR.Email           );
                _AcessaDados.AddInParameter(_DbCommand, "P_CIDADEUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Cidade          );
                _AcessaDados.AddInParameter(_DbCommand, "P_ESTADOUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Estado          );
                _AcessaDados.AddInParameter(_DbCommand, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , pRequest.IntegracaoIR.dataInicio      );
                _AcessaDados.AddInParameter(_DbCommand, "P_STABLOQUEADO"                , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado );
                _AcessaDados.AddInParameter(_DbCommand, "P_CODIGOEVENTO"                , DbType.Int32      , pRequest.IntegracaoIR.CdEvento        );
                _AcessaDados.AddInParameter(_DbCommand, "P_DESCRICAO"                   , DbType.String     , pRequest.IntegracaoIR.Descricao       );
                _AcessaDados.AddInParameter(_DbCommand, "P_TPOPRODUTO"                  , DbType.Int32      , pRequest.IntegracaoIR.TPProduto       );


                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
        }

        public void InserirIntegracaoIRBMF(IntegracaoIRRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            DbConnection conn;

            Conexao Conexao = new Generico.Dados.Conexao();

            DbTransaction transacao;

            DbCommand cmd;

            DbCommand cmdBMF;

            Conexao._ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            conn = Conexao.CreateIConnection();

            conn.Open();

            transacao = conn.BeginTransaction();
                        
            try
            {
                //Insereri BOBESPA
                cmd = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");
                _AcessaDados.AddInParameter(cmd, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , pRequest.IntegracaoIR.IdBovespa             );
                _AcessaDados.AddInParameter(cmd, "P_EMAILUSUARIO"                , DbType.String     , pRequest.IntegracaoIR.Email                 );
                _AcessaDados.AddInParameter(cmd, "P_CIDADEUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Cidade                );
                _AcessaDados.AddInParameter(cmd, "P_ESTADOUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Estado                );
                _AcessaDados.AddInParameter(cmd, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , pRequest.IntegracaoIR.dataInicio            );
                _AcessaDados.AddInParameter(cmd, "P_STABLOQUEADO"                , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado       );
                _AcessaDados.AddInParameter(cmd, "P_CODIGOEVENTO"                , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CADASTRO_NOVO );
                _AcessaDados.AddInParameter(cmd, "P_DESCRICAO"                   , DbType.String     , pRequest.IntegracaoIR.Descricao             );
                _AcessaDados.AddInParameter(cmd, "P_TPOPRODUTO"                  , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BOVESPA        );
                
                _AcessaDados.ExecuteNonQuery(cmd, transacao);


                //Inseri BMF
                cmdBMF = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");
                _AcessaDados.AddInParameter(cmdBMF, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , pRequest.IntegracaoIR.IdBMF                  );
                _AcessaDados.AddInParameter(cmdBMF, "P_EMAILUSUARIO"                , DbType.String     , pRequest.IntegracaoIR.Email                  );
                _AcessaDados.AddInParameter(cmdBMF, "P_CIDADEUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Cidade                 );
                _AcessaDados.AddInParameter(cmdBMF, "P_ESTADOUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Estado                 );
                _AcessaDados.AddInParameter(cmdBMF, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , pRequest.IntegracaoIR.dataInicio             );
                _AcessaDados.AddInParameter(cmdBMF, "P_STABLOQUEADO"                , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado        );
                _AcessaDados.AddInParameter(cmdBMF, "P_CODIGOEVENTO"                , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CADASTRO_NOVO  );
                _AcessaDados.AddInParameter(cmdBMF, "P_DESCRICAO"                   , DbType.String     , pRequest.IntegracaoIR.Descricao              );
                _AcessaDados.AddInParameter(cmdBMF, "P_TPOPRODUTO"                  , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BMF             );
                
                _AcessaDados.ExecuteNonQuery(cmdBMF, transacao);
                
                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw ex;
            }
      
        }

        public void InserirRetrocedente(IntegracaoIRRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            DbConnection conn;

            Conexao Conexao = new Generico.Dados.Conexao();

            DbTransaction transacao;

            DbCommand cmd;

            DbCommand cmdCancelar;

            Conexao._ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            conn = Conexao.CreateIConnection();

            conn.Open();

            transacao = conn.BeginTransaction();
                        
            try
            {
                
                //Insereri BOBESPA
                cmd = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");
                _AcessaDados.AddInParameter(cmd, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , pRequest.IntegracaoIR.IdBovespa             );
                _AcessaDados.AddInParameter(cmd, "P_EMAILUSUARIO"                , DbType.String     , pRequest.IntegracaoIR.Email                 );
                _AcessaDados.AddInParameter(cmd, "P_CIDADEUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Cidade                );
                _AcessaDados.AddInParameter(cmd, "P_ESTADOUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Estado                );
                _AcessaDados.AddInParameter(cmd, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , pRequest.IntegracaoIR.dataInicio            );
                _AcessaDados.AddInParameter(cmd, "P_STABLOQUEADO"                , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado       );
                _AcessaDados.AddInParameter(cmd, "P_CODIGOEVENTO"                , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CADASTRO_NOVO );
                _AcessaDados.AddInParameter(cmd, "P_DESCRICAO"                   , DbType.String     , pRequest.IntegracaoIR.Descricao             );
                _AcessaDados.AddInParameter(cmd, "P_TPOPRODUTO"                  , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BOVESPA        );
                
                _AcessaDados.ExecuteNonQuery(cmd, transacao);


                //Inseri o cancelamento bovespa
                cmdCancelar = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");
                _AcessaDados.AddInParameter(cmdCancelar, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , pRequest.IntegracaoIR.IdBovespa         );
                _AcessaDados.AddInParameter(cmdCancelar, "P_EMAILUSUARIO"                , DbType.String     , pRequest.IntegracaoIR.Email             );
                _AcessaDados.AddInParameter(cmdCancelar, "P_CIDADEUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Cidade            );
                _AcessaDados.AddInParameter(cmdCancelar, "P_ESTADOUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Estado            );
                _AcessaDados.AddInParameter(cmdCancelar, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , pRequest.IntegracaoIR.dataFim           );
                _AcessaDados.AddInParameter(cmdCancelar, "P_STABLOQUEADO"                , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado   );
                _AcessaDados.AddInParameter(cmdCancelar, "P_CODIGOEVENTO"                , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CANCELAR  );
                _AcessaDados.AddInParameter(cmdCancelar, "P_DESCRICAO"                   , DbType.String     , pRequest.IntegracaoIR.Descricao         );
                _AcessaDados.AddInParameter(cmdCancelar, "P_TPOPRODUTO"                  , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BOVESPA    );
               
                _AcessaDados.ExecuteNonQuery(cmdCancelar, transacao);
                
                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw ex;
            }
        }

        public void InserirIRRetrocedenteBMF(IntegracaoIRRequest pRequest)
        {

            AcessaDados _AcessaDados = new AcessaDados();

            DbConnection conn;

            Conexao Conexao = new Generico.Dados.Conexao();

            DbTransaction transacao;

            DbCommand cmd;

            DbCommand cmdCancelar;

            DbCommand cmdBMF;

            DbCommand cmdBMFCancelar;

            Conexao._ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            conn = Conexao.CreateIConnection();

            conn.Open();

            transacao = conn.BeginTransaction();

            try
            {
                //Insereri BOVESPA
                cmd = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");

                _AcessaDados.AddInParameter(cmd, "P_CODIGOBOVESPACLIENTE"       , DbType.Int32      , pRequest.IntegracaoIR.IdBovespa               );
                _AcessaDados.AddInParameter(cmd, "P_EMAILUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Email                   );
                _AcessaDados.AddInParameter(cmd, "P_CIDADEUSUARIO"              , DbType.String     , pRequest.IntegracaoIR.Cidade                  );
                _AcessaDados.AddInParameter(cmd, "P_ESTADOUSUARIO"              , DbType.String     , pRequest.IntegracaoIR.Estado                  );
                _AcessaDados.AddInParameter(cmd, "P_DTAINICIOLANCAMENTOUSUARIO" , DbType.DateTime   , pRequest.IntegracaoIR.dataInicio              );
                _AcessaDados.AddInParameter(cmd, "P_STABLOQUEADO"               , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado         );
                _AcessaDados.AddInParameter(cmd, "P_CODIGOEVENTO"               , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CADASTRO_NOVO   );
                _AcessaDados.AddInParameter(cmd, "P_DESCRICAO"                  , DbType.String     , pRequest.IntegracaoIR.Descricao               );
                _AcessaDados.AddInParameter(cmd, "P_TPOPRODUTO"                 , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BOVESPA          );

                _AcessaDados.ExecuteNonQuery(cmd, transacao);


                //Inseri o cancelamento bovespa
                cmdCancelar = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");
                _AcessaDados.AddInParameter(cmdCancelar, "P_CODIGOBOVESPACLIENTE"       , DbType.Int32      , pRequest.IntegracaoIR.IdBovespa           );
                _AcessaDados.AddInParameter(cmdCancelar, "P_EMAILUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Email               );
                _AcessaDados.AddInParameter(cmdCancelar, "P_CIDADEUSUARIO"              , DbType.String     , pRequest.IntegracaoIR.Cidade              );
                _AcessaDados.AddInParameter(cmdCancelar, "P_ESTADOUSUARIO"              , DbType.String     , pRequest.IntegracaoIR.Estado              );
                _AcessaDados.AddInParameter(cmdCancelar, "P_DTAINICIOLANCAMENTOUSUARIO" , DbType.DateTime   , pRequest.IntegracaoIR.dataFim             );
                _AcessaDados.AddInParameter(cmdCancelar, "P_STABLOQUEADO"               , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado     );
                _AcessaDados.AddInParameter(cmdCancelar, "P_CODIGOEVENTO"               , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CANCELAR    );
                _AcessaDados.AddInParameter(cmdCancelar, "P_DESCRICAO"                  , DbType.String     , pRequest.IntegracaoIR.Descricao           );
                _AcessaDados.AddInParameter(cmdCancelar, "P_TPOPRODUTO"                 , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BOVESPA      );

                _AcessaDados.ExecuteNonQuery(cmdCancelar, transacao);

                //Inseri BMF
                cmdBMF = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");
                _AcessaDados.AddInParameter(cmdBMF, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , pRequest.IntegracaoIR.IdBMF                   );
                _AcessaDados.AddInParameter(cmdBMF, "P_EMAILUSUARIO"                , DbType.String     , pRequest.IntegracaoIR.Email                   );
                _AcessaDados.AddInParameter(cmdBMF, "P_CIDADEUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Cidade                  );
                _AcessaDados.AddInParameter(cmdBMF, "P_ESTADOUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Estado                  );
                _AcessaDados.AddInParameter(cmdBMF, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , pRequest.IntegracaoIR.dataInicio              );
                _AcessaDados.AddInParameter(cmdBMF, "P_STABLOQUEADO"                , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado         );
                _AcessaDados.AddInParameter(cmdBMF, "P_CODIGOEVENTO"                , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CADASTRO_NOVO   );
                _AcessaDados.AddInParameter(cmdBMF, "P_DESCRICAO"                   , DbType.String     , pRequest.IntegracaoIR.Descricao               );
                _AcessaDados.AddInParameter(cmdBMF, "P_TPOPRODUTO"                  , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BMF              );

                _AcessaDados.ExecuteNonQuery(cmdBMF, transacao);


                //Inseri Cancelamento BMF
                cmdBMFCancelar = _AcessaDados.CreateCommand(transacao, CommandType.StoredProcedure, "INS_MYC_INTEGRACAO");
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_CODIGOBOVESPACLIENTE"        , DbType.Int32      , pRequest.IntegracaoIR.IdBMF           );
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_EMAILUSUARIO"                , DbType.String     , pRequest.IntegracaoIR.Email           );
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_CIDADEUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Cidade          );
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_ESTADOUSUARIO"               , DbType.String     , pRequest.IntegracaoIR.Estado          );
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_DTAINICIOLANCAMENTOUSUARIO"  , DbType.DateTime   , pRequest.IntegracaoIR.dataFim         );
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_STABLOQUEADO"                , DbType.String     , pRequest.IntegracaoIR.EstadoBloqueado );
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_CODIGOEVENTO"                , DbType.Int32      , IntegracaoIRInfo.CodigoEvento.CANCELAR);
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_DESCRICAO"                   , DbType.String     , pRequest.IntegracaoIR.Descricao       );
                _AcessaDados.AddInParameter(cmdBMFCancelar, "P_TPOPRODUTO"                  , DbType.Int32      , IntegracaoIRInfo.TipoProduto.BMF      );

                _AcessaDados.ExecuteNonQuery(cmdBMFCancelar, transacao);

                transacao.Commit();

            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw ex;
            }
        }

        // <summary>
        /// Seleciona o cliente da tabela de integração com o Mycapital.
        /// Quando a integração ocorre o Mycapital retira o cliente desta tabela. 
        /// </summary>
        /// <param name="codigoCliente">Codigo CBLC ou BMF do cliente</param>
        public IntegracaoIRResponse SelecionarIntegracaoIR(int codigoCliente)
        {

            AcessaDados lAcessaDados = new AcessaDados("ResultSet");

            Conexao lConexao = new Generico.Dados.Conexao();

            DbCommand lComando;

            lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoMyCapital;

            IntegracaoIRInfo lIntegracaoIR;

            IntegracaoIRResponse lRetorno = new IntegracaoIRResponse();

            lRetorno.ListaIntegracaoIR = new List<IntegracaoIRInfo>();

            try
            {
                lComando = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_MC_VERIFICA_INTEGRACAO");

                lAcessaDados.AddInParameter(lComando, "piCodCliente", DbType.Int32, codigoCliente);

                DataTable lTable = lAcessaDados.ExecuteOracleDataTable(lComando);

                foreach (DataRow linha in lTable.Rows)
                {
                    lIntegracaoIR = new IntegracaoIRInfo();

                    lIntegracaoIR.RetornaCodigoEvento(linha["CODIGOEVENTO"].DBToInt32())                 ;

                    lIntegracaoIR.EstadoBloqueado    = linha["STABLOQUEADO"].DBToString()                ;
                    lIntegracaoIR.Cidade             = linha["CIDADEUSUARIO"].DBToString()               ;
                    lIntegracaoIR.Estado             = linha["ESTADOUSUARIO"].DBToString()               ;
                    lIntegracaoIR.Email              = linha["EMAILUSUARIO"].DBToString()                ;
                    lIntegracaoIR.dataInicio         = linha["DTAINICIOLANCAMENTOUSUARIO"].DBToDateTime();
                    lIntegracaoIR.IdBovespa          = linha["CODIGOBOVESPACLIENTE"].DBToInt32()         ;

                    lRetorno.ListaIntegracaoIR.Add(lIntegracaoIR);
                }

                return lRetorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
