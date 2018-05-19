namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteAlterarSenhaUsuario
    {

        public TransporteAlterarSenhaUsuario() { }

        public string ParentId { get; set; }

        public string SenhaAtual { get; set; }
        
        public string NovaSenha { get; set; }

        public string ConfirmacaoSenha { get; set; }

        public string TipoDeItem 
        { 
            get
            {
                return "AlterarSenha";
            }
        }
    }
}