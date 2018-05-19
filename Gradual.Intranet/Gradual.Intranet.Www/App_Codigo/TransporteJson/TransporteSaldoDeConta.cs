using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.Intranet.Contratos.Dados.Risco;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    [Serializable]
    public class TransporteSaldoDeConta
    {
        #region | Propriedades

        public decimal Acoes_SaldoD0 { get; set; }

        public decimal Acoes_SaldoD1 { get; set; }

        public decimal Acoes_SaldoD2 { get; set; }

        public decimal Acoes_SaldoD3 { get; set; }

        public decimal Acoes_SaldoContaMargem { get; set; }

        public decimal Acoes_Limite { get; set; }

        public decimal Acoes_Limite_Venda { get; set; }

        public decimal Acoes_LimiteTotalAVista { get; set; }

        public decimal Opcoes_SaldoD0 { get; set; }

        public decimal Opcoes_SaldoD1 { get; set; }

        public decimal Opcoes_Limite { get; set; }

        public decimal Opcoes_Limite_Venda { get; set; }

        public decimal Opcoes_LimiteTotal { get; set; }

        public decimal BMF_LimiteOperacional { get; set; }

        public decimal BMF_SaldoMargem { get; set; }

        public decimal SaldoBloqueado { get; set; }

        public decimal BMF_DisponivelParaResgate { get; set; }

        public decimal SaldoAtual { get; set; }

        public decimal Limite_CompraAVista_Total { get; set; }

        public decimal Limite_CompraAVista_Utilizado { get; set; }

        public decimal Limite_CompraAVista_Disponivel { get; set; }

        public decimal Limite_VendaAVista_Total { get; set; }

        public decimal Limite_VendaAVista_Utilizado { get; set; }

        public decimal Limite_VendaAVista_Disponivel { get; set; }

        public decimal Limite_CompraOpcoes_Total { get; set; }

        public decimal Limite_CompraOpcoes_Utilizado { get; set; }

        public decimal Limite_CompraOpcoes_Disponivel { get; set; }

        public decimal Limite_VendaOpcoes_Total { get; set; }

        public decimal Limite_VendaOpcoes_Utilizado { get; set; }

        public decimal Limite_VendaOpcoes_Disponivel { get; set; }

        #endregion

        #region Construtor

        public TransporteSaldoDeConta() { }

        public TransporteSaldoDeConta(ContaCorrenteInfo pContaInfo, ContaCorrenteBMFInfo pContaBMFInfo)
        {
            this.InicializaConta(pContaInfo);
            this.InicializaContaBMF(pContaBMFInfo);
        }
        public TransporteSaldoDeConta(ContaCorrenteInfo pContaInfo)
        {
            this.InicializaConta(pContaInfo);
        }

        public TransporteSaldoDeConta(ContaCorrenteBMFInfo pContaBMFInfo)
        {
            this.InicializaConta(new ContaCorrenteInfo());
            this.InicializaContaBMF(pContaBMFInfo);
        }

        #endregion

        #region Private Methods

        private void InicializaConta(ContaCorrenteInfo pContaInfo)
        {
            if (pContaInfo != null)
            {
                this.SaldoAtual = pContaInfo.SaldoCompraAcoes;

                this.Acoes_SaldoD0 = pContaInfo.SaldoD0;
                this.Acoes_SaldoD1 = pContaInfo.SaldoD1;
                this.Acoes_SaldoD2 = pContaInfo.SaldoD2;
                this.Acoes_SaldoD3 = pContaInfo.SaldoD3;

                // saldo margem = d0
                this.Acoes_SaldoContaMargem = (pContaInfo.SaldoContaMargem.HasValue) ? pContaInfo.SaldoContaMargem.Value : 0;

                // limites faltam vir do serviço;
                this.Acoes_Limite = pContaInfo.LimiteOperacioalDisponivelAVista;
                this.Acoes_Limite_Venda = pContaInfo.LimiteOperacioalDisponivelAVistaVenda;

                //SaldoDisponivelD0 + SaldoLiquidacaoD1 + SaldoLiquidacaoD2 + SaldoLiquidacaoD3 + LimiteAcoes
                this.Acoes_LimiteTotalAVista = this.Acoes_SaldoD0
                                             + this.Acoes_SaldoD1
                                             + this.Acoes_SaldoD2
                                             + this.Acoes_SaldoD3
                                             + this.Acoes_SaldoContaMargem;

                this.Opcoes_Limite = pContaInfo.LimiteOperacioalDisponivelOpcao;
                this.Opcoes_Limite_Venda = pContaInfo.LimiteOperacioalDisponivelOpcaoVenda;

                this.Opcoes_SaldoD0 = Acoes_SaldoD0;
                this.Opcoes_SaldoD1 = Acoes_SaldoD1;

                //SaldoDisponivelD0 + SaldoLiquidacaoD1
                this.Opcoes_LimiteTotal = this.Opcoes_SaldoD0
                                        + this.Opcoes_SaldoD1
                                        + this.Opcoes_Limite
                                        + this.Opcoes_Limite_Venda;

                this.BMF_LimiteOperacional = 0;

                this.BMF_SaldoMargem = 0;

                if (pContaInfo.SaldoBloqueado.HasValue)
                    this.SaldoBloqueado = pContaInfo.SaldoBloqueado.Value;

                // dis. para resgate = ((SaldoMargem - SaldoBloqueado) - LimiteOperacional)
                this.BMF_DisponivelParaResgate = 0;// this.BMF_SaldoMargem - this.BMF_SaldoBloqueado - this.BMF_LimiteOperacional;
            }
        }

        private void InicializaContaBMF(ContaCorrenteBMFInfo pContaBMFInfo)
        {
            if (pContaBMFInfo != null)
            {
                this.BMF_LimiteOperacional = 0;

                this.BMF_SaldoMargem = pContaBMFInfo.SaldoGarantias == null ? 0 : pContaBMFInfo.SaldoGarantias.Value;

                // dis. para resgate = ((SaldoMargem - SaldoBloqueado) - LimiteOperacional)

                if (pContaBMFInfo.SaldoBloqueado.HasValue)
                {
                    this.BMF_DisponivelParaResgate = (this.BMF_SaldoMargem + this.BMF_LimiteOperacional) - pContaBMFInfo.SaldoBloqueado.Value;
                }
                else
                {
                    this.BMF_DisponivelParaResgate = (this.BMF_SaldoMargem + this.BMF_LimiteOperacional);
                }
            }
        }

        public void CarregarDadosDeLimite(List<RiscoLimiteAlocadoInfo> pListaDeLimites)
        {
            string lDescParam;

            if (null != pListaDeLimites && pListaDeLimites.Count > 0)
                pListaDeLimites.ForEach(lLimite =>
                {
                    if (!string.IsNullOrEmpty(lLimite.DsParametro))
                    {
                        lDescParam = lLimite.DsParametro.ToLower();

                        if (lDescParam.Contains("compra"))
                        {
                            if (lDescParam.Contains("vista"))
                            {
                                this.Limite_CompraAVista_Total = lLimite.VlParametro;
                                this.Limite_CompraAVista_Utilizado = lLimite.VlAlocado;
                                this.Limite_CompraAVista_Disponivel = lLimite.VlDisponivel;
                            }
                            else
                            {
                                this.Limite_CompraOpcoes_Total = lLimite.VlParametro;
                                this.Limite_CompraOpcoes_Utilizado = lLimite.VlAlocado;
                                this.Limite_CompraOpcoes_Disponivel = lLimite.VlDisponivel;
                            }
                        }
                        else if (lDescParam.Contains("descoberto"))
                        {
                            if (lDescParam.Contains("vista"))
                            {
                                this.Limite_VendaAVista_Total = lLimite.VlParametro;
                                this.Limite_VendaAVista_Utilizado = lLimite.VlAlocado;
                                this.Limite_VendaAVista_Disponivel = lLimite.VlDisponivel;
                            }
                            else
                            {
                                this.Limite_VendaOpcoes_Total = lLimite.VlParametro;
                                this.Limite_VendaOpcoes_Utilizado = lLimite.VlAlocado;
                                this.Limite_VendaOpcoes_Disponivel = lLimite.VlDisponivel;
                            }
                        }
                    }
                });
        }

        #endregion
    }
}