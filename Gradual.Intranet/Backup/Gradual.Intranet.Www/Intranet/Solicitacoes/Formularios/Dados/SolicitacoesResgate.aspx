<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitacoesResgate.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados.SolicitacoesResgate" %>
<html>
    <head id="Head1" runat="server">
        <title>Gradual Investimentos - Intranet</title>
    </head>
    <body>
        <form id="frmResgate" runat="server">
            <h3 style="margin: 2px; height: 15px">Solicitações de Resgate</h3>

            <div id="divResgate_Filtro" style="float:left; width:30em;">
                <p>
                    <label for="txtResgate_Filtro_Cliente" >Cliente:</label>
                    <input name="txtResgate_Filtro_Cliente" id="txtResgate_Filtro_Cliente" type="text" maxlength="10" style="width: 5em;" />
                </p>
                
                <p>
                    <label for="txtResgate_Filtro_Assessor" >Assessor:</label>
                    <input name="txtResgate_Filtro_Assessor" id="txtResgate_Filtro_Assessor" type="text" maxlength="10" style="width: 5em;" />
                </p>

                <p>
                    <label for="txtResgate_Filtro_DataInicial" >Data Inicial:</label>
                    <input name="txtResgate_Filtro_DataInicial" id="txtResgate_Filtro_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataInicial" class="Mascara_Data Picker_Data" style="float:left; width: 70px;" />
                </p>

                <p>
                    <label for="txtResgate_Filtro_DataFinal" >Data Final:</label>
                    <input name="txtResgate_Filtro_DataFinal" id="txtResgate_Filtro_DataFinal" type="text" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" Propriedade="DataFinal" class="Mascara_Data Picker_Data" style="float:left; width: 70px;" />
                </p>
                
            </div>

            <div id="divResgate_Filtro_Status"style="margin-left: 20em; position:absolute;">
                <p><input name="chkResgate_Filtro_Status" id="chkResgate_Filtro_Todos"         type="radio" value="0" checked="checked" /><label for="chkResgate_Filtro_Todos"      >Todos      </label></p>
                <p><input name="chkResgate_Filtro_Status" id="chkResgate_Filtro_Rejeitadas"    type="radio" value="5" />                  <label for="chkResgate_Filtro_Rejeitadas" >Rejeitadas </label></p>
                <p><input name="chkResgate_Filtro_Status" id="chkResgate_Filtro_Aprovadas"     type="radio" value="4" />                  <label for="chkResgate_Filtro_Aprovadas"  >Aprovadas  </label></p>
                <p><input name="chkResgate_Filtro_Status" id="chkResgate_Filtro_EmAnalise"     type="radio" value="3" />                  <label for="chkResgate_Filtro_EmAnalise"  >Em análise </label></p>
            </div>

            <div style="margin-top:8em; float:right">
                <p>
                    <button class="btnResgate_Busca"  id="btnResgate_Buscar" onclick="return btnResgate_Buscar_Click()" >Buscar     </button>
                    <button class="btnResgate_Filtra" id="btnRegate_Filtrar" onclick="return btnRegate_Filtrar_Click()">Filtrar     </button>
                    <button class="btnResgate_Limpar" id="btnResgate_Limpar" onclick="return btnResgate_Limpar_Click()">Limpar  </button>
                </p>
            </div>
        
            <div id="divResgate_Resultados" style="display:none; margin-top:11em;">
                <table id="tblResgate_Resultados"></table>
                <div id="pager"></div>
                <button class="btnResgate_Aprovar"  onclick="return btnResgate_Aprovar_Click(this)"    id="btnAprovar" >Aprovar Selecionadas   </button>
                <button class="btnResgate_Reprovar" onclick="return btnResgate_Reprovar_Click(this)"   id="btnRejeitar">Reprovar Selecionadas  </button>
            </div>
            
        </form>
    </body>
</html>