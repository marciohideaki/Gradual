<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="CadastroFundos.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.CadastroFundos" %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        <div class="formulario box-generic">            
            <div id="CadastroFundos_pnlFiltroPesquisa">
                
                <!-- Container do filtro -->
                <div class="container-fluid">
                    <input type="hidden" value="" id="hdnIdFundoCadastro" name="hdnIdFundoCadastro"/>
                    <div class="col-md-1"></div>
                    <div class="col-md-8" id="colFormBusca" style="padding-top: 20px">
                    <!--Primeira linha -->
                        <div class="row">
                            <div class="col-md-7 col-xs-6">
                                <input type="text" id="txtCadastroFundoADMNomeFundo" maxlength="100" class="txtCadastroFundoNomeFundo form-control" placeholder="Nome do fundo" />
                            </div>
                            <div class="col-md-5 col-xs-6">
                                <input type="text" id="txtCadastroFundoADMCNPJFundo" class="txtCadastroFundoCNPJFundo cnpj form-control" placeholder="CNPJ do fundo"/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-7 col-xs-6">
                                <input type="text" id="txtCadastroFundoADMNomeAdministrador" maxlength="100" class="txtCadastroFundoNomeAdministrador form-control" placeholder="Nome do administrador" />
                            </div>
                            <div class="col-md-5 col-xs-6">
                                <input type="text" id="txtCadastroFundoADMCNPJAdministrador" class="txtCadastroFundoCNPJAdministrador cnpj form-control" placeholder="CNPJ do administrador"/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-7">
                                <input type="text" id="txtCadastroFundoADMNomeCustodiante" maxlength="100" class="txtCadastroFundoNomeCustodiante form-control" placeholder="Nome do custodiante" />
                            </div>
                            <div class="col-md-5">
                                <input type="text" id="txtCadastroFundoADMCNPJCustodiante" class="txtCadastroFundoCNPJCustodiante cnpj form-control" placeholder="CNPJ do custodiante"/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-7">
                                <input type="text" id="txtCadastroFundoADMNomeGestor" maxlength="100" class="txtCadastroFundoNomeGestor form-control" placeholder="Nome do gestor" />
                            </div>
                            <div class="col-md-5">
                                <input type="text" id="txtCadastroFundoADMCNPJGestor" class="txtCadastroFundoCNPJGestor cnpj form-control" placeholder="CNPJ do gestor"/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3"><input type="text" id="txtCadastroFundoTxGestao" class="txtCadastroFundoTxGestao form-control taxa" placeholder="Taxa de gestão" title="Taxa de gestão"/></div>
                            <div class="col-md-4"><input type="text" id="txtCadastroFundoTxCustodia" class="txtCadastroFundoTxCustodia form-control taxa" placeholder="Taxa de custódia" title="Taxa de custódia"/></div>
                            <div class="col-md-5"><input type="text" id="txtCadastroFundoTxConsultoria" class="txtCadastroFundoTxConsultoria form-control taxa" placeholder="Taxa de consultoria" title="Taxa de consultoria"/></div>
                        </div>

                        <div class="row">
                            <div class="checkbox" style="padding-left: 15px">
                                <label><input type="checkbox" value="" id="chkCadastroFundosInativarFundo" name="chkCadastroFundosInativarFundo">Inativar Fundo</label>
                            </div>  
                        </div>

                        <div class="row">
                            <div id="dvBtnRemoverArquivoRegulamento" style="float: left">
                                <div class="fileUpload btn btn-info btn-sm">
                                    <span>Anexar Regulamento...</span>
                                    <input type="file" id="upAnexarRegulamento" class="upload" />
                                </div>
                            </div>

                            <div id="dvBtnRemoverArquivoContratoGestao" style="padding-left: 10px; float: left">
                                <div class="fileUpload btn btn-info btn-sm">
                                    <span>Anexar Contrato Gestão...</span>
                                    <input type="file" id="upAnexarContratoGestao" class="upload" />
                                </div>
                            </div>

                            <div id="dvBtnRemoverArquivoContratoCustodia" style="padding-left: 10px; float: left">
                                <div class="fileUpload btn btn-info btn-sm">
                                    <span>Anexar Contrato Custódia...</span>
                                    <input type="file" id="upAnexarContratoCustodia" class="upload" />
                                </div>
                            </div>  
                        </div>
                    </div>
                    <div class="col-md-2" style="padding-top: 100px"> 
                        <div class="col-md-12">
                        </div>       
                        <div class="col-md-12" style="padding-bottom: 5px">
                            <input type="button" id="btnCadastroFundosLimpar" value="Limpar" onclick="return LimparDadosCadastroFundo();" class="btn btn-warning  btn-block" />
                        </div>
                        <div class="col-md-12">
                            <input type="button" id="btnCadastroFundosInserir" value="Atualizar" onclick="return btnCadastroFundoAtualizarADM_Click();" style="height: 70px" class="btn btn-primary btn-block btn-lg" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Container da Grid -->
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-1 col-xs-1"></div>
                    <div class="col-md-11 col-xs-11">
                        <table id="gridCadastroFundos" class="table-condensed table-striped"></table>
                        <div id="pgCadastroFundos"></div>
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
