using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Gradual.Generico.Dados;
using Gradual.OMS.ConsolidadorRelatorioCC.Dados.Shared;
using Gradual.OMS.ConsolidadorRelatorioCCLib;
using Gradual.OMS.ConsolidadorRelatorioCCLib.Enum;
using Gradual.OMS.ConsolidadorRelatorioCCLib.Mensageria;
using log4net;

namespace Gradual.OMS.ConsolidadorRelatorioCC
{
    public class PersistenciaContaCorrente
    {
        #region | Atributos

        private const string gNomeConexaoSinacor = "Sinacor";

        private const string gNomeConexaoRisco = "Risco";

        private const string gNomeConexaoContaMargem = "CONTAMARGEM";

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Métodos de negocio

        /// <summary>
        /// Monta o arquivo XML com o estado da conta dos clientes cadastrados com sua projeção DN e Conta Margem.
        /// </summary>
        public SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo> AlimentarConsultaDN(SaldoContaCorrenteRiscoRequest pParametro)
        {
            var lRetorno = new SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo>();
            var lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pccsaldoprojetado_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCdCliente", DbType.Int32, null);
                    lAcessaDados.AddInParameter(lDbCommand, "pCdAssessor", DbType.Int32, null);
                    lAcessaDados.AddInParameter(lDbCommand, "pStNegativoD0", DbType.Byte, null);
                    lAcessaDados.AddInParameter(lDbCommand, "pStNegativoD1", DbType.Byte, null);
                    lAcessaDados.AddInParameter(lDbCommand, "pStNegativoD2", DbType.Byte, null);
                    lAcessaDados.AddInParameter(lDbCommand, "pStNegativoD3", DbType.Byte, null);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                    
                    {   //--> Gravando em XML para teste.
                        //lDataTable.TableName = "pccsaldoprojetado";
                        //lDataTable.WriteXmlSchema(@"c:\pccsaldoprojetado.xsd");
                        //lDataTable.WriteXml(@"c:\pccsaldoprojetado.xml");
                    }

                    if (null != lDataTable)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Objeto = new ContaCorrenteRiscoInfo();

                            // SALDOS 
                            lRetorno.Objeto.SaldoD0 = lDataTable.Rows[i]["D0"].DBToDecimal();
                            lRetorno.Objeto.SaldoD1 = lDataTable.Rows[i]["D1"].DBToDecimal();
                            lRetorno.Objeto.SaldoD2 = lDataTable.Rows[i]["D2"].DBToDecimal();
                            lRetorno.Objeto.SaldoD3 = lDataTable.Rows[i]["D3"].DBToDecimal();
                            lRetorno.Objeto.SaldoContaMargem = ObtemSaldoContaMargem(lDataTable.Rows[i]["cd_cliente"].DBToInt32());

                            // OUTRAS INFORMACOES
                            lRetorno.Objeto.IdClienteSinacor = lDataTable.Rows[i]["cd_cliente"].DBToInt32();
                            lRetorno.Objeto.NomeCliente = lDataTable.Rows[i]["nm_cliente"].DBToString();
                            lRetorno.Objeto.DsCpfCnpj = lDataTable.Rows[i]["cd_cpfcgc"].DBToString();
                            lRetorno.Objeto.IdAssessor = lDataTable.Rows[i]["cd_assessor"].DBToInt32();

                            lRetorno.ObjetoLista.Add(lRetorno.Objeto);
                        }

                        ColecaoObjetos.Remover();
                        ColecaoObjetos.AdicionarItem("SaldoContaCorrenteRiscoResponse", lRetorno);

                        gLogger.Debug(string.Format("{0} registros encontrados", lDataTable.Rows.Count.ToString()));
                    }

                    ColecaoObjetos.DataHoraUltimaAtualizacao = DateTime.Now;

                    lRetorno.StatusResposta = CriticaMensagemEnum.OK;
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("AlimentarConsultaDN", ex);
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.StackTrace = ex.StackTrace;
                lRetorno.StatusResposta = CriticaMensagemEnum.Exception;
                lRetorno.Objeto = null;
            }

            return lRetorno;
        }

        public SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo> ConsultarSaldoCCProjetado(SaldoContaCorrenteRiscoRequest pParametro)
        {
            if (!ColecaoObjetos.ContemItem("SaldoContaCorrenteRiscoResponse"))
            {
                gLogger.Error("Erro ao consultar a instância: O objeto com os registros não contém a instância de 'SaldoContaCorrenteRiscoResponse'.");

                return new SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo>();
            }

            var lRetorno = new SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo>();

            try
            {
                var lSaldoContaCorrenteRiscoResponse = (SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo>)ColecaoObjetos.RetornarItem("SaldoContaCorrenteRiscoResponse");
                var lListaFiltradaCpfCnpj = new List<ContaCorrenteRiscoInfo>();
                IEnumerable<ContaCorrenteRiscoInfo> lRetornoConsulta = null;
                ContaCorrenteRiscoInfo lContaCorrenteRiscoInfo;

                if (null != lSaldoContaCorrenteRiscoResponse)
                {
                    //Caro Programador, o filtro abaixo filtra os clientes pelo CPF/CNPJ informado.
                    if (null != pParametro.ConsultaClientesCpfCnpj && pParametro.ConsultaClientesCpfCnpj.Count > 0)
                    {
                        for (int i = 0; i < pParametro.ConsultaClientesCpfCnpj.Count; i++)
                        {
                            lContaCorrenteRiscoInfo = lSaldoContaCorrenteRiscoResponse.ObjetoLista.Find(
                                delegate(ContaCorrenteRiscoInfo ccr)
                                {
                                    return ccr.DsCpfCnpj == pParametro.ConsultaClientesCpfCnpj[i].Replace(".", "").Replace("-", "").Replace("/", "");
                                });

                            if (null != lContaCorrenteRiscoInfo && !lListaFiltradaCpfCnpj.Contains(lContaCorrenteRiscoInfo))
                                lListaFiltradaCpfCnpj.Add(lContaCorrenteRiscoInfo);

                            lContaCorrenteRiscoInfo = null;
                        }

                        lSaldoContaCorrenteRiscoResponse.ObjetoLista = lListaFiltradaCpfCnpj; //--> Substitui a lista que para processar APENAS os clientes selecionados por CPF/CNPJ.
                    }

                    lRetornoConsulta = lSaldoContaCorrenteRiscoResponse.ObjetoLista.Where<ContaCorrenteRiscoInfo>(
                        delegate(ContaCorrenteRiscoInfo ccr) //--> Realiza o filtro da consulta.
                        {
                            return ((pParametro.ConsultaPosicaoD0 && ccr.SaldoD0 < 0) || (!pParametro.ConsultaPosicaoD0 && (ccr.SaldoD0 < 0 || ccr.SaldoD0 >= 0)))
                                && ((pParametro.ConsultaPosicaoD1 && ccr.SaldoD1 < 0) || (!pParametro.ConsultaPosicaoD1 && (ccr.SaldoD1 < 0 || ccr.SaldoD1 >= 0)))
                                && ((pParametro.ConsultaPosicaoD2 && ccr.SaldoD2 < 0) || (!pParametro.ConsultaPosicaoD2 && (ccr.SaldoD2 < 0 || ccr.SaldoD2 >= 0)))
                                && ((pParametro.ConsultaPosicaoD3 && ccr.SaldoD3 < 0) || (!pParametro.ConsultaPosicaoD3 && (ccr.SaldoD3 < 0 || ccr.SaldoD3 >= 0)))
                                && ((pParametro.ConsultaContaMargem && ccr.SaldoContaMargem < 0) || (!pParametro.ConsultaContaMargem && (ccr.SaldoContaMargem < 0 || ccr.SaldoContaMargem >= 0)))
                                && ((pParametro.ConsultaIdAssessor != null && ccr.IdAssessor == pParametro.ConsultaIdAssessor) || (pParametro.ConsultaIdAssessor == null && ccr.IdAssessor == ccr.IdAssessor))
                                && ((pParametro.IdCliente != 0 && ccr.IdClienteSinacor == pParametro.IdCliente) || (pParametro.IdCliente == 0 && ccr.IdClienteSinacor == ccr.IdClienteSinacor));
                        });

                    foreach (var item in lRetornoConsulta)
                        lRetorno.ObjetoLista.Add(item); //--> Incrementa a lista de retorno com os registros selecionados.

                    lRetorno.ObjetoLista.Sort(delegate(ContaCorrenteRiscoInfo cc1, ContaCorrenteRiscoInfo cc2) { return Comparer<string>.Default.Compare(cc1.NomeCliente, cc2.NomeCliente); });

                    lRetorno.DescricaoResposta = string.Format("Consulta realizada com sucesso às {0}.", DateTime.Now.ToString());
                    lRetorno.StatusResposta = CriticaMensagemEnum.OK;
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("ConsultarSaldoCCProjetado", ex);
                lRetorno.DescricaoResposta = ex.Message;
                lRetorno.StackTrace = ex.StackTrace;
                lRetorno.StatusResposta = CriticaMensagemEnum.Exception;
                lRetorno.Objeto = null;
            }

            return lRetorno;
        }

        public DateTime ConsultarDataHoraUltimaAtualizacao()
        {
            return ColecaoObjetos.DataHoraUltimaAtualizacao;
        }

        /// <summary>
        /// Obtem o saldo de conta margem do cliente
        /// </summary>
        /// <param name="IdCliente">Código do cliente</param>
        /// <returns>Saldo em conta margem do cliente</returns>
        private Nullable<decimal> ObtemSaldoContaMargem(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo> _SaldoContaCorrente = new SaldoContaCorrenteRiscoResponse<ContaCorrenteRiscoInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoContaMargem;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_contamargem"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, IdCliente);

                    var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["VL_LIMITE"]).DBToDecimal();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("ObtemSaldoContaMargem", ex);
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta margem do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }

        #endregion
    }
}
