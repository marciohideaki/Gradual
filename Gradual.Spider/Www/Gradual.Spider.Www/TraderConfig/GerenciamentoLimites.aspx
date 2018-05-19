<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="GerenciamentoLimites.aspx.cs" Inherits="Gradual.Spider.Www.GerenciamentoLimites" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">
    
<div id="page-content" class="telaGerenciamentoLimites">    
    <div id="page-breadcrumb">
        <a title="Dashboard" href="/">
            <i class="glyph-icon icon-dashboard"></i>
            Dashboard
        </a>
        <span>
            <i class="glyph-icon icon-list-alt"></i>
            Trader Config
        </span>
    </div>

    <h3 class="mainTitle page-title">Gerenciamento de Limites</h3>


    <div class="content-box box-cinza-topo">
        <div class="mais"><input type="button" value="Novo" id="novoCliente" class="btn btn-primary"/></div>
        <div class="content-box-header content-box-header-alt bg-default">
            <ul class="form1Linha">
                <li>
                    <div class="input-group"><span class="input-group-addon"><i class="glyph-icon icon-search"></i></span><input type="text" name="codigo" class="form-control form-element-sm" placeholder="Código do cliente" /> 
                    </div>
                </li>

                <li>
                    <label class="checkbox-inline"><input type="checkbox" name="source" value="bolsa" class="form-element-sm" checked="checked"/>Bolsa</label>
                    <label class="checkbox-inline"><input type="checkbox" name="source" value="bmf" class="form-element-sm"/>BMF</label>
                    <label class="checkbox-inline"><input type="checkbox" name="source" value="contaMaster" class="form-element-sm"/>Conta Master</label>
                </li>
                <li class="botao"><input type="button" id="buscarCliente" value="Pesquisar" class="btn btn-sm btn-primary"/></li>
            </ul>
        </div>
    </div>

    <div id="dadosCliente">
        <form runat="server" id="cadastro" class="form-vertical form-border">
            <div class="row">            
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="control-label" for="">Nome:</label>
                        <input class="form-control" id="nome" name="nome" type="text">
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <label class="control-label" for="">Email:</label>
                        <input class="form-control" data-parsley-trigger="change" id="email" name="email"  type="text">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class=" control-label" for="">Assessor:</label>
                    
                        <select class="form-control" data-parsley-trigger="change"  id="assessor" name="assessor">
                            <option>250 - Vinculados</option>
                        </select>
                    </div>
                </div>
            </div>

            <div class="row">            
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Cód. Bovespa: </label>
                        <input class="form-control" data-parsley-trigger="change" id="codBovespa" name="codBovespa"  type="text">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Cód. BMF: </label>
                        <input class="form-control" data-parsley-trigger="change" id="codBMF" name="codBMF"  type="text">
                    </div>
                </div>
            
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Sessão: </label>
                        <select class="form-control" data-parsley-trigger="change"  id="sessao" name="sessao">
                            <option>CGRA362 - Home Broker</option>
                        </select>
                    </div>
                </div>
            </div>
            <div class="row">
            
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label" for="">Localidade:</label>
                        <select class="form-control" data-parsley-trigger="change" id="localidade" name="localidade">
                            <option>Matriz - Gradual JK</option>
                            <option>SP</option>
                            <option>RIO</option>
                        </select>
                    
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="control-label" for="">Conta mãe:</label>
                        <input class="form-control" data-parsley-trigger="change" id="contaMae" name="contaMae"  type="text">
                    </div>
                </div>
                <div class="col-md-2 right">
                    <div class="form-group">
                        <p>&nbsp;</p>
                        <input class="btn btn-md btn-primary" data-parsley-trigger="change" id="salvar" name="salvar" type="button" value="Salvar">
                    </div>
                </div>
            </div>
         
        </form>

        <div class="row">
            
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
                            <div class="col-md-11 pad20A">

                                <div class="checkbox">
                                    <label for="OMS"><input data-parsley-trigger="change" id="OMS" name="permissoes" type="checkbox" value="OMS">Operar no Gradual OMS</label>

                                </div>
                                <div class="checkbox">
                                    <label for="aVista"><input data-parsley-trigger="change" id="aVista" name="permissoes" type="checkbox" value="aVista">Operar no mercado à vista</label>
                                </div>
                                <div class="checkbox">
                                    <label for="opcoes"><input data-parsley-trigger="change" id="opcoes" name="permissoes" type="checkbox" value="opcoes">Operar no mercado de opções</label>
                                </div>
                                <div class="checkbox">
                                    <label for="bmf"><input data-parsley-trigger="change" id="bmf" name="permissoes" type="checkbox" value="bmf">Operar no mercado de BMF</label>
                                </div>
                                <div class="checkbox">
                                    <label for="limiteOperacional"><input data-parsley-trigger="change" id="limiteOperacional" name="permissoes" type="checkbox" value="limiteOperacional">Não validar Limite Operacional</label>
                                </div>
                                <div class="checkbox">
                                    <label for="bloquearEnvio"><input data-parsley-trigger="change" id="bloquearEnvio" name="permissoes" type="checkbox" value="bloquearEnvio">Bloquear envio de Ordens Global</label>
                                </div>
                                <div class="right">
                                    <input class="btn btn-md btn-primary" data-parsley-trigger="change" id="atualizar" name="salvar" type="button" value="Atualizar">
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
                                                <th style="width:15%;">Contrato</th><th style="width:15%;">Intrumento</th><th style="width:15%;">Compra</th><th style="width:15%;">Venda</th><th style="width:15%;">Finger</th><th style="width:25%;">Vencimento</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                <select class="form-control" data-parsley-trigger="change"  id="contrato" name="contrato">
                                                    <option>DI1</option>
                                                    <option>DI2</option>
                                                    <option>DI3</option>
                                                </select>
                                                </td>
                                                <td>
                                                <select class="form-control" data-parsley-trigger="change"  id="instrumento" name="instrumento">
                                                    <option>F15</option>
                                                    <option>F16</option>
                                                    <option>F17</option>
                                                </select>
                                                </td>
                                                <td><input class="form-control" data-parsley-trigger="change" id="compra" name="compra" type="text"></td>
                                                <td><input class="form-control" data-parsley-trigger="change" id="venda" name="venda" type="text"></td>
                                                <td><input class="form-control" data-parsley-trigger="change" id="finger" name="finger" type="text"></td>
                                         
                                                <td><div class="input-group input-group-sm">
                                                    <span class="add-on input-group-addon"><i class="glyph-icon icon-calendar"></i></span> 
                                                    <input type="text" data-date-format="mm/dd/yy"  class="bootstrap-datepicker form-control">
                                                    </div>
                                                </td>
                                            </tr>
                                     
        
                                        </tbody>
                                    </table>
                         
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <br />
                                <p class="right">
                                    <input class="btn btn-default mais" data-parsley-trigger="change" id="mais" name="mais" type="button" value="Incluir">&nbsp;
                                    <input class="btn btn-md btn-primary" data-parsley-trigger="change" id="SalvarBMF" name="SalvarBMF" type="button" value="Salvar">

                                </p>
                            </div>

                        </div>
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
                                                    <input type="text" id="avistaCompra" class="form-control">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label" for="avistaVeda">Venda</label>
                                                <div class="col-sm-8">
                                                    <input type="text" id="avistaVenda" class="form-control">
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
                                                    <input type="text" id="Text1" class="form-control">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label" for="avistaVeda">Venda</label>
                                                <div class="col-sm-8">
                                                    <input type="text" id="Text2" class="form-control">
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
                                                        <input type="text" data-date-format="mm/dd/yy" id="Text3" class="bootstrap-datepicker form-control">
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label" for="avistaVeda">Fat Finger</label>
                                            <div class="col-sm-8">
                                                <input type="text" id="Text4" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-11 right">
                                    <input class="btn btn-md btn-primary" data-parsley-trigger="change" id="Button2" name="salvar" type="button" value="Atualizar">
                                </div>
                            </div>
                    
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
    <div id="warn" class="alert alert-danger">Cliente não encontrado!</div>
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

            $("#novoCliente").on('click', function () {
                $("#warn").fadeOut('fast', function () {
                    $("#dadosCliente").fadeIn('fast');
                });
            });

            $("#buscarCliente").on('click', function () {
                $("#dadosCliente").fadeOut('fast', function () {
                    $("#warn").fadeIn('fast');
                });
            });

            $("#mais").on('click', function () {
                insertTr();
            });

        });

    </script>
</asp:Content>
