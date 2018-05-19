using Gradual.OMS.Library;
using System;

namespace Gradual.FIDC.Adm.DbLib.Mensagem
{
    /// <summary>
    /// Classe de request de fundos x categoria x subcategoria
    /// </summary>
    public class FundoCategoriaSubCategoriaLogRequest : MensagemRequestBase
    {
        public int IdFundoCadastro { get; set; }
        public int IdFundoCategoria { get; set; }
        public int IdFundoSubCategoria { get; set; }
        public DateTime DtAlteracao { get; set; }
        public string TipoTransacao { get; set; }
        public string UsuarioLogado { get; set; }
    }
}
