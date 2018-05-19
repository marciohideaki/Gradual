/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />
/// <reference path="05-GradIntra-Relatorio.js" />

var gGradIntra_Sistema_TipoDeObjetoAtual = "";

var gGradIntra_Sistema_RespostaConsulta_MigracaoEntreAssessores = null;

var gGradIntra_Sistema_ImportacaoPepAtual = null;

function GradIntra_Sistema_AoSelecionarSistema()
{
    if(gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_VARIAVEIS
    || gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_PESSOASVINCULADAS
    || gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_PESSOASEXPOSTASPOL
    || gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_AUTORIZACOES)
    {
        //$("#pnlBusca_Sistema").hide();

        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;
    }

    GradIntra_Sistema_ExibirConteudo();
}

function GradIntra_Sistema_ExibirConteudo()
{
    if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_VARIAVEIS) {
        if (!$("#pnlSistema_VariaveisDoSistema").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) {
            GradIntra_CarregarHtmlVerificandoErro("Sistema/VariaveisDoSistema.aspx"
                                                  , null
                                                  , $("#pnlSistema_VariaveisDoSistema")
                                                  , GradIntra_Sistema_ExibirConteudo_CallBack
                                                 );
        }
        else {
            GradIntra_Sistema_ExibirConteudo_CallBack();
        }
    } else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_VALIDACAOROCKET) 
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

        $("#pnlConteudo_Sistema_ValidacaoCadastralRocket")
            .show()
            .find(".pnlFormularioExtendido")
                .show()
                .addClass(CONST_CLASS_CARREGANDOCONTEUDO);

        GradIntra_Navegacao_CarregarFormulario("Sistema/ClienteValidacaoRocket.aspx", "pnlSistema_ValidacaoCadastralRocket", null, null);
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_IMPORTACAO) 
    {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

        $("#pnlConteudo_Sistema_Importacao")
            .show()
            .find(".pnlFormularioExtendido")
                .show()
                .addClass(CONST_CLASS_CARREGANDOCONTEUDO);

        GradIntra_Navegacao_CarregarFormulario("Clientes/Formularios/ImportarClienteDoSinacor.aspx", "pnlClientes_Importacao", null, null);
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_LINK_PROSPECT) {
        gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;

        $("#pnlConteudo_Sistema_LinkProspect")
            .show()
            .children(".pnlFormulario")
                 .show()
                 .parent()
            .find(".pnlFormularioExtendido")
                 .addClass(CONST_CLASS_CARREGANDOCONTEUDO);

        GradIntra_Navegacao_CarregarFormulario("Clientes/Formularios/LinkParaProspect.aspx", "pnlClientes_LinkProspect", { Acao: 'CarregarHtmlComDados' }, null);
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_PESSOASVINCULADAS) {
        if (!$("#pnlSistema_PessoasVinculadas").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) {
            GradIntra_CarregarHtmlVerificandoErro("Sistema/PessoasVinculadas.aspx"
                                                  , null
                                                  , $("#pnlSistema_PessoasVinculadas")
                                                  , GradIntra_Sistema_ExibirConteudo_CallBack
                                                  , { CustomInputs: ["input[type='checkbox']"] }
                                                 );
        }
        else {
            GradIntra_Sistema_ExibirConteudo_CallBack();
        }
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_PESSOASEXPOSTASPOL) {
        GradIntra_CarregarHtmlVerificandoErro("Sistema/PessoasExpostasPoliticamente.aspx"
                                                , null
                                                , $("#pnlSistema_PessoasExpostasPoliticamente")
                                                , GradIntra_Sistema_ExibirConteudo_CallBack
                                                , { CustomInputs: ["input[type='checkbox']"] }
                                                );
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_AUTORIZACOES) {
        if (!$("#pnlSistema_AutorizacoesDeCadastro").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) {
            GradIntra_CarregarHtmlVerificandoErro("Sistema/Autorizacoes.aspx"
                                                  , null
                                                  , $("#pnlSistema_AutorizacoesDeCadastro")
                                                  , GradIntra_Sistema_ExibirConteudo_CallBack
                                                  , { CustomInputs: ["input[type='checkbox']"] }
                                                 );
        }
        else {
            GradIntra_Sistema_ExibirConteudo_CallBack();
        }
    }
    else {
        if (!$("#pnlSistema_ObjetosDoSistema").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) {
            gGradIntra_Sistema_TipoDeObjetoAtual = $("#cboBusca_Sistema_ObjetosDoSistema").val();

            if (!gGradIntra_Sistema_TipoDeObjetoAtual) {
                gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = false;
            }

            if (gGradIntra_Sistema_TipoDeObjetoAtual != "") {

                var lDados = { Acao: "CarregarHtmlComDados", Tipo: gGradIntra_Sistema_TipoDeObjetoAtual };

                $("#pnlSistema_ObjetosDoSistema")
                    .parent()
                        .show();

                GradIntra_Navegacao_ExibirFormulario("ObjetosDoSistema.aspx", GradIntra_Sistema_ExibirConteudo_CallBack, lDados);
            }
        }
        else {
            GradIntra_Sistema_ExibirConteudo_CallBack();
        }
    }
}

function GradIntra_Sistema_ExibirConteudo_CallBack()
{
    if(gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_VARIAVEIS)
    {
        $("#pnlSistema_VariaveisDoSistema").addClass(CONST_CLASS_CONTEUDOCARREGADO).show().parent().show();
        $("#pnlSistema_ObjetosDoSistema").hide();
        $("#pnlSistema_PessoasVinculadas").hide();
        $("#pnlSistema_PessoasExpostasPoliticamente").hide();
        $("#pnlSistema_AutorizacoesDeCadastro").hide();
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_PESSOASVINCULADAS)
    {
        $("#pnlSistema_VariaveisDoSistema").hide();
        $("#pnlSistema_ObjetosDoSistema").hide();
        $("#pnlSistema_PessoasVinculadas").addClass(CONST_CLASS_CONTEUDOCARREGADO).show().parent().show();
        $("#pnlSistema_PessoasExpostasPoliticamente").hide();
        $("#pnlSistema_AutorizacoesDeCadastro").hide();
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_PESSOASEXPOSTASPOL)
    {
        $("#pnlSistema_VariaveisDoSistema").hide();
        $("#pnlSistema_ObjetosDoSistema").hide();
        $("#pnlSistema_PessoasVinculadas").hide();
        $("#pnlSistema_PessoasExpostasPoliticamente").show().parent().show();
        $("#pnlSistema_AutorizacoesDeCadastro").hide();

        GradIntra_Sistema_PEP_ConfigurarGrid();
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_MIGRACAO)
    {
        $("#pnlConteudo_Sistema_Migracao")
                                    .show()
                                    .children().show()
                                        .find("#pnlClientes_MigracaoEntreAssessores").show()
    }
    else if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_AUTORIZACOES)
    {
        $("#pnlConteudo_Sistema_AutorizacoesDeCadastro").show();

        $("#pnlSistema_VariaveisDoSistema").hide();
        $("#pnlSistema_ObjetosDoSistema").hide();
        $("#pnlSistema_PessoasVinculadas").hide();
        $("#pnlSistema_PessoasExpostasPoliticamente").hide();
        $("#pnlSistema_PessoasExpostasPoliticamente").hide();
        $("#pnlSistema_AutorizacoesDeCadastro").show();

        $("#pnlSistema_AutorizacoesDeCadastro").find("input[type='checkbox']").customInput();
    }
    else
    {
        $("#pnlSistema_VariaveisDoSistema").hide();
        $("#pnlSistema_PessoasVinculadas").hide();
        $("#pnlSistema_PessoasExpostasPoliticamente").hide();
        $("#pnlSistema_ObjetosDoSistema")
            .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
            .find("p[class!='BotoesSubmit']")
                .hide()
                .parent()
            .find("p[class='Form_" + gGradIntra_Sistema_TipoDeObjetoAtual + "']")
                .show()
            .closest(".Conteudo_Container")
                .show();
    }
}

function GradIntra_Sistema_UploadDeArquivo_CallBack(pResposta) 
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);

    var lUrl = "\'Sistema/ObjetosDoSistema.aspx\'";
    //adicionar na tabela:

    $("#tblObjetosdoSistema_ListaDeObjetos tbody")
        .append("<tr rel=\"" + pResposta.ObjetoDeRetorno.Id + "\">"
                + "    <td>" + pResposta.ObjetoDeRetorno.Descricao + "</td>"
                + "    <td>"
                + "        <button class=\"IconButton Excluir\" title=\"Excluir objeto\" onclick=\"return btnSistema_ObjetosDoSistema_Excluir_Click(" + lUrl + ", this)\" style=\"margin-top:2px;margin-bottom:2px\"><span>Excluir</span></button>"
                + "    </td>"
                + "</tr>"
               )
        .find("tr.NenumItem")
            .hide();

    GradIntra_AtivarTooltips($("#tblObjetosdoSistema_ListaDeObjetos tbody tr:last-child"));
}

function GradIntra_Sistema_IncluirObjetoDeSistema()
{
    if(gGradIntra_Sistema_TipoDeObjetoAtual == "Contratos")
    {
        var lUrl = "Sistema/ObjetosDoSistema.aspx?Acao=ReceberArquivo";

        lUrl += "&Termo=" + $("#txtObjetosDoSistema_Contratos_Termo").val();
        lUrl += "&Tipo="  + $("#cboObjetosDoSistema_Contratos_Tipo" ).val();

        $.ajaxFileUpload({
                             url:            lUrl
                            , secureuri:     false
                            , fileElementId: "txtObjetosDoSistema_Contratos_Arquivo"
                            , dataType:      "json"
                            , success:       GradIntra_Sistema_UploadDeArquivo_CallBack
                            , error:         function (data, status, e)
                                             {
                                                 alert(e);
                                             }
                         });
    }
    else
    {
        var lDados = { Acao: "Incluir", Tipo: gGradIntra_Sistema_TipoDeObjetoAtual, Id: "" ,PendenciaAutomatica : "false" };

        if(gGradIntra_Sistema_TipoDeObjetoAtual == "AtividadesIlicitas")
        {
            lDados.Id        = $("#cboObjetosDoSistema_AtividadesIlicitas_Atividade").val();
            lDados.Descricao = $("#cboObjetosDoSistema_AtividadesIlicitas_Atividade option:selected").html();
        }

        if(gGradIntra_Sistema_TipoDeObjetoAtual == "PaisesEmListaNegra")
        {
            lDados.Id        = $("#cboObjetosDoSistema_PaisesEmListaNegra_Pais").val();
            lDados.Descricao = $("#cboObjetosDoSistema_PaisesEmListaNegra_Pais option:selected").html();
        }

        if(gGradIntra_Sistema_TipoDeObjetoAtual == "TiposDePendenciaCadastral")
        {
            lDados.Id = "0";
            lDados.Descricao = $("#txtObjetosDoSistema_TiposDePendenciaCadastral_Descricao").val();
            lDados.PendenciaAutomatica = $("#chkObjetosDoSistema_AtivarInativar_TiposDePendenciaCadastral_Autimatica").is(":checked")
        }
        
        if(gGradIntra_Sistema_TipoDeObjetoAtual == "TaxasDeTermo")
        {
            lDados.Id = "0";
            lDados.ValorTaxa    = $("#txtObjetosDoSistema_TaxasDeTermo_ValorTaxa").val();
            lDados.ValorRolagem = $("#txtObjetosDoSistema_TaxasDeTermo_ValorRolagem").val();
            lDados.NumeroDias   = $("#txtObjetosDoSistema_TaxasDeTermo_NumeroDias").val();
        }

        if(lDados.Id != "")
        {
            var lRowItemExistente = $("#tblObjetosdoSistema_ListaDeObjetos tbody tr[rel='" + lDados.Id + "']")

            if(lRowItemExistente.length == 0)
            {
                GradIntra_CarregarJsonVerificandoErro(  "Sistema/ObjetosDoSistema.aspx"
                                                      , lDados
                                                      , GradIntra_Sistema_IncluirObjetoDeSistema_CallBack
                                                     );
            }
            else
            {
                GradIntra_ExibirMensagem("I", "Este objeto já está na lista.", true);
            }
        }
        else
        {
            GradIntra_ExibirMensagem("A", "Favor selecionar um item para adicionar", true);
        }
    }
}

function GradIntra_Sistema_IncluirObjetoDeSistema_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);

    var lUrl = "\'Sistema/ObjetosDoSistema.aspx\'";
    //adicionar na tabela:

    $("#tblObjetosdoSistema_ListaDeObjetos tbody")
        .append(  "<tr rel=\"" + pResposta.ObjetoDeRetorno.Id         + "\">" 
                + "    <td>"  + pResposta.ObjetoDeRetorno.Descricao  + "</td>"
                + "    <td>"
                + "        <button class=\"IconButton Excluir\" title=\"Excluir objeto\" onclick=\"return btnSistema_ObjetosDoSistema_Excluir_Click(" + lUrl + ", this)\" style=\"margin-top:2px;margin-bottom:2px\"><span>Excluir</span></button>"
                + "    </td>"
                + "</tr>"
               )
        .find("tr.NenumItem")
            .hide();

    GradIntra_AtivarTooltips(  $("#tblObjetosdoSistema_ListaDeObjetos tbody tr:last-child")  );
}

function GradIntra_Sistema_ExcluirObjetoDeSistema(pUrl, pSender) 
{
    var lTextoConfirm = "";

    if ("Contratos" == gGradIntra_Sistema_TipoDeObjetoAtual)
    {
        lTextoConfirm = "Caso este contrato tenha sido assinalado por um cliente este relacionamento será perdido após a exclusão.\n\nTem certeza que deseja excluir este item?";
    }
    else
    {
        lTextoConfirm = "Tem certeza que deseja excluir este item?";
    }

    if (confirm(lTextoConfirm))
    {
        gGradIntra_Sistema_TipoDeObjetoAtual = $("#cboBusca_Sistema_ObjetosDoSistema").val();

        if(gGradIntra_Sistema_TipoDeObjetoAtual == "TaxasDeTermo")
        {
            alert("Não é possível excluir taxas de termo.");

            return false;
        }

        var lTR = $(pSender).closest("tr");

        var lTBody = lTR.closest("tbody");

        if ((lTR.attr("rel") + "") != "")
        {
            //o elemento tinha sido incluido na lista mas não foi no ajax (cliente novo)

            var lDados = new Object();

            lDados.Id = lTR.attr("rel");
            lDados.Acao = "Excluir";
            lDados.Tipo = gGradIntra_Sistema_TipoDeObjetoAtual;

            GradIntra_CarregarJsonVerificandoErro(pUrl
                                                  , lDados
                                                  , function (pResposta) { GradIntra_Sistema_ExcluirObjetoDeSistema_CallBack(pResposta, lTR); }
                                                 );
        }

        if (lTBody.children("tr").length == 2)
        {
            //esperando que haja 1 "Template" e 1 "Nenhuma"

            lTBody.find("tr.Nenhuma").show();
        }
    }

    return false;
}

function GradIntra_Sistema_ExcluirObjetoDeSistema_CallBack(pResposta, pTR)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);

    pTR.remove();
}

function GradIntra_Sistema_SalvarVariaveis()
{
    var lDados = 
    { 
        Acao: "SalvarVariaveis", 
        SalvarVariavel: $("#txtSistema_Variaveis_PeriodicidadeRenovacaoCadastral").val(), 
        Id: $("#hidSistema_Variaveis_PeriodicidadeRenovacaoCadastral_Id").val() 
    };

     GradIntra_CarregarJsonVerificandoErro(  "Sistema/VariaveisDoSistema.aspx"
                                          , lDados
                                          , function(){GradIntra_ExibirMensagem("A", "Variável alterada com sucesso.", true);}
                                          );
}

function GradIntra_Sistema_SalvarPessoasVinculadas(pDivDeFormulario)
{
    if (pDivDeFormulario.validationEngine({returnIsValid:true}))
    {
        var lDados = 
        { 
            Acao:                               "SalvarPessoasVinculadas", 
            CodigoCliente:                      $("#txtPessoasVinculadas_CodigoCliente").val(), 
            Nome:                               $("#txtPessoasVinculadas_Nome").val(),
            CPFCNPJ:                            $("#txtPessoasVinculadas_CPFCNPJ").val(),
            CodigoPessoaVinculadaResponsavel:    $("#txtPessoasVinculadas_CodigoPessoaVinculada").val(),
            FlagAtivo:                          $("#txtPessoasVinculadas_Ativo").is(":checked"),
            Id:                                 $("#hidSistema_PessoasVinculadas_Id").val() 
        };

        GradIntra_CarregarJsonVerificandoErro("Sistema/PessoasVinculadas.aspx", 
                                            lDados, 
                                            GradIntra_Sistema_IncluirPessoasVinculadas_CallBack);
    }
}

function GradIntra_Sistema_BuscarClientePessoasVinculadas()
{
    var spanResposta = $("#txtPessoasVinculadas_Nome_Cliente");

    if (!isNaN(parseInt($("#txtPessoasVinculadas_CodigoCliente").val(), 10)))    
    {
        var lDados = 
        {
            Acao:           "BuscaCliente",
            CodigoCliente: $("#txtPessoasVinculadas_CodigoCliente").val()
        };
    
        GradIntra_CarregarJsonVerificandoErro("Sistema/PessoasVinculadas.aspx", 
                                                lDados
                                                ,function (pResposta, spanResposta) {GradIntra_Sistema_BuscarClientePessoasVinculadas_CallBack(pResposta, spanResposta);}
                                                );
    }
}

function GradIntra_Sistema_BuscarClientePessoasVinculadas_CallBack(pResposta, spanResposta)
{
    $("#txtPessoasVinculadas_Nome_Cliente").html((pResposta.ObjetoDeRetorno == null) ? pResposta.Mensagem :pResposta.ObjetoDeRetorno.NomeCliente);
}

function GradIntra_Sistema_ExcluirPessoasVinculadas(pUrl, pSender)
{
    if (confirm("Tem certeza que deseja excluir esta pessoa?")) 
    {

        //gGradIntra_Sistema_TipoDeObjetoAtual = $("#cboBusca_Sistema_ObjetosDoSistema").val();
        var lTR = $(pSender).closest("tr");

        var lTBody = lTR.closest("tbody");

        if ((lTR.attr("rel") + "") != "") {
            //o elemento tinha sido incluido na lista mas não foi no ajax (cliente novo)

            var lDados = new Object();

            lDados.Id = lTR.attr("rel");
            lDados.Acao = "Excluir";

            GradIntra_CarregarJsonVerificandoErro(pUrl
                                                  , lDados
                                                  , function (pResposta) { GradIntra_Sistema_ExcluirPessoasVinculadas_CallBack(pResposta, lTR); }
                                                 );
        }

        if (lTBody.children("tr").length == 2) {
            //esperando que haja 1 "Template" e 1 "Nenhuma"

            lTBody.find("tr.Nenhuma").show();
        }
    }

    return false;
}

function GradIntra_Sistema_ExcluirPessoasVinculadas_CallBack(pResposta, pTR) 
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);

    if (pResposta.ObjetoDeRetorno == null)
        pTR.remove();
    else
    {
        if (!pResposta.ObjetoDeRetorno)
            pTR.remove();
    }
}

function GradIntra_Sistema_IncluirPessoasVinculadas_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);

    var lUrl = "\'Sistema/PessoasVinculadas.aspx\'";
    //adicionar na tabela:

    $("#tblPessoaVinculadas_ListaDePessoasVinculadas tbody")
        .append(  "<tr rel=\"" + pResposta.ObjetoDeRetorno.CodigoPessoaVinculada   + "\">" 
                + "    <td>"   + pResposta.ObjetoDeRetorno.CodigoPessoaVinculada  + "</td>"
                + "    <td>"   + pResposta.ObjetoDeRetorno.CPFCNPJ  + "</td>"
                + "    <td>"   + pResposta.ObjetoDeRetorno.Nome  + "</td>"
                + "    <td>"   + pResposta.ObjetoDeRetorno.CodigoPessoaVinculadaResponsavel  + "</td>"
                + "    <td>"   + pResposta.ObjetoDeRetorno.CodigoCliente  + "</td>"
                + "    <td>"
                + "        <button class=\"IconButton Excluir\" title=\"Excluir objeto\" onclick=\"return btnSistema_PessoasVinculadas_Excluir_Click(" + lUrl + ", this)\" style=\"margin-top:2px;margin-bottom:2px\"><span>Excluir</span></button>"
                + "    </td>"
                + "</tr>"
               )
        .find("tr.NenumItem")
            .hide();

    GradIntra_AtivarTooltips(  $("#tblPessoaVinculadas_ListaDePessoasVinculadas tbody tr:last-child")  );
}

function GradIntra_Sistema_PEP_ConfigurarGrid()
{
    
    $("#tblPessoaExpostasPoliticamente_ListaDePessoas").jqGrid(
    {
        url:        "Sistema/PessoasExpostasPoliticamente.aspx?Acao=Paginar"
      , datatype:   "json"
      , mtype:      "GET"
      , colModel :  [ 
                        //{"Id","CodGradual","CodBovespa","CodBMF","NomeCliente","CPF","Status","Passo","DataCadastroString","FlagPendencia","DataNascimentoString","Email","Sexo","TipoCliente"}

                        { label:"Código",             name:"Id",             jsonmap:"Id",                  index:"Id",             width:70,   align:"right",  sortable:true }
                      , { label:"Documento",          name:"Documento",      jsonmap:"Documento",           index:"Documento",      width:120,  align:"right",  sortable:true  }
                      , { label:"Nome",               name:"Nome",           jsonmap:"Nome",                index:"Nome",           width:400,  align:"left",   sortable:true  }
                      , { label:"Data de Importação", name:"DataImportacao", jsonmap:"DataImportacao",      index:"DataImportacao", width:100,  align:"left",   sortable:false  }
                    ]
      , height:     470
      , pager:      "#pnlPessoaExpostasPoliticamente_ListaDePessoas_Pager"
      , rowNum:     50
      , sortname:   "id"
      , sortorder:  "desc"
      , viewrecords: true
      , gridview:    true   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader : {
                         root:        "Itens"
                       , page:        "PaginaAtual"
                       , total:       "TotalDePaginas"
                       , records:     "TotalDeItens"
                       , cell:        ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id:          "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
                     }
    }); 

    $("#tblPessoaExpostasPoliticamente_ListaDePessoas").setGridWidth(956);
}

function GradIntra_Sistema_PEP_Buscar()
{
    var lCPF = $("#txtPessoaExpostasPoliticamente_Buscar").val();

    if(lCPF != null && lCPF != "")
    {
        var lDados = {   Acao: "BuscarPEP", CPF: lCPF };

        GradIntra_CarregarJsonVerificandoErro(  "Sistema/PessoasExpostasPoliticamente.aspx"
                                                , lDados
                                                , GradIntra_Sistema_PEP_Buscar_CallBack);
    }
}

function GradIntra_Sistema_PEP_Buscar_CallBack(pResposta)
{
    if(pResposta.ObjetoDeRetorno == 0)
    {
        GradIntra_ExibirMensagem("A", pResposta.Mensagem);
    }
    else
    {
        var lGrid = $("#tblPessoaExpostasPoliticamente_ListaDePessoas")[0];
        lGrid.addJSONData(pResposta.ObjetoDeRetorno);
    }
}

function GradIntra_Sistema_PEP_EnviarEmail()
{
    var lPainelResultado = $("#pnlPessoaExpostasPoliticamente_Resultado");

    var lID = lPainelResultado.attr("PEP_ID");

    if(lID != null && lID != "")
    {
        var lNome, lDocumento;

        lNome      = lPainelResultado.attr("PEP_Nome");
        lDocumento = lPainelResultado.attr("PEP_Documento");

        if(confirm("Tem certeza que deseja enviar email ao compliance relativo à sr.(a)\r\n" + lNome + " (" + lDocumento + ")?"))
        {
            var lDados = {   Acao: "EnviarEmail"
                           , ID: lID
                           , Nome: lNome
                           , Documento: lDocumento };

            GradIntra_CarregarJsonVerificandoErro(  "Sistema/PessoasExpostasPoliticamente.aspx"
                                                    , lDados
                                                    , GradIntra_Sistema_PEP_EnviarEmail_CallBack);
        }
    }
}

function GradIntra_Sistema_PEP_EnviarEmail_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", pResposta.Mensagem);
}

function GradIntra_Sistema_PEP_EnviarArquivoParaImportacao()
{
    var lUrl = "Sistema/PessoasExpostasPoliticamente.aspx?Acao=ReceberArquivo";

    $.ajaxFileUpload({  
                         url:           lUrl
                       , secureuri:     false
                       , fileElementId: "txtPessoaExpostasPoliticamente_Upload"
                       , dataType:      "json"
                       , success:       GradIntra_Sistema_PEP_EnviarArquivoParaImportacao_CallBack
                       , error:         GradIntra_Sistema_PEP_EnviarArquivoParaImportacao_Erro
                     });
}

function GradIntra_Sistema_PEP_EnviarArquivoParaImportacao_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("A", pResposta.Mensagem);

    gGradIntra_Sistema_ImportacaoPepAtual = pResposta.ObjetoDeRetorno;

    gGradIntra_Sistema_ImportacaoPepAtual.Interval = window.setInterval(GradIntra_Sistema_PEP_VerificarImportacao, 2000);
}

function GradIntra_Sistema_PEP_VerificarImportacao()
{
    if(gGradIntra_Sistema_ImportacaoPepAtual != null)
    {
        var lDados = {  Acao:           "VerificarImportacao"
                      , IdDaImportacao: gGradIntra_Sistema_ImportacaoPepAtual.IdDaImportacao
                     };

        GradIntra_CarregarJsonVerificandoErro(  "Sistema/PessoasExpostasPoliticamente.aspx"
                                              , lDados
                                              , GradIntra_Sistema_PEP_VerificarImportacao_CallBack);
    }
}

function GradIntra_Sistema_PEP_VerificarImportacao_CallBack(pResposta)
{
    if(pResposta.Mensagem != "INEXISTENTE")
    {
        gGradIntra_Sistema_ImportacaoPepAtual = pResposta.ObjetoDeRetorno;

        if(gGradIntra_Sistema_ImportacaoPepAtual.StatusDaImportacao == "Finalizada")
        {
            GradIntra_ExibirMensagem("I", "Importacao finalizada com sucesso!", true);
            
            GradIntra_Sistema_PEP_FinalizarImportacao();
        }
        else if(gGradIntra_Sistema_ImportacaoPepAtual.StatusDaSincronizacao == "Com Erro")
        {
            GradIntra_ExibirMensagem("E", gGradIntra_Sistema_ImportacaoPepAtual.MensagemDeFinalizacao);
            
            GradIntra_Sistema_PEP_FinalizarImportacao();
        }
        else
        {
            GradIntra_ExibirMensagem("I", "Importação em andamento: [" + 
                                          gGradIntra_Sistema_ImportacaoPepAtual.RegistrosImportadosComSucesso + "] com sucesso, [" + 
                                          gGradIntra_Sistema_ImportacaoPepAtual.RegistrosComErro              + "] com erro", false);
        }
    }
    else
    {
        //failsafe se acontecer alguma coisa com o application e perder o ID
        window.clearInterval(gGradIntra_Sistema_ImportacaoPepAtual.Interval);
    }
}

function GradIntra_Sistema_PEP_FinalizarImportacao()
{
    window.clearInterval(gGradIntra_Sistema_ImportacaoPepAtual.Interval);

    gGradIntra_Sistema_ImportacaoPepAtual = null;

}

function GradIntra_Sistema_PEP_EnviarArquivoParaImportacao_Erro(data, status, e)
{
    GradIntra_ExibirMensagem("E", "Erro ao enviar arquivo", false, data + "\r\n\r\n" + status + "\r\n\r\n" + e);
}

var glastSel;

function GradIntra_Sistema_MigracaoEntreAssessores_ConfigurarGrid()
{
    $("#tblClientes_MigracaoEntreAssessores_ListaDeCliente").jqGrid(
    {
        url: "Clientes/Formularios/MigracaoEntreAssessores.aspx?Acao=Paginar&IdAssessor=" + $("#cboClientes_MigracaoEntreAssessores_AssessorDe").val()
      , datatype:   "json"
      , mtype:      "GET"
      , colModel:   [ { label:"Selecionar",       name:"StSelecionado",      jsonmap:"StSelecionado",      index:"StSelecionado",      align:"center", width:80,  sortable:true ,formatter: "checkbox", edittype:"checkbox", formatoptions: {disabled : false}, editable: true}
                    , { label: "Cód. Bolsa",      name: "CodBovespa",        jsonmap: "CodBovespa",        index: "CodBovespa",        align: "right", width: 70, sortable:true }
                    , { label:"Cód. Cliente",     name:"IdCliente",          jsonmap:"IdCliente",          index:"IdCliente",          align:"right",  width:80,  sortable:true }
                    , { label:"Nome",             name:"NomeCliente",        jsonmap:"NomeCliente",        index:"NomeCliente",        align:"left",   width:400, sortable:true }
                    , { label:"Tipo de Cliente",  name:"TipoCliente",        jsonmap:"TipoCliente",        index:"TipoCliente",        align:"center", width:80,  sortable:true }
                    , { label:"CPF / CNPJ",       name:"CPF",                jsonmap:"CPF",                index:"CPF",                align:"right",  width:120, sortable:true }
                    , { label:"Status",           name:"Status",             jsonmap:"Status",             index:"Status",             align:"center", width:70,  sortable:true }
                    , { label:"Data de Cadastro", name:"DataCadastroString", jsonmap:"DataCadastroString", index:"DataCadastroString", align:"center", width:120, sortable:true }
                    , { label:"CodBovespa",       name:"CodBovespaHidden",   jsonmap:"CodBovespa",         index:"CodBovespa",         align:"center", width:120, sortable:true }//, visible:false, hidden:true }
                    , { label:"CodSistema",       name:"CodSistema",         jsonmap:"CodSistema",         index:"CodSistema",         align:"center", width:120, sortable:true }//, visible:false, hidden:true }
                    ]
      , height: 470
      , width: 920
      , pager:      "#pnlClientes_MigracaoEntreAssessores_ListaDeCliente_Pager"
      , rowNum:     20
      , sortname:   "NomeCliente"
      , sortorder:  "asc"
      , viewrecords: true
      , gridview:    true   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader:  { root:        "Itens"
                     , page:        "PaginaAtual"
                     , total:       "TotalDePaginas"
                     , records:     "TotalDeItens"
                     , cell:        ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                     , id:          "0"              //primeira propriedade do elemento de linha é o Id
                     , repeatitems: false
                     }
      , onSelectRow: GradIntra_Sistema_MigracaoEntreAssessores_Checkbox_Click
      , gridComplete: Clientes_MigracaoEntreAssessores_ListaDeCliente_chkAssessor_Click
    }).jqGrid('hideCol', ['CodBovespaHidden', 'CodSistema']);

    $("#tblPessoaExpostasPoliticamente_ListaDePessoas").setGridWidth(956);
}

function GradIntra_Sistema_MigracaoEntreAssessores_Checkbox_Click(pIndiceLinhaSelecionada)
{
    var lCheck = $($(this).find("td:first-child input")[pIndiceLinhaSelecionada - 1]);

    lCheck.prop("checked", !lCheck.is(":checked"));

    $("#ckbClientes_MigracaoEntreAssessores_MigrarTodos")
                                   .prop("checked", false)
                                   .next()
                                         .removeClass("checked");

    if ($("#tblClientes_MigracaoEntreAssessores_ListaDeCliente input[type='checkbox']:checked").length > 0)
    {
        $("#pnlClientes_MigracaoEntreAssessores_FormularioPara").show();
    }
    else
    {
        $("#pnlClientes_MigracaoEntreAssessores_FormularioPara").hide();
    }
}

function Grad_Intra_Sistema_MigracaoEntreAssessores_Buscar(pSender)
{
    var lIdAssessor = $("#cboClientes_MigracaoEntreAssessores_AssessorDe").val();
    
    if (lIdAssessor != "")
    {
        var lDados = { Acao: "Paginar", IdAssessor: lIdAssessor, Page: 1 };
        
        GradIntra_CarregarHtmlVerificandoErro("Clientes/Formularios/MigracaoEntreAssessores.aspx"
                                              , null
                                              , $("#pnlClientes_MigracaoEntreAssessores")
                                              , function(pResposta) { GradIntra_Sistema_ExibirConteudo_CallBack(); Grad_Intra_Sistema_MigracaoEntreAssessores_Buscar_Callback(); }
                                              , { CustomInputs: ["input[type='checkbox']"] }
                                              );
    }
    else
    {
        GradIntra_ExibirMensagem("A", "Favor selecionar um assessor", true);
    }

    return false;
}

function Grad_Intra_Sistema_MigracaoEntreAssessores_Buscar_Callback(pResposta)
{
    GradIntra_Sistema_MigracaoEntreAssessores_ConfigurarGrid();
}

function GradIntra_Clientes_MigracaoEntreAssessores_RealizarMigracao()
{
    if ($("#ckbClientes_MigracaoEntreAssessores_MigrarTodos").is(":checked")
    && (!confirm("Esta ação irá realizar a migração de TODOS os clientes deste assessor " + $("#cboClientes_MigracaoEntreAssessores_AssessorDe :selected").html() + " para o assessor " + $("#cboClientes_MigracaoEntreAssessores_AssessorPara :selected").html() + ". Deseja continuar?")))
    {
        return false;
    }

    var lChecks = $("#tblClientes_MigracaoEntreAssessores_ListaDeCliente input[type='checkbox']:checked");

    if(lChecks.length > 0)
    {
        var lDados = { Acao                                : "RealizarMigracao"
                     , AssessorDe                          : $("#cboClientes_MigracaoEntreAssessores_AssessorDe").val()
                     , AssessorPara                        : $("#cboClientes_MigracaoEntreAssessores_AssessorPara").val()
                     , MigrarTodosOsClientes               : $("#ckbClientes_MigracaoEntreAssessores_MigrarTodos").is(":checked")
                     , IDsDosClientes                      : ""
                     , CdBmfBovespaDosClientesSelecionados : ""
                     , CdSistema                           : ""
                     };

        if( (lDados.AssessorDe   == "")
        ||  (lDados.AssessorPara == "")
        ||  (lDados.AssessorDe   == lDados.AssessorPara))
        {
            GradIntra_ExibirMensagem("A", "Assessores inválidos", true); //#JIC
        }
        else
        {
            var lCheck;

            lChecks.each(function()
            {
                lCheck = $(this);

                lDados.IDsDosClientes += $($(this).closest("tr").find("td")[2]).html() + ",";

                lDados.CdBmfBovespaDosClientesSelecionados += $($(this).closest("tr").find("td")[8]).html() + ","

                lDados.CdSistema += $($(this).closest("tr").find("td")[9]).html() + ","
            });

            GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/MigracaoEntreAssessores.aspx"
                                                 , lDados
                                                 , GradIntra_Clientes_MigracaoEntreAssessores_RealizarMigracao_CallBack);
        }
    }
    else
    {
        GradIntra_ExibirMensagem("A", "Favor escolher clientes para migrar", true); //#JIC
    }
}

function GradIntra_Clientes_MigracaoEntreAssessores_RealizarMigracao_CallBack(pResposta)
{
    GradIntra_ExibirMensagem("I", "Migração efetuada com sucesso!", true);

    Grad_Intra_Sistema_MigracaoEntreAssessores_Buscar(); //--> Pra atualizar os dados na tela.
}

function GradIntra_Clientes_MigracaoEntreAssessores_VerificarMarcados()
{
    var lChecks = $("#tblClientes_MigracaoEntreAssessores_ListaDeCliente tbody tr td:first-child input[type='checkbox']:checked");

    var lpnlAlertaSelecionar = $("#pnlClientes_MigracaoEntreAssessores_AlertaSelecionar");
    var lpnlFormularioPara = $("#pnlClientes_MigracaoEntreAssessores_FormularioPara");

    if(lChecks.length > 0)
    {
        lpnlAlertaSelecionar.hide();
        
        lpnlFormularioPara
            .find("label")
                .html("Migrar [" + lChecks.length + "] clientes para:")
                .parent()
            .show();
    }
    else
    {
        lpnlAlertaSelecionar.show();
        lpnlFormularioPara.hide();
    }
}

function ckbClientes_MigracaoEntreAssessores_MigrarTodos_Click(pSender)
{
    if ($(pSender).is(":checked"))
    {
        $("#pnlClientes_MigracaoEntreAssessores_FormularioPara").show();
        $("#tblClientes_MigracaoEntreAssessores_ListaDeCliente input[type='checkbox']").prop("checked", true);
    }
    else
    {
        $("#pnlClientes_MigracaoEntreAssessores_FormularioPara").hide();
        $("#tblClientes_MigracaoEntreAssessores_ListaDeCliente input[type='checkbox']").prop("checked", false);
    }
}

function Clientes_MigracaoEntreAssessores_ListaDeCliente_chkAssessor_Click(pRowId, pICol, pCellContent, pE)
{
    $("#tblClientes_MigracaoEntreAssessores_ListaDeCliente").find("input:checkbox").click(Clientes_MigracaoEntreAssessores_ListaDeCliente_Checkbox_Click);
}

function Clientes_MigracaoEntreAssessores_ListaDeCliente_Checkbox_Click()
{
    var lCheck = $(this);

    $("#ckbClientes_MigracaoEntreAssessores_MigrarTodos")
                                   .prop("checked", false)
                                   .next()
                                         .removeClass("checked");

    if ($("#tblClientes_MigracaoEntreAssessores_ListaDeCliente input[type='checkbox']:checked").length > 0)
    {
        $("#pnlClientes_MigracaoEntreAssessores_FormularioPara").show();
    }
    else
    {
        $("#pnlClientes_MigracaoEntreAssessores_FormularioPara").hide();
    }
}
/*
function btnAutorizarCadastro_Click(pSender, pTipo, pNumero)
{
    pSender = $(pSender);

    var lDados = { Acao: "AutorizarCadastro", IdCliente: pSender.closest("tr").attr("data-IdCliente"), Tipo: pTipo, Numero: pNumero } ;

    pSender.prop("disabled", true);

    pSender.closest("td").addClass("AguardandoAutorizacao");
    
    GradIntra_CarregarJsonVerificandoErro("Sistema/Autorizacoes.aspx", lDados, GradIntra_Sistema_AutorizarCadastro_CallBack);

    return false;
}*/


function chkAut_Click(pSender)
{
    pSender = $(pSender);

    if(pSender.is(":checked"))
    {
        var lId = pSender.attr("id");

        var lOutros = pSender.closest("tr").find("input[id!='" + lId + "']")

        lOutros.prop("checked", false).parent().find("label").removeClass("checked");
        //why not radios? Because TV killed the radio star. 
    }

    var lQtd = $("#tblAutorizacoes_ListaDeAutorizacoesPendentes tbody tr input:checked").length;

    if(lQtd == 0)
    {
        $("#btnAutorizarCadastros").prop("disabled", true).html("Autorizar");
    }
    else
    {
        $("#btnAutorizarCadastros").prop("disabled", false).html("Autorizar " + lQtd + " Cadastro" + ((lQtd == 1) ? "" : "s"));
    }
}

function btnAutorizarCadastros_Click(pSender)
{
    var lChecks = $("#tblAutorizacoes_ListaDeAutorizacoesPendentes tbody tr input:checked");

    var lDados = { Acao: "AutorizarCadastros", Codigos: "" };

    var lCheck;

    lChecks.each(function()
    {
        lCheck = $(this)

        lDados.Codigos += lCheck.attr("id").substr(7) + ",";

        lCheck.closest("td").addClass("AguardandoAutorizacao");
    });

    $("#btnAutorizarCadastros").prop("disabled", true);
    
    $("#tblAutorizacoes_ListaDeAutorizacoesPendentes input").prop("disabled", true);

    GradIntra_CarregarJsonVerificandoErro("Sistema/Autorizacoes.aspx", lDados, GradIntra_Sistema_AutorizarCadastro_CallBack);

    return false;
}

function GradIntra_Sistema_AutorizarCadastro_CallBack(pResposta)
{
    $("#btnAutorizarCadastros").prop("disabled", false).html("Autorizar");

    $("#tblAutorizacoes_ListaDeAutorizacoesPendentes input").prop("disabled", false);

    var lTDs = $("td.AguardandoAutorizacao");

    if(pResposta.Mensagem == "ok")
    {
        var lRespostas = pResposta.ObjetoDeRetorno;
        var lCods;
        for(var a = 0; a < lRespostas.length; a++)
        {
            lCods = lRespostas[a].split("_");

            var lTD = $("#tblAutorizacoes_ListaDeAutorizacoesPendentes tr[data-idcliente='" + lCods[0] + "'] td.AguardandoAutorizacao");

            try
            {
                if(lCods[1] == "S")
                {
                    lTD.closest("tr").remove();
                }
                else if(lCods[1] == "E")
                {
                    lTD.html("(erro)");
                }
                else
                {
                    lTD.html(lCods[1]);
                }

            }catch(erro){}
        }
    }
}

function btnSistema_Autorizacoes_Detalhes_Click(pSender)
{
    pSender = $(pSender);

    GradIntra_Sistema_BuscarDetalhes( pSender.closest("tr").attr("data-IdCliente") );

    return false;
}

function GradIntra_Sistema_BuscarDetalhes(pID)
{
    GradIntra_Navegacao_ExibirFormularioDeNovoItem("Clientes/Formularios/Acoes/VerFichaCadastral.aspx?Acao=CarregarHtmlComDados&Id=" + pID);
}

function chkAut_Todos_Click(pSender)
{
    pSender = $(pSender);

    var lIndex = pSender.closest("td").index();

    var lTRs = pSender.closest("table").find("tbody tr")
    var lTD;

    lTRs.each(function()
    {
        lTD = $(this).find("td:eq(" + lIndex + ")");

        if(pSender.is(":checked"))
        {
            lTD.find("input").prop("checked", true).parent().find("label").addClass("checked");
        }
        else
        {
            lTD.find("input").prop("checked", false).parent().find("label").removeClass("checked");
        }
    });

    
    var lQtd = $("#tblAutorizacoes_ListaDeAutorizacoesPendentes tbody tr input:checked").length;

    if(lQtd == 0)
    {
        $("#btnAutorizarCadastros").prop("disabled", true).html("Autorizar");
    }
    else
    {
        $("#btnAutorizarCadastros").prop("disabled", false).html("Autorizar " + lQtd + " Cadastro" + ((lQtd == 1) ? "" : "s"));
    }
}
