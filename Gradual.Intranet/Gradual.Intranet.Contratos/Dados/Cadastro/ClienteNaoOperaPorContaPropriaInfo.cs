using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class ClienteNaoOperaPorContaPropriaInfo : ICodigoEntidade
    {
        public int IdClienteNaoOperaPorContaPropria { get; set; }

        public int IdCliente { get; set; }

        public string DsNomeClienteRepresentado { get; set; }

        public string DsCpfCnpjClienteRepresentado { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
