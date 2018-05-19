var gGridCategoriaFundosAdministrados;
var gMock_CategoriaFundosAdministradosGrid = {};

function fnCarregarGrids()
{
    $('#rowGridsSubCategorias').hide();

    Grid_Resultado_Fundos_Administrados();
    Grid_Resultado_Fundos_PreOperacionais();
    Grid_Resultado_Fundos_Prateleira();
    Grid_Resultado_Fundos_Constituicao();

    CarregarGridsCategorias();
    //CarregarGridFundosPreOperacionais_Click();
    //CarregarGridFundosPrateleira_Click();

    //remover minimizar grid
    $('.ui-jqgrid-titlebar-close.HeaderButton').remove();

    //remover linha esquerda dos grids
    $('#ManutencaoFundos_Principal .ui-jqgrid-view').css('border-left', 'none');
    
    $('#rowGridsSubCategorias').slideUp(400).delay(400).fadeIn(400);
}

//definição dos grids de sub categoria
function Grid_Resultado_Fundos_Prateleira() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdFundoSubCategoria", jsonmap: "IdFundoSubCategoria", index: "IdFundoSubCategoria", align: "left", sortable: true, hidden: true }
                    , { name: 'edit', index: 'edit', align: 'center', sortable: false, width: '10px' }
                    , { label: "Fundos de Prateleira", name: "DsFundoSubCategoria", jsonmap: "DsFundoSubCategoria", index: "DsFundoSubCategoria", align: "left", sortable: true }
      ]
      , loadonce: true
      , height: 'auto'
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false
        , loadtext: "Loading..."
        //, pager: '#pgCadastroFundos'
      , subGrid: false
      , rowNum: 10
        //, rowList: [20,30,40,50,60]
      , caption: 'Fundos de Prateleira'
      , sortable: true
        , beforeSelectRow: function () {
            return false;
        }
      , afterInsertRow: function (id) {
          var button = "<a class='gridbuttonprat gridbutton' data-id='" + id + "' href='#'>+</a>";
          $(this).setCell(id, "edit", button);
      }
      , loadComplete: function () {
          $(".gridbuttonprat").on('click', function (e) {
              e.preventDefault();
              
              CarregarGridFundos_Prateleira_Subcategorias($(this).data("id"), $(this).text());
          });
      }
    };

    $('#gridFundosPrateleira').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosPrateleira').jqGrid('setGridWidth', 250);
}
function Grid_Resultado_Fundos_Administrados() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
        , colModel: [
                      { label: "Id", key: true, name: "IdFundoSubCategoria", jsonmap: "IdFundoSubCategoria", index: "IdFundoSubCategoria", align: "left", sortable: true, hidden: true }
                    , { name: 'edit', index: 'edit', align: 'center', sortable: false, width: '10px' }
                    , { label: "Fundos Administrados", name: "DsFundoSubCategoria", jsonmap: "DsFundoSubCategoria", index: "DsFundoSubCategoria", align: "left", sortable: true }
          ]
      , loadonce: true
      , height: 'auto'
      , sortorder: "desc"
      , viewrecords: true
      , gridview: false
        //, pager: '#pgCadastroFundos'
      , subGrid: false
      , rowNum: 10000
        , loadtext: "Loading..."
      , caption: 'Fundos Administrados'
      , sortable: true
      , beforeSelectRow: function () {
          return false;
      }
      , afterInsertRow: function (id) {
          var button = "<a class='gridbuttonadm gridbutton' data-id='" + id + "' href='#'>+</a>";
          $(this).setCell(id, "edit", button);
      }
      , loadComplete: function () {
          $(".gridbuttonadm").on('click', function (e) {
              e.preventDefault();
              
              CarregarGridFundos_Administrados_Subcategorias($(this).data("id"), $(this).text());
          });
      }
    };

    $('#gridFundosAdministrados').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosAdministrados').jqGrid('setGridWidth', 250);
}
function Grid_Resultado_Fundos_PreOperacionais() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdFundoSubCategoria", jsonmap: "IdFundoSubCategoria", index: "IdFundoSubCategoria", align: "left", sortable: true, hidden: true }
                    , { name: 'edit', index: 'edit', align: 'center', sortable: false, width: '10px' }
                    , { label: "Fundos Pré-Operacionais", name: "DsFundoSubCategoria", jsonmap: "DsFundoSubCategoria", index: "DsFundoSubCategoria", align: "left", sortable: true }
      ]
      , loadonce: true
      , height: 'auto'
        , loadtext: "Loading..."
      , viewrecords: true
      , gridview: false
        //, pager: '#pgCadastroFundos'
      , subGrid: false
      , rowNum: 10000
        //, rowList: [20,30,40,50,60]
      , caption: 'Fundos Pré-operacionais'
      , sortable: true
        , beforeSelectRow: function () {
            return false;
        }
        , afterInsertRow: function (id) {
            var button = "<a class='gridbuttonpre gridbutton' data-id='" + id + "' href='#'>+</a>";
            $(this).setCell(id, "edit", button);
        }
      , loadComplete: function () {
          $(".gridbuttonpre").on('click', function (e) {
              e.preventDefault();

              CarregarGridFundos_Pre_Subcategorias($(this).data("id"), $(this).text());
          });
      }
    };

    $('#gridFundosPreOperacionais').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosPreOperacionais').jqGrid('setGridWidth', 250);
}
function Grid_Resultado_Fundos_Constituicao() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                      { label: "Id", key: true, name: "IdFundoSubCategoria", jsonmap: "IdFundoSubCategoria", index: "IdFundoSubCategoria", align: "left", sortable: true, hidden: true }
                    , { name: 'edit', index: 'edit', align: 'center', sortable: false, width: '10px' }
                    , { label: "Fundos Pré-Operacionais", name: "DsFundoSubCategoria", jsonmap: "DsFundoSubCategoria", index: "DsFundoSubCategoria", align: "left", sortable: true }
      ]
      , loadonce: true
      , height: 'auto'
        , loadtext: "Loading..."
      , viewrecords: true
      , gridview: false
        //, pager: '#pgCadastroFundos'
      , subGrid: false
      , rowNum: 10000
        //, rowList: [20,30,40,50,60]
      , caption: 'Fundos em Constituição'
      , sortable: true
        , beforeSelectRow: function () {
            return false;
        }
        , afterInsertRow: function (id) {
            var button = "<a class='gridbuttonconst gridbutton' data-id='" + id + "' href='#'>+</a>";
            $(this).setCell(id, "edit", button);
        }
      , loadComplete: function () {
          $(".gridbuttonconst").on('click', function (e) {
              e.preventDefault();

              CarregarGridFundos_Constituicao_Subcategorias($(this).data("id"), $(this).text());
          });
      }
    };

    $('#gridFundosConstituicao').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosConstituicao').jqGrid('setGridWidth', 250);
}

//busca dados dos grids de sub categorias
function CarregarGridsCategorias() {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");

    var lDados =
        {
            Acao: "CarregarGridFundosAdministrados"
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, CarregarGridsCategorias_CallBack);

    return false;
}

function CarregarGridsCategorias_CallBack(pResposta) {
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_CategoriaFundosAdministradosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdFundoSubCategoria: lData.Itens[i].IdFundoSubCategoria,
                        DsFundoSubCategoria: lData.Itens[i].DsFundoSubCategoria
                    };

                gMock_CategoriaFundosAdministradosGrid.rows.push(lObjeto);
            }
        }

        $('#gridFundosAdministrados')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");
        $('#gridFundosPrateleira')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");
        $('#gridFundosPreOperacionais')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");
        $('#gridFundosConstituicao')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");
        
        //oculta os headers das colunas
        //$('#rowGridsSubCategorias .ui-jqgrid-hdiv').hide();

        $("#rowFundosAdministrados .ui-jqgrid-hdiv").hide();
        $("#rowFundosPreOperacionais .ui-jqgrid-hdiv").hide();
        $("#rowFundosPrateleira .ui-jqgrid-hdiv").hide();
        $("#rowFundosConstituicao .ui-jqgrid-hdiv").hide();
    }
}

//definição dos grids
function Definicao_Grid_Fundos_Por_Categoria_Administrados() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
            { label: "Id", key: true, name: "IdFundoCadastro", jsonmap: "IdFundoCadastro", index: "IdFundoCadastro", align: "left", sortable: true, hidden: true },
            { name: 'img', index: 'img', align: 'center', sortable: false, width: '5px' },
            { label: "nomeFundo", name: "nomeFundo", jsonmap: "nomeFundo", index: "nomeFundo", align: "left", sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortname: "nomeFundo"
      , sortorder: "asc"
       ,loadtext: "Loading..."
        ,refreshtext: "Refresh"
        ,refreshtitle: "Reload Grid"
      , viewrecords: true
      , gridview: false
      , subGrid: false
      , rowNum: 10000
      , caption: 'Fundos'      
      , loadComplete: function () {
            $('#gridFundosAdministradosFundosWrapper').show();

            $('.lnkGerenciar').css('color', '#37A6CD');
      }
        , afterInsertRow: function (id) {
            var button = '<span class="glyphicon glyphicon-chevron-right"></span>';
            $(this).setCell(id, "img", button);
        }
        ,onSelectRow: function(id){ 
            redirecionarTelaCadastroFundo(id);
         }
    };

    $('#gridFundosAdministradosFundos').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosAdministradosFundos').jqGrid('setGridWidth', 750);

    //$('#gridFundosAdministradosFundos').width('250px');
}

function redirecionarTelaCadastroFundo(idFundoCadastro) {
        
    editarCadastroFundo(idFundoCadastro);
    window.location.href = "/CadastroFundos/CadastroFundos.aspx?IdFundoCadastroEditar=" + idFundoCadastro;
}

function Definicao_Grid_Fundos_Por_Categoria_Prateleira() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                { label: "Id", key: true, name: "IdFundoCadastro", jsonmap: "IdFundoCadastro", index: "IdFundoCadastro", align: "left", sortable: true, hidden: true },
                { name: 'img', index: 'img', align: 'center', sortable: false, width: '5px' },
                { label: "nomeFundo", name: "nomeFundo", jsonmap: "nomeFundo", index: "nomeFundo", align: "left", sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortname: "nomeFundo"
      , sortorder: "asc"
      , viewrecords: true
      , gridview: false
      , subGrid: false
      , rowNum: 10000
      , caption: 'Fundos'
      , sortable: true
        , loadtext: "Loading..."
        , refreshtext: "Refresh"
        , refreshtitle: "Reload Grid"
        , onSelectRow: function (id) {
            redirecionarTelaCadastroFundo(id);
        }
        , loadComplete: function () {
            $('#gridFundosPrateleiraFundosWrapper').show();
        }
        , afterInsertRow: function (id) {
            var button = '<span class="glyphicon glyphicon-chevron-right"></span>';
            $(this).setCell(id, "img", button);
        }
    };

    $('#gridFundosPrateleiraFundos').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosPrateleiraFundos').jqGrid('setGridWidth', 750);

    //$('#gridFundosPrateleiraFundos').width('250px');
}
function Definicao_Grid_Fundos_Por_Categoria_Pre() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                { label: "Id", key: true, name: "IdFundoCadastro", jsonmap: "IdFundoCadastro", index: "IdFundoCadastro", align: "left", sortable: true, hidden: true },
                { name: 'img', index: 'img', align: 'center', sortable: false, width: '5px' },
                { label: "nomeFundo", name: "nomeFundo", jsonmap: "nomeFundo", index: "nomeFundo", align: "left", sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortname: "nomeFundo"
      , sortorder: "asc"
      , viewrecords: true
      , gridview: false
      , subGrid: false
      , rowNum: 10000
      , caption: 'Fundos'
      , sortable: true
      , loadtext: "Loading..."
      , refreshtext: "Refresh"
      , refreshtitle: "Reload Grid"
        , onSelectRow: function (id) {
            redirecionarTelaCadastroFundo(id);
        }
        , loadComplete: function () {
            $('#gridFundosPreOperacionaisFundosWrapper').show();

            $('.lnkGerenciar').css('color', '#37A6CD');
        }
        , afterInsertRow: function (id) {
            var button = '<span class="glyphicon glyphicon-chevron-right"></span>';
            $(this).setCell(id, "img", button);
        }
    };

    $('#gridFundosPreFundos').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosPreFundos').jqGrid('setGridWidth', 750);

    //$('#gridFundosPreFundos').width('250px');
}
function Definicao_Grid_Fundos_Por_Categoria_Constituicao() {

    gGridCategoriaFundosAdministrados =
    {
        datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
      , colModel: [
                { label: "Id", key: true, name: "IdFundoCadastro", jsonmap: "IdFundoCadastro", index: "IdFundoCadastro", align: "left", sortable: true, hidden: true },
                { name: 'img', index: 'img', align: 'center', sortable: false, width: '5px' },
                { label: "nomeFundo", name: "nomeFundo", jsonmap: "nomeFundo", index: "nomeFundo", align: "left", sortable: false }
      ]
      , loadonce: true
      , height: 'auto'
      , sortorder: "asc"
      , viewrecords: true
      , gridview: false
      , subGrid: false
      , rowNum: 10000
      , caption: 'Fundos'
      , sortable: true
      , loadtext: "Loading..."
      , refreshtext: "Refresh"
      , refreshtitle: "Reload Grid"
        , onSelectRow: function (id) {
            redirecionarTelaCadastroFundo(id);
        }
        , loadComplete: function () {
            $('#gridFundosConstituicaoFundosWrapper').show();

            $('.lnkGerenciar').css('color', '#37A6CD');
        }
        , afterInsertRow: function (id) {
            var button = '<span class="glyphicon glyphicon-chevron-right"></span>';
            $(this).setCell(id, "img", button);
        }
    };

    $('#gridFundosConstituicaoFundos').jqGrid(gGridCategoriaFundosAdministrados);

    $('#gridFundosConstituicaoFundos').jqGrid('setGridWidth', 750);
}

//carregamento dos grids de fundos
function CarregarGridFundos_Prateleira_Subcategorias(id, botao) {

    if (botao === '+') {

        Definicao_Grid_Fundos_Por_Categoria_Prateleira();

        jQuery('#gridFundosPrateleira').jqGrid('setSelection', id);

        $('.gridbuttonprat').text('+');

        $('#gridFundosPrateleira .ui-state-highlight .gridbuttonprat').text('-');

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");

        var lDados =
            {
                Acao: "ResponderCarregarFundosPrateleiraPorCategoriaSubCategoria",
                IdFundoSubCategoria: id,
                IdFundoCategoria: $('#idFundoCategoriaPrateleira').val()
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, CarregarGridFundos_Prateleira_Subcategorias_CallBack);

        //seta título do cabeçalho do grid
        $("#gridFundosPrateleiraFundos").jqGrid("setCaption", $('#gridFundosPrateleira').jqGrid('getCell', id, 'DsFundoSubCategoria'));

        $('#hidSubCategoriaSelecionada').val(id);

        //$('#gridFundosPrateleiraFundosWrapper').show();

    }
    else if (botao === '-') {
        $('#gridFundosPrateleira .ui-state-highlight .gridbuttonprat').text('+');

        jQuery('#gridFundosPrateleira').jqGrid('resetSelection');

        $('#gridFundosPrateleiraFundosWrapper').hide();

    }
    return false;
}

function CarregarGridFundos_Administrados_Subcategorias(id, botao) {

    if (botao === '+') {

        Definicao_Grid_Fundos_Por_Categoria_Administrados();

        jQuery('#gridFundosAdministrados').jqGrid('setSelection', id);

        $('.gridbuttonadm').text('+');

        $('#gridFundosAdministrados .ui-state-highlight .gridbuttonadm').text('-');

        var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");

        var lDados =
            {
                Acao: "ResponderCarregarFundosAdministradosPorCategoriaSubCategoria",
                IdFundoSubCategoria: id,
                IdFundoCategoria: $('#idFundoCategoriaAdministrado').val()
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, CarregarGridFundos_Administrados_Subcategorias_CallBack);

        //seta título do cabeçalho do grid
        $("#gridFundosAdministradosFundos").jqGrid("setCaption", $('#gridFundosAdministrados').jqGrid('getCell', id, 'DsFundoSubCategoria'));

        $('#hidSubCategoriaSelecionada').val(id);
    }
    else if (botao === '-') {
        $('#gridFundosAdministrados .ui-state-highlight .gridbuttonadm').text('+');

        jQuery('#gridFundosAdministrados').jqGrid('resetSelection');

        $('#gridFundosAdministradosFundosWrapper').hide();
    }
    return false;
}
function CarregarGridFundos_Pre_Subcategorias(id, botao) {

    if (botao === '+') {

        Definicao_Grid_Fundos_Por_Categoria_Pre();

        jQuery('#gridFundosPreOperacionais').jqGrid('setSelection', id);

        $('.gridbuttonpre').text('+');

        $('#gridFundosPreOperacionais .ui-state-highlight .gridbuttonpre').text('-');

        //carregamento grid de fundos pré operacionais
        var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");

        var lDados =
            {
                Acao: "ResponderCarregarFundosPreOperacionaisPorCategoriaSubCategoria",
                IdFundoSubCategoria: id,
                IdFundoCategoria: $('#idFundoCategoriaPreOperacional').val()
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, CarregarGridFundos_Pre_Subcategorias_CallBack);

        //seta título do cabeçalho do grid
        $("#gridFundosPreFundos").jqGrid("setCaption", $('#gridFundosPreOperacionais').jqGrid('getCell', id, 'DsFundoSubCategoria'));

        $('#hidSubCategoriaSelecionada').val(id);

    }
    else if (botao === '-') {
        $('#gridFundosPreOperacionais .ui-state-highlight .gridbuttonpre').text('+');
        
        jQuery('#gridFundosPreOperacionais').jqGrid('resetSelection');
        
        $('#gridFundosPreOperacionaisFundosWrapper').hide();
    }
    return false;
}
function CarregarGridFundos_Constituicao_Subcategorias(id, botao) {

    if (botao === '+') {

        Definicao_Grid_Fundos_Por_Categoria_Constituicao();

        jQuery('#gridFundosConstituicao').jqGrid('setSelection', id);

        $('.gridbuttonconst').text('+');

        $('#gridFundosConstituicao .ui-state-highlight .gridbuttonconst').text('-');

        //carregamento grid de fundos pré operacionais
        var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");

        var lDados =
            {
                Acao: "CarregarFundosConstituicaoPorCategoriaSubCategoria",
                IdFundoSubCategoria: id,
                IdFundoCategoria: $('#idFundoCategoriaConstituicao').val()
            };

        Aux_CarregarHtmlVerificandoErro(lUrl, lDados, CarregarGridFundos_Constituicao_Subcategorias_CallBack);

        //seta título do cabeçalho do grid
        $("#gridFundosConstituicaoFundos").jqGrid("setCaption", $('#gridFundosConstituicao').jqGrid('getCell', id, 'DsFundoSubCategoria'));

        $('#hidSubCategoriaSelecionada').val(id);
    }
    else if (botao === '-') {
        $('#gridFundosConstituicao .ui-state-highlight .gridbuttonconst').text('+');

        jQuery('#gridFundosConstituicao').jqGrid('resetSelection');

        $('#gridFundosConstituicaoFundosWrapper').hide();
    }
    return false;
}

//funções de callback do carregamento dos grids de fundos
function CarregarGridFundos_Administrados_Subcategorias_CallBack(pResposta) {
    
    var lData = JSON.parse(pResposta);

    $('#gridFundosAdministradosFundos').jqGrid('clearGridData');
    
    if (!lData.TemErro) {
        gMock_CategoriaFundosAdministradosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        nomeFundo: lData.Itens[i].NomeFundo
                    };

                gMock_CategoriaFundosAdministradosGrid.rows.push(lObjeto);
            }
        }

        $('#gridFundosAdministradosFundos')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");
        
        $("#gridFundosAdministradosFundos").jqGrid('setLabel', 'nomeFundo', '', { 'background': 'white' });

        var idFundoSubCategoria = $('#hidSubCategoriaSelecionada').val();
        var idFundoCategoria = $('#idFundoCategoriaAdministrado').val();

        //adicionar link para gerenciar fundos
        $('#gview_gridFundosAdministradosFundos .ui-jqgrid-htable').replaceWith('<div class="dvlnkGerenciar"><a href="#" id="' +
            'lnkGerenciarFundos' +
            'Adm" class="lnkGerenciar">+ Gerenciar</a></div>');
                
        $('#lnkGerenciarFundosAdm').attr('onclick', 'fnExibirJanelaGerenciarFundos(' + idFundoCategoria + ' ,' + idFundoSubCategoria + ')');

        //altera a cor do link Gerenciar
        $('.lnkGerenciar').css('color', '#37A6CD');

        $('.ui-jqgrid-titlebar-close.HeaderButton').remove();

        //remover linha esquerda dos grids
        $('#ManutencaoFundos_Principal .ui-jqgrid-view').css('border-left', 'none');
    }
}
function CarregarGridFundos_Prateleira_Subcategorias_CallBack(pResposta)
{
    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_CategoriaFundosAdministradosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        nomeFundo: lData.Itens[i].NomeFundo
                    };

                gMock_CategoriaFundosAdministradosGrid.rows.push(lObjeto);
            }
        }

        var idFundoSubCategoria = $('#hidSubCategoriaSelecionada').val();
        var idFundoCategoria = $('#idFundoCategoriaPrateleira').val();

        $('#gridFundosPrateleiraFundos').jqGrid('clearGridData');

        $('#gridFundosPrateleiraFundos')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");

        //altera estilo do fundo do header da coluna
        $("#gridFundosPrateleiraFundos").jqGrid('setLabel', 'nomeFundo', '', { 'background': 'white' });

        //adiciona o link 'Gerenciar'
        $('#gview_gridFundosPrateleiraFundos .ui-jqgrid-htable').replaceWith('<div class="dvlnkGerenciar"><a href="#" id="lnkGerenciarFundosPrateleira" class="lnkGerenciar">+ Gerenciar</a></div>');
        
        $('#lnkGerenciarFundosPrateleira').attr('onclick', 'fnExibirJanelaGerenciarFundos(' + idFundoCategoria + ' ,' + idFundoSubCategoria + ')');

        //altera a cor do link Gerenciar
        $('.lnkGerenciar').css('color', '#37A6CD');

        //remove botão 'ocultar' do header do grid
        $('.ui-jqgrid-titlebar-close.HeaderButton').remove();

        //remover linha esquerda dos grids
        $('#ManutencaoFundos_Principal .ui-jqgrid-view').css('border-left', 'none');
    }
}
function CarregarGridFundos_Pre_Subcategorias_CallBack(pResposta) {

    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_CategoriaFundosAdministradosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        nomeFundo: lData.Itens[i].NomeFundo
                    };

                gMock_CategoriaFundosAdministradosGrid.rows.push(lObjeto);
            }
        }

        $('#gridFundosPreFundos').jqGrid('clearGridData');

        $('#gridFundosPreFundos')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");
        
        //altera estilo do fundo do header da coluna
        $("#gridFundosPreFundos").jqGrid('setLabel', 'nomeFundo', '', { 'background': 'white' });

        var idFundoSubCategoria = $('#hidSubCategoriaSelecionada').val();
        var idFundoCategoria = $('#idFundoCategoriaPreOperacional').val();

        //adiciona o link 'Gerenciar'
        $('#gview_gridFundosPreFundos .ui-jqgrid-htable').replaceWith('<div class="dvlnkGerenciar"><a id="lnkGerenciarFundosPre" href="#" class="lnkGerenciar">+ Gerenciar</a></div>');
        
        $('#lnkGerenciarFundosPre').attr('onclick', 'fnExibirJanelaGerenciarFundos(' + idFundoCategoria + ' ,' + idFundoSubCategoria + ')');

        //altera a cor do link Gerenciar
        $('.lnkGerenciar').css('color', '#37A6CD');

        $('.ui-jqgrid-titlebar-close.HeaderButton').remove();

        //remover linha esquerda dos grids
        $('#ManutencaoFundos_Principal .ui-jqgrid-view').css('border-left', 'none');
    }
}
function CarregarGridFundos_Constituicao_Subcategorias_CallBack(pResposta) {

    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        gMock_CategoriaFundosAdministradosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        if (lData.Itens.length) {
            for (var i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        nomeFundo: lData.Itens[i].NomeFundo
                    };

                gMock_CategoriaFundosAdministradosGrid.rows.push(lObjeto);
            }
        }

        $('#gridFundosConstituicaoFundos').jqGrid('clearGridData');

        $('#gridFundosConstituicaoFundos')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");

        //altera estilo do fundo do header da coluna
        $("#gridFundosConstituicaoFundos").jqGrid('setLabel', 'nomeFundo', '', { 'background': 'white' });

        var idFundoSubCategoria = $('#hidSubCategoriaSelecionada').val();
        var idFundoCategoria = $('#idFundoCategoriaConstituicao').val();

        //adiciona o link 'Gerenciar'
        $('#gview_gridFundosConstituicaoFundos .ui-jqgrid-htable').replaceWith('<div class="dvlnkGerenciar"><a id="lnkGerenciarFundosConstituicao" href="#" class="lnkGerenciar">+ Gerenciar</a></div>');

        $('#lnkGerenciarFundosConstituicao').attr('onclick', 'fnExibirJanelaGerenciarFundos(' + idFundoCategoria + ' ,' + idFundoSubCategoria + ')');

        //altera a cor do link Gerenciar
        $('.lnkGerenciar').css('color', '#37A6CD');

        $('.ui-jqgrid-titlebar-close.HeaderButton').remove();

        //remover linha esquerda dos grids
        $('#ManutencaoFundos_Principal .ui-jqgrid-view').css('border-left', 'none');
    }
}

//exibição do modal de associação fundo x categoria x sub categoria
function fnExibirJanelaGerenciarFundos(idFundoCategoria, idFundoSubCategoria) {
        
    if (idFundoCategoria === $('#idFundoCategoriaAdministrado').val()) {
        $('#mdlGerenciarFundosTitle').text($("#gridFundosAdministrados").jqGrid("getGridParam", "caption") + ' / ' +
            $("#gridFundosAdministradosFundos").jqGrid("getGridParam", "caption")
            );
    }
    else if (idFundoCategoria === $('#idFundoCategoriaPreOperacional').val()) {
        $('#mdlGerenciarFundosTitle').text($("#gridFundosPreOperacionais").jqGrid("getGridParam", "caption") + ' / ' +
            $("#gridFundosPreFundos").jqGrid("getGridParam", "caption")
            );
    }
    else if (idFundoCategoria === $('#idFundoCategoriaPrateleira').val()) {
        $('#mdlGerenciarFundosTitle').text($("#gridFundosPrateleira").jqGrid("getGridParam", "caption") + ' / ' +
            $("#gridFundosPrateleiraFundos").jqGrid("getGridParam", "caption")
            );
    }
    else if (idFundoCategoria === $('#idFundoCategoriaConstituicao').val()) {
        $('#mdlGerenciarFundosTitle').text($("#gridFundosConstituicao").jqGrid("getGridParam", "caption") + ' / ' +
            $("#gridFundosConstituicaoFundos").jqGrid("getGridParam", "caption")
            );
    }
    
    $('#hidIdFundoCategoriaSelecionado').val(idFundoCategoria);
    $('#hidIdFundoSubCategoriaSelecionado').val(idFundoSubCategoria);
    
    Definicao_Grid_Modal_Fundos();
    
    CarregarGridModalFundos(idFundoCategoria, idFundoSubCategoria);
    
    //$('#gridAssociacaoFundosCategoriaSubCategoria').triggerToolbar();
    
    $('#mdlGerenciarFundos').modal('show');
}

//definição do grid contido no modal
function Definicao_Grid_Modal_Fundos() {

    gGridCategoriaFundosAdministrados =
    {
          datatype: "jsonstring"
        , hoverrows: true
        , datastr: gMock_CategoriaFundosAdministradosGrid.rows
        , autowidth: true
        , shrinkToFit: true
        , colModel: [
                     { label: "Id", key: true, name: "IdFundoCadastro", jsonmap: "IdFundoCadastro", index: "IdFundoCadastro", align: "left", sortable: true, hidden: true },
                     { label: "nomeFundo", name: "nomeFundo", jsonmap: "nomeFundo", index: "nomeFundo", align: "left", sortable: false, width: 530 }
        ]
          //, loadonce: true
          , height: 'auto'
          , sortname: "nomeFundo"
          , sortorder: "asc"
          , viewrecords: true
          , gridview: false
          , subGrid: false
          , rowNum: 10000
          , rowList: [10, 20, 30]
          , ignoreCase: true
          , caption: 'Fundos'
          , sortable: true
          , multiselect: true
          , pager: 'pgGridAssociacaoFundosCategoriaSubCategoria'
          , loadtext: "Loading..."        
    };

    $('#gridAssociacaoFundosCategoriaSubCategoria').jqGrid(gGridCategoriaFundosAdministrados);
    
    $("#gridAssociacaoFundosCategoriaSubCategoria").jqGrid('filterToolbar', {
            stringResult: true,
            searchOnEnter: false,
            afterSearch: function () {
                fnSelecionarFundosNoGrid();
            }
    });
}

//busca dados dos grids de sub categorias
function CarregarGridModalFundos(idFundoCategoria, idFundoSubCategoria) {
    var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");

    var lDados =
        {
            Acao: "CarregarGridModalFundos",
            IdFundoCategoria: idFundoCategoria,
            IdFundoSubCategoria: idFundoSubCategoria
        };

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, CarregarGridModalFundos_CallBack);

    return false;
}

//callback do carregamento do grid
function CarregarGridModalFundos_CallBack(pResposta) {

    //zera o campo de filtro do grid
    $('#gridAssociacaoFundosCategoriaSubCategoria')[0].clearToolbar();

    var lData = JSON.parse(pResposta);

    if (!lData.TemErro) {
        
        gMock_CategoriaFundosAdministradosGrid = { page: 1, total: 3, records: 0, rows: [] };

        //adiciona linhas ao grid
        var i;
        if (lData.Itens.length) {
            for (i = 0; i < lData.Itens.length; i++) {
                var lObjeto =
                    {
                        nomeFundo: lData.Itens[i].NomeFundo,
                        IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                        PertenceACategoriaSubCategoria: lData.Itens[i].PertenceACategoriaSubCategoria
                    };

                gMock_CategoriaFundosAdministradosGrid.rows.push(lObjeto);
            }
        }
        
        //preenchimento do grid com os dados retornados
        $('#gridAssociacaoFundosCategoriaSubCategoria')
            .jqGrid('setGridParam', { datatype: 'local', data: gMock_CategoriaFundosAdministradosGrid.rows })
            .trigger("reloadGrid");

        //correção tamanho dos elementos da paginação
        $('.ui-pg-input').width(20);

        //remover botão de minimizar grid
        $('.ui-jqgrid-titlebar-close.HeaderButton').remove();

        var arr = [];
        
        //checar os fundos já associados anteriormente à categoria x sub cartegoria selecionada
        for (i = 0; i < gMock_CategoriaFundosAdministradosGrid.rows.length; i++) {
            if (gMock_CategoriaFundosAdministradosGrid.rows[i].PertenceACategoriaSubCategoria) {
                
                //seleciona as linhas já associadas anteriormente no banco de dados
                //jQuery("#gridAssociacaoFundosCategoriaSubCategoria").jqGrid('setSelection', lData.Itens[i].IdFundoCadastro);
                
                arr.push({
                    IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                    Pertence: true
                });
            }
            else {
                arr.push({
                    IdFundoCadastro: lData.Itens[i].IdFundoCadastro,
                    Pertence: false
                });
            }
        }
                
        //armazena os fundos selecionados
        $('#hidArrayListaSelecionada').val(JSON.stringify(arr));

        fnSelecionarFundosNoGrid();

        $('#gridAssociacaoFundosCategoriaSubCategoria').jqGrid('setGridParam', { onSelectRow: function (id, selected) { fnArmazenarSelecionadosLista(id, selected); } });

        $('#cb_gridAssociacaoFundosCategoriaSubCategoria').hide();
    }
}

function fnAtualizarRelacionamentoFundos() {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");

    var arr1 = JSON.parse($('#hidArrayListaSelecionada').val());

    var arr = [];

    //adiciona os dados selecionados para uma lista para envio ao servidor
    for (var i = 0; i < arr1.length; i++) {

        if (arr1[i].Pertence)
            arr.push({
                IdFundoCadastro: arr1[i].IdFundoCadastro
            });
    }

    var lDados =
        {
            Acao: 'AtualizarRelacionamentoFundos',
            ParamAtualizarRelacionamentoFundos: JSON.stringify(arr),
            IdFundoCategoria: $('#hidIdFundoCategoriaSelecionado').val(),
            IdFundoSubCategoria: $('#hidIdFundoSubCategoriaSelecionado').val()
        }

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, fnAtualizarRelacionamentoFundos_CallBack);
}

function fnAtualizarRelacionamentoFundos_CallBack(pResposta) {

    var resp = JSON.parse(pResposta);

    if (resp.Mensagem === "OK") {
        resp.Mensagem = "Informações atualizadas com sucesso";

        var idFundoCategoriaSelecionado = $('#hidIdFundoCategoriaSelecionado').val();

        if (idFundoCategoriaSelecionado === $('#idFundoCategoriaAdministrado').val()) {
            //recarregar grid de fundos da categoria administrados e sub categoria selecionada
            CarregarGridFundos_Administrados_Subcategorias($('#hidIdFundoSubCategoriaSelecionado').val(), '+');
        }
        else if (idFundoCategoriaSelecionado === $('#idFundoCategoriaPreOperacional').val()) {
            //recarregar grid de fundos da categoria administrados e sub categoria selecionada
            CarregarGridFundos_Pre_Subcategorias($('#hidIdFundoSubCategoriaSelecionado').val(), '+');
        }
        else if (idFundoCategoriaSelecionado === $('#idFundoCategoriaPrateleira').val()) {
            //recarregar grid de fundos da categoria administrados e sub categoria selecionada
            CarregarGridFundos_Prateleira_Subcategorias($('#hidIdFundoSubCategoriaSelecionado').val(), '+');
        }
        else if (idFundoCategoriaSelecionado === $('#idFundoCategoriaConstituicao').val()) {
            //recarregar grid de fundos da categoria administrados e sub categoria selecionada
            CarregarGridFundos_Constituicao_Subcategorias($('#hidIdFundoSubCategoriaSelecionado').val(), '+');
        }
    }
    
    bootbox.alert(resp.Mensagem);

    $('#mdlGerenciarFundos').modal('hide');
}

//Carrega os id das categorias cadastradas na base de dados
function fnCarregarCategoriasParametrizadas() {

    var lUrl = Aux_UrlComRaiz("/CadastroFundos/ManutencaoFundos.aspx");
    
    var lDados =
        {
            Acao: 'CarregarDadosCategoria'
        }

    Aux_CarregarHtmlVerificandoErro(lUrl, lDados, fnCarregarCategoriasParametrizadas_CallBack);
}

function fnCarregarCategoriasParametrizadas_CallBack(pResposta) {
    
    var resp = JSON.parse(pResposta);

    //carrega os campos hidden com as informações de categorias retornadas
    $('#idFundoCategoriaAdministrado').val(resp[0].IdFundoCategoria);
    $('#idFundoCategoriaPreOperacional').val(resp[1].IdFundoCategoria);
    $('#idFundoCategoriaPrateleira').val(resp[2].IdFundoCategoria);
    $('#idFundoCategoriaConstituicao').val(resp[3].IdFundoCategoria);
}

//armazena numa lista cada fundo selecionado, para não perder a seleção após o usuário utilizar o campo de busca
function fnArmazenarSelecionadosLista(id, selected) {

    //recupera a lista de fundos selecionados anteriormente
    var arr = JSON.parse($('#hidArrayListaSelecionada').val());

    //busca o item a ser selecionado / desselecionado
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].IdFundoCadastro == id) {
            arr[i].Pertence = selected;
            break;
        }
    }
    
    //armazena os fundos selecionados
    $('#hidArrayListaSelecionada').val(JSON.stringify(arr));
}

function fnSelecionarFundosNoGrid() {
    
    //zera a seleção do grid
    $("#gridAssociacaoFundosCategoriaSubCategoria").jqGrid('resetSelection');

    var arr = JSON.parse($('#hidArrayListaSelecionada').val());

    //busca o item a ser selecionado / desselecionado
    for (var i = 0; i < arr.length; i++) {

        //seleciona os fundos no grid
        if (arr[i].Pertence)
            $("#gridAssociacaoFundosCategoriaSubCategoria").jqGrid('setSelection', arr[i].IdFundoCadastro);
    }

    $('#hidArrayListaSelecionada').val(JSON.stringify(arr));
}

//executada ao carregar a página
$(document).ready(function () {

    //verifica se a página carregada atual é a tela de manutenção de fundos antes de chamar a função de carregamento de grids
    if ($("#ManutencaoFundos_Principal")[0] != undefined) {

        fnCarregarCategoriasParametrizadas();
        fnCarregarGrids();
    }
});
