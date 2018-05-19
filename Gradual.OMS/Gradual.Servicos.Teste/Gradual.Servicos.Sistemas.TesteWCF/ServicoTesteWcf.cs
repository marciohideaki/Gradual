using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Servicos.Contratos.TesteWCF;
using Gradual.Servicos.Contratos.TesteWCF.Mensagens;

namespace Gradual.Servicos.Sistemas.TesteWCF
{
    public class ServicoTesteWcf : IServicoTesteWcf
    {
        #region IServicoTesteWcf Members

        ServicoWcfPersistencia gPersistencia = new ServicoWcfPersistencia();

        public Contratos.TesteWCF.Mensagens.ReceberMensagemDeTextoResponse ReceberMensagemTexto(ReceberMensagemDeTextoRequest pRequest)
        {
            return gPersistencia.ReceberMensagemDeTexto(pRequest);
        }

        #endregion

        #region IServicoTesteWcf Members


        public ListarMensagensDeTextoResponse ListarMensagensDeTexto(ListarMensagensDeTextoRequest pRequest)
        {
            return gPersistencia.ListarMensagensDeTexto(pRequest);
        }

        public SalvarMensagemDeTextoResponse SalvarMensagemDeTexto(SalvarMensagemDeTextoRequest pRequest)
        {
            return gPersistencia.SalvarMensagemDeTexto(pRequest);
        }

        #endregion
    }
}
