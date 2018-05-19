/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />


function GradIntra_Compliance_AoSelecionarSistema() 
{

    gGradIntra_Navegacao_SubSistemaAtualExibeBusca = true;

    if ($("#pnlConteudo_Compliance_EstatisticaDayTrade").hasClass(CONST_CLASS_CONTEUDOCARREGADO)) 
    {
        $(gGradIntra_Navegacao_PainelDeConteudoAtual).show();

        gGradIntra_Navegacao_SubSistemaAtualExibeConteudo = true;

        var lUrl = gGradIntra_Navegacao_SistemaAtual; 

        $("#pnlCompliance_Formularios_Dados")
            .removeClass(CONST_CLASS_CONTEUDOCARREGADO)
            .hide();
    }
    else
    {
        window.setTimeout(GradIntra_Compliance_AoSelecionarSistema, 750);
    }
}


var gFirstClickBuscarComplianceEstatisticaDayTrade = true;


function btnCompliance_EstatisticaDayTrade_Busca_Click(pSender) 
{
    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao"
                                  , CodigoCliente: $("#txtDBM_FiltroRelatorio_CodigoCliente").val()
                                  , CodAssessor: $("#cboBM_FiltroRelatorio_CodAssessor").val()
                                  , TipoBolsa: $("#cboBM_FiltroRelatorio_TipoBolsa").val()
    };

    $("#div_Compliance_EstatisticaDayTrade_Resultados").show();

    GradIntra_Compliance_EstatisticaDayTrade(lObjetoDeParametros);

    return false;
}

function btnCompliance_NegociosDiretos_Busca_Click(psender) 
{

    var lData = $("#txtCompliance_FiltroNegociosDiretos_Data").val();

    

//    if (lDataDe == "" || lDataAte == "") {
//        alert("É necessário inserir as datas de filtro!");

//        return false;
//    }

//    var lComparaDataDe = parseInt(lDataDe.split("/")[2].toString() + lDataDe.split("/")[1].toString() + lDataDe.split("/")[0].toString());

//    var lComparaDataAte = parseInt(lDataAte.split("/")[2].toString() + lDataAte.split("/")[1].toString() + lDataAte.split("/")[0].toString());

//    if (lComparaDataDe > lComparaDataAte) {
//        alert("A Data Inicial não pode ser maior que a data final!");

//        return false;
//    }

    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao"
                                  //, DataAte:        lDataAte
                                  , Data :        lData
                                  //, Sentido:        $("#cboCompliance_FiltroNegociosDiretos_Sentido").val()
                                  //, CodigoCliente:  $("#txtCompliance_FiltroNegociosDiretos_CodigoCliente").val()
    };

    $("#div_Compliance_NegociosDiretos_Resultados").show();

    GradIntra_Compliance_NegociosDiretos(lObjetoDeParametros);

    return false;
}

function btnCompliance_OrdensAlteradasDayTrade_Busca_Click(psender) 
{
    var lDataDe = $("#txtCompliance_Filtro_OrdensAlteradasDayTrade_DataDe").val();

    var lDataAte = $("#txtCompliance_Filtro_OrdensAlteradasDayTrade_DataAte").val();

    if (lDataDe == "" || lDataAte == "") 
    {
        alert("É necessário inserir as datas de filtro!");

        return false;
    }

    var lComparaDataDe  = parseInt(lDataDe.split("/")[2].toString() + lDataDe.split("/")[1].toString() + lDataDe.split("/")[0].toString());

    var lComparaDataAte = parseInt(lDataAte.split("/")[2].toString() + lDataAte.split("/")[1].toString() + lDataAte.split("/")[0].toString());

    if (lComparaDataDe > lComparaDataAte) 
    {
        alert("A Data Inicial não pode ser maior que a data final!");
        
        return false;
    }
    var lObjetoDeParametros = { Acao: "BuscarItensParaSelecao"
                                  , DataAte: lDataAte
                                  , DataDe: lDataDe
    };

    $("#div_Compliance_OrdensAlteradasDayTrade_Resultados").show();

    GradIntra_Compliance_OrdensAlteradasDayTrade(lObjetoDeParametros);

    return false;
}

var gFirstClickBuscarComplianceOrdensAlteradasDayTrade = true;

function GradIntra_Compliance_OrdensAlteradasDayTrade(pParametros) 
{

    if (gFirstClickBuscarComplianceOrdensAlteradasDayTrade != true) 
    {
        $("#tblBusca_Compliance_Resultados_OrdensAlteradasDayTrade")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarComplianceOrdensAlteradasDayTrade = false;

    $("#tblBusca_Compliance_Resultados_OrdensAlteradasDayTrade").jqGrid(
    {
        url: "Compliance/Formularios/OrdensAlteradasDayTrade.aspx?Acao=BuscarOrdensAlteradasDayTrade"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"               , name: "Id"            , jsonmap: "Id"            , index: "Id"            , width: 50, align: "center", sortable: false }
                      , { label: "Data Hora Ordem"  , name: "DataHoraOrdem" , jsonmap: "DataHoraOrdem" , index: "DataHoraOrdem" , width: 120, align: "center",sortable: true  }
                      , { label: "DayTrade"         , name: "DayTrade"      , jsonmap: "DayTrade"      , index: "DayTrade"      , width: 50, align: "center", sortable: false }
                      , { label: "Justificativa"    , name: "Justificativa" , jsonmap: "Justificativa" , index: "Justificativa" , width: 593, align: "left",  sortable: false }
                      , { label: "Cod. Ordem"       , name: "NumeroSeqOrdem", jsonmap: "NumeroSeqOrdem", index: "NumeroSeqOrdem", width: 75, align: "center", sortable: false }
                      , { label: "Tipo Mercado"     , name: "TipoMercado"   , jsonmap: "TipoMercado"   , index: "TipoMercado"   , width: 55, align: "center", sortable: false }
                  ]
      , height: 550
      , width: 964
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , sortable: true
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , subGridRowExpanded: GradIntra_Compliance_GridOrdensAlteradasDayTradeSubgridExpander
      , afterInsertRow: GradIntra_Compliance_OrdensAlteradasDayTrade_ItemDataBound
    }).jqGrid('hideCol', ['Id']);
}

function GradIntra_Compliance_OrdensAlteradasDayTrade_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_Compliance_Resultados_OrdensAlteradasDayTrade tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');
}

function GradIntra_Compliance_GridOrdensAlteradasDayTradeSubgridExpander(subgrid_id, row_id) 
{
    var subgrid_table_id;

    var NumeroSeqOrdem = $("#" + row_id).find("td").eq(5).html();

    subgrid_table_id = subgrid_id + "_table";

    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

    jQuery("#" + subgrid_table_id)
        .jqGrid({
            url: "Compliance/Formularios/OrdensAlteradasDayTrade.aspx?Acao=BuscarOrdensAlteradas"
        , hoverrows: false
        , datatype: "json"
        , mtype: "POST"
        , postData: { 'NumeroSeqOrdem': NumeroSeqOrdem }
        , shrinkToFit: false
        , colNames: ['Id','Assessor', 'Cliente', 'Conta Erro', 'Data Alteração', 'Desc. Corret.', 'Papel', 'Cod. Ordem', 'Quant.', 'Sentido', 'Pessoa F/J', 'Usuário', 'Usuario Alteração', 'Vinculado']
        , colModel: [
            { label: "Id"                   , name: "Id"                        , index: "Id"                , width: 230, align: "left"   , sortable: false},
            { label: "Assessor"             , name: "Assessor"                  , index: "Assessor"          , width: 230, align: "left"   , sortable: false},
            { label: "CodigoCliente"        , name: "CodigoCliente"             , index: "CodigoCliente"     , width: 50,  align: "center" , sortable: false},
            { label: "ContaErro"            , name: "ContaErro"                 , index: "ContaErro"         , width: 50,  align: "center" , sortable: false},
            { label: "DataAlteracao"        , name: "DataAlteracao"             , index: "DataAlteracao"     , width: 110, align: "center" , sortable: false},
            { label: "DescontoCorretagem"   , name: "DescontoCorretagem"        , index: "DescontoCorretagem", width: 65,  align: "center" , sortable: false},
            { label: "Instrumento"          , name: "Instrumento"               , index: "Instrumento"       , width: 50,  align: "center" , sortable: false},
            //{ label: "NomeCliente"        , name: "NomeCliente"               , index: "NomeCliente"       , width: 180, align: "center" , sortable: false},
            { label: "NumeroSeqOrdem"       , name: "NumeroSeqOrdem"            , index: "NumeroSeqOrdem"    , width: 65,  align: "center" , sortable: false},
            { label: "Quantidade"           , name: "Quantidade"                , index: "Quantidade"        , width: 50,  align: "center" , sortable: false},
            { label: "Sentido"              , name: "Sentido"                   , index: "Sentido"           , width: 45,  align: "center" , sortable: false},
            { label: "TipoPessoa"           , name: "TipoPessoa"                , index: "TipoPessoa"        , width: 50,  align: "center" , sortable: false},
            { label: "Usuario"              , name: "Usuario"                   , index: "Usuario"           , width: 160, align: "left"   , sortable: false},
            { label: "UsuarioAlteracao"     , name: "UsuarioAlteracao"          , index: "UsuarioAlteracao"  , width: 160, align: "left"   , sortable: false},
            { label: "Vinculado"            , name: "Vinculado"                 , index: "Vinculado"         , width: 50,  align: "center" , sortable: false}
          ]
        , jsonReader: {
            root: "Itens"
            , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
            , id: "0"              //primeira propriedade do elemento de linha é o Id
            , repeatitems: false
        }
        , afterInsertRow: GradIntra_Compliance_GridOrdensAlteradasDayTradeSubgridExpander_ItemDataBound
        , width: 1200
        , height: "100%"
        , rowNum: 20
        , sortname: 'num'
        , sortorder: 'asc'
    }).jqGrid('hideCol', ['Id']); ;
}

function GradIntra_Compliance_GridOrdensAlteradasDayTradeSubgridExpander_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_Compliance_Resultados_OrdensAlteradasDayTrade tr[id=" + rowid + "]");

    //lRow.css("height", 30);

    lRow.css("background-color", 'white');
}

function GradIntra_Compliance_EstatisticaDayTrade(pParametros) 
{
    if (gFirstClickBuscarComplianceEstatisticaDayTrade != true) 
    {
        $("#tblBusca_Compliance_Resultados_EstatisticaDayTrade")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarComplianceEstatisticaDayTrade = false;

    $("#tblBusca_Compliance_Resultados_EstatisticaDayTrade").jqGrid(
    {
        url: "Compliance/Formularios/EstatisticaDayTrade.aspx?Acao=BuscarEstatisticaDayTrade"
      , datatype   : "json"
      , mtype      : "POST"
      , hoverrows  : false
      , postData   : pParametros
      , autowidth  : false
      , shrinkToFit: false
      , colModel: [
                        { label: "Id"                           , name: "Id"                         ,jsonmap: "Id"                         , index: "Id"                         , width: 29, align: "center", sortable: false }
                      , { label: "Assessor"                     , name: "CodigoAssessorOculto"       ,jsonmap: "CodigoAssessor"             , index: "CodigoAssessor"             , width: 29, align: "center", sortable: false }
                      , { label: "Assessor"                     , name: "CodigoAssessor"             ,jsonmap: "CodigoAssessor"             , index: "CodigoAssessor"             , width: 40,  align: "center", sortable: false }
                      , { label: "Nome Assessor"                , name: "NomeAssessor"               ,jsonmap: "NomeAssessor"               , index: "NomeAssessor"               , width: 215, align: "left"  , sortable: false }
                      , { label: "Cliente"                      , name: "CodigoCliente"              ,jsonmap: "CodigoCliente"              , index: "CodigoCliente"              , width: 55,  align: "center", sortable: false }
                      , { label: "Nome Cliente"                 , name: "NomeCliente"                ,jsonmap: "NomeCliente"                , index: "NomeCliente"                , width: 200, align: "left"  , sortable: false }
                      , { label: "Idade"                        , name: "Idade"                      ,jsonmap: "Idade"                      , index: "Idade"                      , width: 45,  align: "center", sortable: false }
                      , { label: "Data Negócio"                 , name: "DataNegocio"                ,jsonmap: "DataNegocio"                , index: "DataNegocio"                , width: 105, align: "center", sortable: false }
                      , { label: "NET"                          , name: "NET"                        ,jsonmap: "NET"                        , index: "NET"                        , width: 75,  align: "right", sortable: true }
                      , { label: "% Positivo"                   , name: "PercentualPositivo"         ,jsonmap: "PercentualPositivo"         , index: "PercentualPositivo"         , width: 65,  align: "right", sortable: false }
                      , { label: "Pessoa Vinculada"             , name: "PessoaVinculada"            ,jsonmap: "PessoaVinculada"            , index: "PessoaVinculada"            , width: 75,  align: "center", sortable: false }
                      , { label: "Qtde DayTrade"                , name: "QuantidadeDayTrade"         ,jsonmap: "QuantidadeDayTrade"         , index: "QuantidadeDayTrade"         , width: 85, align: "center",  sortable: false }
                      , { label: "Qtde DayTrade +"              , name: "QuantidadeDayTradePositivo" ,jsonmap: "QuantidadeDayTradePositivo" , index: "QuantidadeDayTradePositivo" , width: 85, align: "center",  sortable: false }
                      , { label: "Tipo Bolsa"                   , name: "TipoBolsa"                  ,jsonmap: "TipoBolsa"                  , index: "TipoBolsa"                  , width: 55,  align: "right", sortable: false }
                      , { label: "Valor Negativo"               , name: "ValorNegativo"              ,jsonmap: "ValorNegativo"              , index: "ValorNegativo"              , width: 78,  align: "right", sortable: true }
                      , { label: "Valor Positivo"               , name: "ValorPositivo"              ,jsonmap: "ValorPositivo"              , index: "ValorPositivo"              , width: 75,  align: "right", sortable: false }
                    ]
      , height     : "100%"
      , width      : 1325
      , rowNum     : 0
      , sortname   : "invid"
      , sortable   : true
      , sortorder  : "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Compliance_Estatisticas_ItemDataBound
  }).jqGrid('hideCol', ['Id', 'CodigoAssessorOculto']);
}

function GradIntra_Compliance_Estatisticas_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_Compliance_Resultados_EstatisticaDayTrade tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');

    if (rowelem.NET.indexOf("-") > -1) 
    {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }

    if (rowelem.ValorNegativo.indexOf("-") > -1) 
    {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' })
    }
}

var gFirstClickBuscarComplianceNegociosDiretos = true;

function GradIntra_Compliance_NegociosDiretos(pParametros) 
{
    if (gFirstClickBuscarComplianceNegociosDiretos != true) 
    {
        $("#tblBusca_Compliance_Resultados_NegociosDiretos")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarComplianceNegociosDiretos = false;

    $("#tblBusca_Compliance_Resultados_NegociosDiretos").jqGrid(
    {
        url: "Compliance/Formularios/NegociosDiretos.aspx?Acao=BuscarNegociosDiretos"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                         { label: "Id"                      , name: "Id"                        , jsonmap: "Id"                      , index: "Id"                       , width: 55,  align: "center", sortable: true }
                       , { label: "Numero Negócio"          , name: "NumeroNegocio"             , jsonmap: "NumeroNegocio"           , index: "NumeroNegocio"            , width: 80, align: "center", sortable: true }
                       , { label: "Cod. Cliente Vendedor"   , name: "CodigoClienteVendedor"     , jsonmap: "CodigoClienteVendedor"   , index: "CodigoClienteVendedor"    , width: 70,  align: "center", sortable: true }
                       , { label: "Nome Cliente Vendedor"   , name: "NomeClienteVendedor"       , jsonmap: "NomeClienteVendedor"     , index: "NomeClienteVendedor"      , width: 310, align: "left"  , sortable: true }
                       , { label: "Sentido"                 , name: "Sentido"                   , jsonmap: "Sentido"                 , index: "Sentido"                  , width: 55,  align: "center", sortable: true }
                       , { label: "Instrumento"             , name: "Instrumento"               , jsonmap: "Instrumento"             , index: "Instrumento"              , width: 70,  align: "center", sortable: true }
                       , { label: "Pessoa Vinculada"        , name: "PessoaVinculadaVendedor"   , jsonmap: "PessoaVinculadaVendedor" , index: "PessoaVinculadaVendedor"  , width: 75,  align: "center", sortable: true }
                       , { label: "Data Negocio"            , name: "DataNegocio"               , jsonmap: "DataNegocio"             , index: "DataNegocio"              , width: 85,  align: "center", sortable: true }
                       , { label: "Cod. Cliente Comprador"  , name: "CodigoClienteComprador"    , jsonmap: "CodigoClienteComprador"  , index: "CodigoClienteComprador"   , width: 85, align: "center", sortable: true }
                       , { label: "Nome Cliente Comprador"  , name: "NomeClienteComprador"      , jsonmap: "NomeClienteComprador"    , index: "NomeClienteComprador"     , width: 310, align: "left", sortable: true }
                       , { label: "Pessoa Vinculada"        , name: "PessoaVinculadaComprador"  , jsonmap: "PessoaVinculadaComprador", index: "PessoaVinculadaComprador" , width: 75, align: "center", sortable: true }
                       
                    ]
      , height: 600
      , width: 1250
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page   : "PaginaAtual"
                       , total  : "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Compliance_NegociosDiretos_ItemDataBound
  }).jqGrid('hideCol', ['Id', 'Sentido']); ;
}

function GradIntra_Compliance_NegociosDiretos_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_Compliance_Resultados_NegociosDiretos tr[id=" + rowid + "]");

    lRow.css("height", 30);

    lRow.css("background-color", 'white');
}

function btnCompliance_ImprimirRelatorio_NegociosDiretos(pSender) 
{
    var lUrl = "Compliance/Formularios/NegociosDiretos.aspx?Acao=CarregarComoCSV";

    window.open(lUrl);

    return false;
}

function btnCompliance_ImprimirRelatorio_EstatisticaDayTrade(pSender) 
{
    var lUrl = "Compliance/Formularios/EstatisticaDayTrade.aspx?Acao=CarregarComoCSV";

    window.open(lUrl);

    return false;
}

function btnCompliance_ImprimirRelatorio_OrdensAlteradasDayTrade(pSender) 
{
    var lUrl = "Compliance/Formularios/OrdensAlteradasDayTrade.aspx?Acao=CarregarComoCSV";

    window.open(lUrl);

    return false;
}


//-->> Churning
var gFirstClickBuscarComplianceChurning = true;

function GradIntra_Compliance_Churning_Intraday(pParametros) 
{
    if (gFirstClickBuscarComplianceChurning != true) 
    {
        $("#tblBusca_Compliance_Churning_Intraday")
            .setGridParam({ postData: pParametros })
                .trigger("reloadGrid");

        return;
    }

    gFirstClickBuscarComplianceChurning = false;

    $("#tblBusca_Compliance_Churning_Intraday").jqGrid(
    {
        url: "Churning.aspx?Acao=BuscarChurning"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , postData: pParametros
      , autowidth: false
      , shrinkToFit: false
      , colModel: [
                         { label: "Check",              name: "check",                  jsonmap: "check",                   index: "check",                 width: 30,  align: "center",    sortable: true }
                       , { label: "Cliente",            name: "CodigoCliente",          jsonmap: "CodigoCliente",           index: "CodigoCliente",         width: 50,  align: "center",    sortable: true }
                       , { label: "Nome Cliente",       name: "NomeCliente",            jsonmap: "NomeCliente",             index: "NomeCliente",           width: 250, align: "left",      sortable: true }
                       , { label: "Tipo",               name: "Tipo",                   jsonmap: "TipoPessoa",              index: "TipoPessoa",            width: 50,  align: "center",    sortable: true }
                       , { label: "Assessor",           name: "CodigoAssessor",         jsonmap: "CodigoAssessor",          index: "CodigoAssessor",        width: 30,  align: "center",    sortable: true }
                       , { label: "Nome Assessor",      name: "NomeAssessor",           jsonmap: "NomeAssessor",            index: "NomeAssessor",          width: 250, align: "left",      sortable: true }
                       , { label: "TR no Dia",          name: "PercentualTRnoDia",      jsonmap: "PercentualTRnoDia",       index: "PercentualTRnoDia",     width: 70,  align: "center",    sortable: true }
                       , { label: "TR no Período",      name: "PercentualTRnoPeriodo",  jsonmap: "PercentualTRnoPeriodo",   index: "PercentualTRnoPeriodo", width: 70,  align: "center",    sortable: true }
                       , { label: "TotalCompras",       name: "ValorCompras",           jsonmap: "ValorCompras",            index: "ValorCompras",          width: 70,  align: "center",    sortable: true }
                       , { label: "Corretagem $",       name: "Corretagem",             jsonmap: "Corretagem",              index: "Corretagem",            width: 70, align: "center", sortable: true }
                       , { label: "CE no Dia",          name: "PercentualCEnoDia",      jsonmap: "PercentualCEnoDia",       index: "PercentualCEnoDia",     width: 70,  align: "center",    sortable: true }
                       , { label: "CE no Período",      name: "PercentualCEnoPeriodo",  jsonmap: "PercentualCEnoPeriodo",   index: "PercentualCEnoPeriodo", width: 70,  align: "center",    sortable: true }
                       , { label: "TotalVendas",        name: "ValorVendas",            jsonmap: "ValorVendas",             index: "ValorVendas",           width: 70,  align: "center",    sortable: true }
                       , { label: "Carteira Média",     name: "ValorCarteira",          jsonmap: "ValorCarteira",           index: "ValorCarteira",         width: 70,  align: "center",    sortable: true }
                       , { label: "Carteira Dia",       name: "ValorCarteiraDia",       jsonmap: "ValorCarteiraDia",        index: "ValorCarteiraDia",      width: 70,  align: "center",    sortable: true }
                       , { label: "L1",                 name: "L1",                     jsonmap: "ValorL1",                 index: "ValorL1",               width: 70,  align: "center",    sortable: true }
                       , { label: "Data",               name: "Data",                   jsonmap: "Data",                    index: "Data",                  width: 70,  align: "center",    sortable: true }
                       , { label: "Portas",             name: "Portas",                 jsonmap: "Portas",                  index: "Portas",                width: 70,  align: "left",      sortable: true }
                    ]
      , height: 600
      , width: 1250
      , rowNum: 0
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow: GradIntra_Compliance_Churning_Intraday_ItemDataBound
  });
    //.jqGrid('hideCol', ['Id', 'Sentido']); ;
}

function GradIntra_Compliance_Churning_Intraday_ItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_Compliance_Churning_Intraday tr[id=" + rowid + "]");

    lRow.css("height", 25);

    lRow.css("background-color", 'white');

    if (rowelem.PercentualTRnoDia.indexOf("-") > -1) 
    {
        lRow.children("td").eq(6).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PercentualTRnoPeriodo.indexOf("-") > -1) 
    {
        lRow.children("td").eq(7).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PercentualCEnoDia.indexOf("-") > -1) 
    {
        lRow.children("td").eq(10).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.PercentualCEnoPeriodo.indexOf("-") > -1) 
    {
        lRow.children("td").eq(11).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.ValorCarteira.indexOf("-") > -1) 
    {
        lRow.children("td").eq(13).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }

    if (rowelem.ValorCarteiraDia.indexOf("-") > -1) 
    {
        lRow.children("td").eq(14).css({ fontWeight: 'normal', fontSize: '12px', color: 'red' });
    }
    
    lRow.children("td").eq(0).html("<img alt=\"\" style=\"cursor: pointer\" onclick=\"return GradIntra_Compliance_Churning_AbrirDetalhamento_click(this)\" src=\"../../../../Skin/Default/Img/Ico_Busca.png\" />");
}

function rdoRisco_Churning_Intraday_PercentualTR(pSender) 
{
    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal', fontSize: '11px' }) });

    $("label[for='" + lId + "']").css({ fontWeight: 'bold', fontSize: '14px' });
}

function rdoRisco_Churning_Intraday_PercentualCE(pSender) 
{
    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal', fontSize: '11px' }) });

    $("label[for='" + lId + "']").css({ fontWeight: 'bold', fontSize: '14px' });
}

function rdoRisco_Churning_Intraday_TotalCompras(pSender) 
{
    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal', fontSize: '11px' }) });

    $("label[for='" + lId + "']").css({ fontWeight: 'bold', fontSize: '14px' });
}

function rdoRisco_Churning_Intraday_TotalVendas(pSender) 
{
    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal', fontSize: '11px' }) });

    $("label[for='" + lId + "']").css({ fontWeight: 'bold', fontSize: '14px' });
}

function rdoRisco_Churning_Intraday_CarteiraMedia(pSender) 
{
    var lId = $(pSender).attr('id')

    $("label").each(function () { $(this).css({ fontWeight: 'normal', fontSize: '11px' }) });

    $("label[for='" + lId + "']").css({ fontWeight: 'bold', fontSize: '14px' });
}

var gStatusPararContagemChurning = false;
var gTimerChurningAtualizacaoAutomatica = 0;

function GradIntra_Compliance_Churning_Intraday_Buscar(pSender, pConsulta) 
{
    gStatusPararContagemChurning = true;

    gTimerChurningAtualizacaoAutomatica = 0; //--> Zerando o contador.

    //$("#pnlCompliance_Churning_Intraday").show();

    $("#pnlCompliance_Churning_Intraday").fadeIn();

    $(".pnlCompliance_Churning_Periodo").html("Período De: " + pConsulta.DataDe + " até: " + pConsulta.DataAte );

    GradIntra_Compliance_Churning_Intraday(pConsulta);

    var lHorarioAtual = new Date();

    window.setTimeout(function () 
    {
        var lUltimaAtualizacao = lHorarioAtual.getDate()
                                 + "/" + (lHorarioAtual.getMonth() + 1)
                                 + "/" + (lHorarioAtual.getFullYear())
                                 + " " + (lHorarioAtual.getHours().toString().length == 1 ? "0" + lHorarioAtual.getHours().toString() : lHorarioAtual.getHours().toString())
                                 + ":" + (lHorarioAtual.getMinutes().toString().length == 1 ? "0" + lHorarioAtual.getMinutes().toString() : lHorarioAtual.getMinutes().toString())
                                 + ":" + (lHorarioAtual.getSeconds().toString().length == 1 ? "0" + lHorarioAtual.getSeconds().toString() : lHorarioAtual.getSeconds().toString());

        gStatusPararContagemChurning = false;

        gTimerChurningAtualizacaoAutomatica = 60; //--> Setando o contador.

        $("#lblRisco_Monitoramento_HoraUltimaConsulta").html(lUltimaAtualizacao)
                                                               .effect("highlight", {}, 7000)
                                                               .prev().effect("highlight", {}, 7000);


        //$("#pnlResultado_MonitoramentoRisco").show(); //--> Exibindo o grid da pesquisa.
        GradIntra_Compliance_Churning_AtualizarAutomaticamente();
    }, 700);
}

function GradIntra_Compliance_Churning_AtualizarAutomaticamente() 
{
    if (!gStatusPararContagemChurning && $("#chkRisco_Compliance_Churning_AtualizarAutomaticamente").is(':checked')) 
    {
        $("#lblRisco_Compliance_Churning_AtualizarAutomaticamente_ContagemRegressiva").show();

        if ((gTimerChurningAtualizacaoAutomatica - 1) >= 0) 
        {
            gTimerChurningAtualizacaoAutomatica = gTimerChurningAtualizacaoAutomatica - 1;
            $("#lblRisco_Compliance_Churning_AtualizarAutomaticamente_ContagemRegressiva").html('Próxima atualização em ' + gTimerChurningAtualizacaoAutomatica + ' segundos.')
            setTimeout('GradIntra_Compliance_Churning_AtualizarAutomaticamente()', 1000);
        }
        else 
        {
            GradIntra_Compliance_Churning_Intraday_Buscar();
        }
    }

    return false;
}

function Compliance_Churning_AtualizarAutomaticamente(pSender) 
{
    if ($(pSender).is(":checked")) 
    {
        gTimerChurningAtualizacaoAutomatica = 60;
        GradIntra_Compliance_Churning_Intraday_Buscar();
    }
    else {
        gTimerChurningAtualizacaoAutomatica = 0; //--> Zerando o contador.
        $("#lblRisco_Compliance_Churning_AtualizarAutomaticamente_ContagemRegressiva").hide();
    }
}

var lPopUpDetalhamento = null;

function GradIntra_Compliance_Churning_AbrirDetalhamento_click(pSender) 
{
    var codigo = $(pSender).parent("td").parent("tr").find("td").eq(1).html(); //--> Código Bovespa do cliente

    var codigoBmf = codigo;//  $(pSender).parent("td").parent("tr").find("td").eq(13).html(); //--> Código bmf do cliente

    var lAssessor = $(pSender).parent("td").parent("tr").find("td").eq(3).html(); //--> Assessor do cliente

    var lNomeCliente = $(pSender).parent("td").parent("tr").find("td").eq(2).html(); //--> Nome do cliente

    var lNomeAssessor = $(pSender).parent("td").parent("tr").find("td").eq(4).html(); //--> Nome do assessor

    if (lPopUpDetalhamento == null) 
    {
        lPopUpDetalhamento = window.open('../../../Risco/Formularios/Dados/MonitoramentoRiscoGeralDetalhamento.aspx', codigo + ' - ' + lNomeCliente, 'height=800, width=1200,scrollbars=1, status=0, toolbar=0, location=0, directories=0, menubar=0, resizable=1, fullscreen=1');

        lPopUpDetalhamento.gCodigoBovespaDetalhes = codigo;

        lPopUpDetalhamento.gCodigoBMFDetalhes = codigoBmf;

        lPopUpDetalhamento.gCodigoAssessor = lAssessor;

        lPopUpDetalhamento.gNomeCliente = lNomeCliente;

        lPopUpDetalhamento.gNomeAssessor = lNomeAssessor;
    }

    if (lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca == null) 
    {
        setTimeout(function () { GradIntra_Compliance_Churning_AbrirDetalhamento_click(pSender) }, 1000);
    }
    else 
    {
        var lObjetoDeParametros = { CodigoCliente: codigo, CodigoClienteBmf: codigoBmf, Assessor: lAssessor, NomeCliente: lNomeCliente, NomeAssessor: lNomeAssessor };

        lPopUpDetalhamento.gTimerMonitoramentoDetalhesAtualizacaoAutomatica = 60;

        lPopUpDetalhamento.GradIntra_Monitoramento_LucroPrejuizo_ResumoCliente_Busca_Pela_Grid(this, lObjetoDeParametros);

        ////lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucrosPrejuizos_AbrirDetalhamentoDaGrid_Busca(this, lObjetoDeParametros);
        //lPopUpDetalhamento.GradIntra_Risco_Monitoramento_LucroPrejuizo_ConfigurarColunas(this, pIdJanela);

        lPopUpDetalhamento = null;
    }

    return false;
}

function btnCompliance_ImprimirRelatorio_Churning(pSender) 
{
    var lUrl = "Churning.aspx?Acao=ExportarExcell";

    window.open(lUrl);

    return false;
}