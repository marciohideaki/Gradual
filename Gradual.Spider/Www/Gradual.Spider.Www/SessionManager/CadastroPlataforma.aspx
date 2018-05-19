<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="CadastroPlataforma.aspx.cs" Inherits="Gradual.Spider.Www.CadastroPlataforma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
    
    Cadastro de Plataformas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">
    <div id="page-content" class="telaCadastroPlataforma">
    <div id="page-breadcrumb">
        <a title="Dashboard" href="/">
            <i class="glyph-icon icon-dashboard"></i>
            Dashboard
        </a>
        <span>
            <i class="glyph-icon icon-linecons-cog"></i>
            Session Management
        </span>
        <div id="pnlMensagem" ><span></span></div>
    </div>

    <h3 class="mainTitle page-title">Cadastro de Plataformas</h3>
    <div class="mais"><input type="button" value="Novo" id="NovoCadastro" class="btn btn-primary" data-toggle='modal' onclick="javascript: Plataforma_AbrirModal()"  /></div>

    <div class="alert alert-success content-box" style="display:none"><p> MENSAGEM </p></div>

    <div class="row">
        <div class="col-md-12">
            <table class="table table-striped table-bordered table-condensed">
                <thead>
                    <tr>
                        <th>Plataforma</th><th>Sessão Cliente</th><th>Sessão Repassador</th><th>Sessão Mesa</th><th>Dt. Últ. Evento</th>
                    </tr>
                </thead>
                <tbody class="Body_Plataforma_Sessao">
                    
                    <%--<asp:Repeater ID="rptListaPlataforma" runat="server">
                        <ItemTemplate>
                            <tr>
                                <th><a href="#" data-toggle="modal" onclick='Plataforma_AbrirModal(<%# Eval("CodigoPlataforma") %>)'><%#Eval("NomePlataforma")%></a></th>
                                <td><%# Eval("NomeSessaoCliente") %></td>
                                <td><%# Eval("NomeSessaoRepassador") %></td>
                                <td><%# Eval("NomeSessaoMesa") %></td>
                                <td><input type="checkbox" name="" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo"   ></td>
                                         
                                <td><%# Eval("DataAtualizacao")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>--%>
                    
                    <%--<tr>
                        <th><a href="#" data-toggle="modal" data-target="#myModal">BLK</a></th>
                        <td>CGRA302</td>
                        <td>CGRA316</td>
                        <td>CGRA802</td>
                        <td><input type="checkbox" name="" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo"></td>
                                         
                        <td>13/10/2014</td>
                    </tr>--%>

                </tbody>
            </table>
                         
        </div>
    </div>

    <% // Modal  %>
<div class="modal fade" id="ModalPlataforma" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 class="modal-title">Cadastro de Plataformas</h4>
            </div>
            <div class="modal-body">
                <form class="form-vertical">
                    
                    <div class="row">            
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="plataforma">Plataforma</label>
                        <select class="form-control col-sm-6 Plataforma_Form_id" data-parsley-trigger="change" id="Select1" name="plataforma">
                            <option value="">[Selecione]</option>
                            <asp:Repeater ID="rptPlataforma" runat="server">
                                <ItemTemplate>
                                    <option value="<%#Eval("CodigoPlataforma") %>"><%#Eval("NomePlataforma") %></option>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%--<option>Plataforma 1</option>
                            <option>Plataforma 2</option>
                            <option>Plataforma 3</option>--%>
                        </select>
                    </div>
                </div>   
                <%--<div class="col-md-2">
                    <input type="checkbox" name="switchPlataforma" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>--%>
            </div>

            <div class="row">            
                <div class="col-md-6">
                    <div class="form-group">
                        <input id="Plataforma_Form_id_sessao_plataforma_cliente" type="hidden" />
                        <label class="control-label col-sm-4" for="sessaoCliente">Sessão Cliente</label>
                        <select class="form-control col-sm-6 Plataforma_Form_id_sessao_cliente" data-parsley-trigger="change"  id="Select2" name="sessaoCliente">
                            <option value="">[Selecione]</option>
                            <asp:Repeater ID="rptSessaoCliente" runat="server">
                            <ItemTemplate>
                                <option value="<%# Eval("CodigoSessao") %>"><%# Eval("NomeSessao") %> </option>
                            </ItemTemplate>
                            </asp:Repeater>
                        </select>
                    </div>
                </div>   
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label" for="sessaoClienteValor">R$ Valor</label>
                        <input type="text" id="Text1" class="form-control col-sm-7 Plataforma_Form_vl_sessao_cliente">
                    </div>
                </div>
                
                <div class="col-md-2">
                    <input type="checkbox" name="switchCliente" class="input-switch Plataforma_Form_ativo_sessao_cliente" checked="checked"  data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>

            </div>
            
            <div class="row">    
                <div class="col-md-6">
                    <div class="form-group">
                        <input id="Plataforma_Form_id_sessao_plataforma_repassador" type="hidden" />
                        <label class="control-label col-sm-4" for="sessaoRepassador">Sessão Repassador</label>
                        <select class="form-control col-sm-6 Plataforma_Form_id_sessao_repassador" data-parsley-trigger="change"  id="Select3" name="sessaoRepassador">
                            <option value="">[Selecione]</option>
                            <asp:Repeater ID="rptSessaoRepassador" runat="server">
                                <ItemTemplate>
                                    <option value="<%# Eval("CodigoSessao") %>"><%# Eval("NomeSessao")%></option>
                                </ItemTemplate>
                            </asp:Repeater>
                            
                        </select>
                    </div>
                </div>
                  
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label" for="sessaoRepassadorValor">R$ Valor</label>
                            <input type="text" id="Text2" class="form-control col-sm-7 Plataforma_Form_vl_sessao_repassador">
                    </div>
                </div>
                
                <div class="col-md-2">
                    <input type="checkbox" name="switchRepassador" class="input-switch Plataforma_Form_ativo_sessao_repassador" checked="checked"  data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>

            </div>


            <div class="row">    
                <div class="col-md-6">
                    <div class="form-group">
                        <input id="Plataforma_Form_id_sessao_plataforma_mesa" type="hidden" />
                        <label class="control-label col-sm-4" for="sessaoMesa">Sessão Mesa</label>
                        <select class="form-control col-sm-6 Plataforma_Form_id_sessao_mesa" data-parsley-trigger="change"  id="Select4" name="sessaoMesa">
                            <option value="">[Selecione]</option>
                            <asp:Repeater ID="rptSessaoMesa" runat="server">
                            <ItemTemplate>
                                <option value="<%# Eval("CodigoSessao") %>"><%# Eval("NomeSessao")%></option>
                            </ItemTemplate>
                            </asp:Repeater>
                        </select>
                    </div>
                </div>
                  
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label" for="sessaoMesaValor">R$ Valor</label>
                            <input type="text" id="Text3" class="form-control col-sm-7 Plataforma_Form_vl_sessao_mesa">
                    </div>
                </div>
                
                <div class="col-md-2">
                    <input type="checkbox" name="switchCliente" class="input-switch Plataforma_Form_ativo_sessao_mesa" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>

            </div>
                </form>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" onclick="return Plataforma_Salvar_Click(this)">Salvar</button>
            </div>
        </div>
    </div>
</div>


</div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= this.RaizDoSite %>/Resc/Skin/Supina/assets-minified/widgets/input-switch/inputswitch.js"></script>
    <script>

        function Page_Load() 
        {
            $('.input-switch').bootstrapSwitch();
            //$('.input-switch')..checkboxpicker();
            Plataforma_Listar_Plataforma_Sessao();
        }
    </script>

</asp:Content>
