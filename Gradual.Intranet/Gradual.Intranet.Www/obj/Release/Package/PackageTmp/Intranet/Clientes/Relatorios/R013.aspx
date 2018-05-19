<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R013.aspx.cs" Inherits="Gradual.Intranet.Www.Intranet.Clientes.Relatorios.R013" %>


<form id="form1" runat="server">

    <table>
        <thead>
            <tr>
                <td>Clientes Encontrados: <span style="font-size:15px;font-weight:bold"><%= gTotalClientes %></span> </td>
            </tr>
        </thead>
    </table>

    <table cellspacing="0" class="GridRelatorio">
    <link rel="Stylesheet" media="all"    href="../Skin/<%= this.SkinEmUso %>/Relatorio.css?v=<%= this.VersaoDoSite %>" />
        <thead
            <tr>
                <td style="text-align: left;">   CPF Cliente      </td>
                <td style="text-align: left;">   Nome Cliente     </td>
                <td style="text-align: left;">   Nome Assessor    </td>
                <td style="text-align: left;">   Estado           </td>
                <td style="text-align: left;">   Cidade           </td>
                <td style="text-align: left;">   Bairro           </td>
                <td style="text-align: left;">   Logradouro       </td>
                <td style="text-align: left;">   Complemento      </td>
                <td style="text-align: left;">   CEP              </td>
                <td style="text-align: center;"> Telefone         </td>
                <td style="text-align: center;"> Ramal            </td>
                <td style="text-align: center;"> Celular1         </td>
                <td style="text-align: center;"> Celular2         </td>
                <td style="text-align: left;">   e-mail           </td>
                <td style="text-align: left;">   Data de Cadastro </td>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <td colspan="6">
                    &nbsp;
                </td>
            </tr>
        </tfoot>
        <tbody>
        <asp:repeater id="rptRelatorio" runat="server">
            <itemtemplate>
            <tr>
                <td style="text-align:left">   <%# Eval("CPFCNPJ").ToString().TrimStart('0') %> </td>
                <td style="text-align:left">   <%# Eval("NomeCliente") %>                       </td>
                <td style="text-align:left">   <%# Eval("NomeAssessor") %>                      </td>
                <td style="text-align:left">   <%# Eval("UF") %>                                </td>
                <td style="text-align:left">   <%# Eval("Cidade") %>                            </td>
                <td style="text-align:left">   <%# Eval("Bairro") %>                            </td>
                <td style="text-align:left">   <%# Eval("Logradouro") %>                        </td>
                <td style="text-align:left">   <%# Eval("Complemento") %>                       </td>
                <td style="text-align:left">   <%# Eval("CEP") %>                               </td>
                <td style="text-align:center;"> <%# Eval("Telefone") %>                          </td>
                <td style="text-align:center;"> <%# Eval("Ramal") %>                             </td>
                <td style="text-align:center; width: 7em;"> <%# Eval("Celular1") %>                          </td>
                <td style="text-align:center; width: 7em;"> <%# Eval("Celular2") %>                          </td>
                <td style="text-align:left">   <%# Eval("Email") %>                             </td>
                <td style="text-align:left">   <%# Eval("DataCadastro") %>                      </td>
            </tr>
            </itemtemplate>
        </asp:repeater>

            <tr id="rowLinhaDeNenhumItem" class="NenhumItem" runat="server">
                <td colspan="6">
                    Nenhum item encontrado.
                </td>
            </tr>

        </tbody>
    </table>

    <table cellspacing="0" class="Grafico_PorAssessor" style="display:none">
        <tbody>

        <asp:repeater id="rptGrafico_PorAssessor" runat="server">
            <itemtemplate>
            <tr>
                <th> <%# Eval("Key")%>   </th>
                <td> <%# Eval("Value")%> </td>
            </tr>
            </itemtemplate>
        </asp:repeater>

        </tbody>
    </table>

    <table cellspacing="0" class="Grafico_PorEstado" style="display:none">
        <tbody>

        <asp:repeater id="rptGrafico_PorEstado" runat="server">
            <itemtemplate>
            <tr>
                <th> <%# Eval("Key")%>   </th>
                <td> <%# Eval("Value")%> </td>
            </tr>
            </itemtemplate>
        </asp:repeater>

        </tbody>
    </table>
    
    <div id="pnlGrafico_PorAssessor" style="width:680px;height:700px; margin:10px;"></div>
    <div id="pnlGrafico_PorEstado" style="float: right; width: 400px; height: 700px; margin: -710px 40px 0px 0px;"></div>
    
</form>
