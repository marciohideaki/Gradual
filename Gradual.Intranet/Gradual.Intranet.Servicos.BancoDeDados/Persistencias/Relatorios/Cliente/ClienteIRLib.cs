using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Contratos.Dados.Fundos;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Cliente
{
    public class ClienteIRLib
    {
        #region Properties
        private static string _ConnectionStringName = "Seguranca";
        private static string _NomeConexaoFundos    = "PlataformaInviXX";
        #endregion

        #region Methods
        /// <summary>
        /// Consultar planos de cliente com filtro de relatório
        /// </summary>
        /// <param name="pRequest">Objeto do tipo ListarProdutosClienteRequest</param>
        /// <returns>Retorna uma lsita com filro efetuado por request</returns>
        public static ConsultarObjetosResponse<ClienteProdutoInfo> ConsultarPlanoClientesFiltrado(ConsultarEntidadeRequest<ClienteProdutoInfo> pRequest)
        {
            ConsultarObjetosResponse<ClienteProdutoInfo> lRetorno = new ConsultarObjetosResponse<ClienteProdutoInfo>();

            ConexaoDbHelper acesso = new ConexaoDbHelper();

            acesso.ConnectionStringName = _ConnectionStringName;

            lRetorno.Resultado = new List<ClienteProdutoInfo>();

            using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_SELECIONAR_CLIENTES_PLANOIR"))
            {
                acesso.AddInParameter(cmd, "@CD_ASSESSOR", DbType.Int32, pRequest.Objeto.CodigoAssessor);

                /*
                 * 
                 * Atenção: Esse input via string não estava funcionando junto com a query da procedure; as datas estavam sendo invertidas pelo servidor.
                 * 
                 * Aparentemente, esse valor "00010101" seria pra indicar um valor SEM filtro por data, porém preciso trocar pra DateTime e 
                 * se fosse usar algum valor pra indicar não-filtro, deveria ser Nulable<Datetime>.
                 * 
                 * Como passar isso aqui sem filtro de data vai retornar registros demais, vou trocar pra Date mesmo e se algum "cliente" dessa função
                 * realmente precisar de não-filtro por data, basta usar um valor inicial muito pra trás e final muito pra frente
                 * 
                 *   - Luciano, 01/03/13
                 * 
                if (pRequest.Objeto.De.ToString("yyyyMMdd") != "00010101")
                    acesso.AddInParameter(cmd, "@DT_INICIAL"    , DbType.String, pRequest.Objeto.De.ToString("yyyyMMdd")   );
                
                if (pRequest.Objeto.Ate.ToString("yyyyMMdd") != "00010101")
                    acesso.AddInParameter(cmd, "@DT_FIM"        , DbType.String, pRequest.Objeto.Ate.ToString("yyyyMMdd")  );
                */

                acesso.AddInParameter(cmd, "@DT_INICIAL", DbType.Date, pRequest.Objeto.De  );
                acesso.AddInParameter(cmd, "@DT_FIM"    , DbType.Date, pRequest.Objeto.Ate );

                if (pRequest.Objeto.CdCblc > 0)
                    acesso.AddInParameter(cmd, "@CD_CBLC", DbType.Int32, pRequest.Objeto.CdCblc);
                
                if (pRequest.Objeto.IdProdutoPlano > 0)
                    acesso.AddInParameter(cmd, "@ID_PRODUTO", DbType.Int32, pRequest.Objeto.IdProdutoPlano);

                DataTable table = acesso.ExecuteDbDataTable(cmd);

                foreach (DataRow dr in table.Rows)
                    lRetorno.Resultado.Add(CriarRegistroFiltrarPlanoClientesInfo(dr));



            }

            if (!pRequest.Objeto.IdProdutoPlano.HasValue || pRequest.Objeto.IdProdutoPlano == 13) //--> Código do produto de Fundos de Investimentos
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = _NomeConexaoFundos;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_termo_relatorio_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente", DbType.Int32, pRequest.Objeto.CdCblc);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtInicial", DbType.Date, pRequest.Objeto.De);
                    lAcessaDados.AddInParameter(lDbCommand, "@dtFinal", DbType.Date, pRequest.Objeto.Ate);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        var lFundo = new ClienteProdutoInfo();

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lFundo = new ClienteProdutoInfo();

                            lFundo.CdCblc              = lDataTable.Rows[i]["idCliente"].DBToInt32();
                            lFundo.NomeProduto         = lDataTable.Rows[i]["NomeFundo"].ToString();
                            lFundo.NomeCliente         = lDataTable.Rows[i]["Ds_nome"].DBToString();
                            lFundo.DsCpfCnpj           = lDataTable.Rows[i]["ds_cpfcnpj"].DBToString();
                            lFundo.DtAdesao            = lDataTable.Rows[i]["DataAdesao"].DBToDateTime();
                            lFundo.VlCobrado           = 0.0M;
                            lFundo.Origem              = lDataTable.Rows[i]["Origem"].DBToString();
                            lFundo.UsuarioLogado       = lDataTable.Rows[i]["UsuarioLogado"].DBToString();
                            lFundo.CodigoUsuarioLogado = lDataTable.Rows[i]["CodigoUsuarioLogado"].DBToInt32();

                            lRetorno.Resultado.Add(lFundo);
                        }
                    }
                }
            }

            return lRetorno;
        }

        #endregion

        #region Métodos de Apoio
        private static ClienteProdutoInfo CriarRegistroFiltrarPlanoClientesInfo(DataRow dr)
        {
            return new ClienteProdutoInfo()
            {
                NomeCliente       = dr["DS_NOME"].DBToString(),
                NomeProduto       = dr["DS_PRODUTO"].DBToString(),
                CdCblc            = dr["CD_CBLC"].DBToInt32(),
                DsCpfCnpj         = dr["DS_CPFCNPJ"].DBToString(),
                StSituacao        = dr["ST_SITUACAO"].DBToChar(),
                DtAdesao          = dr["DT_ADESAO"].DBToDateTime(),
                DtFimAdesao       =   dr["DT_FIM_ADESAO"].DBToDateTime(),
                DtUltima_cobranca = dr["DT_ULTIMA_COBRANCA"].DBToDateTime(),
                IdProdutoPlano    = dr["ID_PRODUTO_PLANO"].DBToInt32(),
                VlCobrado         = dr["VL_COBRADO"].DBToDecimal()
                
            };
        }
        #endregion


    }
}
