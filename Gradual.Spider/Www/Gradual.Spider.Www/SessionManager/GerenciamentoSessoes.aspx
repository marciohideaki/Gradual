<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="GerenciamentoSessoes.aspx.cs" Inherits="Gradual.Spider.Www.GerenciamentoSessoes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">
<div id="page-content" class="telaGerenciamentoSessoes">    
    <div id="page-breadcrumb">
        <a title="Dashboard" href="/">
            <i class="glyph-icon icon-dashboard"></i>
            Dashboard
        </a>
        <span>
            <i class="glyph-icon icon-desktop"></i>
            Session Manager
        </span>
    </div>

    <h3 class="mainTitle page-title">Gerenciamento de Sessões</h3>

    <div class="mais"><input type="button" value="Novo" id="NovoCadastro" class="btn btn-primary" data-toggle='modal' data-target='#myModal'/></div>

    <div class="row">
        <div class="col-md-12">
            <table class="table table-striped table-bordered table-condensed">
                <thead>
                    <tr>
                        <th>Plataforma</th><th>Sessão Cliente</th><th>Sessão Repassador</th><th>Sessão Mesa</th><th>Ativa</th><th>Dt. Últ. Evento</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th><a href="#" data-toggle="modal" data-target="#myModal">ATG</a></th>
                        <td>CGRA302</td>
                        <td>CGRA316</td>
                        <td>CGRA802</td>
                        <td><input type="checkbox" name="" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo"></td>
                                         
                        <td>13/10/2014</td>
                    </tr>
                    
                    <tr>
                        <th><a href="#" data-toggle="modal" data-target="#myModal">BLK</a></th>
                        <td>CGRA302</td>
                        <td>CGRA316</td>
                        <td>CGRA802</td>
                        <td><input type="checkbox" name="" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo"></td>
                                         
                        <td>13/10/2014</td>
                    </tr>
                    
                    <tr>
                        <th><a href="#" data-toggle="modal" data-target="#myModal">FLEXSCAN</a></th>
                        <td>CGRA302</td>
                        <td>CGRA316</td>
                        <td>CGRA802</td>
                        <td><input type="checkbox" name="" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo"></td>
                                         
                        <td>13/10/2014</td>
                    </tr>                 
        
                </tbody>
            </table>
                         
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
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
                        <select class="form-control col-sm-6" data-parsley-trigger="change" id="Select1" name="plataforma">
                            <option>Plataforma 1</option>
                            <option>Plataforma 2</option>
                            <option>Plataforma 3</option>
                        </select>
                    </div>
                </div>   
                <div class="col-md-2">
                    <input type="checkbox" name="switchPlataforma" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>
            </div>

            <div class="row">            
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="sessaoCliente">Sessão Cliente</label>
                        <select class="form-control col-sm-6" data-parsley-trigger="change"  id="Select2" name="sessaoCliente">
                            <option>Sessão</option>
                            <option>Sessão</option>
                            <option>Sessão</option>
                        </select>
                    </div>
                </div>   
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label" for="sessaoClienteValor">R$ Valor</label>
                        <input type="text" id="Text1" class="form-control col-sm-7 sessaoClienteValor">
                    </div>
                </div>
                
                <div class="col-md-2">
                    <input type="checkbox" name="switchCliente" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>

            </div>
            
            <div class="row">    
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="sessaoRepassador">Sessão Repassador</label>
                        <select class="form-control col-sm-6" data-parsley-trigger="change"  id="Select3" name="sessaoRepassador">
                            <option>Sessão</option>
                            <option>Sessão</option>
                            <option>Sessão</option>
                        </select>
                    </div>
                </div>
                  
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label" for="sessaoRepassadorValor">R$ Valor</label>
                            <input type="text" id="Text2" class="form-control col-sm-7 sessaoRepassadorValor">
                    </div>
                </div>
                
                <div class="col-md-2">
                    <input type="checkbox" name="switchRepassador" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>

            </div>


            <div class="row">    
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label col-sm-4" for="sessaoMesa">Sessão Mesa</label>
                        <select class="form-control col-sm-6" data-parsley-trigger="change"  id="Select4" name="sessaoMesa">
                            <option>Sessão</option>
                            <option>Sessão</option>
                            <option>Sessão</option>
                        </select>
                    </div>
                </div>
                  
                <div class="col-md-4">
                    <div class="form-group">
                        <label class="col-sm-3 control-label" for="sessaoMesaValor">R$ Valor</label>
                            <input type="text" id="Text3" class="form-control col-sm-7 sessaoMesaValor">
                    </div>
                </div>
                
                <div class="col-md-2">
                    <input type="checkbox" name="switchCliente" class="input-switch" checked="checked" data-size="normal" data-on-text="Ativo" data-off-text="Inativo">
                </div>

            </div>
                </form>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary">Salvar</button>
            </div>
        </div>
    </div>
</div>

</div>

    


    

    <% // Modal  %>



</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>
