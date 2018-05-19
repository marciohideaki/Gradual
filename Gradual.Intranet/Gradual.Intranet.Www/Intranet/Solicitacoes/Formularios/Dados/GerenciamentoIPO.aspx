<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GerenciamentoIPO.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Solicitacoes.Formularios.Dados.GerenciamentoIPO" %>

<form id="form1" runat="server">

<div class="pnlCliente_IPO_Container">
    
    <ul class="pnlFormulario_Abas_Container">
        <li class="Selecionada"><a href="#" rel="pnlSolicitacoes_IPO"           onclick="return pnlFormulario_Abas_li_a_Click(this)">Cadastro de IPO</a></li>
        <li>                    <a href="#" rel="pnlSolicitacoes_IPO_Cliente"   onclick="return pnlFormulario_Abas_li_a_Click(this)">Gerenciamento IPO/Cliente</a></li>
    </ul>
    <div id="pnlSolicitacoes_IPO">
    
    <table id="tblProdutosGerenciamentoIPO_ListaDeProdutos" class="GridRelatorio" cellspacing="0" style="float: left; width: 55%; margin:0.2em">

        <thead>
            <tr>
                <td>Ativo</td>
                <td>Código ISIN</td>
                <td>Empresa</td>
                <td>Modalidade</td>
                <td>Data Inicial</td>
                <td>Data Final</td>
                <td style="display:none">Hora Máxima</td>
                <td style="display:none">Valor Mínimo</td>
                <td style="display:none">Valor Máximo</td>
                <td style="display:none">% Garantias</td>
                <td>Ativado?</td>
                <td style="display:none">Observacões</td>
                <td>Editar</td>
            </tr>
        </thead>
        <tfoot>
            <tr> 
                <td colspan="13">&nbsp;</td>
            </tr>
        </tfoot>
        <tbody>

            <asp:repeater id="rptListaDeProdutos" runat="server">
            <itemtemplate>

            <tr rel="<%# Eval("CodigoIPO") %>">
                <td><%# Eval("Ativo")%></td>
                <td style="white-space:nowrap"><%# Eval("CodigoISIN")%> </td>
                <td style="white-space:nowrap"><%# Eval("DsEmpresa") %> </td>
                <td style="white-space:nowrap"><%# Eval("Modalidade")%> </td>
                <td style="white-space:nowrap"><%# Eval("DataInicial", "{0:dd/MM/yyyy}")%> </td>
                <td style="white-space:nowrap"><%# Eval("DataFinal", "{0:dd/MM/yyyy}")%> </td>
                <td style="display:none"><%# Eval("HoraMaxima")%> </td>
                <td style="display:none"><%# Eval("VlMinimo", "{0:n}")%> </td>
                <td style="display:none"><%# Eval("VlMaximo", "{0:n}")%> </td>
                <td style="display:none"><%# Eval("VlPercentualGarantia", "{0:n}")%> </td>
                <td><%# (Eval("StAtivo").ToString().ToLower().Trim() == "false" ? "NÃO" : "SIM")%> </td>
                <td style="display:none"><%# Eval("Observacoes")%>  </td>
                <td> <button class="IconButton Editar" title="Editar Produto" onclick="return btnSolicitacoes_GerenciamentoIPO_Editar_Click(this)" style="margin-top:2px;margin-bottom:2px"><span>Excluir</span></button>  </td>
            </tr>

            </itemtemplate>
            </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="13">Nenhum produto encontrado.</td>
            </tr>
        </tbody>

    </table>
    
    <div id="pnlFormulario_GerenciamentoIPO" style="float:right;width:35%" class="pnlFormulario_Campos" >
    
    <h5 style="margin-top: 0.2em;">Cadastro de Oferta Pública (IPO)</h5>
    
        <input type="hidden" id="hidSolicitacoes_IPO_ListaJson" class="ListaJson" runat="server" value="" />


        <input type="hidden" id="hidProduto_Codigo_IPO" Propriedade="CodigoIPO" runat="server" />

        <p >
            <label for="txtProduto_IPO_Ativo">Ativo:</label>
            <input  id="txtProduto_IPO_Ativo" type="text" Propriedade="Ativo" class="validate[required]" maxlength="100" value="" style="float:left; text-transform:uppercase; width: 100px"  />
        </p>

        <p >
            <label for="txtProduto_IPO_CodigoISIN">Código ISIN:</label>
            <input  id="txtProduto_IPO_CodigoISIN" type="text" Propriedade="CodigoISIN" class="validate[required]" maxlength="35" value="" style="float:left; text-transform:uppercase; width:100px"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_Empresa">Empresa:</label>
            <input  id="txtProduto_IPO_Empresa" type="text" Propriedade="DsEmpresa" class="validate[required]" maxlength="150" value="" style="float:left;"  />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="cboProduto_IPO_Modalidade">Tipo:</label>
            <select id="cboProduto_IPO_Modalidade" style="float:left; width: 150px" class="validate[required]" Propriedade ="Modalidade">
                <option value="">Selecione</option>
                <option value="Primaria">Primária</option>
                <option value="Secundaria">Secundária</option>
                <option value="ON">ON</option>
                <option value="PN">PN</option>
                <option value="Primaria ON">Primária ON</option>
                <option value="Primaria PN">Primária PN</option>
                <option value="Secundaria ON">Secundaria ON</option>
                <option value="Secundaria PN">Secundária PN</option>
            </select>
            <%--<input  id="txtProduto_Cambio_Taxas" type="text" Propriedade="Taxas" class="validate[required]" maxlength="7" value="" style="float:left;"  />--%>

        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_DataInicial">Data Inicial:</label>
            <input  id="txtProduto_IPO_DataInicial" type="text" Propriedade="DataInicial" class="Mascara_Data Picker_Data" maxlength="12" value="" style="float:left; width: 100px"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_DataFinal">Data Final:</label>
            <input  id="txtProduto_IPO_DataFinal" type="text" Propriedade="DataFinal" class="Mascara_Data Picker_Data" value="" style="float:left; width:100px"  />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_HoraMaxima">Hora Máxima</label>
            <input  id="txtProduto_IPO_HoraMaxima" type="text" Propriedade="HoraMaxima" class=" validate[required] Mascara_Hora" value="" style="float:left; width: 100px"  />
        </p>
        
        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_VlMinimo">Valor Mínimo</label>
            <input  id="txtProduto_IPO_VlMinimo" type="text" Propriedade="VlMinimo" class="validate[required] ValorMonetario" value="" style="float:left; width:100px"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_VlMaximo">Valor Máximo</label>
            <input  id="txtProduto_IPO_VlMaximo" type="text" Propriedade="VlMaximo" class="validate[required] ValorMonetario" value="" style="float:left; width:100px"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_VlPercentualGarantia">% Garantia</label>
            <input  id="txtProduto_IPO_VlPercentualGarantia" type="text" Propriedade="VlPercentualGarantia" class="validate[required] ValorMonetario" value="" style="float:left;  width:100px"  />
        </p>

        <p class="Form_TiposDePendenciaCadastral">
            <label for="txtProduto_IPO_Observacoes">Observações</label>
            <textarea  id="txtProduto_IPO_Observacoes"  Propriedade="Observacoes" class="" style="float:left;"   />
            
        </p>

        <p class="Form_TiposDePendenciaCadastral" style="padding-left:28%;width:42%;padding-top:20px">
            <label for="chkProduto_IPO_AtivoSite">IPO Ativo no site</label>
            <input  id="chkProduto_IPO_AtivoSite" type="checkbox" Propriedade="StAtivo" class=""  />
        </p>

        <p class="BotoesSubmit">

            <button onclick="return btnProduto_GerenciamentoIPO_IncluirEditar_Click(this)" style="font-size:1em; margin-left: 225px">Salvar IPO</button>

        </p>
    </div>
    </div>
    <div id="pnlSolicitacoes_IPO_Cliente" style="display:none;"> 
        <div id="pnlBusca_Solicitacoes_IPO_Form" class="Busca_Formulario Busca_Formulario_Extendido Busca_Formulario_2Linhas" style="width:99%">
            <p class="LadoALado_Pequeno" style="width:200px">
                <label for="cboBusca_Solicitacoes_IPO_BuscarPor" style="width:80px">Buscar Por:</label>
                <select id="cboBusca_Solicitacoes_IPO_BuscarPor" style="width:10.0em; margin-top:1px">
                    <option value="CodBovespa" selected ="selected">Código Bovespa</option>
                    <option value="CpfCnpj">CPF/CNPJ</option>
                    <option value="CodigoAssessor">Codigo Assessor</option>
                </select>
                
            </p>    
            <p class="LadoALado_Pequeno" style="width:200px">
                <%--<label for="txtBusca_Solicitacoes_IPO_Termo" style="width:80px">Busca:</label>--%>
                <input type="text" id="txtBusca_Solicitacoes_IPO_Termo" maxlength="150" style="width:13.6em;margin-right:0.4em" />
            </p>
            <p class="LadoALado_Pequeno" style="width:150px">
                <label for="cboBusca_Solicitacoes_IPO_Status" >Status:</label>
                <select id="cboBusca_Solicitacoes_IPO_Status" style="width:9.5em">
                    <option value="0">Todos os status           </option>
                    <option value="1">1 - Solicitada            </option>
                    <option value="2">2 - Executada             </option>
                    <option value="3">3 - Cancelada             </option>
                </select>
            </p>
            

            <p class="LadoALado_Pequeno" style="width:175px">
                <label for="txtBusca_Solicitacoes_IPO_DataInicial">Data De:</label>
                <input  id="txtBusca_Solicitacoes_IPO_DataInicial" type="text" value="<%= DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" />
            </p>

            <p class="LadoALado_Pequeno" style="width:350px">
                <label for="txtBusca_Solicitacoes_IPO_DataFinal" >Até:</label>
                <input  id="txtBusca_Solicitacoes_IPO_DataFinal" type="text" value="<%= DateTime.Now.ToString("dd/MM/yyyy") %>" maxlength="10" class="Mascara_Data Picker_Data" style="width:92px" />
                <button class="btnBusca btnBuscaSolicitacoesReservaIPO" onclick="return btnBusca_Click()">Buscar</button>
                <button class="btnBusca btnSolicitacoesAtualizaLimites"  onclick="return btnSolicitacoesGerenciamentoIPO_Limites_Click()" style="width:120px">Atualizar Limites</button>
            </p>
        </div>
        
        <div id="pnlBusca_Solicitacoes_IPO_Resultados" class="Busca_Resultados_Abaixo" style="margin-left: 0px; margin-top: 20px; width:99% ">

            <table id="tblBusca_Solicitacoes_IPO_Resultados"></table>
            <div id="pnlBusca_Solicitacoes_IPO_Resultados_Pager"></div>

            
            <div style="text-align:right; padding-top:1em;">
                <button class="btnBusca" id="btnSolicitacoesSalvarStatus" onclick="return btnSolicitacoes_SalvarStatusSolicitacoes_Click(this)" style="width:120px" >Salvar Status</button>
            </div>
            

            <button class="ExpandirPraBaixo_Abaixo" title="Clique para expandir o painel e ver mais resultados" onclick="return btnResultadoBusca_ExpandirPainelAbaixo_Click(this, 'tblBusca_Solicitacoes_IPO_Resultados')" style="">
                <span>Expandir tabela</span>
            </button>

        </div>
    </div>
</div>
</form>

