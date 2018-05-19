using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Fundos;
using System.Collections.Generic;
using Gradual.Generico.Dados;
using System.Data.Common;
using System;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteRiscoRendaDbLib
    {
        #region | Métodos CRUD

        public  ConsultarObjetosResponse<ClienteFundosInfo> ConsultarClienteFundos(ConsultarEntidadeRequest<ClienteFundosInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ClienteFundosInfo>();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = "OMS";

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_tb_fundos_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(CriarRegistroClienteFundosInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public  ConsultarObjetosResponse<ClienteFundosInfo> ConsultarClientesFundosItau(ConsultarEntidadeRequest<ClienteFundosInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ClienteFundosInfo>();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = "Cadastro";
            string cpfcnpj = "";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cpfcnpj_sel_sp"))
            {
                lAcessaDados.AddInParameter(cmd, "@cd_codigo", DbType.Int32, pParametros.Objeto.IdCliente);

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

            if (lAcessaDados != null)
            {
                lAcessaDados = null;
            }

            var lAcessaDados2 = new ConexaoDbHelper();

            lAcessaDados2.ConnectionStringName = "FundosItau";

            using (var cmd = lAcessaDados2.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
            {
                lAcessaDados2.AddInParameter(cmd, "@dsCpfCnpj", DbType.String, cpfcnpj);

                var table = lAcessaDados2.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {

                    ClienteFundosInfo fundo = new ClienteFundosInfo();

                    fundo.IdCliente       = pParametros.Objeto.IdCliente;
                    fundo.Cota            = dr["valorCota"].DBToDecimal();
                    fundo.DataAtualizacao = dr["dtReferencia"].DBToDateTime();
                    fundo.IOF             = dr["valorIOF"].DBToDecimal();
                    fundo.IR              = dr["valorIR"].DBToDecimal();
                    fundo.NomeFundo       = dr["dsRazaoSocial"].DBToString().Trim();
                    fundo.Quantidade      = dr["quantidadeCotas"].DBToDecimal();
                    fundo.ValorBruto      = dr["valorBruto"].DBToDecimal();
                    fundo.ValorLiquido    = dr["valorLiquido"].DBToDecimal();

                    lRetorno.Resultado.Add(fundo);
                }
            }


            return lRetorno;
        }

        public  ConsultarObjetosResponse<RendaFixaInfo> ConsultarRendaFixa(ConsultarEntidadeRequest<RendaFixaInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RendaFixaInfo>();

            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = "MINICOM";

            using (var cmd = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_posicao_rendafixa"))
            {
                lAcessaDados.AddInParameter(cmd, "@cd_cliente", DbType.Int32, pParametros.Objeto.CodigoCliente );

                var table = lAcessaDados.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                {
                    var RendaFixa = new RendaFixaInfo();

                    RendaFixa.CodigoCliente    = pParametros.Objeto.CodigoCliente;
                    RendaFixa.Titulo           = dr["Titulo"].DBToString();
                    RendaFixa.Aplicacao        = dr["Aplicacao"].DBToDateTime();
                    RendaFixa.Vencimento       = dr["Vencimento"].DBToDateTime();
                    RendaFixa.Taxa             = dr["Taxa"].DBToDecimal();
                    RendaFixa.Quantidade       = dr["Quantidade"].DBToDecimal();
                    RendaFixa.ValorOriginal    = dr["ValorOriginal"].DBToDecimal();
                    RendaFixa.SaldoBruto       = dr["SaldoBruto"].DBToDecimal();
                    RendaFixa.IRRF             = dr["IRRF"].DBToDecimal();
                    RendaFixa.IOF              = dr["IOF"].DBToDecimal();
                    RendaFixa.SaldoLiquido     = dr["SaldoLiquido"].DBToDecimal();

                    lRetorno.Resultado.Add(RendaFixa);
                }
            }


            return lRetorno;
        }

        private ClienteFundosInfo CriarRegistroClienteFundosInfo(DataRow linha)
        {
            return new ClienteFundosInfo()
            {
                IdCliente       = linha["id_cliente"].DBToInt32(),
                Cota            = linha["vl_cota"].DBToDecimal(),
                DataAtualizacao = linha["dt_atualizacao"].DBToDateTime(),
                IOF             = linha["vl_iof"].DBToDecimal(),
                IR              = linha["vl_ir"].DBToDecimal(),
                NomeFundo       = linha["ds_nome_fundo"].DBToString(),
                Quantidade      = linha["vl_quantidade"].DBToDecimal(),
                ValorBruto      = linha["vl_bruto"].DBToDecimal(),
                ValorLiquido    = linha["vl_liquido"].DBToDecimal(),
            };
        }

    }
    public static partial class ClienteDbLib
    {
        public static IntegracaoFundosInfo GetNomeRiscoFundo(int CodigoFundoItau)
        {
            var lRetorno = new IntegracaoFundosInfo();

            var lAcessaDados = new AcessaDados();
           
            //lAcessaDados.Conexao = new Conexao();
            lAcessaDados.ConnectionStringName = "PlataformaInviXX";

            string lSql = "Select b.dsNomeProduto, a.dsRisco, b.idProduto, b.idCodigoAnbima from tbProdutoAnbima a, tbProduto b where b.CodigoFundoItau=" + CodigoFundoItau +
                " and a.CodigoAnbima = b.idCodigoAnbima";

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                if (dt != null && dt.Rows.Count > 0)
                {
                    lRetorno.Risco          = dt.Rows[0]["dsRisco"].DBToString();
                    lRetorno.NomeProduto    = dt.Rows[0]["dsNomeProduto"].DBToString();
                    lRetorno.IdProduto      = dt.Rows[0]["idProduto"].DBToInt32();
                    lRetorno.IdCodigoAnbima = dt.Rows[0]["idCodigoAnbima"].DBToString();
                }
            }

            return lRetorno;
        }

        public static IntegracaoFundosInfo GetFundoFinancialRepate(string CodigoAnbima)
        {
            var lRetorno = new IntegracaoFundosInfo();

            var lAcessaDados = new AcessaDados();

            //lAcessaDados.Conexao = new Conexao();
            lAcessaDados.ConnectionStringName = "Financial";

            string lSql = "Select idCarteira, faixa, PercentualRebateAdministracao, PercentualRebatePerformance from TabelaRebateCarteira where IdCarteira = '" + CodigoAnbima+ "'";

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                lRetorno.DadosMovimentacao = new IntegracaoFundosMovimentoInfo();

                if (dt != null && dt.Rows.Count > 0)
                {
                    //lRetorno.Risco          = dt.Rows[0]["dsRisco"].DBToString();
                    //lRetorno.NomeProduto    = dt.Rows[0]["dsNomeProduto"].DBToString();
                    //lRetorno.IdProduto      = dt.Rows[0]["idProduto"].DBToInt32();
                    lRetorno.IdCodigoAnbima                  = dt.Rows[0]["IdCarteira"].DBToString();
                    lRetorno.DadosMovimentacao.VlTaxaRepasse = dt.Rows[0]["PercentualRebateAdministracao"].DBToDecimal();
                }
            }

            return lRetorno;
        }

        public static IntegracaoFundosInfo GetTaxaAdminFundo(string CodigoAnbima)
        {
            var lRetorno = new IntegracaoFundosInfo();

            var lAcessaDados = new AcessaDados();

            //lAcessaDados.Conexao = new Conexao();
            lAcessaDados.ConnectionStringName = "PlataformaInviXX";

            string lSql = string.Empty;

            lSql = "Select TaxaFixa from tbANBIMATaxaAdm where CodFundo='" +CodigoAnbima+ "'";

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                lRetorno.DadosMovimentacao = new IntegracaoFundosMovimentoInfo();

                if (dt != null && dt.Rows.Count > 0)
                {
                    /*
                    lRetorno.Risco           = dt.Rows[0]["dsRisco"].DBToString();
                    lRetorno.NomeProduto     = dt.Rows[0]["dsProduto"].DBToString();
                    lRetorno.IdProduto       = dt.Rows[0]["idProduto"].DBToInt32();
                    lRetorno.IdCodigoAnbima  = dt.Rows[0]["CodigoAnbima"].DBToString();
                    lRetorno.CodigoFundoItau = dt.Rows[0]["CodigoFundoItau"].ToString();
                     * */

                    lRetorno.DadosMovimentacao.VlTaxaAdmin = dt.Rows[0]["TaxaFixa"].DBToDecimal();
                }
            }

            return lRetorno;

        }

        public static IntegracaoFundosInfo GetNomeRiscoFundo(string CodigoAnbima, int IdFundo)
        {
            var lRetorno = new IntegracaoFundosInfo();

            var lAcessaDados = new AcessaDados();
            
            //lAcessaDados.Conexao = new Conexao();
            lAcessaDados.ConnectionStringName = "PlataformaInviXX";

            string lSql = string.Empty;

            if (CodigoAnbima == string.Empty)
            {
                lSql = "Select a.dsProduto, a.dsRisco, b.idProduto, a.CodigoAnbima, b.CodigoFundoItau " +
                "from tbProdutoAnbima a , tbProduto b where b.idProduto='" + IdFundo + "' and a.CodigoAnbima = b.IdCodigoAnbima";
            }
            else
            {
                lSql = "Select a.dsProduto, a.dsRisco, b.idProduto, a.CodigoAnbima, b.CodigoFundoItau " +
                "from tbProdutoAnbima a , tbProduto b where a.codigoAnbima='" + CodigoAnbima + "' and a.CodigoAnbima = b.IdCodigoAnbima";
            }
            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                if (dt != null && dt.Rows.Count > 0)
                {
                    lRetorno.Risco           = dt.Rows[0]["dsRisco"].DBToString();
                    lRetorno.NomeProduto     = dt.Rows[0]["dsProduto"].DBToString();
                    lRetorno.IdProduto       = dt.Rows[0]["idProduto"].DBToInt32();
                    lRetorno.IdCodigoAnbima  = dt.Rows[0]["CodigoAnbima"].DBToString();
                    lRetorno.CodigoFundoItau = dt.Rows[0]["CodigoFundoItau"].ToString();
                }
            }
            

            return lRetorno;
        }

        public static FundoResponse SelecionarFundoItau(FundoRequest pRequest)
        {
            var lAcessaDados = new ConexaoDbHelper();

            FundoResponse lRetorno = new FundoResponse();

            lRetorno.ListaFundo = new List<ClienteFundosInfo>();

            ClienteFundosInfo lFundo;

            lAcessaDados.ConnectionStringName = "FundosItau";

            using (var _DbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
            {
                lAcessaDados.AddInParameter(_DbCommand, "@dsCpfCnpj", DbType.String, pRequest.CpfDoCliente.PadLeft(15, '0'));

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow lLinha in lTable.Rows)
                {
                    lFundo = new ClienteFundosInfo()
                    {
                        Cota            = lLinha["valorCota"].DBToDecimal(),
                        DataAtualizacao = lLinha["dtReferencia"].DBToDateTime(),
                        IOF             = lLinha["valorIOF"].DBToDecimal(),
                        IR              = lLinha["valorIR"].DBToDecimal(),
                        NomeFundo       = lLinha["dsRazaoSocial"].ToString(),
                        Quantidade      = lLinha["quantidadeCotas"].DBToDecimal(),
                        ValorBruto      = lLinha["valorBruto"].DBToDecimal(),
                        ValorLiquido    = lLinha["valorLiquido"].DBToDecimal(),
                        CodigoFundoItau = lLinha["dsCodFundo"].DBToString()
                    };

                    lRetorno.ListaFundo.Add(lFundo);
                }

            }

            return lRetorno;
        }

        public static Nullable<int> VerificaExistenciaFundoItau(string CodigoAnbima)
        {
            Nullable<int> lCodigoFundoItau = null;
            
            lCodigoFundoItau = null;

            var lAcessaDados = new AcessaDados();
                
                //lAcessaDados.Conexao = new Conexao();

            lAcessaDados.ConnectionStringName = "PlataformaInviXX";

            string lSql = "select * from tbProduto where idCodigoAnbima='" + CodigoAnbima + "'";

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
            {
                DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                if (dt != null && dt.Rows.Count > 0)
                {
                    lCodigoFundoItau = dt.Rows[0]["CodigoFundoItau"].DBToInt32();
                }
            }

            return lCodigoFundoItau;
        }

        public static DataTable ConsultaFundosBritech(Int32 CodigoCliente)
        {
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = "Cadastro";

            using (var _DbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "GET_POSICAOCOTISTA"))
            {
                lAcessaDados.AddInParameter(_DbCommand, "@CodigoCliente", DbType.String, CodigoCliente.ToString());

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(_DbCommand);

                return lTable;
            }

        }

        #endregion

        #region | Métodos de apoio

        private static ClienteFundosInfo CriarRegistroClienteFundosInfo(DataRow linha)
        {
            return new ClienteFundosInfo()
            {
                IdCliente       = linha["id_cliente"].DBToInt32(),
                Cota            = linha["vl_cota"].DBToDecimal(),
                DataAtualizacao = linha["dt_atualizacao"].DBToDateTime(),
                IOF             = linha["vl_iof"].DBToDecimal(),
                IR              = linha["vl_ir"].DBToDecimal(),
                NomeFundo       = linha["ds_nome_fundo"].DBToString(),
                Quantidade      = linha["vl_quantidade"].DBToDecimal(),
                ValorBruto      = linha["vl_bruto"].DBToDecimal(),
                ValorLiquido    = linha["vl_liquido"].DBToDecimal(),
            };
        }

        #endregion
    }
}
