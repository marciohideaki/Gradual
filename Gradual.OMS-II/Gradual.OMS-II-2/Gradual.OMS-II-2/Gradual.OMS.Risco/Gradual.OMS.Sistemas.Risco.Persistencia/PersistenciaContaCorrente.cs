using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Gradual.Generico.Dados;
using log4net;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Enum;

namespace Gradual.OMS.Risco.Persistencia.Lib
{
    public class PersistenciaContaCorrente
    {
        #region | Atributos

        private ILog gLog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string gNomeConexao = "Sinacor";

        private const string gNomeConexaoRisco = "Risco";

        private const string gNomeConexaoContaMargem = "CONTAMARGEM";

        //private const string gCaminhoNomeSchema = @"c:\SaldoProjetadoDN.xsd";

        //private const string gCaminhoNomeXML = @"c:\SaldoProjetadoDN.xml";

        private string gCaminhoNomeSchema = string.Format(@"{0}\SaldoProjetadoDN.xsd", System.Configuration.ConfigurationManager.AppSettings["CaminhoXMLSaldoProjetadoContaCorrente"]);

        private string gCaminhoNomeXML = string.Format(@"{0}\SaldoProjetadoDN.xml", System.Configuration.ConfigurationManager.AppSettings["CaminhoXMLSaldoProjetadoContaCorrente"]);

        #endregion

        #region | Métodos CRUD

        /// <summary>
        /// Obtem o saldo em conta corrente do cliente
        /// </summary>
        /// <param name="pParametro">Objeto com parametros do cliente</param>
        /// <returns>Objeto SaldoContaCorrenteResponse com as informacoes de saldo em conta corrente do cliente </returns>
        public SaldoContaCorrenteResponse<ContaCorrenteInfo> ObterSaldoContaCorrente(SaldoContaCorrenteRequest pParametro)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            SaldoContaCorrenteResponse<ContaCorrenteInfo> _SaldoContaCorrente = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();

            try
            {
                ContaCorrenteInfo lRetorno = new ContaCorrenteInfo();
                lAcessaDados.ConnectionStringName = gNomeConexao;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pccsaldoprojetado"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "PCODCLIENTE", DbType.AnsiString, pParametro.IdCliente.DBToString());

                    lAcessaDados.AddOutParameter(lDbCommand, "PSALDO", DbType.Decimal, 12);
                    lAcessaDados.AddOutParameter(lDbCommand, "PD1", DbType.Decimal, 12);
                    lAcessaDados.AddOutParameter(lDbCommand, "PD2", DbType.Decimal, 12);
                    lAcessaDados.AddOutParameter(lDbCommand, "PD3", DbType.Decimal, 12);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.SaldoD0 = lAcessaDados.GetParameterValue(lDbCommand, "PSALDO").DBToDecimal();
                    lRetorno.SaldoD1 = lAcessaDados.GetParameterValue(lDbCommand, "PD1").DBToDecimal();
                    lRetorno.SaldoD2 = lAcessaDados.GetParameterValue(lDbCommand, "PD2").DBToDecimal();
                    lRetorno.SaldoD3 = lAcessaDados.GetParameterValue(lDbCommand, "PD3").DBToDecimal();
                    lRetorno.SaldoContaMargem = this.ObtemSaldoContaMargem(pParametro.IdCliente);

                    _SaldoContaCorrente.Objeto = lRetorno;
                    _SaldoContaCorrente.DescricaoResposta = string.Format("Saldo de conta corrente do cliente: {0} carregado com sucesso", pParametro.IdCliente.DBToString());
                    _SaldoContaCorrente.StatusResposta = CriticaMensagemEnum.OK;
                }
            }
            catch (Exception ex)
            {
                _SaldoContaCorrente.DescricaoResposta = ex.Message;
                _SaldoContaCorrente.StackTrace = ex.StackTrace;
                _SaldoContaCorrente.StatusResposta = CriticaMensagemEnum.Exception;
                _SaldoContaCorrente.Objeto = null;
            }

            return _SaldoContaCorrente;
        }

        /// <summary>
        /// Obtém o saldo do cliente para D+0, D+1, D+2, D+3 e Conta Margem. Filtragem por CPF/CNPJ, CdAcessor, CdCliente [Sinacor] e posição de saldo.
        /// </summary>
        /// <returns>Lista com posição dos clientes selecionados.</returns>
        public SaldoContaCorrenteResponse<ContaCorrenteInfo> ObterSaldoProjetadoContaCorrente(SaldoContaCorrenteRequest pParametro)
        {
            var lRetorno = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();
            var lStringConsulta = new StringBuilder();

            try
            {
                using (DataTable lDataTablePosicao = new DataTable())
                {
                    lock (lDataTablePosicao)
                    {
                        lDataTablePosicao.ReadXmlSchema(gCaminhoNomeSchema);
                        lDataTablePosicao.ReadXml(gCaminhoNomeXML);

                        {   //--> Montagem da consulta ao xml
                            if (!0.Equals(pParametro.IdCliente))
                                lStringConsulta.AppendFormat(" AND CD_CLIENTE = {0}", pParametro.IdCliente.DBToString());

                            if (null != pParametro.ConsultaIdAssessor)
                                lStringConsulta.AppendFormat(" AND CD_ASSESSOR = {0}", pParametro.ConsultaIdAssessor.Value.DBToString());

                            if (null != pParametro.ConsultaClientesCpfCnpj && pParametro.ConsultaClientesCpfCnpj.Count > 0)
                            {
                                lStringConsulta.Append(" AND CD_CPFCGC IN (");

                                pParametro.ConsultaClientesCpfCnpj.ForEach(delegate(string cpfCnpj) { lStringConsulta.Append(string.Concat(cpfCnpj.Trim().Replace(".", "").Replace(",", "").Replace("-", "").Replace("/", ""), ", ")); });

                                lStringConsulta.Replace(',', ')', lStringConsulta.Length - 2, 1);
                            }

                            if (pParametro.ConsultaPosicaoD0)
                                lStringConsulta.AppendFormat(" AND D0 < 0");

                            if (pParametro.ConsultaPosicaoD1)
                                lStringConsulta.AppendFormat(" AND D1 < 0");

                            if (pParametro.ConsultaPosicaoD2)
                                lStringConsulta.AppendFormat(" AND D2 < 0");

                            if (pParametro.ConsultaPosicaoD3)
                                lStringConsulta.AppendFormat(" AND D3 < 0");

                            if (pParametro.ConsultaContaMargem)
                                lStringConsulta.AppendFormat(" AND CM < 0");
                        }

                        var lResultadoConsulta = lDataTablePosicao.Select(lStringConsulta.ToString().Trim().StartsWith("AND") ? lStringConsulta.ToString().Remove(0, 5) : string.Empty);

                        lDataTablePosicao.Dispose();

                        if (null != lResultadoConsulta) for (int i = 0; i < lResultadoConsulta.Length; i++)
                                lRetorno.ObjetoLista.Add(this.CarregarContaCorrenteInfo(lResultadoConsulta[i]));

                        lRetorno.StatusResposta = CriticaMensagemEnum.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                gLog.Error("Erro ao processar consulda de Conta Corrente Projetada", ex);
                lRetorno.DescricaoResposta = ex.Message + " - " + System.IO.Directory.GetCurrentDirectory();
                lRetorno.StackTrace = ex.StackTrace;
                lRetorno.StatusResposta = CriticaMensagemEnum.Exception;
                lRetorno.Objeto = null;
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        /// <summary>
        /// Obtem o saldo de conta margem do cliente
        /// </summary>
        /// <param name="IdCliente">Código do cliente</param>
        /// <returns>Saldo em conta margem do cliente</returns>
        private Nullable<decimal> ObtemSaldoContaMargem(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            SaldoContaCorrenteResponse<ContaCorrenteInfo> _SaldoContaCorrente = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoContaMargem;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_contamargem"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["VL_LIMITE"]).DBToInt64();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta margem do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }

        }

        /// <summary>
        /// Monta a intância de um ContaCorrenteInfo.
        /// </summary>
        private ContaCorrenteInfo CarregarContaCorrenteInfo(DataRow linha)
        {
            return new ContaCorrenteInfo()
            {
                DsCpfCnpj = linha["CD_CPFCGC"].DBToString().PadLeft(11, '0'),
                IdAssessor = linha["CD_ASSESSOR"].DBToInt32(),
                IdClienteSinacor = linha["CD_CLIENTE"].DBToInt32(),
                NomeCliente = linha["NM_CLIENTE"].DBToString(),
                SaldoD0 = linha["D0"].DBToString().Replace(".", ",").DBToDecimal(),
                SaldoD1 = linha["D1"].DBToString().Replace(".", ",").DBToDecimal(),
                SaldoD2 = linha["D2"].DBToString().Replace(".", ",").DBToDecimal(),
                SaldoD3 = linha["D3"].DBToString().Replace(".", ",").DBToDecimal(),
                SaldoContaMargem = linha["CM"].DBToString().Replace(".", ",").DBToDecimal(),
            };
        }

        #endregion
    }
}

