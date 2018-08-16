/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
function btnResgate_Buscar_Click() 
{
    $("#divResgate_Resultados").show();

    GradIntra_Resgate_Grid();

    return false;
}

function GradIntra_Resgate_Grid() 
{
    $("#tblResgate_Resultados").jqGrid(
    {
        url: "http://localhost:52026/Solicitacoes/Get"
        , datatype      : "json"
        , crossDomain   : true
        , mtype         : "GET"
        , colModel      : 
                        [ { label: "Código"           , name: "Codigo"            , jsonmap: "Codigo"         , index: "Codigo"           , width: "100px", align: "center"   , sortable: false, hidden: true }
                        , { label: "Cód. Cliente"     , name: "Cliente"           , jsonmap: "Cliente"        , index: "Cliente"          , width: "100px", align: "center"   , sortable: false }
                        , { label: "Assessor"         , name: "Assessor"          , jsonmap: "Assessor"       , index: "Assessor"         , width: "100px", align: "center"   , sortable: false }
                        , { label: "Valor Solicitado" , name: "ValorSolicitado"   , jsonmap: "Valor"          , index: "ValorSolicitado"  , width: "100px", align: "right"    , sortable: false, formatter: 'currency', formatoptions: { prefix: 'R$ ', suffix: '', thousandsSeparator: '.'} }
                        , { label: "Saldo D0"         , name: "SaldoD0"           , jsonmap: "SaldoD0"        , index: "SaldoD0"          , width: "100px", align: "right"    , sortable: false, formatter: 'currency', formatoptions: { prefix: 'R$ ', suffix: '', thousandsSeparator: '.'} }
                        , { label: "Saldo Projetado"  , name: "SaldoProjetado"    , jsonmap: "SaldoProjetado" , index: "SaldoProjetado"   , width: "100px", align: "right"    , sortable: false, formatter: 'currency', formatoptions: { prefix: 'R$ ', suffix: '', thousandsSeparator: '.'} }
                        , { label: "Status"           , name: "Status"            , jsonmap: "Codigo_Status"  , index: "Status"           , width: "100px", align: "center"   , sortable: false, formatter: TraduzirStatus  }
                        , { label: "Aprovar"          , name: "Aprovar"           , jsonmap: "Codigo"         , index: "Aprovar"          , width: "150px", align: "center"   , sortable: false, formatter: AdicionarLink   }
                        ]
        , pager         : jQuery('#pager')
        , rowNum        : 10
        , rowList       : [10, 20, 30, 40]
        , height        : '100%'
        , viewrecords   : true
        , autowidth     : true
        , multiselect   : true
        , emptyrecords: 'Nenhuma solictação encontrada'
        , jsonReader    : 
        {
            root            : "rows"
            , page          : "page"
            , total         : "total"
            , records       : "records"
            , repeatitems   : false
            , Id            : "0"
        }
    }).navGrid('#pager',
        {
            edit            : false
            , add           : false
            , del           : false
            , search        : false
            , refresh       : false 
        },
        {// edit options
            zIndex          : 100
            , url: 'http://localhost:52026/Solicitacoes/Edit'
            , closeOnEscape : true
            , closeAfterEdit: true
            , recreateForm  : true
            , afterComplete : 
                function (response) 
                {
                    if (response.responseText) 
                    {
                        alert(response.responseText);
                    }
                }
        },
        {// add options
            zIndex          : 100
            , url: "http://localhost:52026/Solicitacoes/Create"
            , closeOnEscape : true
            , closeAfterAdd : true
            , afterComplete : 
                function (response) 
                {
                    if (response.responseText) 
                    {
                        alert(response.responseText);
                    }
                }
        },
        {// delete options
            zIndex              : 100
            , url               : "http://localhost:60003/Solicitacoes/Delete"
            , closeOnEscape     : true
            , closeAfterDelete  : true
            , recreateForm      : true
            , msg               : "Are you sure you want to delete this row?"
            , afterComplete     : 
                function (response) 
                {
                    if (response.responseText) 
                    {
                        alert(response.responseText);
                    }
                }
        });
}

function AdicionarLink(cellvalue, options, rowObject) 
{
    var links;

    if (rowObject.Codigo_Status == 3)
    {
        links = "<input type='button' value='Aprovar' onclick=\"Aprovar(" + rowObject.Codigo.toString() + ");\"\>" + "<input type='button' value='Reprovar' onclick=\"Rejeitar(" + rowObject.Codigo.toString() + ");\"\>" + "<input type='button' value='Cancelar' onclick=\"Cancelar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>";
    }
    else if (rowObject.Codigo_Status == 4) 
    {
        links = "<input type='button' value='Aprovar' onclick=\"Aprovar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>" + "<input type='button' value='Reprovar' onclick=\"Rejeitar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>" + "<input type='button' value='Cancelar' onclick=\"Cancelar(" + rowObject.Codigo.toString() + ");\" \>";
    }
    else if (rowObject.Codigo_Status == 5) {
        links = "<input type='button' value='Aprovar' onclick=\"Aprovar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>" + "<input type='button' value='Reprovar' onclick=\"Rejeitar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>" + "<input type='button' value='Cancelar' onclick=\"Cancelar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>";
    }
    else
    {
        links = "<input type='button' value='Aprovar' onclick=\"Aprovar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>" + "<input type='button' value='Reprovar' onclick=\"Rejeitar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>" + "<input type='button' value='Cancelar' onclick=\"Cancelar(" + rowObject.Codigo.toString() + ");\" disabled='disabled'\>";
    }
    
    return links;
}

function TraduzirStatus(cellvalue, options, rowObject) 
{
    if (cellvalue == 1) 
    {
        return "Indefinido";
    }
    if (cellvalue == 2) 
    {
        return "Solicitada";
    }
    if (cellvalue == 3) 
    {
        return "Em análise";
    }
    if (cellvalue == 4) 
    {
        return "Aprovada";
    }
    if (cellvalue == 5) 
    {
        return "Rejeitada";
    }
}

function btnRegate_Filtrar_Click(event) 
{
    var prefs = 
    {
        scol: $('#tblResgate_Resultados').jqGrid('getGridParam', 'sortname'),
        sord: $('#tblResgate_Resultados').jqGrid('getGridParam', 'sortorder'),
        page: $('#tblResgate_Resultados').jqGrid('getGridParam', 'page'),
        rows: $('#tblResgate_Resultados').jqGrid('getGridParam', 'rowNum')
    };

    var postDataValues = jQuery("#tblResgate_Resultados").getGridParam('postData');

    postDataValues["Cliente"]       = $("#txtResgate_Filtro_Cliente").val();
    postDataValues["Assessor"]      = $("#txtResgate_Filtro_Assessor").val();
    postDataValues["DataInicial"]   = $("#txtResgate_Filtro_DataInicial").val();
    postDataValues["DataFinal"]     = $("#txtResgate_Filtro_DataFinal").val();
    postDataValues["Status"]        = $('input[name=chkResgate_Filtro_Status]:checked').val();

    $("#tblResgate_Resultados").setGridParam({ postData: postDataValues });

    $('#tblResgate_Resultados').jqGrid('setGridParam', 
        {
            sortname: prefs.scol,
            sortorder: prefs.sord,
            page: 1,
            rowNum: prefs.rows
        });

    $("#tblResgate_Resultados").trigger("reloadGrid");

    return false;
}

function btnResgate_Limpar_Click(event) 
{
    var postDataValues = jQuery("#tblResgate_Resultados").getGridParam('postData');

    postDataValues["Cliente"]       = "";
    postDataValues["Assessor"]      = "";
    postDataValues["DataInicial"]   = "";
    postDataValues["DataFinal"]     = "";
    postDataValues["Status"]        = "";

    $("#tblResgate_Resultados").setGridParam({ postData: postDataValues }).trigger("reloadGrid");

    return false;
}

function btnResgate_Aprovar_Click() 
{
    var ids = $('#tblResgate_Resultados').getGridParam('selarrrow');

    for (var i = ids.length - 1; i >= 0; i--) 
    {
        jQuery.support.cors = true;
        var codigo = $('#tblResgate_Resultados').jqGrid("getCell", ids[i], "Codigo");
        $.ajax
        ({
            url: "http://localhost:52026/Solicitacoes/Aprove?Id=" + codigo + "&Status=4",
            type: 'POST',
            dataType: 'json',
            success: function (data) 
            {
                SucessoAprovar(data);
            }
        });
    }

    jQuery("#tblResgate_Resultados").trigger("reloadGrid");

    return false;
}

function btnResgate_Reprovar_Click() 
{
    var ids = $('#tblResgate_Resultados').getGridParam('selarrrow');

    for (var i = ids.length - 1; i >= 0; i--) 
    {
        var codigo = $('#tblResgate_Resultados').jqGrid("getCell", ids[i], "Codigo");
        jQuery.support.cors = true;
        $.ajax
        ({
            url: "http://localhost:52026/Solicitacoes/Reprove?Id=" + codigo + "&Status=5"
          , type    : 'POST'
          , dataType: 'json'
          , success: function (data) 
            {
                SucessoRejeitar(data);
            }
        });
    }

    jQuery("#tblResgate_Resultados").trigger("reloadGrid");

    return false;
}

function Aprovar(codigo) 
{
    jQuery.support.cors = true;

    $.ajax
    ({
        url         : "http://localhost:52026/Solicitacoes/Aprove?Id=" + codigo + "&Status=4"
      , type        : 'POST'
      , dataType    : 'json'
      , success     : function (data) 
                    {
                        SucessoAcaoGrid(data);
                    }
    });

    jQuery("#tblResgate_Resultados").trigger("reloadGrid");
    return false;
}

function Rejeitar(codigo) 
{
    jQuery.support.cors = true;

    $.ajax
    ({
        url: "http://localhost:52026/Solicitacoes/Reprove?Id=" + codigo + "&Status=5"
      , type    : 'GET'
      , dataType: 'json'
      , success           : function (data) 
                                {
                                    SucessoAcaoGrid(data);
                                }
    });
    
    return false;
}

function Cancelar(codigo) {
    jQuery.support.cors = true;

    $.ajax
    ({
        url: "http://localhost:52026/Transferencias/Cancelar?Id=" + codigo + "&Status=5"
      , type: 'POST'
      , dataType: 'json'
      , success: function (data) {
          SucessoAcaoGrid(data);
      }
    });

    return false;
}

function SucessoAcaoGrid() 
{
    jQuery("#tblResgate_Resultados").trigger("reloadGrid");
}

function SucessoAprovar() 
{
    jQuery("#tblResgate_Resultados").trigger("reloadGrid");
}

function SucessoRejeitar() 
{
    jQuery("#tblResgate_Resultados").trigger("reloadGrid");
}

function ResgateConfirmar_Click(codigo) 
{
    jQuery.support.cors = true;

    $.ajax
    ({
        url                 : "http://localhost:52026/Solicitacoes/Create"
        , type              : 'POST'
        , dataType          : 'json'
        , data              : Solicitacao()
        //data: { Transferencia: JSON.stringify(Transferencia) }
        , success           : function (data) 
                                {
                                    Sucesso(data);
                                },
        error               : function (data) 
                                {
                                    Erro(data);
                                }
    });

    return false;
  }

  function Solicitacao()
  {
    var myString    = jQuery("#cboResgate_Conta option:selected").val()
    var myArray     = myString.split(';');
    var Banco       = myArray[0]
    var Agencia     = myArray[1]
    var AgenciaDig  = myArray[2]
    var Conta       = myArray[3]
    var ContaDig    = myArray[4]

    var Solicitacao = 
    {
        'Nome'                       : gGradIntra_Cadastro_ItemSendoEditado.NomeCliente
        , 'Cliente'                  : jQuery("#hidConta").val()
        , 'Assessor'                 : jQuery("#hidAssessor").val()
        , 'Valor'                    : jQuery("#txtResgate_Valor").val()
        , 'SaldoD0'                  : jQuery("#txtResgate_Saldo").val()
        , 'SaldoProjetado'           : jQuery("#txtResgate_SaldoProjetado").val()
        , 'Banco'                    : Banco
        , 'Agencia'                  : Agencia
        , 'AgenciaDig'               : AgenciaDig
        , 'Conta'                    : Conta
        , 'ContaDig'                 : ContaDig
        , 'CpfCnpj'                  : jQuery("#txtResgate_CPF").val()
        , 'Solicitante'              : jQuery("#hidUsuarioLogado").val()
        , 'Codigo_Origem_Solicitacao': 2 // Intranet
    }
    return Solicitacao;
  }

  function Erro(pErro)
  {
      GradIntra_ExibirMensagem("E", pErro);
  }

  function Sucesso(pResposta) 
  {
      GradIntra_ExibirMensagem("I", pResposta.Resposta);
      EnviarEmailResgate();
  }

  function EnviarEmailResgate(pSolicitacao) 
  {
      var lDados = { Acao: "EnviarEmailResgate", Id: gGradIntra_Cadastro_ItemSendoEditado.Id, Email: gGradIntra_Cadastro_ItemSendoEditado.Email, Solicitacao: Solicitacao() };

      GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/Resgate.aspx"
                                          , lDados
                                          , GradIntra_Clientes_Resgate_EnviarEmailResgate_CallBack);
  }

  function GradIntra_Clientes_Resgate_EnviarEmailResgate_CallBack(pResposta) 
  {
      GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
  }


  /*
  function EnviarSolicitacaoResgate(pSolicitacao) 
  {
      var lDados = { Acao: "ResponderSolicitarResgate", Id: gGradIntra_Cadastro_ItemSendoEditado.Id, Email: gGradIntra_Cadastro_ItemSendoEditado.Email, Solicitacao: pSolicitacao };
      
      GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/Resgate.aspx"
                                          , lDados
                                          , GradIntra_Clientes_Resgate_SolicitarResgate_CallBack);
  }

  function GradIntra_Clientes_Resgate_SolicitarResgate_CallBack(pResposta) 
  {
      GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
  }
  */

  function RetornarTaxa_Click(codigoCliente) 
  {
      jQuery.support.cors = true;

      $.ajax
    ({
        url: "http://localhost:52026/Solicitacoes/TaxaCustodia?CodigoCliente=" + codigoCliente
      , type: 'POST'
      , dataType: 'json'
      , success: function (data) {
          Sucessotaxa(data);
      }
    });

      return false;
  }


  function Sucessotaxa(data) {
      alert(data);
  }