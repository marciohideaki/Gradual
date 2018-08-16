using System;
using Gradual.Intranet;
using Gradual.Intranet.Servicos.Mock;
using System.Data;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.OMS.Library.Servicos;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www
{
    public partial class _Default : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();

            ConfiguracoesValidadas.Iniciar();
            
            //TODO: Verificar acesso ao banco
            
            //TODO: Cookie de "lembrar login", se houver redireciona pra SAC/Default.aspx
            
            // Testando o build target com a solução de copiar arquivo direto pro servidor

            // Pra tirar screenshots

            Server.Transfer("Login.aspx");
        }
    }
}
