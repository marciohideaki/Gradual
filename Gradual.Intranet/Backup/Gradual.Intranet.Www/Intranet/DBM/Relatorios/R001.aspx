<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R001.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.R001" %>

<form id="form1" runat="server">

<div class="CabecalhoRelatorio RelatorioImpressao">

    <h1>Resumo da <span>Praça</span></h1>

    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />

    <h2>Retirado em <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>

</div>

<table class="GridRelatorio" style="margin-left:15px; width: 40%;">
    <thead>
        <tr>
            <td style="text-align: left; width: 6em">
                       
            </td>
            <td style="text-align: left; width: 12em">
                Corretagem
            </td>
            <td style="text-align: left; width: 7em">
                Volume
            </td>
            <td style="text-align: center; width: 6em">
                Cadastrados
            </td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="text-align: left; width: 6em">
                No Mês (Atual)
            </td>
            <td style="text-align: left; width: 12em">
                <input type="text" id="txtDBM_ResumoDoCliente_Tipo" value="<%= CorretagemMes %>" disabled="disabled" />
            </td>
            <td style="text-align: left; width: 7em">
                <input type="text" id="Text1" value="<%= VolumeMes %>" disabled="disabled" />
            </td>
            <td style="text-align: center; width: 6em">
                <input type="text" id="Text2" value="<%= CadastroMes %>" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left; width: 6em">
                Mês Anterior 
            </td>
            <td style="text-align: left; width: 12em">
                <input type="text" id="Text3" value="<%= CorretagemMesAnt %>" disabled="disabled" />
            </td>
            <td style="text-align: left; width: 7em">
                <input type="text" id="Text4" value="<%= VolumeMesAnt %>" disabled="disabled" />
            </td>
            <td style="text-align: center; width: 6em">
                <input type="text" id="Text5" value="<%= CadastroMesAnt %>" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left; width: 6em">
                Média no Período
            </td>
            <td style="text-align: left; width: 12em">
                <input type="text" id="Text6" value="<%= CorretagemPorPeriodo %>" disabled="disabled" />
            </td>
            <td style="text-align: left; width: 7em">
                <input type="text" id="Text7" value="<%= VolumePorPeriodo %>" disabled="disabled" />
            </td>
            <td style="text-align: center; width: 6em">
                <input type="text" id="Text8" value="<%= CadastroPorPeriodo %>" disabled="disabled" />
            </td>
        </tr>
    </tbody>
</table>

<h3 style="margin-left:20px;">Clientes</h3>
<table class="GridRelatorio" style="margin-left:15px; width: 40%;">
    <thead>
        <tr>
            <td>Total</td>
            <td></td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>% Operou</td>
            <td><input type="text" id="Text9" value="<%= PorcentOperou %>" disabled="disabled" /></td>
        </tr>
        <tr>
            <td>% Com Custódia</td>
            <td><input type="text" id="Text10" value="<%= PorcentCustodia %>" disabled="disabled" /></td>
        </tr>
        <tr>
            <td>% Não Opera 90 dias</td>
            <td><input type="text" id="Text11" value="<%= PorcentNaoOperou %>" disabled="disabled" /></td>
        </tr>
        <tr>
            <td>Corretagem Média</td>
            <td><input type="text" id="Text12" value="<%= MediaCorretagem %>" disabled="disabled" /></td>
        </tr>
        <tr>
            <td>Custódia Média</td>
            <td><input type="text" id="Text13" value="<%= MediaCustodia %>" disabled="disabled" /></td>
        </tr>
    </tbody>
</table>

<h3 style="margin-left:20px;">Top5 Clientes </h3>
            
<table class="GridRelatorio" style="margin-left:15px; width: 40%;">
<thead>
    <tr>
        <td>Nome</td>
        <td>Corretagem</td>
        <td>Volume</td>
    </tr>
</thead>
<tbody>
    <asp:repeater id="rptClienteTop5" runat="server">
        <ItemTemplate>
            <tr>
                <td style="text-align:left"> <%# Eval("Nome")%>          </td>
                <td style="text-align:right"><%# Eval("Corretagem")%></td>
                <td style="text-align:right"><%# Eval("Volume")%>        </td>
                            
                            
            </tr>
        </ItemTemplate>
      </asp:repeater> 
<tfoot>
    <tr>
       <td style="text-align:right" colspan="4"><span>Total em porcentagem: </span> <input type="text" id="Text14" value="<%= PorcentagemTop5 %>" disabled="disabled" /> </td>
    </tr>
</tfoot>   
</table>
            
<h3 style="margin-left:20px;">Top10 Clientes</h3>
            
<table class="GridRelatorio" style="margin-left:15px; width: 40%;">
<thead>
    <tr>
        <td>Nome</td>
        <td>Corretagem</td>
        <td>Volume</td>
    </tr>
    </thead>
    <tbody>
    <asp:repeater id="rptClienteTop10" runat="server">
        <ItemTemplate>
        <tr>
            <td style="text-align:left"> <%# Eval("Nome")%>          </td>
            <td style="text-align:right"><%# Eval("Corretagem")%></td>
            <td style="text-align:right"><%# Eval("Volume")%>        </td>
                            
                            
        </tr>
        </ItemTemplate>
    </asp:repeater> 
    </tbody>  
    <tfoot>
    <tr>
        <td style="text-align:right" colspan="4"><span>Total em porcentagem: </span> <input type="text" id="Text17" value="<%= PorcentagemTop10 %>" disabled="disabled" /> </td>
        </tr>
    </tfoot>   
</table> 

<h3 style="margin-left:20px;">Top20 Clientes</h3>
<table class="GridRelatorio" style="margin-left:15px; width: 40%;">
<thead>
    <tr>
        <td>Nome</td>
        <td>Corretagem</td>
        <td>Volume</td>
    </tr>
    </thead>
    <tbody>
    <asp:repeater id="rptClienteTop20" runat="server">
        <ItemTemplate>
        <tr>
            <td style="text-align:left"> <%# Eval("Nome")%>          </td>
            <td style="text-align:right"><%# Eval("Corretagem")%></td>
            <td style="text-align:right"><%# Eval("Volume")%>        </td>
                            
                            
        </tr>
        </ItemTemplate>
    </asp:repeater> 
    </tbody>   
    <tfoot>
    <tr>
        <td style="text-align:right" colspan="4"><span>Total em porcentagem: </span> <input type="text" id="Text15" value="<%= PorcentagemTop20 %>" disabled="disabled" /> </td>
        </tr>
    </tfoot> 
</table>
            
<h3 style="margin-left:20px;">Breakdown Assessor</h3>
<table class="GridRelatorio" style="margin-left:15px; width: 70%;">
<thead>
    <tr>
        <td style="text-align:left">Tipo</td>
        <td style="text-align:left">Nome</td>
        <td style="text-align:right">Código Assessor</td>
        <td style="text-align:right">Volume</td>
        <td style="text-align:right">Corretagem</td>
        <td style="text-align:right">Custódia</td>
    </tr>
</thead>
<tbody>
    <asp:repeater id="rptBreakdownAssessor" runat="server">
        <ItemTemplate>
        <tr>
                <td style="text-align:left"> <%# Eval("Tipo")%>          </td>
                <td style="text-align:left"> <%# Eval("Nome")%>          </td>
                <td style="text-align:right"><%# Eval("CodigoAssessor")%></td>
                <td style="text-align:right"><%# Eval("Volume")%>        </td>
                <td style="text-align:right"><%# Eval("Corretagem")%>    </td>
                <td style="text-align:right"><%# Eval("Custodia")%>      </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</tbody>
</table>

</form>
