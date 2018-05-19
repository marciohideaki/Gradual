<%@ Page Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="FluxoAprovacaoFundos.aspx.cs" Inherits="Gradual.FIDC.Adm.Web.CadastroFundos.FluxoAprovacaoFundos" %>
<asp:Content ID="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="innerLR">
        <div class="formulario box-generic">            
            
            <div class="container-fluid">    
                <input type="hidden" id="hidIdFundoCadastro"/>    
                <input type="hidden" id="hidUltimaEtapaFinalizada"/>
                <input type="hidden" id="FluxoAprovacaoFundo"/>                  
                <div class="row" style="height: 80px">
                    <div class="col-md-3" style="width: 260px; padding-top: 30px">
                        <label>Fundo Selecionado:</label>
                    </div>
                    <div class="col-md-9" style="padding-top:20px">
                        <select disabled="disabled" id="listaSelecionarFundo" class="form-control" onchange="fnCarregarFluxoFundoSelecionado(this)" style="width: 470px">
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
                        <label>Regulamento / Instrumento de constituição</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_1">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_2">
                    <div class="col-md-3" id="dvgrupo_2" style="width: 290px; padding-top: 30px">
                        <label>Opinião Legal</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_2">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_3">
                    <div class="col-md-3" id="dvgrupo_3" style="width: 290px; padding-top: 15px">
                        <label>CNPJ</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_3">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_4">
                    <div class="col-md-3" id="dvgrupo_4" style="width: 290px; padding-top: 30px">
                        <label>Petição CVM</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_4">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_5">
                    <div class="col-md-3" id="dvgrupo_5" style="width: 290px; padding-top: 30px">
                        <label>Monitoramento de rating</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_5">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_6">
                    <div class="col-md-3" id="dvgrupo_6" style="width: 290px; padding-top: 30px">
                        <label>Contrato de custódia</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_6">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_7">
                    <div class="col-md-3" id="dvgrupo_7" style="width: 290px; padding-top: 30px">
                        <label>Cadastro do fundo na custódia</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_7">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_8">
                    <div class="col-md-3" id="dvgrupo_8" style="width: 290px; padding-top: 30px">
                        <label>Abertura de conta no banco cobrador</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_8">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_9">
                    <div class="col-md-3" id="dvgrupo_9" style="width: 290px; padding-top: 30px">
                        <label>Abertura de conta no banco custodiante</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_9">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_10">
                    <div class="col-md-3" id="dvgrupo_10" style="width: 290px; padding-top: 10px">
                        <label>Contrato de cessão / Termo de cessão</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_10">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_11">
                    <div class="col-md-3" id="dvgrupo_11" style="width: 290px; padding-top: 30px">
                        <label>Contrato de consultoria</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_11">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_12">
                    <div class="col-md-3" id="dvgrupo_12" style="width: 290px; padding-top: 30px">
                        <label>Cadastramento no portal do custodiante</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_12">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_13">
                    <div class="col-md-3" id="dvgrupo_13" style="width: 290px; padding-top: 30px">
                        <label>Cadastramento dos cotistas</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_13">
                        </div>
                    </div>
                </div>
                <div class="separator s1 s1"></div>
                <div class="row" id="dvLinhaGrupo_14">
                    <div class="col-md-3" id="dvgrupo_14" style="width: 290px; padding-top: 30px">
                        <label>Conta SELIC</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_14">
                        </div>
                    </div>
                </div>
                <div class="separator s1"></div>
                <div class="row" id="dvLinhaGrupo_15">
                    <div class="col-md-3" id="dvgrupo_15" style="width: 290px; padding-top: 30px">
                        <label>Conta CETIP</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_15">
                        </div>
                    </div>
                </div>
                <div class="separator s1"></div>
                <div class="row" id="dvLinhaGrupo_16">
                    <div class="col-md-3" id="dvgrupo_16" style="width: 290px; padding-top: 10px">
                        <label>ISIN</label>
                    </div>
                    <div class="col-md-9">
                        <div class="row" id="dvLinhaEtapasGrupo_16">
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
