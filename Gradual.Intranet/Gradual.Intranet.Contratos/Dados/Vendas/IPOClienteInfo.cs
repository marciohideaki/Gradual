using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Vendas
{
    /// <summary>
    /// Classe de Info de gerenciamento de IPO do cliente pela intranet.
    /// </summary>
    [Serializable]
    public class IPOClienteInfo :  ICodigoEntidade
    {
        /// <summary>
        /// Código IPO Cliente 
        /// </summary>
        public Nullable<int> CodigoIPOCliente { get; set; }

        /// <summary>
        /// Codigo do Cliente que solicitou a oferta publica
        /// </summary>
        public int CodigoCliente { get; set; }

        /// <summary>
        /// Código ISIN da oferta publica
        /// </summary>
        public string CodigoISIN { get; set; }

        /// <summary>
        /// Data da solicitação da oferta publica
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Data da de do Filtro
        /// </summary>
        public DateTime DataDe { get; set; }

        /// <summary>
        /// Da Até do Filtro
        /// </summary>
        public DateTime DataAte { get; set; }

        /// <summary>
        /// Código do assessor
        /// </summary>
        public int CodigoAssessor { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string NomeCliente { get; set; }

        /// <summary>
        /// Cpf ou cnpj do cliente que solicitou a oferta publica
        /// </summary>
        public string CpfCnpj { get; set; }

        /// <summary>
        /// Empresa que está abrindo o capital
        /// </summary>
        public string Empresa { get; set; }

        /// <summary>
        /// modalidade da oferta publica selecionada pelo
        /// cliente ou assessor
        /// </summary>
        public string Modalidade { get; set; }

        /// <summary>
        /// Valor digitado pelo cliente ou assessor
        /// </summary>
        public decimal ValorReserva { get; set; }

        /// <summary>
        /// quando o cliente condiciona a reserva ao valor máximo da ação, 
        /// valor digitado pelo cliente ou assessor
        /// </summary>
        public decimal ValorMaximo { get; set; }

        /// <summary>
        ///  taxa cadastrada pela Custodia
        /// </summary>
        public decimal TaxaMaxima { get; set; }

        /// <summary>
        /// Soma dos saldos disponível + saldo em fundos + valor em custodia
        /// </summary>
        public decimal Limite { get; set; }

        /// <summary>
        /// Solicitada, Executada ou Cancelada
        /// </summary>
        public eStatusIPO Status { get; set; }

        /// <summary>
        /// Valor Percentual da garantia
        /// </summary>
        public decimal VlPercentualGarantia { get; set; }

        /// <summary>
        /// Pessoa vinculada pela empresa da oferta publica?
        /// Obervação importante: Essa propriedade nao é a pessoa vinculada que temos na intranet.
        /// Essa pessoa vinculada é a verificação se a pessoa é vinculada a empresa da oferta pública ou não.  
        /// </summary>
        public bool PessoaVinculada { get; set; }

        /// <summary>
        /// Número de protocolo gerado pelo site para controle de reservas de IPO
        /// </summary>
        public string NumeroProtocolo { get; set; }

        /// <summary>
        /// Observações da reserva de oferta pública
        /// </summary>
        public string Observacoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
