<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PoupeAplicacaoResgate.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.PoupeDirect.PoupeAplicacaoResgate" %>

<form id="form1" runat="server">

<br />


<div class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_6Linhas" style="width: 80em !important">
<h4>
    Solicitações Poupe Direct para Aplicação e Resgate</h4>


<p style="width: 75em; float: left;">
            <label for="cmbBusca_status_AplicacaoResgate">Status:</label>
            <select id="cmbBusca_Status" propriedade="status" style="width: 268px;">
                <option value="">[Todos]</option>
                <asp:repeater id="rptStatus_FiltroAplicacaoResgate" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("CodigoStatus") %>'><%# Eval("DescStatus")%></option>
                    </ItemTemplate>
                </asp:repeater>
            </select>
</p>

 



<p style="width: 75em;">
    <label for="txtAplicacaoResgate_DataInicial">Data Inicial:</label>
    <input type="text" id="txtAplicacaoResgate_DataInicial" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
</p>
<p style="width: 75em;">
    <label for="txtAplicacaoResgate_DataFinal">Data Final:</label>
    <input type="text" id="txtAplicacaoResgate_DataFinal" value='<%=DateTime.Now.ToString("dd/MM/yyyy") %>' maxlength="10" style="float:left;width: 6em" class="Mascara_Data Picker_Data" />
</p>

<p style="width: 75em; float:left;">

    <label for="txtAplicacaoResgate_CodigoCliente">Código Cliente:</label>
    <input id="txtAplicacaoResgate_CodigoCliente" type="text" propriedade="CodigoCliente" style="width:100px;" />
    
</p>

<p style="float: left; text-align: right; width: 52em;">
    <button id="btnBuscarAprovacao" onclick="return btnBuscar_AplicacaoResgate_Click(this);"> Buscar</button>
</p>

<p style="float: left; text-align: right; width: 54em;">
    <button class="btnBusca" onclick="window.print();return false;" id="btnAplicacaoResgateImprimir" style="width:auto;margin-left:8px;">Imprimir Relatório</button>
</p>

</div>

    
    <div id="pnlConteudo_Aplicacao" class="Conteudo_Container">
            <div  class="pnlFormularioAplicacaoResgate" style="margin-top: 20em; margin-left: -36.5em; display:none;">
            <h4>Aplicação</h4>
                               
                <div id="pnlConteudo_Aplicacao_listaResultado" style="margin-left: 3px; display: none;">
                    <table id="tb_busca_Aplicacao"></table>
                    <div id="pnlBusca_Aplicacao_Resultados_Pager"></div>
                    <div style="text-align:right">
                        <button class="btnBusca" id="btnSolicitacao_AtualizarAplicacao" onclick="return btnSolicitacao_AtualizarAplicacao_Click(this)" style="width: 230px; margin-right: 20px" runat="server" >Aprovar Aplicação</button>
                    </div>
                    
                </div>
            </div>
        </div>

    
    <table class="GridIntranet GridIntranet_CheckSemFundo" style="margin: 0em 3em 1em 3em; width:800px;" cellspacing="0">
    
        <thead>
            
          
        </thead>
    
       
        
</table>



    <div id="pnlConteudo_Resgate" class="Conteudo_Container">
            <div  class="pnlFormularioAplicacaoResgate" style="float: left; margin: 1em 10px 10px; display: none;">
                
                <h4>Resgate</h4>
                <div id="pnlConteudo_Resgate_listaResultado" style="display:none">
                    <table id="tb_busca_Resgate"></table>
                    <div id="pnlBusca_Resgate_Resultados_Pager"></div>
                    <div style="text-align:right">
                        <button class="btnBusca" id="btnSolicitacao_AtualizarResgate" onclick="return btnSolicitacao_AtualizarResgate_Click(this);" style="width: 230px; margin-right: 20px" runat="server">Aprovar Resgate</button>
                    </div>
                    
                </div>
            </div>
        </div>

    <table class="GridIntranet GridIntranet_CheckSemFundo" style="margin: 0em 3em 1em 3em; width:800px;" cellspacing="0">
    
        <thead>
            
          
        </thead>
    </table>
    

<script>btnBuscar_AplicacaoResgate_Click(this);</script>

        
</form>

