/// <reference path="00-Base.js" />
function Cliente_Risco_Permissao_Salvar_Click(pSender)
{
    var lListaPermissoes = $(".Cliente_Risco_Form_Dados").find("input[type='checkbox']"); ;
    var lPermissoes = [];

    lListaPermissoes.each(function () {

        if ($(this).is(":checked")) lPermissoes.push($(this).val());
    });

    var lObjetoJson = 
    {
        ListaPermissoes: lPermissoes
    };

    var lAcao = "SalvarPermissoes";

    var lUrl = "Clientes.aspx";

    var pDivDeFormulario = $(".alert");

    var pOpcoesPosCarregamento = "";

    var lRisco =
    {
        Acao: lAcao,
        ObjetoJson: $.toJSON(lObjetoJson)
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lRisco, Cliente_Risco_Permissao_Salvar_Click_Callback, pOpcoesPosCarregamento, pDivDeFormulario)


    return false;
}

function Cliente_Risco_Permissao_Salvar_Click_Callback(pResposta, pDivDeFormulario)
{
    if (pResposta.TemErro)
    {
        
    }
    else
    {
        
    }
}

function Cliente_Risco_Listar_Dados_Cliente(pCodigoBovespa, pCodigoBmf) 
{
    var lAcao = "ListarDadosRiscoClientes";

    var lUrl = "Clientes.aspx";

    var pDivDeFormulario = $(".alert");

    var pOpcoesPosCarregamento = "";

    var lRisco =
    {
        Acao            : lAcao,
        CodigoCliente   : pCodigoBovespa,
        CodigoClienteBmf: pCodigoBmf
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lRisco, Cliente_Risco_Listar_Dados_Cliente_Callback, pOpcoesPosCarregamento, pDivDeFormulario)
}

function Cliente_Risco_Listar_Dados_Cliente_Callback(pResposta, pDivDeFormulario) 
{
    if (!pResposta.TemErro) 
    {
        var lPermissoes     = pResposta.ObjetoDeRetorno.gPermissoes.ListaPermissoes;
        
        var lLimitesBovespa = pResposta.ObjetoDeRetorno.gLimiteBovespa;
        
        var lLimitesBmf     = pResposta.ObjetoDeRetorno.gLimiteBmf;
        
        var lRestricoes     = pResposta.ObjetoDeRetorno.gRestricoes;

        Cliente_Risco_Carrega_Dados_Permissoes(lPermissoes);
        
        Cliente_Risco_Carrega_Dados_LimitesBovespa(lLimitesBovespa);
        
        Cliente_Risco_Carrega_Dados_LimitesBmf(lLimitesBmf);
        
        Cliente_Risco_Carrega_Dados_Restricoes(lRestricoes);
    }
}

function Cliente_Risco_Carrega_Dados_Permissoes(pPermissoes) 
{
    var lListaCodigoPermissao = [];

    var lListaPermissoes = $(".Cliente_Risco_Form_Dados").find("input[type='checkbox']");

    for (i = 0; i < pPermissoes.length; i++) 
    {
        var lCodigo = pPermissoes[i];

        lListaCodigoPermissao.push(lCodigo);
    }

    lListaPermissoes.each(function () 
    {
        var lCodigoPermissao = $(this).val();

        var lIndiceAchou = $.inArray(parseInt(lCodigoPermissao), lListaCodigoPermissao);

        if (lIndiceAchou > -1) 
        {
            $(this).prop("checked", true);
        }
    });
}

function Cliente_Risco_Carrega_Dados_LimitesBovespa(pLimitesBovespa) 
{
    $(".Cliente_Risco_LimiteBov_Compra_AVista") .val(pLimitesBovespa.LimiteAvistaCompra);
    $(".Cliente_Risco_LimiteBov_Venda_AVista")  .val(pLimitesBovespa.LimiteAvistaVenda);
    $(".Cliente_Risco_LimiteBov_Compra_Opcoes") .val(pLimitesBovespa.LimiteOpcaoCompra);
    $(".Cliente_Risco_LimiteBov_Venda_Opcoes")  .val(pLimitesBovespa.LimiteOpcaoVenda);
    $(".Cliente_Risco_FatFinger_DataValidade")  .val(pLimitesBovespa.FatFingerData);
    $(".Cliente_Risco_FatFinger_Valor")         .val(pLimitesBovespa.FatFinger);
}

function Cliente_Risco_Carrega_Dados_LimitesBmf(pLimitesBmf) 
{
    var lTbody = $(".Cliente_Risco_LimiteBmf_Lista");

    for (i = 0; i < pLimitesBmf.ListaLimiteBmf.length; i++)
    {
        var lLimite = pLimitesBmf.ListaLimiteBmf[i];

        var lLinha = "<tr>";
        lLinha += "<td>" + lLimite.Contrato + ".FUT"   + "</td>";
        lLinha += "<td>" + lLimite.Instrumento            + "</td>";

        if (lLimite.Sentido == "V") 
        {
            lLinha += "<td> 0 </td>";
            lLinha += "<td>" + lLimite.QtDisponivel       + "</td>";
        }
        else if (lLimite.Sentido == "C") 
        {
            lLinha += "<td>" + lLimite.QtDisponivel       + "</td>";
            lLinha += "<td> 0 </td>";
        }

        lLinha += "<td>" + lLimite.QuantidadeMaximaOferta + "</td>";
        lLinha += "<td>" + lLimite.QuantidadeMaximaOferta + "</td>";
        lLinha += "<td>" + lLimite.DataValidade           + "</td>";
        lLinha += "</tr>";

        lTbody.append(lLinha);
    }

    /*
    <tr>
        <td><input class="form-control Cliente_Risco_LimiteBmf_Serie"  data-parsley-trigger="change" id="Text6"    name="compra"   type="text"></td>
        <td><input class="form-control Cliente_Risco_LimiteBmf_Serie"  data-parsley-trigger="change" id="Text5"    name="compra"   type="text"></td>
        <td><input class="form-control Cliente_Risco_LimiteBmf_Compra" data-parsley-trigger="change" id="compra"    name="compra"   type="text"></td>
        <td><input class="form-control Cliente_Risco_LimiteBmf_Venda"  data-parsley-trigger="change" id="venda"     name="venda"    type="text"></td>
        <td><input class="form-control Cliente_Risco_LimiteBmf_Finger" data-parsley-trigger="change" id="finger"    name="finger"   type="text"></td>
        <td><div class="input-group input-group-sm"><span class="add-on input-group-addon"><i class="glyph-icon icon-calendar"></i></span><input type="text" data-date-format="mm/dd/yy"  class="bootstrap-datepicker form-control"></div></td>
    </tr>
    */

    if (lTbody.length > 0) 
    {
        
    }
    
}

function Cliente_Risco_Carrega_Dados_Restricoes(pRestricoes) 
{

}

function Cliente_Risco_LimiteBov_Salvar_Click(pSender) 
{
    var lAcao = "SalvarLimitesBovespa";

    var lUrl = "Clientes.aspx";

    var pDivDeFormulario = $(".alert");

    var pOpcoesPosCarregamento = "";

    var gLimiteBovespa = { LimiteAvistaCompra:"", LimiteAvistaVenda:"", LimiteOpcaoCompra: "", LimiteOpcaoVenda:"", FatFingerData:"", FatFinger:"" };

    gLimiteBovespa.LimiteAvistaCompra = $(".Cliente_Risco_LimiteBov_Compra_AVista").val();
    gLimiteBovespa.LimiteAvistaVenda  = $(".Cliente_Risco_LimiteBov_Venda_AVista").val();
    gLimiteBovespa.LimiteOpcaoCompra  = $(".Cliente_Risco_LimiteBov_Compra_Opcoes").val();
    gLimiteBovespa.LimiteOpcaoVenda   = $(".Cliente_Risco_LimiteBov_Venda_Opcoes").val();
    gLimiteBovespa.FatFingerData      = $(".Cliente_Risco_FatFinger_DataValidade").val();
    gLimiteBovespa.FatFinger          = $(".Cliente_Risco_FatFinger_Valor").val();           

    var lRisco =
    {
        Acao: lAcao,
        ObjetoJson: gLimiteBovespa
    };

    GradSpider_CarregarJsonVerificandoErro(lUrl, lRisco, Cliente_Risco_LimiteBov_Salvar_Click_Callback, pOpcoesPosCarregamento, pDivDeFormulario)

}

function Cliente_Risco_LimiteBov_Salvar_Click_Callback(pResposta, pDivDeFormulario) 
{
    if (!pResposta.TemErro) 
    {
        
    }
}