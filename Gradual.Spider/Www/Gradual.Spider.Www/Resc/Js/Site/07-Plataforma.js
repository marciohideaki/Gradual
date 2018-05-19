/// <reference path="00-Base.js" />
function Plataforma_Salvar_Click(pSender) 
{
    var lObjetoJson =
    {
        CodigoPlataforma:       $(".Plataforma_Form_id").val(),
        
        CodigoSessaoCliente:                $(".Plataforma_Form_id_sessao_cliente").val(),
        AtivoSessaoCliente:                 $(".Plataforma_Form_ativo_sessao_cliente").is(":checked"),
        ValorSessaoCliente:                 $(".Plataforma_Form_vl_sessao_cliente").val(),
        CodigoPlataformaSessaoCliente:      $("#Plataforma_Form_id_sessao_plataforma_cliente").val(),

        CodigoSessaoRepassador:             $(".Plataforma_Form_id_sessao_repassador").val(),
        AtivoSessaoRepassador:              $(".Plataforma_Form_ativo_sessao_repassador").is(":checked"),
        ValorSessaoRepassador:              $(".Plataforma_Form_vl_sessao_repassador").val(),
        CodigoPlataformaSessaoRepassador:   $("#Plataforma_Form_id_sessao_plataforma_repassador").val(),

        CodigoSessaoMesa:                   $(".Plataforma_Form_id_sessao_mesa").val(),
        AtivoSessaoMesa:                    $(".Plataforma_Form_ativo_sessao_mesa").is(":checked"),
        ValorSessaoMesa:                    $(".Plataforma_Form_vl_sessao_mesa").val(),
        CodigoPlataformaSessaoMesa:         $("#Plataforma_Form_id_sessao_plataforma_mesa").val()
    };

    var lAcao = "Cadastrar";

    var lUrl = "CadastroPlataforma.aspx";

    var pDivDeFormulario = $("#pnlMensagem");

    var pOpcoesPosCarregamento = "";

    var lRisco =
    {
        Acao: lAcao,
        ObjetoJson: $.toJSON(lObjetoJson)
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lRisco, Plataforma_Salvar_Click_Callback, pOpcoesPosCarregamento, pDivDeFormulario)
    
    return false;
}



function Plataforma_Salvar_Click_Callback(pResposta, pDivFormulario)
{
    if (pResposta.TemErro) 
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else 
    {
        GradSpider_ExibirMensagemFormulario("A",pResposta.Mensagem, pDivFormulario) ;

        $("#ModalPlataforma").modal('hide');

        Plataforma_Listar_Plataforma_Sessao();
    }

    return false;
}

function Plataforma_Listar_Plataforma_Sessao()
{
    var lAcao = "Listar";

    var lUrl = "CadastroPlataforma.aspx";

    var pDivDeFormulario = $("#pnlMensagem");

    var pOpcoesPosCarregamento = "";

    var lListaPlataforma =
    {
        Acao: lAcao,
        //ObjetoJson: $.toJSON(lObjetoJson)
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lListaPlataforma, Plataforma_Listar_Plataforma_Sessao_Callback, pOpcoesPosCarregamento, pDivDeFormulario)
    
    return false;
}

function Plataforma_Listar_Plataforma_Sessao_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        var lLista = pResposta.ObjetoDeRetorno;

        $(".Body_Plataforma_Sessao").find("tr").remove();

        var lTbody = $(".Body_Plataforma_Sessao");

        for (i = 0; i < lLista.length; i++)
        {
            var lPlataforma = lLista[i];

            var lLinha   = "<tr>";
            lLinha      += "<td>"; 
            lLinha      += "<a href=\"#\" data-toggle=\"modal\" onclick='Plataforma_AbrirModal(" + lPlataforma.CodigoPlataforma + ")'>"+lPlataforma.NomePlataforma+"</td>";
            lLinha      += "<td>" + lPlataforma.NomeSessaoCliente        + "</td>";
            lLinha      += "<td>" + lPlataforma.NomeSessaoRepassador     + "</td>";
            lLinha      += "<td>" + lPlataforma.NomeSessaoMesa           + "</td>";
            lLinha      += "<td>" + lPlataforma.DataAtualizacao          + "</td>";
            lLinha      += "</tr>";

            lTbody.append(lLinha);
        }
    }
}

function Plataforma_Selecionar_Click(pSender)
{
    var lObjetoJson =
    {
        CodigoPlataforma:       $(".Plataforma_id").val(),
    };

    var lAcao = "SelecionaPlataforma";

    var lUrl = "CadastradoPlataforma.aspx";

    var pDivDeFormulario = $("#pnlMensagem");

    var pOpcoesPosCarregamento = "";

    var lPlataforma =
    {
        Acao      : lAcao,
        ObjetoJson: $.toJSON(lObjetoJson)
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lPlataforma, Plataforma_Selecionar_Click_Callback, pOpcoesPosCarregamento, pDivDeFormulario)
    return false;

}

function Plataforma_LimparModal()
{
    $(".Plataforma_Form_id")                    .val("");
        
    $(".Plataforma_Form_id_sessao_cliente")      .val("");
    $(".Plataforma_Form_vl_sessao_cliente")      .val("");
    $(".Plataforma_Form_ativo_sessao_cliente")   .prop('checked',true);
    $('.Plataforma_Form_ativo_sessao_cliente').parent('div').parent('div')
        .removeClass("bootstrap-switch-off")
        .addClass("bootstrap-switch-on");
    
    $(".Plataforma_Form_id_sessao_repassador")   .val("");
    $(".Plataforma_Form_vl_sessao_repassador")   .val("");
    $(".Plataforma_Form_ativo_sessao_repassador").prop('checked',true);
    $('.Plataforma_Form_ativo_sessao_repassador').parent('div').parent('div')
    .removeClass("bootstrap-switch-off")
        .addClass("bootstrap-switch-on");
    
    $(".Plataforma_Form_id_sessao_mesa")        .val("");
    $(".Plataforma_Form_vl_sessao_mesa")        .val("");
    $(".Plataforma_Form_ativo_sessao_mesa")     .prop('checked',true);
    $('.Plataforma_Form_ativo_sessao_mesa').parent('div').parent('div')
        .removeClass("bootstrap-switch-off").addClass("bootstrap-switch-on");
}

function Plataforma_AbrirModal(pIdPlataforma)
{
    var lUrl = "CadastroPlataforma.aspx";

    var pOpcoesPosCarregamento = "";

    var pDivDeFormulario = "";

     //Plataforma_LimparModal();

    if (pIdPlataforma != null)
    {
        var lPlataforma = { Acao: "Selecionar",  CodigoPlataforma : pIdPlataforma  };
        //var ObjetoJson = $.toJSON(lOperador);

        GradSpider_CarregarJsonVerificandoErro(lUrl, lPlataforma, Plataforma_AbrirModal_Callback, pOpcoesPosCarregamento, pDivDeFormulario);

    }else
    {
        Plataforma_LimparModal();
        $("#ModalPlataforma").modal('show');
    }
    
    return false;
}

function Plataforma_AbrirModal_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        var lRetorno = pResposta.ObjetoDeRetorno[0];

        //$(':checkbox').checkboxpicker();

        $(".Plataforma_Form_id")             .val(lRetorno.CodigoPlataforma);
        
        $(".Plataforma_Form_id_sessao_cliente")             .val(lRetorno.CodigoSessaoCliente);
        $(".Plataforma_Form_vl_sessao_cliente")             .val(lRetorno.ValorSessaoCliente);
        $("#Plataforma_Form_id_sessao_plataforma_cliente")  .val(lRetorno.CodigoPlataformaSessaoCliente);
        

        if (lRetorno.AtivoSessaoCliente.toLowerCase()=='true')
        {
            $('.Plataforma_Form_ativo_sessao_cliente').parent('div').parent('div')
                .removeClass("bootstrap-switch-off")
                .addClass("bootstrap-switch-on");

            $(".Plataforma_Form_ativo_sessao_cliente").attr('checked','checked');
        }else
        {
            $('.Plataforma_Form_ativo_sessao_cliente').parent('div').parent('div')
                .removeClass("bootstrap-switch-on")
                .addClass("bootstrap-switch-off");
            
            $(".Plataforma_Form_ativo_sessao_cliente").removeAttr('checked');
        }

        
        $(".Plataforma_Form_id_sessao_repassador")              .val(lRetorno.CodigoSessaoRepassador);
        $(".Plataforma_Form_vl_sessao_repassador")              .val(lRetorno.ValorSessaoRepassador);
        $("#Plataforma_Form_id_sessao_plataforma_repassador")   .val(lRetorno.CodigoPlataformaSessaoRepassador);

        if (lRetorno.AtivoSessaoRepassador.toLowerCase()=='true')
        {
            $('.Plataforma_Form_ativo_sessao_repassador').parent('div').parent('div')
            .removeClass("bootstrap-switch-off")
            .addClass("bootstrap-switch-on");

            $(".Plataforma_Form_ativo_sessao_repassador").attr('checked','checked');
        }
        else
        {
            $('.Plataforma_Form_ativo_sessao_repassador').parent('div').parent('div')
            .removeClass("bootstrap-switch-on")
            .addClass("bootstrap-switch-off");
            
            $(".Plataforma_Form_ativo_sessao_repassador").removeAttr('checked');
        }

        //$(".Plataforma_Form_ativo_sessao_repassador").prop('checked', lRetorno.AtivoSessaoRepassador.toLowerCase()==='true');
        
        $(".Plataforma_Form_id_sessao_mesa")            .val(lRetorno.CodigoSessaoMesa);
        $(".Plataforma_Form_vl_sessao_mesa")            .val(lRetorno.ValorSessaoMesa);
        $("#Plataforma_Form_id_sessao_plataforma_mesa") .val(lRetorno.CodigoPlataformaSessaoMesa);

        if (lRetorno.AtivoSessaoMesa.toLowerCase()=='true')
        {   
            $('.Plataforma_Form_ativo_sessao_mesa').parent('div').parent('div')
            .removeClass("bootstrap-switch-off")
            .addClass("bootstrap-switch-on");     
            $(".Plataforma_Form_ativo_sessao_mesa").attr('checked','checked');
        }else
        {
            
            $('.Plataforma_Form_ativo_sessao_mesa').parent('div').parent('div')
            .removeClass("bootstrap-switch-on")
            .addClass("bootstrap-switch-off");
            $(".Plataforma_Form_ativo_sessao_mesa").removeAttr('checked');
        }

        $("#ModalPlataforma").modal('show');
    }
}


//*************************************************************************************//
//**************Gerenciados de plataformas*********************************************//
//*************************************************************************************//

function Plataforma_Gerenciador_LimparModal()
{
    $(".Plataforma_Form_Id_Plataforma").each(function ()
    {
        var lCheckPLataforma = $(this)
        lCheckPLataforma.prop("checked",false);
    });

     $(".Plataforma_Form_Id_Sessao").val("");

    $(".Plataforma_Form_Id_Cliente").val("");
    $(".Plataforma_Form_Id_Assessor").val("");
    $(".Plataforma_Form_Id_Operador option:eq(0)").prop('selected',true);
}

function Plataforma_Gerenciador_AbrirModal(pIdTrader)
{
    var lUrl = "GerenciadorPlataformas.aspx";

    var pOpcoesPosCarregamento = "";

    var pDivDeFormulario = "";

     //Plataforma_LimparModal();

    if (pIdTrader != null)
    {
        var lPlataforma = { Acao: "Selecionar",  CodigoTrader : pIdTrader  };
        //var ObjetoJson = $.toJSON(lOperador);

        GradSpider_CarregarJsonVerificandoErro(lUrl, lPlataforma, Plataforma_Gerenciador_AbrirModal_Callback, pOpcoesPosCarregamento, pDivDeFormulario);

    }else
    {
        Plataforma_Gerenciador_LimparModal();
        $("#ModalGerenciadorPlataforma").modal('show');
    }

    var lAbaOperador = $(".AbaPlataformaOperador");
    var lAbaCliente  = $(".AbaPlataformaCliente");
    var lAbaAssessor = $(".AbaPlataformaAssessor");

    var lConteudoOperador = $("#operadorConteudo");
    var lConteudoCliente = $("#clienteConteudo");
    var lConteudoAssessor = $("#assessorConteudo");

    lConteudoOperador.css('display') == "block" ?lAbaOperador.show()   : lAbaOperador.hide()  ;
    lConteudoCliente .css('display') == "block" ?lAbaCliente .show()   : lAbaCliente .hide()  ;
    lConteudoAssessor.css('display') == "block" ?lAbaAssessor.show()   : lAbaAssessor.hide()  ;
    
    return false;
}

function Plataforma_Gerenciador_AbrirModal_Callback(pResposta)
{
    var lPlataforma = pResposta.ObjetoDeRetorno;

    Plataforma_Gerenciador_LimparModal();
    
    $("#ModalGerenciadorPlataforma").modal('show');

    for (i=0; i< lPlataforma.length;i++)
    {
        var lPla = lPlataforma[i];

        if (lPla.CodigoAcesso == "1")
        {
            $(".Plataforma_Form_Id_Cliente").val(lPla.CodigoTrader);
        }
        else if (lPla.CodigoAcesso == "2")
        {
            $(".Plataforma_Form_Id_Assessor").val(lPla.CodigoTrader);
        }
        else if (lPla.CodigoAcesso == "3")
        {
            $(".Plataforma_Form_Id_Operador").val(lPla.CodigoTrader);
        }

        if (lPla.CodigoSessao != "0" && lPla.CodigoSessao != "")
        {
            $(".Plataforma_Form_Id_Sessao").val(lPla.CodigoSessao);
        }
    }

    $(".Plataforma_Form_Id_Plataforma").each(function ()
    {
        var lCheckPLataforma = $(this)

        var lidPlataforma = $(this).val();

        for(i= 0; i <  lPlataforma.length; i++)
        {
            var IdPlataforma = lPlataforma[i].CodigoPlataforma;

            if (IdPlataforma == lidPlataforma)
            {
                lCheckPLataforma.prop("checked",true);
            }
        }
    });
}

function Plataforma_Listar_Gerenciador_Plataforma()
{
    var lAcao = "Listar";

    var lUrl = "GerenciadorPlataformas.aspx";

    var pDivDeFormulario = $("#pnlMensagem");

    var pOpcoesPosCarregamento = "";

    var lListaPlataforma =
    {
        Acao: lAcao,
        //ObjetoJson: $.toJSON(lObjetoJson)
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lListaPlataforma, Plataforma_Listar_Gerenciador_Plataforma_Callback, pOpcoesPosCarregamento, pDivDeFormulario)
    
    return false;
}

function Plataforma_Listar_Gerenciador_Plataforma_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        var lLista = pResposta.ObjetoDeRetorno;

        $(".Body_Plataforma_Cliente").find("tr").remove();
        $(".Body_Plataforma_Assessor").find("tr").remove();
        $(".Body_Plataforma_Operador").find("tr").remove();

        for (i = 0; i < lLista.length; i++)
        {
            var lPlataforma = lLista[i];

            if (lPlataforma.CodigoAcesso == "1")
            {
                var lTbody = $(".Body_Plataforma_Cliente");

                var lLinha   = "<tr>";
                lLinha      += "<td>"+lPlataforma.CodigoTrader+"</td>";
                lLinha      += "<td>"+lPlataforma.NomePlataforma+"</td>";
                lLinha      += "<td>"+lPlataforma.NomeSessao+"</td>";
                lLinha      += "<td><a href=\"#\" data-toggle=\"modal\" onclick='Plataforma_Gerenciador_AbrirModal(" + lPlataforma.CodigoTrader + ")'>Detalhes</a></td>";
                lLinha      += "</tr>";

                lTbody.append(lLinha);
            }
            else if (lPlataforma.CodigoAcesso == "2")
            {
                var lTbody = $(".Body_Plataforma_Assessor");
                
                var lLinha   = "<tr>";
                lLinha      += "<td>"+lPlataforma.CodigoTrader+"</td>";
                lLinha      += "<td>"+lPlataforma.NomePlataforma+"</td>";
                lLinha      += "<td>"+lPlataforma.NomeSessao+"</td>";
                lLinha      += "<td><a href=\"#\" data-toggle=\"modal\" onclick='Plataforma_Gerenciador_AbrirModal(" + lPlataforma.CodigoTrader + ")'>Detalhes</a></td>";
                lLinha      += "</tr>";

                lTbody.append(lLinha);
            }
            else if (lPlataforma.CodigoAcesso == "3")
            {
                var lTbody = $(".Body_Plataforma_Operador");

                var lLinha   = "<tr>";
                lLinha      += "<td>"+lPlataforma.CodigoTrader+"</td>";
                lLinha      += "<td>"+lPlataforma.NomePlataforma+"</td>";
                lLinha      += "<td>"+lPlataforma.NomeSessao+"</td>";
                lLinha      += "<td><a href=\"#\" data-toggle=\"modal\" onclick='Plataforma_Gerenciador_AbrirModal(" + lPlataforma.CodigoTrader + ")'>Detalhes</a></td>";
                lLinha      += "</tr>";

                lTbody.append(lLinha);
            }
        }
    }
}

function Plataforma_Gerenciador_Salvar_Click(pSender)
{
    var lConteudoOperador = $("#operadorConteudo");
    var lConteudoCliente  = $("#clienteConteudo");
    var lConteudoAssessor = $("#assessorConteudo");

    var lCodigoAcesso = 1;
    var lCodigoTrader = $(".Plataforma_Form_Id_Cliente")     .val();
    
    if (lConteudoOperador.css('display') == "block")
    {
        lCodigoAcesso = 3;
        lCodigoTrader=      $(".Plataforma_Form_Id_Operador").val();
    }   
    else if (lConteudoAssessor.css('display') == "block")
    {
        lCodigoAcesso = 2;
        lCodigoTrader=    $(".Plataforma_Form_Id_Assessor")  .val();
    }
    
    var lCodigosPlataforma=[];

    if (lCodigoTrader == "0" || lCodigoTrader == "")
    {
        alert("Informe o código do trader -> Operador ou Cliente ou Assessor");
        return false;
    }

    $(".Plataforma_Form_Id_Plataforma").each(function ()
    {
        var lCheckPLataforma = $(this)

        if (lCheckPLataforma.is(":checked"))
        {
            var lidPlataforma = $(this).val();

            lCodigosPlataforma.push(lidPlataforma);
        }
    });
    
    var lCodigoSessao = $(".Plataforma_Form_Id_Sessao").val();

    if (lCodigoSessao == "0") lCodigoSessao = null;

    var lObjetoJson = 
    {
        CodigoTrader:       lCodigoTrader,
        CodigoPlataformas:  lCodigosPlataforma,
        CodigoAcesso:       lCodigoAcesso,
        CodigoSessao:       lCodigoSessao
    };

    var lAcao = "Cadastrar";

    var lUrl = "GerenciadorPlataformas.aspx";

    var pDivDeFormulario = $("#pnlMensagem");

    var pOpcoesPosCarregamento = "";

    var lListaPlataforma =
    {
        Acao: lAcao,
        ObjetoJson: $.toJSON(lObjetoJson)
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lListaPlataforma, Plataforma_Gerenciador_Salvar_Click_Callback, pOpcoesPosCarregamento, pDivDeFormulario)

    return false;
}

function Plataforma_Gerenciador_Salvar_Click_Callback(pResposta, pDivDeFormulario)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        GradSpider_ExibirMensagemFormulario("A",pResposta.Mensagem, pDivDeFormulario) ;

        $("#ModalGerenciadorPlataforma").modal('hide');   

        Plataforma_Listar_Gerenciador_Plataforma();
    }
}

