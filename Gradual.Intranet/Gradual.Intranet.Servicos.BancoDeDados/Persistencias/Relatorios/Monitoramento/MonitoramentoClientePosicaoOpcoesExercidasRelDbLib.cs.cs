using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Monitoramento
{
    public class MonitoramentoClientePosicaoOpcoesExercidasRelDbLib
    {
        private const string gConexaoSinacor = "SINACOR";

        private ClienteResumidoInfo gRetorno = new ClienteResumidoInfo();

        public ConsultarObjetosResponse<ClientePosicaoOpcaoExercidaRelInfo> ConsultarClientePosicaoOpcoesExercidas(ConsultarEntidadeRequest<ClientePosicaoOpcaoExercidaRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ClientePosicaoOpcaoExercidaRelInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = gConexaoSinacor;

            string lAssessores = pParametros.Objeto.ConsultaCodigoAssessor;

            string[] lArrayAssessor = lAssessores.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (lArrayAssessor.Length == 0)
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_OPC_EXERCICIO_SEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente",  DbType.Int32, OpcoesBuscarPor.CodBovespa.Equals(pParametros.Objeto.ConsultaClienteTipo) && !string.IsNullOrWhiteSpace(pParametros.Objeto.ConsultaClienteParametro) ? (pParametros.Objeto.ConsultaClienteParametro.Trim().Length > 8) ? (object)pParametros.Objeto.ConsultaClienteParametro.Trim().Substring(0, 8) : (object)pParametros.Objeto.ConsultaClienteParametro.Trim() : null);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_nome",     DbType.String, OpcoesBuscarPor.NomeCliente.Equals(pParametros.Objeto.ConsultaClienteTipo) && !string.IsNullOrWhiteSpace(pParametros.Objeto.ConsultaClienteParametro) ? (object)pParametros.Objeto.ConsultaClienteParametro.Trim() : null);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_cpfcnpj",  DbType.String, OpcoesBuscarPor.CpfCnpj.Equals(pParametros.Objeto.ConsultaClienteTipo) && !string.IsNullOrWhiteSpace(pParametros.Objeto.ConsultaClienteParametro) ? (object)pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty) : null);

                    lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.AnsiString, pParametros.Objeto.ConsultaCodigoAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "pid_carteira", DbType.Int32, pParametros.Objeto.ConsultaCodigoCarteira);
                    lAcessaDados.AddInParameter(lDbCommand, "pst_sentido",  DbType.Int32, pParametros.Objeto.ConsultaSentidoCompradoLancado);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_papel",    DbType.String, pParametros.Objeto.ConsultaPapel);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_strike",   DbType.Int32, pParametros.Objeto.ConsultarStrike);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_serie",    DbType.String, pParametros.Objeto.ConsultaSerie);
                    lAcessaDados.AddInParameter(lDbCommand, "pdt_vencimento", DbType.DateTime, pParametros.Objeto.ConsultaDtVencimento);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                            lRetorno.Resultado.Add(new ClientePosicaoOpcaoExercidaRelInfo()
                            {
                                CdAssessor           = lLinha["COD_ASSE"].DBToInt32(),
                                CdCliente            = lLinha["COD_CLI"].DBToInt32(),
                                DsPapel              = lLinha["COD_NEG"].DBToString(),
                                DtVencimento         = lLinha["DATA_VENC"].DBToDateTime(),
                                IdCarteira           = lLinha["COD_CART"].DBToInt32(),
                                NmCliente            = lLinha["NOME_CLI"].DBToString(),
                                QtQuantidade         = lLinha["QTDE_TOT"].DBToInt32(),
                                QtQuantidadeAbertura = lLinha["QTDE_DISP"].DBToInt32(),
                                QtQuandidadeD1       = lLinha["QTDE_D1"].DBToInt32(),
                                QtQunatidadeAtual    = lLinha["QTDE_ATUAL"].DBToInt32(),
                                DtStrike             = lLinha["dt_strike"].DBToDateTime(),
                                NomeAssessor         = lLinha["NOME_ASSE"].ToString(),
                                OperacoesDia         = lLinha["OPERACOES_DIA"].ToString(),
                                QtdeExercicio        = lLinha["QTDE_EXERCIDA"].DBToInt32(),
                            });
                }
            }

            foreach (string assessor in lArrayAssessor)
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_OPC_EXERCICIO_SEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, OpcoesBuscarPor.CodBovespa.Equals(pParametros.Objeto.ConsultaClienteTipo) && !string.IsNullOrWhiteSpace(pParametros.Objeto.ConsultaClienteParametro) ? (pParametros.Objeto.ConsultaClienteParametro.Trim().Length > 8) ? (object)pParametros.Objeto.ConsultaClienteParametro.Trim().Substring(0, 8) : (object)pParametros.Objeto.ConsultaClienteParametro.Trim() : null);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_nome", DbType.String, OpcoesBuscarPor.NomeCliente.Equals(pParametros.Objeto.ConsultaClienteTipo) && !string.IsNullOrWhiteSpace(pParametros.Objeto.ConsultaClienteParametro) ? (object)pParametros.Objeto.ConsultaClienteParametro.Trim() : null);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_cpfcnpj", DbType.String, OpcoesBuscarPor.CpfCnpj.Equals(pParametros.Objeto.ConsultaClienteTipo) && !string.IsNullOrWhiteSpace(pParametros.Objeto.ConsultaClienteParametro) ? (object)pParametros.Objeto.ConsultaClienteParametro.Trim().Replace(".", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty) : null);

                    lAcessaDados.AddInParameter(lDbCommand, "pcd_assessor", DbType.AnsiString, assessor );
                    lAcessaDados.AddInParameter(lDbCommand, "pid_carteira", DbType.Int32, pParametros.Objeto.ConsultaCodigoCarteira);
                    lAcessaDados.AddInParameter(lDbCommand, "pst_sentido", DbType.Int32, pParametros.Objeto.ConsultaSentidoCompradoLancado);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_papel", DbType.String, pParametros.Objeto.ConsultaPapel);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_strike", DbType.Int32, pParametros.Objeto.ConsultarStrike);
                    lAcessaDados.AddInParameter(lDbCommand, "pds_serie", DbType.String, pParametros.Objeto.ConsultaSerie);
                    lAcessaDados.AddInParameter(lDbCommand, "pdt_vencimento", DbType.DateTime, pParametros.Objeto.ConsultaDtVencimento);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                            lRetorno.Resultado.Add(new ClientePosicaoOpcaoExercidaRelInfo()
                            {
                                CdAssessor           = lLinha["COD_ASSE"].DBToInt32(),
                                CdCliente            = lLinha["COD_CLI"].DBToInt32(),
                                DsPapel              = lLinha["COD_NEG"].DBToString(),
                                DtVencimento         = lLinha["DATA_VENC"].DBToDateTime(),
                                IdCarteira           = lLinha["COD_CART"].DBToInt32(),
                                NmCliente            = lLinha["NOME_CLI"].DBToString(),
                                QtQuantidade         = lLinha["QTDE_TOT"].DBToInt32(),
                                QtQuantidadeAbertura = lLinha["QTDE_DISP"].DBToInt32(),
                                QtQuandidadeD1       = lLinha["QTDE_D1"].DBToInt32(),
                                QtQunatidadeAtual    = lLinha["QTDE_ATUAL"].DBToInt32(),
                                DtStrike             = lLinha["dt_strike"].DBToDateTime(),
                                NomeAssessor         = lLinha["NOME_ASSE"].ToString(),
                                OperacoesDia         = lLinha["OPERACOES_DIA"].ToString(),
                                QtdeExercicio        = lLinha["QTDE_EXERCIDA"].DBToInt32(),
                            });
                }
            }

            return lRetorno;
        }
    }
}
