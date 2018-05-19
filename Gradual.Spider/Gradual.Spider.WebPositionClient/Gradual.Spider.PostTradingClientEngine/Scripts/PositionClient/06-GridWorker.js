
self.addEventListener("message", function (e, args) {
    self.postMessage(e.data);
    //var lGrid = $('#jqGrid_RiscoResumido');

    //console.log('chegou dados no listener');

}, false);

/*
function RiscoResumido_REST_DataBind_Grid_Update(e) {

    var pGrid, pObjeto;

    var lEncontrou = false;

    var rowIds = pGrid.jqGrid('getDataIDs');

    for (row = 0; row <= rowIds.length; row++) {

        rowData = pGrid.jqGrid('getRowData', row);

        if (rowData['CodigoCliente'] == pObjeto.CodigoCliente) {

            var lRow = jQuery('#jqGrid_RiscoResumido tr:eq(' + row + ')');

            pGrid.jqGrid('setRowData', row, pObjeto);

            postMessage(row, pObjeto);

            lEncontrou = true;

            break;
        }
    }

    if (!lEncontrou) {

        var lastRowDOM = pGrid.jqGrid('getGridParam', 'records');

        lRowID = lastRowDOM + 1;

    }
}
*/