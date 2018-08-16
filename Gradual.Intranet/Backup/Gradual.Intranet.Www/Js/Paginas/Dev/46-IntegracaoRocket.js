/// <reference path="../../Lib/Etc/Dev/moment-with-locales.js" />
var gDescricoes = {};
var table;
var descMap = {};
var gStatus;

function CarregamentoPagina()
{

    var today       = moment();
    var tomorrow    = moment(today, 'DD MM YY').add(1, 'day');
    var yesterday   = moment(today, 'DD MM YY').subtract(1, 'day');

    $("#txtIntegracaoRocket_Filtro_DataDe").val(yesterday.format('DD/MM/YYYY'));
    $("#txtIntegracaoRocket_Filtro_DataAte").val(tomorrow.format('DD/MM/YYYY'));


    $("#txtIntegracaoRocket_Filtro_DataDe").datepicker();

    GradIntra_IntegracaoRocket_BuscarDescricoes();
    GradIntra_IntegracaoRocket_Buscar();

    $("#pnlConteudo_Clientes_IntegracaoRocket").children(".pnlFormulario").hide();

    $("#pnlClientes_IntegracaoRocket").show();
}

function GradIntra_IntegracaoRocket_BuscarDescricoes()
{
    var lDados = {};

    var lDados =
    {
        Acao            : "SolicitarDescricoes"
        , Codigo        : ""
        , Nome          : ""
    }

    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/IntegracaoRocket/Sumario.aspx"
                                            , lDados
                                            , GradIntra_IntegracaoRocket_BuscarDescricoes_CallBack);
}

function GradIntra_IntegracaoRocket_BuscarDescricoes_CallBack(pResposta)
{
    var lResposta = pResposta.ObjetoDeRetorno;

    $(lResposta.Descricoes).each(function ()
    {
        descMap[this.Nome] = this.Descricao;
    });
}

function GradIntra_IntegracaoRocket_MostrarDetalhe(pCodigo, pStatus, pDataConclusao, pScore, pDocumentos, pDadosEntrada, pDadosSaida)
{
    var modal = document.getElementById("detalheModal");

    gStatus = pStatus;
    gDocs = pDocumentos;
    
    $("detalheModal").on("shown.bs.modal", function ()
    {
        $(this).find(".modal-dialog").css({ width: "auto", height: "auto", "max-height": "100%" });
    });

    var lClasse;

    if (pStatus == "APROVADO")
    {
        lClasse = "aprovado";
        $("#header").removeClass("reprovado").removeClass("documentacao").addClass("aprovado");
    }
    else
    {
        if (pDocumentos == true)
        {
            lClasse = "documentacao";
            $("#header").removeClass("aprovado").removeClass("reprovado").addClass("documentacao");
        }
        else
        {
            lClasse = "reprovado";
            $("#header").removeClass("aprovado").removeClass("documentacao").addClass("reprovado");
        }
    }

    var lCabecalho = "<h3 style=\""+ lClasse +"\">Status: " + pStatus + " em : " + moment(pDataConclusao).format("DD/MM/YYYY HH:mm") + " | Score : " + pScore + " Pts</h3>"
    document.getElementById("headerText").innerHTML = lCabecalho;

    //TASK: a div é mostrar antes para permitir o ajuste do cabeçalho (Workaround)
    modal.style.display = "block";

    var objEntrada = jQuery.parseJSON(pDadosEntrada);
    var objSaida = jQuery.parseJSON(pDadosSaida);

    GradIntra_IntegracaoRocket_AjustarRodape(pCodigo, pStatus, pDocumentos, objEntrada.EMAIL);


    if ($.fn.dataTable.isDataTable("#tbDetalhe"))
    {
        var tblDetalhe = $("#tbDetalhe").DataTable();
        tblDetalhe.clear().draw();
        tblDetalhe.rows.add(objSaida.Contextos);
        tblDetalhe.draw();
    }
    else
    {
        var tblDetalhe = $("#tbDetalhe").DataTable(
        {
             data               : objSaida.Contextos
            , scrollY           : "50vh"
            , scrollX           : true
            , bScrollCollapse   : true
            , paging            : false
            , responsive: true
            , language:
            {
                processing: "Processando requisição."
                , emptyTable: "Nenhum registro encontrado, e ou, o Rocket ainda não respondeu."
            }
            , fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull)
            {
                GradIntra_IntegracaoRocket_DecorateRow(nRow);
                return nRow;
            }
            , columnDefs:
            [
                  { width   : 100   , targets: 1 }
                , { visible : false , targets: 0 }
                , { visible : false , targets: 4 }
            ]
            , columns:
            [
                  { title: "Output"     , data: "IDOutput"  }
                , { title: "Nome"       , data: "Nome"      }
                , { title: "Valor"      , data: "Valor"     }
                , { title: "Descrição"  , data: "Descricao" , render: function (data, type, row) { if (row.Nome === null) return ""; return descMap[row.Nome]; } }
                , { title: "TipoCampo"  , data: "TipoCampo" }
            ]
            , fixedColumns  : true
            , bFilter       : false
            , bInfo         : false
            , fixedHeader   :
            {
                  header: true
                , footer: false
            }
        });

        tblDetalhe.draw();
    }
}


function GradIntra_IntegracaoRocket_DecorateRow(row)
{
    if (gStatus == "APROVADO")
    {
        $(row).addClass("aprovado");
    }
    else
    {
        if (gDocs === true)
        {
            $(row).addClass("documentacao");
        }
        else
        {
            $(row).addClass("reprovado");
        }
    }
}


function GradIntra_IntegracaoRocket_SolicitarDocumentacao(pCodigo, pEmail)
{
    var lDados = { Acao: "SolicitarDocumentos", Codigo: pCodigo, Email: pEmail };
        
    GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/IntegracaoRocket/Sumario.aspx"
                                        , lDados
                                        , GradIntra_IntegracaoRocket_SolicitarDocumentacao_CallBack);
}

function GradIntra_IntegracaoRocket_btnIntegracaoRocket_Buscar()
{
    var table = $("#example").DataTable();
    table.clear().draw();

    GradIntra_IntegracaoRocket_Buscar();
    GradIntra_IntegracaoRocket_BuscarDescricoes();

    return false;
}

function GradIntra_IntegracaoRocket_Buscar()
{
    var dtDataInicial   = "";
    var dtDataFinal     = "";
    var strStatus       = "";
    var strTransacao    = "";
    var intCodigo       = "";

    dtDataInicial       = $("#txtIntegracaoRocket_Filtro_DataDe").val();
    dtDataFinal         = $("#txtIntegracaoRocket_Filtro_DataAte").val();
    strStatus           = $("#cboIntegracaoRocket_Filtro_Status").val();
    strTransacao        = $("#cboIntegracaoRocket_Filtro_Transacao").val();
    intCodigo           = $("#txtIntegracaoRocket_Filtro_Codigo").val();

    var date1 = /(\d+)\/(\d+)\/(\d+)/.exec(dtDataInicial);
    date1 = new Date(date1[3], date1[2], date1[1]);
    var date2 = /(\d+)\/(\d+)\/(\d+)/.exec(dtDataFinal);
    date2 = new Date(date2[3], date2[2], date2[1]);
    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

    if (diffDays > 15)
    {
        alert('Solicite um periodo inferior a 15 dias!');
    }
    else
    {

        document.getElementById("processando").style.display = "block";

        var lDados = {};

        var lDados =
        {
            Acao            : "Pesquisar"
            , DataInicial   : dtDataInicial
            , DataFinal     : dtDataFinal
            , Status        : strStatus
            , Transacao     : strTransacao
            , Codigo        : intCodigo
        }

        GradIntra_CarregarJsonVerificandoErro("Clientes/Formularios/Acoes/IntegracaoRocket/Sumario.aspx"
                                                , lDados
                                                , GradIntra_IntegracaoRocket_Buscar_CallBack);
                                                }
}

function GradIntra_IntegracaoRocket_Buscar_CallBack(pResposta)
{

    var lResposta = pResposta.ObjetoDeRetorno;

    if ($.fn.dataTable.isDataTable("#example"))
    {
        table = $("#example").DataTable();
        table.rows.add(pResposta.ObjetoDeRetorno.Capivaras);
        table.draw();
    }
    else
    {
        table = $("#example").DataTable
        ({
            data: pResposta.ObjetoDeRetorno.Capivaras
            , processing: true
            , language:
            {
                processing: "Processando requisição."
                , emptyTable: "Nenhum registro encontrado até o momento."
            }
            , fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull)
            {
                switch (aData.Status)
                {
                    case "REPROVADO":
                        {
                            if (aData.SolicitouDocumentacao)
                            {
                                $(nRow).css("background-color", "#F0E68C !important")
                            }
                            else
                            {
                                $(nRow).css("background-color", "#FF7F50 !important");
                            }

                            break;
                        }
                    case "APROVADO":
                        $(nRow).css("background-color", "#8FBC8F !important");
                        break;
                }


            }
            , scrollY           : "75vh"
            , scrollX           : true
            , bScrollCollapse   : true
            , paging            : false
            , columnDefs        :
            [
                  { width   : 400, targets: 3 }
                , { visible : false, targets: 12 }
            ]
            , columns:
            [
                {
                    className       : "details-control"
                    , orderable     : false
                    , data          : null
                    , defaultContent: ""
                }
                , { title: "Codigo"         , data: "IDCapivara"            }
                , { title: "Cliente"        , data: "Cpf"                   }
                , { title: "Nome"           , data: "Nome"                  }
                , { title: "Transacao"      , data: "Transacao"             }
                , { title: "Solicitacao"    , data: "DataSolicitacaoRocket" , type: "datetime", "render": function (value) { if (value === null) return ""; return moment(value).format("DD/MM/YYYY HH:mm"); } }
                , { title: "Atualizacao"    , data: "DataAtualizacao"       , type: "datetime", "render": function (value) { if (value === null) return ""; return moment(value).format("DD/MM/YYYY HH:mm"); } }
                , { title: "Conclusao"      , data: "DataAtualizacao"       , type: "datetime", "render": function (value) { if (value === null) return ""; return moment(value).format("DD/MM/YYYY HH:mm"); } }
                , { title: "Ticket"         , data: "IDProcessoRocket"      }
                , { title: "Status"         , data: "Status"                }
                , { title: "Documentos"     , data: "SolicitouDocumentacao" , type: "datetime", "render": function (value) { if (value === true) { return "Sim"; } else { return "Não"; } } }
                , { title: "Potuacao"       , data: "Score"                 }
                , { title: "JsonOutput"     , data: "JsonOutput"            }
            ]
            , fixedColumns      : true
            , bFilter           : false
            , bInfo             : false
            , fixedHeader       :
            {
                header  : true
                , footer: false
            }
        });

    }

    // Add event listener for opening and closing details
    $("#example tbody").unbind('click').on("click", "td.details-control", function ()
    {
        var data = table.row(this).data();

        if (data.Status === "EM ANÁLISE")
        {
            alert("O processo atual está em análise!");
        }
        else
        {
            GradIntra_IntegracaoRocket_MostrarDetalhe(data.IDCapivara, data.Status, data.DataAtualizacao, data.Score, data.SolicitouDocumentacao, data.JsonInput, data.JsonOutput);
        }
    });

    document.getElementById("processando").style.display = "none";

}

function GradIntra_IntegracaoRocket_SolicitarDocumentacao_CallBack(pResposta)
{
    document.getElementById("detalheModal").style.display = "none";

    GradIntra_ExibirMensagem("I", pResposta.Mensagem, true);
}

function GradIntra_IntegracaoRocket_FecharDetalhe()
{
    document.getElementById("detalheModal").style.display = "none";

    // Limpa o DataTable da janela modal do detalhe
    if ($.fn.dataTable.isDataTable("#tbDetalhe"))
    {
        var tblDetalhe = $("#tbDetalhe").DataTable();
        tblDetalhe.clear().draw();
    }
}

function GradIntra_IntegracaoRocket_AjustarRodape(pCodigo, pStatus, pDocumento, pEmail)
{

    $("#bntSolicitacao").attr("onclick", "GradIntra_IntegracaoRocket_SolicitarDocumentacao(" + pCodigo + ",'" + pEmail + "');");

    if (pStatus == "REPROVADO")
    {
        if (pDocumento != true)
        {
            document.getElementById("rodapeAprovado").style.display = "none";
            document.getElementById("rodapeReprovado").style.display = "block";
        }
        else
        {
            document.getElementById("rodapeAprovado").style.display = "block";
            document.getElementById("rodapeReprovado").style.display = "none";
        }
    }
    else
    {
        document.getElementById("rodapeAprovado").style.display = "block";
        document.getElementById("rodapeReprovado").style.display = "none";
    }
}