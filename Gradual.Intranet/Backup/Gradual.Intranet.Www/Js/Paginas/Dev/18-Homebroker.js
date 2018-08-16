/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />


function GradIntra_HomeBroker_AoSelecionarSistema()
{
    gGradIntra_Navegacao_SubSistemaAtualExibeBusca = false;
    
    if (gGradIntra_Navegacao_SubSistemaAtual == CONST_SUBSISTEMA_AVISOS)
    {
        if(!$("#pnlHomeBroker_Avisos").hasClass(CONST_CLASS_CONTEUDOCARREGADO))
        {
            GradIntra_CarregarHtmlVerificandoErro(  "HomeBroker/Avisos.aspx"
                                                  , null
                                                  , $("#pnlHomeBroker_Avisos")
                                                  , GradIntra_HomeBroker_ExibirConteudo_CallBack
                                                  , {
                                                        CustomInputs: ["input[type='checkbox']"]
                                                      , HabilitarMascaras: true 
                                                    }
                                                 );
        }
        else
        {
            GradIntra_HomeBroker_ExibirConteudo_CallBack();
        }
    }
    
}


function GradIntra_HomeBroker_ExibirConteudo_CallBack()
{
    $("#pnlHomeBroker_Avisos").addClass(CONST_CLASS_CONTEUDOCARREGADO).show().parent().show();
}

function GradIntra_HomeBroker_EditarAviso(pSender)
{
    var lID = $(pSender).closest("tr").attr("rel");

    var lDados = { Acao: "BuscarAviso", ID: lID };

    GradIntra_CarregarJsonVerificandoErro("HomeBroker/Avisos.aspx", lDados, GradIntra_HomeBroker_EditarAviso_CallBack, null, null);

    return false;
}


function GradIntra_HomeBroker_IncluirEditarAviso(pSender)
{
    var lAvisoObjeto = { CodigoAviso: 0, DataEntrada: "", DataSaida : "", Texto : "", FlagAtivacaoManual : "", CBLCs: "" };

    var lDados;

    //var lForm = $(pSender).closest("div.pnlFormulario_Campos");

    lAvisoObjeto.CodigoAviso = $("#hidHomeBroker_Aviso_Id").val();

    if(lAvisoObjeto.CodigoAviso == "" || lAvisoObjeto.CodigoAviso == null) lAvisoObjeto.CodigoAviso = 0;

    lAvisoObjeto.DataEntrada = $("#txtHomeBroker_Aviso_DataDeEntrada").val();

    lAvisoObjeto.DataSaida = $("#txtHomeBroker_Aviso_DataDeSaida").val();

    lAvisoObjeto.HoraEntrada = $("#txtHomeBroker_Aviso_HoraDeEntrada").val();

    lAvisoObjeto.HoraSaida = $("#txtHomeBroker_Aviso_HoraDeSaida").val();

    lAvisoObjeto.Texto = $("#txtHomeBroker_Aviso_Texto").val();

    lAvisoObjeto.CBLCs = $("#txtHomeBroker_Aviso_CBLCs").val();

    lAvisoObjeto.FlagAtivacaoManual = "N";//( $("#chkHomeBroker_Aviso_FlagAtivacaoManual").is(":checked") ) ? "S" : "N";

    lAvisoObjeto.IdSistema = $("#chkHomeBroker_Aviso_Sistema").val();

    lDados = { Acao: "SalvarAviso", ObjetoJson: $.toJSON( lAvisoObjeto ) };

    GradIntra_CarregarJsonVerificandoErro("HomeBroker/Avisos.aspx", lDados, GradIntra_HomeBroker_IncluirEditarAviso_CallBack, null, null);

    return false;
}


function GradIntra_HomeBroker_IncluirEditarAviso_CallBack(pResposta)
{
    var lCodigoAviso = $("#hidHomeBroker_Aviso_Id").val();

    if(pResposta.ObjetoDeRetorno.CodigoAviso + "" == lCodigoAviso)
    {
        //atualizou um item

        var lTDs = $("#tblAvisosHomeBroker_ListaDeAvisos tbody tr[rel='" + lCodigoAviso + "'] td");
        
        $(lTDs[1]).html( pResposta.ObjetoDeRetorno.DataEntrada + " " + pResposta.ObjetoDeRetorno.HoraEntrada );
        $(lTDs[2]).html( pResposta.ObjetoDeRetorno.DataSaida   + " " + pResposta.ObjetoDeRetorno.HoraSaida   );
        $(lTDs[3]).html( pResposta.ObjetoDeRetorno.TextoTruncado ).attr("title", pResposta.ObjetoDeRetorno.Texto);
        $(lTDs[4]).html( pResposta.ObjetoDeRetorno.FlagAtivacaoManual );
        $(lTDs[5]).html( pResposta.ObjetoDeRetorno.IdSistema );

        GradIntra_HomeBroker_LimparFormulario();
    }
    else
    {
        //incluiu um novo item

        GradIntra_ExibirMensagem("I", "Aviso cadastrado com sucesso.");

        var lTBody = $("#tblAvisosHomeBroker_ListaDeAvisos tbody");

        var lTR = " <tr rel='"        + pResposta.ObjetoDeRetorno.CodigoAviso  + "'>" +
                  "      <td>"        + pResposta.ObjetoDeRetorno.CodigoAviso  + "</td>" +
                  "      <td>"        + pResposta.ObjetoDeRetorno.DataEntrada  +   " "   + pResposta.ObjetoDeRetorno.HoraEntrada    + "</td>" +
                  "      <td>"        + pResposta.ObjetoDeRetorno.DataSaida    +   " "   + pResposta.ObjetoDeRetorno.HoraSaida      + "</td>" +
                  "      <td title='" + pResposta.ObjetoDeRetorno.Texto        + "'>"    + pResposta.ObjetoDeRetorno.TextoTruncado  + "</td>" +
                  "      <td>"        + pResposta.ObjetoDeRetorno.FlagAtivacaoManual  + "</td>" +
                  "      <td>"        + pResposta.ObjetoDeRetorno.IdSistema    + "</td>" +
                  "      <td> <button style='margin-top: 2px; margin-bottom: 2px;' onclick='return btnHomeBroker_Avisos_Editar_Click(this)' title='Editar Aviso' class='IconButton Editar'><span>Excluir</span></button>  </td>" +
                  "  </tr>";

        lTBody.children(":eq(0)").before(lTR);

        lTBody.find("tr.NenhumItem").hide();

        GradIntra_HomeBroker_LimparFormulario();
    }
}

function GradIntra_HomeBroker_EditarAviso_CallBack(pResposta)
{
    $("#hidHomeBroker_Aviso_Id").val( pResposta.ObjetoDeRetorno.CodigoAviso );

    $("#txtHomeBroker_Aviso_DataDeEntrada").val( pResposta.ObjetoDeRetorno.DataEntrada );

    $("#txtHomeBroker_Aviso_HoraDeEntrada").val( pResposta.ObjetoDeRetorno.HoraEntrada );

    $("#txtHomeBroker_Aviso_DataDeSaida").val( pResposta.ObjetoDeRetorno.DataSaida );

    $("#txtHomeBroker_Aviso_HoraDeSaida").val( pResposta.ObjetoDeRetorno.HoraSaida);

    $("#txtHomeBroker_Aviso_Texto").val( pResposta.ObjetoDeRetorno.Texto );

    $("#txtHomeBroker_Aviso_CBLCs").val( pResposta.ObjetoDeRetorno.CBLCs );

    $("#chkHomeBroker_Aviso_Sistema").val( pResposta.ObjetoDeRetorno.IdSistema );

    if(pResposta.ObjetoDeRetorno.FlagAtivacaoManual == "S")
    {
        $("#chkHomeBroker_Aviso_FlagAtivacaoManual")
            .prop("checked", true)
            .next("label")
                .addClass("checked");
    }
    else
    {
        $("#chkHomeBroker_Aviso_FlagAtivacaoManual")
            .prop("checked", false)
            .next("label")
                .removeClass("checked");
    }

    $("#hidHomeBroker_Aviso_Id")
        .closest("div.pnlFormulario_Campos")
        .find("p.BotoesSubmit button")
            .html("Atualizar");
}

function GradIntra_HomeBroker_LimparFormulario()
{
    $("#hidHomeBroker_Aviso_Id").val( "" );
    $("#txtHomeBroker_Aviso_DataDeEntrada").val( "" );
    $("#txtHomeBroker_Aviso_DataDeSaida").val( "" );
    $("#txtHomeBroker_Aviso_HoraDeEntrada").val( "" );
    $("#txtHomeBroker_Aviso_HoraDeSaida").val( "" );
    $("#txtHomeBroker_Aviso_Texto").val("");
    $("#txtHomeBroker_Aviso_CBLCs").val("");
    $("#chkHomeBroker_Aviso_Sistema").val("1");

//    $("#chkHomeBroker_Aviso_FlagAtivacaoManual")
//        .prop("checked", false)
//        .next("label")
//            .removeClass("checked")
//            .closest("div.pnlFormulario_Campos")
//                .find("p.BotoesSubmit button")
//                    .html("Incluir");
    $("#chkHomeBroker_Aviso_Sistema")
        .closest("div.pnlFormulario_Campos")
            .find("p.BotoesSubmit button")
                .html("Incluir");
}