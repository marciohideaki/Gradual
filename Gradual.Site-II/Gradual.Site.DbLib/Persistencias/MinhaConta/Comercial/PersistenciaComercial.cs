using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta
{
    public class PersistenciaComercial
    {
        #region Métodos Private

        private List<CompraInfo> InstanciarListaDeVendasDaTabela(DataTable pTabela)
        {
            List<CompraInfo> lRetorno = new List<CompraInfo>();

            if (pTabela.Rows.Count > 0)
            {
                int lIdCompra, lIdProduto, lIdPagamento;

                Dictionary<int, CompraInfo> lCompras = new Dictionary<int, CompraInfo>();

                List<int> lIdsDeProdutos = new List<int>();
                List<int> lIdsDePagamentos = new List<int>();

                PagamentoInfo lPagamento;
                CompraProdutoInfo lProduto;

                //A view é toda "normalizada" porque normalmente só temos 1 produto e 1 pagamento por venda; mas se vier mais de um,
                //essas verificações de ID desfazem a normalização e inserem "n" produtos e "z" pagamentos dentro da mesma venda sem problemas, sem repetição.

                foreach (DataRow lRow in pTabela.Rows)
                {
                    lIdCompra = lRow["id_venda"].DBToInt32();
                    lIdProduto = lRow["id_produto"].DBToInt32();
                    lIdPagamento = lRow["id_pagamento"].DBToInt32();

                    if (!lCompras.ContainsKey(lIdCompra))
                    {
                        lCompras.Add(lIdCompra, new CompraInfo());

                        lCompras[lIdCompra].DadosDaCompra = new VendaInfo();
                        
                        lCompras[lIdCompra].DadosDaCompra.IdVenda           = lIdCompra;
                        lCompras[lIdCompra].DadosDaCompra.CblcCliente       = lRow["cd_cblc"].DBToInt32();
                        lCompras[lIdCompra].DadosDaCompra.CpfCnpjCliente    = lRow["ds_cpfcnpj"].DBToString();
                        lCompras[lIdCompra].DadosDaCompra.ReferenciaDaVenda = lRow["ds_codigo_referencia"].DBToString();
                        lCompras[lIdCompra].DadosDaCompra.Status            = lRow["st_status"].DBToInt32();
                        lCompras[lIdCompra].DadosDaCompra.Data              = lRow["dt_data"].DBToDateTime();

                        lIdsDeProdutos = new List<int>();
                        lIdsDePagamentos = new List<int>();
                    }

                    if (!lIdsDeProdutos.Contains(lIdProduto))
                    {
                        lProduto = new CompraProdutoInfo();

                        lProduto.IdProduto = lIdProduto;

                        lProduto.Descricao      = lRow["ds_produto"].DBToString();
                        lProduto.PrecoNaCompra  = lRow["vl_valor_bruto"].DBToDecimal();
                        lProduto.PrecoAtual     = lRow["vl_preco"].DBToDecimal();
                        lProduto.Quantidade     = lRow["vl_quantidade"].DBToInt32();
                        lProduto.Text           = lRow["ds_descricao"].DBToString();

                        lCompras[lIdCompra].Produtos.Add(lProduto);

                        lIdsDeProdutos.Add(lIdProduto);
                    }

                    if (!lIdsDePagamentos.Contains(lIdPagamento))
                    {
                        lPagamento = new PagamentoInfo();

                        lPagamento.IdPagamento = lIdPagamento;

                        lPagamento.MetodoCodigo         = lRow["cd_metodo_codigo"].DBToInt32();
                        lPagamento.MetodoTipo           = lRow["cd_metodo_tipo"].DBToInt32();
                        lPagamento.QuantidadeDeParcelas = lRow["vl_quantidade_parcelas"].DBToInt32();
                        lPagamento.Tipo                 = lRow["cd_tipo"].DBToInt32();
                        lPagamento.ValorBruto           = lRow["vl_valor_bruto"].DBToDecimal();
                        lPagamento.ValorDesconto        = lRow["vl_valor_desconto"].DBToDecimal();
                        lPagamento.ValorLiquido         = lRow["vl_valor_liquido"].DBToDecimal();
                        lPagamento.ValorTaxas           = lRow["vl_valor_taxas"].DBToDecimal();

                        lCompras[lIdCompra].Pagamentos.Add(lPagamento);

                        lIdsDePagamentos.Add(lIdPagamento);
                    }

                }

                foreach (int lChave in lCompras.Keys)
                {
                    lRetorno.Add(lCompras[lChave]);
                }
            }

            return lRetorno;
        }

        private List<ProdutoCompradoInfo> InstanciarListaDeProdutosDaTabela(DataTable pTabela)
        {
            List<ProdutoCompradoInfo> lRetorno = new List<ProdutoCompradoInfo>();

            ProdutoCompradoInfo lProduto;

            foreach (DataRow lRow in pTabela.Rows)
            {
                lProduto = new ProdutoCompradoInfo();

                lProduto.IdProduto                 = lRow["id_produto"].DBToInt32();
                lProduto.DescricaoProduto          = lRow["ds_produto"].DBToString();
                lProduto.IdVenda                   = lRow["id_venda"].DBToInt32();
                lProduto.CodigoDeReferenciaDaVenda = lRow["ds_codigo_referencia"].DBToString();
                lProduto.CodigoCblc                = lRow["cd_cblc"].DBToInt32();
                lProduto.CpfCnpj                   = lRow["ds_cpfcnpj"].DBToString();
                lProduto.Status                    = lRow["st_status"].DBToInt32();
                lProduto.DataDaCompra              = lRow["dt_data"].DBToDateTime();
                lProduto.Text = lRow["ds_descricao"].DBToString();

                lRetorno.Add(lProduto);
            }

            return lRetorno;
        }

        #endregion

        #region Métodos Públicos

        public BuscarDadosDosProdutosResponse BuscarDadosDosProdutos(BuscarDadosDosProdutosRequest pRequest)
        {
            BuscarDadosDosProdutosResponse lRetorno = new BuscarDadosDosProdutosResponse();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_produtos_lst"))
            {
                DataTable lTabela = lDados.ExecuteDbDataTable(lCommand);

                int lId;

                lRetorno.DadosDosProdutos = new List<ProdutoInfo>();

                foreach (DataRow lRow in lTabela.Rows)
                {
                    lId = lRow["id_produto_plano"].DBToInt32();

                    if (pRequest.IdProduto.HasValue)
                    {
                        if (pRequest.IdProduto.Value == lId)
                        {
                            lRetorno.DadosDosProdutos.Add(ProdutoInfo.FromDataRow(lRow));

                            break;
                        }
                    }
                    else
                    {
                        lId = lRow["id_plano"].DBToInt32();

                        if (!pRequest.Plano.HasValue || pRequest.Plano.Value == lId)
                        {
                            lRetorno.DadosDosProdutos.Add(ProdutoInfo.FromDataRow(lRow));
                        }
                    }
                }

                lRetorno.DescricaoResposta = string.Format("{0} produtos encontrados, {1} filtrados para a resposta", lTabela.Rows.Count, lRetorno.DadosDosProdutos.Count);

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }

            return lRetorno;
        }

        public InserirLogDePagamentoResponse InserirLogDePagamento(InserirLogDePagamentoRequest pRequest)
        {
            InserirLogDePagamentoResponse lRetorno = new InserirLogDePagamentoResponse();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_pagamento_log_ins"))
            {
                lDados.AddInParameter(lCommand, "dt_data"                    , DbType.DateTime   , pRequest.LogDePagamento.Data                     );
                lDados.AddInParameter(lCommand, "ds_transacao"               , DbType.String     , pRequest.LogDePagamento.ReferenciaDaTransacao    );
                lDados.AddInParameter(lCommand, "ds_codigo_referencia_venda" , DbType.String     , pRequest.LogDePagamento.ReferenciaDaVenda        );
                lDados.AddInParameter(lCommand, "st_direcao"                 , DbType.String     , pRequest.LogDePagamento.Direcao                  );
                lDados.AddInParameter(lCommand, "ds_mensagem"                , DbType.String     , pRequest.LogDePagamento.Mensagem                 );
                lDados.AddInParameter(lCommand, "ds_xml"                     , DbType.String     , pRequest.LogDePagamento.ConteudoXML              );

                object lRetornoBanco = lDados.ExecuteScalar(lCommand);

                int lIdRetorno;

                if (int.TryParse(Convert.ToString(lRetornoBanco), out lIdRetorno))
                    lRetorno.IdDoRegistroIncluido = lIdRetorno;

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }

            return lRetorno;
        }

        public InserirPagamentoResponse InserirPagamento(InserirPagamentoRequest pRequest)
        {
            InserirPagamentoResponse lRetorno = new InserirPagamentoResponse();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_pagamento_ins"))
            {
                lDados.AddInParameter(lCommand, "id_venda"               , DbType.Int32  , pRequest.Pagamento.IdVenda              );
                lDados.AddInParameter(lCommand, "id_transacao"           , DbType.Int32  , null                                    );
                lDados.AddInParameter(lCommand, "cd_tipo"                , DbType.Int32  , pRequest.Pagamento.Tipo                 );
                lDados.AddInParameter(lCommand, "cd_metodo_tipo"         , DbType.Int32  , pRequest.Pagamento.MetodoTipo           );
                lDados.AddInParameter(lCommand, "cd_metodo_codigo"       , DbType.Int32  , pRequest.Pagamento.MetodoCodigo         );
                lDados.AddInParameter(lCommand, "ds_metodo_desc"         , DbType.String , pRequest.Pagamento.MetodoCodigoDesc     );
                lDados.AddInParameter(lCommand, "vl_valor_bruto"         , DbType.Decimal, pRequest.Pagamento.ValorBruto           );
                lDados.AddInParameter(lCommand, "vl_valor_desconto"      , DbType.Decimal, pRequest.Pagamento.ValorDesconto        );
                lDados.AddInParameter(lCommand, "vl_valor_taxas"         , DbType.Decimal, pRequest.Pagamento.ValorTaxas           );
                lDados.AddInParameter(lCommand, "vl_valor_liquido"       , DbType.Decimal, pRequest.Pagamento.ValorLiquido         );
                lDados.AddInParameter(lCommand, "vl_quantidade_parcelas" , DbType.Int32  , pRequest.Pagamento.QuantidadeDeParcelas );

                object lRetornoBanco = lDados.ExecuteScalar(lCommand);

                int lIdRetorno;

                if (int.TryParse(Convert.ToString(lRetornoBanco), out lIdRetorno))
                    lRetorno.IdDoRegistroIncluido = lIdRetorno;

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }

            return lRetorno;
        }

        public InserirTransacaoResponse InserirTransacao(InserirTransacaoRequest pRequest)
        {
            InserirTransacaoResponse lRetorno = new InserirTransacaoResponse();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_transacao_ins"))
            {
                lDados.AddInParameter(lCommand, "dt_data"                           , DbType.DateTime  , pRequest.Transacao.Data                        );
                lDados.AddInParameter(lCommand, "ds_codigo_transacao_no_gateway"    , DbType.String    , pRequest.Transacao.CodigoDaTransacaoNoGateway  );
                lDados.AddInParameter(lCommand, "ds_codigo_referencia_venda"        , DbType.String    , pRequest.Transacao.CodigoDeReferenciaDaVenda   );
                lDados.AddInParameter(lCommand, "cd_tipo"                           , DbType.Int32     , pRequest.Transacao.Tipo                        );
                lDados.AddInParameter(lCommand, "cd_status"                         , DbType.Int32     , pRequest.Transacao.Status                      );
                lDados.AddInParameter(lCommand, "cd_metodo_tipo"                    , DbType.Int32     , pRequest.Transacao.MetodoTipo                  );
                lDados.AddInParameter(lCommand, "cd_metodo_codigo"                  , DbType.Int32     , pRequest.Transacao.MetodoCodigo                );
                lDados.AddInParameter(lCommand, "ds_metodo_desc"                    , DbType.String    , pRequest.Transacao.MetodoCodigoDesc            );
                lDados.AddInParameter(lCommand, "vl_valor_bruto"                    , DbType.Decimal   , pRequest.Transacao.ValorBruto                  );
                lDados.AddInParameter(lCommand, "vl_valor_desconto"                 , DbType.Decimal   , pRequest.Transacao.ValorDesconto               );
                lDados.AddInParameter(lCommand, "vl_valor_taxas"                    , DbType.Decimal   , pRequest.Transacao.ValorTaxas                  );
                lDados.AddInParameter(lCommand, "vl_valor_liquido"                  , DbType.Decimal   , pRequest.Transacao.ValorLiquido                );
                lDados.AddInParameter(lCommand, "vl_quantidade_parcelas"            , DbType.Decimal   , pRequest.Transacao.QuantidadeDeParcelas        );
                lDados.AddInParameter(lCommand, "vl_quantidade_itens"               , DbType.Decimal   , pRequest.Transacao.QuantidadeDeItens           );
                lDados.AddInParameter(lCommand, "ds_item1_desc"                     , DbType.String    , pRequest.Transacao.Item1_Descricao             );
                lDados.AddInParameter(lCommand, "ds_sender_email"                   , DbType.String    , pRequest.Transacao.Cliente_Email               );
                lDados.AddInParameter(lCommand, "ds_sender_nome"                    , DbType.String    , pRequest.Transacao.Cliente_Nome                );
                lDados.AddInParameter(lCommand, "ds_sender_telefone"                , DbType.String    , pRequest.Transacao.Cliente_Telefone            );
                lDados.AddInParameter(lCommand, "ds_xml_recebido_gateway"           , DbType.String    , pRequest.Transacao.XmlRecebidoDoGateway        );

                object lRetornoBanco = lDados.ExecuteScalar(lCommand);

                int lIdRetorno;

                if (int.TryParse(Convert.ToString(lRetornoBanco), out lIdRetorno))
                    lRetorno.IdDoRegistroIncluido = lIdRetorno;

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }

            return lRetorno;
        }

        public InserirVendaResponse InserirVenda(InserirVendaRequest pRequest)
        {
            InserirVendaResponse lRetorno = new InserirVendaResponse();

            AcessaDados lDados = new AcessaDados();

            DbConnection lConexao;

            Conexao Conexao = new Generico.Dados.Conexao();

            DbTransaction lTransacao;

            DbCommand lComandoVenda;

            DbCommand lComandoProdutos;

            Conexao._ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            lConexao = Conexao.CreateIConnection();

            lConexao.Open();

            lTransacao = lConexao.BeginTransaction();

            try
            {
                //Insere Venda

                lComandoVenda = lDados.CreateCommand(lTransacao, CommandType.StoredProcedure, "prc_venda_ins");

                lDados.AddInParameter(lComandoVenda, "ds_codigo_referencia"  , DbType.String     , pRequest.Venda.ReferenciaDaVenda );
                lDados.AddInParameter(lComandoVenda, "cd_cblc"               , DbType.Int32      , pRequest.Venda.CblcCliente       );
                lDados.AddInParameter(lComandoVenda, "ds_cpfcnpj"            , DbType.String     , pRequest.Venda.CpfCnpjCliente    );
                lDados.AddInParameter(lComandoVenda, "st_status"             , DbType.Int32      , pRequest.Venda.Status            );
                lDados.AddInParameter(lComandoVenda, "dt_data"               , DbType.DateTime   , pRequest.Venda.Data              );
                lDados.AddOutParameter(lComandoVenda, "id_inserido"          , DbType.Int32      , 12);

                lDados.ExecuteNonQuery(lComandoVenda, lTransacao);

                int lIdRetornoVenda = lDados.GetParameterValue(lComandoVenda, "id_inserido").DBToInt32();

                lRetorno.IdDoRegistroIncluido = lIdRetornoVenda;

                if (pRequest.Venda.EnderecoDeEntrega != null)
                {
                    lComandoVenda = lDados.CreateCommand(lTransacao, CommandType.StoredProcedure, "prc_venda_endereco_ins");

                    lDados.AddInParameter(lComandoVenda, "id_venda"       , DbType.Int32,  lIdRetornoVenda                                  );
                    lDados.AddInParameter(lComandoVenda, "cd_cep"         , DbType.String, pRequest.Venda.EnderecoDeEntrega.NrCep           );
                    lDados.AddInParameter(lComandoVenda, "cd_cep_ext"     , DbType.String, pRequest.Venda.EnderecoDeEntrega.NrCepExt );
                    lDados.AddInParameter(lComandoVenda, "ds_logradouro"  , DbType.String, pRequest.Venda.EnderecoDeEntrega.DsLogradouro );
                    lDados.AddInParameter(lComandoVenda, "ds_complemento" , DbType.String, pRequest.Venda.EnderecoDeEntrega.DsComplemento );
                    lDados.AddInParameter(lComandoVenda, "ds_bairro"      , DbType.String, pRequest.Venda.EnderecoDeEntrega.DsBairro     );
                    lDados.AddInParameter(lComandoVenda, "ds_cidade"      , DbType.String, pRequest.Venda.EnderecoDeEntrega.DsCidade        );
                    lDados.AddInParameter(lComandoVenda, "cd_uf"          , DbType.String, pRequest.Venda.EnderecoDeEntrega.CdUf            );
                    lDados.AddInParameter(lComandoVenda, "cd_pais"        , DbType.String, pRequest.Venda.EnderecoDeEntrega.CdPais          );
                    lDados.AddInParameter(lComandoVenda, "ds_numero"      , DbType.String, pRequest.Venda.EnderecoDeEntrega.DsNumero        );

                    lDados.AddInParameter(lComandoVenda, "tel_ddd" , DbType.String, pRequest.Venda.TelefoneDeEntrega.DsDdd);
                    lDados.AddInParameter(lComandoVenda, "tel_num" , DbType.String, pRequest.Venda.TelefoneDeEntrega.DsNumero);
                    lDados.AddInParameter(lComandoVenda, "cel_ddd" , DbType.String, pRequest.Venda.CelularDeEntrega.DsDdd);
                    lDados.AddInParameter(lComandoVenda, "cel_num" , DbType.String, pRequest.Venda.CelularDeEntrega.DsNumero);

                    lDados.AddOutParameter(lComandoVenda, "id_venda_endereco", DbType.Int32, 12);

                    lDados.ExecuteNonQuery(lComandoVenda, lTransacao);

                    //lIdRetornoVenda = lDados.GetParameterValue(lComandoVenda, "id_venda_endereco").DBToInt32();
                }

                foreach (VendaProdutoInfo lProduto in pRequest.Venda.Produtos)
                {
                    lComandoProdutos = lDados.CreateCommand(lTransacao, CommandType.StoredProcedure, "prc_venda_produtos_ins");

                    lDados.AddInParameter(lComandoProdutos, "id_venda"         , DbType.Int32, lIdRetornoVenda       );
                    lDados.AddInParameter(lComandoProdutos, "id_produto"       , DbType.Int32, lProduto.IdProduto    );
                    lDados.AddInParameter(lComandoProdutos, "vl_quantidade"    , DbType.Int32, lProduto.Quantidade   );
                    lDados.AddInParameter(lComandoProdutos, "vl_preco"         , DbType.Decimal, lProduto.Preco      );
                    lDados.AddInParameter(lComandoProdutos, "vl_taxas"         , DbType.Decimal, lProduto.TaxasTotal );
                    lDados.AddInParameter(lComandoProdutos, "ds_observacoes"   , DbType.String, lProduto.Obervacoes );

                    lDados.ExecuteNonQuery(lComandoProdutos, lTransacao);
                }

                lTransacao.Commit();

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                return lRetorno;
            }
            catch (Exception ex)
            {
                lTransacao.Rollback();

                throw ex;
            }
        }

        public InserirProdutoPorTransacaoResponse InserirProdutoPorTransacao(InserirProdutoPorTransacaoRequest pRequest)
        {
            InserirProdutoPorTransacaoResponse lRetorno = new InserirProdutoPorTransacaoResponse();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_ClienteProduto_ins_byTransaction"))
            {
                lDados.AddInParameter(lCommand, "id_transacao"     , DbType.Int32    , pRequest.ClienteProduto.IdDaTransacao  );
                lDados.AddInParameter(lCommand, "dt_operacao"      , DbType.DateTime , pRequest.ClienteProduto.DataAdesao     );
                lDados.AddInParameter(lCommand, "st_situacao"      , DbType.String   , pRequest.ClienteProduto.Situacao       );

                if(pRequest.ClienteProduto.DataFimAdesao.HasValue)
                {
                    lDados.AddInParameter(lCommand, "dt_fim_adesao"    , DbType.DateTime , pRequest.ClienteProduto.DataFimAdesao.Value);
                }
                else
                {
                    lDados.AddInParameter(lCommand, "dt_fim_adesao"    , DbType.DateTime , DBNull.Value);
                }

                object lRetornoBanco = lDados.ExecuteScalar(lCommand);

                int lIdRetorno;

                if (int.TryParse(Convert.ToString(lRetornoBanco), out lIdRetorno))
                    lRetorno.IdDoRegistroIncluido = lIdRetorno;

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }

            return lRetorno;
        }

        public BuscarComprasDoClienteResponse BuscarComprasDoCliente(BuscarComprasDoClienteRequest pRequest)
        {
            BuscarComprasDoClienteResponse lRetorno = new BuscarComprasDoClienteResponse();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_compras_lst"))
            {
                lDados.AddInParameter(lCommand, "ds_cpfcnpj", DbType.String, pRequest.CpfCnpj );

                DataTable lTabela = lDados.ExecuteDbDataTable(lCommand);

                lRetorno.ListaDeCompras = InstanciarListaDeVendasDaTabela(lTabela);

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }

            return lRetorno;
        }
        
        public BuscarProdutosDoClienteResponse BuscarProdutosDoCliente(BuscarProdutosDoClienteRequest pRequest)
        {
            BuscarProdutosDoClienteResponse lRetorno = new BuscarProdutosDoClienteResponse();

            AcessaDados lDados = new AcessaDados();

            Conexao lConexao = new Generico.Dados.Conexao();

            lDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSeguranca;

            using (DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_produtos_lst"))
            {
                lDados.AddInParameter(lCommand, "ds_cpfcnpj", DbType.String, pRequest.CpfCnpj );

                DataTable lTabela = lDados.ExecuteDbDataTable(lCommand);

                lRetorno.ListaDeProdutos = InstanciarListaDeProdutosDaTabela(lTabela);

                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                lRetorno.DataResposta = DateTime.Now;
            }

            return lRetorno;
        }

        #endregion
    }
}
