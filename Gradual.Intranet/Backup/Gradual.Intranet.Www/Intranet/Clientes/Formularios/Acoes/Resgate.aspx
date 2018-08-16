<%@ Page Language="C#" enableviewstate="false" AutoEventWireup="true" CodeBehind="Resgate.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.Resgate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gradual Investimentos - Intranet</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divResgate_Principal" name="divResgate_Principal">
            <input type="hidden" id="hidClientes_Conta_ListaJson" class="ListaJson" runat="server" value="" />
            <input type="hidden" id="hidDadosCompletos_PF" class="DadosJson" runat="server" value="" />
            <input type="hidden" id="hidAssessor" runat="server" value="" />
            <input type="hidden" id="hidUsuarioLogado" runat="server" value="" />
            <input type="hidden" id="hidConta" runat="server" />
            <p>
                <label for="txtResgate_NomeCliente">NOME:</label>
                <input id="txtResgate_NomeCliente" Propriedade="NomeCliente" maxlength="60" class="validate[required,length[5,60]]" style="width: 30em;" disabled/>
            </p>

            <p>
                <label for="txtResgate_CPF">CPF:</label>
                <input id="txtResgate_CPF" Propriedade="CPF_CNPJ" maxlength="15" class="validate[funcCall[validatecpf]] ProibirLetras" disabled/>
            </p>

            <p>
                <label for="cboResgate_Conta">CC:</label>
                <select id="cboResgate_Conta" Propriedade="Conta" class="Lista">
                    <asp:Repeater id="rptResgate_Contas" runat='server'>
                        <ItemTemplate>
                            <option value='<%# string.Concat(Eval("Banco"),";",Eval("Agencia"),";",Eval("AgenciaDigito"),";",Eval("ContaCorrente"),";",Eval("ContaDigito"))%>'><%# string.Concat(Eval("Banco"), " - ", Eval("BancoNome"), " - Ag.", Eval("Agencia"), "-", Eval("AgenciaDigito"), " / ", Eval("tipoConta"), " ", Eval("ContaCorrente"), "-", Eval("ContaDigito"))%></option>
                        </ItemTemplate>
                    </asp:Repeater>        
                </select>
            </p>

            <p>
                <label for="txtResgate_Data">DATA:</label>
                <input id="txtResgate_Data" value="<%= string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy")) %>" maxlength="10" style="width: 10em;" disabled/>
            </p>

            <p>
                <label for="txtResgate_Saldo">SALDO DISP.:</label>
                <input id="txtResgate_Saldo" value="<%= this.SaldoDeConta.Acoes_SaldoD0.ToString("n") %>" maxlength="10" style="width: 10em;" disabled/> 
            </p>

            <p>
                <label for="txtResgate_SaldoProjetado">SALDO PROJETADO.:</label>
                <input id="txtResgate_SaldoProjetado" alt="Saldo D0 + Saldo D1 + Saldo D2 + Saldo D3" value="<%= (this.SaldoDeConta.Acoes_SaldoD0 + this.SaldoDeConta.Acoes_SaldoD1 + this.SaldoDeConta.Acoes_SaldoD2 + this.SaldoDeConta.Acoes_SaldoD3).ToString("n") %>" maxlength="10" style="width: 10em;" disabled/> 
            </p>

            <p>
                <label for="txtResgate_Valor">VALOR DA RETIRADA:</label>
                <input id="txtResgate_Valor" type="text" maxlength="10" style="width: 15em;" />
            </p>
            
            <p>
                <button class="" onclick="return ResgateConfirmar_Click(this)" id="btnResgate_Confirmar">CONFIRMAR</button>
                <button class="" onclick="return RetornarTaxa_Click(55475)" id="Button1">Teste</button>
            </p>
        </div>
    </form>
</body>
</html>

