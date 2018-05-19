<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="FluxoAlteracaoRegulamento.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.FluxoAlteracaoRegulamento" %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        <div class="formulario box-generic">            
            
            <div class="container-fluid">    
                <input type="hidden" id="hidIdFundoCadastro"/>    
                <input type="hidden" id="hidUltimaEtapaFinalizada"/> 
                <input type="hidden" id="FluxoAltReg"/> 
                <div class="row" style="height: 80px">
                    <div class="col-md-3" style="width: 260px; padding-top: 30px">
                        <label>Fundo Selecionado:</label>
                    </div>
                    <div class="col-md-9" style="padding-top:20px">
                        <select disabled="disabled" id="listaSelecionarFundo" class="form-control" onchange="fnCarregarFluxoFundoSelecionadoAlteracaoRegulamento(this)" style="width: 470px">
                            <option value="0">Selecione um Fundo</option>
                        </select>
                    </div>
                </div>
                <div class="row" id="dvLinhaLegendas">
                    <div class="col-md-3"  style="width: 280px; padding-top: 30px">
                        
                    </div>
                    <div class="col-md-9">
                        <div class="row">
                            <div class="col-md-4" style="width: 285px">
                            </div>
                            <div class="col-md-2" style="width: 200px">
                                <label>Status</label>
                            </div>
                            <div class="col-md-2" style="width: 130px">
                                <label>Início</label>
                            </div>
                            <div class="col-md-2" style="width: 130px">
                                <label>Finalizado</label>
                            </div>
                            <div class="col-md-2" style="width: 180px">
                                <label>Analista</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="dvLinhaGrupo_1">
                    <div class="col-md-3" id="dvgrupo_1" style="width: 290px; padding-top: 30px">
                        <label>Alteração de Regulamentos</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_1">
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
            padding: 5px 0px 5px 0px;
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
        
    </style>

</asp:Content>
