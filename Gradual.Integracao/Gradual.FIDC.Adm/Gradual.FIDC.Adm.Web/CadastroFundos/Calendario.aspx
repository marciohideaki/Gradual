<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Principal.Master" CodeBehind="Calendario.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.Calendario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #dvCalendarioEventos {
            /*max-width: 1000px;*/
            margin: 0 auto;
        }

        .breadcrumb .glyphicons i:before {
            top: 8px !important;
        }
    </style>

    <div class="innerLR">
        <div class="formulario box-generic">
            <div id="Calendario_pnlPrincipal">
                <br />
                <div class="row">
                    <div class="col-md-offset-1 col-md-10">
                        <div id='dvCalendarioEventos'></div>
                    </div>
                </div>
                <br />
                <br />
            </div>

            <!-- Modal Detalhe Evento -->
            <div id="mdlFiltroAvancado" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <div class="row">
                                <div class="col-md-8">
                                    <h4 id="lblDetalheEventoTitulo" class="modal-title"></h4>
                                </div>
                                <div class="col-md-4 right">
                                    <button type="button" class="btn btn-primary" onclick="return ctrl.getNavigationEvent('previous');"><i class="glyphicon glyphicon-chevron-left"></i></button>
                                    <button type="button" class="btn btn-primary" onclick="return ctrl.getNavigationEvent('next');"><i class="glyphicon glyphicon-chevron-right"></i></button>
                                </div>
                            </div>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-2 right"><b>Hora:</b></div>
                                        <div class="col-md-10"><p id="lblDetalheEventoHora"></p></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2 right"><b>Fundo:</b></div>
                                        <div class="col-md-10"><p id="lblDetalheEventoFundo"></p></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2 right"><b>Descrição:</b></div>
                                        <div class="col-md-10 text-justify"><p id="lblDetalheEventoDesc"></p></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="text-align: center">
                            <div class="row">
                                <div class="col-md-offset-9 col-md-3 right">
                                    <input type="button" class="btn btn-primary btn-block" value="   FECHAR   " data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
