using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class InformeRendimentosTesouroDiretoInfo : ICodigoEntidade
    {
        public DateTime ConsultaAno { get; set; }

        public DateTime ConsultaAnoAnterior { get; set; }

        public string ConsultaCpfCnpj { get; set; }

        public DateTime ConsultaDataNascimento { get; set; }

        public int ConsultaCondicaoDeDependente { get; set; }

        public string Posicao { get; set; }

        public decimal? QuantidadeAnoAnterior { get; set; }

        public decimal? ValorAnoAnterior { get; set; }

        public decimal? Quantidade { get; set; }

        public decimal? Valor { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
