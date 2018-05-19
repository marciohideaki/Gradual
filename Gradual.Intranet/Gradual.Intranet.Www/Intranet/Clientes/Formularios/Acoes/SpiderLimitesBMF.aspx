<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpiderLimitesBMF.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Formularios.Acoes.SpiderLimitesBMF" %>
<form id="form1" runat="server">
<h4>Configurar Limites BM&F</h4>
    <div>
        <div style="text-align:center">
        <button id="btnClientes_Atualizar_Dados_Limites_BMF"  runat="server" style="text-align:center" onclick="return btnClientes_Atualizar_Dados_Spider_Limites_BMF_Click(this)">Atualizar dados de limites Bm&F</button>
        </div>
        <br />
        <div style="z-index:1">
        <p  >
            <label for="cmbCliente_Spider_Indices_Bmf" style="width:85px;" class="MesmaLinha">Consultar:</label>
            <select id="cmbCliente_Spider_Indices_Bmf" size="5"  style="width: 28em;" class="MesmaLinha" onclick ="return cmbCliente_Spider_Indices_Bmf_OnChange(this)">
                <asp:Repeater id="rptClientes_Contratos" runat="server">
                    <ItemTemplate>
                        <option value='<%# Eval("Id") %>'><%# Eval("Descricao")%></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>

        </p>
        </div>
        <div class="pnlCliente_Limites_BMF_Painel">
            <p >
                <label for="txtCliente_Spider_Qtde_Compra" >Qtde Compra: </label>
                <input type="text"  runat="server" id="txtCliente_Spider_Qtde_Compra" class="validate[required,length[1,15],custom[onlyNumber]] ProibirLetras" />
            </p>

             <p>
                <label for="txtCliente_Spider_Qtde_Venda" >Qtde Venda: </label>
                <input type="text"   runat="server" id="txtCliente_Spider_Qtde_Venda" class="validate[required,length[1,15],custom[onlyNumber]] ProibirLetras" />
            </p>

            <p>
                <label for="txtCliente_Spider_Data_validade">Dt. Validade: </label>
                <input type="text"   runat="server" id="txtCliente_Spider_Data_validade" class="Mascara_Data Picker_Data validate[required,custom[data]]" style="float:left; width:5em" />
            </p>                 
            <p>
                <label for="txtCliente_Spider_Instrumento">Instrumento: </label>
                <input type="text"   runat="server" id="txtCliente_Spider_Instrumento" class="validate[required,length[3,15]]" style="text-transform:uppercase" />
                <button type="button"  id="btnIncluirInstumentos"  onclick="return btnClientes_Spider_Limites_BMF_Adiciona_Instrumento_Click(this)">-->></button>
            </p>
            <p>
                <label for="txtCliente_Spider_Qtde_Venda_Instrumento">Qtde Venda Ins: </label>
                    <input type="text"   runat="server" id="txtCliente_Spider_Qtde_Venda_Instrumento" class="validate[required,length[1,15],custom[onlyNumber]] ProibirLetras"  />
            </p>
            
            <p>
                <label for="txtCliente_Spider_Qtde_Compra_Instrumento">Qtde Compra Ins: </label>
                    <input type="text"   runat="server" id="txtCliente_Spider_Qtde_Compra_Instrumento" class="validate[required,length[1,15],custom[onlyNumber]] ProibirLetras"  />
            </p>  

            <p>
                <%--<label for="txtCliente_Qtde_Ordem_Venda2">Qtde Venda: </label>
                    <input type="text"   runat="server" id="txtCliente_Qtde_Ordem_Venda2" class="validate[required,length[3,15],custom[onlyNumber]] ProibirLetras"  />--%>
                    <label for="txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento" >Qtde Máxima Ordem: </label>
                <input type="text"   runat="server" id="txtCliente_Spider_Qtde_Maxima_Ordem_Instrumento" class="validate[required,length[1,15],custom[onlyNumber]] ProibirLetras"  />
            </p>  
           </div>
           <br />
           <br />
           
           
        <div class="pnlCliente_Limites_BMF_Painel pnlCliente_Limites_BMF_Painel_direito">
            <p>
            <label for="txtCliente_Spider_Qtde_Maxima_Ordem" >Qtde Máxima Ordem: </label>
                <input type="text"   runat="server" id="txtCliente_Spider_Qtde_Maxima_Ordem_Contrato" class="validate[required,length[1,15],custom[onlyNumber]] ProibirLetras" />
            </p>
            <br /><br /><br /><br /><br /><br />
            <%--<p>
                
            </p>
            <p>
                
            </p>--%>
            <p style="height:85px">
                <select id="cmbClientes_Spider_Instrumentos_Selecionados" size="5"  style="width: 25.75em;" onClick="return cmbCliente_Spider_Instrumentos_Bmf_OnChange(this)">
                    <%--<asp:Repeater id="rptClientes_Instrumentos_Selecionadados" runat="server">
                        <ItemTemplate>
                            <option value='<%# Eval("Id") %>'><%# Eval("Descricao")%></option>
                        </ItemTemplate>
                    </asp:Repeater>--%>
                </select>
                <br /><br />
                <br /><br />
            <%--</p>
            <p>
            </p>
            <p>
            </p>--%>
            <p>
                <%--<button id="btnClientes_Limites_BMF_Remover"  runat="server" style="text-align:center" onclick="return btnClientes_Limites_BMF_Remove_Instrumento_Click(this)">Remover</button>--%>
            </p>
        </div>
        <p class="BotoesSubmit" style="position:relative; top: 5em" >
            <button id="btnClientes_Spider_Limites_BMF_Salvar"  runat="server" style="text-align:center" onclick="return btnClientes_Salvar_Spider_Limites_BMF_Click(this)">Salvar Limites BMF</button>
        </p>
    </div>
</form>
