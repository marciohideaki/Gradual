/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />

function BuscarProduto_Click(pSender)
{
    var ltxtDataInicial            = $("#txt_DataInicial").val();
    var ltxtDataFinal              = $("#txt_DataFinal").val();
    var ltxtCodigoCliente          = $("#txt_CodigoCliente").val();
    var lComboStatus               = $("#cmbBusca_Status_Poupe_Operacoes option:selected").val();

    
    var lURL = "Solicitacoes/PoupeDirect/PoupeOperacoesResultado.aspx";
    var lObjetoDeParametros = { Acao            : "SelecionarProdutos"
                              , IdStatus        : lComboStatus
                              , DataInicial     : ltxtDataInicial
                              , DataFinal       : ltxtDataFinal
                              , CodigoCliente   : ltxtCodigoCliente
                              , status          : lComboStatus
                              };

    var pDivDeFormulario = $("#PnlGrad_intra_PoupeOperacoes_Resultado");
                                      
    GradIntra_CarregarHtmlVerificandoErro(lURL, lObjetoDeParametros, pDivDeFormulario, function(pResposta) { BuscarProduto_Click_Callback(pResposta); });

}

function BuscarProduto_Click_Callback(pResposta)
{
    
}


function PoupeCompra_Click(pSender)
{
    var lIdsSelecionados = "";

    $(".chkSolicitacoes_PoupeOperacoes_Compra:checked").each(function()
    {
       lIdsSelecionados += $(this).next().val() + ",";
    });

    var lURL = "Solicitacoes/PoupeDirect/PoupeOperacoesResultado.aspx";
    var lObjetoDeParametros = { Acao      : "AtualizarProduto"
                              , lIdsSelecionados: lIdsSelecionados
                              };


    var pDivDeFormulario = $("#PnlGrad_intra_PoupeOperacoes_Resultado");
                                      
    GradIntra_CarregarHtmlVerificandoErro(lURL, lObjetoDeParametros, pDivDeFormulario, function(pResposta) { PoupeCompra_Click_Callback(pResposta); });
}

function PoupeCompra_Click_Callback()
{
    BuscarProduto_Click(null);
}