using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Generico.Dados;
using Gradual.Site.DbLib.Dados.MinhaConta;
using System.Data.Common;
using System.Data;
using Gradual.Site.DbLib.Dados;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Contratos.Dados.Risco;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta
{
    public class PersistenciaMinhaConta
    {
        public ContaBancariaResponse BuscarContasBancariasDoCliente(ContaBancariaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados("ACCOUNT");

            ContaBancariaResponse retorno = new ContaBancariaResponse();

            retorno.ListaContaBancaria = new List<ContaBancariaInfo>();

            ContaBancariaInfo ContaBancaria;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSinacor;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_SINACOR_SEL_CONTAS"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "cd_cliente", DbType.Int32, pRequest.ContaBancaria.CodigoCliente);
               

                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    ContaBancaria = new ContaBancariaInfo();

                    ContaBancaria.CodigoDaEmpresa = linha["cd_empresa"].DBToInt32();

                    ContaBancaria.NumeroDaAgencia = linha["cd_agencia"].DBToString();
                    
                    ContaBancaria.DigitoDaAgencia = linha["dv_agencia"].DBToString();
                    
                    ContaBancaria.NumeroDaConta = linha["nr_conta"].DBToString();
                    
                    ContaBancaria.DigitoDaConta = linha["dv_conta"].DBToString();
                    
                    ContaBancaria.NomeDoBanco = linha["nm_banco"].DBToString();
                    
                    ContaBancaria.CodigoDoBanco = linha["cd_banco"].DBToString();

                    retorno.ListaContaBancaria.Add(ContaBancaria);

                }

            }

            return retorno;
        }

        public ContaCorrenteResponse ObterSaldoContaCorrente(ContaCorrenteRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();

            try
            {
                ContaCorrenteResponse lRetorno = new ContaCorrenteResponse();

                lRetorno.ContaCorrente = new Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo();

                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pccsaldoprojetado"))
                {
                    lRetorno.ContaCorrente.IdClienteSinacor = pRequest.ContaCorrente.IdClienteSinacor;

                    lAcessaDados.AddInParameter(lDbCommand, "PCODCLIENTE", DbType.AnsiString, pRequest.ContaCorrente.IdClienteSinacor);

                    lAcessaDados.AddOutParameter(lDbCommand, "PSALDO"   , DbType.Decimal , 12);
                    lAcessaDados.AddOutParameter(lDbCommand, "PD1"      , DbType.Decimal , 12);
                    lAcessaDados.AddOutParameter(lDbCommand, "PD2"      , DbType.Decimal , 12);
                    lAcessaDados.AddOutParameter(lDbCommand, "PD3"      , DbType.Decimal , 12);



                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lRetorno.ContaCorrente.SaldoD0 = lAcessaDados.GetParameterValue(lDbCommand, "PSALDO").DBToDecimal() ;
                    lRetorno.ContaCorrente.SaldoD1 = lAcessaDados.GetParameterValue(lDbCommand, "PD1").DBToDecimal()    ;
                    lRetorno.ContaCorrente.SaldoD2 = lAcessaDados.GetParameterValue(lDbCommand, "PD2").DBToDecimal()    ;
                    lRetorno.ContaCorrente.SaldoD3 = lAcessaDados.GetParameterValue(lDbCommand, "PD3").DBToDecimal()    ;
                    lRetorno.ContaCorrente.SaldoContaMargem = this.ObtemSaldoContaMargem(pRequest.ContaCorrente.IdClienteSinacor.DBToInt32());
                    lRetorno.ContaCorrente.SaldoBloqueado = this.ObtemSaldoBloqueado(pRequest.ContaCorrente.IdClienteSinacor.DBToInt32());



                    this.ConsultarLimiteOperacionalDisponivel(lRetorno.ContaCorrente);


                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal ObterSaldoAbertura(Int32 pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            decimal lRetorno = 0;
            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                //String lSql = String.Format("select * from CORRWIN.DBMFINAN33  where cd_cliente = {0} and DATA_POSI = fccpregao(-1)", pRequest);

                //String lSql = String.Format("SELECT CD_CLIENTE, DT_REFERENCIA, VL_TOTAL, VL_DISPONIVEL, VL_PROJETADO1, VL_PROJETADO2 FROM VGATCCSALREF WHERE CD_CLIENTE = {0}", pRequest);

                String lSql = String.Format("SELECT * FROM TCCSALDO WHERE CD_CLIENTE = {0}", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lRetorno = dt.Rows[0]["VL_DISP_TRIBUTAVEL"].DBToDecimal();
                    }
                    
                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime ObterDataPregao(Int32 pDias)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            DateTime lRetorno = DateTime.Now;
            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                String lSql = String.Format("select fccpregao({0}) as DataPregao from dual", pDias);
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lRetorno = dt.Rows[0]["DataPregao"].DBToDateTime();
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustodiaBTC> ObterBTC(Int32 pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            List<CustodiaBTC> lRetorno = new List<CustodiaBTC>();   

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                //String lSql = String.Format(
                //                            "SELECT * FROM (                                                        " +
                //                            "SELECT                                                                 " +
                //                            "        A.COD_CLI  as CodigoCliente,                                   " +
                //                            "        C.NM_CLIENTE as NomeCliente,                                   " +
                //                            "        A.TIPO_COTR as TipoContrato,                                   " +
                //                            "        A.COD_ISIN as CodigoISIN,                                      " +
                //                            "        A.COD_NEG as CodigoNegocio,                                    " +
                //                            "        'BTC' as Tipo,                                                 " +
                //                            "        A.COD_CART as CodigoCarteira,                                  " +
                //                            "        A.DATA_ORI as DataOrigem,                                      " +
                //                            "        A.DATA_ABER as DataAbertura,                                   " +
                //                            "        A.DATA_VENC as DataVencimento,                                 " +
                //                            "        A.QTDE_ACOE as QuantidadeAcoes,                                " +
                //                            "        A.PREC_DIA_ANTE as PrecoDiaAnterior,                           " +
                //                            "        A.PREC_MED as PrecoMedio,                                      " +
                //                            "        (A.TAXA_REMU + A.TAXA_COMI) AS TAXA_REMU as TaxaRemuneracao,   " +
                //                            "        A.VAL_LIQ as ValorLiquidacao                                   " +
                //                            "FROM VCFPOSI_BTC A,TSCCLIBOL B, TSCCLIGER C        " +
                //                            "WHERE                                              " +
                //                            "--a.cod_cli       =       IdCliente         AND    " +
                //                            "A.COD_CLI         =       B.CD_CLIENTE      AND    " +
                //                            "B.CD_CPFCGC       =       C.CD_CPFCGC       AND    " +
                //                            "B.DT_NASC_FUND    =       C.DT_NASC_FUND    AND    " +
                //                            "B.CD_CON_DEP      =       C.CD_CON_DEP      AND    " +
                //                            "A.TIPO_COTR = 'T'                                  " +
                //                            "AND A.COD_AGCO    = 120                            " +
                //                            "                                                   " +
                //                            "UNION ALL                                          " +
                //                            "                                                   " +
                //                            "SELECT                                             " +
                //                            "        A.COD_CLI,                                 " +
                //                            "        C.NM_CLIENTE,                              " +
                //                            "        A.TIPO_COTR,                               " +
                //                            "        A.COD_ISIN,                                " +
                //                            "        A.COD_NEG,                                 " +
                //                            "        'BTC',                                     " +
                //                            "        A.COD_CART,                                " +
                //                            "        A.DATA_ORI,                                " +
                //                            "        A.DATA_ABER,                               " +
                //                            "        A.DATA_VENC,                               " +
                //                            "        A.QTDE_ACOE,                               " +
                //                            "        A.PREC_DIA_ANTE,                           " +
                //                            "        A.PREC_MED,                                " +
                //                            "        A.TAXA_REMU,                               " +
                //                            "        A.VAL_LIQ                                  " +
                //                            "FROM VCFPOSI_BTC A,TSCCLIBOL B, TSCCLIGER C        " +
                //                            "WHERE                                              " +
                //                            "a.cod_cli         =       2861         AND         " +
                //                            "A.COD_CLI         =       B.CD_CLIENTE      AND    " +
                //                            "B.CD_CPFCGC       =       C.CD_CPFCGC       AND    " +
                //                            "B.DT_NASC_FUND    =       C.DT_NASC_FUND    AND    " +
                //                            "B.CD_CON_DEP      =       C.CD_CON_DEP      AND    " +
                //                            "A.TIPO_COTR = 'D'                                  " +
                //                            "AND A.COD_AGCO    = 120 )", pRequest);

                String lSql = String.Format(
                                                " SELECT                                        " +
                                                "     num_cotr  AS NumeroContrato               " +
                                                "     , cod_cli_ori AS CodigoCliente            " +
                                                "     , nome_cli AS NomeCliente                 " +
                                                "     , cod_asse AS CodigoAssessor              " +
                                                "     , nome_asse AS NomeAssessor               " +
                                                "     , tipo_cotr AS TipoContrato               " +
                                                "     , cod_neg AS CodigoNegocio                " +
                                                "     , qtde_acoe Quantidade                    " +
                                                "     , data_aber AS Abertura                   " +
                                                "     , data_venc AS Vencimento                 " +
                                                "     , num_dias AS NumeroDias                  " +
                                                "     , prec_med AS Cotacao                     " +
                                                "     , taxa_remu AS Remuneracao                " +
                                                "     , taxa_comi AS Comissao                   " +
                                                "     , (taxa_comi +  taxa_remu) AS TaxaFinal   " +
                                                "     , cod_conp AS Contraparte                 " +
                                                "     , (qtde_acoe * tb_cotacao.vl_preco) AS Financeiro    " +
                                                "     , TRUNC(((taxa_comi/100)*(prec_med * qtde_acoe)),8) AS ValorComissaoAno                                    " +
                                                "     , TRUNC(((Power((1+(taxa_comi/100)),1/252)-1)*(prec_med * qtde_acoe)),8) AS ValorComissaoDia               " +
                                                "     , TRUNC(((Power((1+(taxa_comi/100)),22/252)-1)*(prec_med * qtde_acoe)),8) AS ValorComissaoMes              " +
                                                "     , TRUNC(((Power((1+(taxa_comi/100)),(num_dias/252))-1)*(prec_med * qtde_acoe)),8) AS ValorComissaoALiquidar" +
                                                "     , CASE WHEN TRUNC(((Power((1+(0.0025)),(num_dias/252))-1)*(prec_med * qtde_acoe)),8) <10 THEN 10 ELSE TRUNC(((Power((1+(0.0025)),(num_dias/252))-1)*(prec_med * qtde_acoe)),8) END AS Custo " +
                                                " FROM corrwin.vcfposi_btc  " +
                                                " inner join tb_cotacao on tb_cotacao.ds_ativo = corrwin.vcfposi_btc.cod_neg " +
                                                " WHERE qtde_acoe <> 0      " +
                                                " AND cod_cli_ori = {0}     " +
                                                " ORDER BY  num_cotr", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            CustodiaBTC lCustodia = new CustodiaBTC(lRow);

                            lRetorno.Add(lCustodia);
                        }
                        

                    }
                    
                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustodiaTermo> ObterTermo(Int32 pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            List<CustodiaTermo> lRetorno = new List<CustodiaTermo>();
            

            try
            {

                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                String lSql = String.Format(
                                                " SELECT                                            " +
                                                "cod_cli AS CodigoCliente                           " +
                                                ", nome_cli AS NomeCliente                          " +
                                                ", cod_neg AS CodigoNegocio                         " +
                                                ", qtde_disp AS QuantidadeDisponivel                " +
                                                ", qtde_ori AS QuantidadeOriginal                   " +
                                                ", val_nego AS PrecoTermo                           " +
                                                ", prec_med AS PrecoMadioD1                         " +
                                                ", (val_nego*qtde_disp*-1) AS FinanceiroATermo      " +
                                                ", (prec_med*qtde_disp) AS FinanceiroD1             " +
                                                ", (prec_lqdo-prec_brut) AS CustoTermo              " +
                                                ", ((val_nego*qtde_disp*-1)+(prec_med*qtde_disp)) AS ResultadoTermoD1 " +
                                                ", ABS((tb_cotacao.vl_preco*qtde_disp))-ABS((val_nego*qtde_disp)) AS ResultadoTermo " +
                                                ", data_preg AS DataAbertura                       " +
                                                ", data_venc AS DataVencimento                     " +
                                                ", (qtde_disp*tb_cotacao.vl_preco) as Financeiro    " +
                                                "FROM corrwin.VCFPOSI_TERM                          " +
                                                " inner join tb_cotacao on tb_cotacao.ds_ativo = corrwin.VCFPOSI_TERM.cod_neg " +
                                                "WHERE qtde_disp <> 0 and cod_cli = {0}             " +
                                                "ORDER BY cod_neg, data_venc", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            CustodiaTermo lCustodia = new CustodiaTermo(lRow);

                            //if (lTermoALiquidar != null)
                            //{
                            //    if (lTermoALiquidar.AsEnumerable().Where(x => x.CodigoNegocio.Equals(String.Format("{0}T", lCustodia.CodigoNegocio))).Count() > 0)
                            //    {
                            //        List<CustodiaTermo> lCustodiaTermo = lTermoALiquidar.AsEnumerable().Where(x => x.CodigoNegocio.Equals(String.Format("{0}T", lCustodia.CodigoNegocio))).ToList();

                            //        foreach (CustodiaTermo lOcorrencia in lCustodiaTermo)
                            //        {
                            //            lCustodia.QuantidadeDisponivel += lOcorrencia.QuantidadeDisponivel;
                            //        }
                            //    }
                            //}

                            lRetorno.Add(lCustodia);
                        }
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustodiaTermo> ObterTermoALiquidar(Int32 pRequest)
        {
            AcessaDados lAcessaDados        = new AcessaDados();
            Conexao Conexao                 = new Generico.Dados.Conexao();
            List<CustodiaTermo> lRetorno    = new List<CustodiaTermo>();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                String lSql = String.Format(
                                                " SELECT DISTINCT                                   " +
                                                " COD_CLI as Codigocliente                          " +
                                                " , COD_NEG as CodigoNegocio                        " +
                                                " , COD_ISIN as CodigoISIN                          " +
                                                " , DATA_LIQD as DataLiquidacao                     " +
                                                " , SUM(QTDE_PAP_LQDO) as QuantidadeALiquidar       " +
                                                " FROM CORRWIN.TCFCONL a                            " +
                                                " WHERE DATA_LIQD >= TRUNC(sysdate)                 " +
                                                " AND COD_CLI = {0}                                 " +
                                                " AND IND_POSI = 'C'                                " +
                                                " GROUP BY COD_CLI, COD_NEG, COD_ISIN,  DATA_LIQD   " +
                                                " ORDER BY DATA_LIQD", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            CustodiaTermo lCustodia = new CustodiaTermo();
                            lCustodia.CustodiaTermoALiquidar(lRow);

                            lRetorno.Add(lCustodia);
                        }
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustodiaTesouro> ObterTesouroDireto(Int32 pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            List<CustodiaTesouro> lRetorno = new List<CustodiaTesouro>();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                String lSql = String.Format("select * from tcfposi_tedi where cod_cli = {0} and val_posi > 0 and qtde_titu > 0 ", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            CustodiaTesouro lCustodia = new CustodiaTesouro(lRow);

                            lRetorno.Add(lCustodia);
                        }
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DateTime? ObterDataPosicaoFundo(String pCodigoAnbima, decimal pCota)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            DateTime? lRetorno = null;
            try
            {
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                String lSql = String.Format("select top 100 * from tbANBIMAFundosDia where CodFundo = '{0}' and valcota={1} order by data desc", pCodigoAnbima, pCota.ToString().Replace(",","."));

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lRetorno = dt.Rows[0]["Data"].DBToDateTime();
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Garantia> ObterGarantiaDinheiro(Int32 pRequest)
        {
            AcessaDados lAcessaDados        = new AcessaDados();
            Conexao Conexao                 = new Generico.Dados.Conexao();
            List<Garantia> lRetorno         = new List<Garantia>();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                String lSql = String.Format("SELECT * FROM TCFDGAR WHERE DATA_MVTO = TRUNC(SYSDATE) AND DATA_DEPO < TRUNC(SYSDATE) AND COD_ATIV LIKE '%DINHEIRO%' AND COD_CLI = {0} ", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            Garantia lCustodia = new Garantia(lRow);

                            lRetorno.Add(lCustodia);
                        }
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Provento> ObterGarantiaDividendo(Int32 pRequest)
        {
            AcessaDados lAcessaDados    = new AcessaDados();
            Conexao Conexao             = new Generico.Dados.Conexao();
            List<Provento> lRetorno     = new List<Provento>();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                String lSql = String.Format("SELECT DISTINCT A.*, B.COD_NEG FROM TCFDGAR A INNER JOIN TCFPAP_MERC B ON B.COD_ISIN = A.COD_ISIN WHERE  COD_ATIV LIKE 'DIVI%' AND B.TIPO_MERC = 'VIS' AND COD_CLI = {0}", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            Provento lProvento = new Provento();

                            lProvento.CodigoCliente = lRow["COD_CLI"].DBToInt32();
                            lProvento.DataPagamento = lRow["DATA_MVTO"].DBToDateTime();

                            String lDescricao = String.Empty;

                            switch(lRow["COD_ATIV"].DBToString().Trim())
                            {
                                case "DIVI":
                                {
                                    lDescricao = "DIVIDENDO";
                                    break;
                                }
                                default:
                                {
                                    lDescricao = lRow["COD_ATIV"].DBToString().Trim();
                                    break;
                                }
                            }

                            lProvento.Evento        = lDescricao;
                            lProvento.Ativo         = lRow["COD_NEG"].DBToString();
                            lProvento.Quantidade    = lRow["QTDE_GARN"].DBToInt32();
                            lProvento.Valor         = lRow["VAL_GARN_DEPO"].DBToDecimal();

                            lRetorno.Add(lProvento);
                        }
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GarantiaBMF> ObterGarantiaDinheiroBMF(Int32 pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            List<GarantiaBMF> lRetorno = new List<GarantiaBMF>();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
                String lSql = String.Format("SELECT * FROM CORRWIN.TMFATIVO a INNER JOIN CORRWIN.TMFTIPO_ATIV b on a.CD_ATIVO = b.CD_ATIVO WHERE CD_CLIENTE = {0} AND TRIM(b.DS_ATIVO) = 'M. Nacional' and DT_DATMOV = fccpregao(-1)", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            GarantiaBMF lCustodia = new GarantiaBMF(lRow);

                            lRetorno.Add(lCustodia);
                        }
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ChamadaMargem> ObterChamadaMargem(Int32 pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            List<ChamadaMargem> lRetorno = new List<ChamadaMargem>();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;

                String lSql = String.Format("SELECT Bolsa, CodigoCliente, DataLancamento, Descricao, sum(vl_debito) as ValorDebito, sum(vl_credito) as ValorCredito, sum(vl_lancamento) as ValorLancamento FROM " +
                                            " ( " +
                                            "   SELECT 'BOV' as Bolsa, cd_cliente as CodigoCliente, dt_lancamento as DataLancamento, 'CHAMADA DE MARGEM BOV' as Descricao, vl_debito, vl_credito, vl_lancamento FROM V_TCCMOVTO " +
                                            "   where cd_historico in (18,19) " +
                                            "   UNION ALL " +
                                            "   SELECT 'BMF' as Bolsa, cd_cliente as CodigoCliente, dt_lancamento as DataLancamento, 'CHAMADA DE MARGEM BMF' as Descricao, vl_debito, vl_credito, vl_lancamento FROM V_TCCMOVTO " +
                                            "   where cd_historico in (567,568) " +
                                            " ) " +
                                            " WHERE DataLancamento = TRUNC(SYSDATE) " +
                                            " AND CodigoCliente = {0} " +
                                            " group by Bolsa, CodigoCliente, DataLancamento, Descricao", pRequest);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow lRow in dt.Rows)
                        {
                            ChamadaMargem lChamadaMargem = new ChamadaMargem(lRow);
                            lRetorno.Add(lChamadaMargem);
                        }
                    }

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Provento> ObterProventos(Int32 pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;
            List<Provento> lListaProventos = new List<Provento>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_PROVENTOS_LST"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "pDataDe", DbType.DateTime, ObterDataPregao(0));
                _AcessaDados.AddInParameter(_DbCommand, "pDataAte", DbType.DateTime, ObterDataPregao(30));

                if (pRequest > 0)
                {
                    _AcessaDados.AddInParameter(_DbCommand, "pClienteCodigo", DbType.Int32, pRequest);
                }

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    Provento lProvento = new Provento();

                    lProvento.CodigoCliente = item["CD_CLIENTE"].DBToInt32();
                    lProvento.DataPagamento = item["DT_PAGAMENTO"].DBToDateTime();

                    String lDescricao = String.Empty;

                    switch (item["TP_PROVENTO"].DBToString().Trim())
                    {
                        case "JUROS SOBRE CAPITAL PROPRIO":
                            {
                                lDescricao = "JCP";
                                break;
                            }
                        default:
                            {
                                lDescricao = item["TP_PROVENTO"].DBToString().Trim();
                                break;
                            }
                    }

                    lProvento.Evento        = lDescricao;
                    lProvento.Ativo         = item["DS_ATIVO"].DBToString();
                    lProvento.Quantidade    = item["VL_QUANTIDADE"].DBToInt32();
                    lProvento.Valor         = item["VL_VALOR"].DBToDecimal();

                    lListaProventos.Add(lProvento);
                }
            }

            return lListaProventos;
        }

        public List<ResgateFundo> ObterResgateFundo(Int32 pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastro;
            List<ResgateFundo> lListaResgates = new List<ResgateFundo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "GET_RESGATESCOTISTA"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CodigoCliente", DbType.String, pRequest);
                
                DataTable lDataTable = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach(DataRow lRow in lDataTable.Rows)
                {
                    ResgateFundo lResgate = new ResgateFundo(lRow);
                    lListaResgates.Add(lResgate);
                }
            }

            return lListaResgates;
        }

        public DateTime? ObterDataProcessamentoFundo(Int32 pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();
            Conexao Conexao = new Generico.Dados.Conexao();
            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoFinancial;
            List<ResgateFundo> lListaResgates = new List<ResgateFundo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, String.Format("SELECT * FROM Cliente WHERE IdCliente = {0}", pRequest)))
            {
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if(_table.Rows.Count > 0)
                {
                    return _table.Rows[0]["DataDia"].DBToDateTime();
                }
            }

            return null;
        }

        public List<PosicaoFundo> ObterPosicaoFundo(Int32 pRequest)
        {
            AcessaDados _AcessaDados            = new AcessaDados();
            Conexao Conexao                     = new Generico.Dados.Conexao();
            _AcessaDados.ConnectionStringName   = ServicoPersistenciaSite.ConexaoCadastro;
            List<PosicaoFundo> lListaPosicoes   = new List<PosicaoFundo>();

            //System.String lSQL = String.Empty;
            //lSQL += "SELECT * FROM PosicaoCotista WITH (NOLOCK)";
            //lSQL += " INNER JOIN Carteira ON Carteira.IdCarteira = PosicaoCotista.IdCarteira";
            //lSQL += " INNER JOIN Cliente ON Cliente.IdCliente = Carteira.IdCarteira";
            //lSQL += " WHERE IdCotista = {0}";
            //lSQL += " AND ValorBruto > 0";

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "GET_POSICAOCOTISTA"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CodigoCliente", DbType.String, pRequest);
                
                DataTable lDataTable = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach(DataRow lRow in lDataTable.Rows)
                {
                    PosicaoFundo lPosicao = new PosicaoFundo(lRow);
                    lListaPosicoes.Add(lPosicao);
                }
            }

            return lListaPosicoes;
        }

        public CustodiaResponse ObterPosicaoAtual(int CBLC)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;

            CustodiaResponse Retorno = new CustodiaResponse();

            Retorno.ListaCustodia = new List<CustodiaInfo>();

            CustodiaInfo Custodia;

            //using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_custodia_bove"))
            //{
            //    _AcessaDados.AddInParameter(_DbCommand, "IdCliente", DbType.String, CBLC);

            //    DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

            //    string lCotacao = "";

            //    decimal lFatorCotacao;

            //    foreach (DataRow item in _table.Rows)
            //    {
            //        Custodia = new CustodiaInfo();

            //        Custodia.CodigoAtivo                = item["cod_neg"].DBToString();

            //        Custodia.NomeAtivo                  = item["NOME_EMP_EMI"].DBToString();

            //        Custodia.Mercado                    = item["tipo_merc"].DBToString();

            //        Custodia.TipoCarteira               = item["desc_cart"].DBToString();

            //        Custodia.TipoGrupo                  = item["tipo_grup"].DBToString();

            //        Custodia.QuantidadeTotal            = item["qtde_atual"].DBToDecimal();

            //        Custodia.SaldoD1                    = item["qtde_da1"].DBToDecimal();

            //        Custodia.SaldoD2                    = item["qtde_da2"].DBToDecimal();

            //        Custodia.SaldoD3                    = item["qtde_da3"].DBToDecimal();

            //        Custodia.QuantidadeAexecutarCompra  = item["qtde_aexe_cpa"].DBToDecimal();

            //        Custodia.QuantidadeAexecutarVenda   = item["qtde_aexe_vda"].DBToDecimal();

            //        Custodia.DataVencimento             = item["DATA_VENC"].DBToDateTime();

            //        Custodia.ValorAtual                 = item["val_posi"].DBToDecimal();

            //        lFatorCotacao                       = item["FAT_COT"].DBToDecimal();

            //        lCotacao = "0";

            //        try
            //        {
            //            lCotacao = this.ObterUltimaCotacao(Custodia.CodigoAtivo);
            //        }
            //        catch { }

            //        Custodia.ValorCotacao   = Convert.ToDecimal(lCotacao != "" ? lCotacao : "0");//indefinido, buscar do sinal

            //        if (Custodia.TipoGrupo == "ACAO")
            //        {
            //            if (lFatorCotacao == 0) lFatorCotacao = 1;

            //            Custodia.ValorFinanceiro = (Custodia.QuantidadeTotal * Custodia.ValorCotacao) / lFatorCotacao;
            //        }
            //        else
            //        {
            //            Custodia.ValorFinanceiro = Custodia.ValorAtual;
            //        }

            //        Retorno.ListaCustodia.Add(Custodia);
            //    }
            //}

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET2"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "IdCliente", DbType.String, CBLC);
                _AcessaDados.AddInParameter(_DbCommand, "IdClienteBMF", DbType.Int32, null);

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                string lCotacao = "";

                decimal lFatorCotacao;

                foreach (DataRow item in _table.Rows)
                {
                    Custodia = new CustodiaInfo();

                    Custodia.CodigoAtivo = item["COD_NEG"].DBToString();

                    Custodia.NomeAtivo = item["NOME_EMP_EMI"].DBToString();

                    Custodia.Mercado = item["TIPO_MERC"].DBToString();

                    Custodia.TipoCarteira = item["DESC_CART"].DBToString();

                    Custodia.TipoGrupo = item["TIPO_GRUP"].DBToString();

                    Custodia.QuantidadeTotal = item["QTDE_ATUAL"].DBToDecimal();

                    Custodia.SaldoD1 = item["QTDE_DA1"].DBToDecimal();

                    Custodia.SaldoD2 = item["QTDE_DA2"].DBToDecimal();

                    Custodia.SaldoD3 = item["QTDE_DA3"].DBToDecimal();

                    Custodia.QuantidadeAexecutarCompra = item["QTDE_AEXE_CPA"].DBToDecimal();

                    Custodia.QuantidadeAexecutarVenda = item["QTDE_AEXE_VDA"].DBToDecimal();

                    Custodia.DataVencimento = item["DATA_VENC"].DBToDateTime();

                    Custodia.ValorAtual = item["VAL_POSI"].DBToDecimal();

                    lFatorCotacao = item["FAT_COT"].DBToDecimal();

                    lCotacao = "0";

                    try
                    {
                        lCotacao = this.ObterUltimaCotacao(Custodia.CodigoAtivo);
                    }
                    catch { }

                    Custodia.ValorCotacao = Convert.ToDecimal(lCotacao != "" ? lCotacao : "0");//indefinido, buscar do sinal

                    if (Custodia.TipoGrupo == "ACAO")
                    {
                        if (lFatorCotacao == 0) lFatorCotacao = 1;

                        Custodia.ValorFinanceiro = (Custodia.QuantidadeTotal * Custodia.ValorCotacao) / lFatorCotacao;
                    }
                    else
                    {
                        Custodia.ValorFinanceiro = Custodia.ValorAtual;
                    }

                    Retorno.ListaCustodia.Add(Custodia);
                }
            }

            return Retorno;
        }

        public CustodiaResponse ObterPosicaoAtualBMF(int CBLC)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;

            CustodiaResponse Retorno = new CustodiaResponse();

            Retorno.ListaCustodia = new List<CustodiaInfo>();

            CustodiaInfo Custodia;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_custodia_bmf"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "IdCliente", DbType.String, CBLC);

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                
                string cotacao = "";

                foreach (DataRow item in _table.Rows)
                {
                    Custodia = new CustodiaInfo();

                    Custodia.CodigoAtivo                = item["cod_neg"].DBToString();

                    Custodia.NomeAtivo                  = item["NOME_EMP_EMI"].DBToString();

                    Custodia.Mercado                    = item["tipo_merc"].DBToString();

                    Custodia.TipoCarteira               = item["desc_cart"].DBToString();

                    Custodia.QuantidadeTotal            = item["qtde_atual"].DBToDecimal();

                    Custodia.ValorFinanceiro            = Custodia.QuantidadeTotal * Custodia.ValorCotacao;

                    Custodia.SaldoD1                    = item["qtde_da1"].DBToDecimal();

                    Custodia.SaldoD2                    = item["qtde_da2"].DBToDecimal();

                    Custodia.SaldoD3                    = item["qtde_da3"].DBToDecimal();

                    Custodia.QuantidadeAexecutarCompra  = item["qtde_aexe_cpa"].DBToDecimal();

                    Custodia.QuantidadeAexecutarVenda   = item["qtde_aexe_vda"].DBToDecimal();

                    Custodia.DataVencimento             = item["DATA_VENC"].DBToDateTime();

                    Custodia.TipoGrupo                  = item["Tipo_Grup"].ToString();

                    cotacao                 = this.ObterUltimaCotacao(Custodia.CodigoAtivo);

                    Custodia.ValorCotacao   = Convert.ToDecimal(cotacao != "" ? cotacao : "0");//indefinido, buscar do sinal

                    Retorno.ListaCustodia.Add(Custodia);
                }
            }

            return Retorno;
        }

        public UltimasNegociacoesResponse ConsultarUltimasNegociacoesCliente(UltimasNegociacoesRequest pRequest)
        {
            UltimasNegociacoesResponse lRetorno = new UltimasNegociacoesResponse();

            lRetorno.ListaUltimasNegociacoes = new List<UltimasNegociacoesInfo>();

            AcessaDados lAcessaDados = new AcessaDados();

            UltimasNegociacoesInfo lInfo;

            lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CLIENTE_DT_NEG"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE", DbType.String, pRequest.UltimasNegociacoes.CdCliente);

                lAcessaDados.AddInParameter(lDbCommand, "CD_CLIENTE_BMF", DbType.String, pRequest.UltimasNegociacoes.CdClienteBmf);

                lAcessaDados.AddInParameter(lDbCommand, "DT_INICIAL", DbType.DateTime, pRequest.UltimasNegociacoes.DataInicial);

                lAcessaDados.AddInParameter(lDbCommand, "DT_FINAL", DbType.DateTime, pRequest.UltimasNegociacoes.DataFinal);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                
                foreach (DataRow row in lDataTable.Rows)
                {
                    lInfo = new UltimasNegociacoesInfo();

                    lInfo.CdCliente = row["CD_CLIENTE"].DBToInt32();

                    lInfo.TipoBolsa = row["TIPO_BOLSA"].DBToString();

                    lInfo.DtUltimasNegociacoes = row["DT_NEGOCIO"].DBToDateTime();

                    lRetorno.ListaUltimasNegociacoes.Add(lInfo);
                }

            }

            return lRetorno;
        }

        public AtivoResponse ListarAtivo(AtivoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            AtivoResponse Retorno = new AtivoResponse();

            Retorno.ListaAtivo = new List<AtivoInfo>();

            AtivoInfo ativo;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoRendaFixa;


            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PR_SATIVO"))
            {
                if (!string.IsNullOrEmpty(pRequest.Ativo.CodigoAtivo))
                    _AcessaDados.AddInParameter(_DbCommand, "@p_ID_ATIVO", DbType.String, pRequest.Ativo.CodigoAtivo);

                if (pRequest.Ativo.CodigoProduto > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PRODUTO", DbType.String, pRequest.Ativo.CodigoProduto);


                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    ativo = new AtivoInfo()
                    {
                        CodigoAtivo     = linha["id_ativo"].DBToString(),
                        CodigoProduto   = linha["id_produto"].DBToInt32(),
                    };

                    Retorno.ListaAtivo.Add(ativo);

                }
            }




            return Retorno;
        }

        public InformeRendimentosTesouroResponse GetRendimentoTesouroDireto(InformeRendimentosTesouroRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastroOracle;

            InformeRendimentosTesouroResponse Retorno = new InformeRendimentosTesouroResponse();

            Retorno.ListaInformeRendimentosTesouro = new List<InformeRendimentosTesouroInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sinacor_rendimento_tesouro"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CPF"           , DbType.Int64  , pRequest.InformeRendimentosTesouro.CPF                );

                _AcessaDados.AddInParameter(_DbCommand, "NASCIMENTO"    , DbType.Date   , pRequest.InformeRendimentosTesouro.DataNascimento     );

                _AcessaDados.AddInParameter(_DbCommand, "DEPENDENTE"    , DbType.Int16  , pRequest.InformeRendimentosTesouro.CondicaoDependente );

                _AcessaDados.AddInParameter(_DbCommand, "ANO"           , DbType.Date   , pRequest.InformeRendimentosTesouro.AnoAtual           );

                _AcessaDados.AddInParameter(_DbCommand, "ANOANTERIOR"   , DbType.Date  , pRequest.InformeRendimentosTesouro.AnoAnterior         );

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                InformeRendimentosTesouroInfo _InformeRendimentosTesouro;

                foreach (DataRow item in _table.Rows)
                {
                    _InformeRendimentosTesouro = new InformeRendimentosTesouroInfo();

                    _InformeRendimentosTesouro.Posicao                  = item["Posicao"].DBToString()          ;

                    _InformeRendimentosTesouro.Quantidade               = item["qtd"].DBToInt32()               ;

                    _InformeRendimentosTesouro.QuantidadeAnoAnterior    = item["qtdAnoAnterior"].DBToDecimal()  ;

                    _InformeRendimentosTesouro.Valor                    = item["vlr"].DBToDecimal()             ;

                    _InformeRendimentosTesouro.ValorAnoAnterior         = item["vlrAnoAnterior"].DBToDecimal()  ;

                    Retorno.ListaInformeRendimentosTesouro.Add(_InformeRendimentosTesouro);
                }
            }
            return Retorno;
        }

        public InformeRendimentosResponse GetRendimento(InformeRendimentosRequest pRequest)
        {

            AcessaDados _AcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastroOracle;

            InformeRendimentosResponse Retorno = new InformeRendimentosResponse();

            Retorno.ListaInformeRendimentos = new List<InformeRendimentosInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sinacor_sel_rendimento"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CPF"           , DbType.Int64  , pRequest.InformeRendimentos.CPF               );

                _AcessaDados.AddInParameter(_DbCommand, "NASCIMENTO"    , DbType.Date   , pRequest.InformeRendimentos.DataNascimento    );

                _AcessaDados.AddInParameter(_DbCommand, "DEPENDENTE"    , DbType.Int16  , pRequest.InformeRendimentos.CondicaoDependente);

                _AcessaDados.AddInParameter(_DbCommand, "DATAINICIO"    , DbType.Date   , pRequest.InformeRendimentos.DataInicio        );

                _AcessaDados.AddInParameter(_DbCommand, "DATAFIM"       , DbType.Date   , pRequest.InformeRendimentos.DataFim           );

                _AcessaDados.AddInParameter(_DbCommand, "CODIGORETENCAO", DbType.Int16  , pRequest.InformeRendimentos.CondicaoRetencao) ;

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                InformeRendimentosInfo _ERendimento;

                foreach (DataRow item in _table.Rows)
                {
                    _ERendimento = new InformeRendimentosInfo();

                    _ERendimento.Data       = item["Data"].DBToString()         ;

                    _ERendimento.Imposto    = item["Imposto"].DBToDecimal()     ;

                    _ERendimento.Rendimento = item["Rendimento"].DBToDecimal()  ;

                    Retorno.ListaInformeRendimentos.Add(_ERendimento);
                }
            }

            return Retorno;
        }

        public SinacorEnderecoResponse GetEnderecoSinacorCustodia(SinacorEnderecoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastroOracle;

            SinacorEnderecoResponse Retorno = new SinacorEnderecoResponse();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sinacor_sel_ende_cus"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CPF"       , DbType.Int64  , pRequest.SinacorEndereco.CPF              );

                _AcessaDados.AddInParameter(_DbCommand, "NASCIMENTO", DbType.Date   , pRequest.SinacorEndereco.DataNascimento   );

                _AcessaDados.AddInParameter(_DbCommand, "DEPENDENTE", DbType.Int16  , pRequest.SinacorEndereco.CondicaoDependente);

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                Retorno.ListaSinacorEndereco = new List<SinacorEnderecoInfo>();

                Retorno.SinacorEndereco = new SinacorEnderecoInfo();

                if (_table.Rows.Count > 0)
                {
                    Retorno.SinacorEndereco.Bairro      = _table.Rows[0]["nm_bairro"].DBToString()      ;

                    Retorno.SinacorEndereco.Cep         = _table.Rows[0]["cep"].DBToString()            ;

                    Retorno.SinacorEndereco.Cidade      = _table.Rows[0]["nm_cidade"].DBToString()      ;

                    Retorno.SinacorEndereco.Complemento = _table.Rows[0]["nm_comp_ende"].DBToString()   ;

                    Retorno.SinacorEndereco.Rua         = _table.Rows[0]["nm_logradouro"].DBToString()  ;

                    Retorno.SinacorEndereco.UF          = _table.Rows[0]["sg_estado"].DBToString()      ;

                    Retorno.SinacorEndereco.Numero      = _table.Rows[0]["nr_predio"].DBToString()      ;

                    
                }
            }

            return Retorno;
        }

        public SinacorEnderecoResponse GetEnderecoSinacorCorrespondencia(SinacorEnderecoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastroOracle;

            SinacorEnderecoResponse Retorno = new SinacorEnderecoResponse();

            Retorno.SinacorEndereco = new SinacorEnderecoInfo();

            Retorno.ListaSinacorEndereco = new List<SinacorEnderecoInfo>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sinacor_sel_ende_corresp"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "CPF"       , DbType.Int64  , pRequest.SinacorEndereco.CPF                  );

                _AcessaDados.AddInParameter(_DbCommand, "NASCIMENTO", DbType.Date   , pRequest.SinacorEndereco.DataNascimento       );

                _AcessaDados.AddInParameter(_DbCommand, "DEPENDENTE", DbType.Int16  , pRequest.SinacorEndereco.CondicaoDependente   );

                DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    Retorno.SinacorEndereco.Bairro      = _table.Rows[0]["nm_bairro"].DBToString()      ;

                    Retorno.SinacorEndereco.Cep         = _table.Rows[0]["cep"].DBToString()            ;

                    Retorno.SinacorEndereco.Cidade      = _table.Rows[0]["nm_cidade"].DBToString()      ;

                    Retorno.SinacorEndereco.Complemento = _table.Rows[0]["nm_comp_ende"].DBToString()   ;

                    Retorno.SinacorEndereco.Rua         = _table.Rows[0]["nm_logradouro"].DBToString()  ;

                    Retorno.SinacorEndereco.UF          = _table.Rows[0]["sg_estado"].DBToString()      ;

                    Retorno.SinacorEndereco.Numero      = _table.Rows[0]["nr_predio"].DBToString()      ;
                }
            }

            return Retorno;
        }

        public TipoTecladoResponse GetTipoTeclado(TipoTecladoRequest pRequest)
        {
            TipoTecladoResponse lResposta = new TipoTecladoResponse();

            AcessaDados lAcessaDados = new AcessaDados();

            Conexao Conexao = new Generico.Dados.Conexao();

            lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoCadastro;

            using (DbCommand _DbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipoteclado_lst_sp"))
            {
                lAcessaDados.AddInParameter(_DbCommand, "@CodigoCliente", DbType.Int64, pRequest.CodigoCliente);

                lAcessaDados.AddInParameter(_DbCommand, "@CodigoLogin", DbType.String, pRequest.CodigoLogin);

                lAcessaDados.AddInParameter(_DbCommand, "@Email", DbType.String, pRequest.Email);

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(_DbCommand);

                // Por possuir registro na tabela, provavelmente uma das senhas já encontra-se no novo padrão (teclado dinâmico)
                if (lTable.Rows.Count > 0)
                {
                    // Tanto a senha quanto a assinatura não foram alteradas (QWERTY / Padrão)
                    if (!lTable.Rows[0]["SenhaAlterada"].DBToBoolean() && !lTable.Rows[0]["AssinaturaAlterada"].DBToBoolean())
                    {
                        lResposta.Teclado = TipoTeclado.QWERTY;
                        lResposta.Mensagem = "Atenção! Para sua segurança e conveniência estamos implementando um novo teclado virtual!\r\nSerá necessário alterar sua SENHA e sua ASSINATURA ELETRÔNICA!";
                    }
                    // Tanto a senha quanto a assinatura já foram alteradas (Dinâmico)
                    else if (lTable.Rows[0]["SenhaAlterada"].DBToBoolean() && lTable.Rows[0]["AssinaturaAlterada"].DBToBoolean())
                    {
                        lResposta.Teclado = TipoTeclado.DINAMICO;
                        lResposta.Mensagem = String.Empty;
                    }
                    // A senha já foi alterada porém a assinatura ainda não foi alterada (Dinâmico na Senha / QWERTY na Assinatura)
                    else if (lTable.Rows[0]["SenhaAlterada"].DBToBoolean() && !lTable.Rows[0]["AssinaturaAlterada"].DBToBoolean())
                    {
                        lResposta.Teclado = TipoTeclado.DINAMICO_SENHA;
                        lResposta.Mensagem = "Atenção! Para sua segurança e conveniência estamos implementando um novo teclado virtual!\r\nSerá necessário alterar sua ASSINATURA!";
                    }
                    // A senha ainda não foi alterada porém a assinatura já foi alterada (QWERTY na senha / Dinâmico na Assinatura)
                    else if (!lTable.Rows[0]["SenhaAlterada"].DBToBoolean() && lTable.Rows[0]["AssinaturaAlterada"].DBToBoolean())
                    {
                        lResposta.Teclado = TipoTeclado.DINAMICO_ASSINATURA;
                        lResposta.Mensagem = "Atenção! Para sua segurança e conveniência estamos implementando um novo teclado virtual!\r\n Será necessário alterar sua SENHA!";
                    }
                }
                // Por não possuir registro na tabela, provavelmente nenhuma das senhas encontra-se no novo padrão (teclado dinâmico)
                else
                {
                    lResposta.Teclado = TipoTeclado.QWERTY;
                }
            }

            return lResposta;
        }

        #region Metodos Internos

        /// <summary>
        /// Obtem o saldo de conta margem do cliente
        /// </summary>
        /// <param name="IdCliente">Código do cliente</param>
        /// <returns>Saldo em conta margem do cliente</returns>
        private Nullable<decimal> ObtemSaldoContaMargem(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao conexao = new Generico.Dados.Conexao();
            //SaldoContaCorrenteResponse<ContaCorrenteInfo> _SaldoContaCorrente = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoContaMargem;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_contamargem"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["VL_LIMITE"].DBToDecimal());
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
        /// Obtem o saldo bloqueado em CC do cliente.
        /// </summary>
        /// <param name="IdCliente">Código do cliente</param>
        /// <returns>Saldo Bloqueado em CC</returns>
        private Nullable<decimal> ObtemSaldoBloqueado(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Conexao conexao = new Generico.Dados.Conexao();

            try
            {
                lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoRisco;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_saldo_bloqueado_cliente"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@Account", DbType.AnsiString, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["SaldoBloqueado"].DBToDecimal());
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
        /// Busca o limite operacional disponível para o cliente.
        /// </summary>
        private void ConsultarLimiteOperacionalDisponivel(Gradual.OMS.ContaCorrente.Lib.ContaCorrenteInfo pParametro)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoRisco;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_limites_cliente_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.IdClienteSinacor);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    DataRow lRow = null;

                    int lIdParametro;

                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRow = lDataTable.Rows[i];

                        if (lRow["id_parametro"] != DBNull.Value)
                        {
                            lIdParametro = lRow["id_parametro"].DBToInt32();

                            switch (lIdParametro)
                            {
                                case 12:

                                    pParametro.LimiteOperacioalDisponivelAVista = lDataTable.Rows[i]["valor"].DBToDecimal();

                                    break;

                                case 13:

                                    pParametro.LimiteOperacioalDisponivelOpcao = lDataTable.Rows[i]["valor"].DBToDecimal();

                                    break;
                            }
                        }
                    }
                }
            }
        }

        private string ObterUltimaCotacao(string Instrumento)
        {
            string UltimaCotacao = string.Empty;
            try
            {
                IServicoCotacao _ServicoCotacao = Ativador.Get<IServicoCotacao>();
                string Cotacao = _ServicoCotacao.ReceberTickerCotacao(Instrumento);
                UltimaCotacao = decimal.Parse(Cotacao.Substring(74, 13)).ToString();
            }
            catch (Exception ex)
            {
                string StackTrace = ex.StackTrace;
                string Message = ex.Message;
                Exception InnerException = ex.InnerException;

                throw ex;
            }
            return UltimaCotacao;
        }

        public List<RiscoLimiteAlocadoInfo> ConsultarRiscoLimiteAlocadoPorClienteNovoOMS(RiscoLimiteAlocadoInfo pParametros)
        {
            var lRetorno = new List<RiscoLimiteAlocadoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoRisco;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_limite_alocado_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.ConsultaIdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Add(this.CarregarEntidadeRiscoLimiteAlocadoInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        private RiscoLimiteAlocadoInfo CarregarEntidadeRiscoLimiteAlocadoInfo(DataRow pLinha)
        {
            return new RiscoLimiteAlocadoInfo()
            {
                DsParametro  = pLinha["ds_parametro"].DBToString(),
                IdParametro  = pLinha["id_parametro"].DBToInt32(),
                VlAlocado    = pLinha["vl_alocado"].DBToDecimal(),
                VlDisponivel = pLinha["vl_disponivel"].DBToDecimal(),
                VlParametro  = pLinha["vl_parametro"].DBToDecimal(),
            };
        }

        public ClienteFundoResponse ConsultarClientesFundosItau(ClienteFundoRequest pParametros)
        {
            var lRetorno = new ClienteFundoResponse();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "Cadastro";
            string cpfcnpj                    = "";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cpfcnpj_sel_sp"))
            {
                lAcessaDados.AddInParameter(cmd, "@cd_codigo", DbType.Int32, pParametros.IdCliente);

                var table = lAcessaDados.ExecuteDbDataTable(cmd);

                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];

                    cpfcnpj = dr["ds_cpfcnpj"].DBToString().PadLeft(15, '0');
                }
            }

            if (string.IsNullOrEmpty(cpfcnpj))
            {
                return lRetorno;
            }

            lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "FundosItau";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
            {
                lAcessaDados.AddInParameter(cmd, "@dsCpfCnpj", DbType.String, cpfcnpj);

                var table = lAcessaDados.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {

                    ClienteFundosInfo fundo = new ClienteFundosInfo();

                    fundo.IdCliente       = pParametros.IdCliente;
                    fundo.Cota            = dr["valorCota"].DBToDecimal();
                    fundo.DataAtualizacao = dr["dtReferencia"].DBToDateTime();
                    fundo.IOF             = dr["valorIOF"].DBToDecimal();
                    fundo.IR              = dr["valorIR"].DBToDecimal();
                    fundo.NomeFundo       = dr["dsRazaoSocial"].DBToString().Trim();
                    fundo.Quantidade      = dr["quantidadeCotas"].DBToDecimal();
                    fundo.ValorBruto      = dr["valorBruto"].DBToDecimal();
                    fundo.ValorLiquido    = dr["valorLiquido"].DBToDecimal();

                    lRetorno.ListaFundos.Add(fundo);
                }
            }


            return lRetorno;
        }

        public ClienteClubeResponse  ConsultarClienteClubes(ClienteClubeRequest pParametros)
        {
            var lRetorno = new ClienteClubeResponse();

            var lAcessaDados = new AcessaDados();

            DateTime UltimoDiaUtil = SelecionaUltimoDiaUtil(); //seleciona o ultimo dia ultil. 

            lAcessaDados.ConnectionStringName = "Clubes";

            List<string> lNomeClubeExiste = new List<string>();

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MC_CLUBES_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.IdCliente);

                lAcessaDados.AddInParameter(lDbCommand, "@DT_POSICAO", DbType.DateTime, UltimoDiaUtil);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lNomeClubeExiste.Clear();

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.ListaClube.Add(CriarRegistroClienteClubesInfo(lDataTable.Rows[i]));

            }

            return lRetorno;
        }

        private static ClienteClubesInfo CriarRegistroClienteClubesInfo(DataRow linha)
        {
            return new ClienteClubesInfo()
            {
                IdCliente       = linha["cd_cliente"].DBToInt32(),
                Cota            = linha["vl_cota"].DBToDecimal(),
                DataAtualizacao = linha["dt_atualizacao"].DBToDateTime(),
                IOF             = linha["vl_iof"].DBToDecimal(),
                IR              = linha["vl_ir"].DBToDecimal(),
                NomeClube       = linha["ds_nome_clube"].DBToString(),
                Quantidade      = linha["vl_quantidade"].DBToDecimal(),
                ValorBruto      = linha["vl_bruto"].DBToDecimal(),
                ValorLiquido    = linha["vl_liquido"].DBToDecimal(),
            };
        }

        public static DateTime SelecionaUltimoDiaUtil()
        {
            AcessaDados _AcessaDados = new AcessaDados();

            DateTime dataRetorno;
            
            _AcessaDados.ConnectionStringName = "Trade";
            
            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RETORNA_ULTIMO_DIA_UTIL"))
            {
                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                dataRetorno = Convert.ToDateTime(tabela.Rows[0]["dataUtil"]);

            }

            return dataRetorno;
        }

        public DateTime DataPregaoAnterior(Int32 pDias)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            DateTime dataRetorno;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RETORNA_ULTIMO_DIA_UTIL"))
            {
                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                dataRetorno = Convert.ToDateTime(tabela.Rows[0]["dataUtil"]);

            }
            return dataRetorno;
        }

        #endregion
    }
}
