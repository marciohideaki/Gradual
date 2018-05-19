/// <reference path="00-Base.js" />
/*
var mydata = {
            "page": "1", "total": 3, "records": "13", "rows": [
                       { id: "1",  operador: "123451", sessao: "123456789123456789", "data": "01/01/2010",sigla: 123, localidade: "vale5"},
                       { id: "2",  operador: "123452",  sessao: "123456789123456789","data": "02/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "3",  operador: "123463",  sessao: "123456789123456789","data": "03/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "4",  operador: "1234564", sessao: "123456789123456789","data": "04/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "5",  operador: "1234565", sessao: "123456789123456789","data": "05/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "6",  operador: "1234566", sessao: "123456789123456789","data": "06/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "7",  operador: "1234567", sessao: "123456789123456789","data": "07/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "8",  operador: "1234568", sessao: "123456789123456789","data": "08/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "9",  operador: "1234569", sessao: "123456789123456789","data": "09/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "10", operador: "1234560", sessao: "123456789123456789","data": "10/01/2010", sigla: 123, localidade: "vale5" },
                       { id: "11", operador: "1234570", sessao: "123456789123456789","data": "11/01/2010", sigla: 123, localidade: "vale5" },
                       { id: "12", operador: "1234572", sessao: "123456789123456789","data": "12/01/2010", sigla: 123, localidade: "vale5" },
                       { id: "13", operador: "1234673", sessao: "123456789123456789","data": "13/01/2010", sigla: 123, localidade: "vale5" }
            ]
        };
        */
//var gOperador = { rows:[]};

var gridOperador =  
{
    datatype: "local"
    //, data: gOperador.rows
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
    , pager: "#pagOperador"
    , colNames: ['Id', 'Operador', 'Sessão', 'Data', 'Sigla', 'Localidade', ""]
    , colModel: [
              { name: 'Id',             index: 'Id',                                            sorttype:   "int",      hidden: true }
            , { name: 'CodigoOperador', index: 'CodigoOperador',    width: 100,                     sorttype:   "string"    }
            , { name: 'Sessao',         index: 'Sessao',            width: 150,                     sorttype:   "float"     }
            , { name: 'Data',           index: 'Data',              width: 150, align:"center",     sorttype:   "float"     }
            , { name: 'Sigla',          index: 'Sigla',             width: 150, align: "right",     sorttype:   "string"    }
            , { name: 'Localidade',     index: 'Localidade',        width: 200, align: "center",    sorttype:   "string"    }
            , { name: 'acoes',      width: 80 }
        ]
    ,
    afterInsertRow: function (rowid, rowdata, rowelem) 
    {
        var tdAplicar    = $("#" + rowid + " td:last");
        var textoAplicar = "detalhes";
        var lIdLogin     = rowelem.Id;
        var linkAplicar  = "<a href='#' class='font-orange' onclick='javascript: Operador_AbrirModal("+ lIdLogin +");'>" + textoAplicar + " " + rowid + "</a>";

        $(tdAplicar).html(linkAplicar);
    }
}

function Operador_PreecheGrid(pOperador)
{
    //gOperador = pOperador;
    gridOperador.data = pOperador.rows;
    $("#gridOperador").jqGrid(gridOperador);
}

function Operador_Salvar() 
{

    var lObjetoJson = {
        Id:                     $(".Operador_Form_idLogin").val(),
        Nome:                   $(".Operador_Form_NomeOperador").val(),
        CodigoOperador:         $(".Operador_Form_CodigoOperador").val(),
        CodigoLocalidade:       $(".Operador_Form_CodigoLocalidade").val(),
        Email:                  $(".Operador_Form_EmailOperador").val(),
        CodAssessor:            $(".Operador_Form_CodigoAssessor").val(),
        Sigla:                  $(".Operador_Form_SiglaOperador").val(),
        Sessao:                 $(".Operador_Form_SessaoOperador").val(),
        AcessaPlataforma:       $(".Operador_Form_AcessaPlataforma").is(":checked"),
        CodAssessorAssociado:   $(".Operador_Form_AssessoresVinculados").val()
        };
    
    var ldLogin =  $(".Operador_Form_idLogin").val();
    
    var lAcao = "Cadastrar";

    if (ldLogin == "" || ldLogin == null)
    {
        lAcao = "Cadastrar";
    }
    else
    {
        lAcao = "Atualizar";
    }

    var lOperador = 
    {
        Acao       : lAcao,
        ObjetoJson : $.toJSON(lObjetoJson)
    };

    var lUrl = "Operador.aspx";

    var pOpcoesPosCarregamento = "";

    var pDivDeFormulario = $("#pnlMensagem");

    GradSpider_CarregarJsonVerificandoErro(lUrl, lOperador, Operador_Salvar_Callback, pOpcoesPosCarregamento, pDivDeFormulario)

    return false;
}

function Operador_Salvar_Callback(pResposta, pPainelParaAtualizar) 
{
    if (pResposta.TemErro) 
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else 
    {
        GradSpider_ExibirMensagemFormulario("A",pResposta, pPainelParaAtualizar) ;

        $("#ModalOperador").modal('hide');
    }

    return false;
}

function Operador_PreencherForm() 
{
    var lObjetoJson = 
    {
        idLogin: $(".Operador_Form_idLogin").val(),
    };

    var lOperador =
    {
        Acao: "Selecionar",
        ObjetoJson: $.toJSON(lObjetoJson)
    };

    var lUrl = "Operador.aspx";

    var pOpcoesPosCarregamento = "";

    var pDivDeFormulario = "";

    GradSpider_CarregarJsonVerificandoErro(lUrl, lOperador, Operador_PreencherForm_Callback, pOpcoesPosCarregamento, pDivDeFormulario)
    
    return false;
}

function Operador_PreencherForm_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        var lRetorno = pResposta.ObjetoDeRetorno;

        $(".Operador_Form_NomeOperador")        .val(lRetorno.Nome);
        $(".Operador_Form_CodigoOperador")      .val(lRetorno.CodigoOperador);
        $(".Operador_Form_EmailOperador")       .val(lRetorno.Email);
        $(".Operador_Form_CodigoAssessor")      .val(lRetorno.CodAssessor);
        $(".Operador_Form_CodigoLocalidade")    .val(lRetorno.CodigoLocalidade);
        $(".Operador_Form_SiglaOperador")       .val(lRetorno.Sigla);
        $(".Operador_Form_SessaoOperador")      .val(lRetorno.Sessao);
        $(".Operador_Form_AcessaPlataforma")    .val(lRetorno.AcessaoPlataforma);
        $(".Operador_Form_AssessoresVinculados").val(lRetorno.CodAssessorAssociado);
    }
}

function Operador_LimparModal()
{
    $(".Operador_Form_NomeOperador")        .val("");
    $(".Operador_Form_CodigoOperador")      .val("");
    $(".Operador_Form_EmailOperador")       .val("");
    $(".Operador_Form_CodigoLocalidade")    .val("");
    $(".Operador_Form_CodigoAssessor")      .val("");
    $(".Operador_Form_SiglaOperador")       .val("");
    $(".Operador_Form_SessaoOperador")      .val("");
    $(".Operador_Form_AcessaPlataforma")    .val("");
    $(".Operador_Form_AssessoresVinculados").val("");

}

function Operador_AbrirModal(pIdLogin)
{
    var lUrl = "Operador.aspx";

    var pOpcoesPosCarregamento = "";

    var pDivDeFormulario = "";

    if (pIdLogin != null)
    {
        var lOperador = { Acao: "Selecionar", ObjetoJson: $.toJSON({  Id : pIdLogin }) };
        //var ObjetoJson = $.toJSON(lOperador);

        GradSpider_CarregarJsonVerificandoErro(lUrl, lOperador, Operador_AbrirModal_Callback, pOpcoesPosCarregamento, pDivDeFormulario);

    }else
    {
        Operador_LimparModal();
        $("#ModalOperador").modal('show');
    }
    
    return false;
}


function Operador_AbrirModal_Callback(pResposta)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        var lRetorno = pResposta.ObjetoDeRetorno;

        $(".Operador_Form_idLogin")             .val(lRetorno.Id);
        $(".Operador_Form_NomeOperador")        .val(lRetorno.Nome);
        $(".Operador_Form_CodigoOperador")      .val(lRetorno.CodigoOperador);
        $(".Operador_Form_EmailOperador")       .val(lRetorno.Email);
        $(".Operador_Form_CodigoAssessor")      .val(lRetorno.CodAssessor);
        $(".Operador_Form_CodigoLocalidade")    .val(lRetorno.CodigoLocalidade);
        $(".Operador_Form_SiglaOperador")       .val(lRetorno.Sigla);
        $(".Operador_Form_SessaoOperador")      .val(lRetorno.Sessao);
        $(".Operador_Form_AcessaPlataforma")    .val(lRetorno.AcessarPlataforma);
        $(".Operador_Form_AssessoresVinculados").val(lRetorno.CodAssessorAssociado);

        $("#ModalOperador").modal('show');
    }
}

function Operador_Filtro_Busca(pSender)
{
    var lUrl = "Operador.aspx";

    var pOpcoesPosCarregamento = "";

    var pDivDeFormulario = $("#pnlMensagem");

    var lObjeto = 
    {
        Nome:               $(".Operador_Filtro_NomeOperador").val(),
        Sessao:             $(".Operador_Filtro_Sessao").val(),
        CodigoLocalidade:   $(".Operador_Filtro_Localidade").val(),
        Sigla:              $(".Operador_Filtro_Sigla").val(),
    };

    if (lObjeto.Sessao == "[Selecione]") lObjeto.Sessao="";

    if (lObjeto.CodigoLocalidade == "[Selecione]") lObjeto.CodigoLocalidade = "";


    var lOperador = { Acao: "Buscar", ObjetoJson: $.toJSON(lObjeto) };
    //var ObjetoJson = $.toJSON(lOperador);

    GradSpider_CarregarJsonVerificandoErro(lUrl, lOperador, Operador_Filtro_Busca_Callback, pOpcoesPosCarregamento, pDivDeFormulario);

    return false;
}

function Operador_Filtro_Busca_Callback(pResposta, pPainelParaAtualizar)
{
    if (pResposta.TemErro)
    {
        GradSpider_TratarRespostaComErro(pResposta);
    }
    else
    {
        $('#gridOperador').jqGrid('GridUnload');

        var lOperador = pResposta.ObjetoDeRetorno;
        
        gridOperador.data = lOperador.rows;

        $("#gridOperador").jqGrid(gridOperador);
    }
}