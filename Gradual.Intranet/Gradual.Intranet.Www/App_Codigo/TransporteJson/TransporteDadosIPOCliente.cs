using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Vendas;
using System.Globalization;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    /// <summary>
    /// Classe de transporte para gerenciamento de IPO de clientes
    /// </summary>
    public class TransporteDadosIPOCliente
    {
        /// <summary>
        /// Código IPO Cliente
        /// </summary>
        public string CodigoIPOCliente { get; set; }
        
        /// <summary>
        /// Codigo do Cliente que solicitou a oferta publica
        /// </summary>
        public string CodigoCliente    { get; set; }

        /// <summary>
        /// Código ISIN da oferta publica
        /// </summary>
        public string CodigoISIN    { get; set; }

        /// <summary>
        /// Data da solicitação da oferta publica
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Código do assessor
        /// </summary>
        public string CodigoAssessor { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string NomeCliente   { get; set; }

        /// <summary>
        /// Cpf ou cnpj do cliente que solicitou a oferta publica
        /// </summary>
        public string CpfCnpj       { get; set; }

        /// <summary>
        /// Empresa que está abrindo o capital
        /// </summary>
        public string Empresa       { get; set; }

        /// <summary>
        /// modalidade da oferta publica selecionada pelo
        /// cliente ou assessor
        /// </summary>
        public string Modalidade    { get; set; }

        /// <summary>
        /// Valor digitado pelo cliente ou assessor
        /// </summary>
        public string ValorReserva { get; set; }

        /// <summary>
        /// quando o cliente condiciona a reserva ao valor máximo da ação, 
        /// valor digitado pelo cliente ou assessor
        /// </summary>
        public string ValorMaximo { get; set; }

        /// <summary>
        ///  taxa cadastrada pela Custodia
        /// </summary>
        public string TaxaMaxima { get; set; }

        /// <summary>
        /// Soma dos saldos disponível + saldo em fundos + valor em custodia
        /// </summary>
        public string Limite { get; set; }

        /// <summary>
        /// Solicitada, Executada ou Cancelada
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Lista de Transporte de IPO CLiente (Solicitação de Reserva de Oferta Publica)
        /// </summary>
        public List<TransporteDadosIPOCliente> ListaTransporte { get; set; }
        #region Constructors
        public TransporteDadosIPOCliente() {}

        /// <summary>
        /// Número de protocolo gerado pelo site na hora da reserva de oferta publica efetuada 
        /// pelo cliente ou pelo assessor pela intranet
        /// </summary>
        public string NumeroProtocolo { get; set; }

        /// <summary>
        /// Observacoes das reservas de IPO 
        /// </summary>
        public string Observacoes { get; set; }

        /// <summary>
        /// Construtor de IPOClienteInfo para transporte
        /// </summary>
        /// <param name="lIPOClienteInfo"></param>
        public TransporteDadosIPOCliente(IPOClienteInfo lIPOClienteInfo )
        {
            this.CodigoIPOCliente = lIPOClienteInfo.CodigoIPOCliente.HasValue ? lIPOClienteInfo.CodigoIPOCliente.Value.ToString() : string.Empty;
            this.CodigoCliente    = lIPOClienteInfo.CodigoCliente.ToString();
            this.CodigoISIN       = lIPOClienteInfo.CodigoISIN;
            this.Data             = lIPOClienteInfo.Data.ToString("dd/MM/yyyy");
            this.CodigoAssessor   = lIPOClienteInfo.CodigoAssessor.ToString();
            this.NomeCliente      = lIPOClienteInfo.NomeCliente;
            this.CpfCnpj          = lIPOClienteInfo.CpfCnpj;
            this.Empresa          = lIPOClienteInfo.Empresa;
            this.Modalidade       = lIPOClienteInfo.Modalidade;
            this.ValorReserva     = lIPOClienteInfo.ValorReserva.ToString("N2");
            this.ValorMaximo      = lIPOClienteInfo.ValorMaximo.ToString("N2");
            this.TaxaMaxima       = lIPOClienteInfo.TaxaMaxima.ToString("N2");
            this.Limite           = lIPOClienteInfo.Limite.ToString("N2");
            this.Status           = lIPOClienteInfo.Status.ToString();
            this.CodigoIPOCliente = lIPOClienteInfo.CodigoIPOCliente.ToString();
            this.NumeroProtocolo  = lIPOClienteInfo.NumeroProtocolo;
            this.Observacoes      = lIPOClienteInfo.Observacoes;

        }

        
        #endregion

        /// <summary>
        /// Método que efetua o cast para a info em IPOCLienteInfo (Solicitação de reserva de Oferta Pública)
        /// </summary>
        /// <returns>Retorna um objeto convertido</returns>
        public IPOClienteInfo ToProdutoIPOClienteInfo()
        {
            var lRetorno = new IPOClienteInfo();

            var lInfo = new CultureInfo("pt-BR");

            if (!string.IsNullOrEmpty(this.CodigoIPOCliente))
            {
                lRetorno.CodigoIPOCliente = Convert.ToInt32(this.CodigoIPOCliente);
            }

            lRetorno.CodigoCliente   = this.CodigoCliente.DBToInt32();
            lRetorno.CodigoAssessor  = this.CodigoAssessor.DBToInt32();
            lRetorno.CodigoISIN      = this.CodigoISIN;
            lRetorno.CpfCnpj         = this.CpfCnpj;
            lRetorno.Data            = this.Data.DBToDateTime();
            lRetorno.Empresa         = this.Empresa;
            lRetorno.Limite          = Convert.ToDecimal(this.Limite, lInfo);
            lRetorno.Modalidade      = this.Modalidade;
            lRetorno.NomeCliente     = this.NomeCliente;
            lRetorno.Status          = (eStatusIPO)Enum.Parse(typeof(eStatusIPO), this.Status);
            lRetorno.TaxaMaxima      = Convert.ToDecimal(this.TaxaMaxima, lInfo);
            lRetorno.ValorMaximo     = Convert.ToDecimal(this.ValorMaximo, lInfo);
            lRetorno.ValorReserva    = Convert.ToDecimal(this.ValorReserva, lInfo);
            lRetorno.NumeroProtocolo = this.NumeroProtocolo;
            lRetorno.Observacoes     = this.Observacoes;

            return lRetorno;
        }
    }
}