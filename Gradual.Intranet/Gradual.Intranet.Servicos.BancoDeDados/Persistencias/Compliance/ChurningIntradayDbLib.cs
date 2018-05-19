using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Contratos.Dados.Compliance;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Compliance
{
    public class ChurningIntradayDbLib
    {


        private void ObterCorretagemPeriodo(ChurningIntradayInfo pRequest, ref ChurningIntradayInfo pRetorno)
        {
            var lListaCorreta = new List<CorretagemChurning>();

            var lAcessaDados = new AcessaDados();

            CorretagemChurning lChurning ;

            lAcessaDados.ConnectionStringName = "SinacorExportacao";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TURNOVER_CORRETA_PER_SEL"))
            {
                var lDataInicial = this.GetDateBrockage(pRequest.DataDe, pRequest.ListaFeriados);

                var lDataFinal = this.GetDateBrockage(pRequest.DataAte, pRequest.ListaFeriados);

                lAcessaDados.AddInParameter(lDbCommand, "pDataInicial", DbType.DateTime, lDataInicial);

                lAcessaDados.AddInParameter(lDbCommand, "pDataFinal", DbType.DateTime, lDataFinal);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow                = lDataTable.Rows[i];
                        lChurning                   = new CorretagemChurning();

                        lChurning.CodigoCliente     = lRow["cd_cliente"].DBToInt32();
                        
                        lChurning.DataPosicao       = lRow["dt_datmov"].DBToDateTime();
                        
                        lChurning.ValorCorretagem   = lRow["VL_VALCOR"].DBToDecimal();

                        lListaCorreta.Add(lChurning);
                    }
                }
            }

            foreach (CorretagemChurning info in lListaCorreta)
            {
                var lChurnFounded = pRetorno.Resultado.Find(churn => { return churn.CodigoCliente == info.CodigoCliente; });

                if (lChurnFounded != null)
                {
                    lChurnFounded.ValorCorretagem += info.ValorCorretagem;
                }
            }
        }

        private void ObterCorretagemDia(ChurningIntradayInfo pRequest, ref ChurningIntradayInfo pRetorno)
        {
            var lListaCorreta = new List<CorretagemChurning>();

            var lAcessaDados = new AcessaDados();

            CorretagemChurning lChurning;

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CORRETA_ULT_DIA_SEL"))
            {
                var lDataAtual = this.GetDateBrockage( DateTime.Now.AddDays(-1).Date, pRequest.ListaFeriados);

                lAcessaDados.AddInParameter(lDbCommand, "pDataAtual", DbType.DateTime, lDataAtual);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lChurning = new CorretagemChurning();

                        lChurning.CodigoCliente     = lRow["cd_cliente"].DBToInt32();
                        lChurning.ValorCorretagemDia   = lRow["VL_VALCOR"].DBToDecimal();

                        lListaCorreta.Add(lChurning);
                    }
                }
            }

            foreach (CorretagemChurning info in lListaCorreta)
            {
                var lChurnFounded = pRetorno.Resultado.Find(churn => { return churn.CodigoCliente == info.CodigoCliente; });

                if (lChurnFounded != null)
                {
                    lChurnFounded.ValorCorretagemDia = info.ValorCorretagemDia;
                }
            }
        }

        private void ObterCarteiraDiaria(ChurningIntradayInfo pRequest, ref ChurningIntradayInfo pRetorno)
        {
            List<CarteiraChurningDia> lRetorno = new List<CarteiraChurningDia>();
            var lAcessaDados = new AcessaDados();

            CarteiraChurningDia lChurning = null;

            /*
            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_churning_carteira_dia_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32,    pRequest.CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        lChurning = new CarteiraChurningDia();

                        lChurning.CodigoCliente    = lRow["CodigoCliente"].DBToInt32();
                        lChurning.ValorCarteiraDia = lRow["VlCarteiraDia"].DBToDecimal();
                        lChurning.Data             = lRow["Data"].DBToDateTime();
                        lChurning.ValorComprasDia  = lRow["ValorCompras"].DBToDecimal();
                        lChurning.ValorVendasDia   = lRow["ValorVendas"].DBToDecimal();
                        

                        lRetorno.Add(lChurning);
                    }
                }
            }
            */
            lAcessaDados.ConnectionStringName = "SinacorExportacao";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TURNOVER_CARTEIRA_DIA_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, pRequest.CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        lChurning = new CarteiraChurningDia();

                        lChurning.CodigoCliente         = lRow["CodigoCliente"].DBToInt32();
                        lChurning.ValorCarteiraDia      = lRow["VlCarteiraDia"].DBToDecimal();
                        lChurning.Data                  = lRow["Data"].DBToDateTime();
                        lChurning.ValorComprasDia       = lRow["ValorCompras"].DBToDecimal();
                        lChurning.ValorVendasDia        = lRow["ValorVendas"].DBToDecimal();
                        //lChurning.ValorCorretagemDia    = this.SelecionaValorCorretagem(lChurning.CodigoCliente, DateTime.Now);

                        lRetorno.Add(lChurning);
                    }
                }
            }

            var lListaChannel = ListarClientesPortas(pRequest);
            
            var lListaCliente = new List<int>();

            lRetorno.ForEach(cliente =>
            {
                lListaCliente.Add(cliente.CodigoCliente);
            });

            

            foreach (ChurningIntradayInfo info in pRetorno.Resultado)
            {
                var lLista = lRetorno.Find(churn =>{ return churn.CodigoCliente == info.CodigoCliente; });

                if (lLista != null)
                {
                    info.ValorCarteiraDia   = lLista.ValorCarteiraDia;
                    info.ValorComprasDia    = lLista.ValorComprasDia;
                    info.ValorVendasDia     = lLista.ValorVendasDia;
                }

                var lEncontradoChannel = lListaChannel.Find(channel => { return channel.CodigoCliente == info.CodigoCliente; });

                if (lEncontradoChannel.CodigoCliente != 0)
                {
                    
                    lEncontradoChannel.ListaPortas.ForEach(portas=> 
                    {
                        if (info.Porta != null)
                        {
                            if (!info.Porta.Contains(portas.ToString()))
                            {
                                info.Porta +=  portas + ",";
                            }
                        }
                        else
                        {
                            info.Porta += portas + ",";
                        }
                    });
                }

                info.TipoPessoa = SelecionarClienteTipo(info.CodigoCliente.Value);
            }
        }

        public ChurningIntradayInfo ObterMonitoramentoIntradiario(ChurningIntradayInfo pRequest)
        {
            var lRetorno = new ChurningIntradayInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SinacorExportacao";

            lRetorno.Resultado = new List<ChurningIntradayInfo>();
            
            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TURNOVER_PERIODO_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, pRequest.CodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "pDataDe", DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "pDataAte", DbType.DateTime, pRequest.DataAte);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        var lChurning = new ChurningIntradayInfo();

                        int lCodigoCliente = int.Parse(lRow["CodigoCliente"].ToString());

                        lChurning = lRetorno.Resultado.Find(churning=>  { return  churning.CodigoCliente ==lCodigoCliente; } );

                        if (lRetorno.Resultado.Count == 0 || lChurning == null)
                        {
                            lChurning = new ChurningIntradayInfo();

                            lChurning.NomeAssessor          = lRow["NomeAssessor"].ToString();
                            lChurning.CodigoAssessor        = int.Parse(lRow["CodigoAssessor"].ToString());
                            lChurning.NomeCliente           = lRow["NomeCliente"].ToString();
                            lChurning.CodigoCliente         = int.Parse(lRow["CodigoCliente"].ToString());
                            lChurning.Data                  = lRow["DATA_POSI"].DBToDateTime();
                            lChurning.ValorVendas           = lRow["VlVendas"].DBToDecimal();
                            lChurning.ValorCompras          = lRow["VlCompras"].DBToDecimal();
                            lChurning.ValorCarteira         = lRow["VlCarteira"].DBToDecimal();
                            lChurning.ValorCarteiraMedia    = lRow["vlCarteiramedia"].DBToDecimal();

                            DateTime lData = this.GetDateBrockage(lRow["DATA_POSI"].DBToDateTime(), pRequest.ListaFeriados);

                            lChurning.ValorL1               = SelecionarValorL1 (lChurning.CodigoCliente.Value, lData );
                            //lChurning.ValorCorretagem       = SelecionaValorCorretagem(lChurning.CodigoCliente.Value, lData);

                            lRetorno.Resultado.Add(lChurning);
                        }
                        else
                        {
                            lRetorno.Resultado.Remove(lChurning);

                            lChurning.ValorVendas        += lRow["VlVendas"].DBToDecimal();
                            lChurning.ValorCompras       += lRow["VlCompras"].DBToDecimal();
                            lChurning.ValorCarteira      += lRow["VlCarteira"].DBToDecimal();
                            lChurning.ValorCarteiraMedia = lRow["vlCarteiramedia"].DBToDecimal();
                            
                            //DateTime lData = this.GetDateBrockage(lRow["DATA_POSI"].DBToDateTime(), pRequest.ListaFeriados);

                            //lChurning.ValorCorretagem    += SelecionaValorCorretagem(lChurning.CodigoCliente.Value , lData);

                            lRetorno.Resultado.Add(lChurning);
 
                        }
                    }
                }
            }

            ObterCorretagemPeriodo(pRequest, ref lRetorno);

            ObterCorretagemDia(pRequest, ref lRetorno);

            ObterCarteiraDiaria(pRequest, ref lRetorno);

            EfetuaCalculoTR(pRequest, ref lRetorno);

            EfetuaCalculoCE(pRequest, ref lRetorno);

            EfetuaFiltroChurningIntraday(pRequest, ref lRetorno);

            EfetuaFiltroChurningIntradayPorta(pRequest, ref lRetorno);

            return lRetorno;
        }

        private void EfetuaCalculoCE(ChurningIntradayInfo pRequest, ref ChurningIntradayInfo lRetorno)
        {
            TimeSpan lSpan = pRequest.DataAte - pRequest.DataDe;

            decimal lTotalMeses = Decimal.Parse((lSpan.TotalDays / 30).ToString());
            /*
            foreach (ChurningIntradayInfo info in lRetorno.Resultado)
            {
                decimal lTotalVendasMes = info.ValorVendas * 12M;
                decimal lTotalVendasDia = info.ValorVendasDia * 0.03334M;
                decimal lCarteira       = info.ValorCarteiraMedia * (lTotalMeses < 1 ? 1: lTotalMeses);

                //info.PercentualCEnoPeriodo = (lTotalVendasMes / lCarteira);
                info.PercentualCEnoPeriodo = (info.ValorVendas / (lCarteira * 100));
                //info.PercentualCEnoDia     = (lTotalVendasDia / (info.ValorCarteiraDia == 0 ? 1 : info.ValorCarteiraDia));
                info.PercentualCEnoDia = (info.ValorVendasDia / (info.ValorCarteiraDia == 0 ? 100 : (info.ValorCarteiraDia * 100)));
            }
            */
            foreach (ChurningIntradayInfo info in lRetorno.Resultado)
            {
                /*
                decimal lTotalVendasMes = info.ValorVendas        * 12M;
                decimal lTotalVendasDia = info.ValorVendasDia     * 0.03334M;
                decimal lCarteira       = info.ValorCarteiraMedia * (lTotalMeses < 1 ? 1 : lTotalMeses);
                */
                decimal lTotalPeriodo = (((info.ValorCorretagem == 0 ? 0.000001M : info.ValorCorretagem) * 12) / ((info.ValorCarteiraMedia != 0 ? info.ValorCarteiraMedia : 1) * lTotalMeses));

                decimal lTotalDia = (info.ValorCorretagemDia == 0 ? 0.000001M : info.ValorCorretagemDia) / (info.ValorCorretagem != 0 ? info.ValorCorretagem : 1);
                
                info.PercentualCEnoPeriodo = lTotalPeriodo;
                
                info.PercentualCEnoDia = lTotalDia;

                //info.PercentualCEnoPeriodo = (lTotalVendasMes / lCarteira);
                //info.PercentualCEnoPeriodo = (info.ValorVendas / (lCarteira * 100));
                //info.PercentualCEnoDia     = (lTotalVendasDia / (info.ValorCarteiraDia == 0 ? 1 : info.ValorCarteiraDia));
                //info.PercentualCEnoDia = (info.ValorVendasDia / (info.ValorCarteiraDia == 0 ? 100 : (info.ValorCarteiraDia * 100)));
            }
        }

        private void EfetuaCalculoTR(ChurningIntradayInfo pRequest, ref ChurningIntradayInfo lRetorno)
        {
            TimeSpan lSpan = pRequest.DataAte - pRequest.DataDe;

            decimal lTotalMeses = Decimal.Parse((lSpan.TotalDays / 30).ToString());

            foreach (ChurningIntradayInfo info in lRetorno.Resultado)
            {
                /*
                decimal lTotalComprasMes = info.ValorCompras * 12M;
                decimal lTotalComprasDia = info.ValorComprasDia *0.03334M;
                decimal lCarteira = info.ValorCarteiraMedia * (lTotalMeses < 1 ? 1 : lTotalMeses);
                */

                decimal lTotalPeriodo = (((info.ValorCompras == 0 ? 0.000001M : info.ValorCompras) * 12) / ((info.ValorCarteiraMedia != 0 ? info.ValorCarteiraMedia : 1)* lTotalMeses));

                decimal lTotalDia = (info.ValorComprasDia == 0 ? 0.000001M : info.ValorComprasDia) / (info.ValorCompras != 0  ? info.ValorCompras : 1);

                info.PercentualTRnoPeriodo = lTotalPeriodo;

                info.PercentualTRnoDia = lTotalDia;


                //info.PercentualTRnoPeriodo = (lTotalComprasMes / lCarteira );
                //info.PercentualTRnoDia     = (lTotalComprasDia / (info.ValorCarteiraDia == 0 ? 1 : (info.ValorCarteiraDia)));

                //info.PercentualTRnoPeriodo = (info.ValorCompras / lCarteira);
                //info.PercentualTRnoDia = (info.ValorComprasDia / (info.ValorCarteiraDia == 0 ? 100 : (info.ValorCarteiraDia * 100)));
            }
        }

        private void EfetuaFiltroChurningIntradayPorta(ChurningIntradayInfo pRequest, ref ChurningIntradayInfo lRetorno)
        {
            IEnumerable<ChurningIntradayInfo> lRetornoValor = from a in lRetorno.Resultado select a;

            if (!string.IsNullOrWhiteSpace(  pRequest.Porta))
            {
                lRetornoValor = from a in lRetorno.Resultado where a.Porta!= null && a.Porta.Contains(pRequest.Porta) select a;
            }

            lRetorno.Resultado = lRetornoValor.ToList();
        }

        private void EfetuaFiltroChurningIntraday(ChurningIntradayInfo pRequest, ref ChurningIntradayInfo lRetorno)
        {
            IEnumerable<ChurningIntradayInfo> lRetornoValor;

            switch (pRequest.enumPercentualCE)
            {
                case enumPercentualCE.ABAIXO_10:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualCEnoPeriodo <= 10 select a;
                    break;
                case enumPercentualCE.ENTRE_10_E_15:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualCEnoPeriodo > 10 && a.PercentualCEnoPeriodo < 15 select a;
                    break;
                case enumPercentualCE.ENTRE_15_E_20:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualCEnoPeriodo > 15 && a.PercentualCEnoPeriodo < 20 select a;
                    break;
                case enumPercentualCE.ACIMA_20:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualCEnoPeriodo > 20 select a;
                    break;

                default:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
            }

            lRetorno.Resultado = lRetornoValor.ToList();

            switch (pRequest.enumPercentualTR)
            {
                case enumPercentualTR.ABAIXO_2:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualTRnoPeriodo <= 2 select a;
                    break;
                case enumPercentualTR.ENTRE_2_E_8:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualTRnoPeriodo > 2 && a.PercentualTRnoPeriodo < 8 select a;
                    break;
                case enumPercentualTR.ACIMA_8:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualTRnoPeriodo > 8 select a;
                    break;
                default:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
            }

            lRetorno.Resultado = lRetornoValor.ToList();

            //switch(pRequest.enumTotalCompras)
            //{
            //    case enumTotalCompras.ABAIXO_500M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorCompras <= 500000  select a;
            //        break;
            //    case enumTotalCompras.ENTRE_500M_E_1000M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorCompras > 500000 && a.ValorCompras < 1000000 select a;
            //        break;
            //    case enumTotalCompras.ACIMA_1000M :
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorCompras > 1000000 select a;
            //        break;
            //    default:
            //        lRetornoValor = from a in lRetorno.Resultado select a;
            //        break;
            //}

            //lRetorno.Resultado = lRetornoValor.ToList();

            //switch (pRequest.enumTotalVendas)
            //{
            //    case enumTotalVendas.ABAIXO_500M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorVendas <= 500000 select a;
            //        break;
            //    case enumTotalVendas.ENTRE_500M_E_1000M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorVendas > 500000 && a.ValorVendas < 1000000 select a;
            //        break;
            //    case enumTotalVendas.ACIMA_1000M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorVendas > 1000000 select a;
            //        break;
            //    default:
            //        lRetornoValor = from a in lRetorno.Resultado select a;
            //        break;
            //}

            //lRetorno.Resultado = lRetornoValor.ToList();

            //switch (pRequest.enumCarteiraMedia)
            //{
            //    case enumCarteiraMedia.ABAIXO_500M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorCarteiraMedia <= 500000 select a;
            //        break;
            //    case enumCarteiraMedia.ENTRE_500M_E_1000M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorCarteiraMedia > 500000 && a.ValorCarteiraMedia < 1000000 select a;
            //        break;
            //    case enumCarteiraMedia.ACIMA_1000M:
            //        lRetornoValor = from a in lRetorno.Resultado where a.ValorCarteiraMedia > 1000000 select a;
            //        break;
            //    default:
            //        lRetornoValor = from a in lRetorno.Resultado select a;
            //        break;
            //}

            //lRetorno.Resultado = lRetornoValor.ToList();

            if (pRequest.CodigoAssessor.HasValue)
            {
                List<int> lClientes = ClienteDbLib.ReceberListaClientesAssessoresVinculados(pRequest.CodigoAssessor.Value);

                lRetornoValor = from a in lRetornoValor where lClientes.Contains(a.CodigoCliente.Value) select a;

                lRetorno.Resultado = lRetornoValor.ToList();
            }

        }

        private List<ChannelClienteLista> ListarClientesPortas(ChurningIntradayInfo pRequest)
        {
            var lRetorno = new List<ChannelClienteLista>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CHANNELID_CLIENTE_DT_LST"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "DtDe", DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "DtAte", DbType.DateTime, pRequest.DataAte);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        string lCodigoCliente = lRow["CodigoCliente"].DBToString().Substring(0, lRow["CodigoCliente"].DBToString().Length - 2);

                        int CodigoCliente     = lCodigoCliente.DBToInt32();
                        int lChannelID        = lRow["ChannelID"].DBToInt32();

                        ChannelClienteLista lEncontrado = lRetorno.Find(channel=> { return channel.CodigoCliente == CodigoCliente; });

                        if (lEncontrado.CodigoCliente != 0)
                        {
                            lEncontrado.ListaPortas.Add(lChannelID);
                        }
                        else
                        {
                            var lChannelCliente = new ChannelClienteLista();

                            lChannelCliente.CodigoCliente = CodigoCliente;
                            lChannelCliente.ListaPortas = new List<int>();
                            lChannelCliente.ListaPortas.Add(lChannelID);

                            lRetorno.Add(lChannelCliente);
                        }
                    }
                }
            }

            return lRetorno;
        }

        private decimal SelecionaValorCorretagem(int CodigoCliente, DateTime DataPosicao)
        {
            decimal lRetorno = 0M;
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SinacorExportacao";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TURNOVER_CORRETA_DIA_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pData", DbType.DateTime, DataPosicao);

                lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, CodigoCliente);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lRetorno = lRow["VL_VALCOR"].DBToDecimal();
                    }

                }
            }

            return lRetorno;
        }

        private decimal SelecionarValorL1(int CodigoCliente, DateTime DataPosicao)
        {
            decimal lRetorno = 0M;
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_churning_exposicao_intraday_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@Data", DbType.DateTime, DataPosicao);
                
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32, CodigoCliente );

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lRetorno = lRow["ValorL1"].DBToDecimal();
                    }

                }
            }

            return lRetorno;
        }

        private string SelecionarClienteTipo(int CodigoCliente)
        {
            string lRetorno = string.Empty;
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TIPO_CLIENTE_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, CodigoCliente);
                
                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        /*
                        var lTipoCliente = new TipoCliente();

                        lTipoCliente.CodigoCliente = lRow["CodigoCliente"].DBToInt32();
                        lTipoCliente.TipoDeCliente = lRow["TipoCliente"].DBToString();
                        */
                        lRetorno = lRow["TipoCliente"].DBToString();
                        
                    }
                }
            }

            return lRetorno;
        }

        public DateTime GetDateBrockage(DateTime pDate, List<DateTime> pFeriados)
        {
            bool lEhDiaUtilIntervaloValido = false;

            int lDiasUteis = 0;

            while (lDiasUteis < 1)
            {
                pDate = pDate.AddDays(-1);

                lEhDiaUtilIntervaloValido = pDate.DayOfWeek == DayOfWeek.Saturday ||
                    pDate.DayOfWeek == DayOfWeek.Sunday || pFeriados.Contains(pDate);

                if (!lEhDiaUtilIntervaloValido)
                    lDiasUteis++;
            }

            return pDate;
        }
    }

    public struct ChannelClienteLista
    {
        public int CodigoCliente {get; set;}
        public List<int> ListaPortas {get; set;}
    }

    public struct TipoCliente
    {
        public int CodigoCliente    { get; set; }
        public string TipoDeCliente      { get; set; }
    }

    public struct CorretagemChurning
    {
        public int CodigoCliente { get; set; }
        public DateTime DataPosicao { get; set; }
        public decimal ValorCorretagem { get; set; }
        public decimal ValorCorretagemDia { get; set; }
    }
}
