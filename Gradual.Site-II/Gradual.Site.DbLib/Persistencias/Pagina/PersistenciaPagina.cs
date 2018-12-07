using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Site.DbLib.Dados;
using Gradual.Site.DbLib.Mensagens;
using System.Globalization;

namespace Gradual.Site.DbLib.Persistencias.Pagina
{
    public class PersistenciaPagina
    {

        public static string ConexaoPortal
        {
            get { return "ConexaoPortal"; }
        }

        #region Selecao

        public ConteudoResponse SelecionarConteudo(ConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            ConteudoResponse lRetorno = new ConteudoResponse();

            lRetorno.ListaConteudo = new List<ConteudoInfo>();

            lRetorno.Conteudo = pRequest.Conteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SCONTEUDO"))
            {
                if (pRequest.Conteudo.CodigoConteudo> 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoConteudo);

                if (pRequest.Conteudo.CodigoTipoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoTipoConteudo);

               DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaConteudo.Add(this.MontaObjetoConteudo(linha));
            }
            return lRetorno;
        }

        public ConteudoResponse SelecionarConteudoPorPropriedade(ConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            ConteudoResponse lRetorno = new ConteudoResponse();

            lRetorno.ListaConteudo = new List<ConteudoInfo>();

            lRetorno.Conteudo = pRequest.Conteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SCONTEUDO_POR_PROPRIEDADE"))
            {
                if (pRequest.Conteudo.CodigoTipoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoTipoConteudo);

                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE01", DbType.String, pRequest.Conteudo.ValorPropriedade1);
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE02", DbType.String, pRequest.Conteudo.ValorPropriedade2);
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE03", DbType.String, pRequest.Conteudo.ValorPropriedade3);
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE04", DbType.String, pRequest.Conteudo.ValorPropriedade4);
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE05", DbType.String, pRequest.Conteudo.ValorPropriedade5);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaConteudo.Add(this.MontaObjetoConteudo(linha));
            }
            return lRetorno;
        }

        public ConteudoResponse SelecionarConteudoEntreDatas(ConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            ConteudoResponse lRetorno = new ConteudoResponse();

            lRetorno.ListaConteudo = new List<ConteudoInfo>();

            lRetorno.Conteudo = pRequest.Conteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SCONTEUDO_ENTRE_DATAS"))
            {
                if (pRequest.Conteudo.CodigoTipoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoTipoConteudo);

                _AcessaDados.AddInParameter(_DbCommand, "@P_DATA", DbType.DateTime, pRequest.Conteudo.DataConsulta);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaConteudo.Add(this.MontaObjetoConteudo(linha));
            }
            return lRetorno;
        }

        public TipoConteudoResponse SelecionarTipoConteudo(TipoConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            TipoConteudoResponse lRetorno = new TipoConteudoResponse();

            lRetorno.ListaTipoConteudo = new List<TipoDeConteudoInfo>();

            lRetorno.TipoConteudo = pRequest.TipoConteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_STIPO_CONTEUDO"))
            {
                if (pRequest.TipoConteudo.IdTipoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.TipoConteudo.IdTipoConteudo);

               
                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaTipoConteudo.Add(this.MontaObjetoTipoConteudo(linha));
            }
            return lRetorno;
        }

        public WidgetResponse SelecionarWdiget(WidgetRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            WidgetResponse lRetorno = new WidgetResponse();

            lRetorno.ListaWidget = new List<WidgetInfo>();

            lRetorno.Widget = pRequest.Widget;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SWIDGET"))
            {
                if (pRequest.Widget.CodigoWidget > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_WIDGET", DbType.Int32, pRequest.Widget.CodigoWidget);

                if (pRequest.Widget.CodigoEstrutura > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ESTRUTURA", DbType.Int32, pRequest.Widget.CodigoEstrutura);

                if (pRequest.Widget.CodigoListaConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CONTEUDO", DbType.Int32, pRequest.Widget.CodigoListaConteudo);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaWidget.Add(this.MontaObjetoWidget(linha));
            }
            return lRetorno;
        }

        public WidgetResponse SelecionarWdigetOrdemDecrescente(WidgetRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            WidgetResponse lRetorno = new WidgetResponse();

            lRetorno.ListaWidget = new List<WidgetInfo>();

            lRetorno.Widget = pRequest.Widget;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SWIDGET_ORDEM_DECRESCENTE"))
            {
                if (pRequest.Widget.CodigoWidget > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_WIDGET", DbType.Int32, pRequest.Widget.CodigoWidget);

                if (pRequest.Widget.CodigoEstrutura > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ESTRUTURA", DbType.Int32, pRequest.Widget.CodigoEstrutura);

                if (pRequest.Widget.CodigoListaConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CONTEUDO", DbType.Int32, pRequest.Widget.CodigoListaConteudo);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaWidget.Add(this.MontaObjetoWidget(linha));
            }
            return lRetorno;
        }

        public EstruturaResponse SelecionarEstrutura(EstruturaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            EstruturaResponse lRetorno = new EstruturaResponse();

            lRetorno.ListaEstrutura = new List<EstruturaInfo>();

            lRetorno.Estrutura = pRequest.Estrutura;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SESTRUTURA"))
            {
                if (pRequest.Estrutura.CodigoEstrutura > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ESTRUTURA", DbType.Int32, pRequest.Estrutura.CodigoEstrutura);

                if (pRequest.Estrutura.CodigoPagina > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, pRequest.Estrutura.CodigoPagina);

                if (pRequest.Estrutura.TipoUsuario > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_TP_USUARIO", DbType.Int32, pRequest.Estrutura.TipoUsuario);

                /*
                if (pRequest.Estrutura.FlagPublicada != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_FL_PUBLICADA", DbType.Boolean, pRequest.Estrutura.FlagPublicada);
                else
                    _AcessaDados.AddInParameter(_DbCommand, "@P_FL_PUBLICADA", DbType.Boolean, true);
                */

                if (pRequest.Estrutura.NomeAutor != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_NM_AUTOR", DbType.Int32, pRequest.Estrutura.NomeAutor);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaEstrutura.Add(this.MontaObjetoEstrutura(linha));
            }
            return lRetorno;
        }

        public PaginaResponse SelecionarPagina(PaginaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            PaginaResponse lRetorno = new PaginaResponse();

            lRetorno.ListaPagina = new List<PaginaInfo>();

            lRetorno.Pagina = pRequest.Pagina;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SPAGINA"))
            {
                if (pRequest.Pagina.CodigoPagina > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, pRequest.Pagina.CodigoPagina);
                
                if (pRequest.Pagina.NomePagina != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_NM_PAGINA", DbType.String, pRequest.Pagina.NomePagina);
                
                if (pRequest.Pagina.DescURL != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_URL_PAGINA", DbType.String, pRequest.Pagina.DescURL);
                
                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (tabela.Rows.Count > 0)
                {
                    lRetorno.Pagina.CodigoPagina = Convert.ToInt32(tabela.Rows[0]["ID_PAGINA"]);

                    foreach (DataRow linha in tabela.Rows)
                        lRetorno.ListaPagina.Add(this.MontaObjetoPagina(linha));
                }
            }

            return lRetorno;
        }
        
        public PaginaResponse SelecionarPaginaCompleta(PaginaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            PaginaResponse lRetorno = new PaginaResponse();

            lRetorno.ListaPagina = new List<PaginaInfo>();

            lRetorno.Pagina = pRequest.Pagina;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SPAGINA_COMPLETA"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, pRequest.Pagina.CodigoPagina);

                if (pRequest.VersaoDaEstrutura != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VERSAO", DbType.String, pRequest.VersaoDaEstrutura);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (tabela.Rows.Count > 0)
                {
                    int lIdPagina = -1;
                    int lIdVersao = -1;
                    int lIdEstrutura = -1;
                    int lIdWidget = -1;

                    foreach (DataRow lRow in tabela.Rows)
                    {
                        if (lRow["ID_PAGINA"] != DBNull.Value && lIdPagina != Convert.ToInt32(lRow["ID_PAGINA"]))
                        {
                            lIdPagina = Convert.ToInt32(lRow["ID_PAGINA"]);

                            lRetorno.Pagina = MontaObjetoPagina(lRow, false);
                        }
                        
                        if (lRow["ID_VERSAO"] != DBNull.Value && lIdVersao != Convert.ToInt32(lRow["ID_VERSAO"]))
                        {
                            lIdVersao = Convert.ToInt32(lRow["ID_VERSAO"]);

                            lRetorno.Pagina.Versoes.Add(MontaObjetoVersao(lRow));
                        }

                        if (lRow["ID_ESTRUTURA"] != DBNull.Value && lIdEstrutura != Convert.ToInt32(lRow["ID_ESTRUTURA"]))
                        {
                            lIdEstrutura = Convert.ToInt32(lRow["ID_ESTRUTURA"]);

                            lRetorno.Pagina.Versoes[lRetorno.Pagina.Versoes.Count - 1].ListaEstrutura.Add(MontaObjetoEstrutura(lRow, false));
                        }

                        if (lRow["ID_WIDGET"] != DBNull.Value && lIdWidget != Convert.ToInt32(lRow["ID_WIDGET"]))
                        {
                            lIdWidget = Convert.ToInt32(lRow["ID_WIDGET"]);

                            lRetorno.Pagina.Versoes[lRetorno.Pagina.Versoes.Count - 1].ListaEstrutura[lRetorno.Pagina.Versoes[lRetorno.Pagina.Versoes.Count - 1].ListaEstrutura.Count - 1].ListaWidget.Add(MontaObjetoWidget(lRow));
                        }
                    }
                }
            }

            return lRetorno;
        }

        public PaginaResponse SelecionarPaginas(PaginaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            PaginaResponse lRetorno = new PaginaResponse();

            lRetorno.ListaPagina = new List<PaginaInfo>();

            lRetorno.Pagina = pRequest.Pagina;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SPAGINA_LST"))
            {
                DataTable lTable = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow lLinha in lTable.Rows)
                    lRetorno.ListaPagina.Add(this.MontaObjetoPagina(lLinha, false));
            }

            return lRetorno;
        }

        public BuscarVersoesResponse BuscarVersoes(BuscarVersoesRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            BuscarVersoesResponse lRetorno = new BuscarVersoesResponse();

            lRetorno.Versoes = new List<string>();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SVERSOES"))
            {
                DataTable lTable = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow lLinha in lTable.Rows)
                {
                    lRetorno.Versoes.Add(lLinha["NM_IDENTIFICACAO"].DBToString());
                }
            }

            return lRetorno;
        }

        public VersaoResponse IncluirVersao(VersaoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            VersaoResponse lRetorno = new VersaoResponse();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_MVERSAO"))
            {
                _AcessaDados.AddOutParameter(_DbCommand, "@P_ID_VERSAO", DbType.Int32, 1);
                
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, pRequest.Versao.CodigoPagina);

                _AcessaDados.AddInParameter(_DbCommand, "@P_NM_IDENTIFICACAO", DbType.String, pRequest.Versao.CodigoDeIdentificacao);

                _AcessaDados.ExecuteNonQuery(_DbCommand);

                lRetorno.Versao = pRequest.Versao;

                lRetorno.Versao.CodigoVersao = Convert.ToInt32(_DbCommand.Parameters["@P_ID_PAGINA"].Value);
            }

            return lRetorno;
        }

        public VersaoResponse PublicarVersao(VersaoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            VersaoResponse lRetorno = new VersaoResponse();

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_PUBLICAR_VERSAO"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_NM_IDENTIFICACAO", DbType.String, pRequest.Versao.CodigoDeIdentificacao);
                
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, pRequest.Versao.CodigoPagina);

                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }

            return lRetorno;
        }

        public PaginaResponse BuscarPaginasEVersoes(PaginaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            PaginaResponse lRetorno = new PaginaResponse();

            lRetorno.ListaPagina = new List<PaginaInfo>();

            lRetorno.Pagina = pRequest.Pagina;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SPAGINAS_VERSOES"))
            {
                DataTable lTable = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                int lIdPagina = -1;
                int lIdVersao = -1;
                int lIdEstrutura = -1;

                // Atenção: a ordenação da tabela por ID_PAGINA, ID_VERSAO, ID_ESTRUTURA é fundamental para esse método 
                // destrinchar o join das tabelas nos objetos

                foreach (DataRow lLinha in lTable.Rows)
                {
                    if (lLinha["ID_PAGINA"].DBToInt32() != lIdPagina)
                    {
                        lRetorno.ListaPagina.Add(this.MontaObjetoPagina(lLinha, false));

                        lIdPagina = lLinha["ID_PAGINA"].DBToInt32();
                    }

                    if (lLinha["ID_VERSAO"].DBToInt32() != lIdVersao)
                    {
                        lRetorno.ListaPagina[lRetorno.ListaPagina.Count - 1].Versoes.Add(this.MontaObjetoVersao(lLinha));

                        lIdVersao = lLinha["ID_VERSAO"].DBToInt32();
                    }

                    if (lLinha["ID_ESTRUTURA"].DBToInt32() != lIdEstrutura)
                    {
                        lRetorno.ListaPagina[lRetorno.ListaPagina.Count - 1].Versoes[lRetorno.ListaPagina[lRetorno.ListaPagina.Count - 1].Versoes.Count - 1].ListaEstrutura.Add(this.MontaObjetoEstrutura(lLinha, false));

                        lIdEstrutura = lLinha["ID_ESTRUTURA"].DBToInt32();
                    }
                }
            }

            return lRetorno;
        }

        public PaginaConteudoResponse SelecionarPaginaConteudo(PaginaConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            PaginaConteudoResponse lRetorno = new PaginaConteudoResponse();

            lRetorno.ListaPaginaConteudo = new List<PaginaConteudoInfo>();

            lRetorno.PaginaConteudo = pRequest.PaginaConteudo;

            PaginaConteudoInfo lPaginaInfo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SPAGINA_CONTEUDO_COMPLETA"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_TERMO", DbType.String, pRequest.PaginaConteudo.ConteudoTermo);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    lPaginaInfo = new PaginaConteudoInfo();

                    lPaginaInfo.CodigoPagina    = linha["ID_PAGINA"].DBToInt32()            ;
                    lPaginaInfo.NomePagina      = linha["NM_PAGINA"].DBToString()           ;
                    lPaginaInfo.DescURL         = linha["DS_URL"].DBToString()              ;
                    lPaginaInfo.WidgetJson      = linha["DS_WIDGET_JSON"].DBToString()      ;
                    lPaginaInfo.ConteudoJson    = linha["DS_CONTEUDO_JSON"].DBToString()    ;
                    lPaginaInfo.ConteudoHTML    = linha["DS_CONTEUDO_HTML"].DBToString()    ;
                    lPaginaInfo.ConteudoTermo = pRequest.PaginaConteudo.ConteudoTermo;

                    lRetorno.ListaPaginaConteudo.Add(lPaginaInfo);
                }

                
            }

            return lRetorno;
        }

        public ListaConteudoResponse SelecionarListaConteudo(ListaConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            ListaConteudoResponse lRetorno = new ListaConteudoResponse();

            lRetorno.ListaConteudo = new List<ListaConteudoInfo>();

            lRetorno.Conteudo = pRequest.Conteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SLISTA_CONTEUDO"))
            {
                if (pRequest.Conteudo.CodigoLista > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_LISTA", DbType.Int32, pRequest.Conteudo.CodigoLista);

                if (pRequest.Conteudo.CodigoTipoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoTipoConteudo);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                    lRetorno.ListaConteudo.Add(this.MontaObjetoListaConteudo(linha));
            }

            return lRetorno;
        }

        public BuscarItensDaListaResponse BuscarItensDaLista(BuscarItensDaListaRequest pRequest)
        {
            BuscarItensDaListaResponse lResposta = new BuscarItensDaListaResponse();

            AcessaDados lAcessaDados = new AcessaDados();

            DataTable lTabela;

            DbCommand lCommand;
            try
            {
                string lRegra = "";
                int lIdTipoConteudo = -1;
                string lDescricaoLista = "";

                lAcessaDados.ConnectionStringName = ConexaoPortal;

                lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SLISTA_CONTEUDO");

                lResposta.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                if (pRequest.IdDaLista == -1)
                {
                    lIdTipoConteudo = Convert.ToInt32(pRequest.CodigoMensagem.Substring(0, pRequest.CodigoMensagem.IndexOf(';')));
                    lRegra = pRequest.CodigoMensagem.Substring(pRequest.CodigoMensagem.IndexOf(';') + 1);
                    lDescricaoLista = "Teste";
                }
                else
                {
                    lAcessaDados.AddInParameter(lCommand, "@P_ID_LISTA", DbType.Int32, pRequest.IdDaLista);

                    lTabela = lAcessaDados.ExecuteDbDataTable(lCommand);

                    if (lTabela.Rows.Count > 0)
                    {
                        lRegra          = lTabela.Rows[0]["DS_REGRA"].DBToString();
                        lIdTipoConteudo = lTabela.Rows[0]["ID_TIPO_CONTEUDO"].DBToInt32();
                        lDescricaoLista = lTabela.Rows[0]["DS_LISTA"].DBToString();
                    }
                    else
                    {
                        lResposta.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroNegocio;

                        lResposta.DescricaoResposta = string.Format("Não foi encontrado no banco de dados lista com ID = [{0}]", pRequest.IdDaLista);
                    }
                }

                if (lResposta.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    lCommand = PrepararComandoAPartirDaRegra(ref lAcessaDados, lIdTipoConteudo, lRegra);

                    lTabela = lAcessaDados.ExecuteDbDataTable(lCommand);

                    foreach (DataRow lLinha in lTabela.Rows)
                        lResposta.Itens.Add(this.MontaObjetoConteudo(lLinha));

                    lResposta.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                    lResposta.IdTipoConteudo = lIdTipoConteudo;
                    lResposta.DescricaoDaLista = lDescricaoLista;
                }
            }
            catch (Exception ex)
            {
                lResposta.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;

                lResposta.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }
            
            return lResposta;
        }

        public BuscarItensDaListaResponse BuscarBannersLaterais(BuscarItensDaListaRequest pRequest)
        {
            BuscarItensDaListaResponse lResposta = new BuscarItensDaListaResponse();

            AcessaDados lAcessaDados = new AcessaDados();
            DataTable lTabela;
            DbCommand lCommand;

            try
            {
                lAcessaDados.ConnectionStringName = ConexaoPortal;

                lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SBANNERS_LATERAIS");

                lTabela = lAcessaDados.ExecuteDbDataTable(lCommand);

                foreach (DataRow lLinha in lTabela.Rows)
                    lResposta.Itens.Add(this.MontaObjetoConteudo(lLinha));

                lResposta.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lResposta.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;

                lResposta.DescricaoResposta = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
            }
            
            return lResposta;
        }

        #endregion

        #region Insert/Update

        public PaginaResponse InserirPagina(PaginaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            PaginaResponse lRetorno = new PaginaResponse();

            lRetorno.Pagina = pRequest.Pagina;

            bool lNovoRegistro = !(pRequest.Pagina.CodigoPagina.HasValue && pRequest.Pagina.CodigoPagina.Value > 0);

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_MPAGINA"))
            {
                if (lNovoRegistro)
                {
                    _AcessaDados.AddOutParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, 1);
                }
                else
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, pRequest.Pagina.CodigoPagina);
                }

                if (pRequest.Pagina.NomePagina != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_NM_PAGINA", DbType.String, pRequest.Pagina.NomePagina);

                if (pRequest.Pagina.DescURL != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DS_URL", DbType.String, pRequest.Pagina.DescURL);
                
                if (pRequest.Pagina.TipoEstrutura != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DS_TIPO", DbType.String, pRequest.Pagina.TipoEstrutura);

                if (pRequest.Pagina.Versoes != null)
                {
                    _AcessaDados.AddInParameter(_DbCommand, "@P_VERSAO", DbType.String, pRequest.Pagina.VersaoPublicada.CodigoDeIdentificacao);
                }

                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_MERGEFROM", DbType.Int32, pRequest.MergeFrom);

                try
                {
                    _AcessaDados.ExecuteNonQuery(_DbCommand);

                    lRetorno.Pagina = new PaginaInfo();

                    if (lNovoRegistro)
                    {
                        lRetorno.Pagina.CodigoPagina = Convert.ToInt32(_DbCommand.Parameters["@P_ID_PAGINA"].Value);
                    }
                    else
                    {
                        lRetorno.Pagina.CodigoPagina = pRequest.Pagina.CodigoPagina;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower() == "url_ja_existe")
                    {
                        lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroValidacao;
                        lRetorno.DescricaoResposta = "url_ja_existe";
                    }
                    else
                    {
                        throw ex;
                    }
                }

            }

            return lRetorno;
        }

        public EstruturaResponse InserirEstrutura(EstruturaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            EstruturaResponse lRetorno = new EstruturaResponse();

            lRetorno.Estrutura = pRequest.Estrutura;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_MESTRUTURA"))
            {
                if (pRequest.Estrutura.CodigoEstrutura > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_ESTRUTURA", DbType.Int32, pRequest.Estrutura.CodigoEstrutura);

                /*
                if (pRequest.Estrutura.FlagPublicada != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_FL_PUBLICADA", DbType.Boolean, pRequest.Estrutura.FlagPublicada);
                else
                    _AcessaDados.AddInParameter(_DbCommand, "@P_FL_PUBLICADA", DbType.Boolean, true);
                */

                if (pRequest.Estrutura.NomeAutor != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_NM_AUTOR", DbType.Int32, pRequest.Estrutura.NomeAutor);

                
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA"          , DbType.Int32      , pRequest.Estrutura.CodigoPagina   );
                
                _AcessaDados.AddInParameter(_DbCommand, "@P_TP_USUARIO"         , DbType.Int32      , pRequest.Estrutura.TipoUsuario    );

                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_CRIACAO"         , DbType.DateTime   , DateTime.Now                      );
                             
                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_ESTRUTURA_JSON"  , DbType.String     , pRequest.Estrutura.EstruturaJson  );

               _AcessaDados.ExecuteNonQuery(_DbCommand);

               lRetorno.Estrutura.CodigoEstrutura = _DbCommand.Parameters["@P_ID_RETORNO"].Value.DBToInt32();//retorna o ID inserido na tabela

            }
            return lRetorno;
        }

        public EstruturaResponse InserirEstrutura(EstruturaRequest pRequest, DbTransaction ptransacao, AcessaDados pAcessoDados)
        {
            EstruturaResponse lRetorno = new EstruturaResponse();

            lRetorno.Estrutura = pRequest.Estrutura;

            using (DbCommand _DbCommand = pAcessoDados.CreateCommand(ptransacao, CommandType.StoredProcedure, "SP_MESTRUTURA"))
            {
                if (pRequest.Estrutura.CodigoEstrutura > 0)
                    pAcessoDados.AddInParameter(_DbCommand, "@P_ID_ESTRUTURA", DbType.Int32, pRequest.Estrutura.CodigoEstrutura);

                /*
                if (pRequest.Estrutura.FlagPublicada != null)
                    pAcessoDados.AddInParameter(_DbCommand, "@P_FL_PUBLICADA", DbType.Boolean, pRequest.Estrutura.FlagPublicada);
                else
                    pAcessoDados.AddInParameter(_DbCommand, "@P_FL_PUBLICADA", DbType.Boolean, true);
                */

                if (pRequest.Estrutura.NomeAutor != null)
                    pAcessoDados.AddInParameter(_DbCommand, "@P_NM_AUTOR", DbType.String, pRequest.Estrutura.NomeAutor);


                pAcessoDados.AddInParameter(_DbCommand, "@P_ID_PAGINA"          , DbType.Int32      , pRequest.Estrutura.CodigoPagina   );

                pAcessoDados.AddInParameter(_DbCommand, "@P_TP_USUARIO"         , DbType.Int32      , pRequest.Estrutura.TipoUsuario    );

                pAcessoDados.AddInParameter(_DbCommand, "@P_DT_CRIACAO"         , DbType.DateTime   , DateTime.Now                      );

                pAcessoDados.AddInParameter(_DbCommand, "@P_DS_ESTRUTURA_JSON"  , DbType.String     , pRequest.Estrutura.EstruturaJson  );

                pAcessoDados.AddOutParameter(_DbCommand, "@P_ID_RETORNO"        , DbType.Int32      , 0                                 );

                pAcessoDados.ExecuteNonQuery(_DbCommand);

                lRetorno.Estrutura.CodigoEstrutura = _DbCommand.Parameters["@P_ID_RETORNO"].Value.DBToInt32();//retorna o ID inserido na tabela

            }
            return lRetorno;
        }

        public WidgetResponse InserirWidget(WidgetRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            WidgetResponse lRetorno = new WidgetResponse();

            lRetorno.Widget = pRequest.Widget;

            if (pRequest.Widget.CodigoEstrutura < 1)
                throw new Exception("Codigo da Estrutura é obrigatório.");

            int OrdemPagina = pRequest.Widget.OrdemPagina; 

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_MWIDGET"))
            {
                if (pRequest.Widget.CodigoWidget > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_WIDGET", DbType.Int32, pRequest.Widget.CodigoWidget);
               else
                    OrdemPagina = this.RecuperarProximoOrdemPagina(pRequest.Widget.CodigoEstrutura); //Novo Widget pega a ultima posição mais um

                if (pRequest.Widget.CodigoListaConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_LISTA", DbType.Int32 , pRequest.Widget.CodigoListaConteudo);

                _AcessaDados.AddInParameter(_DbCommand  , "@P_ID_ESTRUTURA"     , DbType.Int32  , pRequest.Widget.CodigoEstrutura   );

                _AcessaDados.AddInParameter(_DbCommand  , "@P_NR_ORDEM_PAGINA"  , DbType.Int32  , OrdemPagina                       );

                _AcessaDados.AddInParameter(_DbCommand  , "@P_DS_WIDGET_JSON"   , DbType.String , pRequest.Widget.WidgetJson        );

                _AcessaDados.AddOutParameter(_DbCommand , "@P_ID_RETORNO"       , DbType.Int32  , 0                                 );

                _AcessaDados.ExecuteNonQuery(_DbCommand);

                lRetorno.Widget.CodigoWidget = _DbCommand.Parameters["@P_ID_RETORNO"].Value.DBToInt32();//retorna o ID inserido na tabela

            }
            return lRetorno;
        }

        public WidgetResponse InserirWidget(WidgetRequest pRequest, DbTransaction ptransacao, AcessaDados pAcessoDados)
        {
            WidgetResponse lRetorno = new WidgetResponse();

            lRetorno.Widget = pRequest.Widget;

            if (pRequest.Widget.CodigoEstrutura < 1)
                throw new Exception("Codigo da Estrutura é obrigatório.");

            using (DbCommand _DbCommand = pAcessoDados.CreateCommand(ptransacao, CommandType.StoredProcedure, "SP_MWIDGET"))
            {
                if (pRequest.Widget.CodigoWidget > 0)
                    pAcessoDados.AddInParameter(_DbCommand, "@P_ID_WIDGET", DbType.Int32, pRequest.Widget.CodigoWidget);

                if (pRequest.Widget.CodigoListaConteudo > 0)
                    pAcessoDados.AddInParameter(_DbCommand, "@P_ID_LISTA", DbType.Int32 , pRequest.Widget.CodigoListaConteudo);

                pAcessoDados.AddInParameter(_DbCommand, "@P_ID_ESTRUTURA"   , DbType.Int32  , pRequest.Widget.CodigoEstrutura   );

                pAcessoDados.AddInParameter(_DbCommand, "@P_NR_ORDEM_PAGINA", DbType.Int32  , pRequest.Widget.OrdemPagina       );

                pAcessoDados.AddInParameter(_DbCommand, "@P_DS_WIDGET_JSON" , DbType.String , pRequest.Widget.WidgetJson        );

                pAcessoDados.AddOutParameter(_DbCommand, "@P_ID_RETORNO"    , DbType.Int32  , 0);

                pAcessoDados.ExecuteNonQuery(_DbCommand);

                lRetorno.Widget.CodigoWidget = _DbCommand.Parameters["@P_ID_RETORNO"].Value.DBToInt32();//retorna o ID inserido na tabela

            }
            return lRetorno;
        }

        public ConteudoResponse InserirConteudo(ConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            ConteudoResponse lRetorno = new ConteudoResponse();

            lRetorno.Conteudo = pRequest.Conteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_MCONTEUDO"))
            {
                if (pRequest.Conteudo.CodigoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoConteudo);

                if (pRequest.Conteudo.CodigoTipoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoTipoConteudo);

                _AcessaDados.AddInParameter(_DbCommand, "@P_DT_CRIACAO"         , DbType.DateTime   , pRequest.Conteudo.DtCriacao           );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE01"   , DbType.String     , pRequest.Conteudo.ValorPropriedade1   );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE02"   , DbType.String     , pRequest.Conteudo.ValorPropriedade2   );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE03"   , DbType.String     , pRequest.Conteudo.ValorPropriedade3   );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE04"   , DbType.String     , pRequest.Conteudo.ValorPropriedade4   );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_PROPRIEDADE05"   , DbType.String     , pRequest.Conteudo.ValorPropriedade5   );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_CONTEUDO_JSON"   , DbType.String     , pRequest.Conteudo.ConteudoJson        );
                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_CONTEUDO_HTML"   , DbType.String     , pRequest.Conteudo.ConteudoHtml        );
                _AcessaDados.AddInParameter(_DbCommand, "@P_VL_TAG"             , DbType.String     , pRequest.Conteudo.ValorTag            );
                _AcessaDados.AddOutParameter(_DbCommand, "@P_ID_RETORNO"        , DbType.Int32      , 0                                     );

                if (pRequest.Conteudo.DtInicio != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_INICIO" , DbType.DateTime , pRequest.Conteudo.DtInicio);

                if (pRequest.Conteudo.DtFim != null)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_DT_FIM"  , DbType.DateTime  , pRequest.Conteudo.DtFim);
                      
                _AcessaDados.ExecuteNonQuery(_DbCommand);

                lRetorno.Conteudo.CodigoConteudo = _DbCommand.Parameters["@P_ID_RETORNO"].Value.DBToInt32();//retorna o ID inserido na tabela

                
            }
            return lRetorno;
        }

        public ListaConteudoResponse InserirListaConteudo(ListaConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            ListaConteudoResponse lRetorno = new ListaConteudoResponse();

            lRetorno.Conteudo = pRequest.Conteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_MLISTA_CONTEUDO"))
            {
                if (pRequest.Conteudo.CodigoLista > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_LISTA", DbType.Int32, pRequest.Conteudo.CodigoLista);

                if (pRequest.Conteudo.CodigoTipoConteudo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoTipoConteudo    );

                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_REGRA"   , DbType.String, pRequest.Conteudo.Regra            );

                _AcessaDados.AddInParameter(_DbCommand, "@P_DS_LISTA"   , DbType.String, pRequest.Conteudo.DescricaoLista   );

                _AcessaDados.AddOutParameter(_DbCommand, "@P_ID_RETORNO", DbType.Int32  , 0                                 );
                

                _AcessaDados.ExecuteNonQuery(_DbCommand);

                lRetorno.Conteudo.CodigoLista = _DbCommand.Parameters["@P_ID_RETORNO"].Value.DBToInt32();//retorna o ID inserido na tabela


            }
            return lRetorno;
        }

        public AtualizarOrdemDoWidgetNaPaginaResponse AtualizarOrdemDoWidgetNaPagina(AtualizarOrdemDoWidgetNaPaginaRequest pRequest)
        {
            AcessaDados lDados = new AcessaDados();

            lDados.ConnectionStringName = ConexaoPortal;

            lDados.Conexao._ConnectionStringName = ConexaoPortal;

            DbTransaction lTrans;

            DbConnection lConnection;

            lConnection = lDados.Conexao.CreateIConnection();

            lConnection.Open();

            lTrans = lConnection.BeginTransaction();

            AtualizarOrdemDoWidgetNaPaginaResponse lRetorno = new AtualizarOrdemDoWidgetNaPaginaResponse();

            try
            {
                int lOrdemNaPagina = 1;

                foreach (int lIDdoWidget in pRequest.OrdemDeWidgets)
                {
                    using (DbCommand lCommand = lDados.CreateCommand(lTrans, CommandType.StoredProcedure, "SP_UORDEM_PAGINA_WIDGET"))
                    {
                        lDados.AddInParameter(lCommand, "@P_ID_WIDGET"   , DbType.Int32  , lIDdoWidget);

                        lDados.AddInParameter(lCommand, "@P_NR_ORDEM_PAGINA", DbType.Int32  , lOrdemNaPagina);

                        lDados.ExecuteNonQuery(lCommand);

                        lOrdemNaPagina++;
                    }
                }

                lTrans.Commit();
            }
            catch (Exception ex)
            {
                lTrans.Rollback();
                throw ex;
            }
            finally
            {
                lConnection.Close();
                lConnection.Dispose();

                lConnection = null;

                lTrans = null;
            }

            
            return lRetorno;
        }

        #endregion

        #region Delete

        public WidgetResponse ApagarWidget(WidgetRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            _AcessaDados.Conexao._ConnectionStringName = ConexaoPortal;

            DbTransaction transacao;

            DbConnection ConnectionSql;

            ConnectionSql = _AcessaDados.Conexao.CreateIConnection();

            ConnectionSql.Open();

            transacao = ConnectionSql.BeginTransaction();

            WidgetResponse lRetorno = new WidgetResponse();

            try
            {
                if (pRequest.Widget.CodigoWidget < 1)
                    throw new Exception("O código do Widget é obrigatório.");

                WidgetRequest WidgetSelecionado = new WidgetRequest();
                WidgetSelecionado.Widget = new WidgetInfo();
                WidgetSelecionado.Widget.CodigoWidget = pRequest.Widget.CodigoWidget;

                WidgetResponse resposta = SelecionarWdiget(WidgetSelecionado);

                if (resposta.ListaWidget.Count > 0)
                {
                    WidgetInfo WidgetExcluir = resposta.ListaWidget[0];

                    WidgetRequest WidgetGeral = new WidgetRequest();

                    WidgetGeral.Widget = new WidgetInfo();
                    WidgetGeral.Widget.CodigoEstrutura = WidgetExcluir.CodigoEstrutura;

                    WidgetResponse widEstrutura = this.SelecionarWdiget(WidgetGeral);

                    //arruma a ordem dos widget
                    foreach (WidgetInfo item in widEstrutura.ListaWidget)
                    {
                        if (item.OrdemPagina > WidgetExcluir.OrdemPagina)
                        {
                            this.AtulizarOrdemPaginaWidget(item.CodigoWidget, (item.OrdemPagina - 1), transacao, _AcessaDados);
                        }
                    }

                    //apaga o widget
                    this.ApagarWidget(WidgetExcluir.CodigoWidget, transacao, _AcessaDados);

                    transacao.Commit();
                }
                else
                {
                    //widget não existia antes, vamos dizer que excluiu...

                    transacao.Rollback();
                }
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw ex;
            }
            finally
            {
                ConnectionSql.Close();
                ConnectionSql.Dispose();
                ConnectionSql = null;
                transacao = null;
            }

            return lRetorno;
        }

        public ConteudoResponse ApagarConteudo(ConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            ConteudoResponse lRetorno = new ConteudoResponse();

            lRetorno.Conteudo = pRequest.Conteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_DCONTEUDO"))
            {
               _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CONTEUDO", DbType.Int32, pRequest.Conteudo.CodigoConteudo);

               _AcessaDados.ExecuteNonQuery(_DbCommand);
            }

            return lRetorno;
        }

        public TipoConteudoResponse ApagarTipoConteudo(TipoConteudoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            TipoConteudoResponse lRetorno = new TipoConteudoResponse();

            lRetorno.TipoConteudo = pRequest.TipoConteudo;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_DTIPO_CONTEUDO"))
            {

                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pRequest.TipoConteudo.IdTipoConteudo);

                _AcessaDados.ExecuteNonQuery(_DbCommand);


            }


            return lRetorno;
        }

        public ListaConteudoResponse ApagarListaConteudo(ListaConteudoRequest pRequest)
        {
            AcessaDados lDados = new AcessaDados();

            lDados.ConnectionStringName = ConexaoPortal;

            ListaConteudoResponse lRetorno = new ListaConteudoResponse();

            DbCommand lCommand;

            DataTable lTable;
            
            lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "SP_DLISTA_WIDGETS");

            lDados.AddInParameter(lCommand, "@P_ID_LISTA", DbType.Int32, pRequest.Conteudo.CodigoLista);

            lTable = lDados.ExecuteDbDataTable(lCommand);

            if (lTable.Rows.Count > 0)
            {
                //existem widgets que se referenciam à tabela.

                string lRetornoExtendido = "Existem páginas que têm widgets que utilizam essa lista, favor editar os widgets primeiro:\r\n\r\n";

                foreach (DataRow lRow in lTable.Rows)
                {
                    lRetornoExtendido += string.Format("{0} - {1}\r\n", lRow["NM_PAGINA"], lRow["DS_URL"]);
                }

                lRetorno.DescricaoResposta = lRetornoExtendido;
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroValidacao;
            }
            else
            {
                lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "SP_DLISTA_CONTEUDO");

                lDados.AddInParameter(lCommand, "@P_ID_LISTA", DbType.Int32, pRequest.Conteudo.CodigoLista);

                lDados.ExecuteNonQuery(lCommand);

                lRetorno.Conteudo = pRequest.Conteudo;
            
            }


            return lRetorno;
        }

        public PaginaResponse ExcluirPagina(PaginaRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            _AcessaDados.ConnectionStringName = ConexaoPortal;

            PaginaResponse lRetorno = new PaginaResponse();

            lRetorno.Pagina = pRequest.Pagina;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_DPAGINA"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_PAGINA", DbType.Int32, pRequest.Pagina.CodigoPagina);

                _AcessaDados.ExecuteNonQuery(_DbCommand);
            }

            return lRetorno;
        }

        public EstruturaResponse CopiarEstrutra(EstruturaRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ConexaoPortal;

            EstruturaResponse lRetorno = new EstruturaResponse();

            try
            {
                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_DUPLICAR_ESTRUTURA"))
                {
                    lAcessaDados.AddInParameter(lCommand,  "@P_ID_ESTRUTURA",    DbType.Int32, pRequest.Estrutura.CodigoEstrutura);
                    lAcessaDados.AddInParameter(lCommand,  "@P_ID_USUARIO_NOVO", DbType.Int32, pRequest.Estrutura.TipoUsuario);
                    lAcessaDados.AddOutParameter(lCommand, "@P_ID_RETORNO",      DbType.Int32, 1);

                    lAcessaDados.ExecuteNonQuery(lCommand);

                    lRetorno.Estrutura = new EstruturaInfo();

                    lRetorno.Estrutura.CodigoEstrutura = Convert.ToInt32(lCommand.Parameters["@P_ID_ESTRUTURA"].Value);

                    lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.OK;

                    lRetorno.DataResposta = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = OMS.Library.MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lRetorno;
        }

        #endregion

        #region Metodos Internos

        private ListaConteudoInfo MontaObjetoListaConteudo(DataRow linha)
        {
            ListaConteudoInfo lRetorno = new ListaConteudoInfo();

            //TipoConteudoRequest lRequest = new TipoConteudoRequest();
            //TipoConteudoResponse lResponse;

            lRetorno.CodigoLista         = linha["ID_LISTA"].DBToInt32();
            lRetorno.CodigoTipoConteudo  = linha["ID_TIPO_CONTEUDO"].DBToInt32();
            lRetorno.Regra               = linha["DS_REGRA"].ToString();
            lRetorno.DescricaoLista      = linha["DS_LISTA"].ToString();

            /*
            lRequest.TipoConteudo = new TipoDeConteudoInfo();

            lRequest.TipoConteudo.IdTipoConteudo = lRetorno.CodigoTipoConteudo;

            lResponse = this.SelecionarTipoConteudo(lRequest);

            if (lResponse.ListaTipoConteudo.Count > 0)
            {
                lRetorno.ObjetoTipoConteudo = lResponse.ListaTipoConteudo[0];
            }
            else
            {
                lRetorno.ObjetoTipoConteudo = new TipoDeConteudoInfo();
            }
            */

            return lRetorno;
        }

        private ConteudoInfo MontaObjetoConteudo(DataRow pLinha)
        {
            ConteudoInfo lRetorno = new ConteudoInfo();

            lRetorno.CodigoConteudo     = pLinha["ID_CONTEUDO"].DBToInt32();
            lRetorno.CodigoTipoConteudo = pLinha["ID_TIPO_CONTEUDO"].DBToInt32();
            lRetorno.DtCriacao          = pLinha["DT_CRIACAO"].DBToDateTime();
            lRetorno.ValorPropriedade1  = pLinha["VL_PROPRIEDADE01"].ToString();
            lRetorno.ValorPropriedade2  = pLinha["VL_PROPRIEDADE02"].ToString();
            lRetorno.ValorPropriedade3  = pLinha["VL_PROPRIEDADE03"].ToString();
            lRetorno.ValorPropriedade4  = pLinha["VL_PROPRIEDADE04"].ToString();
            lRetorno.ValorPropriedade5  = pLinha["VL_PROPRIEDADE05"].ToString();
            lRetorno.ConteudoJson       = pLinha["DS_CONTEUDO_JSON"].ToString();
            lRetorno.ConteudoHtml       = pLinha["DS_CONTEUDO_HTML"].ToString();
            lRetorno.DtInicio           = pLinha["DT_INICIO"].DBToDateTime();
            lRetorno.DtFim              = pLinha["DT_Fim"].DBToDateTime();
            lRetorno.ValorTag           = pLinha["VL_TAG"].DBToString();

            return lRetorno;
        }

        private TipoDeConteudoInfo MontaObjetoTipoConteudo(DataRow pLinha)
        {
            TipoDeConteudoInfo lRetorno = new TipoDeConteudoInfo();

            lRetorno.IdTipoConteudo     = pLinha["ID_TIPO_CONTEUDO"].DBToInt32();
            lRetorno.Descricao          = pLinha["DS_CONTEUDO"].ToString();
            lRetorno.NomePropriedade1   = pLinha["NM_PROPRIEDADE01"].ToString();
            lRetorno.NomePropriedade2   = pLinha["NM_PROPRIEDADE02"].ToString();
            lRetorno.NomePropriedade3   = pLinha["NM_PROPRIEDADE03"].ToString();
            lRetorno.NomePropriedade4   = pLinha["NM_PROPRIEDADE04"].ToString();
            lRetorno.NomePropriedade5   = pLinha["NM_PROPRIEDADE05"].ToString();
            lRetorno.TipoDeConteudoJson = pLinha["DS_TIPO_CONTEUDO_JSON"].ToString();

            return lRetorno;
        }

        private WidgetInfo MontaObjetoWidget(DataRow pLinha)
        {
            WidgetInfo lRetorno = new WidgetInfo();

            lRetorno.CodigoWidget           = pLinha["ID_WIDGET"].DBToInt32();
            lRetorno.CodigoEstrutura        = pLinha["ID_ESTRUTURA"].DBToInt32();
            lRetorno.WidgetJson             = pLinha["DS_WIDGET_JSON"].ToString();
            lRetorno.CodigoListaConteudo    = pLinha["ID_LISTA"].DBToInt32();
            lRetorno.OrdemPagina            = pLinha["NR_ORDEM_PAGINA"].DBToInt32();

            /*
            ListaConteudoRequest  lRequest = new ListaConteudoRequest();
            ListaConteudoResponse lResponse;

            lRequest.Conteudo = new ListaConteudoInfo();
            lRequest.Conteudo.CodigoLista = lRetorno.CodigoListaConteudo;

            lResponse = this.SelecionarListaConteudo(lRequest);

            if (lResponse.ListaConteudo.Count > 0)
            {
                lRetorno.ObjetoListaConteudo = lResponse.ListaConteudo[0];
            }
            else
            {
                lRetorno.ObjetoListaConteudo = new ListaConteudoInfo();
            }
            */

            return lRetorno;
        }

        private EstruturaInfo MontaObjetoEstrutura(DataRow pLinha, bool pPreencherWidgets = true)
        {
            EstruturaInfo lRetorno = new EstruturaInfo();

            lRetorno.CodigoEstrutura = pLinha["ID_ESTRUTURA"].DBToInt32();
            lRetorno.TipoUsuario     = pLinha["TP_USUARIO"].DBToInt32();
            
            try
            {
                lRetorno.CodigoPagina = pLinha["ID_PAGINA"].DBToInt32();
            }
            catch { }

            try
            {
                lRetorno.NomeAutor = pLinha["NM_AUTOR"].DBToString();
                lRetorno.DtCriacao = pLinha["DT_CRIACAO"].DBToDateTime();
            }
            catch { }
            
            try
            {
                lRetorno.IdentificadorVersao = pLinha["NM_IDENTIFICACAO"].DBToString();
            }
            catch { }

            lRetorno.ListaWidget = new List<WidgetInfo>();

            if (pPreencherWidgets)
            {
                WidgetRequest lRequest = new WidgetRequest();
                WidgetResponse lResponse;

                lRequest.Widget = new WidgetInfo();
                lRequest.Widget.CodigoEstrutura = lRetorno.CodigoEstrutura;

                lResponse = this.SelecionarWdiget(lRequest);

                lRetorno.ListaWidget = lResponse.ListaWidget;
            }

            return lRetorno;
        }
        
        private VersaoInfo MontaObjetoVersao(DataRow pLinha)
        {
            VersaoInfo lRetorno = new VersaoInfo();

            lRetorno.CodigoVersao           = pLinha["ID_VERSAO"].DBToInt32();
            lRetorno.CodigoPagina           = pLinha["ID_PAGINA"].DBToInt32();
            lRetorno.CodigoDeIdentificacao  = pLinha["NM_IDENTIFICACAO"].DBToString();
            lRetorno.Publicada              = pLinha["FL_PUBLICADA"].DBToBoolean();

            lRetorno.ListaEstrutura = new List<EstruturaInfo>();

            try
            {
                lRetorno.DataCriacao = pLinha["DT_CRIACAO"].DBToDateTime();
            }
            catch { }

            return lRetorno;
        }

        private PaginaInfo MontaObjetoPagina(DataRow pLinha, bool pPreencherEstrutura = true)
        {
            PaginaInfo lRetorno = new PaginaInfo();
            
            lRetorno.CodigoPagina  = pLinha["ID_PAGINA"].DBToInt32();
            lRetorno.NomePagina    = pLinha["NM_PAGINA"].DBToString();
            lRetorno.DescURL       = pLinha["DS_URL"].DBToString();

            lRetorno.Versoes = new List<VersaoInfo>();

            try
            {
                lRetorno.TipoEstrutura = pLinha["TP_ESTRUTURA"].DBToString();
            }
            catch { }

            if (pPreencherEstrutura)
            {
                /*
                EstruturaRequest lRequest = new EstruturaRequest();
                EstruturaResponse lResponse;
                
                lRetorno.ListaEstrutura = new List<EstruturaInfo>();

                lRequest.Estrutura = new EstruturaInfo();
                lRequest.Estrutura.CodigoPagina = lRetorno.CodigoPagina.Value;

                lResponse = this.SelecionarEstrutura(lRequest);

                lRetorno.ListaEstrutura = lResponse.ListaEstrutura;
                */
            }

            return lRetorno;
        }

        private void AtulizarOrdemPaginaWidget(int pIDWidget, int pNumeroOrdemPagina, DbTransaction pTransacao, AcessaDados pAcessoDados)
        {
            using (DbCommand lCommand = pAcessoDados.CreateCommand(pTransacao, CommandType.StoredProcedure, "SP_UORDEM_PAGINA_WIDGET"))
            {
                pAcessoDados.AddInParameter(lCommand, "@P_ID_WIDGET"      , DbType.Int32, pIDWidget);
                pAcessoDados.AddInParameter(lCommand, "@P_NR_ORDEM_PAGINA", DbType.Int32, pNumeroOrdemPagina);

                pAcessoDados.ExecuteNonQuery(lCommand);
            }
        }

        private void ApagarWidget(int pIDWidget, DbTransaction pTransacao, AcessaDados pAcessoDados)
        {
            using (DbCommand lCommand = pAcessoDados.CreateCommand(pTransacao, CommandType.StoredProcedure, "SP_DWIDGET"))
            {
                pAcessoDados.AddInParameter(lCommand, "@P_ID_WIDGET", DbType.Int32, pIDWidget);
                                
                pAcessoDados.ExecuteNonQuery(lCommand);
            }
        }

        private void ApagarTodosWidgetEstrutura(EstruturaInfo pEstruturaInfo, DbTransaction pTransacao, AcessaDados pAcessoDados)
        {
            foreach (WidgetInfo lItem in pEstruturaInfo.ListaWidget)
            {
                using (DbCommand lCommand = pAcessoDados.CreateCommand(pTransacao, CommandType.StoredProcedure, "SP_DWIDGET"))
                {
                    pAcessoDados.AddInParameter(lCommand, "@P_ID_WIDGET", DbType.Int32, lItem.CodigoWidget);

                    pAcessoDados.ExecuteNonQuery(lCommand);
                }
            }
        }

        private string PegarValorDeParametro(string pParametro)
        {
            string lRetorno = pParametro.Trim();

            lRetorno = lRetorno.Substring(lRetorno.IndexOf('[') + 1);
            lRetorno = lRetorno.TrimEnd(']');

            return lRetorno;
        }

        private DateTime PegarDataDeParametro(string pParametro)
        {
            DateTime lRetorno;

            CultureInfo lInfo = new CultureInfo("pt-BR");

            if (!DateTime.TryParseExact(pParametro, "dd/MM/yyyy", lInfo, DateTimeStyles.None, out lRetorno))
            {
                if (!DateTime.TryParseExact(pParametro, "dd/MM/yyyy HH:mm", lInfo, DateTimeStyles.None, out lRetorno))
                {
                    if (!DateTime.TryParseExact(pParametro, "dd/MM/yyyy HH:mm:ss", lInfo, DateTimeStyles.None, out lRetorno))
                    {
                        if (!DateTime.TryParseExact(pParametro, "d/M/yyyy", lInfo, DateTimeStyles.None, out lRetorno))
                        {
                            if (!DateTime.TryParseExact(pParametro, "d/M/yy", lInfo, DateTimeStyles.None, out lRetorno))
                            {
                                throw new Exception(string.Format("Formato de data [{0}] inválido para parâmetro.", pParametro));
                            }
                        }
                    }
                }
            }

            return lRetorno;
        }

        private DbCommand PrepararComandoAPartirDaRegra(ref AcessaDados pAcessaDados, int pIdTipoConteudo, string pRegra)
        {
            DbCommand lComando = null;

            string lFuncao;
            string[] lParametros;

            lFuncao = pRegra.Substring(0, pRegra.IndexOf('(')).ToLower();

            switch (lFuncao)
            {
                case "todos" :

                    //seleciona todos os conteúdos da lista

                    lComando = pAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SCONTEUDO");

                    pAcessaDados.AddInParameter(lComando, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pIdTipoConteudo);

                    break;

                case "valorentredatainicialfinal":

                    //seleciona os conteúdos cujo um determinado valor PARAMETRO_1 esteja entre duas propriedades PARAMETRO_2 e PARAMETRO_3
                    //ValorEntreDataInicialFinal($[DataHoje])
                    
                    lParametros = pRegra.Substring(pRegra.IndexOf('('), pRegra.IndexOf(')') - pRegra.IndexOf('(')).Split(',');

                    if (lParametros.Length > 0)
                    {
                        lParametros[0] = PegarValorDeParametro(lParametros[0]);

                        lComando = pAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SCONTEUDO_ENTRE_DATAS");

                        pAcessaDados.AddInParameter(lComando, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pIdTipoConteudo);

                        DateTime lData;

                        if (lParametros[0].ToLower() == "datahoje")
                        {
                            lData = DateTime.Now;
                        }
                        else
                        {
                            lData = PegarDataDeParametro(lParametros[0]);
                        }

                        pAcessaDados.AddInParameter(lComando, "@P_DATA", DbType.DateTime, lData);
                    }

                    //if(!DateTime.TryParseExact(

                    break;

                case "propriedadeigual" :

                    //seleciona os conteúdos cuja propriedade esteja dentro da comparação com algum valor específico
                    //PropriedadeIgualValor($[FlagPublicado], $[true])

                    lComando = pAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SCONTEUDO_POR_PROPRIEDADE");

                    pAcessaDados.AddInParameter(lComando, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pIdTipoConteudo);

                    lParametros = pRegra.Substring(pRegra.IndexOf('('), pRegra.IndexOf(')') - pRegra.IndexOf('(')).Split(',');

                    if (lParametros.Length == 2)
                    {

                        lParametros[0] = PegarValorDeParametro(lParametros[0]).ToLower();
                        lParametros[1] = PegarValorDeParametro(lParametros[1]);

                        DateTime lData;

                        switch (lParametros[0])
                        {
                            case "datacadastro":

                                lData = PegarDataDeParametro(lParametros[1]);

                                pAcessaDados.AddInParameter(lComando, "@P_DT_CRIACAO", DbType.DateTime, lData);

                                break;

                            case "datainicial":

                                lData = PegarDataDeParametro(lParametros[1]);

                                pAcessaDados.AddInParameter(lComando, "@P_DT_INICIO", DbType.DateTime, lData);

                                break;

                            case "datafinal":

                                lData = PegarDataDeParametro(lParametros[1]);

                                pAcessaDados.AddInParameter(lComando, "@P_DT_FIM", DbType.DateTime, lParametros[1]);

                                break;

                            case "tag":

                                pAcessaDados.AddInParameter(lComando, "@P_VL_TAG", DbType.String, lParametros[1]);

                                break;

                            case "flagpublicado":

                                //por padrão, a propriedade "FlagPublicado" tem que estar na propriedade 5:

                                pAcessaDados.AddInParameter(lComando, "@P_VL_PROPRIEDADE05", DbType.String, lParametros[1]);

                                break;

                            case "codigoconteudo":

                                //por padrão, a propriedade "FlagPublicado" tem que estar na propriedade 5:

                                pAcessaDados.AddInParameter(lComando, "@P_CODIGO_CONTEUDO", DbType.Int32, lParametros[1]);

                                break;

                            default:

                                //TODO: Aqui tem que dar throw numa exception específica, e se 

                                break;
                        }

                    }
                    else
                    {
                        throw new Exception(string.Format("Número inválido de parâmetros para 'PropriedadeIgualValor': [{0}] Esperado: [2]", lParametros.Length));
                    }

                    break;


                case "propriedadediferente" :

                    //seleciona os conteúdos cuja propriedade esteja dentro da comparação com algum valor específico
                    //PropriedadeIgualValor($[FlagPublicado], $[true])

                    lComando = pAcessaDados.CreateCommand(CommandType.StoredProcedure, "SP_SCONTEUDO_POR_PROPRIEDADE_DIFERENTE");

                    pAcessaDados.AddInParameter(lComando, "@P_ID_TIPO_CONTEUDO", DbType.Int32, pIdTipoConteudo);

                    lParametros = pRegra.Substring(pRegra.IndexOf('('), pRegra.IndexOf(')') - pRegra.IndexOf('(')).Split(',');

                    if (lParametros.Length == 2)
                    {

                        lParametros[0] = PegarValorDeParametro(lParametros[0]).ToLower();
                        lParametros[1] = PegarValorDeParametro(lParametros[1]);

                        DateTime lData;

                        switch (lParametros[0])
                        {
                            case "datacadastro":

                                lData = PegarDataDeParametro(lParametros[1]);

                                pAcessaDados.AddInParameter(lComando, "@P_DT_CRIACAO", DbType.DateTime, lData);

                                break;

                            case "datainicial":

                                lData = PegarDataDeParametro(lParametros[1]);

                                pAcessaDados.AddInParameter(lComando, "@P_DT_INICIO", DbType.DateTime, lData);

                                break;

                            case "datafinal":

                                lData = PegarDataDeParametro(lParametros[1]);

                                pAcessaDados.AddInParameter(lComando, "@P_DT_FIM", DbType.Int32, lParametros[1]);

                                break;

                            case "tag":

                                pAcessaDados.AddInParameter(lComando, "@P_VL_TAG", DbType.String, lParametros[1]);

                                break;

                            case "flagpublicado":

                                //por padrão, a propriedade "FlagPublicado" tem que estar na propriedade 5:

                                pAcessaDados.AddInParameter(lComando, "@P_VL_PROPRIEDADE05", DbType.String, lParametros[1]);

                                break;

                            default:

                                //TODO: Aqui tem que dar throw numa exception específica, e se 

                                break;
                        }

                    }
                    else
                    {
                        throw new Exception(string.Format("Número inválido de parâmetros para 'PropriedadeDifereDeValor': [{0}] Esperado: [2]", lParametros.Length));
                    }

                    break;


                default:
                    break;
            }

            return lComando;
        }

        private int RecuperarProximoOrdemPagina(int IDEstrutura)
        {
            int lRetorno = 0;

            WidgetRequest lRequest = new WidgetRequest();
            WidgetResponse lResponse;

            lRequest.Widget = new WidgetInfo();
            lRequest.Widget.CodigoEstrutura = IDEstrutura;

            lResponse = this.SelecionarWdiget(lRequest);

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                if (lResponse.ListaWidget.Count > 0)
                    lRetorno = lResponse.ListaWidget[lResponse.ListaWidget.Count - 1].OrdemPagina;
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }

            return (lRetorno + 1);
        }

        #endregion
    }
}
