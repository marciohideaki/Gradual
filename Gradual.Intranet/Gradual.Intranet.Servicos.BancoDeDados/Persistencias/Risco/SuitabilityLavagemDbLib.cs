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
    public class SuitabilityLavagemDbLib
    {
        public SuitabilityLavagemInfo ObterSuitabilityLavagem(SuitabilityLavagemInfo pRequest)
        {
            var lRetorno = new SuitabilityLavagemInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RiscoOMS";

            lRetorno.Resultado = new List<SuitabilityLavagemInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_suitability_lavagem_sel2"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente",   DbType.Int32,    pRequest.CodigoCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@DataDe",          DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DataAte",         DbType.DateTime, pRequest.DataAte);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        lRetorno.Resultado.Add(new SuitabilityLavagemInfo()
                        {
                            NomeAssessor      = lRow["NomeAssessor"].ToString(),
                            CodigoAssessor    = int.Parse(lRow["CodigoAssessor"].ToString()),
                            NomeCliente       = lRow["NomeCliente"].ToString(),
                            CodigoCliente     = int.Parse(lRow["CodigoCliente"].ToString()),
                            PercentualVOLxSFP = lRow["PercentualVOLxSFP"].DBToDecimal(),
                            Volume            = lRow["VolumeBovespa"].DBToDecimal(),
                            SFP               = decimal.Parse(lRow["SFP"].ToString()),
                            Data              = DateTime.Parse(lRow["Data"].ToString()),
                            CodigoClienteBmf  = int.Parse(lRow["CodigoClienteBmf"].ToString()),
                            ArquivoCiencia    = lRow["ArquivoCiencia"].ToString(),
                            ArquivoCienciaData = lRow["ArquivoCienciaData"].ToString(),
                            Suitability       = null
                        });
                    }
                }
            }

            EfetuaFiltroSuitabilityLavagem(pRequest, ref lRetorno);

            return lRetorno;
        }

        private List<SuitabilityClienteDataNaoEnquadrados> ObterSuitabilityStatusData(SuitabilityClienteDataNaoEnquadrados pRequest)
        {
            var lRetorno     = new List<SuitabilityClienteDataNaoEnquadrados>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SinacorExportacao";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_suitability_lavagem_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pDataDe", DbType.DateTime, pRequest.DataDe);
                lAcessaDados.AddInParameter(lDbCommand, "pDataAte", DbType.DateTime, pRequest.DataAte);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        lRetorno.Add(new SuitabilityClienteDataNaoEnquadrados()
                        {
                            CodigoBovespa = int.Parse(lRow["cd_cliente"].ToString()),
                            Data          = DateTime.Parse( lRow["data"].ToString())
                        });
                    }
                }
            }

            return lRetorno;
        }

        private List<SuitabilityClienteProduto> ObterProdutoPerfilSuitability()
        {
            var lRetorno = new  List<SuitabilityClienteProduto>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RiscoOMS";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_suitability_lavagem_sel"))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        lRetorno.Add(new SuitabilityClienteProduto()
                        {
                            Produto = lRow["ds_produto"].ToString(),
                            TipoPerfil = lRow["tipo_perfil"].ToString()
                        });
                    }
                }

            }

            return lRetorno;
        }

        private void EfetuaFiltroSuitabilityLavagem(SuitabilityLavagemInfo pRequest, ref SuitabilityLavagemInfo lRetorno)
        {
            IEnumerable<SuitabilityLavagemInfo> lRetornoValor;
            
            switch (pRequest.enumVolume)
            {
                case enumVolume.ABAIXO_500M:
                    lRetornoValor = from a in lRetorno.Resultado where a.Volume < 500000 select a;
                    break;
                case enumVolume.ENTRE_500M_E_1000M:
                    lRetornoValor = from a in lRetorno.Resultado where a.Volume > 500000 && a.Volume < 1000000 select a;
                    break;
                case enumVolume.ACIMA_1000M:
                    lRetornoValor = from a in lRetorno.Resultado where a.Volume > 1000000 select a;
                    break;
                default:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
            }

            lRetorno.Resultado = lRetornoValor.ToList();

            switch (pRequest.enumVOLxSFP)
            {
                case enumVOLxSFP.ABAIXO_20:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualVOLxSFP < 20 select a;
                    break;
                case enumVOLxSFP.ENTRE_20_E_50:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualVOLxSFP > 20 && a.PercentualVOLxSFP < 50 select a;
                    break;
                case enumVOLxSFP.ACIMA_50:
                    lRetornoValor = from a in lRetorno.Resultado where a.PercentualVOLxSFP > 50 select a;
                    break;

                default:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
            }

            lRetorno.Resultado = lRetornoValor.ToList();

            if (pRequest.CodigoAssessor.HasValue)
            {
                List<int> lClientes = ClienteDbLib.ReceberListaClientesAssessoresVinculados(pRequest.CodigoAssessor.Value);

                lRetornoValor = from a in lRetornoValor where lClientes.Contains(a.CodigoCliente.Value) select a;

                lRetorno.Resultado = lRetornoValor.ToList();
            }

            SuitabilityClienteDataNaoEnquadrados lRequest = new SuitabilityClienteDataNaoEnquadrados();
            lRequest.DataDe = pRequest.DataDe.Value;
            lRequest.DataAte = pRequest.DataAte.Value;

            List<SuitabilityClienteDataNaoEnquadrados> lListaSuitability =  this.ObterSuitabilityStatusData(lRequest);

            foreach (SuitabilityLavagemInfo info in  lRetorno.Resultado )
            {
                var lSuitability = lListaSuitability.Find(suit => { return suit.CodigoBovespa == info.CodigoCliente; });

                if (lSuitability != null)
                {
                    info.Suitability = "Não Enquadrado";
                }
                else
                {
                    info.Suitability = "Enquadrado";
                }
            }

            switch (pRequest.enumEnquadrado)
            {
                case enumEnquadrado.Todos:
                    lRetornoValor = from a in lRetorno.Resultado select a;
                    break;
                case enumEnquadrado.Enquadrado:
                    lRetornoValor = from a in lRetorno.Resultado where a.Suitability == "Enquadrado" select a;
                    break;

                case enumEnquadrado.NaoEnquadrado:
                    lRetornoValor = from a in lRetorno.Resultado where a.Suitability == "Não Enquadrado" select a;
                    break;
            }

            lRetorno.Resultado = lRetornoValor.ToList();

            List<int> lstClientesInstitucionais = ObterClientesInstitucionais();

            //lRetorno.Resultado = (from a in lRetornoValor where !lstClientesInstitucionais.Contains(a.CodigoCliente.Value) select a ).ToList();

            List<int> lstClientesInstitucionaisBmf = ObterClientesInstitucionaisBMF();

            lRetorno.Resultado = (from a in lRetorno.Resultado where !lstClientesInstitucionaisBmf.Contains(a.CodigoCliente.Value) && !lstClientesInstitucionais.Contains(a.CodigoCliente.Value) select a).ToList();
        }

        private List<int> ObterClientesInstitucionais()
        {
            var lRetorno     = new List<int>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTES_INT_LST"))
            {
                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];
                        
                        int lCodigoCliente = int.Parse(lRow["cd_cliente"].ToString());

                        if (!lRetorno.Contains((lCodigoCliente)))
                            lRetorno.Add( lCodigoCliente);
                    }
                }
            }

            return lRetorno;
        }

        private List<int> ObterClientesInstitucionaisBMF()
        {
            var lRetorno = new List<int>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "SINACOR";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTES_INT_BMF_LST"))
            {
                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow = lDataTable.Rows[i];

                        int lCodigoCliente = int.Parse(lRow["cd_cliente"].ToString());

                        if (!lRetorno.Contains((lCodigoCliente)))
                            lRetorno.Add(lCodigoCliente);

                    }
                }
            }

            return lRetorno;
        }
    }
}
