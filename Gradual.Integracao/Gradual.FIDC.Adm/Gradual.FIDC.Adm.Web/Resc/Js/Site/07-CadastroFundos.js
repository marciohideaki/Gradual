var gGridFundos;

//executa atualizacao
function btnCadastroFundoAtualizarADM_Click() {

    //verifica dados obrigatórios
    var erros = '';

    if ($("#txtCadastroFundoADMNomeFundo").val() === '')
        erros += 'Informe o nome do fundo\n';
    if ($("#txtCadastroFundoADMCNPJFundo").val() === '')
        erros += 'Informe o CNPJ do fundo\n';

    if ($("#txtCadastroFundoADMNomeAdministrador").val() === '')
        erros += 'Informe o nome do administrador\n';
    if ($("#txtCadastroFundoADMCNPJAdministrador").val() === '')
        erros += 'Informe o CNPJ do administrador\n';

    if ($("#txtCadastroFundoADMNomeCustodiante").val() === '')
        erros += 'Informe o nome do custodiante\n';
    if ($("#txtCadastroFundoADMCNPJCustodiante").val() === '')
        erros += 'Informe o CNPJ do custodiante\n';

    if ($("#txtCadastroFundoADMNomeGestor").val() === '')
        erros += 'Informe o nome do gestor\n';
    if ($("#txtCadastroFundoADMCNPJGestor").val() === '')
        erros += 'Informe o CNPJ do gestor\n';

    if ($("#txtCadastroFundoTxGestao").val() === '')
        erros += 'Informe a taxa de gestão\n';
    if ($("#txtCadastroFundoTxCustodia").val() === '')
        erros += 'Informe a taxa de custódia\n';
    if ($("#txtCadastroFundoTxConsultoria").val() === '')
        erros += 'Informe a taxa de consultoria\n';

    //caso todos os dados estejam preenchidos corretamente, prossegue com a atualização do fundo
    if (erros === "") {

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroFundos.aspx");
        
        var lDados = new FormData();

        lDados.append("Acao", "AtualizarValor");
        lDados.append("NomeFundo", $("#txtCadastroFundoADMNomeFundo").val());
        lDados.append("CNPJFundo", $("#txtCadastroFundoADMCNPJFundo").val());
        lDados.append("NomeAdministrador", $("#txtCadastroFundoADMNomeAdministrador").val());
        lDados.append("CNPJAdministrador", $("#txtCadastroFundoADMCNPJAdministrador").val());
        lDados.append("NomeCustodiante", $("#txtCadastroFundoADMNomeCustodiante").val());
        lDados.append("CNPJCustodiante", $("#txtCadastroFundoADMCNPJCustodiante").val());
        lDados.append("NomeGestor", $("#txtCadastroFundoADMNomeGestor").val());
        lDados.append("CNPJGestor", $("#txtCadastroFundoADMCNPJGestor").val());
        lDados.append("TxGestao", $("#txtCadastroFundoTxGestao").val());
        lDados.append("TxCustodia", $("#txtCadastroFundoTxCustodia").val());
        lDados.append("TxConsultoria", $("#txtCadastroFundoTxConsultoria").val());
        lDados.append("InativarFundo", $("#chkCadastroFundosInativarFundo").is(':checked'));
        lDados.append("IdFundoCadastro", $("#hdnIdFundoCadastro").val());

        lDados.append("ArquivoAnexoRegulamento", obterAnexosCadastroFundosRegulamento());
        lDados.append("ArquivoAnexoContratoGestao", obterAnexosCadastroFundosContratoGestao());
        lDados.append("ArquivoAnexoContratoCustodia", obterAnexosCadastroFundosContratoCustodia());

        $.ajax({
            url: lUrl,
            type: "POST",
            data: lDados,
            processData: false,
            contentType: false,
            async: false,
            success: btnCadastroFundoAtualizarADMLiquidados_Click_Callback,
            error: Aux_TratarRespostaComErro
        });
    }
    else {
        alert(erros);
    }

    return false;
}

//função de retorno da atualização de fundos
function btnCadastroFundoAtualizarADMLiquidados_Click_Callback(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK")
        resp.Mensagem = "Fundo atualizado com sucesso";

    LimparDadosCadastroFundo();

    bootbox.alert(resp.Mensagem);
}

function btnFiltrarCadastroFundos()
{
    Grid_Resultado_Fundos();

    FiltroCadastroFundos_Click();
}

var gMock_CadastroFundosGrid = {};

//função que busca dados do grid de cadastro de fundos
function FiltroCadastroFundos_Click() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroFundos.aspx");

    var lDados =
        {
            Acao: "CarregarHtmlComDados"
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, FiltroCadastroFundos_Click_CallBack);

    return false;
}

//função de retorno do carregamento do grid de cadastro de fundos
function FiltroCadastroFundos_Click_CallBack(pResposta) {
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_CadastroFundosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        NomeFundo: lData.Itens[i].NomeFundo,
                        NomeAdministrador: lData.Itens[i].NomeAdministrador,
                        NomeCustodiante: lData.Itens[i].NomeCustodiante,
                        NomeGestor: lData.Itens[i].NomeGestor,
                        Status: lData.Itens[i].Status
                    };

                gMock_CadastroFundosGrid.rows.push(lObjeto);
            }
        }

        $('#gridCadastroFundos')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CadastroFundosGrid.rows })
            .trigger("reloadGrid");

        $('.ui-pg-input').width(20);
    }
}

///Método que monta o grid
function Grid_Resultado_Fundos() {

    gGridFundos =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CadastroFundosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdFundoCadastro", jsonmap: "IdFundoCadastro", index: "IdFundoCadastro", align: "left", sortable: true, hidden: true }
                    , { label: "Nome Fundo", name: "NomeFundo", jsonmap: "NomeFundo", index: "NomeFundo", align: "left", sortable: true }
                    , { label: "Nome Administrador", name: "NomeAdministrador", jsonmap: "NomeAdministrador", index: "NomeAdministrador", width: 150, align: "left", sortable: true }
                    , { label: "Nome Custodiante", name: "NomeCustodiante", jsonmap: "NomeCustodiante", index: "NomeCustodiante", width: 150, align: "left", sortable: true }
                    , { label: "Nome Gestor", name: "NomeGestor", jsonmap: "NomeGestor", index: "NomeGestor", width: 150, align: "left", sortable: true }
                    , { label: "Status", name: "Status", jsonmap: "Status", index: "Status", width: 40, align: "left", sortable: false }
                    , { name: '', index: 'editar', width: 30, formatter: editarFormatter, sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortname: "NomeFundo"
      , sortorder: "asc"
      , viewrecords: true
      , gridview: false
      , pager: '#pgCadastroFundos'
      , subGrid: false
      , rowNum: 20
      , rowList: [20,30,40,50,60]
      , caption: 'Cadastro de Fundos'      
      , sortable: true
      , beforeSelectRow: function () {
          return false;
      }
    };

    $('#gridCadastroFundos').jqGrid(gGridFundos);

    $('#gridCadastroFundos').jqGrid('setGridWidth', 1010);
}

function editarFormatter(cellvalue, options) {    
    return "<a href='#' style='height:25px;width:5px;' type='button' title='Editar' onclick=\"editarCadastroFundo(" + options.rowId + ")\" >Editar</a>";
}

function editarCadastroFundo(id) {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/CadastroFundos.aspx");

    var lDados =
        {
            Acao: "EditarFundo"
          , IdFundoCadastro: id                        
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, editarCadastroFundo_Callback);

    jQuery('#gridCadastroFundos').jqGrid('setSelection', id);

    return false;
}

function editarCadastroFundo_Callback(pResposta) {

    var resp = JSON.parse(pResposta);
    resp = resp[0];

    $("#txtCadastroFundoADMNomeFundo").val(resp.NomeFundo);
    $("#txtCadastroFundoADMCNPJFundo").val(resp.CNPJFundo);
    $("#txtCadastroFundoADMNomeAdministrador").val(resp.NomeAdministrador);
    $("#txtCadastroFundoADMCNPJAdministrador").val(resp.CNPJAdministrador);
    $("#txtCadastroFundoADMNomeCustodiante").val(resp.NomeCustodiante);
    $("#txtCadastroFundoADMCNPJCustodiante").val(resp.CNPJCustodiante);
    $("#txtCadastroFundoADMNomeGestor").val(resp.NomeGestor);
    $("#txtCadastroFundoADMCNPJGestor").val(resp.CNPJGestor);
    $('#hdnIdFundoCadastro').val(resp.IdFundoCadastro);

    $("#txtCadastroFundoTxGestao").val(resp.TxGestao.toString().replace(".", ","));
    $("#txtCadastroFundoTxCustodia").val(resp.TxCustodia.toString().replace(".", ","));
    $("#txtCadastroFundoTxConsultoria").val(resp.TxConsultoria.toString().replace(".", ","));
    
    $('#chkCadastroFundosInativarFundo').removeAttr('disabled');

    if (resp.IsAtivo)
        $('#chkCadastroFundosInativarFundo').prop('checked', false);
    else
        $('#chkCadastroFundosInativarFundo').prop('checked', true);

    $(".cnpj").mask("99.999.999/9999-99");

    return false;
}

$(document).ready(function () {

    if ($('#chkCadastroFundosInativarFundo')[0] != undefined) {

        //define mascara de cnpj
        $(".cnpj").mask("99.999.999/9999-99");

        $('.taxa').mask('000,00', { reverse: true });
        
        $('#chkCadastroFundosInativarFundo').attr('disabled', 'disabled');
               
        formatarCamposUploadCadastroFundos();

        btnFiltrarCadastroFundos();

        var idFundoCadastro = $.urlParamCadastroFundos('IdFundoCadastroEditar');

        if (idFundoCadastro !== undefined) {
            editarCadastroFundo(idFundoCadastro);
        }
    }
});

function formatarCamposUploadCadastroFundos() {

    jQuery("#upAnexarRegulamento").change(function () {
                
        $("#dvBtnRemoverArquivoRegulamento a").remove();

        $("#dvBtnRemoverArquivoRegulamento").append("<a id='anRemoverArquivoRegulamentoUpload' class='removerAnexo' title='remover anexo' type='button' onclick=removerAnexoRegulamentoCadastroFundos() class='removerArquivo'>X</a>");

        var fileUpload = $("#upAnexarRegulamento");
    });

    jQuery("#upAnexarContratoGestao").change(function () {

        $("#dvBtnRemoverArquivoContratoGestao a").remove();
        
        $("#dvBtnRemoverArquivoContratoGestao").append("<a id='anRemoverArquivoContratoGestaoUpload' class='removerAnexo' title='remover anexo' type='button' onclick=removerAnexoContratoGestaoCadastroFundos() class='removerArquivo'>X</a>");

        var fileUpload = $("#upAnexarContratoGestao");
    });

    jQuery("#upAnexarContratoCustodia").change(function () {

        $("#dvBtnRemoverArquivoContratoCustodia a").remove();
        
        $("#dvBtnRemoverArquivoContratoCustodia").append("<a id='anRemoverArquivoContratoCustodiaUpload' class='removerAnexo' title='remover anexo' type='button' onclick=removerAnexoContratoCustodiaCadastroFundos() class='removerArquivo'>X</a>");

        var fileUpload = $("#upAnexarContratoCustodia");
    });

}

function removerAnexoRegulamentoCadastroFundos() {

    $("#dvBtnRemoverArquivoRegulamento a").remove();

    var input = $("#upAnexarRegulamento");

    input.replaceWith(input.val('').clone(true));
}

function removerAnexoContratoGestaoCadastroFundos() {
    
    $("#dvBtnRemoverArquivoContratoGestao a").remove();

    var input = $("#upAnexarContratoGestao");

    input.replaceWith(input.val('').clone(true));
}

function removerAnexoContratoCustodiaCadastroFundos() {

    $("#dvBtnRemoverArquivoContratoCustodia a").remove();

    var input = $("#upAnexarContratoCustodia");

    input.replaceWith(input.val('').clone(true));
}

//limpa campos da tela
function LimparDadosCadastroFundo() {

    $("#txtCadastroFundoADMNomeFundo").val('');
    $("#txtCadastroFundoADMCNPJFundo").val('');
    $("#txtCadastroFundoADMNomeAdministrador").val('');
    $("#txtCadastroFundoADMCNPJAdministrador").val('');
    $("#txtCadastroFundoADMNomeCustodiante").val('');
    $("#txtCadastroFundoADMCNPJCustodiante").val('');
    $("#txtCadastroFundoADMNomeGestor").val('');
    $("#txtCadastroFundoADMCNPJGestor").val('');

    $("#txtCadastroFundoTxGestao").val('');
    $("#txtCadastroFundoTxCustodia").val('');
    $("#txtCadastroFundoTxConsultoria").val('');

    //limpa o campo que indica o fundo a ser atualizado
    $('#hdnIdFundoCadastro').val('0');

    $('#chkCadastroFundosInativarFundo').attr('disabled', 'disabled');
    $('#chkCadastroFundosInativarFundo').prop('checked', false);

    removerAnexoRegulamentoCadastroFundos();
    removerAnexoContratoGestaoCadastroFundos();
    removerAnexoContratoCustodiaCadastroFundos();

    btnFiltrarCadastroFundos();
}

function obterAnexosCadastroFundosRegulamento() {

    var input = $("#upAnexarRegulamento")[0];

    if (input.files && input.files[0]) {

        var file = input.files[0];
        var fr = new FileReader();

        fr.readAsDataURL(file);

        return file;
    }

    return null;
}

function obterAnexosCadastroFundosContratoGestao() {
        
    var input = $("#upAnexarContratoGestao")[0];

    if (input.files && input.files[0]) {

        var file = input.files[0];
        var fr = new FileReader();

        fr.readAsDataURL(file);

        return file;
    }

    return null;
}

function obterAnexosCadastroFundosContratoCustodia() {

    var input = $("#upAnexarContratoCustodia")[0];

    if (input.files && input.files[0]) {

        var file = input.files[0];
        var fr = new FileReader();

        fr.readAsDataURL(file);

        return file;
    }

    return null;
}

$.urlParamCadastroFundos = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);

    if(results !== null) return results[1] || 0;
}