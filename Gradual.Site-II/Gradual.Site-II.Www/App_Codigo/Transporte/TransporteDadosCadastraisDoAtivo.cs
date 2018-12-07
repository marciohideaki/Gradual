using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;

namespace Gradual.Site.Www
{
    public class TransporteDadosCadastraisDoAtivo
    {
        #region Propriedades

        public string FatorDeCotacao { get; set; }

        public string NomeDaEmpresa { get; set; }

        public string VencimentoDaOpcao { get; set; }

        public string LoteMinimo { get; set; }

        #endregion

        #region Métodos Private

        private void CarregarDadosDoAtivo(string pAtivo)
        {
            AcessaDados lAcessaDados = null;
            DbCommand lCommand = null;
            DataTable lTable = null;

            try
            {
                lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "OMS";

                lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_dados_ativo_sel");

                lAcessaDados.AddInParameter(lCommand, "@cd_negociacao", DbType.String, pAtivo);

                lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                if ((lTable != null) && (lTable.Rows.Count > 0))
                {
                    this.FatorDeCotacao = lTable.Rows[0]["fator_cot"].ToString();
                    this.LoteMinimo     = lTable.Rows[0]["Lote_Minimo"].ToString();
                    this.NomeDaEmpresa  = lTable.Rows[0]["NomeEmpresa"].ToString();

                    if (lTable.Rows[0]["dt_vencimento"] != DBNull.Value && !string.IsNullOrEmpty(lTable.Rows[0]["dt_vencimento"].ToString()))
                        this.VencimentoDaOpcao = DateTime.Parse(lTable.Rows[0]["dt_vencimento"].ToString()).ToString("dd/MM/yyyy");
                }
            }
            catch
            {
            }
            finally
            {
                lAcessaDados = null;
                lCommand.Dispose();
                lCommand = null;
            }
        }

        #endregion

        #region Construtores
        
        public TransporteDadosCadastraisDoAtivo() { }
        
        public TransporteDadosCadastraisDoAtivo(string pAtivo) : this()
        {
            this.CarregarDadosDoAtivo(pAtivo);
        }

        #endregion
    }
}