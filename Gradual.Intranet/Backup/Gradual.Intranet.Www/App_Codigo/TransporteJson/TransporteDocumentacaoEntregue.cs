using System;
using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteDocumentacaoEntregue
    {
        public string Id { get; set; }

        public string IdCliente { get; set; }

        public string IdLoginUsuarioLogado { get; set; }

        public string DtAdesaoDocumento { get; set; }

        public string DsObservacao { get; set; }

        public string TipoDeItem { get { return "DocumentacaoEntregue"; } }

        public List<TransporteDocumentacaoEntregue> TraduzirLista(List<ClienteDocumentacaoEntregueInfo> pParametros)
        {
            var lRetorno = new List<TransporteDocumentacaoEntregue>();

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(cde =>
                {
                    lRetorno.Add(new TransporteDocumentacaoEntregue()
                    {
                        DtAdesaoDocumento = cde.DtAdesaoDocumento.DBToString(),
                        DsObservacao = cde.DsObservacao,
                        IdCliente = cde.IdCliente.DBToString(),
                        Id = cde.IdDocumentacaoEntregue.DBToString(),
                        IdLoginUsuarioLogado = cde.IdLoginUsuarioLogado.DBToString(),
                    });
                });

            return lRetorno;
        }

        public TransporteDocumentacaoEntregue TraduzirLista(ClienteDocumentacaoEntregueInfo pParametros) 
        {
            return new TransporteDocumentacaoEntregue()
            {
                DtAdesaoDocumento = pParametros.DtAdesaoDocumento.DBToString(),
                DsObservacao = pParametros.DsObservacao,
                IdCliente = pParametros.IdCliente.DBToString(),
                Id = pParametros.IdDocumentacaoEntregue.DBToString(),
                IdLoginUsuarioLogado = pParametros.IdLoginUsuarioLogado.DBToString(),
            };
        }

        public ClienteDocumentacaoEntregueInfo ToClienteDocumentacaoEntregueInfo()
        {
            return new ClienteDocumentacaoEntregueInfo()
            {
                DsObservacao = this.DsObservacao,
                DtAdesaoDocumento = this.DtAdesaoDocumento.DBToDateTime(),
                IdCliente = this.IdCliente.DBToInt32(),
                IdDocumentacaoEntregue = string.IsNullOrWhiteSpace(this.Id) ? new Nullable<int>() : this.Id.DBToInt32(),
                IdLoginUsuarioLogado = this.IdLoginUsuarioLogado.DBToInt32(),
            };
        }
    }
}