<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="AlteracaoRegulamentoConsultaFundos.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.AlteracaoRegulamentoConsultaFundos" validateRequest="false"  %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    
    <style type="text/css">
        #content {
            margin: 0 0 0 0px !important;
        }

        .datepicker {
            box-shadow: 2px 2px 10px #888888 !important;
        }

        .modal-dialog {
            width: 640px !important;
        }

        .breadcrumb .glyphicons i:before {
            top: 8px !important;
        }

        .dropdown-menu.open {
            width: 100% !important;
        }

        .bootstrap-select.form-control {
            border: 1px solid #ccc !important;
        }

        .btn-default {
            background: #fff;
        }
    </style>

    <link rel="stylesheet" href="../Resc/Js/Lib/bootstrap-select-1.12.1/dist/css/bootstrap-select.css" />
    <script type="text/javascript" src="../Resc/Js/Lib/bootstrap-select-1.12.1/dist/js/bootstrap-select.js"></script>

    <div class="innerLR">
        <div class="formulario box-generic">            
            <div id="AlteracaoRegulamentoConsultaFundos_pnlFiltroPesquisa">                
                <!-- Container do filtro -->
                <div class="container-fluid">
                    <div class="col-md-1"></div>
                    <div class="col-md-9" id="colFormBusca">
                    <!--Primeira linha -->
                        <div class="row">
                            <div class="col-md-10 col-xs-10" style="padding-left: 25px; padding-top: 10px">
                            <h4>Alteração de Regulamento</h4></div>
                            <div class="col-md-5 col-xs-5" style="padding-left: 25px">
                                <select id="listaFundosConstituicao" class="form-control selectpicker show-tick" data-live-search="true">
                                    <option value="0">Selecione um Fundo</option>
                                </select>
                            </div>
                            <div class="col-md-5 col-xs-5">
                                <select id="listaGruposFundosConstituicao" class="form-control">
                                    <option value="0">Selecione um Grupo</option>
                                </select>
                            </div>
                            <div class="col-md-2 col-xs-2">
                                <input type="button" id="btnBuscarDados" value="Consultar" onclick="return AltRequerimentofnBuscarDadosAlteracaoRegulamentoConsultaFundos();" class="btn btn-info btn-block" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-5 col-xs-5">
                                <div class="col-md-5 col-xs-5" style="width: 110px">
                                    <div class="checkbox disabled">
                                        <label><input type="checkbox" id="chkFundosPendentes" checked>Pendentes</label>
                                    </div>
                                </div>
                                <div class="col-md-5 col-xs-5">
                                    <div class="checkbox disabled">
                                        <label><input type="checkbox" id="chkFundosConcluidos" checked>Concluídos</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5 col-xs-5">
                                <div style="width: 165px; float:left">
                                    <input type="text" id="txtDataDe" class="form-control" placeholder="Data De" style="width: 130px" />
                                    <span id="imgDtDe" class="glyphicon glyphicon-calendar"></span>
                                </div>
                                <div style="width: 180px; float:left">
                                    <input type="text" id="txtDataAte" class="form-control" placeholder="Data Até" style="width: 130px" />
                                    <span id="imgDtAte" class="glyphicon glyphicon-calendar"></span>
                                </div>
                            </div> 
                            <div class="col-md-2 col-xs-2">
                                <input type="button" id="btnLimparFiltros" value="Limpar Filtros" onclick="return fnLimparFiltrosAlteracaoRegulamentoConsultaFundos();" class="btn btn-warning btn-block" />
                            </div>                     
                        </div>
                    </div>
                </div>
             </div>
            <div class="separator"></div>
            <!-- Container da Grid -->
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-1 col-xs-1"></div>
                    <div class="col-md-10 col-xs-10">
                        <table id="gridConsultaFundos" class="table-condensed table-striped"></table>
                        <div id="pgGridConsultaFundos"></div>
                    </div>
                </div>
            </div>
            <div id="mdlEnviarEmail" class="modal fade" role="dialog">
              <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                  <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 id="mdlEnviarEmailTitle" class="modal-title">Enviar e-mail</h4>
                  </div>
                  <div class="modal-body">
                    <input type="hidden" id="hidDadosEmail" />
                    <div class="row" style="height: 80px; padding-left: 16px; padding-top: 20px">
                        <label for="txtEmailsConsultaFundos">E-mails (separados por vírgula):</label>
                        <input id="txtEmailsConsultaFundos" type="text" class="form-control" style="width:500px"/>
                    </div>
                    <div id="dvConteudoEmailConsultaFundos" style="height: 200px">
                        
                    </div>
                  </div>
                  <div class="modal-footer" style="text-align: center">
                    <button type="button" class="btn btn-primary" onclick="AltRequerimentofnEnviarEmailAlteracaoRegulamentoConsultaFundos()">Enviar</button>
                  </div>
                </div>

              </div>
            </div>
        </div>        

    </div>
    
    <style>
        .colspan2lines {
            height: 100px
        }

        a.glyphicons i:before {
            font-size: 15px;
            color: #37a6cd;
            display: inline-block;
            padding: 5px 0 5px 0;
        }

        /*altera cor da linha selecionada no grid desta tela*/
        .ui-widget-content tr.ui-row-ltr.ui-state-highlight,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight th,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight td {
            background: #efefb2;
        }

        span.glyphicon-calendar {
            font-size: 1.5em;
        }
        
    </style>
</asp:Content>
