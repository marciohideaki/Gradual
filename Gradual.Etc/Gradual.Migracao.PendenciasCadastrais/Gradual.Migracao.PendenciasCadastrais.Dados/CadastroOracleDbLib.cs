using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using System.Collections.Generic;
using Gradual.Migracao.PendenciasCadastrais.Entidades;

namespace Gradual.Migracao.PendenciasCadastrais.Dados
{
    public class CadastroOracleDbLib
    {
        public List<ClientePendenciaOracleInfo> RecuperarPendenciasPorCliente()
        {
            var lRetorno = new List<ClientePendenciaOracleInfo>();
            var lAcessaDados = new AcessaDados();

            var lQuery = @"SELECT     cli.cpf cpfcnpj
                           ,          pen.documento
                           ,          pen.cpf
                           ,          pen.certidaocasamento
                           ,          pen.comprovanteendereco
                           ,          pen.procuracao
                           ,          pen.comprovanterenda
                           ,          pen.contrato
                           ,          pen.serasa
                           ,          pen.datacadastro
                           ,          pen.dataresolucao
                           ,          pen.descricao 
                           FROM       pendencia pen
                           INNER JOIN cliente   cli ON cli.id_cliente = pen.id_cliente
                           WHERE      pen.documento           = 'S'
                           OR         pen.cpf                 = 'S'
                           OR         pen.certidaocasamento   = 'S'
                           OR         pen.comprovanteendereco = 'S'
                           OR         pen.procuracao          = 'S'
                           OR         pen.comprovanterenda    = 'S'
                           OR         pen.contrato            = 'S'
                           OR         pen.serasa              = 'S'
                           ORDER BY   pen.id_cliente";

            lAcessaDados.ConnectionStringName = "ConexaoOracle";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow linha in lDataTable.Rows)
                            lRetorno.Add(new ClientePendenciaOracleInfo()
                            {
                                DsCpfCnpj = linha["cpfcnpj"].DBToString(),
                                PendenciaCertidaoCasamento = linha["certidaocasamento"].DBToBoolean(),
                                PendenciaComprovanteEndereco = linha["comprovanteendereco"].DBToBoolean(),
                                PendenciaComprovanteRenda = linha["comprovanterenda"].DBToBoolean(),
                                PendenciaContrato = linha["contrato"].DBToBoolean(),
                                PendenciaCPF = linha["cpf"].DBToBoolean(),
                                PendenciaDataCadastro = linha["datacadastro"].DBToDateTime(),
                                PendenciaDataSolucao = linha["dataresolucao"].DBToDateTime(),
                                PendenciaDescricao = linha["descricao"].DBToString(),
                                PendenciaDocumento = linha["documento"].DBToBoolean(),
                                PendenciaProcuracao = linha["procuracao"].DBToBoolean(),
                                PendenciaSerasa = linha["serasa"].DBToBoolean(),
                            });
            }

            return lRetorno;
        }
    }
}
