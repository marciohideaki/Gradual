using System;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;
using log4net;

namespace Gradual.Servico.FichaCadastral
{
    public class ServicoFichaCadastral : IServicoFichaCadastral
    {
        public ServicoFichaCadastral()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region | Implementação de IServicoFichaCadastral

        public ReceberObjetoResponse<Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo> GerarFichaCadastralPF(ReceberEntidadeRequest<Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo> pParametro)
        {
            return new FichaCadastral_PF().GerarFichaCadastral_PF(pParametro);
        }

        public DocumentResponse GerarFichaCadastralPFBytes(ReceberEntidadeRequest<Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo> pParametro)
        {
            return new FichaCadastral_PF().GerarFichaCadastral_PF_Bytes(pParametro, TipoFicha.FichaCadastral);
        }

        public DocumentResponse GerarFichaCambioPFBytes(ReceberEntidadeRequest<Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo> pParametro)
        {
            return new FichaCadastral_PF().GerarFichaCadastral_PF_Bytes(pParametro, TipoFicha.ContratoCambio);
        }

        public ReceberObjetoResponse<Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo> GerarFichaCadastralPJ(ReceberEntidadeRequest<Gradual.Servico.FichaCadastral.Lib.FichaCadastralGradualInfo> pParametro)
        {
            return new FichaCadastral_PJ().GerarFichaCadastral_PJ(pParametro);
        }

        public ReceberObjetoResponse<Gradual.Servico.FichaCadastral.Lib.TermoAdesaoGradualInfo> GerarTermoDeAdesao(ReceberEntidadeRequest<Gradual.Servico.FichaCadastral.Lib.TermoAdesaoGradualInfo> pParametro)
        {
            return new TermoAdesao_PF().GerarTermoDeAdesao_PF(pParametro);
        }

        #endregion
    }
}
