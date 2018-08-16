<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="R006.aspx.cs"
    Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R006" %>

<form id="form1" runat="server">
<div class="CabecalhoRelatorio RelatorioImpressao">
<link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
    <h1>
        Relatório de <span>Clientes que Efetuaram Suitability</span></h1>
    <img src="../Skin/<%= this.SkinEmUso %>/Img/GradIntra_Relatorio_Titulo_FundoGradual.png" />
    <h2>
        Retirado em <span>
            <%= string.Format("{0} às {1}", DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm")) %></span></h2>
</div>
<table cellspacing="0" class="GridRelatorio">
    <thead>
        <tr>
            <td style="text-align: center; width: 6em">
                ID Cliente
            </td>
            <td style="text-align: left; width: 6em">
                Bovespa
            </td>
            <td style="text-align: left; width: 20%">
                Nome
            </td>
            <td style="text-align: left; width: auto">
                Cpf/Cnpj
            </td>
            <td style="text-align: left; width: 6em">
                Assessor
            </td>
            <td style="text-align: center; width: auto">
                Data da Realização
            </td>
            <td style="text-align: left; width: auto">
                Status
            </td>
            <td style="text-align: left; width: auto">
                Resultado
            </td>
            <td style="text-align: left; width: auto">
                Local Realizado
            </td>
            <td style="text-align: left; width: 6em">
                Peso
            </td>
            <td style="text-align: left; width: 6em">
                Respostas
            </td>
            <td style="text-align: left; width: 6em">
                Realiz. p/ Cliente
            </td>
            <td style="text-align: left; width: 6em">
                Ciência
            </td>
        </tr>
    </thead>
    <tfoot>
        <tr>
            <td colspan="14">
                &nbsp;
            </td>
        </tr>
    </tfoot>
    <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:center">  <%# Eval("Id") %>                          </td>
                <td style="text-align:center">  <%# Eval("CodigoBovespa") %>               </td>
                <td style="text-align:left">    <%# Eval("Nome") %>                        </td>
                <td style="text-align:left">    <%# Eval("CpfCnpj") %>                     </td>
                <td style="text-align:center">  <%# Eval("Assessor") %>                    </td>
                <td style="text-align:center">  <%# Eval("UltimaAlteracaoSuitability") %>  </td>
                <td style="text-align:left">    <%# Eval("Status") %>                      </td>
                <td style="text-align:left">    <%# Eval("ResultadoDaAnalise") %>          </td>
                <td style="text-align:left">    <%# Eval("Local") %>                       </td>
                <td style="text-align:center">  <%# Eval("Peso") %>                        </td>
                <td style="text-align:center">  <%# Eval("Respostas") %>                   </td>
                <td style="text-align:center">  <%# Eval("RealizadoPeloCliente") %>        </td>
                <td style="text-align:center">  <%# Eval("ArquivoCienciaLink") %>          </td>
             </tr>

            </itemtemplate>
            </asp:repeater>
            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="14">
                    Nenhum item encontrado.
                </td>
            </tr>
            <tr id="rowLinhaCarregandoMais" class="NenhumItem" runat="server" visible="false">
                <td colspan="14">
                    Carregando dados, favor aguardar...
                </td>
            </tr>
    </tbody>
</table>
</form>
