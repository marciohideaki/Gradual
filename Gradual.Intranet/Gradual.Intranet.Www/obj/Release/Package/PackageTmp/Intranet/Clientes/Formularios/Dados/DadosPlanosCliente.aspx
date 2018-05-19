<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DadosPlanosCliente.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados.DadosPlanosCliente" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cliente - Produtos</title>
</head>
<body>
    <form id="form1" runat="server">
    
        <div id="pnlCliente_Produtos" style="display: none;"></div>

        <h4>Produtos contratados do Cliente</h4>

        <h5 style="margin-bottom: 2em"></h5>

        <div runat="server" id="divCliente_Produtos_ClienteDesde" style="margin-top: 15px; width: auto; display: none;">
            <h5>Dados de cobrança do plano Direct</h5>
        
            <p style="width: 166px; margin: -10px 0pt 0pt 34px;">
                <label style="text-align: left; width: 78px;">Data de adesão</label>
                <label style="width: auto; float: right;"> - <asp:Label runat="server" ID="lblClienteProdutosDataDeAdesao"> </asp:Label></label>
            </p>
            <p style="width: 98%; margin: -5px 0pt 0pt 0px;">
                <label style="width: 125px;">Último vencimento</label>
                <label style="width: auto;"> - <asp:Label runat="server" ID="lblClienteProdutosUltimoVencimento"> </asp:Label></label>
            </p>
        </div>

        <div class="pnlCliente_Produtos_Contratados" style="padding-top: 20px; float: left; width: 100%;">
            <h5 style="margin-bottom: 0px;">Selecione abaixo os produtos do cliente</h5>
            
            <asp:Repeater id="rptCliente_Produtos" runat="server" OnItemDataBound="rptCliente_Produtos_ItemDataBound">
                <ItemTemplate>
                    <p style="margin: auto 0px auto 20px; width: 70%;">
                        <a href="#" onclick="return btnGradIntra_Cliente_Produto_Detalhes_Click(this);" IdProduto='<%#Eval("IdProduto")%>' style="float: left; margin-top: 4px;" title="Detalhes do plano">[ + ]</a>
                        <label id="lblCliente_Produtos" style="text-align:left" runat="server" idProduto='<%#Eval("IdProduto")%>'><%#Eval("DsProduto")%></label>
                        <input id="chkCliente_Produtos" type="checkbox" runat="server" onclick="return chkGradIntra_Clientes_Produtos_DadosPlanosCliente_Click(this);"  />
                    </p>
                    <div id="divCliente_Produtos_Detalhes_<%#Eval("IdProduto")%>" style="display: none; float:left;">
                        <table class="GridRelatorio" style="margin-left: 4em;">
                            <thead>
                                <tr>
                                    <td>Data de adesão</td>
                                    <td>Data da cobrança </td>
                                    <td>Valor cobrado (R$)</td>
                                    <td>Data início do plano</td>
                                    <td>Data fim do plano</td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td align="center"><label  runat="server" ID="txtDataAdesao">N/A</label></td>
                                    <td align="center"><label  runat="server" ID="txtDataCobranca">N/A</label></td>
                                    <td><label  runat="server" ID="txtValorCobrado">N/A</label></td>
                                    <td align="center"><label  runat="server" ID="txtDataInicio">N/A</label></td>
                                    <td align="center"><label  runat="server" ID="txtDataFim">N/A</label></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Repeater id="rptCliente_ProdutoPoupeDirect" runat="server" OnItemDataBound="rptCliente_Produtos_ItemDataBound">
                <ItemTemplate>
                    <p style="margin: auto 0px auto 20px; width: 70%;">
                        <a href="#" onclick="return btnGradIntra_Cliente_Produto_Detalhes_Click(this);" IdProduto="Poupe<%#Eval("IdProduto")%>" style="float: left; margin-top: 4px;" title="Detalhes do plano">[ + ]</a>
                        <label id="lblCliente_Produtos" style="text-align:left" IdProduto='<%# DataBinder.Eval(Container.DataItem, "IdProduto", "Poupe{0}")%>' runat="server"><%#Eval("DsProduto")%></label>
                        <input id="chkCliente_Produtos" type="checkbox" runat="server" checked="checked" onclick="return chkGradIntra_Clientes_Produtos_DadosPlanosCliente_Click(this);"  />
                    </p>
                    <div id="divCliente_Produtos_Detalhes_Poupe<%#Eval("IdProduto")%>" style="display: none; float:left;">
                        <table class="GridRelatorio" style="margin-left: 4em;">
                            <thead>
                                <tr>
                                    <td>Ativo</td>
                                    <td>Data da solicitação </td>
                                    <td>Data Vencimento</td>
                                    <td>Data Retro Troca Ativo</td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td align="center"><label  runat="server" ID="txtDataAdesao"><%# Eval("Ativo")%></label></td>
                                    <td align="center"><label  runat="server" ID="txtDataCobranca"><%# Eval("DataSolicitacao")%></label></td>
                                    <td><label  runat="server" ID="txtValorCobrado"><%# Eval("DataVencimento")%></label></td>
                                    <td align="center"><label  runat="server" ID="txtDataInicio"><%# Eval("DataTrocaAtivo") %></label></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <p style="width: 50%;" class="BotoesSubmit">
                <button runat="server" id="btnCliente_Produtos" onclick="return btnClientes_Produtos_Click(this)">Salvar Dados</button>
            </p>
        </div>
        <div id="pnlCliente_Fundos" style="display:block">
            <h4>Termos aderidos dos fundos do Cliente</h4>
            
            <p style="width: 600px;">
                <label style="text-align: left; width: 200px;">Selecione o Fundo para aderir o termo:</label>
                <select id="cboFundoTermo" >
                <asp:Repeater ID="rptFundos" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("CodigoFundo") %>'><%# Eval("NomeFundo") %> </option>
                    </ItemTemplate>
                </asp:Repeater>
                </select>
                 <button runat="server" id="btnCliente_AderirTermo" onclick="return btnClientes_Adesao_Termo_Fundos_Click(this)">Aderir Termo</button>
            </p>
            
            <table class="GridRelatorio" style="margin-left: 1em">
                <thead>
                    <tr>
                        <td>Codigo Anbima   </td>
                        <td>Fundo           </td>
                        <td>Usuário Logado  </td>
                        <td>Origem          </td>
                        <td>Data adesão     </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater id="rptTermoFundoAderido" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="left"><%# Eval("CodigoFundoAnbima")%></td>
                                <td align="left"><%# Eval("NomeFundo")%>        </td>
                                <td align="left"><%# Eval("UsuarioLogado")%>  </td>
                                <td align="left"><%# Eval("Origem")%>         </td>
                                <td align="center"><%# Eval("DataAdesao")%>     </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

        </div>
    </form>
</body>
</html>
