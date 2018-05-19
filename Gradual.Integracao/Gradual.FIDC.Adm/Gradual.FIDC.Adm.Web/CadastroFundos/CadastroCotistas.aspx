<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="CadastroCotistas.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.CadastroCotistas" %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        <div class="formulario box-generic">            
            <div id="CadastroCotistas_pnlFiltroPesquisa">
                
                <!-- Container do filtro -->
                <div class="container-fluid" style="padding-top: 20px">
                    <input type="hidden" value="" id="hdnIdCotistaFidc" name="hdnIdCotistaFidc"/>
                    <div class="col-md-2"></div>
                    <div class="col-md-6" id="colFormBusca">
                    <!--Primeira linha -->
                        <div class="row">
                            <div class="col-md-12">
                                <input type="text" id="txtCadastroCotistaNomeCotista" maxlength="100" class="txtCadastroCotistaNomeCotista form-control" placeholder="Nome do Cotista" title="Nome do Cotista" />
                            </div>
                        </div>   
                        <div class="row">
                            <div class="col-md-12">
                                <input type="text" id="txtCadastroCotistaEmailCotista" class="txtCadastroCotistaEmailCotista form-control" placeholder="Email do Cotista" title="Email do Cotista"/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <input type="text" id="txtCadastroCotistaCpfCnpjCotista" class="txtCadastroCotistaCpfCnpjCotista cnpj form-control" placeholder="CPF / CNPJ" title="CPF / CNPJ"/>
                            </div>
                            <div class="col-md-6">
                                <input style="width: 90%" type="text" id="txtCadastroCotistaDtNascCotista" class="txtCadastroCotistaDtNascCotista form-control" placeholder="Data Nascimento / Fundação" title="Data Nascimento / Fundação"/>
                                <span id="imgDtNascimentoFundacao" class="glyphicon glyphicon-calendar"></span>
                            </div>
                        </div>
                        <div class="row">
                            <div style="height: 20px"></div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <input type="text" id="txtCadastroCotistaClasseCotas" class="txtCadastroCotistaClasseCotas form-control" placeholder="Classe de cotas" title="Classe de cotas"/>
                            </div>
                            <div class="col-md-6">
                                <input style="width: 90%" type="text" id="txtCadastroCotistaDtVencimentoCadastro" class="txtCadastroCotistaDtVencimentoCadastro form-control" placeholder="Data vencimento cadastro" title="Data vencimento cadastro"/>
                                <span id="imgDtVencimentoCadastro" class="glyphicon glyphicon-calendar"></span>
                            </div>                            
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <input type="text" id="txtCadastroCotistaQuantidadeCotas" class="txtCadastroCotistaQuantidadeCotas form-control" placeholder="Quantidade de cotas" title="Quantidade de cotas"/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="checkbox" style="padding-left: 15px">
                                <label><input type="checkbox" value="" id="chkCadastroCotistaInativar" name="chkCadastroCotistaInativar">Cotista Ativo</label>
                            </div>  
                        </div>
                    </div>
                    <div class="col-md-2" style="padding-top: 10px"> 
                        <div class="row">
                            <ol class="form-control" style="visibility: hidden"></ol>
                        </div>
                        <div class="row">
                            <ol class="form-control" style="visibility: hidden"></ol>
                        </div>
                        <div class="col-md-12" style="padding-bottom: 5px">
                            <input type="button" id="btnCadastroCotistasLimpar" value="Limpar" onclick="return LimparDadosCadastroCotista();" class="btn btn-warning  btn-block" />
                        </div>
                        <div class="col-md-12" style="padding-bottom: 5px;">
                            <input type="button" id="btnCadastroCotistasInserir" value="Atualizar" onclick="return btnCadastroCotistaAtualizar_Click();" style="height: 70px" class="btn btn-primary btn-block btn-lg" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Container da Grid -->
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-10">
                        <table id="gridCadastroCotistas" class="table-condensed table-striped"></table>
                        <div id="pgCadastroCotistas"></div>
                    </div>
                </div>
            </div>
        </div>

        <div id="mdlAdicionarProcuradores" class="modal fade" role="dialog">
              <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                  <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 id="mdlAdicionarProcuradoresTitle" class="modal-title">Procuradores</h4>
                  </div>
                  <div class="modal-body">
                    <input type="hidden" id="hidAddProcuradoresIdCotistaFidc" />
                    <input type="hidden" id="hidAddProcuradoresIdCotistaFidcProcurador" />
                    <div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="text" id="txtCadastroCotistaNomeProcurador" class="txtCadastroCotistaNomeProcurador form-control" placeholder="Nome procurador" title="Nome procurador"/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="text" id="txtCadastroCotistaCpfProcurador" class="txtCadastroCotistaCpfProcurador cpf form-control" placeholder="Cpf procurador" title="CPF procurador"/>
                            </div>
                        </div>
                        <div class="row">
                            <div id="dvBtnRemoverArquivoDecreto" style="padding-left: 5px; float: left">
                                <div class="fileUpload btn btn-info btn-sm">
                                    <span>Anexar Decreto...</span>
                                    <input type="file" id="upAnexarDecreto" class="upload" />
                                </div>
                            </div>

                            <div id="dvBtnRemoverArquivoProcuracao" style="padding-left: 5px; float: left">
                                <div class="fileUpload btn btn-info btn-sm">
                                    <span>Anexar Procuracao...</span>
                                    <input type="file" id="upAnexarProcuracao" class="upload" />
                                </div>
                            </div>

                            <div id="dvBtnRemoverArquivoTermo" style="padding-left: 5px; float: left">
                                <div class="fileUpload btn btn-info btn-sm">
                                    <span>Anexar Termo...</span>
                                    <input type="file" id="upAnexarTermo" class="upload" />
                                </div>
                            </div>  
                        </div>
                        <div id="gridCotistaProcuradoresWrapper">
                            <table id="gridCotistaProcuradores" class="table-condensed"></table>                
                            <div id="pgGridCotistaProcuradores"></div>
                        </div>
                    </div>
                  </div>
                  <div class="modal-footer" style="text-align: center">
                    <button type="button" class="btn btn-primary" onclick="fnGravarCotistaProcurador()">Gravar</button>
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

        input.form-control {
          text-transform: uppercase;
        }

        .glyphicongrid {
            font-size: 18px;
        }

        .glyphicon-calendar {
            font-size: 18px;
        }

        .spanAdicionarProcuradores{
            cursor: pointer
        }

        .fileUpload {
            position: relative;
            overflow: hidden;
            margin: 10px;
        }
        .fileUpload input.upload {
            position: absolute;
            top: 0;
            right: 0;
            margin: 0;
            padding: 0;
            font-size: 20px;
            cursor: pointer;
            opacity: 0;
            filter: alpha(opacity=0);
        }

        .removerAnexo { cursor: pointer; }
    </style>
</asp:Content>
