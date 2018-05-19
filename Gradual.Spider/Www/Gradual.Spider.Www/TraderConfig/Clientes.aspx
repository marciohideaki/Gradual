<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Gradual.Spider.Www.Clientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
    Clientes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">

<div id="page-content" class="telaClientes">

    <div id="page-breadcrumb">
        <a title="Dashboard" href="/">
            <i class="glyph-icon icon-dashboard"></i>
            Dashboard
        </a>
        <span>
            <i class="glyph-icon icon-linecons-cog"></i>
            Trader Config
        </span>
        
        <div id="pnlMensagem"><span></span></div>
                
    </div>

    <h3 class="mainTitle page-title">Clientes</h3>

    <div class="content-box box-cinza-topo">
        <div class="mais"><input type="button" value="Novo" id="novoCliente" class="btn btn-primary Cliente_Pesquisa_CodigoCliente" onclick="return Cliente_Form_Novo(this)"/></div>
        <div class="content-box-header content-box-header-alt bg-default">
            <ul class="form1Linha">
                <li>
                    <div class="input-group"><span class="input-group-addon"><i class="glyph-icon icon-search"></i></span><input type="text" name="codigo" class="form-control form-element-sm Cliente_Pesquisa_Codigo" placeholder="Código do cliente" /> 
                    </div>
                </li>

                <li>
                    <label class="checkbox-inline"><input type="checkbox" name="source" value="bolsa" class="form-element-sm Cliente_Pesquisa_Bovespa" checked="checked"/>Bolsa</label>
                    <label class="checkbox-inline"><input type="checkbox" name="source" value="bmf" class="form-element-sm Cliente_Pesquisa_Bmf"/>BMF</label>
                    <label class="checkbox-inline"><input type="checkbox" name="source" value="contaMaster" class="form-element-sm Cliente_Pesquisa_Conta_Master"/>Conta Master</label>
                </li>
                <li class="botao"><input type="button" id="buscarCliente" value="Pesquisar" class="btn btn-sm btn-primary Cliente_Pesquisa_Botao_Pesquisar" onclick="return Cliente_Filtro_Seleciona(this)"/></li>
            </ul>
        </div>
    </div>

    <div class="alert alert-success content-box" style="display:none"><p> MENSAGEM </p></div>

    <div id="dadosCliente" class="Cliente_Form_Dados">
        <form runat="server" id="cadastro" class="form-vertical form-border">
        <input type="hidden" id="Cliente_Form_IdLogin" />
            <div class="row">            
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="control-label" for="">Nome: <span class="required">*</span></label>
                        <input class="form-control Cliente_Form_Nome" id="nome" name="nome" required="" type="text">
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="control-label" for="">Email: <span class="required">*</span></label>
                        <input class="form-control Cliente_Form_Email" data-parsley-trigger="change" id="email" name="email" required="" type="text">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class=" control-label" for="">Assessor:</label>
                    
                        <select class="form-control Cliente_Form_Assessor" data-parsley-trigger="change"  id="assessor" name="assessor">
                            <option>[Selecione]</option>
                            <asp:repeater id="rptClienteFormAssessor" runat="server">
                                <itemtemplate>
                                    <option value='<%# Eval("Id") %>'><%# Eval("Id").ToString().PadLeft(4, '0') %> - <%# Eval("Value") %></option>
                                </itemtemplate>
                            </asp:repeater>
                        </select>
                    </div>
                </div>
            </div>

            <div class="row">            
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Cód. Bovespa: <span class="required">*</span></label>
                        <input class="form-control Cliente_Form_CodigoBovespa" data-parsley-trigger="change" id="codBovespa" name="codBovespa" required="" type="text">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Cód. BMF: <span class="required">*</span></label>
                        <input class="form-control Cliente_Form_CodigoBmf" data-parsley-trigger="change" id="codBMF" name="codBMF" required="" type="text">
                    </div>
                </div>
            
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Sessão: <span class="required">*</span></label>
                        <select class="form-control Cliente_Form_Sessao" data-parsley-trigger="change"  id="sessao" name="sessao">
                            <option>[Selecione]</option>
                            <asp:Repeater ID="rptClienteFormSessao" runat="server">
                                <ItemTemplate>
                                    <option value="<%#Eval("key") %>"><%#Eval("value") %></option>
                                </ItemTemplate>
                            </asp:Repeater>
                        </select>
                    </div>
                </div>
            </div>
            <div class="row">
            
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label" for="">Localidade: <span class="required">*</span></label>
                        <select class="form-control Cliente_Form_Localidade" data-parsley-trigger="change" id="localidade" name="localidade">
                            <option>[Selecione]</option>
                            <asp:Repeater ID="rptClienteFormLocalidade" runat="server">
                                <ItemTemplate>
                                    <option value="<%#Eval("key") %>"><%#Eval("value") %></option>
                                </ItemTemplate>
                            </asp:Repeater>
                        </select>
                    
                    </div>
                </div>
                <%--<div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Conta mãe: <span class="required">*</span></label>
                        <input class="form-control" data-parsley-trigger="change" id="contaMae" name="contaMae" required="" type="text">
                    </div>
                </div>--%>
                <div class="col-md-2 right">
                    <div class="form-group">
                        <p>&nbsp;</p>
                        <input type="button" class="btn btn-md btn-primary Cliente_Form_Salvar" data-parsley-trigger="change" id="salvar" name="salvar"  value="Salvar" onclick="return Cliente_Salvar(this)" >
                    </div>
                </div>
            </div>
         
        </form>
        </div>

        <div class="row Risco_Form_Dados" >
            
            <div class="col-md-12">
            
                <div class="content-box tabs ui-tabs ui-widget ui-widget-content ui-corner-all">

                <div class="bg-primary">
                    <ul id="tabs" class="nav nav-tabs ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
                        <li class="ui-state-default ui-corner-top active"><a href="#aba-permissoes" data-toggle="tab">Permissões</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#aba-bovespa" data-toggle="tab">Limites Bovespa</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#aba-bmf" data-toggle="tab">Limite BMF</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#aba-restricoes" data-toggle="tab">Restrições</a></li>
                    </ul>
                </div>

            
                <div id="tabsConteudo" class="tab-content">

                    <div class="tab-pane page-box fade active in" id="aba-permissoes">
                        
                        <div class="row">
                            <div class="col-md-11 pad20A Cliente_Risco_Form_Dados">
                                <asp:Repeater ID="Cliente_Risco_Permissao" runat="server">
                                    <ItemTemplate>
                                        <div class="checkbox">
                                            <label for="OMS"><input data-parsley-trigger="change" id='Cliente_Risco_Permissao_<%# Eval("CodigoPermissao") %>' name="permissoes" type="checkbox"   value='<%# Eval("CodigoPermissao") %>'><%# Eval("NomePermissao")%></label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>


                                <div class="right">
                                    <input class="btn btn-md btn-primary Cliente_Risco_Permissao_Salvar" data-parsley-trigger="change" id="atualizar" name="salvar" type="button" value="Atualizar" onclick="return Cliente_Risco_Permissao_Salvar_Click(this)">
                                </div>
                                

                            </div>
                        </div>
                    </div>
                    
                    <div class="tab-pane  page-box fade" id="aba-bmf">
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table table-striped table-bordered table-condensed">
                                        <thead>
                                            <tr>
                                                <th style="width:15%;">Contrato</th><th style="width:15%;">Intrumento</th><th style="width:15%;">Compra</th><th style="width:15%;">Venda</th><th style="width:15%;">Finger</th><th style="width:15%">Máximo</th><th style="width:25%;">Vencimento</th>
                                            </tr>
                                        </thead>
                                        <%--<tbody class="Cliente_Risco_LimiteBmf_Lista">
                                            <tr>
                                                <td>
                                                <select class="form-control Cliente_Risco_LimiteBmf_Contrato" data-parsley-trigger="change"  id="contrato" name="contrato">
                                                <asp:Repeater ID="rptCommodities" runat="server" class="rptCommodities">
                                                    <ItemTemplate>
                                                        <option value='<%#Eval("key") %>'><%#Eval("value") %></option>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                    
                                                </select>
                                                </td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Serie"      data-parsley-trigger="change" id="Text5"    name="compra"   type="text"></td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Compra" data-parsley-trigger="change" id="compra"    name="compra"   type="text"></td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Venda"  data-parsley-trigger="change" id="venda"     name="venda"    type="text"></td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Finger" data-parsley-trigger="change" id="finger"    name="finger"   type="text"></td>
                                         
                                                <td><div class="input-group input-group-sm">
                                                    <span class="add-on input-group-addon"><i class="glyph-icon icon-calendar"></i></span> 
                                                    <input type="text" data-date-format="mm/dd/yy"  class="bootstrap-datepicker form-control">
                                                    </div>
                                                </td>
                                            </tr>
                                     
        
                                        </tbody>--%>
                                        <tbody class="Cliente_Risco_LimiteBmf_Lista">
                                            <%--<tr>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Serie"  data-parsley-trigger="change" id="Text6"    name="compra"   type="text"></td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Serie"  data-parsley-trigger="change" id="Text5"    name="compra"   type="text"></td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Compra" data-parsley-trigger="change" id="compra"    name="compra"   type="text"></td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Venda"  data-parsley-trigger="change" id="venda"     name="venda"    type="text"></td>
                                                <td><input class="form-control Cliente_Risco_LimiteBmf_Finger" data-parsley-trigger="change" id="finger"    name="finger"   type="text"></td>
                                                <td><div class="input-group input-group-sm"><span class="add-on input-group-addon"><i class="glyph-icon icon-calendar"></i></span><input type="text" data-date-format="mm/dd/yy"  class="bootstrap-datepicker form-control"></div></td>
                                            </tr>--%>
                                        </tbody>
                                    </table>
                         
                                </div>
                        </div>
                        
                        <%--
                        <div class="row">
                            <div class="col-md-12">
                                <br />
                                <p class="right">
                                    <input class="btn btn-default mais" data-parsley-trigger="change" id="mais" name="mais" type="button" value="Incluir">&nbsp;
                                    <input class="btn btn-md btn-primary" data-parsley-trigger="change" id="SalvarBMF" name="SalvarBMF" type="button" value="Salvar">
                                </p>
                            </div>
                        </div>
                        --%>
                    </div>
                    
                    <div class="tab-pane page-box fade" id="aba-bovespa">
                        <form id="Form1" class="form-horizontal" data-parsley-validate="">
                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-5  pad20A">
                                    <div class="content-box">
                                        <h3 class="content-box-header bg-blue-alt"><i class="glyph-icon icon-bars"></i> A Vista</h3>

                                        <div class="content-box-wrapper">

                                            <div class="form-group">
                                                <label class="col-sm-3 control-label" for="avistaCompra">Compra</label>
                                                <div class="col-sm-8">
                                                    <input type="text" id="avistaCompra" class="form-control Cliente_Risco_LimiteBov_Compra_AVista">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label" for="avistaVeda">Venda</label>
                                                <div class="col-sm-8">
                                                    <input type="text" id="avistaVenda" class="form-control Cliente_Risco_LimiteBov_Venda_AVista">
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                                <div class="col-md-5  pad20A">

                                    <div class="content-box">
                                        <h3 class="content-box-header bg-green"><i class="glyph-icon icon-bars"></i> Opções</h3>

                                        <div class="content-box-wrapper">
                                            
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label" for="avistaCompra">Compra</label>
                                                <div class="col-sm-8">
                                                    <input type="text" id="Text1" class="form-control Cliente_Risco_LimiteBov_Compra_Opcoes">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label" for="avistaVeda">Venda</label>
                                                <div class="col-sm-8">
                                                    <input type="text" id="Text2" class="form-control Cliente_Risco_LimiteBov_Venda_Opcoes">
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-4">
                                    <div class="row">
                                    
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label" for="avistaCompra">Data</label>
                                            <div class="col-sm-8">

                                                <div class="input-group input-group-sm">
                                                        <span class="add-on input-group-addon"><i class="glyph-icon icon-calendar"></i></span> 
                                                        <input type="text" data-date-format="mm/dd/yy" id="Text3" class="bootstrap-datepicker form-control Cliente_Risco_FatFinger_DataValidade">
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label" for="avistaVeda">Fat Finger</label>
                                            <div class="col-sm-8">
                                                <input type="text" id="Text4" class="form-control Cliente_Risco_FatFinger_Valor">
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>

                            </div>
                            <%--
                            <div class="row">
                                <div class="col-md-11 right">
                                    <input class="btn btn-md btn-primary" data-parsley-trigger="change" id="Button2" name="salvar" type="button" value="Atualizar" click="return Cliente_Risco_LimiteBov_Salvar_Click(this)">
                                </div>
                            </div>
                            --%>
                    
                        </form>
                    </div>

                    <div class="tab-pane page-box fade" id="aba-restricoes">
                        restricoes
                    </div>

                </div>
            </div>
        </div>

            
    </div>
    </div>
    <%--<div id="warn">Cliente não encontrado!</div>--%>
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        function insertTr() {
            $(".bootstrap-datepicker").datepicker("destroy");

            var tr = $("#aba-bmf tbody tr:last").clone(),
                tbody = $("#aba-bmf table tbody"),
                inputs = tr.find('input');

            inputs.each(function (i) {
                var that = this;
               
                $(that).removeAttr('id').val("");
            });

            tbody.append(tr);
            $("#aba-bmf table .bootstrap-datepicker").datepicker({ dateFormat: "dd/mm/yy" });
    
        }

        $(document).ready(function () {
            
            $(".bootstrap-datepicker").datepicker({ dateFormat: "dd/mm/yy" });

            //            $("#novoCliente").on('click', function () {
            //                $("#warn").fadeOut('fast', function () {
            //                    $("#dadosCliente").fadeIn('fast');
            //                });
            //            });

            $("#buscarCliente").on('click', function () {
                $("#dadosCliente").fadeOut('fast', function () {
                    $("#warn").fadeIn('fast');
                });
            });

            $("#mais").on('click', function () {
                insertTr();
            });

            $(".Risco_Form_Dados").hide();
        });

    </script>
</asp:Content>
