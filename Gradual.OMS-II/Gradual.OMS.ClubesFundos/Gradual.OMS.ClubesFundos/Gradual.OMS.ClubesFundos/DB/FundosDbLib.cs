#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.ClubesFundos.Lib.Dados;
using Gradual.OMS.ClubesFundos.Lib.Util;
using Gradual.OMS.ClubesFundos.Lib;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using log4net;
#endregion;

namespace Gradual.OMS.ClubesFundos
{
    public class FundosDbLib
    {
        #region Propriedades
        private const string _ConnectionStringName = "OMS";
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region ConsultarClientesFundos

        /// <summary>
        /// Não utilizar esse método.
        /// Por: André C. Miguel
        /// Data: 19/04/2013
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        [Obsolete("Esse metodo está obsoleto e não deve ser utilizado")]
        public List<FundosInfo> ConsultarClientesFundos(ListarFundosRequest pRequest)
        {
            List<FundosInfo> lRetorno = new List<FundosInfo>();

            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = _ConnectionStringName;

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_tb_fundos_sel"))
            {
                acesso.AddInParameter(cmd, "@id_cliente", DbType.Int32, pRequest.IdCliente);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Add(CriarRegistroClienteFundosInfo(dr));
            }

            return lRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public List<FundosInfo> ConsultarClientesFundosItau(ListarFundosRequest pRequest)
        {
            List<FundosInfo> lRetorno = new List<FundosInfo>();

            AcessaDados acesso = new AcessaDados();

            acesso.ConnectionStringName = "Cadastro";
            string cpfcnpj ="";

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "cpfcnpj_sel_sp"))
            {
                acesso.AddInParameter(cmd, "@cd_codigo", DbType.Int32, pRequest.IdCliente);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];

                    cpfcnpj = dr["ds_cpfcnpj"].DBToString().PadLeft(15, '0');
                }
            }

            logger.Debug("Obteve CPF/CNPJ [" + cpfcnpj + "]");

            acesso = new AcessaDados();

            acesso.ConnectionStringName = "FundosItau";

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
            {
                acesso.AddInParameter(cmd, "@dsCpfCnpj", DbType.String, cpfcnpj);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach( DataRow dr in table.Rows )
                {

                    FundosInfo fundo = new FundosInfo();

                    fundo.IdCliente = pRequest.IdCliente;
                    fundo.Cota = dr["valorCota"].DBToDecimal();
                    fundo.DataAtualizacao = dr["dtReferencia"].DBToDateTime();
                    fundo.IOF = dr["valorIOF"].DBToDecimal();
                    fundo.IR = dr["valorIR"].DBToDecimal();
                    fundo.NomeFundo = dr["dsRazaoSocial"].DBToString();
                    fundo.Quantidade = dr["quantidadeCotas"].DBToDecimal();
                    fundo.ValorBruto = dr["valorBruto"].DBToDecimal();
                    fundo.ValorLiquido = dr["valorLiquido"].DBToDecimal();
                    fundo.Cnpj = dr["dsCnpj"].DBToString();

                    //info.Agencia = lRow["Agencia"].ToString();
                    //info.BancoCli = lRow["Banco"].ToString();
                    //info.Conta = lRow["Conta"].ToString();
                    //info.CPFCNPJ = lRow["dsCpfCnpj"].ToString();
                    //info.DataProcessamento = lRow["dtProcessamento"].DBToDateTime();
                    //info.DataReferencia = lRow["dtReferencia"].DBToDateTime();
                    //info.DigitoConta = lRow["DigitoConta"].ToString();
                    //info.IDCotista = lRow["idCotista"].ToString();
                    //info.IDFundo = lRow["idFundo"].DBToInt32();
                    //info.IDMovimento = lRow["idMovimento"].DBToInt32();
                    //info.IDProcessamento = lRow["idProcessamento"].DBToInt32();
                    //info.QtdeCotas = lRow["quantidadeCotas"].DBToDecimal();
                    //info.ValorBruto = lRow["valorBruto"].DBToDecimal();
                    //info.ValorCota = lRow["valorCota"].DBToDecimal();
                    //info.ValorIR = lRow["valorIR"].DBToDecimal();
                    //info.ValorLiquido = lRow["valorLiquido"].DBToDecimal();

                    lRetorno.Add(fundo);
                }
            }


            return lRetorno;
        }

        #endregion

        #region Métodos de Aopio
        private FundosInfo CriarRegistroClienteFundosInfo(DataRow linha)
        {
            return new FundosInfo()
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
