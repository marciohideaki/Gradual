<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="GerenciadorPlataformas.aspx.cs" Inherits="Gradual.Spider.Www.GerenciadorPlataformas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
    Gerenciador de Plataformas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">
<div id="page-content" class="telaGerenciadorPlataforma">
    <div id="page-breadcrumb">
        <a title="Dashboard" href="/">
            <i class="glyph-icon icon-dashboard"></i>
            Dashboard
        </a>
        <span>
            <i class="glyph-icon icon-linecons-cog"></i>
            Session Management
        </span>
        <div id="pnlMensagem"  ><span></span></div>
    </div>

    <h3 class="mainTitle page-title">Gerenciador de Plataformas</h3>
    <div class="mais"><input type="button" value="Novo" id="Novo" class="btn btn-primary" data-toggle='modal' onclick="Plataforma_Gerenciador_AbrirModal()" /></div>
    
    <div class="alert alert-success content-box" style="display:none"><p> MENSAGEM </p></div>

    <div class="row">
        <div class="col-md-12">
            <div class="content-box tabs ui-tabs ui-widget ui-widget-content ui-corner-all">
                    
                <div class="bg-primary">
                    <ul id="tabs" class="nav nav-tabs ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
                        <li class="ui-state-default ui-corner-top active"><a href="#operadorConteudo" data-toggle="tab">Operador</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#clienteConteudo" data-toggle="tab">Cliente</a></li>
                        <li class="ui-state-default ui-corner-top"><a href="#assessorConteudo" data-toggle="tab">Assessor</a></li>
                    </ul>
                </div>
                <div id="tabsConteudo" class="tab-content">
                    <div class="tab-pane fade page-box active in" id="operadorConteudo">
                        <table class="table table-striped table-bordered table-condensed">
                                <thead>
                                    <tr>
                                        <th>Operador</th><th>Plataformas</th><th>Sessão</th><th></th>
                                    </tr>
                                </thead>
                                <tbody class="Body_Plataforma_Operador">
                                    <%--
                                    <tr>
                                        <td>186</td><td>ATG</td><td></td><td><a href="#" data-toggle="modal" onclick="return Plataforma_Gerenciador_AbrirModal()">Detalhes</a></td>
                                    </tr>
                                    --%>
                                </tbody>
                            </table>
                    </div>

                    <div class="tab-pane page-box fade" id="clienteConteudo">
                        <table class="table table-striped table-bordered table-condensed">
                            <thead>
                                <tr>
                                    <th>Cliente</th><th>Plataformas</th><th>Sessão</th><th></th>
                                </tr>
                            </thead>
                            <tbody class="Body_Plataforma_Cliente">
                                <%--
                                <tr>
                                    <td>186</td><td>ATG</td><td></td><td><a href="#" data-toggle="modal" data-target="#myModal">Detalhes</a></td>
                                </tr>
                                --%>
                            </tbody>
                        </table>
                    </div>

                    <div class="tab-pane page-box fade" id="assessorConteudo">
                        <table class="table table-striped table-bordered table-condensed">
                            <thead>
                                <tr>
                                    <th>Assessor</th><th>Plataformas</th><th>Sessão</th><th></th>
                                </tr>
                            </thead>
                            <tbody class="Body_Plataforma_Assessor">
                                <%--
                                <tr>
                                    <td>186</td><td>ATG</td><td></td><td><a href="#" data-toggle="modal" data-target="#myModal">Detalhes</a></td>
                                </tr>
                                --%>
                            </tbody>
                        </table>
                    </div>

                </div>

            </div>
        </div>
    </div>


    <% // Modal  %>
<div class="modal fade" id="ModalGerenciadorPlataforma" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 class="modal-title">Gerenciador de Plataformas</h4>
            </div>
            <div class="modal-body">
                <form  id="gerenciadorPlataforma" class="form-vertical">
                    <div class="row">            
                        <div class="col-md-6 AbaPlataformaOperador">
                            <div class="form-group">
                                <label class="control-label" for="operador">Operador</label>
                                <select class="form-control Plataforma_Form_Id_Operador" data-parsley-trigger="change" id="operador" name="operador">
                                    <option value="">[Selecione]</option>
                                    <asp:Repeater ID="rptOperador" runat="server">
                                        <ItemTemplate>
                                            <option value="<%# Eval("Id") %>"><%# Eval("Id") %> - <%# Eval("Nome")%></option>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </select>
                            </div>
                        </div>   
                        <div class="col-md-6 AbaPlataformaCliente" style="display:none">
                            <div class="form-group">
                                <label class="control-label" for="operador">Cliente</label>
                                <input type="text" id="Text1" class="form-control col-sm-5 Plataforma_Form_Id_Cliente" />
                                <%--<select class="form-control Plataforma_Form_Id_Cliente" data-parsley-trigger="change" id="Select1" name="operador">
                                    <option>[Selecione]</option>
                                    <asp:Repeater ID="rptCliente" runat="server">
                                        <ItemTemplate>
                                            <option value="<%# Eval("id_Cliente") %>"><%# Eval("ds_Cliente") %></option>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </select>--%>
                            </div>
                        </div>
                        <div class="col-md-6 AbaPlataformaAssessor" style="display:none">
                            <div class="form-group">
                                <label class="control-label" for="operador">Assessor</label>
                                <input type="text" id="Text2" class="form-control col-sm-5 Plataforma_Form_Id_Assessor">
                                <%--<select class="form-control Plataforma_Form_Id_Assessor" data-parsley-trigger="change" id="Select2" name="operador">
                                    <option>[Selecione]</option>
                                    <asp:Repeater ID="rptAssessor" runat="server">
                                        <ItemTemplate>
                                            <option value="<%# Eval("id_assessor") %>"><%# Eval("ds_assessor") %></option>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </select>--%>
                            </div>
                        </div>
                         <div class="col-md-2 ">
                            <div class="form-group">
                                <label class="control-label" for="operador">Sessao</label>
                                <%--<input type="text" id="Text3" class="form-control col-sm-5 Plataforma_Form_Id_Sessao" />--%>
                                
                                <select class="form-control Plataforma_Form_Id_Sessao" data-parsley-trigger="change" id="Select2" name="operador">
                                    <option value="0">[Selecione]</option>
                                    <asp:Repeater ID="rptSessoes" runat="server">
                                        <ItemTemplate>
                                            <option value="<%# Eval("CodigoSessao") %>"><%# Eval("NomeSessao") %></option>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </select>

                            </div>
                        </div>
                    </div>

                    <div class="row">
                    <asp:Repeater ID="rptPlataformas" runat="server">
                        <ItemTemplate>
                            <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked" value="<%# Eval("CodigoPlataforma") %>" class="Plataforma_Form_Id_Plataforma" ><%# Eval("NomePlataforma") %></label>
                            </div>
                        </div>
                        </ItemTemplate>
                    </asp:Repeater>            
                        <%--<div class="col-md-1">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked">GTI</label>
                            </div>
                        </div>       
                        <div class="col-md-1">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked">ATG</label>
                            </div>
                        </div>             
                        <div class="col-md-1">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked">BLK</label>
                            </div>
                        </div>             
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked">Flexscan</label>
                            </div>
                        </div>             
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked">Bloomberg</label>
                            </div>
                        </div>             
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked">Care Order</label>
                            </div>
                        </div>             
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label" >
                                <input type="checkbox" checked="checked">Profit Chart</label>
                            </div>
                        </div> --%>        

                    </div>
                    
                    <%--<div class="row">            
                        <div class="col-md-3">
                            <div class="form-group desconto">
                                <label class="control-label" for="operador">% desconto no ponto </label>
                                <input type="text" name="desconto" class="form-control descontoInput" />
                            </div>
                        </div>  
                        <div class="col-md-2">
                            <div class="form-group data">
                                <input type="text" name="desconto" class="form-control " placeholder="dd/mm/aaaa" />
                                <i class="glyph-icon icon-calendar datepicker"></i>
                            </div>
                        </div>            
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="control-label" for="operador">
                                <input type="checkbox" name="desconto" class="descontoInput" /> Ignorar cobrança</label>
                            </div>
                        </div>            
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="control-label" for="operador">
                                <input type="checkbox" name="desconto" class="descontoInput" /> Debitar Sinacor</label>
                            </div>
                        </div>   
                    </div>--%>
                    
                </form>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" onclick="return Plataforma_Gerenciador_Salvar_Click(this)">Salvar</button>
            </div>
        </div>
    </div>
</div>



</div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
     <script>
         function Page_Load() {
             Plataforma_Listar_Gerenciador_Plataforma();
         }
    </script>

</asp:Content>



