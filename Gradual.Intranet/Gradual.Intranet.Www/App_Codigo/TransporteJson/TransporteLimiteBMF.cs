using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Core.OMS.LimiteBMF.Lib;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteLimiteBMF
    {
        #region Propriedades
        public List<TransporteLimiteBMFContrato> Contratos { get; set; }
        public List<TransporteLimiteBMFInstrumento> Instrumentos { get; set; }
        #endregion

        #region Construtor

        public TransporteLimiteBMF()
        {
            this.Contratos                = new List<TransporteLimiteBMFContrato>();
            this.Instrumentos             = new List<TransporteLimiteBMFInstrumento>();
            this.InstrumentosSelecionados = new List<ComboItemAux>();

            this.QtdeCompra                 ="0";
            this.QtdeCompraDisponivel       ="0";
            this.QtdeVenda                  ="0";
            this.QtdeVendaDisponivel        ="0";
            this.QtdeMaximaOrdemContrato    ="0";
            this.DtValidade                 ="";

            this.QtdeCompraInstrumento = "0";
            this.QtdeVendaInstrumento = "0";
            this.QtdeVendaInstrumentoDisponivel  = "0";
            this.QtdeCompraInstrumentoDisponivel = "0";
            this.QtdeMaximaOrdemInstrumento = "0";
        }

        #endregion

        #region Propriedades de Tela
        public string QtdeCompraInstrumento { get; set; }
        public string QtdeVendaInstrumento { get; set; }
        public string QtdeVendaInstrumentoDisponivel { get; set; }
        public string QtdeCompraInstrumentoDisponivel { get; set; }
        public string QtdeMaximaOrdemInstrumento { get; set; }

        public string QtdeCompra { get; set; }
        public string QtdeCompraDisponivel { get; set; }
        public string QtdeVenda { get; set; }
        public string QtdeVendaDisponivel { get; set; }
        public string QtdeMaximaOrdemContrato { get; set; }
        public string DtValidade { get; set; }
        
        public List<ComboItemAux> InstrumentosSelecionados { get; set; }
        #endregion

        #region Métodos
        public TransporteLimiteBMF TraduzirInstrumentoNovoParaTela(List<ClienteParametroLimiteBMFInfo> pContratos, List<ClienteParametroBMFInstrumentoInfo> pInstrumentos, string getContrato)
        {
            var lRetorno = new TransporteLimiteBMF();

            List<int> lIdsContratosEncontrados = new List<int>();

            try
            {
                var lContratoCompra = pContratos.Find(contrato => { return contrato.Sentido == "C" && contrato.Contrato == getContrato; });

                var lContratoVenda = pContratos.Find(contrato => { return contrato.Sentido == "V" && contrato.Contrato == getContrato; });

                if (lContratoCompra == null || lContratoVenda == null)
                {
                    return lRetorno;
                }

                var lInstrumentoCompra = pInstrumentos.Find(instrumento => { return instrumento.Sentido.ToString() == "C" && instrumento.Instrumento.Substring(0, 3) == getContrato && instrumento.IdClienteParametroInstrumento == 0; });

                var lInstrumentoVenda = pInstrumentos.Find(instrumento => { return instrumento.Sentido.ToString() == "V" && instrumento.Instrumento.Substring(0, 3) == getContrato && instrumento.IdClienteParametroInstrumento == 0; });

                lRetorno.QtdeCompra = lContratoCompra.QuantidadeTotal.ToString();

                lRetorno.QtdeCompraDisponivel = lContratoCompra.QuantidadeDisponivel.ToString();

                lRetorno.QtdeVenda = lContratoVenda.QuantidadeTotal.ToString();

                lRetorno.QtdeVendaDisponivel = lContratoVenda.QuantidadeDisponivel.ToString();

                lRetorno.QtdeCompraInstrumento = lInstrumentoCompra.QtTotalInstrumento.ToString();

                lRetorno.QtdeVendaInstrumento = lInstrumentoVenda.QtTotalInstrumento.ToString();

                lRetorno.QtdeVendaInstrumentoDisponivel = lInstrumentoVenda.QtDisponivel.ToString();

                lRetorno.QtdeCompraInstrumentoDisponivel = lInstrumentoCompra.QtDisponivel.ToString();

                lRetorno.DtValidade = lContratoVenda.DataValidade.ToString("dd/MM/yyyy");

                lRetorno.QtdeMaximaOrdemContrato = lContratoVenda.QuantidadeMaximaOferta.ToString();

                lRetorno.QtdeMaximaOrdemInstrumento = lInstrumentoVenda.QuantidadeMaximaOferta.ToString();

                List<ClienteParametroLimiteBMFInfo> lListContratoEncontrato = pContratos.FindAll(contrato => { return contrato.Contrato == getContrato; });

                //lListContratoEncontrato.ForEach(contrato =>
                //{
                //    lIdsContratosEncontrados.Add(contrato.idClienteParametroBMF);
                //});

                //IEnumerable<ClienteParametroBMFInstrumentoInfo> lListInstrumentoEncontratos = from a in pInstrumentos where lIdsContratosEncontrados.Contains(a.IdClienteParametroBMF) select a;

                IEnumerable<ClienteParametroBMFInstrumentoInfo> lListInstrumentoEncontratos = from a in pInstrumentos where a.Instrumento.Substring(0,3).ToUpper() == getContrato  select a;

                List<string> lTempInstrumentos = new List<string>();

                string lVendaCompra = "V##venda##  C##compra##";

                lListInstrumentoEncontratos.ToList().ForEach(instrumento =>
                {
                    var lInstrumentoEncontrado = lRetorno.InstrumentosSelecionados.Find(inst => { return inst.Id == instrumento.Instrumento; });

                    string lNovaLinha = string.Empty;

                    if (lInstrumentoEncontrado == null)
                    {
                        lVendaCompra = "V##venda##  C##compra##";

                        lNovaLinha = instrumento.Instrumento + " " + lVendaCompra + " " + "M" + instrumento.QuantidadeMaximaOferta;

                        lNovaLinha = (instrumento.Sentido.ToString().Equals("V")) ? lNovaLinha.Replace("##venda##", instrumento.QtTotalInstrumento.ToString()) : lNovaLinha.Replace("##compra##", instrumento.QtTotalInstrumento.ToString());
                    }
                    else
                    {
                        lNovaLinha = lInstrumentoEncontrado.Descricao;

                        lNovaLinha = (instrumento.Sentido.ToString().Equals("V")) ? lNovaLinha.Replace("##venda##", instrumento.QtTotalInstrumento.ToString()) : lNovaLinha.Replace("##compra##", instrumento.QtTotalInstrumento.ToString());

                        lRetorno.InstrumentosSelecionados.Remove(lInstrumentoEncontrado);
                    }

                    lRetorno.InstrumentosSelecionados.Add(new ComboItemAux(instrumento.Instrumento, lNovaLinha));
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lRetorno;
        }

        public TransporteLimiteBMF TraduzirInstrumentoParaTela(List<ClienteParametroLimiteBMFInfo> pContratos, List<ClienteParametroBMFInstrumentoInfo> pInstrumentos, string getContrato, string getInstrumento)
        {
            var lRetorno = new TransporteLimiteBMF();

            List<int> lIdsContratosEncontrados = new List<int>();

            try
            {
                var lContratoCompra = pContratos.Find(contrato => { return contrato.Sentido == "C" && contrato.Contrato == getContrato;  });

                var lContratoVenda = pContratos.Find(contrato => { return contrato.Sentido == "V" && contrato.Contrato == getContrato; });

                if (lContratoCompra == null || lContratoVenda == null)
                {
                    return lRetorno;
                }

                var lInstrumentoCompra = pInstrumentos.Find(instrumento => { return instrumento.Sentido.ToString() == "C" && instrumento.Instrumento == getInstrumento; });

                var lInstrumentoVenda = pInstrumentos.Find(instrumento => { return instrumento.Sentido.ToString() == "V" && instrumento.Instrumento == getInstrumento; });

                lRetorno.QtdeCompra = lContratoCompra.QuantidadeTotal.ToString();

                lRetorno.QtdeCompraDisponivel = lContratoCompra.QuantidadeDisponivel.ToString();

                lRetorno.QtdeVenda = lContratoVenda.QuantidadeTotal.ToString();

                lRetorno.QtdeVendaDisponivel = lContratoVenda.QuantidadeDisponivel.ToString();

                lRetorno.DtValidade = lContratoVenda.DataValidade.ToString("dd/MM/yyyy");

                lRetorno.QtdeCompraInstrumento = lInstrumentoCompra.QtTotalInstrumento.ToString();

                lRetorno.QtdeVendaInstrumento = lInstrumentoVenda.QtTotalInstrumento.ToString();

                lRetorno.QtdeVendaInstrumentoDisponivel = lInstrumentoVenda.QtDisponivel.ToString();

                lRetorno.QtdeCompraInstrumentoDisponivel = lInstrumentoCompra.QtDisponivel.ToString();

                lRetorno.QtdeMaximaOrdemContrato = lContratoVenda.QuantidadeMaximaOferta.ToString();

                lRetorno.QtdeMaximaOrdemInstrumento = lInstrumentoVenda.QuantidadeMaximaOferta.ToString();

                List<ClienteParametroLimiteBMFInfo> lListContratoEncontrato = pContratos.FindAll(contrato => { return contrato.Contrato == getContrato; });

                lListContratoEncontrato.ForEach(contrato =>
                {
                    lIdsContratosEncontrados.Add(contrato.idClienteParametroBMF);
                });

                IEnumerable<ClienteParametroBMFInstrumentoInfo> lListInstrumentoEncontratos = from a in pInstrumentos where lIdsContratosEncontrados.Contains(a.IdClienteParametroBMF) select a;

                List<string> lTempInstrumentos = new List<string>();

                string lVendaCompra = "V##venda##  C##compra##";

                lListInstrumentoEncontratos.ToList().ForEach(instrumento =>
                {
                    var lInstrumentoEncontrado = lRetorno.InstrumentosSelecionados.Find(inst => { return inst.Id == instrumento.Instrumento; });

                    string lNovaLinha = string.Empty;

                    if (lInstrumentoEncontrado == null)
                    {
                        lVendaCompra = "V##venda##  C##compra##";

                        lNovaLinha = instrumento.Instrumento + " " + lVendaCompra + " " + "M" + instrumento.QuantidadeMaximaOferta;

                        lNovaLinha = (instrumento.Sentido.ToString().Equals("V")) ? lNovaLinha.Replace("##venda##", instrumento.QtTotalInstrumento.ToString()) : lNovaLinha.Replace("##compra##", instrumento.QtTotalInstrumento.ToString());
                    }
                    else
                    {
                        lNovaLinha = lInstrumentoEncontrado.Descricao;

                        lNovaLinha = (instrumento.Sentido.ToString().Equals("V")) ? lNovaLinha.Replace("##venda##", instrumento.QtTotalInstrumento.ToString()) : lNovaLinha.Replace("##compra##", instrumento.QtTotalInstrumento.ToString());

                        lRetorno.InstrumentosSelecionados.Remove(lInstrumentoEncontrado);
                    }

                    lRetorno.InstrumentosSelecionados.Add(new ComboItemAux(instrumento.Instrumento, lNovaLinha));
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lRetorno;
        }

        public TransporteLimiteBMF TraduzirParaTela(List<ClienteParametroLimiteBMFInfo> pContratos, List<ClienteParametroBMFInstrumentoInfo> pInstrumentos, string getContrato)
        {
            var lRetorno = new TransporteLimiteBMF();

            List<int> lIdsContratosEncontrados = new List<int>();

            try
            {
                var lContratoCompra = pContratos.Find(contrato => { return contrato.Sentido == "C" && contrato.Contrato == getContrato; });

                var lContratoVenda = pContratos.Find(contrato => { return contrato.Sentido == "V" && contrato.Contrato == getContrato; });

                if (lContratoCompra == null || lContratoVenda == null)
                {
                    return lRetorno;
                }

                var lInstrumentoCompra = pInstrumentos.Find(instrumento => { return instrumento.Sentido.ToString() == "C" && instrumento.Instrumento.Substring(0, 3) == getContrato; });

                var lInstrumentoVenda = pInstrumentos.Find(instrumento => { return instrumento.Sentido.ToString() == "V" && instrumento.Instrumento.Substring(0, 3) == getContrato; });

                lRetorno.QtdeCompra = lContratoCompra.QuantidadeTotal.ToString();
                
                lRetorno.QtdeCompraDisponivel = lContratoCompra.QuantidadeDisponivel.ToString();
                
                lRetorno.QtdeVenda = lContratoVenda.QuantidadeTotal.ToString();
                
                lRetorno.QtdeVendaDisponivel = lContratoVenda.QuantidadeDisponivel.ToString();
                
                lRetorno.DtValidade = lContratoVenda.DataValidade.ToString("dd/MM/yyyy");

                if (lInstrumentoCompra != null)
                {
                    lRetorno.QtdeCompraInstrumento = lInstrumentoCompra.QtTotalInstrumento.ToString();
                    lRetorno.QtdeCompraInstrumentoDisponivel = lInstrumentoCompra.QtDisponivel.ToString();
                }

                if (lInstrumentoVenda != null)
                {
                    lRetorno.QtdeVendaInstrumento = lInstrumentoVenda.QtTotalInstrumento.ToString();
                    lRetorno.QtdeVendaInstrumentoDisponivel = lInstrumentoVenda.QtDisponivel.ToString();
                    lRetorno.QtdeMaximaOrdemInstrumento = lInstrumentoVenda.QuantidadeMaximaOferta.ToString();
                }
                

                lRetorno.QtdeMaximaOrdemContrato = lContratoVenda.QuantidadeMaximaOferta.ToString();

                List<ClienteParametroLimiteBMFInfo> lListContratoEncontrato = pContratos.FindAll(contrato => { return contrato.Contrato == getContrato; });

                lListContratoEncontrato.ForEach(contrato =>
                {
                    lIdsContratosEncontrados.Add(contrato.idClienteParametroBMF);
                });

                IEnumerable<ClienteParametroBMFInstrumentoInfo> lListInstrumentoEncontratos = from a in pInstrumentos where lIdsContratosEncontrados.Contains(a.IdClienteParametroBMF) select a;

                List<string> lTempInstrumentos = new List<string>();

                string lVendaCompra = "V##venda##  C##compra##";

                lListInstrumentoEncontratos.ToList().ForEach(instrumento =>
                {
                    var lInstrumentoEncontrado = lRetorno.InstrumentosSelecionados.Find(inst => { return inst.Id == instrumento.Instrumento; });

                    string lNovaLinha = string.Empty;

                    if (lInstrumentoEncontrado ==null)
                    {
                        lVendaCompra = "V##venda##  C##compra##";

                        lNovaLinha = instrumento.Instrumento + " " + lVendaCompra + " " + "M" + instrumento.QuantidadeMaximaOferta;

                        lNovaLinha = (instrumento.Sentido.ToString().Equals("V")) ? lNovaLinha.Replace("##venda##", instrumento.QtTotalInstrumento.ToString()) : lNovaLinha.Replace("##compra##", instrumento.QtTotalInstrumento.ToString());
                    }else
                    {
                        lNovaLinha = lInstrumentoEncontrado.Descricao;

                        lNovaLinha = (instrumento.Sentido.ToString().Equals("V")) ? lNovaLinha.Replace("##venda##", instrumento.QtTotalInstrumento.ToString()) : lNovaLinha.Replace("##compra##", instrumento.QtTotalInstrumento.ToString());

                        lRetorno.InstrumentosSelecionados.Remove(lInstrumentoEncontrado);
                    }

                    lRetorno.InstrumentosSelecionados.Add(new ComboItemAux(instrumento.Instrumento, lNovaLinha));
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lRetorno;
        }

        public void Traduzir( List<ClienteParametroLimiteBMFInfo> listContrato,  List<ClienteParametroBMFInstrumentoInfo> listInstrumento )
        {
            listContrato.ForEach(contrato => {
                TransporteLimiteBMFContrato lCont = new TransporteLimiteBMFContrato();
                lCont.CodigoBMF                   = contrato.Account.ToString();
                lCont.Contrato                    = contrato.Contrato;
                lCont.DtMovimento                 = contrato.DataMovimento.ToString("dd/MM/yyyy");
                lCont.DtValidade                  = contrato.DataValidade.ToString("dd/MM/yyyy");
                lCont.IdClienteParametroBmf       = contrato.idClienteParametroBMF.ToString();
                lCont.IdClientePermissao          = contrato.idClientePermissao.ToString();
                lCont.QtdeDisponivel              = contrato.QuantidadeDisponivel.ToString();
                lCont.QtdeMaxOferta               = contrato.QuantidadeMaximaOferta.ToString();
                lCont.QtdeTotal                   = contrato.QuantidadeTotal.ToString();
                lCont.Sentido                     = contrato.Sentido;
                lCont.StRenovacao                 = contrato.RenovacaoAutomatica.ToString();

                this.Contratos.Add(lCont);

            });

            listInstrumento.ForEach(Instrumento => {
                TransporteLimiteBMFInstrumento lIns = new TransporteLimiteBMFInstrumento();
                lIns.Contrato                       = Instrumento.ContratoBase;
                lIns.DtMovimento                    = Instrumento.dtMovimento.ToString("dd/MM/yyyy");
                lIns.IdClienteParamentorInstrumento = Instrumento.IdClienteParametroInstrumento.ToString();
                lIns.IdClienteParametroBMF          = Instrumento.IdClienteParametroBMF.ToString();
                lIns.Instrumento                    = Instrumento.Instrumento;
                lIns.QtdeDisponivel                 = Instrumento.QtDisponivel.ToString();
                lIns.QtdeTotalContratoPai           = Instrumento.QtTotalContratoPai.ToString();
                lIns.QtdeTotalInstrumento           = Instrumento.QtTotalInstrumento.ToString();
                lIns.QtMaxOferta                    = Instrumento.QuantidadeMaximaOferta.ToString();
                lIns.Sentido                        = Instrumento.Sentido.ToString();

                this.Instrumentos.Add(lIns);

            });
        }
        
        #endregion
    }
    
    public class TransporteLimiteBMFContrato
    {
        #region Propriedades

        public string IdClienteParametroBmf { get; set; }

        public string IdClientePermissao { get; set; }

        public string CodigoBMF { get; set; }

        public string Contrato { get; set; }

        public string Sentido { get; set; }

        public string QtdeTotal { get; set; }

        public string QtdeDisponivel { get; set; }

        public string QtdeMaxOferta { get; set; }

        public string StRenovacao { get; set; }

        public string DtMovimento { get; set; }

        public string DtValidade { get; set; }
        /*
        public string Codigo { get; set; }

        public string Nome { get; set; }
        
        public decimal TCD { get; set; }
        
        public decimal TVD { get; set; }
        
        public decimal LCI { get; set; }
        
        public decimal LVI { get; set; }
        */
        

        #endregion

        #region Construtor

        public TransporteLimiteBMFContrato()
        {
            
        }

        #endregion
    }
    
    public class TransporteLimiteBMFInstrumento
    {
        #region Propriedades
        public string IdClienteParamentorInstrumento { get; set; }

        public string IdClienteParametroBMF { get; set; }

        public string Contrato { get; set; }

        public string Instrumento { get; set; }

        public string QtdeTotalContratoPai { get; set; }

        public string QtdeTotalInstrumento { get; set; }

        public string QtdeDisponivel { get; set; }

        public string DtMovimento { get; set; }

        public string QtMaxOferta { get; set; }

        public string Sentido { get; set; }

        /*
        public string Codigo { get; set; }

        public string Nome { get; set; }

        public decimal TCD { get; set; }

        public decimal TVD { get; set; }

        public decimal LCI { get; set; }

        public decimal LVI { get; set; }
        */
        #endregion
    }

    public class ComboItemAux
    {
        #region propriedades
        public string Id { get; set; }
        public string Descricao { get; set; }
        #endregion

        #region Construtores
        public ComboItemAux(){}

        public ComboItemAux(string _id, string _descricao)
        {
            this.Id = _id;
            this.Descricao = _descricao;
        }
        #endregion
    }

}