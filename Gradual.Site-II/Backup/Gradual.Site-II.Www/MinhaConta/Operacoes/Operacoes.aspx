<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Operacoes.aspx.cs" Inherits="Gradual.Site.Www.MinhaConta.Operacoes.Operacoes" MasterPageFile="~/PaginaInterna.Master" %>

<%@ Register src="~/Resc/UserControls/Sauron.ascx" tagname="Sauron" tagprefix="ucSauron" %>

<%@ Register src="~/Resc/UserControls/MinhaConta/AbasOperacoes.ascx" tagname="AbasOperacoes" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="PaginaConteudo">
<div id="conteudo" class="interna">
    <div class="row">
        <div class="col2">
            <uc1:AbasOperacoes ID="AbasOperacoes1" Modo="menu" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col1">
            <div class="menu-exportar clear">
                <h3>Envio de Ordens <div class="lblTituloAlteracao" style="display:inline" ></div> </h3>
            </div>
        </div>
    </div>
    <div class="row">
    <div class="col3-3">
        <div class="form-consulta form-padrao-envioordens clear">
            <div class="row">
                <div class="col1">
                    <div class="campo-consulta">
                        <label>Ativo</label>
                        <input type="text"  name="input-padrao" style="text-transform:uppercase" 
                            onkeydown="return txtOrdem_Ativo_KeyDown(this, event)" id="txtOrdem_Ativo"
                            onblur="return txtOrdem_Ativo_Blur(this, event)" tabindex="0" />
                    </div>
                </div>
             </div>

             <div class="row">
                <div class="col1">
                    <div class="campo-consulta">
                        <label>Validade</label>
                        <select class="cboOrdem_Validade" disabled="disabled" tabindex="1">
                            <option value="4" selected>Para o Dia</option>
                            <option value="5">30 Dias</option>
                            <option value="6">Executa ou Cancela</option>
                        </select>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    <div class="campo-consulta">
                        <label>Quantidade</label>
                        <input type="text" class="txtOrdem_Quantidade ValorNumerico" maxlength="10" 
                            onkeydown="return valNum_OnKeyDown(event)" 
                            onblur="txtOrdem_Quantidade_Blur(this)" disabled="disabled" tabindex="2" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    <div class="campo-consulta">
                        <label>Tipo</label>
                        <select class="cboOrdem_Tipo" onchange="return cboOrdem_Tipo_Change(this)" 
                            disabled="disabled" tabindex="3">
                            <option value="2" selected>Ordem Normal</option>
                            <option value="5">Start Compra</option>
                            <option value="5">Stop Venda</option>
                        </select>
                    </div>
                </div>
              </div>

              <div class="row">
                <div class="col1">
                    <div class="campo-consulta">
                        <label>Preço</label>
                        <input type="text" class="txtOrdem_Preco ValorNumerico" maxlength="10" 
                            onkeydown="return valF2Num_OnKeyDown(event)" 
                            onblur="txtOrdem_Quantidade_Blur(this)" disabled="disabled" tabindex="4" />
                    </div>
                </div>
              </div>

              <div class="row">
                <div class="col1">
                    <div class="campo-consulta">
                        <label>Valor</label>
                        <input class="txtOrdem_Valor" type="text" readonly="readonly" tabindex="5" />
                        <%--<span class="lblOrdem_Valor ValorNumerico" style="width:10.2em">&nbsp;</span>--%>
                    </div>
                </div>
            </div>

            <div class="row rowStopStart" style="display:none">
                <div class="col1">
                    <div class="campo-consulta">
                        <label>Preço disparo Stop/Start:</label>
                        <input type="text" class="txtOrdem_OrdemStartStop_PrecoDisparo ValorNumerico" tabindex="6" onkeydown="return valF2Num_OnKeyDown(event)" maxlength="10" disabled="disabled" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col1">
                    <div class="form-padrao-envioordens">
                        <div class="campo-basico campo-senha">
                            <label>Assinatura Eletrônica:</label>
                            <input type="password" value="" class="txtOrdem_Assinatura mostrar-teclado" name="input-padrao" tabindex="7" />
                            <button class="teclado-virtual-envioordens" type="button"><img src="../../Resc/Skin/Default/Img/teclado.png" alt="Teclado Virtual" /></button>
                        </div>
                        <small class="esqueceu-assinatura"><a href="../Cadastro/EsqueciAssinatura.aspx">Esqueceu sua assinatura?</a></small>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3-3">
                    <div class="lista-checkbox">
                        <label class="checkbox"><input type="checkbox" value="" id="chkOrdem_ManterAssinatura" /> Manter a Assinatura Eletrônica</label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col3-3">
                    <input class="botao btn-azul btn-erica btnComprar" type="submit" value="Comprar" onclick="return btnOrdem_Comprar_Click(this)" disabled="disabled" />
                    <input class="botao btn-verde btn-erica btnVender" type="submit" value="Vender" onclick="return btnOrdem_Vender_Click(this)"  disabled="disabled" />
                </div>
            </div>
        </div>
    </div>

    <div class="col3" style="margin-left:150px">
        <div class="col2">
            <div class="abas-menu" >

                <ul class="abas-menu">
                    <li class="ativo" data-IdConteudo="BookIntegral"   ><a href="#" id="A1" onclick="return DadosDoAtivo_SelecionarAba(this, 1)">Integral</a></li>
                    <li               data-IdConteudo="BookFracionario"><a href="#" id="A2" onclick="return DadosDoAtivo_SelecionarAba(this, 2)">Fracionário</a></li>
                </ul>

                <div id="abas-conteudo" class="abas-conteudo">

                    <div class="aba" data-IdConteudo="BookIntegral">

                        <div class="row" id="Div3">
                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Código <br /><span class="lblDadosDoAtivo_Codigo"></span></p>
                            </div>

                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Preço <br /> <span class="lblDadosDoAtivo_Preco"></span></p>
                            </div>

                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Variação <br /><span class="lblDadosDoAtivo_Variacao"></span></p>
                            </div>

                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Lote Mín. <br /><span class="lblDadosDoAtivo_LoteMinimo"></span></p>
                            </div>
                        </div>
                        <div class="row">
                            <button class="botao btn-padrao btn-erica btn-aba" type="button" onclick="return Ordem_RecarregarDadosDoAtivo()">Atualizar</button>
                        </div>

                        <div class="row">
                            <div class="col1">
                                <table class="PainelFinanceiro resumo" cellspacing="0" style="width:100%; margin-top:15px">
                                    <thead>
                                    <tr class="tabela-titulo alignCenter tabela-type-small">
                                        <td colspan="4"><span>Painel Financeiro</span></td>
                                    </tr>
                                    </thead>
                                    <tbody>

                                    <tr class="tabela-type-very-small">    
                                        <td class="alignRight">Máxima</td>    
                                        <td class="alignLeft" data-propriedade="jXD">0,00</td>     
                                        <td class="alignRight">Fech. Anterior</th>    
                                        <td class="alignLeft" data-propriedade="jVF">0,00</td>     
                                    </tr>
                                    <tr class="tabela-type-very-small">    
                                        <td class="alignRight">Mínima</td>    
                                        <td class="alignLeft" data-propriedade="jND">0,00</td>     
                                        <td class="alignRight">Volume</td>            
                                        <td class="alignLeft" data-propriedade="jLA">0,00</td>         
                                    </tr>
                                    <tr class="tabela-type-very-small">    
                                        <td class="alignRight">Abertura</td>  
                                        <td class="alignLeft" data-propriedade="jVA">0,00</td>     
                                        <td class="alignRight">Quantidade</td>        
                                        <td class="alignLeft" data-propriedade="jNN">0,00</td>     
                                    </tr>

                                    </tbody>
                                </table>
                                <table class="OfertasDeCompraVenda" >
                                    <thead>
                                        <tr class="tabela-titulo alignCenter tabela-type-small">
                                            <td colspan="6">Ofertas de Compra e Venda</td>
                                        </tr>
                                        <tr class="tabela-area alignCenter tabela-type-small">
                                            <td colspan="3">Ofertas de Compra</td>
                                            <td colspan="3">Ofertas de Venda</td>
                                        </tr>
                                        <tr class="tabela-area alignCenter tabela-type-small">
                                            <td>Corr.</td>
                                            <td>Qtde.</td>
                                            <td>Valor</td>
                                            <td>Valor</td>
                                            <td>Qtde.</td>
                                            <td>Corr.</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>

                                    </tbody>
                                </table>
                            </div>
                        </div>

                    </div>

                    <div class="aba" data-IdConteudo="BookFracionario">

                        <div class="row">
                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Código <br /><span class="lblDadosDoAtivo_Codigo"></span></p>
                            </div>

                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Preço <br /><span class="lblDadosDoAtivo_Preco"></span></p>
                            </div>

                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Variação <br /><span class="lblDadosDoAtivo_Variacao"></span></p>
                            </div>

                            <div class="col4">
                                <p class="quadro-cinza-envioordens">Lote Mín. <br /><span class="lblDadosDoAtivo_LoteMinimo"></span></p>
                            </div>
                        </div>
                    
                        <div class="row">
                            <button class="botao btn-padrao btn-erica btn-aba" type="button" onclick="return Ordem_RecarregarDadosDoAtivo()">Atualizar</button>
                        </div>

                        <div class="row">
                            <div class="col1">
                                <table class="PainelFinanceiro resumo" cellspacing="0" style="width:100%; margin-top:15px">
                                    <thead>
                                    <tr class="tabela-titulo alignCenter tabela-type-small">
                                        <td colspan="4"><span>Painel Financeiro</span></td>
                                    </tr>
                                    </thead>
                                    <tbody>

                                    <tr class="tabela-type-very-small">    
                                        <td class="alignRight">Máxima</td>    
                                        <td class="alignLeft" data-propriedade="jXD">0,00</td>     
                                        <td class="alignRight">Fech. Anterior</th>    
                                        <td class="alignLeft" data-propriedade="jVF">0,00</td>     
                                    </tr>
                                    <tr class="tabela-type-very-small">    
                                        <td class="alignRight">Mínima</td>    
                                        <td class="alignLeft" data-propriedade="jND">0,00</td>     
                                        <td class="alignRight">Volume</td>            
                                        <td class="alignLeft" data-propriedade="jLA">0,00</td>         
                                    </tr>
                                    <tr class="tabela-type-very-small">    
                                        <td class="alignRight">Abertura</td>  
                                        <td class="alignLeft" data-propriedade="jVA">0,00</td>     
                                        <td class="alignRight">Quantidade</td>        
                                        <td class="alignLeft" data-propriedade="jNN">0,00</td>     
                                    </tr>
                                    </tbody>
                                </table>

                                <table class="OfertasDeCompraVendaFracionario" >
                                    <thead>
                                    <tr class="tabela-titulo alignCenter tabela-type-small">
                                        <td colspan="6">Ofertas de Compra e Venda</td>
                                    </tr>
                                    <tr class="tabela-area alignCenter tabela-type-small">
                                        <td colspan="3">Ofertas de Compra</td>
                                        <td colspan="3">Ofertas de Venda</td>
                                    </tr>
                                    <tr class="tabela-area alignCenter tabela-type-small">
                                        <td>Corr.</td>
                                        <td>Qtde.</td>
                                        <td>Valor</td>
                                        <td>Valor</td>
                                        <td>Qtde.</td>
                                        <td>Corr.</td>
                                    </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trA tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>
                                        <tr class="trB tabela-type-very-small">
                                            <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td><td class="alignRight">&nbsp;</td>
                                        </tr>

                                    </tbody>
                                </table>
                            </div>
                        </div>

                    </div>

                </div>

            </div>
        </div>
    </div>
</div>
    <div class="row">
    <%--<div class="col1">
        <div id="abas" class="abas-menu" >
            <ul class="abas-menu">
                
                <button class="botao btn-padrao btn-erica btn-aba" type="button" onclick="return Ordem_RecarregarDadosDoAtivo()">Atualizar</button>
                <li class="ativo" data-IdConteudo="BookIntegral"   ><a href="#" id="li_Integral" onclick="return DadosDoAtivo_SelecionarAba(this, 1)">Integral</a></li>
                <li               data-IdConteudo="BookFracionario"><a href="#" id="li_Fracionario" onclick="return DadosDoAtivo_SelecionarAba(this, 2)">Fracionário</a></li>
            </ul>
                            
            <div id="abas-conteudo" class="abas-conteudo">
                <div class="aba" data-IdConteudo="BookIntegral">
                                    
                    <div class="row" id="pnlDadosDoAtivo">
                        <div class="col4">
                            <p class="quadro-cinza">Código <span class="lblDadosDoAtivo_Codigo"></span></p>
                        </div>
                                        
                        <div class="col4">
                            <p class="quadro-cinza">Preço <span class="lblDadosDoAtivo_Preco"></span></p>
                        </div>
                                        
                        <div class="col4">
                            <p class="quadro-cinza">Variação <span class="lblDadosDoAtivo_Variacao"></span></p>
                        </div>
                                        
                        <div class="col4">
                            <p class="quadro-cinza">Lote Mín. <span class="lblDadosDoAtivo_LoteMinimo"></span></p>
                        </div>
                    </div>
                                    
                    <div class="row">
                        <div class="col1">
                            <table class="tabela OfertasDeCompraVenda" >
                                <thead>
                                    <tr class="tabela-titulo alignCenter">
                                        <td colspan="6">Ofertas de Compra e Venda</td>
                                    </tr>
                                    <tr class="tabela-area alignCenter">
                                        <td colspan="3">Ofertas de Compra</td>
                                        <td colspan="3">Ofertas de Venda</td>
                                    </tr>
                                    <tr class="tabela-area alignCenter">
                                        <td>Corr.</td>
                                        <td>Qtde.</td>
                                        <td>Valor</td>
                                        <td>Valor</td>
                                        <td>Qtde.</td>
                                        <td>Corr.</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </div>
                                    
                </div>
                                
                <div class="aba" data-IdConteudo="BookFracionario">
                                    
                    <div class="row">
                        <div class="col4">
                            <p class="quadro-cinza">Código <span class="lblDadosDoAtivo_Codigo"></span></p>
                        </div>
                                        
                        <div class="col4">
                            <p class="quadro-cinza">Preço <span class="lblDadosDoAtivo_Preco"></span></p>
                        </div>
                                        
                        <div class="col4">
                            <p class="quadro-cinza">Variação <span class="lblDadosDoAtivo_Variacao"></span></p>
                        </div>
                                        
                        <div class="col4">
                            <p class="quadro-cinza">Lote Mín. <span class="lblDadosDoAtivo_LoteMinimo"></span></p>
                        </div>
                    </div>
                                    
                    <div class="row">
                        <div class="col1">
                            <table class="tabela OfertasDeCompraVendaFracionario" >
                                <thead>
                                <tr class="tabela-titulo alignCenter">
                                    <td colspan="6">Ofertas de Compra e Venda</td>
                                </tr>
                                <tr class="tabela-area alignCenter">
                                    <td colspan="3">Ofertas de Compra</td>
                                    <td colspan="3">Ofertas de Venda</td>
                                </tr>
                                <tr class="tabela-area alignCenter">
                                    <td>Corr.</td>
                                    <td>Qtde.</td>
                                    <td>Valor</td>
                                    <td>Valor</td>
                                    <td>Qtde.</td>
                                    <td>Corr.</td>
                                </tr>
                                </thead>
                                <tbody>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trA">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>
                                    <tr class="trB">
                                        <td style="text-align:left">&nbsp;</td><td>&nbsp;</td><td class="tabela-azul alignRight">&nbsp;</td>        <td class="tabela-verde alignLeft">&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                                    </tr>

                                </tbody>
                            </table>
                        </div>
                    </div>
                                    
                </div>
                                
            </div>
                            
        </div>
    </div>--%>
</div>
</div>
<div id="pnlMessageBox" style="display:none">

    <div class="pnlMessageBox_Content pnlMessageBox_Content_Confirmacao" style="display:none">
        <p style="text-align:center">
            Confirmar <em class="lblSentido">&nbsp;</em> de papel <em class="lblPapel">&nbsp;</em> com validade <em class="lblValidade">&nbsp;</em><br />
            <span class="pnlSomatoria">
            <em class="lblQuantidade">&nbsp;</em> a <em class="lblPreco">&nbsp;</em> totalizando <em class="lblTotal" style="color:#3c6033">&nbsp;</em>
            </span>
        </p>

        <p class="pnlOrdem_MensagemDeConfirmacao_BotoesDeConfirmacao" style="text-align:center">
            <button class="botao btn-padrao btn-erica" onclick="return btnOrdem_Confirmar_Click(this)">Confirmar</button>
            <button class="botao btn-padrao btn-erica"  onclick="return btnOrdem_Cancelar_Click(this)">Cancelar</button>
        </p>

    </div>

    <div class="pnlMessageBox_Content pnlMessageBox_Content_Aguarde" style="display:none">
        <p><span>Enviando Ordem, por favor aguarde...</span></p>
    </div>

    <div class="pnlMessageBox_Content pnlMessageBox_Content_MensagemQualquer" style="display:none">

        <h5>Titulo</h5>
        <div class="pnlMessageBox_Content_Criticas"></div>

        <p class="pnlOrdem_MensagemDeConfirmacao_BotoesDeConfirmacao" style="text-align:center">
            <button class="botao btn-padrao btn-erica" onclick="return btnOrdem_Ok_Click(this)" style="padding:0.3em 0.4em">OK</button>
        </p>
    </div>
</div>
</section>

<ucSauron:Sauron id="Sauron1" runat="server" nomedapagina="Financeiro - Operações" />

</asp:Content>

