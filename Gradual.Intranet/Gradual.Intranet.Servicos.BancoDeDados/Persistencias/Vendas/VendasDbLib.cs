using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static class VendasDbLib
    {
        #region Propriedades

        public static string NomeDaConexaoDeVendas
        {
            get
            {
                return "Seguranca"; //os dados de vendas ficam no  DirectTradeControleAcesso
            }
        }

        #endregion

        #region Métodos Private

        private static VendaDeFerramentaInfo InstanciarDaLinha(DataRow pRow)
        {
            VendaDeFerramentaInfo lRetorno = new VendaDeFerramentaInfo();

            lRetorno.IdVenda = pRow["id_venda"].DBToInt32();
            lRetorno.DsCodigoReferencia = pRow["ds_codigo_referencia"].DBToString();

            if (pRow["cd_cblc"] != DBNull.Value)
                lRetorno.CdCBLC = pRow["cd_cblc"].DBToInt32();

            lRetorno.DsCpfCnpj    = pRow["ds_cpfcnpj"].DBToString();
            lRetorno.StStatus     = pRow["st_status"].DBToInt32();
            lRetorno.DtData       = pRow["dt_data"].DBToDateTime();
            lRetorno.VlQuantidade = pRow["vl_quantidade"].DBToInt32();
            lRetorno.VlPreco      = pRow["vl_preco"].DBToDecimal();
            lRetorno.IdProduto    = pRow["id_produto"].DBToInt32();

            if (pRow["ds_produto"] != DBNull.Value)
                lRetorno.DsProduto = pRow["ds_produto"].DBToString();

            if (pRow["ds_tel"] != DBNull.Value)
                lRetorno.TelDeEntrega = pRow["ds_tel"].DBToString();

            if (pRow["ds_cel"] != DBNull.Value)
                lRetorno.CelDeEntrega = pRow["ds_cel"].DBToString();

            lRetorno.IdPagamento    = pRow["id_pagamento"].DBToInt32();
            lRetorno.CdTipo         = pRow["cd_tipo"].DBToInt32();
            lRetorno.CdMetodoTipo   = pRow["cd_metodo_tipo"].DBToInt32();
            lRetorno.CdMetodoCodigo = pRow["cd_metodo_codigo"].DBToInt32();

            if(pRow["ds_metodo_desc"] != DBNull.Value)
                lRetorno.DsMetodoDesc = pRow["ds_metodo_desc"].DBToString();
            
            lRetorno.VlValorBruto         = pRow["vl_valor_bruto"].DBToDecimal();
            lRetorno.VlValorDesconto      = pRow["vl_valor_desconto"].DBToDecimal();
            lRetorno.VlValorTaxas         = pRow["vl_valor_taxas"].DBToDecimal();

            if(pRow["vl_taxas_produto"] != DBNull.Value)
                lRetorno.VlValorTaxaProduto   = pRow["vl_taxas_produto"].DBToDecimal();
            
            if(pRow["ds_observacoes"] != DBNull.Value)
                lRetorno.DsObservacoes   = pRow["ds_observacoes"].DBToString();

            lRetorno.VlValorLiquido       = pRow["vl_valor_liquido"].DBToDecimal();
            lRetorno.VlQuantidadeParcelas = pRow["vl_quantidade_parcelas"].DBToDecimal();

            if (pRow["id_venda_endereco"] != DBNull.Value)
            {
                lRetorno.EnderecoDeEntrega = new ClienteEnderecoInfo();

                if (pRow["cd_cep"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.NrCep = Convert.ToInt32(pRow["cd_cep"]);

                if (pRow["cd_cep_ext"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.NrCepExt = Convert.ToInt32(pRow["cd_cep_ext"]);

                if (pRow["ds_logradouro"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.DsLogradouro = Convert.ToString(pRow["ds_logradouro"]);
                
                if (pRow["ds_bairro"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.DsBairro = Convert.ToString(pRow["ds_bairro"]);
                
                if (pRow["ds_cidade"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.DsCidade = Convert.ToString(pRow["ds_cidade"]);

                if (pRow["cd_uf"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.CdUf = Convert.ToString(pRow["cd_uf"]);
                
                if (pRow["cd_pais"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.CdPais = Convert.ToString(pRow["cd_pais"]);
                
                if (pRow["ds_numero"] != DBNull.Value)
                    lRetorno.EnderecoDeEntrega.DsNumero = Convert.ToString(pRow["ds_numero"]);

                /*
                , ve.ds_logradouro
                , ve.ds_bairro
                , ve.ds_cidade
                , ve.cd_uf
                , ve.cd_pais
                , ve.ds_numero
                 */
            }

            return lRetorno;
        }

        private static PagamentoLogInfo InstanciarPagamentoLogDaLinha(DataRow pRow)
        {
            PagamentoLogInfo lRetorno = new PagamentoLogInfo();

            lRetorno.IdPagamentoLog = pRow["id_pagamento_log"].DBToInt32();

            lRetorno.DtData = pRow["dt_data"].DBToDateTime();
            
            lRetorno.DsTransacao = pRow["ds_transacao"].DBToString();
            lRetorno.DsCodigoReferenciaVenda = pRow["ds_codigo_referencia_venda"].DBToString();
            lRetorno.StDirecao = pRow["st_direcao"].DBToString();
            lRetorno.DsMensagem = pRow["ds_mensagem"].DBToString();
            lRetorno.DsXML = pRow["ds_xml"].DBToString();

            return lRetorno;
        }

        private static AlteracaoDeVendaInfo InstanciarAlteracaoDeVendaDaLinha(DataRow pRow)
        {
            AlteracaoDeVendaInfo lRetorno = new AlteracaoDeVendaInfo();

            lRetorno.IdVendaAlteracao = pRow["id_venda_alteracao"].DBToInt32();

            lRetorno.IdVenda              = pRow["id_venda"].DBToInt32();
            lRetorno.DsPropriedades       = pRow["ds_propriedades"].DBToString();
            lRetorno.DsValoresAnteriores  = pRow["ds_valores_anteriores"].DBToString();
            lRetorno.DsValoresModificados = pRow["ds_valores_modificados"].DBToString();
            lRetorno.DtData               = pRow["dt_data"].DBToDateTime();
            lRetorno.DsUsuario            = pRow["ds_usuario"].DBToString();
            lRetorno.DsMotivo             = pRow["ds_motivo"].DBToString();

            return lRetorno;
        }

        #endregion

        #region Métodos Públicos

        public static ConsultarObjetosResponse<VendaDeFerramentaInfo> ConsultarVendaDeFerramenta(ConsultarEntidadeRequest<VendaDeFerramentaInfo> pParametros)
        {
            ConsultarObjetosResponse<VendaDeFerramentaInfo> lRetorno = new ConsultarObjetosResponse<VendaDeFerramentaInfo>();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = VendasDbLib.NomeDaConexaoDeVendas;

            lRetorno.Resultado = new List<VendaDeFerramentaInfo>();

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_venda_busca"))
            {
                lAcessaDados.AddInParameter(lCommand, "id_venda",   DbType.Int32,    pParametros.Objeto.Busca_IdVenda);
                lAcessaDados.AddInParameter(lCommand, "cd_cblc",    DbType.Int32,    pParametros.Objeto.Busca_CdCBLC);
                lAcessaDados.AddInParameter(lCommand, "ds_cpfcnpj", DbType.String,   pParametros.Objeto.Busca_DsCpfCnpj);
                lAcessaDados.AddInParameter(lCommand, "st_status",  DbType.Int32,    pParametros.Objeto.Busca_StStatus);
                lAcessaDados.AddInParameter(lCommand, "dt_datade",  DbType.DateTime, pParametros.Objeto.Busca_DataDe);
                lAcessaDados.AddInParameter(lCommand, "dt_dataate", DbType.DateTime, pParametros.Objeto.Busca_DataAte);

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetorno.Resultado.Add(InstanciarDaLinha(lRow));
                }
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<PagamentoLogInfo> ConsultarPagamentoLogDaVenda(ConsultarEntidadeRequest<PagamentoLogInfo> pParametros)
        {
            ConsultarObjetosResponse<PagamentoLogInfo> lRetorno = new ConsultarObjetosResponse<PagamentoLogInfo>();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = VendasDbLib.NomeDaConexaoDeVendas;

            lRetorno.Resultado = new List<PagamentoLogInfo>();

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_VendaPagamentosLog_sel"))
            {
                lAcessaDados.AddInParameter(lCommand, "id_venda", DbType.Int32, pParametros.Objeto.Busca_IdVenda);

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetorno.Resultado.Add(InstanciarPagamentoLogDaLinha(lRow));
                }
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<AlteracaoDeVendaInfo> ConsultarAlteracoesDaVenda(ConsultarEntidadeRequest<AlteracaoDeVendaInfo> pParametros)
        {
            ConsultarObjetosResponse<AlteracaoDeVendaInfo> lRetorno = new ConsultarObjetosResponse<AlteracaoDeVendaInfo>();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = VendasDbLib.NomeDaConexaoDeVendas;

            lRetorno.Resultado = new List<AlteracaoDeVendaInfo>();

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_venda_alteracao_lst"))
            {
                lAcessaDados.AddInParameter(lCommand, "id_venda", DbType.Int32, pParametros.Objeto.Busca_IdVenda);

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetorno.Resultado.Add(InstanciarAlteracaoDeVendaDaLinha(lRow));
                }
            }

            return lRetorno;
        }
        
        public static SalvarEntidadeResponse<AlteracaoDeVendaInfo> SalvarAlteracaoDeVenda(SalvarObjetoRequest<AlteracaoDeVendaInfo> pParametros)
        {
            try
            {
                SalvarEntidadeResponse<AlteracaoDeVendaInfo> lResponse;

                AcessaDados lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = VendasDbLib.NomeDaConexaoDeVendas;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_venda_alteracao_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "id_venda",                 DbType.Int32,    pParametros.Objeto.IdVenda);
                    lAcessaDados.AddInParameter(lDbCommand, "ds_propriedades",          DbType.String,   pParametros.Objeto.DsPropriedades);
                    lAcessaDados.AddInParameter(lDbCommand, "ds_valores_anteriores",    DbType.String,   pParametros.Objeto.DsValoresAnteriores);
                    lAcessaDados.AddInParameter(lDbCommand, "ds_valores_modificados",   DbType.String,   pParametros.Objeto.DsValoresModificados);
                    lAcessaDados.AddInParameter(lDbCommand, "dt_data",                  DbType.DateTime, pParametros.Objeto.DtData);
                    lAcessaDados.AddInParameter(lDbCommand, "ds_usuario",               DbType.String,   pParametros.Objeto.DsUsuario);
                    lAcessaDados.AddInParameter(lDbCommand, "ds_motivo",                DbType.String,   pParametros.Objeto.DsMotivo);

                    lAcessaDados.AddOutParameter(lDbCommand, "CodigoVendaAlteracao", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse = new SalvarEntidadeResponse<AlteracaoDeVendaInfo>();

                    lResponse.Codigo = lDbCommand.Parameters["CodigoVendaAlteracao"].Value.DBToInt32();

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return lResponse;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir,ex);

                throw ex;
            }
        }

        #endregion

    }
}
