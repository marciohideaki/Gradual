

//  jQuery e javascript na solution Gradual.Intranet

//1. jQuery

//  A função jQuery está encapsulada no símbolo $, e o que ela faz, essencialmente, é selecionar um conjunto
//  de objetos HTML do DOM e extendê-los com várias outras funções para que seja mais fácil manipulá-los,
//  inclusive já verificando qual o navegador sendo utilizado pelo usuário e mantendo isso transparente
//  para o desenvolvedor.
    
//  O jQuery funciona com seletores de CSS, e alguns pseudo-seletores a mais. Para relembrar:
    
        $("div")                // seleciona todos os DIVs
        $("#menu1")             // seleciona o elemento que tenha ID = "menu1"
        $(".menu1")             // seleciona todos os elementos que tenham class = "menu1"
        $("ul.menu1 li")        // seleciona todos os elementos "li" dentro de todas as ULs que tiverem class = "menu1"
        $("div[title='menu']")  // seleciona os DIVs que tiverem atributo title = "menu1" ( [] funciona com qualquer atributo)
        $("div:visible")        // todos os divs que estiverem visíveis (:visible é um pseudo-atributo específico do jQuery)
        
//  Uma característica importante do jQuery é o "chaining" - encadeamento - que significa simplesmente que
//  todas as funções derivadas do jQuery retornam um objeto jQuery novamente. Isso faz o desenvolvedor escrever
//  menos código e melhora a performance, porque a parte mais custosa é sempre buscar pelos elementos no DOM,
//  e ao sempre retornar a seleção pronta, pode-se fazer várias coisas num mesmo conjunto de elementos sem
//  que seja necessária a re-avaliação do conjunto. Ou seja:
    
        $("div")                                    // seleciona todos os DIVs
            .addClass("escondido")                  // adiciona a classe 'escondido' para todos eles
            .hide()                                 // adiciona style=display:none para todos eles
            .children("a")                          // seleciona agora todos os links ("a") dentro desses DIVs (a seleção se modifica)
                .attr("title",    null)             // remove o atributo "title" dos links
                .attr("disabled", "disabled")       // adiciona o atributo "disabled" pra todos os links
                .removeClass("ativo")               // remove a classe "ativo" de todos os links.
                
//      O retorno de todas as funções: $, addClass, hide, children, attr, removeClass, é sempre o conjunto original nas
//      funções que não mudam a seleção (addClass, hide, attr, removeClass) ou o novo conjunto, nas funções que mudam a
//      seleção (children).
        
//      Fora a própria função jQuery, as funções que mudam a seleção mais utilizadas são as de "trasversing", ou seja,
//      as de "caminhar" pelo DOM HTML, para que você consiga afetar elementos acima ou abaixo dos elementos atuais. 
        
//      Considerando o HTML abaixo:

            <form>
                <ul id="menu">
                    <li><a href="#">link 1</a>
                    <li><a href="#">link 2</a>
                    <li><a href="#">link 3</a>
                </ul>
                
                <div class="painel">
                    <p title='primeiro'>Primeiro Painel</p>
                </div
                
                <div class="painel">
                    <p title='segundo'>Segundo Painel</p>
                </div
            </form>
                
        //Podemos caminhar pela estrutura da seguinte maneira:
        
            $("#menu li a")                                 // seleciona todos os links dentro dos LIs do UL (3 elementos)
                .parent()                                   // "sobe" pros LIs (3 elementos)
                    .closest("form")                        // "sobe" até o form (1 elemento)
                        .children(".painel")                // "desce" pros elementos que tem a classe "painel" (2 DIVs)
                            .find("[title='segundo']")      // "desce" pro elemento que têm atributo title = "segundo" (1 P)
        
//
//      Detalhe:
//          
//          parent() e children() "sobem" e "descem" um nível apenas. 
//          
//              - No caso acima, $("form").children("p") não retorna nada porque os Ps são filhos dos DIVs
//              
//          closest() e find() "sobem" e "descem" até encontrar um elemento que satisfaça a seleção
//          
//              - Acima, $("p[title='primeiro']").closest("form") sobe até o form.
//              
//          next() e previous() caminham no mesmo nível.
//          
//              - Acima, $("ul").next() acha o primeiro div.painel


// Javascript / JSON
//
//   Algumas características importantes para lembrar sobre o javascript:
//  
//  - Os tipos são "fracos", quaisquer variáveis podem assumir quaisquer valores, independentes do tipo.
//    
//    Um caso comum onde nós utilizamos isso é para "economizar" variáveis na tipagem das coisas, especialmente
//    parâmetros. Um exemplo:
      
      function FazAlgumaCoisa(pParametro)
      {
          pParametro = $(pParametro)            // assim, "convertemos" o parâmetro para jQuery se ele veio como 
                                                // objeto ou como jQuery mesmo. Não precisamos de outra variável.
      }
      
      
//  - Objetos aceitam propriedades e funções "à vontade"
    
      var lObjeto = new Object();
      
      lObjeto.Nome         = "Luciano";
      lObjeto.SobreNome    = "Leal";
      lObjeto.NomeCompleto = function() { return this.Nome + " " + this.SobreNome; };
      
//  - Objetos podem ser definidos utilizando "Javascript Object Notation" - JSON:
    
      var lObjeto = { Nome: "Luciano", SobreNome: "Leal", NomeCompleto: function() { return this.Nome + " " + this.SobreNome; }}
      
//    Isso é muito utilizado pelo jQuery para passagem de parâmetros. Muitas funções aceitam um parametro "options"
//    que deve ser passado como um objeto javascript, então se utiliza o JSON:
      
        $("#pnlMensagemAdicional")          // seleciona o elemento que tem id = "pnlMensagemAdicional"
            .css({                          // css é a função que coloca valores no atributo "style" do elemento, aceita como 
                                            // parâmetro um objeto, cuja definição começa com {
                      width:   100          // propriedade "width" do objeto = 100
                    , heigth:  60           // propriedade "height" do objeto = 60
                    , opacity: 0.5          // propriedade "opacity" do objeto = 0.5
                 });                        // fecha o objeto e a função.
        
        
//    Os objetos podem ser serializados e deserializados em JSON para string com as funções:
      
        $.toJSON(   "{width: 100, height: 60, opacity: 0.5}" );     //retorna o objeto com as propriedades
        $.evalJSON(  {width: 100, height: 60, opacity: 0.5}  );     //retorna a string que representa o objeto
      
//      Obs: as funções não são serializadas, somente as propriedades.
    
//  - Funções podem ser usadas como variáveis
    
//    Passar funções como variáveis é muito comum especialmente para as chamadas ajax, onde quase sempre uma
//    função deve ser executada após a chamada ajax voltou do servidor (que, por sua natureza asíncrona, pode ser
//    a qualquer momento, ou nunca - timeout)
      
//    Para passar uma função, deve-se utilizar o nome somente (sem parênteses) ou uma função anônima. Quem CHAMA deve
//    saber a "assinatura" dos métodos, para chamar corretamente. Geralmente se usa uma função sem métodos, mas em alguns
//    casos, parâmetros são passados (no caso de uma chamada ajax de sucesso, pode-se passar como parâmetro o resultado
//    da chamada).
      
//    Exemplo:
      
        function GradIntra_CarregarJsonVerificandoErro(pUrl, pDadosDoRequest, pCallBackDeSucesso, pOpcoesPosCarregamento)
        {
            // pCallBackDeSucesso é a função pra ser executada depois que o ajax voltou com sucesso. Ela deve esperar que 
            // GradIntra_CarregarHtmlVerificandoErro irá executá-la passando como parâmetro o objeto de resposta que veio
            // da chamada ajax.
            
            
            $.ajax({                                                // função ajax do jQuery
                      url:      pUrl                                // url para enviar o request
                    , type:     "post"                              // método que ele vai usar pra enviar o request
                    , cache:    false                               // adiciona um parâmetro aleatório para que o IE não responda do cache
                    , dataType: "json"                              // irá serializar a resposta como objeto javascript automaticamente.
                    , data:     pDadosDoRequest                     // objeto de variáveis para enviar pro request
                    , success:  pCallBackDeSucesso                  // função de CallBack para ser executada se o request retornar status 200 (ok)
                    , error:    GradIntra_TratarRespostaComErro     // função de CallBack para ser executada se o request retornar status != 200
                   });
        }
    
//  - Se uma coisa existe, é verdade.
    
//    Ou seja, para testar se algo não é "undefined", basta testar por if(algo). Isso é muito utilizado
//    quando pegamos um atributo, e pode ser que ele nem exista no elemento em questão; se não existir,
//    ele vem como "undefined" e tentar mexer com ele vai sempre retornar erro. Assim, fazemos:
      
        var lTitulo = $("#elemento").attr("title");
        
        if(lTitulo)
        {
            // se ele tinha um título, qualquer que fosse o valor, esse if retorna true. Se for "undefined", retorna false.
        }
        
//      Mas como o atributo que queremos pode existir e estar vazio, é comum usarmos o if assim:
        
        if(lTitulo && lTitulo != "")
        {
            //aí tem alguma coisa mesmo.
        }
        
//3. Técnicas comuns utilizadas na solution

//  - Marcação de estado dos objetos com classes que não têm função visual
    
//      Em determinados casos, é importante marcar o estado de um elemento HTML. Para isso, basta adicionar uma
//      classe específica que seja descritiva e não tenha necessariamente valor visual. Para isso, usamos as funções
//      addClass e removeClass, em conjundo com a função hasClass para verificar o estado. Exemplo:
        
            
            // ao clicar em um item do menu, verificamos se ele está carregando conteúdo, se tiver, retornamos a função:
            
            if(lDiv.hasClass("Carregando_Conteudo") return;
        
            // se passar, marcamos o div como "carregando_conteudo":
            
            lDiv.addClass("Carregando_Conteudo");
            
            // no final do carregamento, removemos a classe:
            
            lDiv.removeClass("Carregando_Conteudo");
            
//      Adicionar e remover classes adicionais não causa efeito nenhum nos elementos.

//  - "DataBind" javascript usando um atributo próprio.
    
//      Essa é uma solução simples para fazer o "DataBind" de elementos HTML com um objeto JSON. Utiliza-se um
//      atributo particular (no nosso caso, o atributo chama-se "Propriedade") para marcar qual propriedade do 
//      objeto está atrelado ao elemento. Depois, existem duas funções que fazem a "transferência" dos valores:
//      uma para transferir do formulário pro objeto, e outra para fazer o inverso. Assim:

            <div class="painel">

                <input type="text" Propriedade="Nome" />
                <input type="text" Propriedade="SobreNome" />

                <input type="checkbox" Propriedade="Ativo" />

                <select Propriedade="Sexo">
                    <option value="M">Masculino</option>
                    <option value="F">Feminino</option>
                </select>

            </div>

//      As funções de "transferência" são GradIntra_InstanciarObjetoDosControles e GradIntra_ExibirObjetoNoFormulario:

            var lCliente = GradIntra_InstanciarObjetoDosControles("div.painel");

            alert(lCliente.Nome);       // o valor que estava no input

            // a função vê todos os elementos que têm o atributo "Propriedade" e atribui o valor do elemento ao objeto.

            GradIntra_ExibirObjetoNoFormulario(lCliente, "div.painel");

            // a função vê todos os elementos que têm o atributo "Propriedade" e atribui o valor da propriedade no objeto ao elemento.

//      Para ver como o databind funciona, ver o js 01-GradIntra-Principal.js

//4. Padronização e Formatação

//  No javascript, a padronização de código se faz muito mais importante porque não temos tipagem forte, nem capacidade
//  para refactoring automático. Assim, atentar para as seguintes normas:
    
//      Organização:
        
//          - Dentro da pasta /Lib/Gradual/Dev/ temos arquivos js de biblioteca geral, independente do projeto da
//              Intranet. 00-GradAuxiliares.js têm funções auxiliares e 01-GradSettings.js é a biblioteca de settings
//              de usuário gravadas em cookie.
                
//          - Dentro da pasta /Paginas/Dev/ temos os js da Intranet em geral, com subpastas para cada projeto (SAC, Seguranca,
//              Risco, etC)
                
//          - Os arquivos js têm numeração na frente porque posteriormente será utilizado um sistema de minimização que 
//              ordena os arquivos pelo nome, então os números forçam a ordem correta dos arquivos
    
//      Padronização:
        
//          - Atentar para os nomes das variáveis serem descritivos, completos, em português, capitalizados, e com underscore
//              somente para separar hierarquia, como os namespaces.
                
//          - Utilizar os prefixos "p" para parâmetro, "l", para local e "g" para global
    
//          - Utilizar o "namespace" completo em cada função, para podermos saber de onde vem cada uma delas:
            
                GradIntra_Projeto_Modulo_Funcao
                
//              ex.:
                    GradIntra_ExibirMensagem(...                    // função "global" do projeto Intranet
                    GradIntra_Clientes_CarregarFormulario(...   // função do projeto "SAC", módulo "Cadastro"
                    
//          - Utilizar o mesmo nome da função original com sufixo _CallBack para funções callback.
            
//              ex.:
                    GradIntra_SAC_Busca_EfetuarBusca(...
                    GradIntra_SAC_Busca_EfetuarBusca_Callback(...
            
//          - Utilizar as tags de "link" de jasvascript apropriadas para o Visual Studio reconhecer funções
//              em arquivos diferentes
                
//              ex.:
                    /// <reference path="../../../../Lib/Gradual/Dev/00-GradAuxiliares.js" />
                    
//              Porém, só utilizar as referências pertinentes porque isso faz o Visual Studio ler demorar mais
//              para abrir o arquivo (ele lê todas as referências antes)
            
//          - Utilizar os comentários de função para que o Visual Studio possa dar informações no auto-complete,
//              procurar observar a padronização e nomes de parâmetros com descrições semelhantes:
                
                function GradIntra_Busca_SelecionarClientesIndividualmente(pTR)
                {
                ///<summary>Seleciona como clientes para edição todas as linhas marcadas da tabela de resultados.</summary>
                ///<param name="pTR" type="Objeto_jQuery">(opcional) Array de TRs pré-selecionado, quando a funcão é chamada automaticamente de GradIntra_Busca_MarcarDesmarcarTR.</param>
                ///<returns>false</returns>
                        
                function GradIntra_SAC_Busca_EfetuarBusca_Callback(pResposta)
                {
                ///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_SAC_Busca_EfetuarBusca: faz o "databind" da tabela de resultados.</summary>
                ///<param name="pResposta" type="Objeto_JSON">Objeto "ChamadaAjaxResponse" que retornou do code-behind.</param>
                ///<returns>void</returns>
                
                function GradIntra_Clientes_CarregarFormulario(pGrupo, pPagina, pDadosDoRequest)
                {
                ///<summary>Exibe um formulário dentro do painel pnlFormulario_Dados, verificando se é necessário fazer request ou não.</summary>
                ///<param name="pGrupo"  type="String">Grupo a qual o formulário pertence, 'Acoes' ou 'Dados'. É composto na URL da chamada ajax.</param>
                ///<param name="pPagina" type="String">Página que será chamada. Em caso de 'DadosBasicos' ou 'DadosCompletos' é adicionado o sufixo _PF ou _PJ dependendo do cliente atual.</param>
                ///<param name="pDadosDoRequest" type="Objeto_JSON">(opcional) Objeto de dados para a request ajax, é enviado completo para a chamada $.ajax(). Se não for passado, assume-se {Acao: 'CarregarHtml'}.</param>
                ///<returns>void</returns>
                            
//          - A exceção para os nomes das funções e do parâmetro são os "event handlers", que devem espelhar os do C#:
//              um nome de objeto aproximado (não necessariamente o ID porque o event handler geralmente é pra mais
//              de um objeto) com _Evento(pSender):
                
                function lnkMenuPrincipal_Click(pSender)
                {
                }
                                
                function lnkSubMenu_Cadastro_Busca_Click(pSender)
                {
                }
            
                function lstClientesSelecionados_LI_MouseEnter(pSender)
                {
                }
                
//              Obs.: Sempre que executar o "click" de um elemento tipo input ou button, retornar "false" para que a página
//                    não dê "post" automaticamente