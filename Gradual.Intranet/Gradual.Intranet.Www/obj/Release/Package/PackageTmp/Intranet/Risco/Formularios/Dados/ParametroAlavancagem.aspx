<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParametroAlavancagem.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Risco.Formularios.Dados.ParametroAlavancagem" %>

<form id="form1" runat="server">

    <h4>Parâmetro de Alavancagem</h4>
    
    <p style="width:50px; padding-right:20px; margin-top:5px;">
        <label style="float:right;" for="GradIntra_Risco_ItensDoGrupo_Grupos">Grupos</label>
	</p> 

    <p class="ui-widget" style="width:350px;">
        <select class="GradIntra_ComboBox_Pesquisa AtivarAutoComplete" id="cmbGradIntra_Risco_ParametroDeAlavancagem_Grupos">
		    <option value="">[Selecione]</option>
            <asp:repeater id="rptGradIntra_ComboBox_Digitavel" runat="server">
                <itemtemplate>
		            <option value="<%# Eval("Id") %>"><%# Eval("Descricao")%></option>
                </itemtemplate>
            </asp:repeater>
	    </select>
    </p>
    
    <p style="float: left; width: 20%; margin-left: -25px; margin-top: 5px;">
        <button class="" id="btnGradIntra_Risco_ParametroDeAlavancagem_SelecionarGrupo" onclick="return btnGradIntra_Risco_ParametroDeAlavancagem_SelecionarGrupo_Click(this)">Selecionar</button>
    </p>
    
    <div id="divGradIntra_Risco_ParametrosDeAlavancagem_Detalhes" style="width: 100%; margin-top: 100px; display: none;">

        <div style="width: 310px;">
            <h5>Parâmetros do Grupo:</h5>

            <p style="float: left; width: 148px;">
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_CC" style="width: 85px;">Conta Corrente:</label>
                <input type="text" id="txtGradIntraRisco_ParametroDeAlavancagem_CC" dir="rtl" Propriedade="CC" maxlength="6" class="validate[required] ProibirLetras" style="width: 38px;" />
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_CC" style="float: right; padding-left: 5px; width: 5px;">%</label>
            </p>
            <p style="float: left; width: 148px;">
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_Custodia" style="width: 85px;">Custódia:</label>
                <input type="text" id="txtGradIntraRisco_ParametroDeAlavancagem_Custodia" dir="rtl" Propriedade="Custodia" maxlength="6" class="validate[required] ProibirLetras" style="width: 38px;" />
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_Custodia" style="float: right; padding-left: 5px; width: 5px;">%</label>
            </p>
        </div>

        <div style="width: 310px; margin-top: 100px; float: left; position: absolute;">
            <h5>Utilizar Carteira:</h5>

            <p style="float: left; width: 148px;">

                <label for="ckbGradIntraRisco_ParametroDeAlavancagem_Carteira23">Garantia a Prazo (23)</label>
                <input type="checkbox" id="ckbGradIntraRisco_ParametroDeAlavancagem_Carteira23" propedade="Carteira23" />

                <label for="ckbGradIntraRisco_ParametroDeAlavancagem_Carteira27">Carteira de Opção (27)</label>
                <input type="checkbox" id="ckbGradIntraRisco_ParametroDeAlavancagem_Carteira27" propedade="Carteira27" />
            </p>
        </div>
        
        <div style="display: block; float: left; margin-top: -46px; width: 240px; padding-left: 76px;">

            <h5>Percentuais de Alavancagem:</h5>

            <p style="float: left; width: 148px;">
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_CompraAVista" style="width: 85px;">Compra à Vista:</label>
                <input type="text" id="txtGradIntraRisco_ParametroDeAlavancagem_CompraAVista" dir="rtl" Propriedade="Compra à Vista" maxlength="6" class="validate[required] ProibirLetras" style="width: 38px;" />
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_CompraAVista" style="float: right; padding-left: 5px; width: 5px;">%</label>
            </p>

            <p style="float: left; width: 148px;">
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_VendaAVista" style="width: 85px;">Venda à Vista:</label>
                <input type="text" id="txtGradIntraRisco_ParametroDeAlavancagem_VendaAVista" dir="rtl" Propriedade="Venda à Vista" maxlength="6" class="validate[required] ProibirLetras" style="width: 38px;" />
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_VendaAVista" style="float: right; padding-left: 5px; width: 5px;">%</label>
            </p>

            <p style="float: left; width: 148px;">
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_CompraOpcao" style="width: 85px;">Compra Opção:</label>
                <input type="text" id="txtGradIntraRisco_ParametroDeAlavancagem_CompraOpcao" dir="rtl" Propriedade="Compra à Opção" maxlength="6" class="validate[required] ProibirLetras" style="width: 38px;" />
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_CompraOpcao" style="float: right; padding-left: 5px; width: 5px;">%</label>
            </p>

            <p style="float: left; width: 148px;">
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_VendaOpcao" style="width: 85px;">Venda Opção:</label>
                <input type="text" id="txtGradIntraRisco_ParametroDeAlavancagem_VendaOpcao" dir="rtl" Propriedade="Venda à Opção" maxlength="6" class="validate[required] ProibirLetras" style="width: 38px;" />
                <label for="txtGradIntraRisco_ParametroDeAlavancagem_VendaOpcao" style="float: right; padding-left: 5px; width: 5px;">%</label>
            </p>

        </div>
    
        <p style="float: left; width: 20%; margin-top: 20px; margin-left: 415px;">
            <button class="" id="btnGradIntra_Risco_ParametroDeAlavancagem_SalvarParametros" onclick="return GradIntra_Risco_ParametroDeAlavancagem_SalvarParametros_Click(this)">Atualizar</button>
        </p>

    </div>

</form>
