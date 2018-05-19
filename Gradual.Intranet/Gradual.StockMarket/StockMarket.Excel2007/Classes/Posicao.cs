/*****************************************************************************
 MainModule...: Acompanhamento
 SubModule....:
 Author.......: Hideaki
 Date.........: 01/10/2013
 Porpouse.....: 

 Modifications:
 Author               Date       Reason
 -------------------- ---------- ---------------------------------------------
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace StockMarket.Excel2007.Classes
{
    public class Posicao : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private System.String instrumento;
        private System.Int32 quantidadeAbertaCompra;
        private System.Int32 quantidadeExecutadaCompra;
        private System.Int32 quantidadeAbertaVenda;
        private System.Int32 quantidadeExecutadaVenda;
        private System.Int32 quantidadeNetAberta;
        private System.Int32 quantidadeNetExecutada;
        private System.Decimal precoMedioCompra;
        private System.Decimal precoMedioVenda;
        private System.Decimal financeiroTotalAbertaCompra;
        private System.Decimal financeiroTotalAbertaVenda;
        private System.Decimal financeiroTotalExecutadaCompra;
        private System.Decimal financeiroTotalExecutadaVenda;
        private System.Decimal financeiroNetAbertas;
        private System.Decimal financeiroNetExecutadas;

        public Posicao()
        {
            this.Instrumento = System.String.Empty;
            this.QuantidadeAbertaCompra = 0;
            this.QuantidadeExecutadaCompra = 0;
            this.QuantidadeAbertaVenda = 0;
            this.QuantidadeExecutadaVenda = 0;
            this.FinanceiroTotalExecutadaCompra = 0;
            this.FinanceiroTotalExecutadaVenda = 0;
            this.PrecoMedioCompra = 0;
            this.PrecoMedioVenda = 0;
            this.FinanceiroNetAbertas = 0;
            this.FinanceiroNetExecutadas = 0;
        }

        /// <summary>
        ///     Instrumento referente a posição em negociação do cliente
        /// </summary>
        public System.String Instrumento
        {
            get
            {
                return this.instrumento;
            }

            set
            {
                if (value != this.instrumento)
                {
                    this.instrumento = value;
                    NotifyPropertyChanged("Instrumento");
                }
            }
        }

        /// <summary>
        ///     Quantidade total de ações em aberto em todos os negócios de compra do cliente para o instrumento discriminado
        /// </summary>
        public System.Int32 QuantidadeAbertaCompra
        {
            get
            {
                return this.quantidadeAbertaCompra;
            }

            set
            {
                if (value != this.quantidadeAbertaCompra)
                {
                    this.quantidadeAbertaCompra = value;
                    CalcularNet();
                    NotifyPropertyChanged("QuantidadeAbertaCompra");
                }
            }
        }

        /// <summary>
        ///     Quantidade total de ações executadas em todos os negócios de compra do cliente para o instrumento discriminado
        /// </summary>
        public System.Int32 QuantidadeExecutadaCompra
        {
            get
            {
                return this.quantidadeExecutadaCompra;
            }

            set
            {
                if (value != this.quantidadeExecutadaCompra)
                {
                    this.quantidadeExecutadaCompra = value;
                    CalcularNet();
                    NotifyPropertyChanged("QuantidadeExecutadaCompra");
                }
            }
        }

        /// <summary>
        ///     Quantidade total de ações em aberto em todos os negócios de venda do cliente para o instrumento discriminado
        /// </summary>
        public System.Int32 QuantidadeAbertaVenda
        {
            get
            {
                return this.quantidadeAbertaVenda;
            }

            set
            {
                if (value != this.quantidadeAbertaVenda)
                {
                    this.quantidadeAbertaVenda = value;
                    CalcularNet();
                    NotifyPropertyChanged("QuantidadeAbertaVenda");
                }
            }
        }

        /// <summary>
        ///     Quantidade total de ações executadas em todos os negócios de compra do cliente para o instrumento discriminado
        /// </summary>
        public System.Int32 QuantidadeExecutadaVenda
        {
            get
            {
                return this.quantidadeExecutadaVenda;
            }

            set
            {
                if (value != this.quantidadeExecutadaVenda)
                {
                    this.quantidadeExecutadaVenda = value;
                    CalcularNet();
                    NotifyPropertyChanged("QuantidadeExecutadaVenda");
                }
            }
        }

        /// <summary>
        ///     Quantidade Net de ações executadas em todos os negócios do cliente para o instrumento discriminado
        /// </summary>
        public System.Int32 QuantidadeNetExecutada
        {
            get
            {
                return this.quantidadeNetExecutada;
            }

            set
            {
                if (value != this.quantidadeNetExecutada)
                {
                    this.quantidadeNetExecutada = value;
                    NotifyPropertyChanged("QuantidadeNetExecutada");
                }
            }
        }

        /// <summary>
        ///     Quantidade Net de ações em aberto em todos os negócios do cliente para o instrumento discriminado
        /// </summary>
        public System.Int32 QuantidadeNetAberta
        {
            get
            {
                return this.quantidadeNetAberta;
            }

            set
            {
                if (value != this.quantidadeNetAberta)
                {
                    this.quantidadeNetAberta = value;
                    NotifyPropertyChanged("QuantidadeNetAberta");
                }
            }
        }

        /// <summary>
        ///     Preço médio de compra em todos os negócios do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal PrecoMedioCompra
        {
            get
            {
                return this.precoMedioCompra;
            }

            set
            {
                if (value != this.precoMedioCompra)
                {
                    this.precoMedioCompra = value;
                    NotifyPropertyChanged("PrecoMedioCompra");
                }
            }
        }

        /// <summary>
        ///     Preço médio de venda em todos os negócios do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal PrecoMedioVenda
        {
            get
            {
                return this.precoMedioVenda;
            }

            set
            {
                if (value != this.precoMedioVenda)
                {
                    this.precoMedioVenda = value;
                    NotifyPropertyChanged("PrecoMedioVenda");
                }
            }
        }

        /// <summary>
        ///     Volume Financeiro de compra em todos os negócios em aberto do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal FinanceiroTotalAbertaCompra
        {
            get
            {
                return this.financeiroTotalAbertaCompra;
            }

            set
            {
                if (value != this.financeiroTotalAbertaCompra)
                {
                    this.financeiroTotalAbertaCompra = value;
                    NotifyPropertyChanged("FinanceiroTotalAbertaCompra");
                }
            }
        }

        /// <summary>
        ///     Volume Financeiro de compra em todos os negócios em aberto do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal FinanceiroTotalAbertaVenda
        {
            get
            {
                return this.financeiroTotalAbertaVenda;
            }

            set
            {
                if (value != this.financeiroTotalAbertaVenda)
                {
                    this.financeiroTotalAbertaVenda = value;
                    NotifyPropertyChanged("FinanceiroTotalAbertaVenda");
                }
            }
        }

        /// <summary>
        ///     Volume Financeiro de compra em todos os negócios executados do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal FinanceiroTotalExecutadaCompra
        {
            get
            {
                return this.financeiroTotalExecutadaCompra;
            }

            set
            {
                if (value != this.financeiroTotalExecutadaCompra)
                {
                    this.financeiroTotalExecutadaCompra = value;
                    NotifyPropertyChanged("FinanceiroTotalExecutadaCompra");
                }
            }
        }

        /// <summary>
        ///     Volume Financeiro de venda em todos os negócios executados do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal FinanceiroTotalExecutadaVenda
        {
            get
            {
                return this.financeiroTotalExecutadaVenda;
            }

            set
            {
                if (value != this.financeiroTotalExecutadaVenda)
                {
                    this.financeiroTotalExecutadaVenda = value;
                    NotifyPropertyChanged("FinanceiroTotalExecutadaVenda");
                }
            }
        }

        /// <summary>
        ///     Volume Financeiro Net de todos os negócios em aberto do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal FinanceiroNetAbertas
        {
            get
            {
                return this.financeiroNetAbertas;
            }

            set
            {
                if (value != this.financeiroNetAbertas)
                {
                    this.financeiroNetAbertas = value;
                    NotifyPropertyChanged("FinanceiroNetAbertas");
                }
            }
        }

        /// <summary>
        ///     Volume Financeiro Net de todos os negócios executados do cliente para o instrumento discriminado
        /// </summary>
        public System.Decimal FinanceiroNetExecutadas
        {
            get
            {
                return this.financeiroNetExecutadas;
            }

            set
            {
                if (value != this.financeiroNetExecutadas)
                {
                    this.financeiroNetExecutadas = value;
                    NotifyPropertyChanged("FinanceiroNetExecutadas");
                }
            }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void CalcularNet()
        {
            try
            {
                this.QuantidadeNetAberta = this.QuantidadeAbertaCompra - this.QuantidadeAbertaVenda;
                this.QuantidadeNetExecutada = this.QuantidadeExecutadaCompra - this.QuantidadeExecutadaVenda;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Posicao.CalcularNet(): {0}", ex.Message));
            }
        }
    }
}
