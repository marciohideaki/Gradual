
namespace Gradual.Intranet.Contratos.Dados.Views
{
    /// <summary>
    /// View que pegue o ID na tabela PaisBlackList, mais o código e o nome do país
    /// </summary>
    public class ViewPaisesEmBlackListInfo
    {
        #region Propriedades

        public int IdPaisEmBlackList { get; set; }

        public string CodPais { get; set; }

        public string NomeDoPais { get; set; }

        #endregion
    }
}
