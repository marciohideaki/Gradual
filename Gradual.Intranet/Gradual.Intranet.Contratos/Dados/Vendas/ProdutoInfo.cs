using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ProdutoInfo : ICodigoEntidade
    {
        #region Propriedades

        public Nullable<int> IdProduto { get; set; }

        public int IdPlano { get; set; }

        public string DsNomeProduto { get; set; }

        public decimal VlPreco { get; set; }

        public decimal VlPrecoCartao { get; set; }

        public bool FlSuspenso { get; set; }

        public string DsMensagemSuspenso { get; set; }

        public string DsDescricao { get; set; }

        public bool FlApareceProdutos { get; set; }
        
        public decimal VlTaxa { get; set; }
        
        public decimal VlTaxa2 { get; set; }

        public string UrlImagem { get; set; }
        
        public string UrlImagem2 { get; set; }
        
        public string UrlImagem3 { get; set; }
        
        public string UrlImagem4 { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
