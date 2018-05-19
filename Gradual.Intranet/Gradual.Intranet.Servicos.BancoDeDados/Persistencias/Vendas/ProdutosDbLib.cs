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
    public static class ProdutosDbLib
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

        private static ProdutoInfo InstanciarProdutoDaLinha(DataRow pRow)
        {
            ProdutoInfo lRetorno = new ProdutoInfo();

            lRetorno.IdProduto = Convert.ToInt32(pRow["id_produto_plano"]);

            lRetorno.IdPlano = Convert.ToInt32(pRow["id_plano"]);

            if(pRow["ds_produto"] != DBNull.Value)
                lRetorno.DsNomeProduto = Convert.ToString(pRow["ds_produto"]);

            if(pRow["vl_preco"] != DBNull.Value)
                lRetorno.VlPreco = Convert.ToDecimal(pRow["vl_preco"]);
            
            if(pRow["vl_preco_cartao"] != DBNull.Value)
                lRetorno.VlPrecoCartao = Convert.ToDecimal(pRow["vl_preco_cartao"]);

            if(pRow["fl_suspenso"] != DBNull.Value)
                lRetorno.FlSuspenso = Convert.ToBoolean(pRow["fl_suspenso"]);

            if(pRow["ds_mensagem_suspenso"] != DBNull.Value)
                lRetorno.DsMensagemSuspenso = Convert.ToString(pRow["ds_mensagem_suspenso"]);

            if(pRow["fl_aparece_produtos"] != DBNull.Value)
                lRetorno.FlApareceProdutos = Convert.ToBoolean(pRow["fl_aparece_produtos"]);
            
            if(pRow["vl_taxa"] != DBNull.Value)
                lRetorno.VlTaxa = Convert.ToDecimal(pRow["vl_taxa"]);

            if(pRow["vl_taxa2"] != DBNull.Value)
                lRetorno.VlTaxa2 = Convert.ToDecimal(pRow["vl_taxa2"]);

            if(pRow["url_imagem"] != DBNull.Value)
                lRetorno.UrlImagem = Convert.ToString(pRow["url_imagem"]);

            if(pRow["url_imagem2"] != DBNull.Value)
                lRetorno.UrlImagem2 = Convert.ToString(pRow["url_imagem2"]);

            if(pRow["url_imagem3"] != DBNull.Value)
                lRetorno.UrlImagem3 = Convert.ToString(pRow["url_imagem3"]);

            if(pRow["url_imagem4"] != DBNull.Value)
                lRetorno.UrlImagem4 = Convert.ToString(pRow["url_imagem4"]);

            return lRetorno;
        }

        #endregion

        #region Métodos Públicos

        public static ConsultarObjetosResponse<ProdutoInfo> ConsultarProdutos(ConsultarEntidadeRequest<ProdutoInfo> pParametros)
        {
            ConsultarObjetosResponse<ProdutoInfo> lRetorno = new ConsultarObjetosResponse<ProdutoInfo>();

            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = VendasDbLib.NomeDaConexaoDeVendas;

            lRetorno.Resultado = new List<ProdutoInfo>();

            using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_produtos_lst"))
            {
                if (pParametros.Objeto != null && pParametros.Objeto.IdPlano != 0)
                {
                    lAcessaDados.AddInParameter(lCommand, "id_plano", DbType.Int32, pParametros.Objeto.IdPlano);
                }

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetorno.Resultado.Add(InstanciarProdutoDaLinha(lRow));
                }
            }

            return lRetorno;
        }
        
        public static SalvarEntidadeResponse<ProdutoInfo> Salvar(SalvarObjetoRequest<ProdutoInfo> pParametros)
        {
            if (pParametros.Objeto.IdProduto.HasValue)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Incluir(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ProdutoInfo> Atualizar(SalvarObjetoRequest<ProdutoInfo> pParametros)
        {
            SalvarEntidadeResponse<ProdutoInfo> lResponse = new SalvarEntidadeResponse<ProdutoInfo>();

            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = VendasDbLib.NomeDaConexaoDeVendas;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_Produtos_upd"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "@id_produto",           DbType.Int32,   pParametros.Objeto.IdProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_produto",           DbType.String,  pParametros.Objeto.DsNomeProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_preco",             DbType.Decimal, pParametros.Objeto.VlPreco);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_preco_cartao",      DbType.Decimal, pParametros.Objeto.VlPrecoCartao);
                    lAcessaDados.AddInParameter(lDbCommand, "@fl_suspenso",          DbType.Boolean, pParametros.Objeto.FlSuspenso);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_mensagem_suspenso", DbType.String,  pParametros.Objeto.DsMensagemSuspenso);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_descricao",         DbType.String,  pParametros.Objeto.DsDescricao);
                    lAcessaDados.AddInParameter(lDbCommand, "@fl_aparece_produtos",  DbType.Boolean, pParametros.Objeto.FlApareceProdutos);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_taxa",              DbType.Decimal, pParametros.Objeto.VlTaxa);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_taxa2",             DbType.Decimal, pParametros.Objeto.VlTaxa2);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem",           DbType.String,  pParametros.Objeto.UrlImagem);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem2",           DbType.String,  pParametros.Objeto.UrlImagem2);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem3",           DbType.String,  pParametros.Objeto.UrlImagem3);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem4",           DbType.String,  pParametros.Objeto.UrlImagem4);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.Objeto = pParametros.Objeto;

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);

                throw ex;
            }

            return lResponse;
        }

        private static SalvarEntidadeResponse<ProdutoInfo> Incluir(SalvarObjetoRequest<ProdutoInfo> pParametros)
        {
            SalvarEntidadeResponse<ProdutoInfo> lResponse = new SalvarEntidadeResponse<ProdutoInfo>();

            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = VendasDbLib.NomeDaConexaoDeVendas;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_Produtos_ins"))
                {
                    /*
                        @id_plano int
                        ,@ds_produto varchar(500)
                        ,@vl_preco money
                        ,@fl_suspenso bit
                        ,@ds_mensagem_suspenso varchar(500) = null
                        ,@ds_descricao text = null
                        ,@fl_aparece_produtos bit
                        ,@vl_taxa money = null
                        ,@url_imagem varchar(500) = null
                        ,@id_produto int output
                     */

                    lAcessaDados.AddInParameter(lDbCommand, "@id_plano",             DbType.Int32,   pParametros.Objeto.IdPlano);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_produto",           DbType.String,  pParametros.Objeto.DsNomeProduto);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_preco",             DbType.Decimal, pParametros.Objeto.VlPreco);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_preco_cartao",      DbType.Decimal, pParametros.Objeto.VlPrecoCartao);
                    lAcessaDados.AddInParameter(lDbCommand, "@fl_suspenso",          DbType.Boolean, pParametros.Objeto.FlSuspenso);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_mensagem_suspenso", DbType.String,  pParametros.Objeto.DsMensagemSuspenso);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_descricao",         DbType.String,  pParametros.Objeto.DsDescricao);
                    lAcessaDados.AddInParameter(lDbCommand, "@fl_aparece_produtos",  DbType.Boolean, pParametros.Objeto.FlApareceProdutos);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_taxa",              DbType.Decimal, pParametros.Objeto.VlTaxa);
                    lAcessaDados.AddInParameter(lDbCommand, "@vl_taxa2",             DbType.Decimal, pParametros.Objeto.VlTaxa2);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem",           DbType.String,  pParametros.Objeto.UrlImagem);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem2",          DbType.String,  pParametros.Objeto.UrlImagem2);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem3",          DbType.String,  pParametros.Objeto.UrlImagem3);
                    lAcessaDados.AddInParameter(lDbCommand, "@url_imagem4",          DbType.String,  pParametros.Objeto.UrlImagem4);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_produto", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    lResponse.Objeto = pParametros.Objeto;

                    lResponse.Objeto.IdProduto = Convert.ToInt32(lDbCommand.Parameters["@id_produto"].Value);

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);

                throw ex;
            }

            return lResponse;
        }

        #endregion

    }
}
