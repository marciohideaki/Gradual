<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="ManutencaoFundos.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.ManutencaoFundos" %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        <div class="formulario box-generic">  
            
            <%-- códigos das categorias de fundos--%>
            <input type="hidden" id="idFundoCategoriaAdministrado" value="0"/>
            <input type="hidden" id="idFundoCategoriaPreOperacional" value="0"/>
            <input type="hidden" id="idFundoCategoriaPrateleira" value="0"/>
            <input type="hidden" id="idFundoCategoriaConstituicao" value="0"/>

            <input type="hidden" id="hidSubCategoriaSelecionada" value="0"/>

            <%-- armazena dados dos fundos selecionados no grid do modal --%>
            <input type="hidden" id="hidArrayListaSelecionada"/>

            <div id="ManutencaoFundos_Principal">
                <!-- Container do filtro -->
                <div class="container-fluid">
                    <div style="height:20px" class="separator">
                    </div> 
                    
                    <div class="row" id="rowFundosAdministrados">
                        <div class="col-md-1"></div>
                        <div class="col-md-3" id="colFundosAdministrados">
                            <table id="gridFundosAdministrados" class="table-condensed"></table>                        
                        </div>
                        <div class="col-md-7">
                            <div id="gridFundosAdministradosFundosWrapper">
                                <table id="gridFundosAdministradosFundos" class="table-condensed"></table> 
                            </div>               
                        </div>
                    </div>
                    
                    <div class="row" id="rowFundosPreOperacionais">
                        <div class="col-md-1"></div>
                        <div class="col-md-3">
                            <table id="gridFundosPreOperacionais" class="table-condensed"></table>                        
                        </div>
                        <div class="col-md-7">
                            <div id="gridFundosPreOperacionaisFundosWrapper">
                                <table id="gridFundosPreFundos" class="table-condensed"></table> 
                            </div>               
                        </div>
                    </div>
                    
                    <div class="row" id="rowFundosPrateleira">
                        <div class="col-md-1"></div>
                        <div class="col-md-3">
                            <table id="gridFundosPrateleira" class="table-condensed"></table>                        
                        </div>
                        <div class="col-md-7">
                            <div id="gridFundosPrateleiraFundosWrapper">
                                <table id="gridFundosPrateleiraFundos" class="table-condensed"></table> 
                            </div>               
                        </div>
                    </div>
                    
                    <div class="row" id="rowFundosConstituicao">
                        <div class="col-md-1"></div>
                        <div class="col-md-3">
                            <table id="gridFundosConstituicao" class="table-condensed"></table>                        
                        </div>
                        <div class="col-md-7">
                            <div id="gridFundosConstituicaoFundosWrapper">
                                <table id="gridFundosConstituicaoFundos" class="table-condensed"></table> 
                            </div>               
                        </div>
                    </div>

                    <%--<div class="row" id="rowGridsSubCategorias">
                        <div class="col-md-1"></div>
                        <div class="col-md-3" id="colFundosAdministradosx">
                            <table id="gridFundosAdministradosx" class="table-condensed"></table>                        
                        </div>
                        <div class="col-md-3">
                            <table id="gridFundosPreOperacionaisx" class="table-condensed"></table>
                        </div>
                        <div class="col-md-3">
                            <table id="gridFundosPrateleirax" class="table-condensed"></table>
                        </div>
                        <div class="col-md-2">
                        </div>
                    </div> 
                    <div class="row">
                        <div class="col-md-1"></div>
                        <div class="col-md-3">
                            <div id="gridFundosAdministradosFundosWrapperx">
                                <table id="gridFundosAdministradosFundosx" class="table-condensed"></table> 
                            </div>               
                        </div>
                        <div class="col-md-3">
                            <div id="gridFundosPreOperacionaisFundosWrapperx">
                                <table id="gridFundosPreFundosx" class="table-condensed"></table>   
                            </div>             
                        </div>
                        <div class="col-md-3">
                            <div id="gridFundosPrateleiraFundosWrapperx">
                                <table id="gridFundosPrateleiraFundosx" class="table-condensed"></table>                
                            </div>
                        </div>
                        <div class="col-md-2">
                            
                        </div>
                    </div>--%>
                </div>
            </div>

            <div id="mdlGerenciarFundos" class="modal fade" role="dialog">
              <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                  <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 id="mdlGerenciarFundosTitle" class="modal-title">Gerenciamento de fundos</h4>
                  </div>
                  <div class="modal-body">
                    <input type="hidden" id="hidIdFundoCategoriaSelecionado" />
                    <input type="hidden" id="hidIdFundoSubCategoriaSelecionado" />
                    <div>
                        <div id="gridAssociacaoFundosCategoriaSubCategoriaWrapper">
                            <table id="gridAssociacaoFundosCategoriaSubCategoria" class="table-condensed"></table>                
                            <%--<div id="pgGridAssociacaoFundosCategoriaSubCategoria"></div>--%>
                        </div>
                    </div>
                  </div>
                  <div class="modal-footer" style="text-align: center">
                    <button type="button" class="btn btn-primary" onclick="fnAtualizarRelacionamentoFundos()">Atualizar Associação</button>
                  </div>
                </div>

              </div>
            </div>
        </div>
    </div>
    
    <style>

        .categorias-header {
            background-color: navy;
            color: white;
            height: 30px;
            text-align: left;
            vertical-align: middle;
            font-size: 13px;
            font-family: 'Arial Rounded MT';
            padding-left: 20px;
            padding-top: 5px
        }
        
        .ui-jqgrid .ui-widget-header {
            background-color: navy;
            background-image: none;
            border-color: navy;
            font-size: 13px;
            font-weight: normal;
            font-family: 'Arial Rounded MT';
            padding-left: 20px
        }
        
        .gridbutton {
            font-size: large
        }

        .ui-jqgrid-view{
            border-color: black;
            border-width: 1px
        }

        .ui-jqgrid .ui-jqgrid-htable {
           table-layout:auto;
           margin:0em;
        }

        .dvlnkGerenciar {
            padding-left: 5px;
            height: 40px;
            padding-top: 10px
        }

        .lnkGerenciar{
            font-size: 17px;
            font-weight: 100;
            font-family: 'Arial'
        }
        
        /*omitir bordas dos grids na página principal*/
        #ManutencaoFundos_Principal .ui-jqgrid { border-right-width: 0px; border-left-width: 0px; }
        #ManutencaoFundos_Principal .ui-jqgrid { border-width: 0px; }
        #ManutencaoFundos_Principal .ui-jqgrid tr.ui-row-ltr td { border-right-color: transparent; }
        #ManutencaoFundos_Principal .ui-jqgrid tr.ui-row-ltr td { border-left-color: transparent; }
        #ManutencaoFundos_Principal .ui-jqgrid tr.ui-row-ltr td { border-bottom-color: transparent; }
        #ManutencaoFundos_Principal th.ui-th-column { border-right-color: transparent !important }
        #ManutencaoFundos_Principal .ui-jqgrid-labels .ui-th-column { border-right-color: transparent }

        #ManutencaoFundos_Principal .ui-jqgrid .ui-jqgrid-hdiv {
            overflow-x: visible
        }

        .imgGridFundos {
            font-size: large
        }

        /*altera cor da linha selecionada no grid desta tela*/
        .ui-widget-content tr.ui-row-ltr.ui-state-highlight,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight th,
        .table-striped tbody tr:nth-child(2n+1).ui-state-highlight td { background:#efefb2 }

    </style>
</asp:Content>

