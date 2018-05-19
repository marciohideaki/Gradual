using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class PoupeDirectProdutoInfo : ICodigoEntidade
    {
        public int IdProduto { get; set; }

        public string DsProduto { get; set; }

        public int VlPermanenciaMinima { get; set; }

        public decimal VlPerrcentualMulta { get; set; }

        public int QtDiasParaVencimento { get; set; }

        public int QtDiasRetroTrocaPlano { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
