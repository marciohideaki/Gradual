using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Spider.Lib;
using Gradual.Spider.Lib.Dados;
using Gradual.Core.OMS.LimiteBMF.Lib;

namespace Gradual.Spider.Www.App_Codigo.Transporte
{
    public class Transporte_Risco
    {
        #region Propriedades
        public Permissoes       gPermissoes     { get; set; }
        public LimiteBmf        gLimiteBmf      { get; set; }
        public LimiteBovespa    gLimiteBovespa  { get; set; }
        public Restricoes       gRestricoes     { get; set; }
        #endregion

        public class Permissoes
        {
            public eTipoLimite TipoLimite { get; set; }

            public bool StAtivo { get; set; }

            public string Limite { get; set; }

            public string Vencimento { get; set; }

            public string NomePermissao { get; set; }

            public string CodigoPermissao { get; set; }

            public List<int> ListaPermissoes { get; set; }

            public List<Transporte_Risco.Permissoes> TraduziPermissoesTela(List<RiscoPermissaoInfo> pListaPermissoes, List<RiscoPermissaoAssociadaInfo> pListaPermissaoClientes )
            {
                var lRetorno = new List<Transporte_Risco.Permissoes>();

                foreach (RiscoPermissaoInfo perm in pListaPermissoes)
                {
                    var lTrans = new Transporte_Risco.Permissoes();

                    lTrans.NomePermissao   = perm.NomePermissao;
                    lTrans.CodigoPermissao = perm.CodigoPermissao.ToString();

                    var lTemPermissao = pListaPermissaoClientes.Find(cliente => { return cliente.PermissaoRisco.CodigoPermissao == perm.CodigoPermissao; });
                    
                    lTrans.StAtivo = lTemPermissao != null ?  true : false;

                    lRetorno.Add(lTrans);
                }


                return lRetorno;
            }

            public Transporte_Risco.Permissoes Traduzir(List<RiscoPermissaoAssociadaInfo> pListaPermissaoClientes)
            {
                var lRetorno = new Transporte_Risco.Permissoes();

                lRetorno.ListaPermissoes = new List<int>();

                pListaPermissaoClientes.ForEach(permissao => 
                {
                    lRetorno.ListaPermissoes.Add(permissao.PermissaoRisco.CodigoPermissao);
                });

                return lRetorno;
            }
        }

        public class LimiteBovespa
        {
            public string LimiteAvistaCompra { get; set; }
            public string LimiteAvistaVenda { get; set; }
            public string LimiteOpcaoCompra { get; set; }
            public string LimiteOpcaoVenda  { get; set; }
            public string FatFinger         { get; set; }
            public string FatFingerData     { get; set; }

            public Transporte_Risco.LimiteBovespa TraduzirLista( List<RiscoParametroClienteInfo> pParametro )
            {
                var lRetorno = new Transporte_Risco.LimiteBovespa();

                pParametro.ForEach(parametro =>
                {
                    if (parametro.Parametro.NomeParametro.Contains("vista"))
                    {
                        if (parametro.Parametro.NomeParametro.Contains("compra"))
                        {
                            lRetorno.LimiteAvistaCompra =  parametro.Valor.HasValue ?  parametro.Valor.Value.ToString("N2") : "0" ;
                        }
                        else
                        {
                            lRetorno.LimiteAvistaVenda = parametro.Valor.HasValue ?   parametro.Valor.Value.ToString("N2") : "0";
                        }
                    }
                    else
                    {
                        if (parametro.Parametro.NomeParametro.Contains("compra"))
                        {
                            lRetorno.LimiteOpcaoCompra = parametro.Valor.HasValue ? parametro.Valor.Value.ToString("N2") : "0";
                        }
                        else
                        {
                            lRetorno.LimiteOpcaoVenda = parametro.Valor.HasValue ? parametro.Valor.Value.ToString("N2") : "0";
                        }
                    }

                    if (parametro.Parametro.NomeParametro.Contains("maximo"))
                    {
                        lRetorno.FatFinger     = parametro.Valor.HasValue ? parametro.Valor.Value.ToString("N2") : "0";
                        lRetorno.FatFingerData = parametro.DataValidade.ToString();
                    }
                });

                return lRetorno;
            }
        }

        public class LimiteBmf
        {
            #region Propriedades
            public string Account                                   { get; set; }
            
            public string Contrato                                  { get; set; }

            public string DataMovimento                             { get; set; }

            public string DataValidade                              { get; set; }

            public string idClienteParametroBMF                     { get; set; }

            public string idClientePermissao                        { get; set; }

            public string QuantidadeDisponivel                      { get; set; }

            public string QuantidadeMaximaOferta                    { get; set; }

            public string QuantidadeTotal                           { get; set; }

            public string RenovacaoAutomatica                       { get; set; }
            
            public string Sentido                                   { get; set; }
            #endregion

            #region Propriedades instrumento
            public string ContratoBase                              { get; set; }
            
            public string dtMovimento                               { get; set; }
            
            public string IdClienteParametroBMF                     { get; set; }

            public string IdClienteParametroInstrumento             { get; set; }
            
            public string Instrumento                               { get; set; }

            public string QtDisponivel                              { get; set; }

            public string QtTotalContratoPai                        { get; set; }

            public string QtTotalInstrumento                        { get; set; }

            public List<Transporte_Risco.LimiteBmf> ListaLimiteBmf  { get; set; }

            #endregion

            public Transporte_Risco.LimiteBmf TraduzirListas(ListarLimiteBMFResponse Limites)
            {
                var lRetorno = new Transporte_Risco.LimiteBmf();

                lRetorno.ListaLimiteBmf = new List<Transporte_Risco.LimiteBmf>();
                
                foreach (ClienteParametroBMFInstrumentoInfo limInstrumento in Limites.ListaLimitesInstrumentos)
                {
                    var lLimite = new Transporte_Risco.LimiteBmf();

                    //lLimite.Contrato                    = limInstrumento.ContratoBase;

                    lLimite.ContratoBase                = limInstrumento.ContratoBase;

                    lLimite.dtMovimento                 = limInstrumento.dtMovimento.ToString("dd/MM/yyyy");

                    lLimite.IdClienteParametroBMF       = limInstrumento.IdClienteParametroBMF.ToString();

                    lLimite.IdClienteParametroInstrumento = limInstrumento.IdClienteParametroInstrumento.ToString();

                    lLimite.Instrumento                 = limInstrumento.Instrumento;

                    lLimite.QtDisponivel                = limInstrumento.QtDisponivel.ToString();

                    lLimite.QtTotalContratoPai          = limInstrumento.QtTotalContratoPai.ToString();

                    lLimite.QtTotalInstrumento          = limInstrumento.QtTotalInstrumento.ToString();

                    lLimite.QuantidadeMaximaOferta      = limInstrumento.QuantidadeMaximaOferta.ToString();

                    lLimite.Sentido                     = limInstrumento.Sentido.ToString();

                    foreach (ClienteParametroLimiteBMFInfo lim in Limites.ListaLimites)
                    {

                        if (lim.Contrato == limInstrumento.ContratoBase)
                        {
                            lLimite.Contrato = lim.Contrato;

                            lLimite.DataMovimento = lim.DataMovimento.ToString("dd/MM/yyyy");

                            lLimite.DataValidade = lim.DataValidade.ToString("dd/MM/yyyy");

                            lLimite.idClienteParametroBMF = lim.idClienteParametroBMF.ToString();

                            lLimite.idClientePermissao = lim.idClientePermissao.ToString();

                            lLimite.QuantidadeDisponivel = lim.QuantidadeDisponivel.ToString();

                            lLimite.QuantidadeMaximaOferta = lim.QuantidadeMaximaOferta.ToString();

                            lLimite.QuantidadeTotal = lim.QuantidadeTotal.ToString();

                            lLimite.RenovacaoAutomatica = lim.RenovacaoAutomatica.ToString();
                        }
                    }

                    lRetorno.ListaLimiteBmf.Add(lLimite);
                }

                var ListaOrdenada = lRetorno.ListaLimiteBmf.OrderBy(l=> l.Contrato).ToList();

                lRetorno.ListaLimiteBmf = ListaOrdenada;

                return lRetorno;
            }
        }

        public class Restricoes
        {
            public Transporte_Risco.Restricoes Traduzir()
            {
                var lRetorno = new Transporte_Risco.Restricoes();

                return lRetorno;
            }
        }
    }
}