using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.CadastroPapeis.Mensagens;
using Gradual.OMS.Persistencia.CadastroPapeis.Entidades;
using System.Collections;
using Gradual.OMS.Contratos.CadastroPapeis.Dados;
using System.Threading;
using Gradual.OMS.Contratos.Comum.Dados;
using System.Configuration;
using System.Runtime.CompilerServices;
using Gradual.OMS.Library;
using Gradual.OMS.Contratos.CadastroPapeis;

namespace Gradual.OMS.Sistemas.CadastroPapeis
{
    public class ServicoCadastroPapeis : IServicoCadastroPapeis
    {
        #region Atributes
        private static int _temporizadorListagemBovespaBmf;
        
        private static AutoResetEvent autoEvent = new AutoResetEvent(false);

        private static Timer stateTimer;

        #endregion

        #region Constructors
        public ServicoCadastroPapeis()
        {
            TimerCallback CallBack = ListarPapeisNegociadosBovespaBmf;

            if (stateTimer == null)
                stateTimer = new Timer(CallBack, autoEvent, 5000, TemporizadorListagemBovespaBmf);
        }
        #endregion

        #region Properties
        
        private static Hashtable ListaPapeisNegociados { get; set; }

        private static int TemporizadorListagemBovespaBmf
        {
            get
            {
                if (_temporizadorListagemBovespaBmf == 0)
                {
                    _temporizadorListagemBovespaBmf = Convert.ToInt32(ConfigurationManager.AppSettings["TemporizadorListagemBovespaBmf"].ToString());
                    //_temporizadorListagemBovespaBmf = 50;
                }
                //transformando em horas
                return (_temporizadorListagemBovespaBmf * 1000) * 3600;
            }
        }

        #endregion

        /// <summary>
        /// Consulta na memória Efetuando um query de Linq dentro de uma hashtable que está na memória
        /// </summary>
        /// <param name="pRequest">Request do papel negociado</param>
        /// <returns>Retorna uma lista dos papeis que estão em na memória</returns>
        public ConsultarPapelNegociadoResponse ConsultarPapelNegociado(ConsultarPapelNegociadoRequest pRequest)
        {
            ConsultarPapelNegociadoResponse lResposta = new ConsultarPapelNegociadoResponse()
                {
                    CodigoMensagemRequest = pRequest.CodigoMensagem
                };

            PapelNegociadoBmfDbLib lPapelBmfDb = new PapelNegociadoBmfDbLib();

            PapelNegociadoBovespaDbLib lPapelBovespaDb = new PapelNegociadoBovespaDbLib();

            bool lPapelEncontrado = false;

            try
            {
                //Se ainda não estiver carregado, pedimos para carregar novamente os dados na 
                //hashtable chamando o método ListarPapeisNegociadosBovespaBmf
                if (ListaPapeisNegociados == null)
                    ListarPapeisNegociadosBovespaBmf(null);

                lock (ListaPapeisNegociados)
                {
                    var lPapeis = from a in ListaPapeisNegociados.Cast<DictionaryEntry>()
                                  select a;

                    if (lPapeis.Count() > 0)
                    {
                        lResposta.LstPapelBmfInfo = new List<PapelNegociadoBmfInfo>();

                        lResposta.LstPapelBovespaInfo = new List<PapelNegociadoBovespaInfo>();

                        lResposta.LstPapelInfo = new Hashtable();

                        ///Filtro de ativos
                        if (pRequest.LstAtivos.Count > 0)
                        {
                            if (pRequest.LstAtivos.Count == 1)
                            { 
                                string lBusca = pRequest.LstAtivos[0];

                                lPapeis = from a in ListaPapeisNegociados.Cast<DictionaryEntry>()
                                          where a.Key.ToString().Contains(lBusca)
                                          select a;
                            }
                            else

                                lPapeis = from a in ListaPapeisNegociados.Cast<DictionaryEntry>()
                                          where pRequest.LstAtivos.Contains(a.Key.ToString())
                                          select a;
                        }
                        //filtro com a data de vencimento no resultado
                        if (pRequest.DataVencimento != null)
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                bool lReturn = false;
                                
                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBovespaInfo"))

                                    lReturn = (((PapelNegociadoBovespaInfo)dic.Value).DataVencimento != null && ((PapelNegociadoBovespaInfo)dic.Value).DataVencimento.Value.ToString("dd/MM/yyyy").Equals(pRequest.DataVencimento.Value.ToString("dd/MM/yyyy")));

                                else if (lNameType.Equals("PapelNegociadoBmfInfo"))

                                    lReturn = (((PapelNegociadoBmfInfo)dic.Value).DataVencimento != null && ((PapelNegociadoBmfInfo)dic.Value).DataVencimento.Value.ToString("dd/MM/yyyy").Equals(pRequest.DataVencimento.Value.ToString("dd/MM/yyyy")));

                                else
                                    return lReturn;
                                
                                return lReturn;
                            });

                        ///Filtro com o tipo de mercado no resultado
                        if (!pRequest.TipoMercado.Equals(0))
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBovespaInfo"))

                                    return (((PapelNegociadoBovespaInfo)dic.Value).TipoMercado == pRequest.TipoMercado);

                                else

                                    return false;
                            });

                        ///Filtro de Tipo Segmento de mercado: Se for BMF efetua o filtro 
                        if (pRequest.DescTipoMercado != null && pRequest.DescTipoMercado.Equals("BMF"))
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBmfInfo"))

                                    return (pRequest.DescTipoMercado.Equals("BMF"));

                                else
                                    return false;

                            });

                        if (pRequest.DescTipoMercado != null && pRequest.DescTipoMercado.Equals("BOV"))
                            lPapeis = lPapeis.Where(delegate(DictionaryEntry dic)
                            {
                                string lNameType = dic.Value.GetType().Name;

                                if (lNameType.Equals("PapelNegociadoBovespaInfo"))

                                    return (pRequest.DescTipoMercado.Equals("BOV"));

                                else
                                    return false;
                            });

                        foreach (var item in lPapeis)
                        {
                            var lItem = item.Value;

                            string lNameType = item.Value.GetType().Name;

                            if (lNameType.Equals("PapelNegociadoBmfInfo"))
                                lResposta.LstPapelBmfInfo.Add((PapelNegociadoBmfInfo)item.Value);

                            if (lNameType.Equals("PapelNegociadoBovespaInfo"))
                                lResposta.LstPapelBovespaInfo.Add((PapelNegociadoBovespaInfo)item.Value);

                            if (!lResposta.LstPapelInfo.Contains(item.Key))
                                lResposta.LstPapelInfo.Add(item.Key, item.Value);
                        }

                        lPapelEncontrado = true;
                    }

                    if (lPapelEncontrado)
                    {
                        lResposta.DescricaoResposta = "Ativo(s)  encontrado(s) com sucesso.";
                        lResposta.StatusResposta = Contratos.Comum.Mensagens.MensagemResponseStatusEnum.OK;
                    }
                    else
                    {
                        lResposta.DescricaoResposta = string.Concat("Ativo(s) não encontrado");
                        lResposta.StatusResposta = Contratos.Comum.Mensagens.MensagemResponseStatusEnum.ErroValidacao;
                    }
                }
            }
            catch (Exception ex)
            {
                lResposta.DescricaoResposta = ex.Message;
                lResposta.StatusResposta = Contratos.Comum.Mensagens.MensagemResponseStatusEnum.ErroPrograma;
                Log.EfetuarLog(ex, pRequest);
            }

            return lResposta;
        }

        #region Métodos de apoio
        
        /// <summary>
        /// Métod para listar os papeis que estão no banco e inserir na hashtable estática
        /// </summary>
        /// <param name="pStateTransaction"></param>
        [MethodImpl(MethodImplOptions.PreserveSig)]        
        private void ListarPapeisNegociadosBovespaBmf(object pStateTransaction)
        {
            try
            {
                ConsultarPapelNegociadoResponse lResposta = new ConsultarPapelNegociadoResponse();

                object lStateThread = pStateTransaction;

                PapelNegociadoBmfDbLib lPapelBmfDb = new PapelNegociadoBmfDbLib();

                PapelNegociadoBovespaDbLib lPapelBovespaDb = new PapelNegociadoBovespaDbLib();
                
                List<PapelNegociadoBovespaInfo> listPapelBovespa = lPapelBovespaDb.ListarPapelNegociadoBovespa();

                List<PapelNegociadoBmfInfo> listPapelBmf = lPapelBmfDb.ListarPapelNegociadoBmf();

                ListaPapeisNegociados = new Hashtable();

                ListaPapeisNegociados.Clear();

                lock (ListaPapeisNegociados)
                {
                    ListaPapeisNegociados.Clear();

                    foreach (PapelNegociadoBovespaInfo item in listPapelBovespa)
                    {
                        if (!ListaPapeisNegociados.Contains(item.CodNegociacao.Trim()))
                            ListaPapeisNegociados.Add(item.CodNegociacao.Trim(), item);
                    }

                    foreach (PapelNegociadoBmfInfo item in listPapelBmf)
                    {
                        if (!ListaPapeisNegociados.Contains(string.Concat(item.CodMercadoria.Trim(), item.SerieVencimento.Trim())))
                            ListaPapeisNegociados.Add(string.Concat(item.CodMercadoria.Trim(),item.SerieVencimento.Trim()), item);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, pStateTransaction);
            }
        }
        #endregion

    }
 
}
