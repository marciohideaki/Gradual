using System;
using System.Collections.Generic;
using Gradual.OMS.Library;
using Gradual.OMS.Risco.Regra.Lib;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.OMS.Risco.Regra.Lib.Mensagens;
using Newtonsoft.Json;
using System.Data.Common;

namespace Gradual.Intranet.Servicos.Mock
{
    public class ServicoRegrasRisco : IServicoRegrasRisco
    {
        #region IServicoRegrasRisco Members

        public ListarGruposResponse ListarGrupos(ListarGruposRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ListarParametrosRiscoResponse ListarParametrosRisco(ListarParametrosRiscoRequest lRequest)
        {
            ListarParametrosRiscoResponse lResponse;

            lResponse = JsonConvert.DeserializeObject<ListarParametrosRiscoResponse>("{\"ParametrosRisco\":[{\"NomeParametro\":\"Parametro - Limite descoberto no mercado a vista\",\"CodigoParametro\":5,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimiteDescobertoAVista\"},{\"NomeParametro\":\"Parametro - Limite descoberto no mercado de opcoes\",\"CodigoParametro\":7,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimietDescobertoOpcoes\"},{\"NomeParametro\":\"Parametro - Limite em Conta Corrente\",\"CodigoParametro\":9,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimiteContaMargem\"},{\"NomeParametro\":\"Parametro - Limite em Conta Margem\",\"CodigoParametro\":11,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimiteContaMargem\"},{\"NomeParametro\":\"Parametro - Limite em Custodia\",\"CodigoParametro\":10,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimiteCustodia\"},{\"NomeParametro\":\"Parametro - Limite maximo por boleta\",\"CodigoParametro\":8,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimiteMaximoBoleta\"},{\"NomeParametro\":\"Parametro - Limite para compra no mercado a vista\",\"CodigoParametro\":4,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimiteCompraAVista\"},{\"NomeParametro\":\"Parametro - Limite para compra no mercado de opcoes\",\"CodigoParametro\":6,\"Bolsa\":1,\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaLimiteCompraOpcoes\"}],\"CodigoMensagemRequest\":\"2b677a8d-63d6-45d0-9188-9951c091032c\",\"StatusResposta\":0,\"DescricaoResposta\":null,\"Criticas\":[],\"DataResposta\":\"\\/Date(1281113505007-0300)\\/\",\"ResponseTag\":null,\"CodigoMensagem\":\"99da5e8f-5ea1-429a-91d6-bf7dd3433b9e\",\"CodigoSessao\":null,\"DataReferencia\":\"\\/Date(-62135589600000-0200)\\/\"}");

            return lResponse;
        }

        public ListarParametrosRiscoClienteResponse ListarParametrosRiscoCliente(ListarParametrosRiscoClienteRequest lRequest)
        {
            ListarParametrosRiscoClienteResponse lResponse = new ListarParametrosRiscoClienteResponse();

            lResponse.ParametrosRiscoCliente = new List<ParametroRiscoClienteInfo>();

            lResponse.ParametrosRiscoCliente.Add(JsonConvert.DeserializeObject<ParametroRiscoClienteInfo>("{\"CodigoParametroCliente\":15,\"CodigoCliente\":33755,\"Parametro\":{\"NomeParametro\":\"Parametro - Limite descoberto no mercado a vista\",\"CodigoParametro\":5,\"Bolsa\":1,\"NameSpace\":null,\"Metodo\":null},\"Valor\":100.0,\"DataValidade\":\"\\/Date(1286679600000-0300)\\/\",\"ParametroRiscoClienteValores\":[{\"m_ParametroRiscoClienteInfo\":null,\"CodigoParametroClienteValor\":52,\"ParametroCliente\":null,\"ValorAlocado\":0.0000,\"ValorDisponivel\":100.0000,\"Descricao\":\"Parametro - Limite descoberto no mercado a vista\",\"DataMovimento\":\"\\/Date(1281357933490-0300)\\/\"}],\"Grupo\":null}"));

            lResponse.StatusResposta = MensagemResponseStatusEnum.OK;

            return lResponse;
        }

        public ListarPermissoesRiscoResponse ListarPermissoesRisco(ListarPermissoesRiscoRequest lRequest)
        {
            ListarPermissoesRiscoResponse lResponse;

            lResponse = JsonConvert.DeserializeObject<ListarPermissoesRiscoResponse>("{\"Permissoes\":[{\"Bolsa\":1,\"CodigoPermissao\":25,\"NomePermissao\":\"Permissao  - Utilizar Limite para operar no mercado a vista\",\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaPemissaoUtilizarLimiteAVista\"},{\"Bolsa\":1,\"CodigoPermissao\":26,\"NomePermissao\":\"Permissao - Efetuar venda de acoes a descoberto\",\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaPermissaoEfetuarVendaDescoberto\"},{\"Bolsa\":1,\"CodigoPermissao\":22,\"NomePermissao\":\"Permissao - Operar no mercado a vista\",\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaPermissaoMercadoAVista\"},{\"Bolsa\":1,\"CodigoPermissao\":29,\"NomePermissao\":\"Permissao - Operar no mercado de opções\",\"NameSpace\":\"\",\"Metodo\":\"\"},{\"Bolsa\":1,\"CodigoPermissao\":23,\"NomePermissao\":\"Permissao - Operar o instrumento\",\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaPermissaoOperarInstrumento\"},{\"Bolsa\":1,\"CodigoPermissao\":28,\"NomePermissao\":\"Permissao - Permissao para venda descoberta no mercado a vista\",\"NameSpace\":\"\",\"Metodo\":\"\"},{\"Bolsa\":1,\"CodigoPermissao\":24,\"NomePermissao\":\"Permissao - Utilizar Conta Margem\",\"NameSpace\":\"Gradual.OMS.Sistemas.Risco.ServicoRisco\",\"Metodo\":\"ValidaPermissaoContaMargem\"},{\"Bolsa\":1,\"CodigoPermissao\":30,\"NomePermissao\":\"Permissao - Utilizar Limite para operar no mercado de opções\",\"NameSpace\":\"\",\"Metodo\":\"\"}],\"CodigoMensagemRequest\":\"9fbc8329-d154-430e-a8df-f9cbd579bded\",\"StatusResposta\":0,\"DescricaoResposta\":null,\"Criticas\":[],\"DataResposta\":\"\\/Date(1281104373991-0300)\\/\",\"ResponseTag\":null,\"CodigoMensagem\":\"f81c8cd0-1036-486b-bf17-c24b3a743b3f\",\"CodigoSessao\":null,\"DataReferencia\":\"\\/Date(-62135589600000-0200)\\/\"}");

            return lResponse;
        }

        public ReceberGrupoResponse ReceberGrupo(ReceberGrupoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ReceberParametroRiscoResponse ReceberParametroRisco(ReceberParametroRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ReceberParametroRiscoClienteResponse ReceberParametroRiscoCliente(ReceberParametroRiscoClienteRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ReceberPermissaoRiscoResponse ReceberPermissaoRisco(ReceberPermissaoRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverGrupoItemResponse RemoverGrupoItem(RemoverGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarGrupoResponse SalvarGrupo(SalvarGrupoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarGrupoItemResponse SalvarGrupoItem(SalvarGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarParametroRiscoClienteResponse SalvarParametroRiscoCliente(SalvarParametroRiscoClienteRequest lRequest)
        {
            SalvarParametroRiscoClienteResponse lResponse = new SalvarParametroRiscoClienteResponse();

            lResponse.DescricaoResposta = "42";

            lResponse.StatusResposta = MensagemResponseStatusEnum.OK;

            return lResponse;
        }

        #endregion

        #region IServicoRegrasRisco Members

        public SalvarParametroRiscoResponse SalvarParametroRisco(SalvarParametroRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarPermissaoRiscoResponse SalvarPermissaoRisco(SalvarPermissaoRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members

        public ListarClientePermissaoParametroResponse ListarAssociacao(ListarClientePermissaoParametroRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ListarBolsaResponse ListarBolsasRisco(ListarBolsaRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ReceberClientePermissaoParametroResponse ReceberAssociacao(ReceberClientePermissaoParametroRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverClientePermissaoParametroResponse RemoverAssociacao(RemoverClientePermissaoParametroRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverGrupoRiscoResponse RemoverGrupoRisco(RemoverGrupoRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverParametroRiscoResponse RemoverParametroRisco(RemoverParametroRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverPermissaoRiscoResponse RemoverPermissaoRisco(RemoverPermissaoRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarClientePermissaoParametroResponse SalvarAssociacao(SalvarClientePermissaoParametroRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ListarPermissoesRiscoClienteResponse ListarPermissoesRiscoCliente(ListarPermissoesRiscoClienteRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members

        public MensagemResponseBase SalvarPermissoesRiscoAssociadas(SalvarPermissoesRiscoAssociadasRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarParametroRiscoClienteResponse SalvarExpirarLimite(SalvarParametroRiscoClienteRequest pRequest)
        {
            throw new NotImplementedException();
        }
        
        public ListarParametrosRiscoClienteResponse ListaLimitePorCliente(ListarParametrosRiscoClienteRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarParametrosRiscoClienteResponse ListarLimitePorCliente(ListarParametrosRiscoClienteRequest pParametro)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        public ListarPermissoesRiscoClienteResponse ListarPermissoesRisco(ListarPermissoesRiscoClienteRequest pParametros)
        {
            throw new NotImplementedException();
        }

        public ReceberClientePermissaoParametroResponse ReceberAssociacao(ReceberClientePermissaoParametroRequest lRequest, bool pEfetuarLog = false)
        {
            throw new NotImplementedException();
        }

        public ReceberGrupoResponse ReceberGrupo(ReceberGrupoRequest lRequest, bool pEfetuarLog = false)
        {
            throw new NotImplementedException();
        }

        public ReceberParametroRiscoResponse ReceberParametroRisco(ReceberParametroRiscoRequest lRequest, bool pEfetuarLog = false)
        {
            throw new NotImplementedException();
        }

        public ReceberParametroRiscoClienteResponse ReceberParametroRiscoCliente(ReceberParametroRiscoClienteRequest lRequest, bool pEfetuarLog = false)
        {
            throw new NotImplementedException();
        }

        public ReceberPermissaoRiscoResponse ReceberPermissaoRisco(ReceberPermissaoRiscoRequest lRequest, bool pEfetuarLog = false)
        {
            throw new NotImplementedException();
        }

        public ListarBloqueiroInstrumentoResponse ListarBloqueioPorCliente(ListarBloqueiroInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarBloqueioInstrumentoResponse SalvarBloqueioInstrumento(DbTransaction pDbTransaction, SalvarBloqueioInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioPorCliente(DbTransaction pDbTransaction, RemoverBloqueioInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarClienteParametroGrupoResponse ListarClienteParametroGrupo(ListarClienteParametroGrupoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public RemoverClienteParametroGrupoResponse RemoverClienteParametroGrupo(DbTransaction pDbTransaction, RemoverClienteParametroGrupoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarClienteParametroGrupoResponse SalvarClienteParametroGrupo(DbTransaction pDbTransaction, SalvarClienteParametroGrupoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public OMS.Persistencia.ConsultarObjetosResponse<ClienteLimiteInfo> ConsultarLimitesDoCliente(OMS.Persistencia.ConsultarObjetosRequest<ClienteLimiteInfo> pParametros)
        {
            throw new NotImplementedException();
        }

        public OMS.Persistencia.ConsultarObjetosResponse<ClienteLimiteMovimentoInfo> ConsultarMovimentacaoDosLimitesDoCliente(OMS.Persistencia.ConsultarObjetosRequest<ClienteLimiteMovimentoInfo> pParametros)
        {
            throw new NotImplementedException();
        }


        public ListarGrupoItemResponse ListarGrupoItens(ListarGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ReceberGrupoItemResponse ReceberGrupoItem(ReceberGrupoItemRequest pRequest)
        {
            throw new NotImplementedException();
        }


        public ListarParametroAlavancagemResponse ListarParametroAlavancagem(ListarParametroAlavancagemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarParametroAlavancagemResponse SalvarParametroAlavancagem(SalvarParametroAlavancagemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #region IServicoRegrasRisco Members


        public ListarMonitoramentoRiscoResponse ListarMonitoramentoDeRisco(ListarMonitoramentoRiscoRequest pParametros)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ListarBloqueiroInstrumentoResponse ListarBloqueioClienteInstrumentoDirecao(ListarBloqueiroInstrumentoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItem(ListarRegraGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItemGlobal(ListarRegraGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioClienteInstrumentoDirecao(RemoverClienteBloqueioRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItem(RemoverRegraGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItemGlobal(RemoverRegraGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarBloqueioInstrumentoResponse SalvarClienteBloqueioInstrumentoDirecao(SalvarBloqueioInstrumentoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItem(SalvarRegraGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItemGlobal(SalvarRegraGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ReceberTravaExposicaoResponse ReceberTravaExposicao(ReceberTravaExposicaoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarTravaExposicaoResponse SalvarTravaExposicao(SalvarTravaExposicaoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ReceberFatFingerClienteResponse ReceberFatFingerCliente(ReceberFatFingerClienteRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverFatFingerClienteResponse RemoverFatFingerCliente(RemoverFatFingerClienteRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarFatFingerClienteResponse SalvarFatFingerCliente(SalvarFatFingerClienteRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ListarParametrosRiscoClienteResponse ListarLimitePorClienteNovoOMS(ListarParametrosRiscoClienteRequest pParametro)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public SalvarParametroRiscoClienteResponse SalvarExpirarLimiteNovoOMS(SalvarParametroRiscoClienteRequest pRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ListarBloqueiroInstrumentoResponse ListarBloqueioPorClienteNovoOMS(ListarBloqueiroInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarClienteParametroGrupoResponse ListarClienteParametroGrupoNovoOMS(ListarClienteParametroGrupoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarPermissoesRiscoClienteResponse ListarPermissoesRiscoClienteNovoOMS(ListarPermissoesRiscoClienteRequest pParametros)
        {
            throw new NotImplementedException();
        }

        public ListarPermissoesRiscoResponse ListarPermissoesRiscoNovoOMS(ListarPermissoesRiscoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarParametroRiscoClienteResponse SalvarParametroRiscoClienteNovoOMS(SalvarParametroRiscoClienteRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public MensagemResponseBase SalvarPermissoesRiscoAssociadasNovoOMS(SalvarPermissoesRiscoAssociadasRequest lRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IServicoRegrasRisco Members


        public ListarGruposResponse ListarGruposNovoOMS(ListarGruposRequest pParametro)
        {
            throw new NotImplementedException();
        }

        #endregion


        public ListarBloqueiroInstrumentoResponse ListarBloqueioPorClienteSpider(ListarBloqueiroInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarClienteParametroGrupoResponse ListarClienteParametroGrupoSpider(ListarClienteParametroGrupoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarGruposResponse ListarGruposSpider(ListarGruposRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public ListarParametrosRiscoClienteResponse ListarLimitePorClienteSpider(ListarParametrosRiscoClienteRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarPermissoesRiscoClienteResponse ListarPermissoesRiscoClienteSpider(ListarPermissoesRiscoClienteRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarPermissoesRiscoResponse ListarPermissoesRiscoSpider(ListarPermissoesRiscoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarParametroRiscoClienteResponse SalvarExpirarLimiteSpider(SalvarParametroRiscoClienteRequest pRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarParametroRiscoClienteResponse SalvarParametroRiscoClienteSpider(SalvarParametroRiscoClienteRequest pRequest)
        {
            throw new NotImplementedException();
        }

        public MensagemResponseBase SalvarPermissoesRiscoAssociadasSpider(SalvarPermissoesRiscoAssociadasRequest pRequest)
        {
            throw new NotImplementedException();
        }


        public ListarGrupoItemResponse ListarGrupoItensSpider(ListarGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItemGlobalSpider(ListarRegraGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ListarRegraGrupoItemResponse ListarRegraGrupoItemSpider(ListarRegraGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public ReceberTravaExposicaoResponse ReceberTravaExposicaoSpider(ReceberTravaExposicaoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioClienteInstrumentoDirecaoSpider(RemoverClienteBloqueioRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public RemoverBloqueioInstumentoResponse RemoverBloqueioPorClienteSpider(DbTransaction pDbTransaction, RemoverBloqueioInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public RemoverGrupoItemResponse RemoverGrupoItemSpider(RemoverGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverGrupoRiscoResponse RemoverGrupoRiscoSpider(RemoverGrupoRiscoRequest pRequest)
        {
            throw new NotImplementedException();
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItemGlobalSpider(RemoverRegraGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public RemoverRegraGrupoItemResponse RemoverRegraGrupoItemSpider(RemoverRegraGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarBloqueioInstrumentoResponse SalvarBloqueioInstrumentoSpider(DbTransaction pDbTransaction, SalvarBloqueioInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarBloqueioInstrumentoResponse SalvarClienteBloqueioInstrumentoDirecaoSpider(SalvarBloqueioInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarGrupoItemResponse SalvarGrupoItemSpider(SalvarGrupoItemRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarGrupoResponse SalvarGrupoSpider(SalvarGrupoRequest lRequest)
        {
            throw new NotImplementedException();
        }

        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItemGlobalSpider(SalvarRegraGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarRegraGrupoItemResponse SalvarRegraGrupoItemSpider(SalvarRegraGrupoItemRequest pParametro)
        {
            throw new NotImplementedException();
        }

        public SalvarTravaExposicaoResponse SalvarTravaExposicaoSpider(SalvarTravaExposicaoRequest pParametro)
        {
            throw new NotImplementedException();
        }


        public ListarBloqueiroInstrumentoResponse ListarBloqueioClienteInstrumentoDirecaoSpider(ListarBloqueiroInstrumentoRequest pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
