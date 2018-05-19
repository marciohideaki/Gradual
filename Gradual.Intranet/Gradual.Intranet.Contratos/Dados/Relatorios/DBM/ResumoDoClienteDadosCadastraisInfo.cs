using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoClienteDadosCadastraisInfo : ICodigoEntidade
    {
        public int? ConsultaCdCliente { get; set; }

        public string ConsultaCodigoAssessor { get; set; }

        public string ConsultaNmCliente { get; set; }

        public int CdCliente { get; set; }

        public string NmCliente { get; set; }

        public DateTime DtCriacao { get; set; }

        public DateTime DtUltimaOperacao { get; set; }

        public string DsTipoCliente { get; set; }

        public string NmCidade { get; set; }

        public string DsUF  { get; set; }

        public string  NmLogradouro{ get; set; }

        public string  NrPredio{ get; set; }

        public string  NmComplementoEndereco{ get; set; }

        public string  NmBairro{ get; set; }

        public string  CDDddTel{ get; set; }

        public string  NrTelefone{ get; set; }

        public string  NrRamal{ get; set; }

        public string  CdDddCelular1{ get; set; }

        public string  NrCelular1{ get; set; }

        public string  CdDddCelular2{ get; set; }

        public string  NrCelular2{ get; set; }

        public string  NmEmail{ get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
