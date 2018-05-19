using System;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Contratos.Dados
{
    public class FichaCadastralInfo: ICodigoEntidade
    {
        #region | Propriedades

        public long IdCliente { get; set; }

        public ReceberObjetoResponse<ClienteInfo> ClienteResponse { get; set; }

        public ConsultarObjetosResponse<ClienteBancoInfo> ClienteBancoResponse { get; set; }

        public ConsultarObjetosResponse<ClienteContaInfo> ClienteContaResponse { get; set; }

        public ConsultarObjetosResponse<ClienteEnderecoInfo> ClienteEnderecoResponse { get; set; }

        public ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo> ClienteProcuradorRepresentanteResponse { get; set; }

        public ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo> ClienteSituacaoFinanceiraPatrimonialResponse { get; set; }

        public ReceberObjetoResponse<LoginInfo> ClienteLoginResponse { get; set; }

        public ConsultarObjetosResponse<ClienteTelefoneInfo> ClienteTelefoneReponse { get; set; }

        public ConsultarObjetosResponse<ClienteEmitenteInfo> ClienteEmitenteResponse { get; set; }

        public ConsultarObjetosResponse<ClienteContratoInfo> ClienteContratoResponse { get; set; }

        public ConsultarObjetosResponse<ClienteControladoraInfo> ClienteControladoraResponse { get; set; }

        public ConsultarObjetosResponse<ClienteDiretorInfo> ClienteDiretorResponse { get; set; }

        public ConsultarObjetosResponse<ClienteInvestidorNaoResidenteInfo> ClienteIvestidorNaoResidenteResponse { get; set; }

        public ReceberObjetoResponse<ClienteNaoOperaPorContaPropriaInfo> ClienteNaoOperaPorContaPropriaResponse { get; set; }
        
        #endregion

        #region | Construtores

        public FichaCadastralInfo()
        {
            this.ClienteResponse                              = new ReceberObjetoResponse<ClienteInfo>();
            this.ClienteBancoResponse                         = new ConsultarObjetosResponse<ClienteBancoInfo>();
            this.ClienteContaResponse                         = new ConsultarObjetosResponse<ClienteContaInfo>();
            this.ClienteEnderecoResponse                      = new ConsultarObjetosResponse<ClienteEnderecoInfo>();
            this.ClienteProcuradorRepresentanteResponse       = new ConsultarObjetosResponse<ClienteProcuradorRepresentanteInfo>();
            this.ClienteSituacaoFinanceiraPatrimonialResponse = new ConsultarObjetosResponse<ClienteSituacaoFinanceiraPatrimonialInfo>();
            this.ClienteLoginResponse                         = new ReceberObjetoResponse<LoginInfo>();
            this.ClienteTelefoneReponse                       = new ConsultarObjetosResponse<ClienteTelefoneInfo>();
            this.ClienteEmitenteResponse                      = new ConsultarObjetosResponse<ClienteEmitenteInfo>();
            this.ClienteContratoResponse                      = new ConsultarObjetosResponse<ClienteContratoInfo>();
            this.ClienteControladoraResponse                  = new ConsultarObjetosResponse<ClienteControladoraInfo>();
            this.ClienteDiretorResponse                       = new ConsultarObjetosResponse<ClienteDiretorInfo>();
            this.ClienteNaoOperaPorContaPropriaResponse       = new ReceberObjetoResponse<ClienteNaoOperaPorContaPropriaInfo>();
        }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
