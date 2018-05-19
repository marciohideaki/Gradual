
namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    /// <summary>
    /// Objeto de retorno da ChamadaAjax quando a chamada é um cadastro
    /// </summary>
    public class TransporteRetornoDeCadastro
    {
        #region Propriedades

        public string IdCadastrado { get; set; }

        #endregion

        #region Construtor

        public TransporteRetornoDeCadastro() { }

        public TransporteRetornoDeCadastro(int pIdCadastrado)
        {
            this.IdCadastrado = pIdCadastrado.ToString();
        }

        public TransporteRetornoDeCadastro(string pIdCadastrado)
        {
            this.IdCadastrado = pIdCadastrado;
        }

        #endregion
    }
}
