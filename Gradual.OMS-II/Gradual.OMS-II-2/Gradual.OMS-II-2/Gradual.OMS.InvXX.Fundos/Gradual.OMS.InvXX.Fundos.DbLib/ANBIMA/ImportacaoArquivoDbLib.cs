using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.OMS.InvXX.Fundos.Lib.ANBIMA;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA
{
    public class ImportacaoArquivoDbLib
    {
        #region Propriedades
        private const string ConnectionStringName = "SIANBIMA43";
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["SIANBIMA43"].ConnectionString;
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Métodos Importação Site
        public void ImportarFundos(ANBIMAFundosInfo pInfo)
        {
            if (pInfo.CodigoFundo == string.Empty)
                return;

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_FUNDOS_INS"))
                {
                    lAcessaDados.AddInParameter(lComm, "@CodigoFundo",          DbType.String,      pInfo.CodigoFundo);
                    lAcessaDados.AddInParameter(lComm, "@CodigoInstituicao",    DbType.String,      pInfo.CodigoInstituicao);
                    lAcessaDados.AddInParameter(lComm, "@NomeFanatasia",        DbType.String,      pInfo.NomeFanatasia);
                    lAcessaDados.AddInParameter(lComm, "@Gestor",               DbType.String,      pInfo.Gestor);
                    lAcessaDados.AddInParameter(lComm, "@CodigoTipo",           DbType.Int32,       pInfo.CodigoTipo);
                    lAcessaDados.AddInParameter(lComm, "@DataInicial",          DbType.DateTime,    pInfo.DataInicial);
                    lAcessaDados.AddInParameter(lComm, "@DataFim",              DbType.DateTime,    pInfo.DataFim);
                    lAcessaDados.AddInParameter(lComm, "@DataInfo",             DbType.DateTime,    pInfo.DataInfo);
                    lAcessaDados.AddInParameter(lComm, "@PerfilCota",           DbType.String,      pInfo.PerfilCota);
                    lAcessaDados.AddInParameter(lComm, "@DataDiv",              DbType.DateTime,    pInfo.DataDiv);
                    lAcessaDados.AddInParameter(lComm, "@RazaoSocial",          DbType.String,      pInfo.RazaoSocial);
                    lAcessaDados.AddInParameter(lComm, "@CNPJ",                 DbType.String,      pInfo.CNPJ);
                    lAcessaDados.AddInParameter(lComm, "@Aberto",               DbType.String,      pInfo.Aberto);
                    lAcessaDados.AddInParameter(lComm, "@Exclusivo",            DbType.String,      pInfo.Exclusivo);
                    lAcessaDados.AddInParameter(lComm, "@PrazoEmissaoCotas",    DbType.String,      pInfo.PrazoEmissaoCotas);
                    lAcessaDados.AddInParameter(lComm, "@PrazoConvResg",        DbType.String,      pInfo.PrazoConvResg);
                    lAcessaDados.AddInParameter(lComm, "@PrazoPagtoResg",       DbType.String,      pInfo.PrazoPagtoResg);
                    lAcessaDados.AddInParameter(lComm, "@CarenciaUniversal",    DbType.Int32,       pInfo.CarenciaUniversal);
                    lAcessaDados.AddInParameter(lComm, "@CarenciaCiclica",      DbType.Int32,       pInfo.CarenciaCiclica);
                    lAcessaDados.AddInParameter(lComm, "@CotaAbertura",         DbType.String,      pInfo.CotaAbertura);
                    lAcessaDados.AddInParameter(lComm, "@PeriodoDivulgacao",    DbType.Int32,       pInfo.PeriodoDivulgacao);
                    lAcessaDados.AddInParameter(lComm, "@DataHora",             DbType.DateTime,    pInfo.DataHora);

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                }
                
            }
        }

        public void ImportarMovimentoCota(ANBIMAMovimentoCotaSiteInfo pInfo)
        {
            
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_MOVIMENTOCOTA_INS"))
                {
                    lAcessaDados.AddInParameter(lComm, "@Data",                             DbType.DateTime,    pInfo.Data);
                    lAcessaDados.AddInParameter(lComm, "@ValorMinAplicacaoInicial",         DbType.Decimal,     pInfo.ValorMinAplicacaoInicial);
                    lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacaoAdicional",      DbType.Decimal,     pInfo.ValorMiniAplicacaoAdicional);
                    lAcessaDados.AddInParameter(lComm, "@ValorMiniResgate",                 DbType.Decimal,     pInfo.ValorMiniResgate);
                    lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacao",               DbType.Decimal,     pInfo.ValorMiniAplicacao);
                    lAcessaDados.AddInParameter(lComm, "@Identificador",                    DbType.String,      pInfo.Identificador);
                    lAcessaDados.AddInParameter(lComm, "@DiasConversaoAplicacao",           DbType.String,      pInfo.DiasConversaoAplicacao);
                    lAcessaDados.AddInParameter(lComm, "@DiasConversaoResgate",             DbType.String,      pInfo.DiasConversaoResgate);
                    lAcessaDados.AddInParameter(lComm, "@DiasConversaoResgateAntecipado",   DbType.String,      pInfo.DiasConversaoResgateAntecipado);
                    lAcessaDados.AddInParameter(lComm, "@DiasPagamentoResgate",             DbType.String,      pInfo.DiasPagamentoResgate);
                    lAcessaDados.AddInParameter(lComm, "@ValorTaxaAdministracao",           DbType.Decimal,     pInfo.ValorTaxaAdministracao);
                    lAcessaDados.AddInParameter(lComm, "@ValorTaxaAdministracaoMaxima",     DbType.String,      pInfo.ValorTaxaAdministracaoMaxima);
                    lAcessaDados.AddInParameter(lComm, "@ValorTaxaResgateAntecipado",       DbType.String,      pInfo.ValorTaxaResgateAntecipado);
                    lAcessaDados.AddInParameter(lComm, "@ValorPatrimonioLiquido",           DbType.Decimal,     pInfo.ValorPatrimonioLiquido);
                    lAcessaDados.AddInParameter(lComm, "@CobraTaxaPerformance",             DbType.String,      pInfo.CobraTaxaPerformance);
                    lAcessaDados.AddInParameter(lComm, "@ValorTaxaPerformance",             DbType.String,      pInfo.ValorTaxaPerformance);
                    lAcessaDados.AddInParameter(lComm, "@DataHora",                         DbType.DateTime,    pInfo.DataHora);

                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                }
                
            }
        }

        public void ImportarRentabilidadeDia(FundosDiaInfo pInfo)
        {
            try
            {
                if (pInfo.CodigoFundo == string.Empty)
                    return;

                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_RENTDIA_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo", DbType.String, pInfo.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Data", DbType.DateTime, pInfo.Data);
                        lAcessaDados.AddInParameter(lComm, "@PL", DbType.Decimal, pInfo.Pl);
                        lAcessaDados.AddInParameter(lComm, "@ValorCota", DbType.Decimal, pInfo.ValorCota);
                        lAcessaDados.AddInParameter(lComm, "@RentDia", DbType.Decimal, pInfo.RentabilidadeDia);
                        lAcessaDados.AddInParameter(lComm, "@RentMes", DbType.Decimal, pInfo.RentabilidadeMes);
                        lAcessaDados.AddInParameter(lComm, "@RentAno", DbType.Decimal, pInfo.RentabilidadeAno);
                        lAcessaDados.AddInParameter(lComm, "@DataHora", DbType.DateTime, DateTime.Now);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ImportarRentabilidadeMes(FundosMesInfo pInfo)
        {
            try
            {
                if (pInfo.CodigoFundo == string.Empty)
                    return;

                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_RENTMES_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo",  DbType.String,  pInfo.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Mes",          DbType.Int32,   pInfo.DataMes);
                        lAcessaDados.AddInParameter(lComm, "@Ano",          DbType.Int32,   pInfo.DataAno);
                        lAcessaDados.AddInParameter(lComm, "@PL",           DbType.Decimal, pInfo.ValorPL);
                        lAcessaDados.AddInParameter(lComm, "@ValorCota",    DbType.Decimal, pInfo.ValorCota);
                        lAcessaDados.AddInParameter(lComm, "@RentMes",      DbType.Decimal, pInfo.RentabilidadeMes);
                        lAcessaDados.AddInParameter(lComm, "@RentAno",      DbType.Decimal, pInfo.RentabilidadeAno);
                        lAcessaDados.AddInParameter(lComm, "@CodigoTipo",   DbType.Int32,   pInfo.CodigoTipo);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",     DbType.DateTime, pInfo.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetCodigoProduto(string CodigoAnbima)
        {
            int lRetorno = 0;

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.Text, "select idProduto from tbProduto where idCodigoAnbima = " + CodigoAnbima))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);

                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            lRetorno = dr1["idProduto"].DBToInt32();
                        }
                    }
                }
            }

            return lRetorno;
        }

        


        #endregion

    }
}
