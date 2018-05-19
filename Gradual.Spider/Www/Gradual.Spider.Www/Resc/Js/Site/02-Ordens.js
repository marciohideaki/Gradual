
var mydata = {"page": "1", "total": 3, "records": "13", "rows": [
                   { id: "order1", sessao: "123451", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1000, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-01" },
                   { id: "order2", sessao: "123452", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1100, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-02" },
                   { id: "order3", sessao: "123463", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1200, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-09-01" },
                   { id: "order4", sessao: "1234564", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1300, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-04" },
                   { id: "order5", sessao: "1234565", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1400, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-05" },
                   { id: "order6", sessao: "1234566", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1500, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-09-06" },
                   { id: "order7", sessao: "1234567", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1600, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-04" },
                   { id: "order8", sessao: "1234568", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1700, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-03" },
                   { id: "order9", sessao: "1234569", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1800, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-09-01" },
                   { id: "order10", sessao: "1234560", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 1900, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-03" },
                   { id: "order11", sessao: "1234570", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 2000, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-09-01" },
                   { id: "order12", sessao: "1234572", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 2100, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-10-02" },
                   { id: "order13", sessao: "1234673", ClodId: "123456789123456789", cliente: 123, papel: "vale5", qtdeTotal: 2200, qtdeExec: 800, qtdeRes: 200, status: "parc. executada", hora: "12:00", tipo: "válida para o dia", validade: "2007-09-01" }
        ]
        };


        var subgrid = {
            order1: [
                { id: "1", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 100, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", },
                { id: "2", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 200, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", },
                { id: "3", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 300, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", },
                { id: "4", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 400, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", },
                { id: "5", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 500, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", }
            ],
            order2: [
                { id: "1", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 200, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", },
                { id: "2", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 300, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", },
                { id: "3", "descricao": "msg descrição", "qtdeSolic": 1000, "qtdeExec": 400, "leaves": 100, "preco": "20,00", "status": "parcialmente executada", "dataHora": "01/01/2000 15:00", },
            ]
        };

var gridOrdens =  
{
        datatype: "local"
        , data: mydata.rows
        //, data: data.responseText
       // url: "/Ordens/Ordens.aspx?Acao=PesquisarOrdens"
        //, datatype: "json"
        , height: "100%"
        , jsonReader: {
            repeatitems: false
        }
        , sortable: true
        , autoencode: true
        , loadonce: true
        , viewrecords: true
        , rowNum: 5
        , pager: "#paging"
        , colNames: ['Id', 'Sessão', 'CLOdID', 'Cliente', 'Papel', 'Qtd. Total', 'Qtd. Exec.', 'Qtd. Res', 'Status', 'Hora', 'Tipo', 'Validade']
        , colModel: [
              { name: 'ClOrdID',                index: 'ClOrdID',               sorttype: "string", hidden: true }
            , { name: 'Sessao',                 index: 'Sessao',                width: 60,  sorttype: "float" }
            , { name: 'ClOrdID',                index: 'ClOrdID',               width: 140 }
            , { name: 'Account',                index: 'Account',               width: 70, align: "right", sorttype: "float" }
            , { name: 'Symbol',                 index: 'Symbol',                width: 60, align: "right", sorttype: "float" }
            , { name: 'OrderQty',               index: 'OrderQty',              width: 75, align: "right", sorttype: "float" }
            , { name: 'CumQty',                 index: 'CumQty',                width: 75, align: "right", sortable: 'float' }
            , { name: 'OrderQtyRemmaining',     index: 'OrderQtyRemmaining',    width: 75, align: "right", sortable: 'float' }
            , { name: 'OrdStatus',              index: 'OrdStatus',             width: 110, sortable: 'string' }
            , { name: 'hora',                   index: 'hora',                  width: 50,  sortable: 'time' }
            , { name: 'tipo',                   index: 'tipo',                  width: 120, sortable: false }
            , { name: 'validade',               index: 'validade',              width: 80,  sortable: 'time' }
        ]
        , subGrid: true
        , subGridRowExpanded: function (subgridDivId, rowId) {
                     
            var subgridTableId = subgridDivId + "_t";
            $("#" + subgridDivId).html("<table id='" + subgridTableId + "'></table>");
            $("#" + subgridTableId).jqGrid({
                datatype: 'local',
                data: subgrid[rowId],
                colNames: ['', 'Qtde Solicitada', 'Qtde Exec.', 'Leaves', 'Preço', 'Status', 'Data', 'Descrição'],
                colModel: [
                    { name: 'id', hidden:true},
                    { name: 'qtdeSolic', index: 'qtdeSolic', width: 100 },
                    { name: 'qtdeExec', index: 'qtdeExec', width: 100 },
                    { name: 'leaves', index: 'leaves', width: 100 },
                    { name: 'preco', index: 'preco', width: 100 },
                    { name: 'status', index: 'status', width: 100 },
                    { name: 'dataHora', index: 'dataHora', width: 100 },
                    { name: 'descricao', index: 'descricao', width: 100 }
                ],
                gridview: true,
                rownumbers: true,
                autoencode: true,
                sortname: 'hora',
                sortorder: 'desc',
                height: '100%',
            });
        },
        subGridOptions: {
     
            plusicon: "glyph-icon icon-plus",
            minusicon: "glyph-icon icon-minus",
            openicon: ""
        }
        , multiselect: true
}


 gridOrdens.pager = "#pagOrdens_Todas";
$("#gridOrdens_Todas").jqGrid(gridOrdens);

 gridOrdens.pager = "#pagOrdens_Executadas";
$("#gridOrdens_Executadas").jqGrid(gridOrdens);

 gridOrdens.pager = "#pagOrdens_Abertas";
$("#gridOrdens_Abertas").jqGrid(gridOrdens);

 gridOrdens.pager = "#pagOrdens_Canceladas";
$("#gridOrdens_Canceladas").jqGrid(gridOrdens);

 gridOrdens.pager = "#pagOrdens_Rejeitadas";
$("#gridOrdens_Rejeitadas").jqGrid(gridOrdens);

 gridOrdens.pager = "#pagOrdens_Historicas";
$("#gridOrdens_Historicas").jqGrid(gridOrdens);