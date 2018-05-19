using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public class MonitoramentoIntradiarioDbLib
    {
        public MonitoramentoIntradiarioInfo ObterMonitoramentoIntradiario(MonitoramentoIntradiarioInfo pRequest)
        {
            var lRetorno       = new MonitoramentoIntradiarioInfo();
            var lAcessaDados   = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RiscoOMS"; 

            lRetorno.Resultado = new List<MonitoramentoIntradiarioInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_exposicao_intradiario_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente",       DbType.Int32, pRequest.CodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor",      DbType.Int32, pRequest.CodigoAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoNETxSFP",       DbType.Int32, pRequest.enumNETxSFP);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoEXPxExposicao", DbType.Int32, pRequest.enumEXPxPosicao);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoNet",           DbType.Int32, pRequest.enumNET);
                lAcessaDados.AddInParameter(lDbCommand, "@DataDe",              DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DataAte",             DbType.DateTime, pRequest.DataAte);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i  = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        lRetorno.Resultado.Add(new MonitoramentoIntradiarioInfo() 
                        {
                            NomeAssessor     = lRow["NomeAssessor"].ToString(),
                            CodigoAssessor   = int.Parse( lRow["CodigoAssessor"].ToString()),
                            NomeCliente      = lRow["NomeCliente"].ToString(),
                            CodigoCliente    = int.Parse( lRow["CodigoCliente"].ToString()),
                            EXPxPosicao      = decimal.Parse(lRow["EXPxPosicao"].ToString()),
                            Net              = decimal.Parse(lRow["Net"].ToString()),
                            NETxSFP          = decimal.Parse(lRow["NETxSFP"].ToString()),
                            SFP              = decimal.Parse(lRow["SFP"].ToString()),
                            Posicao          = lRow["Posicao"].ToString()== "" ? 0 : decimal.Parse(lRow["Posicao"].ToString()),
                            Exposicao        = lRow["Exposicao"].ToString() == "" ? 0 :  decimal.Parse(lRow["Exposicao"].ToString()),
                            Data             = DateTime.Parse( lRow["Data"].ToString()),
                            CodigoClienteBmf = int.Parse(lRow["CodigoClienteBmf"].ToString())
                        });
                    }
                }
            }

            EfetuaFiltroMonitoramentoIntradiario(pRequest, ref lRetorno);

            return lRetorno;
        }

        private void EfetuaFiltroMonitoramentoIntradiario(MonitoramentoIntradiarioInfo pRequest, ref MonitoramentoIntradiarioInfo lRetorno)
        {
            IEnumerable<MonitoramentoIntradiarioInfo> lRetornoValor;

            switch (pRequest.enumEXPxPosicao)
            {
                case EnumEXPxPosicao.ABAIXO_20:
                    lRetornoValor = from a in lRetorno.Resultado where  a.EXPxPosicao  < 20 select a;
                    break;
                case EnumEXPxPosicao.ENTRE_20_E_50:
                    lRetornoValor = from a in lRetorno.Resultado where a.EXPxPosicao > 20 && a.EXPxPosicao < 50 select a;
                    break;
                case EnumEXPxPosicao.ACIMA_50:
                    lRetornoValor = from a in lRetorno.Resultado where a.EXPxPosicao > 50 select a;
                    break;

                default:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
            }

            lRetorno.Resultado = lRetornoValor.ToList();

            switch(pRequest.enumNET)
            {
                case EnumNet.ABAIXO_500_MIL:
                    lRetornoValor = from a in lRetorno.Resultado where a.Net < 500000 select a;
                    break;
                case EnumNet.ENTRE_500_1000_MIL:
                    lRetornoValor = from a in lRetorno.Resultado where a.Net > 500000 && a.Net < 1000000 select a;
                    break;
                case EnumNet.ACIMA_1000:
                    lRetornoValor = from a in lRetorno.Resultado where a.Net > 1000000 select a;
                    break;
                default:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
            }
            
            lRetorno.Resultado = lRetornoValor.ToList();

            switch (pRequest.enumNETxSFP)
            {
                case EnumNETxSFP.ABAIXO_20:
                    lRetornoValor = from a in lRetorno.Resultado where a.NETxSFP < 20  select a;
                    break;
                case EnumNETxSFP.ENTRE_20_E_50:
                    lRetornoValor = from a in lRetorno.Resultado where a.NETxSFP > 20 && a.NETxSFP < 50  select a;
                    break;
                case EnumNETxSFP.ACIMA_50:
                    lRetornoValor = from a in lRetorno.Resultado where a.NETxSFP > 50  select a;
                    break;

                default:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
            }

            lRetorno.Resultado = lRetornoValor.ToList();

            if (pRequest.CodigoAssessor.HasValue)
            {
                List<int> lClientes =  ClienteDbLib.ReceberListaClientesAssessoresVinculados(pRequest.CodigoAssessor.Value);

                lRetornoValor = from a in lRetornoValor where lClientes.Contains(a.CodigoCliente.Value) select a;

                lRetorno.Resultado = lRetornoValor.ToList();
            }

        }
    }
}
