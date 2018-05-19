using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Basket.Lib;
using Gradual.OMS.Ordens.Persistencia.Lib;
using Gradual.OMS.Library;
using Gradual.OMS.Ordens.Lib;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.OMS.Basket
{
    public class ServicoBasket : IServicoBasket
    {

        #region IServicoBasket Members

        public GravarBasketResponse GravarNovaBasket(Lib.Mensageria.GravarBasketRequest _request)
        {
            try
            {
                return new PersistenciaOrdens().InserirClienteBasket(_request._BasketInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ListarItensBasketResponse ListarItemsBasket(int CodigoBasket)
        {
            try
            {
                return new PersistenciaOrdens().SelecionarItemsBasket(CodigoBasket);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public GravarItemBasketResponse GravarItemBasket(GravarItemBasketRequest _request)
        {
            try
            {
                return new PersistenciaOrdens().InserirItemClienteBasket(_request._BasketItemInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public EnviarAtualizaoPrecoGlobalBasketResponse EnviarAtualizaoPrecoGlobal(EnviarAtualizaoPrecoGlobalRequest _request)
        {
            try
            {                
                EnviarAtualizaoPrecoGlobalBasketResponse _response = new EnviarAtualizaoPrecoGlobalBasketResponse();

                if (_request.IndicadorOscilacao == 'S')
                {
                    _request.PrecoBasket = (_request.PrecoBasket / 100);
                }
                _response = new PersistenciaOrdens().AtualizaocaoGlobalClienteBasket(_request);

                return _response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public EnviarOrdemBasketResponse EnviarOrdemBasket(EnviarOrdemBasketRequest _request)
        {
            EnviarOrdemBasketResponse _response = new EnviarOrdemBasketResponse();

            IServicoOrdens _gServico = Ativador.Get<IServicoOrdens>();

            try
            {
                ListarItensBasketResponse _resposta = new PersistenciaOrdens().SelecionarItemsBasket(_request.CodigoBasket);

                foreach (BasketItemInfo info in _resposta.ListaItemsBasket)
                {
                    EnviarOrdemRequest OrdemRequest = new EnviarOrdemRequest();

                    ClienteOrdemInfo _ClienteOrdemInfo = new ClienteOrdemInfo();

                    _ClienteOrdemInfo.CodigoCliente = info.CodigoCliente;
                    _ClienteOrdemInfo.DataHoraSolicitacao = DateTime.Now;
                    _ClienteOrdemInfo.DataValidade = info.DataValidade;

                    if (info.LadoOferta == "C")
                    {
                        _ClienteOrdemInfo.DirecaoOrdem = OrdemDirecaoEnum.Compra;
                    }
                    else
                    {
                        _ClienteOrdemInfo.DirecaoOrdem = OrdemDirecaoEnum.Venda;
                    }

                    _ClienteOrdemInfo.Instrumento = info.Papel;
                    _ClienteOrdemInfo.PortaControleOrdem = _request.PortaControle.ToString();
                    _ClienteOrdemInfo.Preco = double.Parse(info.Preco.ToString());
                    _ClienteOrdemInfo.Quantidade = info.Quantidade;
                    _ClienteOrdemInfo.QuantidadeAparente = info.QuantidadeAparente;
                    _ClienteOrdemInfo.QuantidadeMinima = info.QuantidadeMinima;
                    _ClienteOrdemInfo.TipoDeOrdem = OrdemTipoEnum.Limitada;
                    _ClienteOrdemInfo.ValidadeOrdem = Gradual.OMS.RoteadorOrdens.Lib.Dados.OrdemValidadeEnum.ValidaParaODia;

                    OrdemRequest.ClienteOrdemInfo = _ClienteOrdemInfo;

                    EnviarOrdemResponse _resp = _gServico.EnviarOrdem(OrdemRequest);
                }

                new PersistenciaOrdens().AtualizarStatusBasket("D", _request.CodigoBasket);

                _response.Sucesso = true;

                return _response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public AtualizaItemBasketResponse AtualizarItemBasket(AtualizaItemBasketRequest _request)
        {
            try
            {
                

                return new PersistenciaOrdens().AtualizarItemBasket(_request._BasketItemInfo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ExcluirBasketResponse ExcluirBasket (ExcluirBasketRequest _request)
        {
            try{

                return new PersistenciaOrdens().ExcluirBasket(_request);
            }
            catch (Exception ex){
                throw (ex);
            }
        }

        public ExcluirItemBasketResponse ExcluirItemBasket(ExcluirItemBasketRequest _request)
        {
            try
            {
                return new PersistenciaOrdens().ExcluirItemBasket(_request);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion
    }
}
