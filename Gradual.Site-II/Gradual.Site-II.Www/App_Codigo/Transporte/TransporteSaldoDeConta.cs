using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.ContaCorrente.Lib;

namespace Gradual.Site.Www
{
    [Serializable]
    public class TransporteSaldoDeConta
    {
        #region Propriedades

        public decimal Acoes_SaldoD0 { get; set; }

        public decimal Acoes_SaldoD1 { get; set; }

        public decimal Acoes_SaldoD2 { get; set; }

        public decimal Acoes_SaldoD3 { get; set; }

        public decimal Acoes_SaldoContaMargem { get; set; }

        public decimal Acoes_Limite { get; set; }

        public decimal Acoes_LimiteTotalAVista { get; set; }

        public decimal Opcoes_SaldoD0 { get; set; }

        public decimal Opcoes_SaldoD1 { get; set; }

        public decimal Opcoes_Limite { get; set; }

        public decimal Opcoes_LimiteTotal { get; set; }

        public decimal BMF_LimiteOperacional { get; set; }

        public decimal BMF_SaldoMargem { get; set; }

        public decimal SaldoBloqueado { get; set; }

        public decimal Acoes_SaldoBloqueado { get { return this.SaldoBloqueado; } }

        public decimal Opcoes_SaldoBloqueado { get { return this.SaldoBloqueado; } }

        public decimal BMF_SaldoBloqueado { get { return this.SaldoBloqueado; } }

        public decimal BMF_DisponivelParaResgate { get; set; }

        public decimal SaldoAtual { get; set; }

        public decimal SaldoProjetado
        {
            get
            {
                return (this.Acoes_SaldoD0 + this.Acoes_SaldoD1 + this.Acoes_SaldoD2 + this.Acoes_SaldoD3);
            }
        }

        public decimal TotalFundos { get; set; }

        public decimal SaldoTotal { get; set; }

        public decimal SomatoriaCustodia { get; set; }
        #endregion

        #region Construtores

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
                if (pContaInfo.SaldoContaMargem.HasValue)
                    this.Acoes_SaldoContaMargem = pContaInfo.SaldoContaMargem.Value;

                // limites faltam vir do serviço;
                //this.Acoes_Limite = pContaInfo.LimiteOperacioalDisponivelAVista;

                if (pContaInfo.SaldoBloqueado.HasValue)
                    this.SaldoBloqueado = pContaInfo.SaldoBloqueado.Value;

                //de acordo com WI 470: ( D0 + D1 + D2 + D3 + ContaMargem + LimiteAcoes + SaldoBloqueado )

                //this.SaldoProjetado =
                //      this.Acoes_SaldoD0
                //    + this.Acoes_SaldoD1
                //    + this.Acoes_SaldoD2
                //    + this.Acoes_SaldoD3
                //    + this.Acoes_SaldoContaMargem;
                    

                this.Acoes_LimiteTotalAVista =
                      this.Acoes_SaldoD0
                    + this.Acoes_SaldoD1
                    + this.Acoes_SaldoD2
                    + this.Acoes_SaldoD3
                    + this.Acoes_SaldoContaMargem
                    //+ this.Acoes_Limite
                    + this.SaldoBloqueado;

                this.Opcoes_Limite = pContaInfo.LimiteOperacioalDisponivelOpcao;

                this.Opcoes_SaldoD0 = Acoes_SaldoD0;
                this.Opcoes_SaldoD1 = Acoes_SaldoD1;


                //SaldoDisponivelD0 + SaldoLiquidacaoD1
                this.Opcoes_LimiteTotal =
                    this.Opcoes_SaldoD0
                    + this.Opcoes_SaldoD1
                    //+ this.Opcoes_Limite
                    + this.SaldoBloqueado;

                this.BMF_LimiteOperacional = 0;

                this.BMF_SaldoMargem = 0;

                // dis. para resgate = ((SaldoMargem - SaldoBloqueado) - LimiteOperacional)
                this.BMF_DisponivelParaResgate = this.BMF_SaldoMargem - this.BMF_LimiteOperacional;

            }
        }


        private void InicializaContaBMF(ContaCorrenteBMFInfo pContaBMFInfo)
        {

            if (pContaBMFInfo != null)
            {

                this.BMF_LimiteOperacional = 0;

                this.BMF_SaldoMargem = pContaBMFInfo.SaldoGarantias == null ? 0 : pContaBMFInfo.SaldoGarantias.Value;

                // dis. para resgate = ((SaldoMargem - SaldoBloqueado) - LimiteOperacional)
                this.BMF_DisponivelParaResgate = this.BMF_SaldoMargem + this.BMF_LimiteOperacional;
            }
        }

    }
}