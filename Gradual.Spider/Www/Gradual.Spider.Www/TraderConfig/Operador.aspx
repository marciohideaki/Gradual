<%@ Page Title="" Language="C#" MasterPageFile="~/Spider.Master" AutoEventWireup="true" CodeBehind="Operador.aspx.cs" Inherits="Gradual.Spider.Www.CadastroOperador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerTitle" runat="server">
    Operador
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Main" runat="server">

<div id="page-content" class="telaOperador">
    <div id="page-breadcrumb">
        <a title="Dashboard" href="/">
            <i class="glyph-icon icon-dashboard"></i>
            Dashboard
        </a>
        <span>
            <i class="glyph-icon icon-linecons-cog"></i>
            Trader Config
        </span>

        <div id="pnlMensagem" ><span></span></div>
    </div>

    <h3 class="mainTitle page-title">Operador</h3>

    <div class="content-box box-cinza-topo box-topo-operador">
        <div class="mais"><input type="button" value="Novo" id="NovoCliente" class="btn btn-primary"  onclick="javascript: Operador_AbrirModal();" /></div>
        <div class="content-box-header content-box-header-alt bg-default">
            <ul class="form1Linha">
                <li>
                    <label>Nome do operador: </label> <br />
                    <input type="text"  class="form-control form-element-sm Operador_Filtro_NomeOperador" placeholder="Nome" /> 
                </li>
                <li>    
                    <label>Sessão</label><br />
                        <select name="sessao" class="form-control form-element-sm Operador_Filtro_Sessao">
                            <option>[Selecione]</option>
                            <asp:repeater id="rptAssessorFiltro" runat="server">
                                    <itemtemplate>
                                        <option value='<%#Eval("key") %>'><%#Eval("value") %></option>
                                    </itemtemplate>
                                </asp:repeater>
                        </select>
                    </li>

                <li>
                    <label>Localidade</label><br />
                    <select   class="form-control form-element-sm Operador_Filtro_Localidade">
                        <option>[Selecione]</option>
                        <asp:repeater id="rptLocalidadeFiltro" runat="server">
                            <itemtemplate>
                                <option value='<%#Eval("key") %>'><%#Eval("value") %></option>
                            </itemtemplate>
                        </asp:repeater>
                    </select>
                </li>
                <li>
                    <label>Sigla: </label><br /><input type="text" name="nome"  class="form-control form-element-sm Operador_Filtro_Sigla" />  
                </li>
                <li class="botao"><input type="button" id="buscarCliente" value="Buscar" class="btn btn-sm btn-primary Operador_Filtro_BuscaOperador" onclick="return Operador_Filtro_Busca(this)"/></li>
            </ul>
        </div>
    </div>
    
    <div class="alert alert-success content-box" style="display:none"><p> MENSAGEM </p></div>

    <div class="content-box ui-corner-all pad20A">
        <table id="gridOperador" class="table table-condensed table-striped table-hover"></table> 
        <div id="paging"></div>
    </div>

</div>
    
<% // Modal  %>
<div class="modal fade" id="ModalOperador" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog ">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 class="modal-title">Operador</h4>
                <input type="hidden" class="Operador_Form_idLogin" />
            </div>
            <div class="modal-body">
                <form class="form-vertical">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label for="" class="control-label">Nome do Operador</label>
                                <input type="text" required="" name="nome" class="form-control Operador_Form_NomeOperador" />
                            </div>
                        </div>
                    
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="" class="control-label">Código do Operador</label>
                                <input type="text" required="" name="nome" class="form-control Operador_Form_CodigoOperador" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label for="" class="control-label">E-mail</label>
                                <input type="text" required="" name="nome" id="Text2" class="form-control Operador_Form_EmailOperador" />
                            </div>
                        </div>
                    
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="" class="control-label">Assessor</label>
                                <select name="sentido" class="form-control form-element-sm Operador_Form_CodigoAssessor">
                                    <option value="">[ Selecione ]</option>
                                    <asp:repeater id="rptAssessorSinacor" runat="server">
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
                                <label for="" class="control-label">Sigla</label>
                                <input type="text" required="" name="nome" class="form-control Operador_Form_SiglaOperador" />
                            </div>
                        </div>
                    
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="" class="control-label">Sessão</label>
                                <select name="sentido"  class="form-control form-element-sm Operador_Form_SessaoOperador">
                                <option>[Selecione]</option>
                                <asp:repeater id="rptSessaoAssessor" runat="server">
                                    <itemtemplate>
                                        <option value='<%#Eval("key") %>'><%#Eval("value") %></option>
                                    </itemtemplate>
                                </asp:repeater>
                                    
                                </select>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <br /><br />
                            <div class="form-group">
                                <label for="" class="control-label"><input type="checkbox" class="Operador_Form_AcessaPlataforma" />Acessar Plataforma</label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label for="" class="control-label">Assessor(es) Vinculado(s)</label>
                                <input type="text" required="" name="nome" id="Text5" class="form-control Operador_Form_AssessoresVinculados" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="" class="control-label">Localidade</label>
                                <select class="form-control form-element-sm Operador_Form_CodigoLocalidade">
                                <option>[Selecione]</option>
                                <asp:repeater id="rptLocalidade" runat="server">
                                    <itemtemplate>
                                        <option value='<%#Eval("key") %>'><%#Eval("value") %></option>
                                    </itemtemplate>
                                </asp:repeater>
                                </select>
                            </div>
                        </div>
                    </div>

                </form>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" onclick="return Operador_Salvar()">Salvar</button>
            </div>
        </div>
    </div>
</div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script>
    /*
        //Grid aba 1 
        var mydata = {
            "page": "1", "total": 3, "records": "13", "rows": [
                       { id: "1",  operador: "123451", sessao: "123456789123456789", "data": "01/01/2010",sigla: 123, localidade: "vale5"},
                       { id: "2",  operador: "123452",  sessao: "123456789123456789","data": "02/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "3",  operador: "123463",  sessao: "123456789123456789","data": "03/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "4",  operador: "1234564", sessao: "123456789123456789","data": "04/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "5",  operador: "1234565", sessao: "123456789123456789","data": "05/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "6",  operador: "1234566", sessao: "123456789123456789","data": "06/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "7",  operador: "1234567", sessao: "123456789123456789","data": "07/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "8",  operador: "1234568", sessao: "123456789123456789","data": "08/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "9",  operador: "1234569", sessao: "123456789123456789","data": "09/01/2010", sigla: 123, localidade: "vale5"},
                       { id: "10", operador: "1234560", sessao: "123456789123456789","data": "10/01/2010", sigla: 123, localidade: "vale5" },
                       { id: "11", operador: "1234570", sessao: "123456789123456789","data": "11/01/2010", sigla: 123, localidade: "vale5" },
                       { id: "12", operador: "1234572", sessao: "123456789123456789","data": "12/01/2010", sigla: 123, localidade: "vale5" },
                       { id: "13", operador: "1234673", sessao: "123456789123456789","data": "13/01/2010", sigla: 123, localidade: "vale5" }
            ]
        };

        $("#grid").jqGrid({
            datatype: "local"
            , data: mydata.rows
            , height: "100%"
            , jsonReader: {
                repeatitems: false
            }
            , sortable: true
            , autoencode: true
            , loadonce: true
            , viewrecords: true
            , rowNum: 10
            , pager: "#paging"
            , colNames: ['Id', 'Operador', 'Sessão', 'Data', 'Sigla', 'Localidade', ""]
            , colModel: [
                { name: 'id', index: 'id', sorttype: "int", hidden: true }
                , { name: 'operador', index: 'Operador', width: 100, sorttype:"string" }
                , { name: 'sessao', index: 'sessao', width: 150,  sorttype: "float" }
                , { name: 'data', index: 'data', width: 150, align:"center", sorttype: "float" }
                , { name: 'sigla', index: 'sigla', width: 150, align: "right", sorttype: "string" }
                , { name: 'localidade', index: 'localidade', width: 200, align: "center", sorttype: "string" }
                , { name: 'acoes', width: 80 }
            ]
            , afterInsertRow: function (rowid, rowdata, rowelem) {

                var tdAplicar = $("#" + rowid + " td:last");
                var textoAplicar = "detalhes";
                var linkAplicar = "<a href='#' class='font-orange' data-toggle='modal' data-target='#myModal'>" + textoAplicar + " " + rowid + "</a>";

                $(tdAplicar).html(linkAplicar);
            }

        });
        */
    </script>

</asp:Content>
