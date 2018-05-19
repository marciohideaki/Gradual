using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class InformeRendimentosInfo : ICodigoEntidade
    {
        public string ConsultaCpfCnpj { get; set; }

        public DateTime ConsultaDataNascimento { get; set; }

        public int ConsultaCondicaoDeDependente { get; set; }

        public DateTime ConsultaDataInicio { get; set; }

        public DateTime ConsultaDataFim { get; set; }

        public int ConsultaTipoInforme { get; set; }

        public string Data { get; set; }
        
        public Nullable<decimal> Rendimento { get; set; }
        
        public Nullable<decimal> Imposto { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
