<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacoesRelatorio.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.Fundos.OperacoesRelatorio" %>
<h4>Relatório de Movimentações Financeiras</h4>
                                    
<p>Acompanhe o relatório de todas as suas aplicações ou resgates já realizados em Fundos de Investimento com a Gradual.</p>
                                    <input type="hidden" id="Posicao_Aba_Simular_Selecionada" runat="server" />
<div class="row">
    <div class="form-padrao clear">
        <div class="col5">
            <div class="campo-consulta">
                <label>Produto:</label>
                <asp:DropDownList runat="server" id="ddlFiltroFundos"></asp:DropDownList>
            </div>
        </div>
                                            
        <div class="col5">
            <div class="campo-consulta">
                <label>Data entre:</label>
                <asp:TextBox runat="server" ID="txtDataDe" CssClass="calendario"  MaxLength="10" />
            </div>
        </div>
                                            
        <div class="col5">
            <div class="campo-consulta">
                <label></label>
                <asp:TextBox runat="server" ID="txtDataAte" CssClass="calendario"  MaxLength="10" />
            </div>
        </div>
                                            
        <div class="col5">
            <div class="campo-consulta">
                <label>Horário entre</label>
                <asp:TextBox runat="server" ID="txtHoraDe" Text="00:00" />
            </div>
        </div>
                                            
        <div class="col5">
            <div class="campo-consulta">
                <label></label>
                <asp:TextBox runat="server" ID="txtHoraAte" Text="23:59" />
            </div>
        </div>
    </div>
</div>
                                    
<h5 class="noMarginBot">Status</h5>

<div class="row">
    <div class="col5">
        <div class="lista-checkbox">
            <label class="checkbox"><asp:CheckBox id="chkSolicitado" runat="server" Text="Solicitado" /></label>
        </div>
    </div>
                                        
    <div class="col5">
        <div class="lista-checkbox">
            <label class="checkbox"><asp:CheckBox id="chkEmProcessamento" runat="server" Text="Em Processamento" /> </label>
        </div>
    </div>
                                        
    <div class="col5">
        <div class="lista-checkbox">
            <label class="checkbox"><asp:CheckBox id="chkCancelado" runat="server" Text="Cancelado" /></label>
        </div>
    </div>
                                        
    <div class="col5">
        <div class="lista-checkbox">
            <label class="checkbox"><asp:CheckBox id="chkExecutado" runat="server" Text="Executado" /> </label>
        </div>
    </div>
</div>
                                    
                                    
<h5 class="noMarginBot">Tipo</h5>

<div class="row">
    <div class="col5">
        <div class="lista-checkbox">
            <label class="checkbox"><asp:CheckBox id="chkAplicacao" runat="server" Text="Aplicação" /></label>
        </div>
    </div>
                                        
    <div class="col5">
        <div class="lista-checkbox">
            <label class="checkbox"><asp:CheckBox id="chkResgate" runat="server" Text="Resgate" /></label>
        </div>
    </div>
</div>
                                    
<div class="row">
    <div class="col1">
        <div class="lista-checkbox">
            <asp:Button id="btnBuscarSolicitacoes" class="botao btn-padrao btn-erica margeando" runat="server" onclick="btnBuscarSolicitacoes_Click" Text="Buscar" OnClientClick="btnRelatoriosFundos_Click(this, 'AbaRelatorios')" style="margin-bottom:20px" />
            
        </div>
    </div>
</div>  
                                    
<div class="row">
    <div class="col1">
        <table class="tabela">
            <tr class="tabela-titulo">
                <td>Fundo</td>
                <td>Saldo Atual</td>
                <td>Vlr. Solic.</td>
                <%--<td>Saldo em cc</td>--%>
                <td>Dt. Solicit.</td>
                <td>Dt. Agendamento</td>
                <td>Operação</td>
                <td>Status</td>
                <td>Detalhes</td>
                <td>Observação</td>
            </tr>
            <asp:Repeater runat="server" ID="rptListaDeSolicitacoes">
            <itemtemplate>
            <tr class="tabela-type-small">
                <td><%#Eval("NomeFundo")%></td>
                <td><%#Eval("SaldoAtual")%></td>
                <td><%#Eval("ValorSolicitado","{0:N2}")%></td>
                <%--<td><%#Eval("SaldoCC")%></td>--%>
                <td><%#Eval("DtHrSolicitacao")%></td>
                <td><%#Eval("DtAgendamento")%></td>
                <td><%#Eval("TipoOperacao")%></td>
                <td><%#Eval("Status")%></td>
                <td><%#Eval("MotivoStatus")%></td>
                <td><%#Eval("Observacoes")%></td>
            </tr>
            </itemtemplate>
            </asp:Repeater>
            <tr id="trNenhumSolicitacoes" runat="server" visible="false">
                <td colspan="9">Nenum item encontrado.</td>
            </tr>                                    
        </table>
    </div>
</div>
<script>
    //$('input[type="checkbox"], input[type="radio"]').iCheck();
    $("#txtHoraDe").mask("00:00");
    $("#txtHoraAte").mask("00:00");
</script>
