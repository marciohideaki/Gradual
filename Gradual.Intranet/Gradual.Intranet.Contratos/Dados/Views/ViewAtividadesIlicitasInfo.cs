
namespace Gradual.Intranet.Contratos.Dados.Views
{
    /// <summary>
    /// View que pegue o ID na tabela AtividadeBlackList, mais o código e o nome da atividade
    /// </summary>
    public class ViewAtividadesIlicitasInfo
    {
        #region Propriedades

        public int IdAtividadeEmBlackList { get; set; }

        public int CodAtividade { get; set; }

        public string DescricaoDaAtividade { get; set; }

        #endregion
    }
}
