<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="CalendarioEventos.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.CalendarioEventos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    </style>

    <link rel="stylesheet" href="../Resc/Js/Lib/bootstrap-select-1.12.1/dist/css/bootstrap-select.css" />
    <script type="text/javascript" src="../Resc/Js/Lib/bootstrap-select-1.12.1/dist/js/bootstrap-select.js"></script>

    <div class="innerLR">
        <div class="formulario box-generic">
            <div id="CalendarioEventos_pnlPrincipal">
                <br />
                <div class="row">
                    <%-- Calendario --%>
                    <div class="col-md-3 col-md-offset-1">
                        <div id="datepickerCadEvento"></div>
                        <br />
                    </div>

                    <%-- Formulario --%>
                    <div class="col-md-7">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <div class="input-group date" id="dtHorarioEvento">
                                        <input type="datetime" class="form-control" id="txtHorarioEvento" maxlength="5" placeholder="00:00" />
                                        <span class="input-group-addon" id="dtHorarioEventoIcone">
                                            <span class="glyphicon glyphicon-time"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <textarea id="txtDescEvento" class="form-control" rows="3" maxlength="300" placeholder="DESCRIÇÃO DO EVENTO"></textarea>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <select id="lsRelacionarFundo" class="form-control selectpicker show-tick" data-live-search="true">
                                    <option value="0">RELACIONAR FUNDO</option>
                                </select>
                            </div>
                        </div>
                        <div class="row" style="padding-top:8px !important;">
                            <div class="col-md-12">
                                <input type="email" id="txtEmailNotificacao" maxlength="50" class="form-control" placeholder="  E-MAIL DE NOTIFICAÇÃO" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" value="" id="chkEnviarNotifDia">Enviar notificação no dia</label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" value="" id="chkMostrarHome">Mostrar na home</label>
                                </div>
                            </div>
                            <div class="col-md-3 col-md-offset-1">
                                <input type="button" id="btnSalvarEvento" value="Salvar Evento" onclick="return addEvento();" class="btn btn-primary btn-block" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <!-- Container da Grid -->
            <div>
                <br />
                <div class="row">
                    <div class="col-md-offset-1 col-md-7">
                        <br />
                        <b>
                            <label id="lblDataFiltro"></label>
                        </b>
                    </div>
                    <div class="col-md-offset-1 col-md-2">
                        <button id="btnFiltroAvancado" type="button" class="btn btn-info btn-block" onclick="advFilter();">
                            Filtro Avançado <i class="glyphicon glyphicon-filter"></i>
                        </button>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-10 col-md-offset-1 col-xs-12">
                        <div class="container-fluid">
                            <table id="gridEventos" class="table-condensed table-striped"></table>
                            <div id="pgEventos"></div>
                        </div>
                    </div>
                </div>
                <br />
            </div>

            <!-- Modal Filtro Avancado -->
            <div id="mdlFiltroAvancado" class="modal fade" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 id="mdlFiltroAvancadoTitle" class="modal-title">Filtro Avançado</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        Início
                                        <div class="input-group date" id="dtFiltroAvancadoInicio">
                                            <input type="datetime" class="form-control" id="txtFiltroAvancadoInicio" placeholder="____/____/________" />
                                            <span class="input-group-addon" id="dtFiltroAvancadoInicioIcone">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        Fim
                                        <div class="input-group date" id="dtFiltroAvancadoFim">
                                            <input type="datetime" class="form-control" id="txtFiltroAvancadoFim" placeholder="____/____/________" />
                                            <span class="input-group-addon" id="dtFiltroAvancadoFimIcone">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 right">
                                    <br />
                                    <div class="checkbox">
                                        <label title="Somente eventos futuros (limitado a 1 ano).">
                                            <input type="checkbox" value="" id="chkFiltroAvancadoSomenteEventosFuturos" onclick="return somenteFuturos();" >Somente eventos futuros</label>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <select id="lsFiltroAvancadoFundoRelacionado" class="form-control selectpicker show-tick" data-live-search="true">
                                        <option value="0">FUNDO RELACIONADO</option>
                                    </select>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <div class="row">
                                <div class="col-md-12 right">
                                    <button type="button" class="btn btn-primary" onclick="getEventosFiltroAvancado()">&nbsp;&nbsp;&nbsp;BUSCAR&nbsp;&nbsp;&nbsp;</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
