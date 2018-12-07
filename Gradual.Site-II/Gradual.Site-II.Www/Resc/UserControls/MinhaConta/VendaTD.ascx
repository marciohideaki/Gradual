<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendaTD.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.MinhaConta.VendaTD" %>
<asp:MultiView ID="Venda_Views" runat="server" ActiveViewIndex="0"  >
    <asp:View ID="Venda_ViewGridTitulosDisponiveis" runat="server" >

        <div  class="form-consulta-tesouro form-padrao clear">
            <div class="row">
                <div class="col3">
                    <div class="campo-consulta-tesouro">
                        <label>Tipo de Título:</label>
                        <asp:DropDownList ID="cboTipoDeTitulo" datavaluefield="Codigo" datatextfield="Nome" runat="server"></asp:DropDownList>                                                    
                    </div>
                </div>
                                                        
                <div class="col3">
                    <div class="campo-consulta-tesouro">
                        <label>Vencimento:</label>
                        <asp:TextBox ID="txtBusca_Vencimento" runat="server" CssClass="calendario"></asp:TextBox>
                    </div>
                </div>

                <div class="col3">
                    <div class="campo-consulta-tesouro">
                        <label>Indexadores:</label>
                        <asp:DropDownList ID="cboIndexadores" datavaluefield="Codigo" datatextfield="Nome" runat="server" ></asp:DropDownList>                                                    
                    </div>
                </div>
            </div>
                                                    
            <div class="row" style="margin-bottom:20px">
                <div class="col1">
                    <asp:Button ID="btnRealizarBusca" runat="server" text="Consultar" OnClick="btnRealizarBusca_Click" CssClass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Venda')" />
                    &nbsp;&nbsp;
                    <asp:button id="btnVerCesta" runat="server" onclick="btnVerCesta_Click" text="Minha Cesta" cssclass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Venda')" style="width:110px" />
                </div>
            </div>
        <asp:Gridview id="VendaGridTitulosDisponiveis" 
            runat="server"
            autogeneratecolumns="False" 
            onsorting="grdTitulosDisponiveis_Sorting"
            onselectedindexchanged="VendaGridTitulosDisponiveis_SelectedIndexChanged"
            onpageindexchanging="grdTitulosDisponiveis_PageIndexChanging"
            datakeynames="CodigoTitulo" 
            GridLines="None"
            Width="100%"
            ShowFooter="false" 
            CssClass="tabela"
            style="margin-bottom:20px"
            >
            <HeaderStyle CssClass="tabela-titulo" Height="40px" />
            <RowStyle CssClass="tabela-type-small"  />
            <Columns>

                <asp:TemplateField HeaderText="Tipo" SortExpression="TipoTitulo">
                    <ItemTemplate>
                        <%#Eval("TipoTitulo") != "" ? Eval("TipoTitulo") : "-"%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"  />
                </asp:TemplateField>

                <asp:BoundField HeaderText="Título" DataField="NomeTitulo"  SortExpression="NomeTitulo" NullDisplayText="-" >
                </asp:BoundField>

                <asp:TemplateField HeaderText="Vencimento" SortExpression="DataVencimento"> 
                    <ItemTemplate>
                        <%#Eval("DataVencimento") != "" ? DateTime.Parse(Eval("DataVencimento").ToString()).ToString("dd/MM/yyyy") : "-"%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Indexador" SortExpression="TipoIndexadorNome"> 
                    <ItemTemplate>
                        <%#Eval("TipoIndexadorNome") != "" ? Eval("TipoIndexadorNome") : "-"%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Taxa de Juros (% ao ano)" SortExpression="ValorTaxaVenda"> 
                    <ItemTemplate>
                        <%#Eval("ValorTaxaVenda") != "" ? float.Parse(Eval("ValorTaxaVenda").ToString().Replace('.', ',')).ToString() + "%" : "-"%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Preço de 1 Título" SortExpression="ValorVenda"> 
                    <ItemTemplate>
                        <%#Eval("ValorVenda") != "" ? String.Format("{0:c}", float.Parse(Eval("ValorVenda").ToString().Replace('.', ','))) : "-"%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Qtd. Disponível p/ Venda" SortExpression="QuantidadeSaldo"> 
                    <ItemTemplate>
                        <%#Eval("QuantidadeSaldo") != "" ? float.Parse(Eval("QuantidadeSaldo").ToString().Replace('.', ',')).ToString("N2") : "-"%> 
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:ButtonField CommandName="Select" Text="Vender" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:ButtonField>

            </Columns>

        </asp:Gridview>
        </div>
    </asp:View>

    <asp:View ID="Venda_ViewBoletaVenda" runat="server">
        <div class="form-consulta-tesouro form-padrao clear" >
            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label >Titulo</label>
                        <asp:Label ID="lblBoleta_Titulo" runat="server" ></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row"></div>
            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label >Vencimento</label>
                        <asp:Label ID="lblBoleta_DataDeVencimento" runat="server" ></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row"></div>
            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label>Preço</label>
                        <asp:Label ID="lblBoleta_PrecoUnitario" runat="server" ></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row"></div>
            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label>Quantidade</label>
                        <asp:TextBox ID="txtBoleta_Quantidade" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col3">
                    <div class="campo-consulta">
                        <label></label>
                        <asp:button id="btnBoleta_CalcularTotal" runat="server" text="Calcular Total" OnClick="btnBoleta_CalcularTotal_Click"  CssClass="botao btn-padrao btn-erica" style="width:200px" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label>Vlr. Venda</label>
                        <asp:TextBox ID="txtBoleta_ValorDaVenda" runat="server" CssClass="GradualTextBox GradualTextBoxReadOnly" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label>Taxa CBLC</label>
                        <asp:TextBox ID="txtBoleta_TaxaCBLC" runat="server" CssClass="GradualTextBox GradualTextBoxReadOnly" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label>Taxa Ag. Custódia</label>
                        <asp:TextBox ID="txtBoleta_TaxaAgCustodia" runat="server" CssClass="GradualTextBox GradualTextBoxReadOnly" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <label>Valor Total</label>
                        <asp:TextBox ID="txtBoleta_ValorTotalDaVenda" runat="server" onkeypress="reais(this,event)" onkeydown="backspace(this,event)" ></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3">
                    <div class="campo-consulta">
                        <asp:button id="btnBoleta_CalcularQuantidade"   runat="server" cssclass="botao btn-padrao btn-erica" text="Calcular Quantidade" onclick="btnBoleta_CalcularQuantidade_Click" style="width:200px" />
                    </div>
                </div>
                <div class="col3">
                    <div class="campo-consulta">
                        <asp:button id="btnBoleta_IncluirTituloNaCesta" runat="server" cssclass="botao btn-padrao btn-erica" text="Incluir na Cesta" onclick="btnBoleta_IncluirTituloNaCesta_Click" style="width:200px" />
                    </div>
                </div>
                <div class="col3">
                    <div class="campo-consulta">
                        <asp:button id="btnBoleta_Voltar"               runat="server" cssclass="botao btn-padrao btn-erica" text="Voltar" OnClick="btnBoleta_Voltar_Click"  OnClientClick="btnTesouroCompra_Click(this, 'Venda')" style="width:200px"/>
                    </div>
                </div>
            </div>
        </div>
    </asp:View>

    <asp:View runat="server" ID="Venda_ViewCesta">
        <asp:Gridview  id="grdCesta"  runat="server" 
        AutoGenerateColumns="false"
        AllowPaging="false" 
        AllowSorting="false" 
        onrowediting="grdCesta_RowEditing" 
        DataKeyNames="CodigoTitulo" 
        CellPadding="0"
        GridLines="None"
        Width="100%" 
        ShowFooter="false" 
        RowHighlightColor="#FCF6D4" 
        MouseHoverRowHighlightEnabled="true"

        onrowcancelingedit="grdCesta_RowCancelingEdit" 
        onrowupdated="grdCesta_RowUpdated1" 
        onrowupdating="grdCesta_RowUpdating1" 
        onrowdeleting="grdCesta_RowDeleting"
        style="width:86%"
        class="tabela">

        <Columns>
            <asp:BoundField HeaderText="Tipo" ReadOnly="true" DataField="TipoTitulo" NullDisplayText="-"/>

            <asp:BoundField HeaderText="Título" ReadOnly="true" DataField="NomeTitulo" NullDisplayText="-"/>

            <asp:TemplateField HeaderText="Vencimento" SortExpression="DataVencimento"> 
                <ItemTemplate>
                    <%#Eval("DataVencimento") != "" ? DateTime.Parse(Eval("DataVencimento").ToString()).ToString("dd/MM/yyyy") : "-"%>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Quantidade">
                <ItemTemplate>
                    <%#Eval("QuantidadeVenda").ToString().Replace(".", ",")%>
                    <asp:HiddenField runat="server" ID="hdObjRef" Value='<%#Eval("CodigoTitulo")%>' />
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtQuantidade" Text='<%#Eval("QuantidadeVenda").ToString().Replace(".",",")%>' ></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdObjRef" Value='<%#Eval("CodigoTitulo")%>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Valor da Venda">
                <ItemTemplate>
                    <%#Eval("ValorTitulo") != "" ? String.Format("{0:c}", Double.Parse(Eval("ValorTitulo").ToString().Replace(".", ",")) * Double.Parse(Eval("QuantidadeVenda").ToString().Replace(".", ","))) : "-"%>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Taxa CBLC">
                <ItemTemplate>
                    <%#Eval("ValorTaxaCBLC") != "" ? String.Format("{0:c}", float.Parse(Eval("ValorTaxaCBLC").ToString().Replace('.', ','))) : "-"%>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Taxa Ag. de Cust.">
                <ItemTemplate>
                    <%#Eval("ValorTaxaAC") != "" ? String.Format("{0:c}", float.Parse(Eval("ValorTaxaAC").ToString().Replace('.', ','))) : "-"%>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Valor após pgto taxas">
                <ItemTemplate>
                    <%# Eval("ValorTitulo") != "" ? (Double.Parse(Eval("ValorTaxaCBLC").ToString().Replace(".", ",")) + Double.Parse(Eval("ValorTaxaAC").ToString().Replace(".", ",")) + (Double.Parse(Eval("ValorTitulo").ToString().Replace(".", ",")) * Double.Parse(Eval("QuantidadeVenda").ToString().Replace(".", ",")))).ToString("c") : "-"%>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CommandField  
                HeaderStyle-Width="30px"
                ShowDeleteButton="false"
                DeleteText="Excluir"
                ShowEditButton="true"
                EditText="Editar"
                ShowCancelButton="true"
                CancelText="Cancelar"
                UpdateText="Atualizar"
                ShowSelectButton="false"
                ButtonType="Link"
                />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" CssClass="GradualTahoma11" OnClientClick="btnTesouroCompra_Click(this, 'Venda')" >Excluir</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
        </Columns>

    </asp:Gridview>

        <div class="form-consulta-tesouro form-padrao clear">
            <div class="row">
                <div class="col3">
                    <div class="campo-basico campo-senha">
                        <label>Ass. Eletrônica:</label>
                        <asp:textbox type="password" id="txtAssinaturaEletronica" runat="server" class="teclado-dinamico"  />
                        <button class="teclado-virtual" type="button"><img alt="Teclado Virtual" src="../../Resc/Skin/Default/Img/teclado.png"></button>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3">
                    <div class="campo-consulta-tesouro">
                        <asp:button id="btnCesta_ConfirmarVenda" runat="server" text="Confirmar Venda" onclick="btnCesta_ConfirmarVenda_Click" cssclass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Venda')" style="width:200px"/>
                    </div>
                </div>
            </div>
        </div>

 <div class="row" style="margin-bottom:20px">
    <div class="col3">
        <div class="campo-consulta-tesouro">
        <asp:button id="btnCesta_Desistir" runat="server" text="Desistir da Venda" onclick="btnCesta_Desistir_Click" cssclass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Venda')" style="width:200px" />
        </div>
    </div>

    <div class="col3">
        <div class="campo-consulta-tesouro">
            <asp:button id="btnCesta_AdicionarMais" runat="server" text="Adicionar Títulos" onclick="btnCesta_AdicionarMais_Click" cssclass="botao btn-padrao btn-erica"  OnClientClick="btnTesouroCompra_Click(this, 'Venda')" style="width:200px"/>
        </div>
    </div>

    <div class="col3">
        <div class="campo-consulta-tesouro">
            <asp:button id="btnCesta_Voltar" runat="server" text="Voltar para Consulta" onclick="btnCesta_Voltar_Click" cssclass="botao btn-padrao btn-erica" OnClientClick="btnTesouroCompra_Click(this, 'Venda')" style="width:200px"/>
        </div>
    </div>
</div>
</asp:View>
</asp:MultiView>
