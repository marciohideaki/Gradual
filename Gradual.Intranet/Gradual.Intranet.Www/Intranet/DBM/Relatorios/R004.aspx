<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R004.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.DBM.Relatorios.R004" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao RelatorioDBMImpressao">
    <h1>Relatório de <span>LTV do Cliente</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em
        <span><%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span>
    </h2>

    <p style="margin-left: 12px; width: 75%;">
        <label>Cliente:</label>
        <label><%= gNomeCliente %></label>
    </p>

    <div runat="server" id="divLTVDoCliente_Detalhes" visible="false">
        <table class="GridRelatorio" style="margin-left: 10px; width: 50%;">
            <thead>
                <tr>
                    <td style="text-align: left;">Mês de ocorrência</td>
                    <td style="text-align: center;">Corretagem no período (R$)</td>
                    <td style="text-align: center;">Volume no período (R$)</td>
                </tr>
            </thead>
            <tbody>
                <asp:repeater runat="server" id="rptLTVDoCliente_Detalhes">
                    <itemtemplate>
                        <tr>            
                            <td style="text-align: left;"> <%# Eval("Mes")%></td>
                            <td style="text-align: right;"><%# Eval("Corretagem")%></td>
                            <td style="text-align: right;"><%# Eval("Volume")%></td>
                        </tr>
                    </itemtemplate>
                </asp:repeater>
            </tbody>
        </table>
    </div>

    <h2 style="margin-top:25px;"><span runat="server" id="lblLTVDoCliente_Detalhes" visible="false">Nenhum registro encontrado.</span></h2>
    
</div>

</form>
<script language="javascript" type="text/javascript">DefinirCoresValores_Load();</script>