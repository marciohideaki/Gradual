using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Migracao.PendenciasCadastrais.Entidades;
using System;
using System.IO;
using System.Text;

namespace Gradual.Migracao.PendenciasCadastrais.Dados
{
    public class CadastroSQLDbLib
    {
        public void ExcluirTipoPendencia(DbTransaction pDbTransaction)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "ConexaoSQL";

            string lQueryDelClientePendencia = "DELETE FROM dbo.tb_cliente_pendenciacadastral";
            string lQueryDelTipoPendencia = "DELETE FROM dbo.tb_tipo_pendenciacadastral";


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.Text, lQueryDelClientePendencia))
            {
                lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
            }

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.Text, lQueryDelTipoPendencia))
            {
                lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
            }
        }

        public void InserirTipoPendencias(DbTransaction pDbTransaction)
        {
            var lAcessaDados = new AcessaDados();
            var lListaTipoPendencias = new List<string>();
            var lQuery = "INSERT INTO dbo.tb_tipo_pendenciacadastral (ds_pendencia, st_automatica) VALUES ('{0}', 0)";

            lAcessaDados.ConnectionStringName = "ConexaoSQL";

            lListaTipoPendencias.Add("Documento");
            lListaTipoPendencias.Add("CPF/CNPJ");
            lListaTipoPendencias.Add("Certidão de Nascimento");
            lListaTipoPendencias.Add("Certidão de Casamento");
            lListaTipoPendencias.Add("Comprovante de Endereço");
            lListaTipoPendencias.Add("Procuração");
            lListaTipoPendencias.Add("Comprovante de Renda");
            lListaTipoPendencias.Add("Contrato");
            lListaTipoPendencias.Add("SERASA");

            lListaTipoPendencias.ForEach(
                lDescricaoPendencia =>
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.Text, string.Format(lQuery, lDescricaoPendencia)))
                    {
                        lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
                    }
                });
        }

        public void InserirPendenciaCadastral(DbTransaction pDbTransaction, List<ClientePendenciaSQLInfo> pListaPendencias)
        {
            var lAcessaDados = new AcessaDados();
            var lQueryPopulada = string.Empty;
            var lQueryPadrao = @"INSERT INTO dbo.tb_cliente_pendenciacadastral
                                        (    id_tipo_pendencia
                                        ,    id_cliente
                                        ,    ds_pendencia
                                        ,    dt_cadastropendencia)
                                 VALUES (    {0}
                                        ,    {1}
                                        ,   '{2}'
                                        ,    {3}";

            lAcessaDados.ConnectionStringName = "ConexaoSql";

            if (null != pListaPendencias)
                pListaPendencias.ForEach(
                    lpe =>
                    {
                        lQueryPopulada = string.Format(lQueryPadrao, lpe.IdTipoPendencia.DBToString(), lpe.IdCliente.DBToString(), lpe.DsPendencia, lpe.DtCadastroPendencia.DBToDateTime());

                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.Text, lQueryPopulada))
                        {
                            lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
                        }
                    });
        }

        public List<TipoPendenciaInfo> BuscarTipoPendencias(DbConnection pDbConnection, DbTransaction pDbTransaction)
        {
            var lRetorno = new List<TipoPendenciaInfo>();
            var lAcessaDados = new AcessaDados();
            var lQuery = "SELECT * FROM tb_tipo_pendenciacadastral";

            lAcessaDados.ConnectionStringName = "ConexaoSql";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.Text, lQuery))
            {
                lDbCommand.Transaction = pDbTransaction;

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand, pDbConnection);

                if (lDataTable != null && lDataTable.Rows.Count > 0) foreach (DataRow linha in lDataTable.Rows)
                        lRetorno.Add(new TipoPendenciaInfo()
                        {
                            DsTipoPendencia = linha["ds_pendencia"].DBToString(),
                            IdTipoPendencia = linha["id_tipo_pendencia"].DBToInt32(),
                        });
            }

            return lRetorno;
        }

        public List<ClientePendenciaSQLInfo> TraduzirPendencias(List<ClientePendenciaOracleInfo> pPendenciasOracle, List<TipoPendenciaInfo> pListaPendenciaInfo)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new List<ClientePendenciaSQLInfo>();
            var lIdCliente = default(int);
            var lListaCpfsInvalidos = new List<string>();

            lAcessaDados.ConnectionStringName = "ConexaoSQL";

            //--> Definindo os Id's dos tipos de Pendência.
            int lIdPendenciaCertidaoCasamento = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("casamento"); }).IdTipoPendencia,
                lIdPendenciaComprovanteEndereco = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("endere"); }).IdTipoPendencia,
                lIdPendenciaComprovanteRenda = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("renda"); }).IdTipoPendencia,
                lIdPendenciaContrato = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("contrato"); }).IdTipoPendencia,
                lIdPendenciaCPF = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("cpf"); }).IdTipoPendencia,
                lIdPendenciaDocumento = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("documento"); }).IdTipoPendencia,
                lIdPendenciaProcuracao = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("procura"); }).IdTipoPendencia,
                lIdPendenciaSerasa = pListaPendenciaInfo.Find(lpi => { return lpi.DsTipoPendencia.ToLower().Contains("serasa"); }).IdTipoPendencia;

            if (null != pPendenciasOracle && pPendenciasOracle.Count > 0)
                pPendenciasOracle.ForEach(
                    por =>
                    {
                        lIdCliente = this.RecuperarIdCliente(por.DsCpfCnpj);

                        if (!0.Equals(lIdCliente))
                        {

                            if (por.PendenciaCertidaoCasamento)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaCertidaoCasamento
                                });
                            }
                            if (por.PendenciaComprovanteEndereco)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaComprovanteEndereco
                                });
                            }
                            if (por.PendenciaComprovanteRenda)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaComprovanteRenda
                                });
                            }
                            if (por.PendenciaContrato)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaContrato
                                });
                            }
                            if (por.PendenciaCPF)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaCPF
                                });
                            }
                            if (por.PendenciaDocumento)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaDocumento
                                });
                            }
                            if (por.PendenciaProcuracao)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaProcuracao
                                });
                            }
                            if (por.PendenciaSerasa)
                            {
                                lRetorno.Add(new ClientePendenciaSQLInfo()
                                {
                                    DsPendencia = por.PendenciaDescricao,
                                    DtCadastroPendencia = por.PendenciaDataCadastro,
                                    IdCliente = lIdCliente,
                                    IdTipoPendencia = lIdPendenciaSerasa
                                });
                            }
                        }
                        else 
                        {
                            lListaCpfsInvalidos.Add(por.DsCpfCnpj);
                        }
                    });

            if (lListaCpfsInvalidos.Count > 0)
                this.GravarLogCpfInvalido(lListaCpfsInvalidos);

            return lRetorno;
        }

        public void InserirClientePendencia(DbTransaction pDbTransaction, List<ClientePendenciaSQLInfo> pClientePendencias)
        {
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "ConexaoSql";
            var lQueryPopulada = string.Empty;
            var lQueryPadrao = @"INSERT INTO dbo.tb_cliente_pendenciacadastral 
                                        (    id_tipo_pendencia
                                        ,    id_cliente
                                        ,    ds_pendencia
                                        ,    dt_cadastropendencia)
                                 VALUES (    {0}
                                        ,    {1}
                                        ,   '{2}'
                                        ,   '{3}')";

            if (null != pClientePendencias && pClientePendencias.Count > 0)
                pClientePendencias.ForEach(
                    cpe =>
                    {
                        lQueryPopulada = string.Format(lQueryPadrao, cpe.IdTipoPendencia, cpe.IdCliente, cpe.DsPendencia, cpe.DtCadastroPendencia.DBToStringDateTime());

                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.Text, lQueryPopulada))
                        {
                            try { lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction); }
                            catch (Exception ex) { throw ex; }
                        }
                    });
        }

        public int RecuperarIdCliente(string pCpfCnpj)
        {
            int lRetorno = default(int);
            var lAcessaDados = new AcessaDados();
            var lQuery = "SELECT id_cliente FROM dbo.tb_cliente WHERE ds_cpfcnpj = '{0}'";

            lAcessaDados.ConnectionStringName = "ConexaoSql";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, string.Format(lQuery, pCpfCnpj)))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    lRetorno = lDataTable.Rows[0]["id_cliente"].DBToInt32();
            }

            return lRetorno;
        }

        private void GravarLogCpfInvalido(List<string> pListaCpfsInvalidos)
        {
            using (StreamWriter lStreamWriter = new StreamWriter(@"c:\Gradual.Migracao.PendenciasCadastrais.CpfsInvalidos.log"))
            {
                lStreamWriter.WriteLine(string.Format("Os {0} CPF/CNPJ's abaixo representam os clientes que não constam na base DirectTrade.", pListaCpfsInvalidos.Count.ToString()));

                pListaCpfsInvalidos.ForEach(
                    cpf =>
                    {
                        lStreamWriter.WriteLine(cpf);
                    });
            }
        }
    }
}
