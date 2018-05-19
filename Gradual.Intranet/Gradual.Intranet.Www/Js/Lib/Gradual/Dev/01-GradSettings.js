
/*

    GradSettings
        .UltimoSistema      // Último sistema escolhido no menu
        .UltimoSubSistema   // Último subsistema escolhido no menu

*/

var gPreferenciasDoUsuario;                                 // Objeto de preferências do usuário

var gNomeDoCookie    = "gradintra_preferencias";            // Nome do cookie salvo na máquina do usuário

var gExpiracaoDoCookie = 15;                                // Expiração do cookie em dias

var gFlagPodeSalvarPreferencias = true;                     // Flag para "travar" o salvamento das preferências

var gTimeoutParaSalvarPreferencias = 1000;                  // MS para re-habilitar salvar as configurações

var gContagemEsperadaDeConteudosCarregados = 0;             // Quantidade esperada de conteúdos carregados (praticamente a qtd de janelas)

var gContagemDeVerificacoesDeConteudosCarregados = 0;       // Quantidade de conteúdos que foram carregados (recebido via GradWindow)

function Preferencias_CriarObjetoDePreferencias()
{
///<summary>[Função Interna] Cria o objeto que vai guardar todas as preferências do usuário</summary>
///<returns>void</returns>

    var lPreferencias = new Object();

    lPreferencias.UltimoSistema = "Clientes";

    lPreferencias.UltimoSubSistema = "Busca";

    return lPreferencias;
}

function Preferencias_CarregarCookieDoUsuario()
{
///<summary>Carrega o cookie e o objeto de preferências em memória</summary>
///<returns>void</returns>

    var lCookieLocal = $.cookie(gNomeDoCookie);
        
    if(lCookieLocal != null)
    {
        gPreferenciasDoUsuario = $.evalJSON(lCookieLocal);
    }
    else
    {        
        gPreferenciasDoUsuario = Preferencias_CriarObjetoDePreferencias();

        Preferencias_SalvarCookieDoUsuario();
    }
}

function Preferencias_ResetarCookieDoUsuario()
{
///<summary>Reseta o cookie de preferências do usuário</summary>
///<returns>void</returns>

    $.cookie(gNomeDoCookie, null);
        
    alert("Recarregue a página para resetar");
    
    return false;
}

function Preferencias_SalvarCookieDoUsuario()
{    
///<summary>Salva o cookie de preferências</summary>
///<returns>void</returns>

    //console.log("salvando preferências...");
    
    $.cookie(gNomeDoCookie, $.toJSON( gPreferenciasDoUsuario ));
    
    //console.log("preferências salvas...");
}

function Preferencias_IniciarPreferencias()
{
///<summary>Inicia as preferências, carregando o cookie</summary>
///<returns>void</returns>

    Preferencias_CarregarCookieDoUsuario();
    
}

function Preferencias_HabilitarSalvar()
{
///<summary>Habilita a gravação das preferências</summary>
///<returns>void</returns>

    gFlagPodeSalvarPreferencias = true;
}

function Preferencias_DesabilitarSalvar()
{
///<summary>Desabilita a gravação das preferências</summary>
///<returns>void</returns>

    gFlagPodeSalvarPreferencias = false;
}

function Preferencias_SalvarUltimoSistemaSelecionado(pNomeDoSistema)
{
///<summary>Salva nas preferências do usuário o último sistema escolhido no menu</summary>
///<param name="pNomeDoSistema" type="String">Nome do sistema</param>
///<returns>void</returns>

    if(gFlagPodeSalvarPreferencias)
    {
        gPreferenciasDoUsuario.UltimoSistema = pNomeDoSistema;
        
        Preferencias_SalvarCookieDoUsuario();
    }
}

function Preferencias_SalvarUltimoSubSistemaSelecionado(pNomeDoSistema, pNomeDoSubSistema)
{
///<summary>Salva nas preferências do usuário o último subsistema escolhido no menu</summary>
///<param name="pNomeDoSistema" type="String">Nome do sistema</param>
///<param name="pNomeDoSubSistema" type="String">Nome do subsistema</param>
///<returns>void</returns>

    if
    (   gFlagPodeSalvarPreferencias && 
        (gPreferenciasDoUsuario.UltimoSistema    != pNomeDoSistema ||
         gPreferenciasDoUsuario.UltimoSubSistema != pNomeDoSubSistema)
    )
    {
        gPreferenciasDoUsuario.UltimoSistema = pNomeDoSistema;
        gPreferenciasDoUsuario.UltimoSubSistema = pNomeDoSubSistema;
        
        Preferencias_SalvarCookieDoUsuario();
    }
}

function Preferencias_FlagPodeSalvarPreferenciasLiberada()
{
    return gFlagPodeSalvarPreferencias;
}