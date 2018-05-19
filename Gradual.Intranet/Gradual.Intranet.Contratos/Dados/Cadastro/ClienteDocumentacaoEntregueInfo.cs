using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class ClienteDocumentacaoEntregueInfo : ICodigoEntidade
    {
        public int? IdDocumentacaoEntregue { get; set; }

        public int IdCliente { get; set; }

        public int IdLoginUsuarioLogado { get; set; }

        public DateTime DtAdesaoDocumento { get; set; }

        public string DsObservacao { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
