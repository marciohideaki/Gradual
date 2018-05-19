/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="04-GradIntra-Cadastro.js" />
/// <reference path="05-GradIntra-Relatorio.js" />

/*

    Funções de Busca

*/

function GradIntra_Busca_ConfigurarGridDeResultados_Clientes()
{
    
    $("#tblBusca_Clientes_Resultados").jqGrid(
    {
        url:        "Buscas/Clientes.aspx?Acao=Paginar"
      , datatype:   "json"
      , mtype:      "GET"
      , colModel :  [ 
                        //{"Id","CodGradual","CodBovespa","CodBMF","NomeCliente","CPF","Status","Passo","DataCadastroString","FlagPendencia","DataNascimentoString","Email","Sexo","TipoCliente"}
                        { label:"Código",         name:"Id",                 jsonmap:"Id",                   index:"Id",                 width:70,   align:"right",  sortable:false  } 
                      , { label:"Cód Assessor",   name:"CodAssessor",        jsonmap:"CodAssessor",          index:"CodAssessor",        width:50,   align:"left",   sortable:false, hidden:true  }
                      , { label:"Cód. Bov.",      name:"CodBovespa",         jsonmap:"CodBovespa",           index:"CodBovespa",         width:70,   align:"right",  sortable:false, hidden:true  } 
                      , { label:"Cód. Bmf.",      name:"CodBMF",             jsonmap:"CodBMF",               index:"CodBMF",             width:70,   align:"right",  sortable:false, hidden:true  } 
                                                                                                                                                                     
                      , { label:"Cód. Bov.",      name:"CodBovespaComConta", jsonmap:"CodBovespaComConta",   index:"CodBovespaComConta", width:70,   align:"right",  sortable:false  } 
                      , { label:"Cód. Bmf.",      name:"CodBMFComConta",     jsonmap:"CodBMFComConta",       index:"CodBMFComConta",     width:70,   align:"right",  sortable:false  } 
                                                                                                                                                                     
                      , { label:"Nome",           name:"NomeCliente",        jsonmap:"NomeCliente",          index:"NomeCliente",        width:360,  align:"left",   sortable:false  } 
                      , { label:"CPF / CNPJ",     name:"CPF",                jsonmap:"CPF",                  index:"CPF",                width:70,   align:"left",   sortable:false  } 
                      , { label:"Status",         name:"Status",             jsonmap:"Status",               index:"Status",             width:50,   align:"left",   sortable:false, hidden:true  }
                      , { label:"Tipo",           name:"TipoCliente",        jsonmap:"TipoCliente",          index:"TipoCliente",        width:50,   align:"center", sortable:false  } 
                      , { label:"Sexo",           name:"Sexo",               jsonmap:"Sexo",                 index:"Sexo",               width:50,   align:"left",   sortable:false, hidden:true  }
                      , { label:"Passo",          name:"Passo",              jsonmap:"Passo",                index:"Passo",              width:50,   align:"left",   sortable:false, hidden:true  }
                      , { label:"Tem Pendência",  name:"FlagPendencia",      jsonmap:"FlagPendencia",        index:"FlagPendencia",      width:50,   align:"left",   sortable:false, hidden:true  }
                      , { label:"Email",          name:"Email",              jsonmap:"Email",                index:"Email",              width:50,   align:"left",   sortable:false, hidden:true  }
                      , { label:"Data de Cad.",   name:"DataCadastro",       jsonmap:"DataCadastroString",   index:"DataCadastro",       width:100,  align:"center", sortable:false  }
                      , { label:"Data de Recad.", name:"DataRecadastro",     jsonmap:"DataRecadastroString", index:"DataRecadastro",     width:100,  align:"left",   sortable:false, hidden:true  }
                      , { label:"Cliente CISE",   name:"Cise",               jsonmap:"Cise",                 index:"Cise",               width:50,   align:"left",   sortable:false, hidden:true  } 
                      , { label:"Cód. Bov.",      name:"CodBovespaAtiva",    jsonmap:"CodBovespaAtiva",      index:"CodBovespaAtiva",    width:70,   align:"right",  sortable:false, hidden:true  } 
                      , { label:"Cód. Bmf.",      name:"CodBMFAtiva",        jsonmap:"CodBMFAtiva",          index:"CodBMFAtiva",        width:70,   align:"right",  sortable:false, hidden:true  } 
                    ]
      , height:     90
      , pager:      "#pnlBusca_Clientes_Resultados_Pager"
      , rowNum:     20
      , sortname:   "invid"
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
      , ondblClickRow: GradIntra_Busca_SelecionarItensIndividualmente
    }); 

    //Chama o evento de page resize pra já ajustar:
    PageResize();
}

function GradIntra_Busca_ConfigurarGridDeResultados_ReservaIPO()
{
    $("#tblBusca_ReservaIPO_Resultados").jqGrid(
    {
        url: "Clientes/Formularios/ReservarIPO.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [
                        { label: "Código",          name: "Id",             jsonmap: "Id",                  index: "Id",            width: 70, align: "right", sortable: false }
                      , { label: "Cód Assessor",    name: "CodAssessor",    jsonmap: "CodAssessor",         index: "CodAssessor",   width: 50, align: "left", sortable: false, hidden: true }
                      , { label: "Cód. Bov.",       name: "CodBovespa",     jsonmap: "CodBovespa",          index: "CodBovespa",    width: 70, align: "right", sortable: false }
                      , { label: "Cód. Bmf.",       name: "CodBMF",         jsonmap: "CodBMF",              index: "CodBMF",        width: 70, align: "right", sortable: false }
                      , { label: "Nome",            name: "NomeCliente",    jsonmap: "NomeCliente",         index: "NomeCliente",   width: 290, align: "left", sortable: false }
                      , { label: "CPF / CNPJ",      name: "CPF",            jsonmap: "CPF",                 index: "CPF",           width: 70, align: "left", sortable: false }
                      , { label: "Tipo",            name: "TipoCliente",    jsonmap: "TipoCliente",         index: "TipoCliente",   width: 50, align: "left", sortable: false }
                      , { label: "Data de Cad.",    name: "DataCadastro",   jsonmap: "DataCadastroString",  index: "DataCadastro",  width: 100, align: "left",   sortable: false }
                      , { label: "Reservar IPO",    name: "ReservarIPO",    jsonmap: "Check",               index: "Check",         width: 70, align: "center", sortable: false }  
                    ]
      , height: 470
      , pager: "#pnlBusca_ReservaIPO_Resultados_Pager"
      , rowNum: 20
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader: { root: "Itens"
                    , page: "PaginaAtual"
                    , total: "TotalDePaginas"
                    , records: "TotalDeItens"
                    , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id: "0"              //primeira propriedade do elemento de linha é o Id
                    , repeatitems: false
      }
      , afterInsertRow: GradIntra_Busca_ReservaIPOItemDataBound
      , beforeSelectRow: GradIntra_Busca_OrdensSelectRow
  });

    //Chama o evento de page resize pra já ajustar:
    $("#tblBusca_ReservaIPO_Resultados").setGridWidth( 958 );
}


function GradIntra_Busca_ConfigurarGridDeResultados_Seguranca() 
{
    $("#tblBusca_Seguranca_Resultados").jqGrid(
    {
        url: "Buscas/Seguranca.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [
        //{"Id","CodGradual","CodBovespa","CodBMF","NomeCliente","CPF","Status","Passo","DataCadastroString","FlagPendencia","DataNascimentoString","Email","Sexo","TipoCliente"}

                        { label: "Descrição"     , name: "Descricao"   , jsonmap: "Descricao"   , index: "Descricao"   , width: 480, align: "left", sortable: false }
                      , { label: "Código"        , name: "Id"          , jsonmap: "Id"          , index: "Id"          , width: 270, align: "left", sortable: false }
                      , { label: "Tipo De Objeto", name: "TipoDeObjeto", jsonmap: "TipoDeObjeto", index: "TipoDeObjeto", width: 0  , align: "left", sortable: false, hidden: true }
                      , { label: "Nome"          , name: "Nome"        , jsonmap: "Nome"        , index: "Nome"        , width: 0  , align: "left", sortable: false, hidden: true }
                      , { label: "Email"         , name: "Email"       , jsonmap: "Email"       , index: "Email"       , width: 0  , align: "left", sortable: false, hidden: true }
                    ]
      , height: 96
      , pager: "#pnlBusca_Seguranca_Resultados_Pager"
      , rowNum: 20
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: true   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , ondblClickRow: GradIntra_Busca_SelecionarItensIndividualmente
    });

    //Chama o evento de page resize pra já ajustar:
    PageResize();
}


function GradIntra_Busca_Aplicacao()
{
       $("#tb_busca_Aplicacao").jqGrid(
    {
        //url: "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx?Acao=CarregarAplicacao&CodigoCliente=" + pObjetoDeParametros.CodigoCliente + "&status=" + pObjetoDeParametros.IdStatus + "&DataFinal=" + pObjetoDeParametros.DataFinal + "&DataInicial=" + pObjetoDeParametros.DataInicial
        url: "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx?Acao=CarregarAplicacao"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , colModel: [
                          { label: "Aprovar"                , name: "CodigoTabela"          , jsonmap: "Check"              , index: "Check"              , width: 15  , align: "center", sortable: false }  
                      ,   { label: "CdStatus"               , name: "CodigoStatus"         , jsonmap: "CodigoStatus"        , index: "CodigoStatus"       , width: 15  , align: "center", sortable: false, hidden: true }  
                      , { label: "Cd Cliente"               , name: "CodigoCliente"         , jsonmap: "CodigoCliente"      , index: "CodigoCliente"      , width: 15  , align: "center", sortable: false }
                      , { label: "Valor Aplicação R$"       , name: "ValorSolicitado"       , jsonmap: "ValorSolicitado"    , index: "ValorSolicitado"    , width: 26  , align: "center", sortable: false }
                      , { label: "Descição Produto"         , name: "DescricaoProduto"      , jsonmap: "DescricaoProduto"   , index: "DescricaoProduto"   , width: 30  , align: "center", sortable: false }
                      , { label: "Data Solicitação"         , name: "Data Solicitação"      , jsonmap: "DtSolicitacao"      , index: "DtSolicitacao"      , width: 22  , align: "center", sortable: false }
                      , { label: "Status"                   , name: "DescricaoStatus"       , jsonmap: "DescricaoStatus"    , index: "DescricaoStatus"    , width: 35  , align: "center", sortable: false }
                      , { label: "Descrição da Aplicacao"   , name: "Descricao"             , jsonmap: "Descricao"          , index: "Descricao"          , width: 135 , align: "center", sortable: false }
                      
                      
                    ]
      , height: 250
      , pager: "#pnlBusca_Aplicacao_Resultados_Pager"
      //, rowNum: 10
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
                       , id: "CodigoTabela"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow    : GradIntra_Busca_AplicacaoItemDataBound
//      , beforeSelectRow   : GradIntra_Busca_AplicacaoSelectRow
//      , subGridRowExpanded: GradIntra_Busca_AplicacaoSubgrid
    });

    //Chama o evento de page resize pra já ajustar:
    $("#tb_busca_Aplicacao").setGridWidth( 960 );
    //PageResize();
}


function GradIntra_Busca_Resgate()
{
       $("#tb_busca_Resgate").jqGrid(
    {
        //url: "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx?Acao=CarregarResgate&CodigoCliente=" + pObjetoDeParametros.CodigoCliente + "&status=" + pObjetoDeParametros.IdStatus + "&DataFinal=" + pObjetoDeParametros.DataFinal + "&DataInicial=" + pObjetoDeParametros.DataInicial
        url: "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx?Acao=CarregarResgate"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , colModel: [
                          { label: "Aprovar"                , name: "CodigoTabela"          , jsonmap: "Check"              , index: "Check"              , width: 15  , align: "center", sortable: false }  
                      ,   { label: "CdStatus"               , name: "CodigoStatus"         , jsonmap: "CodigoStatus"        , index: "CodigoStatus"       , width: 15  , align: "center", sortable: false, hidden: true }  
                      , { label: "Cd Cliente"               , name: "CodigoCliente"         , jsonmap: "CodigoCliente"      , index: "CodigoCliente"      , width: 15  , align: "center", sortable: false }
                      , { label: "Valor Aplicação R$"       , name: "ValorSolicitado"       , jsonmap: "ValorSolicitado"    , index: "ValorSolicitado"    , width: 26  , align: "center", sortable: false }
                      , { label: "Descição Produto"         , name: "DescricaoProduto"      , jsonmap: "DescricaoProduto"   , index: "DescricaoProduto"   , width: 30  , align: "center", sortable: false }
                      , { label: "Data Solicitação"         , name: "Data Solicitação"      , jsonmap: "DtSolicitacao"      , index: "DtSolicitacao"      , width: 22  , align: "center", sortable: false }
                      , { label: "Status"                   , name: "DescricaoStatus"       , jsonmap: "DescricaoStatus"    , index: "DescricaoStatus"    , width: 35  , align: "center", sortable: false }
                      , { label: "Descrição Resgate"        , name: "Descricao"             , jsonmap: "Descricao"          , index: "Descricao"          , width: 135 , align: "center", sortable: false }
                      
                      
                    ]
      , height: 250
      , pager: "#pnlBusca_Resgate_Resultados_Pager"
      //, rowNum: 20
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: false
      , jsonReader: {
                         root: "Itens2"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
        , afterInsertRow    : GradIntra_Busca_ResgateItemDataBound
//      , beforeSelectRow   : GradIntra_Busca_AplicacaoSelectRow
//      , subGridRowExpanded: GradIntra_Busca_AplicacaoSubgrid
    });

    //Chama o evento de page resize pra já ajustar:
    $("#tb_busca_Resgate").setGridWidth( 960 );
    //PageResize();
}

function GradIntra_Busca_ConfigurarGridDeResultados_OrdensStop()
{
    $("#tblBusca_OrdensStop_Resultados").jqGrid(
    {
        url: "Monitoramento/ResultadoOrdensStop.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "POST"
      , hoverrows: false
      , colModel: [
                        { label: "Cancel"          , name: "Cancel"                , jsonmap: "Check"              , index: "Check"                , width: 29  , align: "center", sortable: false }  
                      , { label: "Numero da Ordem" , name: "Id"                    , jsonmap: "Id"                 , index: "Id"                   , width: 20  , align: "center", sortable: false }
                      , { label: "Cliente"         , name: "Cliente"               , jsonmap: "Cliente"            , index: "Cliente"              , width: 65  , align: "center", sortable: false }
                      , { label: "Papel"           , name: "Papel"                 , jsonmap: "Papel"              , index: "Papel"                , width: 54  , align: "center", sortable: false }
                      , { label: "Qtde"            , name: "Quantidade"            , jsonmap: "Quantidade"         , index: "Quantidade"           , width: 54  , align: "center", sortable: false }
                      , { label: "R$ Disparo Loss" , name: "PrecoDisparo"          , jsonmap: "PrecoDisparo"       , index: "PrecoDisparo"         , width: 75  , align: "center", sortable: false }
                      , { label: "R$ Lanç. Loss"   , name: "PrecoLancamento"       , jsonmap: "PrecoLancamento"    , index: "PrecoLancamento"      , width: 75  , align: "center", sortable: false }

                      , { label: "R$ Disparo Gain" , name: "PrecoDisparoGain"      , jsonmap: "PrecoDisparoGain"   , index: "PrecoDisparoGain"     , width: 75  , align: "center", sortable: false }
                      , { label: "R$ Lanç. Gain"   , name: "PrecoLancamentoGain"   , jsonmap: "PrecoLancamentoGain", index: "PrecoLancamentoGain"  , width: 75  , align: "center", sortable: false }

                      , { label: "Inicio móvel"    , name: "PrecoInicioMovel"      , jsonmap: "PrecoInicioMovel"   , index: "PrecoInicioMovel"     , width: 75  , align: "center", sortable: false }
                      , { label: "Ajuste Movel"    , name: "PrecoAjusteMovel"      , jsonmap: "PrecoAjusteMovel"   , index: "PrecoAjusteMovel"     , width: 75  , align: "center", sortable: false }
                      , { label: "Preço Envio"     , name: "PrecoEnvio"            , jsonmap: "PrecoEnvio"         , index: "PrecoEnvio"           , width: 75  , align: "center", sortable: false }
                      , { label: "Status"          , name: "Status"                , jsonmap: "Status"             , index: "Status"               , width: 60  , align: "center", sortable: false }
                      , { label: "Tipo"            , name: "Tipo"                  , jsonmap: "Tipo"               , index: "Tipo"                 , width: 74  , align: "center", sortable: false }
                      , { label: "Hora"            , name: "Hora"                  , jsonmap: "Hora"               , index: "Hora"                 , width: 60  , align: "center", sortable: false }
                      , { label: "L/G"             , name: "LossGain"              , jsonmap: "LossGain"           , index: "LossGain"             , width: 54  , align: "center", sortable: false }
                      , { label: "Validade"        , name: "Validade"              , jsonmap: "Validade"           , index: "Validade"             , width: 89  , align: "center", sortable: false }
                     
                      //, { label: "Envida"           , name: "Envida"                , jsonmap: "Envida"             , index: "Envida"               , width: 10  , align: "left", sortable: false }
                    ]
      , height: 470
      , pager: "#pnlBusca_OrdensStop_Resultados_Pager"
      , rowNum: 20
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: {
                         root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
      }
      , afterInsertRow    : GradIntra_Busca_OrdensStopItemDataBound
      , beforeSelectRow   : GradIntra_Busca_OrdensSelectRow
      , subGridRowExpanded: GradIntra_Busca_OrdensStopSubgrid
    });

    //Chama o evento de page resize pra já ajustar:
    PageResize();
}

//function GradIntra_Busca_ConfigurarGridDeResultados_OrdensNovoOMS() {
//    $("#tblBusca_OrdensNovoOMS_Resultados").jqGrid(
//    {
//        url: "Monitoramento/ResultadoOrdensNovoOMS.aspx?Acao=Paginar"
//      , datatype: "json"
//      , mtype: "POST"
//      , colModel: [
//                        { label: "Cancel",      name: "Cancel",                 jsonmap: "Check",               index: "Check",                 width: 42, align: "center", sortable: false }
//                      , { label: "Id",          name: "Id",                     jsonmap: "Id",                  index: "Id",                    width: 173, align: "center", sortable: false }
//                      , { label: "Cliente",     name: "CodigoCliente",          jsonmap: "CodigoCliente",       index: "CodigoCliente",         width: 74, align: "center", sortable: false }
//                      , { label: "Hora",        name: "Hora",                   jsonmap: "Hora",                index: "Hora",                  width: 54, align: "center", sortable: false }
//                      , { label: "Status",      name: "Status",                 jsonmap: "Status",              index: "Status",                width: 85, align: "center", sortable: false }
//                      , { label: "C/V",         name: "CompraVenda",            jsonmap: "CompraVenda",         index: "CompraVenda",           width: 46, align: "center", sortable: false }
//                      , { label: "Papel",       name: "Papel",                  jsonmap: "Papel",               index: "Papel",                 width: 58, align: "center", sortable: false }
//                      , { label: "Quantidade",  name: "Quantidade",             jsonmap: "Quantidade",          index: "Quantidade",            width: 95, align: "center", sortable: false }
//                      , { label: "Preço",       name: "Preco",                  jsonmap: "Preco",               index: "Preco",                 width: 60, align: "center", sortable: false }
//                      , { label: "Preço Stop",  name: "PrecoStartStop",         jsonmap: "PrecoStartStop",      index: "PrecoStartStop",        width: 60, align: "center", sortable: false }
//                      , { label: "Qtd. Exec.",  name: "QuantidadeExecutada",    jsonmap: "QuantidadeExecutada", index: "QuantidadeExecutada",   width: 65, align: "center", sortable: false }
//                      , { label: "Tipo",        name: "Tipo",                   jsonmap: "Tipo",                index: "Tipo",                  width: 95, align: "left", sortable: false }
//                      , { label: "Validade",    name: "Validade",               jsonmap: "Validade",            index: "Validade",              width: 89, align: "center", sortable: false }
//                      , { label: "Porta",       name: "Porta",                  jsonmap: "Porta",               index: "Porta",                 width: 40, align: "center", sortable: false }
//                    ]
//      , width: 950
//      , height: 960
//      , pager: "#pnlBusca_OrdensNovoOMS_Resultados_Pager"
//      , rowNum: 40
//      , hoverrows: false
//      , sortname: "invid"
//      , sortorder: "desc"
//      , viewrecords: true
//      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
//      , subGrid: true
//      , jsonReader: {
//          root: "Itens"
//                       , page: "PaginaAtual"
//                       , total: "TotalDePaginas"
//                       , records: "TotalDeItens"
//                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
//                       , id: "0"              //primeira propriedade do elemento de linha é o Id
//                       , repeatitems: false

//      }
//      , afterInsertRow: GradIntra_Busca_OrdensNovoOMSItemDataBound
//      , beforeSelectRow: GradIntra_Busca_OrdensNovoOMSSelectRow
//      , subGridRowExpanded: GradIntra_Busca_OrdensNovoOMSSubgrid
//    });

//    //Chama o evento de page resize pra já ajustar:
//    PageResize();
//}


function GradIntra_Busca_ConfigurarGridDeResultados_OrdensSpider() 
{
    $("#tblBusca_OrdensSpider_Resultados").jqGrid(
    {
        url: "Monitoramento/ResultadoOrdensSpider.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "POST"
      , colModel: [
                        { label: "Cancel",      name: "Cancel",                 jsonmap: "Check",               index: "Check",                 width: 42, align: "center", sortable: false }
                      , { label: "Id",          name: "Id",                     jsonmap: "Id",                  index: "Id",                    width: 173, align: "center", sortable: false }
                      , { label: "Cliente",     name: "CodigoCliente",          jsonmap: "CodigoCliente",       index: "CodigoCliente",         width: 74, align: "center", sortable: false }
                      , { label: "Hora",        name: "Hora",                   jsonmap: "Hora",                index: "Hora",                  width: 54, align: "center", sortable: false }
                      , { label: "Status",      name: "Status",                 jsonmap: "Status",              index: "Status",                width: 85, align: "center", sortable: false }
                      , { label: "C/V",         name: "CompraVenda",            jsonmap: "CompraVenda",         index: "CompraVenda",           width: 46, align: "center", sortable: false }
                      , { label: "Papel",       name: "Papel",                  jsonmap: "Papel",               index: "Papel",                 width: 58, align: "center", sortable: false }
                      , { label: "Quantidade",  name: "Quantidade",             jsonmap: "Quantidade",          index: "Quantidade",            width: 95, align: "center", sortable: false }
                      , { label: "Preço",       name: "Preco",                  jsonmap: "Preco",               index: "Preco",                 width: 60, align: "center", sortable: false }
                      , { label: "Preço Stop",  name: "PrecoStartStop",         jsonmap: "PrecoStartStop",      index: "PrecoStartStop",        width: 60, align: "center", sortable: false }
                      , { label: "Qtd. Exec.",  name: "QuantidadeExecutada",    jsonmap: "QuantidadeExecutada", index: "QuantidadeExecutada",   width: 65, align: "center", sortable: false }
                      , { label: "Tipo",        name: "Tipo",                   jsonmap: "Tipo",                index: "Tipo",                  width: 95, align: "left", sortable: false }
                      , { label: "Validade",    name: "Validade",               jsonmap: "Validade",            index: "Validade",              width: 89, align: "center", sortable: false }
                      , { label: "Porta",       name: "Porta",                  jsonmap: "Porta",               index: "Porta",                 width: 40, align: "center", sortable: false }
                    ]
      , width: 950
      , height: 480
      , pager: "#pnlBusca_OrdensSpider_Resultados_Pager"
      , rowNum: 40
      , hoverrows: false
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false

      }
      , afterInsertRow: GradIntra_Busca_OrdensSpiderItemDataBound
      , beforeSelectRow: GradIntra_Busca_OrdensSpiderSelectRow
      , subGridRowExpanded: GradIntra_Busca_OrdensSpiderSubgrid
    });

    //Chama o evento de page resize pra já ajustar:
    PageResize();
}


function GradIntra_Busca_ConfigurarGridDeResultados_OrdensNovoOMS() 
{
    $("#tblBusca_OrdensNovoOMS_Resultados").jqGrid(
    {
        url: "Monitoramento/ResultadoOrdensNovoOMS.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "POST"
      , colModel: [
                        { label: "Cancel"       , name: "Cancel"                , jsonmap: "Check"                  , index: "Check"                , width: 42, align: "center"    , sortable: false }
                      , { label: "Id"           , name: "Id"                    , jsonmap: "Id"                     , index: "Id"                   , width: 173, align: "center"   , sortable: false }
                      , { label: "Cliente"      , name: "CodigoCliente"         , jsonmap: "CodigoCliente"          , index: "CodigoCliente"        , width: 74, align: "center"    , sortable: false }
                      , { label: "Hora"         , name: "Hora"                  , jsonmap: "Hora"                   , index: "Hora"                 , width: 54, align: "center"    , sortable: false }
                      , { label: "Status"       , name: "Status"                , jsonmap: "Status"                 , index: "Status"               , width: 85, align: "center"    , sortable: false }
                      , { label: "C/V"          , name: "CompraVenda"           , jsonmap: "CompraVenda"            , index: "CompraVenda"          , width: 46, align: "center"    , sortable: false }
                      , { label: "Papel"        , name: "Papel"                 , jsonmap: "Papel"                  , index: "Papel"                , width: 58, align: "center"    , sortable: false }
                      , { label: "Quantidade"   , name: "Quantidade"            , jsonmap: "Quantidade"             , index: "Quantidade"           , width: 95, align: "center"    , sortable: false }
                      , { label: "Preço"        , name: "Preco"                 , jsonmap: "Preco"                  , index: "Preco"                , width: 60, align: "center"    , sortable: false }
                      , { label: "Preço Stop"   , name: "PrecoStartStop"        , jsonmap: "PrecoStartStop"         , index: "PrecoStartStop"       , width: 60, align: "center"    , sortable: false }
                      , { label: "Qtd. Exec."   , name: "QuantidadeExecutada"   , jsonmap: "QuantidadeExecutada"    , index: "QuantidadeExecutada"  , width: 65, align: "center"    , sortable: false }
                      , { label: "Tipo"         , name: "Tipo"                  , jsonmap: "Tipo"                   , index: "Tipo"                 , width: 95, align: "left"      , sortable: false }
                      , { label: "Validade"     , name: "Validade"              , jsonmap: "Validade"               , index: "Validade"             , width: 89, align: "center"    , sortable: false }
                      , { label: "Porta"        , name: "Porta"                 , jsonmap: "Porta"                  , index: "Porta"                , width: 40, align: "center"    , sortable: false }
                    ]
      , width: 950
      , height: 480
      , pager: "#pnlBusca_OrdensNovoOMS_Resultados_Pager"
      , rowNum: 40
      , hoverrows: false
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: {
          root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false

      }
      , afterInsertRow:     GradIntra_Busca_OrdensNovoOMSItemDataBound
      , beforeSelectRow:    GradIntra_Busca_OrdensNovoOMSSelectRow
      , subGridRowExpanded: GradIntra_Busca_OrdensNovoOMSSubgrid
    });

    //Chama o evento de page resize pra já ajustar:
    PageResize();
}

function GradIntra_Busca_ConfigurarGridDeResultados_OrdensSinacor() 
{
        
    $("#tblBusca_Ordens_Resultados").jqGrid(
    {
        url: "Monitoramento/ResultadoOrdens.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "POST"
      , colModel: [
                        { label: "Cancel"           , name: "Cancel"                , jsonmap: "Check"              , index: "Check"                , width: 42  , align: "center", sortable: false }  
                      , { label: "Id"               , name: "Id"                    , jsonmap: "Id"                 , index: "Id"                   , width: 173 , align: "center", sortable: false }
                      , { label: "Cliente"          , name: "CodigoCliente"         , jsonmap: "CodigoCliente"      , index: "CodigoCliente"        , width: 74  , align: "center", sortable: false }
                      , { label: "Hora"             , name: "Hora"                  , jsonmap: "Hora"               , index: "Hora"                 , width: 54  , align: "center", sortable: false }
                      , { label: "Status"           , name: "Status"                , jsonmap: "Status"             , index: "Status"               , width: 85  , align: "center", sortable: false }
                      , { label: "C/V"              , name: "CompraVenda"           , jsonmap: "CompraVenda"        , index: "CompraVenda"          , width: 46  , align: "center", sortable: false }
                      , { label: "Papel"            , name: "Papel"                 , jsonmap: "Papel"              , index: "Papel"                , width: 58  , align: "center", sortable: false }
                      , { label: "Quantidade"       , name: "Quantidade"            , jsonmap: "Quantidade"         , index: "Quantidade"           , width: 95  , align: "center", sortable: false }
                      , { label: "Preço"            , name: "Preco"                 , jsonmap: "Preco"              , index: "Preco"                , width: 60  , align: "center", sortable: false }
                      , { label: "Qtd. Exec."       , name: "QuantidadeExecutada"   , jsonmap: "QuantidadeExecutada", index: "QuantidadeExecutada"  , width: 65  , align: "center", sortable: false }
                      , { label: "Tipo"             , name: "Tipo"                  , jsonmap: "Tipo"               , index: "Tipo"                 , width: 95  , align: "left"  , sortable: false }
                      , { label: "Validade"         , name: "Validade"              , jsonmap: "Validade"           , index: "Validade"             , width: 89  , align: "center", sortable: false }
                      , { label: "Porta"            , name: "Porta"                 , jsonmap: "Porta"              , index: "Porta"                , width: 1   , align: "left"  , sortable: false, visible:false }
                    ]
      , height: 470
      , pager: "#pnlBusca_Ordens_Resultados_Pager"
      , rowNum: 20
      , hoverrows: false
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , subGrid: true
      , jsonReader: {
                         root: "Itens"
                       , page: "PaginaAtual"
                       , total: "TotalDePaginas"
                       , records: "TotalDeItens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
                       
      }
      , afterInsertRow:     GradIntra_Busca_OrdensItemDataBound
      , beforeSelectRow:    GradIntra_Busca_OrdensSelectRow
      , subGridRowExpanded: GradIntra_Busca_OrdensSubgrid
    });

    //Chama o evento de page resize pra já ajustar:
    PageResize();
}

function GradIntra_Busca_ConfigurarGridDeResultados_GerenciamentoIPO() {
    var lastsel2;
    $("#tblBusca_Solicitacoes_IPO_Resultados").jqGrid(
    {
        url: "Buscas/Solicitacoes_IPO.aspx?Acao=Paginar"
        , datatype: "json"
        , mtype: "GET"
        , colNames: ["Codigo IPO Cliente","Cliente","Codigo ISIN","Data","Codigo Assessor","Nome Cliente","CpfCnpj","Empresa",
         "Tipo","Valor Reserva","Valor Maximo","Taxa Maxima","Limite","Status", "Numero Protocolo", "Observacoes"]
        , colModel: [
        { name: "CodigoIPOCliente",   jsonmap: "CodigoIPOCliente",width:65,    index: "CodigoIPOCliente",   align: "center", sortable:false, visible:false },
        { name: "CodigoCliente",      jsonmap: "CodigoCliente",   width:65,    index: "CodigoCliente",      align: "center", sortable:false },
        { name: "CodigoISIN",         jsonmap: "CodigoISIN",      width:85,    index: "CodigoISIN",         align: "center", sortable:false },
        { name: "Data",               jsonmap: "Data",            width:65,    index: "Data",               align: "center", sortable:false },
        { name: "CodigoAssessor",     jsonmap: "CodigoAssessor",  width:65,    index: "CodigoAssessor",     align: "right", sortable:false },
        { name: "NomeCliente",        jsonmap: "NomeCliente",     width:120,   index: "NomeCliente",        align: "left", sortable:false },
        { name: "CpfCnpj",            jsonmap: "CpfCnpj",         width:85,    index: "CpfCnpj",            align: "right", sortable:false },
        { name: "Empresa",            jsonmap: "Empresa",         width:85,    index: "Empresa",            align: "left", sortable:false },
        { name: "Modalidade",         jsonmap: "Modalidade",      width:65,    index: "Modalidade",         align: "left", sortable:false },
        { name: "ValorReserva",       jsonmap: "ValorReserva",    width:65,    index: "ValorReserva",       align: "right", sortable:false },
        { name: "ValorMaximo",        jsonmap: "ValorMaximo",     width:65,    index: "ValorMaximo",        align: "right", sortable:false },
        { name: "TaxaMaxima",         jsonmap: "TaxaMaxima",      width:65,    index: "TaxaMaxima",         align: "right", sortable:false },
        { name: "Limite",             jsonmap: "Limite",          width:65,    index: "Limite",             align: "right", sortable:false },
        { name: "Status",             jsonmap: "Status",          width:70,    index: "Status",                             sortable: false,
            editable: true, 
            edittype: "select",
            editoptions:
            {
                value: "1:Solicitada;2:Executada;3:Cancelada",
//                dataInit: function (elem) 
//                {
//                    setTimeout(function() {
//                        $(elem).combobox();
//                        $("#toggle").click(function (){});
//                    },50)
//                }
            } 
        },
        { name: "NumeroProtocolo",  jsonmap: "NumeroProtocolo", width: 105, index: "NumeroProtocolo" ,  align: "center", sortable:false },
        { name: "Observacoes",      jsonmap: "Observacoes",     width: 105, index: "Observacoes" ,      align: "center", sortable:false,
            editable: true,
            edittype: "textarea" 
            }
        ]
      , width: 1190
      , height: 90
      , pager: "#pnlBusca_Solicitacoes_IPO_Resultados_Pager"
      , rowNum: 20
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , hoverrows: false
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      //, shrinkToFit: false
      , subGrid: false
      , editable: true
      //, autowidth: true
      , jsonReader: { root: "Itens"
                    , page: "PaginaAtual"
                    , total: "TotalDePaginas"
                    , records: "TotalDeItens"
                    , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id: "0"              //primeira propriedade do elemento de linha é o Id
                    , repeatitems: false
      }
      , loadComplete: function () {
          //$("#tblBusca_Solicitacoes_IPO_Resultados").setColProp('Status', { editoptions: { value: countries} });
          lastsel2 = "";
      }
      ,onSelectRow: function(id)
      {
//		if(id && id!==lastsel2)
//        {
			jQuery('#tblBusca_Solicitacoes_IPO_Resultados').jqGrid('restoreRow',lastsel2);
			jQuery('#tblBusca_Solicitacoes_IPO_Resultados').jqGrid('editRow',id,true);
			lastsel2=id;
		//}
      }
    });

    
}

function GradIntra_Busca_ConfigurarGridDeResultados_VendasDeFerramentas()
{
    $("#tblBusca_Solicitacoes_VendasDeFerramentas_Resultados").jqGrid(
    {
        url: "Buscas/Solicitacoes_VendasDeFerramentas.aspx?Acao=Paginar"
      , datatype: "json"
      , mtype: "GET"
      , colModel: [
                        { label: "Código",                name: "Id",                   jsonmap: "Id",                   index: "Id",                   width: 70,  align: "right",  sortable: false }
                      , { label: "Cód Ref.",              name: "CodigoDeReferencia",   jsonmap: "CodigoDeReferencia",   index: "CodigoDeReferencia",   width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "CBLC",                  name: "CBLC",                 jsonmap: "CBLC",                 index: "CBLC",                 width: 70,  align: "right",  sortable: false }
                      , { label: "CPF / CNPJ",            name: "CpfCnpj",              jsonmap: "CpfCnpj",              index: "CpfCnpj",              width:100,  align: "left",   sortable: false }
                      , { label: "Status",                name: "Status",               jsonmap: "Status",               index: "Status",               width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "Status",                name: "DescricaoStatus",      jsonmap: "DescricaoStatus",      index: "Status",               width: 80,  align: "left",   sortable: false }
                      , { label: "Data",                  name: "Data",                 jsonmap: "Data",                 index: "Data",                 width:110,  align: "left",   sortable: false }
                      , { label: "Quantidade",            name: "Quantidade",           jsonmap: "Quantidade",           index: "Quantidade",           width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "Preco",                 name: "Preco",                jsonmap: "Preco",                index: "Preco",                width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "IdProduto",             name: "IdProduto",            jsonmap: "IdProduto",            index: "IdProduto",            width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "DescProduto",           name: "DescProduto",          jsonmap: "DescProduto",          index: "DescProduto",          width:120,  align: "left",   sortable: false, hidden: true }
                      , { label: "Produto",               name: "Alias",                jsonmap: "Alias",                index: "Alias",                width:120,  align: "left",   sortable: false }
                      , { label: "IdPagamento",           name: "IdPagamento",          jsonmap: "IdPagamento",          index: "IdPagamento",          width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "Tipo",                  name: "Tipo",                 jsonmap: "Tipo",                 index: "Tipo",                 width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "MetodoTipo",            name: "MetodoTipo",           jsonmap: "MetodoTipo",           index: "MetodoTipo",           width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "MetodoCodigo",          name: "MetodoCodigo",         jsonmap: "MetodoCodigo",         index: "MetodoCodigo",         width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "Método de Pgto",        name: "MetodoDesc",           jsonmap: "MetodoDesc",           index: "MetodoDesc",           width:120,  align: "left",   sortable: false }
                      , { label: "ValorBruto",            name: "ValorBruto",           jsonmap: "ValorBruto",           index: "ValorBruto",           width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "ValorDesconto",         name: "ValorDesconto",        jsonmap: "ValorDesconto",        index: "ValorDesconto",        width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "ValorTaxas",            name: "ValorTaxas",           jsonmap: "ValorTaxas",           index: "ValorTaxas",           width: 50,  align: "left",   sortable: false, hidden: true }
                      , { label: "Valor Total",           name: "ValorLiquido",         jsonmap: "ValorLiquido",         index: "ValorLiquido",         width: 70,  align: "left",   sortable: false }
                      , { label: "QuantidadeDeParcelas",  name: "QuantidadeDeParcelas", jsonmap: "QuantidadeDeParcelas", index: "QuantidadeDeParcelas", width: 50,  align: "left",   sortable: false, hidden: true }
                    ]
      , height: 96
      , pager: "#pnlBusca_Solicitacoes_VendasDeFerramentas_Resultados_Pager"
      , rowNum: 20
      , sortname: "invid"
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader: { root: "Itens"
                    , page: "PaginaAtual"
                    , total: "TotalDePaginas"
                    , records: "TotalDeItens"
                    , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                    , id: "0"              //primeira propriedade do elemento de linha é o Id
                    , repeatitems: false
      }
      , ondblClickRow: GradIntra_Busca_SelecionarItensIndividualmente
  });

    
  //PageResize();
}



function GradIntra_Busca_OrdensStopSubgrid(subgrid_id, row_id) 
{
    var subgrid_table_id;

       subgrid_table_id = subgrid_id+"_t";

       var NumeroSeqOrdem = $("#" + row_id).find("td").eq(2).html();

       jQuery("#"+subgrid_id).html("<table id='"+subgrid_table_id+"' class='scroll'></table>");

       jQuery("#"+subgrid_table_id).jqGrid({
           url: "Monitoramento/ResultadoOrdensStop.aspx?Acao=CarregarDetalhes&id=" + NumeroSeqOrdem,
          hoverrows: false,
          datatype: "json",
          mtype: "POST",
          colNames: ['Data Situação', 'Situação'],
          colModel: [
            {label:"Data Situação", name:"DataSituacao"  ,index:"DataSituacao"  ,width:120,  align:"center"},
            {label:"Situação",      name:"Situacao"      ,index:"Situacao"      ,width:550,  align:"left"},
          ],
          jsonReader: {
                         root: "Itens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
                       
          },
          height: "100%"  ,
          rowNum:20       ,
          sortname: 'num' ,
          sortorder: 'asc'
          });
  }

  function GradIntra_Busca_OrdensSpiderSubgrid(subgrid_id, row_id)
   {
      var subgrid_table_id;

      subgrid_table_id = subgrid_id + "_t";

      jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

      var NumeroSeqOrdem = encodeURIComponent( $("#tblBusca_OrdensSpider_Resultados tr").closest("#" + row_id).find("td").eq(2).html());

      jQuery("#" + subgrid_table_id).jqGrid({
          url: "Monitoramento/ResultadoOrdensSpider.aspx?Acao=CarregarDetalhes&id=" + NumeroSeqOrdem,
          datatype: "json",
          hoverrows: false,
          mtype: "POST",
          colNames: ['Qtd Solicitada', 'Qtd Remanescente', 'Qtd Executada', 'Preco', 'Situação', 'Data', 'Descrição'],
          colModel: [
            { label: "Qtd Solicitada",   name: "QuantidadeSolicitada",   index: "QuantidadeSolicitada",   width: 80, align: "center" },
            { label: "Qtd Remanescente", name: "QuantidadeRemanescente", index: "QuantidadeRemanescente", width: 80, align: "center" },
            { label: "Qtd Executada",    name: "QuantidadeExecutada",    index: "QuantidadeExecutada",    width: 80, align: "center" },
            { label: "Preço",            name: "Preco",                  index: "Preco",                  width: 80, align: "center" },
            { label: "Situação",         name: "Situacao",               index: "Situacao",               width: 150, align: "left" },
            { label: "Data",             name: "Data",                   index: "Data",                   width: 120, align: "center" },
            { label: "Descrição",        name: "Descricao",              index: "Descricao",              width: 650, align: "left" },
          ],
          jsonReader: {
              root: "Itens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false

          },
          height: "100%",
          rowNum: 20,
          sortname: 'num',
          sortorder: 'asc'
      });
  }

function GradIntra_Busca_OrdensNovoOMSSubgrid(subgrid_id, row_id) 
{
    var subgrid_table_id;

    subgrid_table_id = subgrid_id + "_t";

    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

    var NumeroSeqOrdem = $("#tblBusca_OrdensNovoOMS_Resultados tr").closest("#" + row_id).find("td").eq(2).html();
    //  $("#tblBusca_OrdensNovoOMS_Resultados tr#" + row_id).find("td").eq(2).html(); // $("#tblBusca_OrdensNovoOMS_Resultados tr:eq(" + row_id + ") td:eq(2)").html();

    jQuery("#" + subgrid_table_id).jqGrid({
        url: "Monitoramento/ResultadoOrdensNovoOMS.aspx?Acao=CarregarDetalhes&id=" + NumeroSeqOrdem,
        datatype: "json",
        hoverrows: false,
        mtype: "POST",
        colNames: ['Qtd Solicitada', 'Qtd Remanescente', 'Qtd Executada','Preco', 'Situação', 'Data', 'Descrição'],
        colModel: [
            { label: "Qtd Solicitada"   , name: "QuantidadeSolicitada"      , index: "QuantidadeSolicitada"     , width: 80, align: "center" },
            { label: "Qtd Remanescente" , name: "QuantidadeRemanescente"    , index: "QuantidadeRemanescente"   , width: 80, align: "center" },
            { label: "Qtd Executada"    , name: "QuantidadeExecutada"       , index: "QuantidadeExecutada"      , width: 80, align: "center" },
            { label: "Preço"            , name: "Preco"                     , index: "Preco"                    , width: 80, align: "center" },
            { label: "Situação"         , name: "Situacao"                  , index: "Situacao"                 , width: 150, align: "left" },
            { label: "Data"             , name: "Data"                      , index: "Data"                     , width: 120, align: "center" },
            { label: "Descrição"        , name: "Descricao"                 , index: "Descricao"                , width: 650, align: "left" },
          ],
        jsonReader: {
            root: "Itens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false

        },
        height: "100%",
        rowNum: 40,
        sortname: 'num',
        sortorder: 'asc'
    });
}

function GradIntra_Busca_OrdensSubgrid(subgrid_id, row_id) 
{
       var subgrid_table_id;

       subgrid_table_id = subgrid_id+"_t";

       jQuery("#"+subgrid_id).html("<table id='"+subgrid_table_id+"' class='scroll'></table>");

       var NumeroSeqOrdem = $("#" + row_id).find("td").eq(2).html();

       jQuery("#"+subgrid_table_id).jqGrid({
           url: "Monitoramento/ResultadoOrdens.aspx?Acao=CarregarDetalhes&id=" + NumeroSeqOrdem,
          datatype: "json",
          hoverrows: false,
          mtype: "POST",
          colNames: ['Qtd Solicitada', 'Qtd Remanescente', 'Qtd Executada', 'Situação','Data','Descrição'],
          colModel: [
            {label:"Qtd Solicitada",    name:"QuantidadeSolicitada"  ,index:"QuantidadeSolicitada"   ,width:80,  align:"center"},
            {label:"Qtd Remanescente",  name:"QuantidadeRemanescente",index:"QuantidadeRemanescente" ,width:80,  align:"center"},
            {label:"Qtd Executada",     name:"QuantidadeExecutada"   ,index:"QuantidadeExecutada"    ,width:80,  align:"center"},
            {label:"Situação",          name:"Situacao"              ,index:"Situacao"               ,width:150,  align:"left"  },
            {label:"Data",              name:"Data"                  ,index:"Data"                   ,width:120, align:"center"},
            {label:"Descrição",         name:"Descricao"             ,index:"Descricao"              ,width:650, align:"left"  },
          ],
          jsonReader: {
                         root: "Itens"
                       , cell: ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                       , id: "0"              //primeira propriedade do elemento de linha é o Id
                       , repeatitems: false
                       
          },
          height: "100%"  ,
          rowNum:20       ,
          sortname: 'num' ,
          sortorder: 'asc'
          });
    }

function GradIntra_Busca_OrdensSpiderSelectRow(rowid, e) 
{
        return false;
}

function GradIntra_Busca_OrdensNovoOMSSelectRow(rowid, e) 
{
    return false;
}
function GradIntra_Busca_OrdensSelectRow(rowid, e)
{
    return false;
}

function GradIntra_Busca_AplicacaoSelectRow(rowid, e)
{
    return false;
}

function GradIntra_Busca_OrdensSpiderItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_OrdensSpider_Resultados tr[id=" + rowid + "]");

    lRow.addClass(rowelem.Status.toUpperCase());

    //lRow.children("td").eq(0).RemoveClass(rowelem.Status.toUpperCase());

    lRow.children("td").eq(1)
        .html("<input type='checkbox' id='checkCancel_" + rowelem.Id + "' rel='" + rowelem.Id + "' >" +
              "<label class='CheckLabel' for='checkCancel_" + rowelem.Id + "'>&nbsp;</label>")
              .find("input").customInput();
    //.customInput();

    //lRow.find("td:eq(0) input").customInput();
}

function GradIntra_Busca_OrdensNovoOMSItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_OrdensNovoOMS_Resultados tr[id=" + rowid + "]");

    lRow.addClass(rowelem.Status.toUpperCase());

    //lRow.children("td").eq(0).RemoveClass(rowelem.Status.toUpperCase());

    lRow.children("td").eq(1)
        .html("<input type='checkbox' id='checkCancel_" + rowelem.Id + "' rel='" + rowelem.Id + "' >" +
              "<label class='CheckLabel' for='checkCancel_" + rowelem.Id + "'>&nbsp;</label>")
              .find("input").customInput();
    //.customInput();

    //lRow.find("td:eq(0) input").customInput();
}

function GradIntra_Busca_OrdensItemDataBound(rowid, rowdata, rowelem)
{
    var lRow =  $("#tblBusca_Ordens_Resultados tr[id="+ rowid+"]");

    lRow.addClass(rowelem.Status.toUpperCase());
    
    //lRow.children("td").eq(0).RemoveClass(rowelem.Status.toUpperCase());

    lRow.children("td").eq(1)
        .html("<input type='checkbox' id='checkCancel_"+rowelem.Id+"' rel='"+rowelem.Id+"' >" + 
              "<label class='CheckLabel' for='checkCancel_"+rowelem.Id+"'>&nbsp;</label>")
              .find("input").customInput();
    //.customInput();

    //lRow.find("td:eq(0) input").customInput();
}

function GradIntra_Busca_ReservaIPOItemDataBound(rowid, rowdata, rowelem) 
{
    var lRow = $("#tblBusca_ReservaIPO_Resultados tr[id=" + rowid + "]");
    
    lRow.addClass(rowelem.Status.toUpperCase());

    lRow.children("td").eq(8).html("<button class=\"btnBusca\" id=\"btnReservarIPO_" + rowelem.Id + rowdata.CodBMF + rowdata.CodBovespa
    + "\" onclick=\"return GradIntra_Busca_AbrirJanela_ReservaIPO(" + rowdata.CodBovespa + ",'" +rowdata.NomeCliente+"','"+rowdata.CPF+"');\" >Reservar</button>")
                                   .find("button").customInput();

    //ReservarIPO
}

function GradIntra_Busca_OrdensStopItemDataBound(rowid, rowdata, rowelem)
{
    var lRow = $("#tblBusca_OrdensStop_Resultados tr[id="+ rowid+"]");

    lRow.addClass(rowelem.Status.toUpperCase());
    
    //lRow.children("td").eq(0).RemoveClass(rowelem.Status.toUpperCase());

    lRow.children("td").eq(1).html("<input type='checkbox' id='checkCancel_" + rowelem.Id + "' rel='" + rowelem.Id + "' >" + 
                                   "<label class='CheckLabel' for='checkCancel_"+rowelem.Id+"'>&nbsp;</label>")
                                   .find("input").customInput();
}


function GradIntra_Busca_AplicacaoItemDataBound(rowid, rowdata, rowelem)
{
    var lRow = $("#tb_busca_Aplicacao tr[Id="+ rowid+"]");

     if(rowdata.CodigoStatus == "AGUADANDO_APROVACAO")
    {
        lRow.css("background", "#CD5C5C");

        lRow.children("td").eq(0).html("<input type='checkbox' class='checkAplicacaoAprovar' id='check' >"  + 
                                   "<input type='hidden' id='hiddenAprovacaoAplicacao' value='" +rowelem.CodigoTabela+"' />")
                                   .find("input").customInput();
    }
    else
    {
        lRow.css("background", "#8FBC8F");
    }
}

function GradIntra_Busca_ResgateItemDataBound(rowid, rowdata, rowelem)
{
    var lRow = $("#tb_busca_Resgate tr[Id="+ rowid+"]");

    

    if(rowdata.CodigoStatus == "AGUADANDO_APROVACAO")
    {
        lRow.css("background", "#CD5C5C");

        lRow.children("td").eq(0).html("<input type='checkbox' class='checkResgateAprovar' id='check' >"  + 
                                   "<input type='hidden' id='hiddenAprovacaoResgate' value='" +rowelem.CodigoTabela+"' />")
                                   .find("input").customInput();

    }
    else
    {
        lRow.css("background", "#8FBC8F");
    }
}

function GradIntra_Busca_ConfigurarGridDeResultados_MigracaoAssessor()
{
    
}

function GradIntra_Busca_ConfigurarGridDeResultados_Risco()
{
    $("#tblBusca_Risco_Resultados").jqGrid(
    {
        url:        "Buscas/Risco.aspx?Acao=Paginar"
      , datatype:   "json"
      , mtype:      "GET"
      , colModel :  [ 
                        //{"Id", "Descricao", "Regras"}

                        { label:"Descrição.",      name:"Descricao",     jsonmap:"Descricao",     index:"Descricao",     width:380,  align:"left",   sortable:false  } 
                      , { label:"Código",          name:"Id",            jsonmap:"Id",            index:"Id",            width:100,  align:"right",  sortable:false  } 
                      , { label:"Tipo De Objeto",  name:"TipoDeObjeto",  jsonmap:"TipoDeObjeto",  index: "TipoDeObjeto", width: 0  , align:"left",   sortable:false, hidden: true }
                      , { label:"Regras",          name:"Regras",        jsonmap:"Regras",        index:"Regras",        width: 0,   align:"left",   sortable:false, hidden:true  }
                    ]
      , shrinkToFit: false
      , height:      96
      , pager:       "#pnlBusca_Risco_Resultados_Pager"
      , rowNum:      20
      , sortname:    "invid"
      , sortorder:   "desc"
      , viewrecords:  true
      , gridview:     true   //flag importante para velocidade, mas exclui evento afterRowInsert; Ver documentação em http://www.trirand.com/jqgridwiki/doku.php?id=wiki:options
      , jsonReader :  {
                          root:        "Itens"
                        , page:        "PaginaAtual"
                        , total:       "TotalDePaginas"
                        , records:     "TotalDeItens"
                        , cell:        ""               //vazio obrigatoriamente para pegar as propriedades do objeto serializado
                        , id:          "0"              //primeira propriedade do elemento de linha é o Id
                        , repeatitems: false
                      }
      , ondblClickRow: GradIntra_Busca_SelecionarItensIndividualmente
    }); 

    //Chama o evento de page resize pra já ajustar:
    PageResize();
}


function GradIntra_Busca_BuscarAplicacao(pObjetoDeParametros,pDivDeFormulario, pDivFormResultados, pURL, pObjetoDeParametros)
{
    
    var lComboBusca_Selecionado    = $("#cmbBusca_Status").val();
    var ltxtDataInicial            = $("#txtAplicacaoResgate_DataInicial").val();
    var ltxtDataFinal              = $("#txtAplicacaoResgate_DataFinal").val();
    var ltxtCodigoCliente          = $("#txtAplicacaoResgate_CodigoCliente").val();
    var lComboStatus               = $("#cmbBusca_Status").val();

    var lURL = "Solicitacoes/PoupeDirect/PoupeAplicacaoResgate.aspx";
    var lObjetoDeParametros = { Acao            : "SelecionarAprovacoes"
                              , IdStatus        : lComboBusca_Selecionado
                              , DataInicial     : ltxtDataInicial
                              , DataFinal       : ltxtDataFinal
                              , CodigoCliente   : ltxtCodigoCliente
                              , status          : lComboStatus
                              };
    
    var pDivDeFormulario = $("#pnlConteudo_Solicitacoes_PoupeDirect_Dados");

    GradIntra_CarregarJsonVerificandoErro( lURL
                                         , lObjetoDeParametros
                                         , function(pResposta) { GradIntra_Busca_BuscarAplicacao_Callback(pResposta) ;}
                                         , null
                                         , pDivDeFormulario
                                         );
}


function GradIntra_Busca_BuscarAplicacao_Callback(pResposta)
{

var lPainelDeResultado = "";
var lPainelDeResultadoResgte = "";

var pDivDeFormulario = $("#pnlConteudo_Solicitacoes_PoupeDirect_Dados");


    GradIntra_Busca_Aplicacao(); //GradIntra_Busca_Aplicacao
         lPainelDeResultado   = $("#pnlConteudo_Aplicacao")
                                    .show()
                                    .addClass(CONST_CLASS_CONTEUDOCARREGADO)
                                    .children()
                                    .show()
                                    .find("#pnlConteudo_Aplicacao_listaResultado")


    GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);
   
    var lGrid = lPainelDeResultado.find("table.ui-jqgrid-btable")[0];
    
    lPainelDeResultado.show();

    lPainelDeResultado
        .parent()
        .find(".Aguarde")
        .hide();



    lGrid.addJSONData(pResposta.ObjetoDeRetorno);

     


    GradIntra_Busca_Resgate(); //GradIntra_Busca_Resgate
    lPainelDeResultadoResgte   = $("#pnlConteudo_Resgate")
                                    .show()
                                    .addClass(CONST_CLASS_CONTEUDOCARREGADO)
                                    .children()
                                    .show()
                                    .find("#pnlConteudo_Resgate_listaResultado")

      
    var lGridResgate = lPainelDeResultadoResgte.find("table.ui-jqgrid-btable")[0];
    
    lPainelDeResultadoResgte.show();

    lPainelDeResultadoResgte
        .parent()
        .find(".Aguarde")
        .hide();

    lGridResgate.addJSONData(pResposta.ObjetoDeRetorno);

}


function GradIntra_Busca_BuscarItensParaSelecaoOrdens (pObjetoDeParametros,pDivDeFormulario, pDivFormResultados, pURL)
{
    if(!pObjetoDeParametros || pObjetoDeParametros == null)
        pObjetoDeParametros = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);
    
    pObjetoDeParametros.Acao = "BuscarItensParaSelecao";

    pDivDeFormulario
        .find(".btnBusca")
            .html("Aguarde...")
        .parent()
        .find("input, select, button")
            .prop("disabled", true);

//    var lDivId = pDivFormResultados
//                    .show()
//                    .parent()
//                        .attr("id");

    GradIntra_CarregarJsonVerificandoErro(  pURL
                                          , pObjetoDeParametros
                                          , function(pResposta) { GradIntra_Busca_BuscarItensParaSelecaoOrdens_Callback(pResposta, pDivDeFormulario ); }
                                         );

}

function GradIntra_Busca_BuscarItensParaSelecaoOrdens_Callback(pResposta, pDivDeFormulario)
{
///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_Busca_BuscarItensParaSelecao: faz o "databind" da tabela de resultados.</summary>
///<param name="pResposta" type="Objeto_JSON">Objeto "ChamadaAjaxResponse" que retornou do code-behind.</param>
///<returns>void</returns>
    
    var lPainelDeResultado = "";

    if (pDivDeFormulario.attr("id") == "pnlBusca_Monitoramento_OrdensStop_Form") {
        GradIntra_Busca_ConfigurarGridDeResultados_OrdensStop();
        lPainelDeResultado = $("#pnlConteudo_Monitoramento_OrdensStop")
                                    .show()
                                    .addClass(CONST_CLASS_CONTEUDOCARREGADO)
                                    .children()
                                    .show()
                                    .find("#pnlMonitoramento_ListaDeResultados")
    }
    else if (pDivDeFormulario.attr("id") == "pnlBusca_Monitoramento_OrdensNovoOMS_Form") 
    {
        GradIntra_Busca_ConfigurarGridDeResultados_OrdensNovoOMS();
        lPainelDeResultado = $("#pnlConteudo_Monitoramento_OrdensNovoOMS")
                                    .show()
                                    .addClass(CONST_CLASS_CONTEUDOCARREGADO)
                                    .children()
                                    .show()
                                .find("#pnlMonitoramento_OrdenNovoOMS_ListaDeResultados")
    }
    else if (pDivDeFormulario.attr("id") == "pnlBusca_Monitoramento_OrdensSpider_Form") 
    {
        GradIntra_Busca_ConfigurarGridDeResultados_OrdensSpider();
        lPainelDeResultado = $("#pnlConteudo_Monitoramento_OrdensSpider")
                                    .show()
                                    .addClass(CONST_CLASS_CONTEUDOCARREGADO)
                                    .children()
                                    .show()
                                .find("#pnlMonitoramento_OrdenSpider_ListaDeResultados")
    
    
    }
    else {
        GradIntra_Busca_ConfigurarGridDeResultados_OrdensSinacor();
        lPainelDeResultado = $("#pnlConteudo_Monitoramento_Ordens")
                                    .show()
                                    .addClass(CONST_CLASS_CONTEUDOCARREGADO)
                                    .children()
                                    .show()
                                .find("#pnlMonitoramento_ListaDeResultados")
    }

    GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);

    lPainelDeResultado.show();

    var lGrid = lPainelDeResultado.find("table.ui-jqgrid-btable")[0];
    
    lPainelDeResultado
        .parent()
        .find(".Aguarde")
        .hide();

    lGrid.addJSONData(pResposta.ObjetoDeRetorno);


    pDivDeFormulario 
            .find(".btnBusca")
                .html("Buscar")
            .parent()
            .find("input, select, button")
                .prop("disabled", false);
}

function GradIntra_Busca_BuscarItensParaSelecao(pObjetoDeParametros, pDivDeFormulario, pURL)
{
    if(!pObjetoDeParametros || pObjetoDeParametros == null)
        pObjetoDeParametros = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);

    pObjetoDeParametros.Acao = "BuscarItensParaSelecao";

    pDivDeFormulario
        .find(".btnBusca")
            .html("Aguarde...")
        .parent()
        .find("input, select, button")
            .prop("disabled", true);

    var lDivId = pDivDeFormulario.closest(".Busca_Container").attr("id");

    GradIntra_CarregarJsonVerificandoErro(  pURL
                                          , pObjetoDeParametros
                                          , function(pResposta) { GradIntra_Busca_BuscarItensParaSelecao_Callback(pResposta, lDivId); }
                                         );
}

function GradIntra_Busca_BuscarItensParaSelecao_Callback(pResposta, pIdDoPainelDeBusca)
{
///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_Busca_BuscarItensParaSelecao: faz o "databind" da tabela de resultados.</summary>
///<param name="pResposta" type="Objeto_JSON">Objeto "ChamadaAjaxResponse" que retornou do code-behind.</param>
///<returns>void</returns>

    var lPainelDeBusca   = $("#" + pIdDoPainelDeBusca);
    var lIdPainelDeBusca = lPainelDeBusca.attr("id");

    GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);

    var lGrid = lPainelDeBusca.find(".Busca_Resultados table.ui-jqgrid-btable")[0];

    if (lGrid == undefined) {
        lPainelDeBusca.find(".Busca_Resultados_Abaixo table.ui-jqgrid-btable")[0];
    }

    lGrid.addJSONData(pResposta.ObjetoDeRetorno);

    lPainelDeBusca
        .find(".Busca_Formulario")
            .find(".btnBusca")
                .html("Buscar")
            .parent()
            .find("input, select, button")
                .prop("disabled", false);
}

function GradIntra_Busca_BuscarItensParaSelecaoGerenciamentoIPO(pObjetoDeParametros, pDivDeFormulario, pURL, IdDivBusca) {
    if (!pObjetoDeParametros || pObjetoDeParametros == null)
        pObjetoDeParametros = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);

    pObjetoDeParametros.Acao = "BuscarItensParaSelecao";

    pDivDeFormulario
        .find(".btnBusca")
            .html("Aguarde...")
        .parent()
        .find("input, select, button")
            .prop("disabled", true);

    GradIntra_CarregarJsonVerificandoErro(pURL
                                          , pObjetoDeParametros
                                          , function (pResposta) { GradIntra_Busca_BuscarItensParaSelecaoGerenciamentoIPO_Callback(pResposta, IdDivBusca); }
                                         );
}

function GradIntra_Busca_BuscarItensParaSelecaoGerenciamentoIPO_Callback(pResposta, pIdDoPainelDeBusca) {
    ///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_Busca_BuscarItensParaSelecao: faz o "databind" da tabela de resultados.</summary>
    ///<param name="pResposta" type="Objeto_JSON">Objeto "ChamadaAjaxResponse" que retornou do code-behind.</param>
    ///<returns>void</returns>

    var lPainelDeBusca = $("#" + pIdDoPainelDeBusca);
    var lIdPainelDeBusca = lPainelDeBusca.attr("id");

    GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);

    var lGrid = lPainelDeBusca.find(".Busca_Resultados table.ui-jqgrid-btable")[0];

    if (lGrid == undefined) {
        lGrid = lPainelDeBusca.find(".Busca_Resultados_Abaixo table.ui-jqgrid-btable")[0];
    }

    lGrid.addJSONData(pResposta.ObjetoDeRetorno);

    lPainelDeBusca
        .find(".Busca_Formulario")
            .find(".btnBuscaSolicitacoesReservaIPO")
                .html("Buscar")
            .parent()
            .find(".btnSolicitacoesAtualizaLimites")
                .html("Atualizar Limites")
            .parent()
            .find("input, select, button")
                .prop("disabled", false);
}


function GradIntra_Busca_BuscarItensParaSelecaoReservaIPO(pObjetoDeParametros, pDivDeFormulario, pURL)
{
    if (!pObjetoDeParametros || pObjetoDeParametros == null)
        pObjetoDeParametros = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);

    pObjetoDeParametros.Acao = "BuscarItensParaSelecao";

    pDivDeFormulario
        .find(".btnBusca")
            .html("Aguarde...")
        .parent()
        .find("input, select, button")
            .prop("disabled", true);

    var lDivId = pDivDeFormulario.closest(".Busca_Container").attr("id");

    GradIntra_CarregarJsonVerificandoErro(pURL
                                          , pObjetoDeParametros
                                          , function (pResposta) { GradIntra_Busca_BuscarItensParaSelecaoReservaIPO_Callback(pResposta, pDivDeFormulario); }
                                         );
}

function GradIntra_Busca_BuscarItensParaSelecaoReservaIPO_Callback(pResposta, pDivDeFormulario) {
    ///<summary>[CallBack] Função de CallBack de sucesso para GradIntra_Busca_BuscarItensParaSelecao: faz o "databind" da tabela de resultados.</summary>
    ///<param name="pResposta" type="Objeto_JSON">Objeto "ChamadaAjaxResponse" que retornou do code-behind.</param>
    ///<returns>void</returns>

    var lPainelDeResultado = "";

    GradIntra_Busca_ConfigurarGridDeResultados_ReservaIPO();

    lPainelDeResultado = $("#pnlConteudo_Clientes_ReservaIPO")
                                    .show()
                                    .addClass(CONST_CLASS_CONTEUDOCARREGADO)
                                    .children()
                                    .show()
                                    .find("#pnlClientes_Reserva_IPO_Resultados");

    GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);

    lPainelDeResultado.show();

    var lGrid = lPainelDeResultado.find("table.ui-jqgrid-btable")[0];

    lPainelDeResultado
        .parent()
        .find(".Aguarde")
        .hide();

    lGrid.addJSONData(pResposta.ObjetoDeRetorno);

    pDivDeFormulario
            .find(".btnBusca")
                .html("Buscar")
            .parent()
            .find("input, select, button")
                .prop("disabled", false);
}


function GradIntra_Busca_SelecionarItensIndividualmente(rowid, iRow, iCol, e)
{
    var lItemNovoNaListaDeSelecionados;

    var lJson;
    var lObjeto = $(e.currentTarget).getRowData(rowid);

    lJson = $.toJSON(lObjeto);
        
    lItemNovoNaListaDeSelecionados = GradIntra_Navegacao_AdicionarItemNaListaDeItensSelecionados(lJson, null, true);
        
    if(lItemNovoNaListaDeSelecionados)
        GradIntra_Navegacao_ExpandirDadosDoPrimeiroItemSelecionado();

    return false;
}


function GradIntra_Busca_BuscarItensParaListagemSimples(pDivDeFormulario, pDivDeResultados, pURL, pOpcoesDeBusca, pOrdenarPor, pCallBack)
{
    if(pOpcoesDeBusca)
    {
        if(pOpcoesDeBusca.CamposObrigatorios && pOpcoesDeBusca.CamposObrigatorios != null)
        {
            var lCampo;
            var lCamposValidos = true;

            $(pOpcoesDeBusca.CamposObrigatorios).each(function()
            {
                if(lCamposValidos)
                {
                    lCampo = $("#" + this);
                    lCamposValidos = !(
                                           (lCampo.val() == "")
                                        || (lCampo.val() == "__/__/____" && lCampo.hasClass(CONST_CLASS_PICKERDEDATA))
                                     );
                }
            });

            if(!lCamposValidos)
            {
                var lLabel = lCampo.prev("label").html();

                //GradIntra_ExibirMensagem("O", "Favor preencher o campo '" + lLabel + "'");

                alert("Favor preencher o campo '" + lLabel + "'");

                lCampo.focus();

                return;
            }
        }
    }
    
    var lData;
    
    lData = GradIntra_InstanciarObjetoDosControles(pDivDeFormulario);
    
    lData.Acao = "BuscarItensParaListagemSimples";

    lData.OrdenarPor = pOrdenarPor;

    pDivDeFormulario
        .find("input, select, button")
            .prop("disabled", true);

    pDivDeResultados
        .hide()
        .parent()
        .addClass(CONST_CLASS_CARREGANDOCONTEUDO)
        .find(".Aguarde")
            .attr("style", null)
            .show()
            .parent()
        .show();

    window.setTimeout(function () {
        GradIntra_CarregarHtmlVerificandoErro(pURL
                                              , lData
                                              , pDivDeResultados
                                              , function(pResposta, pDivDeResultados) { GradIntra_Busca_BuscarItensParaListagemSimples_Callback(pResposta, pDivDeResultados, pCallBack); }
                                              , { CustomInputs: [".GridIntranet td input[type='checkbox']"] }
                                             );

    }, gGradIntra_Config_DelayDeRequest);
    
    return;
}

function GradIntra_Busca_BuscarItensParaListagemSimples_Callback(pResposta, pDivDeResultados, pCallBack)
{
    pDivDeResultados
        .parent()
        .find(".Aguarde")
                .animate
                (
                      { marginTop: -80, opacity: 0 }
                    , {
                        duration: 400
                        , specialEasing: { marginTop: "easeOutQuad", opacity: "easeOutCubic" }
                        , complete: function () {
                            $("#pnlConteudo .Busca_Container:visible")
                                            .find("input, select, button")
                                                .prop("disabled", false);

                            //--> Inabilita o combo de assores para impedir que os assessores vejam 
                            GradIntra_Relatorios_Gerais_Load(); // os assessorados por outros.
                            GradIntra_RelatoriosDBM_DesabilitarFiltrosParaAssessores();
                            
                            pDivDeResultados
                                            .parent()
                                                .removeClass(CONST_CLASS_CARREGANDOCONTEUDO);

                            pDivDeResultados.fadeIn(300);

                            if (pCallBack && pCallBack != null)
                                pCallBack();
                        }
                    }
                );

        pDivDeResultados.parent().parent().show();

        GradIntra_Relatorios_Complexos_ReceberParte(); //--> Relatórios financeiros do 
}

function GradIntra_Busca_AbrirJanela_ReservaIPO(pCodBovespa , pNomeCliente, pCpfCnpj) 
{
//    var lItemNovoNaListaDeSelecionados;

    //var lJson;

//    var lPainelDeResultado = $("#pnlConteudo_Clientes_ReservaIPO").find("#pnlClientes_Reserva_IPO_Resultados");
//    var lGrid = lPainelDeResultado.find("table.ui-jqgrid-btable")[0];

//    var lObjeto = lGrid.getRowData(pRowid);

    //lJson = $.toJSON(lObjeto);

    var lDados = new Object();

    lDados.CodBovespa  = pCodBovespa;
    lDados.NomeCliente = pNomeCliente;
    lDados.CpfCnpj     = pCpfCnpj;

    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/ReservarIPO.aspx?Acao=ReservarIPO"
                                          , lDados
                                          , GradIntra_Busca_AbrirJanela_ReservaIPO_CallBack
                                         );


    try
    {
        event.cancelBubble = true;
    }catch(Erro){}

    return false;
}

function GradIntra_Busca_AbrirJanela_ReservaIPO_CallBack(pResposta) 
{
    //GradIntra_ExibirMensagem("A", pResposta.Mensagem, true);
    //window.open(pResposta.Mensagem, "ReservaIPO", "height = 856, width = 540, toolbar = no, location = no, status=yes, scrollbars=1");
    window.open(pResposta.MensagemExtendida, "ReservaIPO", "height = 850, width = 655, toolbar = no, location = no, status=yes, scrollbars=no, resizable=false");
    //width=740,height=540
    return false;
}

function txtBusca_Clientes_Termo_IPO_KeyDown(pParametro)
{
    $(pParametro).bind('keypress', function(e)
    {
        if (e.keyCode == 13)
        {
            btnBusca_Click(pParametro);
            try
            {
                event.cancelBubble = true;
            }
            catch (Erro) { }
                
            return false;
        }
    });
} 
