<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuloCMS.ascx.cs" Inherits="Gradual.Site.Www.Resc.UserControls.ModuloCMS" %>


<div id="pnlModuloCMS" style="display:none">

    <ul id="pnlBotoesSecoes">
        <li>
            <button title="Tipo de Usuário" onclick="return btnCMS_TipoDeUsuario_Click(this)" class="Tipo_<%= this.PaginaBase.AliasDoTipoDeUsuario %>"><span>Tipo de Usuário</span></button>
        </li>
        <li>
            <button title="Páginas" onclick="return btnCMS_Paginas_Click(this)"><span>Páginas</span></button>
        </li>
        <li>
            <button title="Estrutura da Página" onclick="return btnCMS_EstruturaDaPagina_Click(this)"><span>Estrutura da Página</span></button>
        </li>
        <li>
            <button title="Conteúdo Dinâmico" onclick="return btnCMS_ConteudoDinamico_Click(this)"><span>Conteúdo Dinâmico</span></button>
        </li>
        <li>
            <button title="Arquivos" onclick="return btnCMS_Imagens_Click(this)"><span>Imagens</span></button>
        </li>
        <li>
            <button title="Informações da Página" onclick="return btnCMS_InformacoesDaPagina_Click(this)" title="Salvar"><span>Informações</span></button>
        </li>
    </ul>

    <div id="pnlTipoDeUsuario" style="display:none; opacity:0; width: 10px;">
        <button id="btnTipoDeUsuario_Todos"         runat="server" onclick="return btnTipoDeUsuario_Click(1)" title="Todos">Todos</button>
        <button id="btnTipoDeUsuario_Visitante"     runat="server" onclick="return btnTipoDeUsuario_Click(2)" title="Visitante">Visitante</button>
        <button id="btnCopiar_Visitante"            runat="server" onclick="return CopiarEstrutura(2)"        title="Copiar a estrutura atual para -> Visitante" class="Copiar">&nbsp;</button>
        <button id="btnTipoDeUsuario_Cliente"       runat="server" onclick="return btnTipoDeUsuario_Click(3)" title="Cliente">Cliente</button>
        <button id="btnCopiar_Cliente"              runat="server" onclick="return CopiarEstrutura(3)"        title="Copiar a estrutura atual para -> Cliente" class="Copiar">&nbsp;</button>
    </div>

    <div class="pnlFormulariosCMS">

        <div id="pnlEstruturaContainer" class="ContainerCMS">

            <div class="ContainerCMS_Painel">

                <h4>Estrutura da Página</h4>

                <p>
                    <label for="cboCMS_Estrutura_AdicionarWidget">Adicionar:</label>
                    <select id="cboCMS_Estrutura_AdicionarWidget">
                        <option value="Acordeon">Acordeon</option>
                        <option value="Abas">Abas</option>
                        <option value="Lista">Lista</option>
                        <option value="ListaDeDefinicao">Lista de Definição</option>
                        <option value="Repetidor">Repetidor</option>
                        <option value="Embed">Código</option>
                        <option value="Titulo">Título</option>
                        <option value="Texto">Texto</option>
                        <option value="TextoHTML">Texto HTML</option>
                        <option value="Imagem">Imagem</option>
                        <option value="Tabela">Tabela</option>
                    </select>

                    <button class="BotaoIcone IconeMais" title="Adicionar" onclick="return btnCMS_Estrutura_AdicionarWidget_Click(this)"><span>Adicionar</span></button>
                </p>
                
                <div class="ContainerListaItens">

                    <ul id="lstEstruturaDaPagina" class="ListaDeItens" data-IdEstrutura="<%= this.IdEstrutura %>">

                    </ul>
                </div>
            </div>



            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Abas" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Abas</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <table id="lstEdicaoWidget_Abas_Itens" class="EdicaoDeWidget_ListaTabelada">
                            <thead>
                                <tr>
                                    <td style="width:35%">Título da Aba</td>
                                    <td style="width:15%">Tipo</td>
                                    <td style="width:50%">Conteúdo</td>
                                </tr>
                                <tr>
                                    <td style="width:35%">
                                        <input type="text" id="txtEdicaoWidget_Abas_NovoItem_Texto" style="width:90%;" />
                                    </td>
                                    <td style="width:15%">
                                        <select id="cboEdicaoWidget_Abas_NovoItem_Tipo" style="width:82%; float:left">
                                            <option value="Link">Link</option>
                                            <option value="Embutida">Embutida</option>
                                        </select>
                                    </td>
                                    <td style="width:50%">
                                        <select id="cboEdicaoWidget_Abas_NovoItem_Conteudo" style="width:86%; float:left">
                                            
                                            <option value="-1">&lt;Criar uma Nova Página&gt;</option>
                                            <asp:repeater id="rptPaginasParaAba" runat="server">
                                            <itemtemplate>
                                            <option value="<%# Eval("CodigoPagina") %>" data-URL="<%# Eval("DescURL") %>" data-NomePagina="<%# Eval("NomePagina") %>" title="<%# Eval("CodigoPagina") %> - <%# Eval("NomePagina") %>"><%# Eval("DescURL") %></option>
                                            </itemtemplate>
                                            </asp:repeater>

                                        </select>

                                        <button onclick="return btnEdicaoWidget_Abas_NovoItem_Adicionar_Click(this)" title="Adicionar" class="BotaoIcone IconeMais" style="float:left"><span>Adicionar</span></button>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="NenhumItem">
                                    <td colspan="3">Nenhum Item...</td>
                                </tr>
                            </tbody>
                        </table>

                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_Abas_AtributoClass">Classe:</label>
                            <select  id="cboEdicaoWidget_Abas_AtributoClass" onchange="Widget_Aba_Elemento_Change(this)">
                                <option value="menu-tabs">Padrão</option>
                                <option value="menu-tabs menu-tabs-full menu-tabs-3">Largura Máxima: 3 Itens</option>
                                <option value="menu-tabs menu-tabs-full menu-tabs-4">Largura Máxima: 4 Itens</option>
                                <option value="menu-tabs menu-tabs-full menu-tabs-5">Largura Máxima: 5 Itens</option>
                                <option value="menu-tabs menu-tabs-full menu-tabs-6">Largura Máxima: 6 Itens</option>
                                <option value="menu-tabs menu-tabs-full menu-tabs-7">Largura Máxima: 7 Itens</option>
                                <option value="menu-tabs menu-tabs-full menu-tabs-6-min">6 Itens, texto menor</option>
                                <option value="menu-tabs menu-tabs-full menu-tabs-7-min">7 Itens, texto menor</option>
                                <option value="abas-menu abas-simples">Aba Simples</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_Abas_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_Abas_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Aba_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>

                        <div id="pnlEstruturaContainer_PainelEdicaoWidget_Abas_NovaPagina" style="display:none">

                            <p>
                                <label for="txtEdicaoWidget_Abas_NovaPagina_Titulo">Título:</label>
                                <input  id="txtEdicaoWidget_Abas_NovaPagina_Titulo" type="text">
                            </p>
                            <p>
                                <label for="txtEdicaoWidget_Abas_NovaPagina_Url">Url:</label>
                                <input  id="txtEdicaoWidget_Abas_NovaPagina_Url" type="text">
                            </p>
                            <p>
                                <label for="cboEdicaoWidget_Abas_NovaPagina_Modo">Modo:</label>
                                <select id="cboEdicaoWidget_Abas_NovaPagina_Modo" style="width:75%">
                                    <option value="F">Página Única</option>
                                    <option value="D">Página Separada entre Visitantes e Clientes</option>
                                </select>
                            </p>

                            <p>
                                <button onclick="return btnEdicaoWidget_Abas_CancelarNovaPagina_Click()"  style="margin:1em 2em 0 220px;">Cancelar</button>
                                <button onclick="return btnEdicaoWidget_Abas_CriarNovaPagina_Click(this)" style="margin:1em 0em 0em 0em;" data-referencia="Abas">Criar Nova Página</button>
                            </p>

                        </div>

                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Abas_Salvar_Click(this)" >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Abas_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Texto</strong> preencher cada linha com o título da aba : id da página de conteúdo
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Acordeon" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Acordeon</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <table id="lstEdicaoWidget_Acordeon_Itens" class="EdicaoDeWidget_ListaTabelada">
                            <thead>
                                <tr>
                                    <td style="width:50%">Texto do Título</td>
                                    <td style="width:50%">Conteúdo</td>
                                </tr>
                                <tr>
                                    <td style="width:50%">
                                        <input type="text" id="txtEdicaoWidget_Acordeon_NovoItem_Texto" style="width:90%;" />
                                    </td>
                                    <td style="width:50%">
                                        <select id="cboEdicaoWidget_Acordeon_NovoItem_Conteudo" style="width:86%; float:left">
                                            
                                            <option value="-1">&lt;Criar uma Nova Página&gt;</option>
                                            <asp:repeater id="rptPaginasParaAcordeon" runat="server">
                                            <itemtemplate>
                                            <option value="<%# Eval("CodigoPagina") %>" data-URL="<%# Eval("DescURL") %>" data-NomePagina="<%# Eval("NomePagina") %>" title="<%# Eval("CodigoPagina") %> - <%# Eval("NomePagina") %>"><%# Eval("DescURL") %></option>
                                            </itemtemplate>
                                            </asp:repeater>

                                        </select>

                                        <button onclick="return btnEdicaoWidget_Acordeon_NovoItem_Adicionar_Click(this)" title="Adicionar" class="BotaoIcone IconeMais" style="float:left"><span>Adicionar</span></button>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="NenhumItem">
                                    <td colspan="3">Nenhum Item...</td>
                                </tr>
                            </tbody>
                        </table>

                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_Acordeon_AtributoClass">Classe:</label>
                            <select  id="cboEdicaoWidget_Acordeon_AtributoClass" onchange="Widget_Acordeon_Elemento_Change(this)">
                                <option value="acordeon">Seta</option>
                                <option value="acordeon-box">Aba Simples</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_Acordeon_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_Acordeon_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Acordeon_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>

                        <div id="pnlEstruturaContainer_PainelEdicaoWidget_Acordeon_NovaPagina" style="display:none">

                            <p>
                                <label for="txtEdicaoWidget_Acordeon_NovaPagina_Titulo">Título:</label>
                                <input  id="txtEdicaoWidget_Acordeon_NovaPagina_Titulo" type="text" />
                            </p>
                            <p>
                                <label for="txtEdicaoWidget_Acordeon_NovaPagina_Url">Url:</label>
                                <input  id="txtEdicaoWidget_Acordeon_NovaPagina_Url" type="text" />
                            </p>
                            <p>
                                <label for="cboEdicaoWidget_Acordeon_NovaPagina_Modo">Modo:</label>
                                <select id="cboEdicaoWidget_Acordeon_NovaPagina_Modo" style="width:75%" >
                                    <option value="F">Página Única</option>
                                    <option value="D">Página Separada entre Visitantes e Clientes</option>
                                </select>
                            </p>

                            <p>
                                <button onclick="return btnEdicaoWidget_Abas_CancelarNovaPagina_Click()"  style="margin:1em 2em 0 220px;">Cancelar</button>
                                <button onclick="return btnEdicaoWidget_Abas_CriarNovaPagina_Click(this)" style="margin:1em 0em 0em 0em;" data-referencia="Acordeon">Criar Nova Página</button>
                            </p>

                        </div>

                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Acordeon_Salvar_Click(this)" >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Acordeon_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Acordeon</strong> 
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Titulo" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Título</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            <label  for="cboEdicaoWidget_Titulo_Nivel">Nível:</label>
                            <select  id="cboEdicaoWidget_Titulo_Nivel" onchange="Widget_Titulo_Elemento_Change(this)">
                                <option value="1">Nível 1</option>
                                <option value="2">Nível 2</option>
                                <option value="3">Nível 3</option>
                                <option value="4">Nível 4</option>
                            </select>
                        </p>
                        <p>
                            <label  for="cboEdicaoWidget_Titulo_AtributoClass">Classe:</label>
                            <select  id="cboEdicaoWidget_Titulo_AtributoClass" onchange="Widget_Titulo_Elemento_Change(this)">
                                <option value="">Padrão</option>
                                <option value="Sublinhado">Sublinhado</option>
                            </select>
                        </p>
                        <p>
                            <label for="txtEdicaoWidget_Titulo_Texto">Texto:</label>
                            <input  id="txtEdicaoWidget_Titulo_Texto" type="text" onchange="Widget_Titulo_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                        <p>
                            <label for="txtEdicaoWidget_Titulo_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_Titulo_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Titulo_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Titulo_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Titulo_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Nível</strong> indica se o título é principal, secundário, terciário ou quaternário.
                        </p>
                        <p>
                            <strong>Texto</strong> suporta os atalhos de link [](href(:_blank opcional)|estilo:valor|NomeDaClasse), **negrito** e *itálico*
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Texto" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Texto</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            <textarea  id="txtEdicaoWidget_Texto_Texto" type="text" onchange="Widget_Texto_Elemento_Change(this)"></textarea>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_Texto_AtributoClass">Classe:</label>
                            <select  id="cboEdicaoWidget_Texto_AtributoClass" onchange="Widget_Texto_Elemento_Change(this)">
                                <option value="">Padrão</option>
                                <option value="SemFundo">Sem Fundo</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_Texto_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_Texto_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Texto_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_Form" style="display:none">
                        
                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Texto_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Texto_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Texto</strong> suporta os atalhos de link [](href(:_blank opcional)|estilo:valor|NomeDaClasse), **negrito** e *itálico*
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_TextoHTML" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Texto HTML</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            
                            <label for="txtEdicaoWidget_TextoHTML_ConteudoHTML_Preview">Conteúdo:</label>
                            <input  id="txtEdicaoWidget_TextoHTML_ConteudoHTML_Preview" type="text" onkeydown="return txtEdicaoConteudoDinamico_KeyDown(this, event)" onfocus="iptEdicaoDeHTML_Focus(this)"  value="&lt;html&gt;" />

                            <input type="hidden" data-propriedade="ConteudoHTML" id="txtEdicaoWidget_TextoHTML_ConteudoHTML" />
                        </p>
                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_TextoHTML_AtributoClass">Classe:</label>
                            <select  id="cboEdicaoWidget_TextoHTML_AtributoClass" onchange="Widget_Texto_Elemento_Change(this)">
                                <option value="">Padrão</option>
                                <option value="SemFundo">Sem Fundo</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_TextoHTML_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_TextoHTML_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Texto_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_Form" style="display:none">
                        
                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_TextoHTML_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_TextoHTML_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            Utilize o editor de HTML.
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Imagem" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Imagem</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            <label for="txtEdicaoWidget_Imagem_AtributoSrc">URL:</label>
                            <input  id="txtEdicaoWidget_Imagem_AtributoSrc" type="text" class="PickerDeArquivo" onchange="Widget_Imagem_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>

                        <p>
                            <label for="txtEdicaoWidget_Imagem_AtributoAlt" title="Texto Alternativo">Texto Alt.:</label>
                            <input  id="txtEdicaoWidget_Imagem_AtributoAlt" type="text" onchange="Widget_Imagem_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>

                        <p>
                            <label for="txtEdicaoWidget_Imagem_LinkPara">Link Para:</label>
                            <input  id="txtEdicaoWidget_Imagem_LinkPara" type="text" onchange="Widget_Imagem_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>

                        <p>
                            <label for="">Tamanho:</label>
                            <input  id="rdoEdicaoWidget_Imagem_Tamanho_Automatico" name="rdoEdicaoWidget_Imagem_Tamanho" type="radio" checked="checked" onchange="rdoEdicaoWidget_Imagem_Tamanho_Change(this)" style="width:1em" />
                            <label for="rdoEdicaoWidget_Imagem_Tamanho_Automatico" style="width:7em; text-align:left;">Automático</label>

                            <input  id="rdoEdicaoWidget_Imagem_Tamanho_Fixo" name="rdoEdicaoWidget_Imagem_Tamanho" type="radio" onchange="rdoEdicaoWidget_Imagem_Tamanho_Change(this)" style="width:1em" />
                            <label for="rdoEdicaoWidget_Imagem_Tamanho_Fixo" style="width:3em; text-align:left;">Fixo:</label>

                            <input  id="txtEdicaoWidget_Imagem_Tamanho_Fixo_Largura" type="text" onchange="Widget_Imagem_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" class="InputLargura" disabled="disabled" />
                            <input  id="txtEdicaoWidget_Imagem_Tamanho_Fixo_Altura"  type="text" onchange="Widget_Imagem_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" class="InputAltura"  disabled="disabled" />
                        </p>

                        <p>
                            <input  id="chkEdicaoWidget_Imagem_HabilitarZoom" type="checkbox" onchange="Widget_Imagem_Elemento_Change(this)" style="margin-left:42%; width:1em" disabled="disabled" />
                            <label for="chkEdicaoWidget_Imagem_HabilitarZoom" style="width:16em; text-align:left">Habilitar Zoom para essa imagem</label>
                        </p>

                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_Imagem_AtributoClass" style="width:31%">Classe:</label>
                            <select  id="cboEdicaoWidget_Imagem_AtributoClass" onchange="Widget_Imagem_Elemento_Change(this)">
                                <option value="">Padrão</option>
                                <option value="ComBorda">Com Borda</option>
                                <option value="ComSombra">Com Sombra</option>
                                <option value="ComBordaESombra">Com Borda e Sombra</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_Imagem_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_Imagem_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Imagem_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Imagem_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Imagem_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>URL</strong> é a fonte da imagem.
                        </p>
                        <p>
                            <strong>Texto Alt.</strong> é o texto que aparece quando o usuário fica com o mouse em cima da imagem
                        </p>
                        <p>
                            <strong>Link Para</strong> é a URL para navegar se o usuário clicar na imagem (opcional)
                        </p>
                        <p>
                            <strong>Tamanho</strong> quando automático, exibe o tamanho real da imagem; fixo é um outro tamanho fixo
                        </p>
                        <p>
                            <strong>Habilitar Zoom</strong> funciona quando for tamanho fixo, e o click da imagem a exibe no tamanho real
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Lista" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Lista</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            <label  for="cboEdicaoWidget_Lista_TipoDeLista" style="width:20.2%">Tipo de Lista:</label>
                            <select  id="cboEdicaoWidget_Lista_TipoDeLista" onchange="cboEdicaoWidget_Lista_TipoDeLista_Change(this)">
                                <option value="E" selected>Estática</option>
                                <option value="D">Dinâmica</option>
                            </select>
                        </p>

                        <p class="FormEstatica">
                            <textarea  id="txtEdicaoWidget_Lista_ItensEstaticos" wrap="off" type="text" onchange="Widget_Lista_Elemento_Change(this)" class="EdicaoMonospace"></textarea>
                        </p>

                        <p class="FormDinamica MetadeDoTamanho" style="display:none">

                            <label style="width:41%">Tipo de Conteúdo:</label>
                            <select id="cboEdicaoWidget_Lista_TipoDeConteudo" onchange="cboEdicaoWidget_Lista_TipoDeConteudo_Change(this)" style="width:20%">
                                <option value="">-Selecione-</option>

                            <asp:repeater id="rptTipoConteudo_WidgetLista" runat="server">
                            <itemtemplate>
                                <option value="<%#Eval("IdTipoConteudo") %>" data-TipoDeConteudoJson='<%#Eval("TipoDeConteudoJson") %>'><%#Eval("Descricao") %></option>
                            </itemtemplate>
                            </asp:repeater>

                            </select>
                        </p>

                        <p class="FormDinamica MetadeDoTamanho" style="display:none; height:1.8em">

                            <label for="cboEdicaoWidget_Lista_IdListaDinamica" style="width:20%">Lista:</label>
                            <select id="cboEdicaoWidget_Lista_IdListaDinamica" style="width:60%;display:none" onchange="Widget_Lista_Elemento_Change(this)">
                                <option value="">(...)</option>
                            </select>
                        </p>

                        <p class="FormDinamica MetadeDoTamanho" style="display:none">
                            <label for="cboEdicaoWidget_Lista_PropDisponiveis" style="width:41%">Propriedades:</label>
                            <select id="cboEdicaoWidget_Lista_PropDisponiveis" style="width:20%;display:none"></select>
                        </p>

                        <p class="FormDinamica MetadeDoTamanho" style="display:none">
                            <label for="cboEdicaoWidget_Lista_Ordenacao" style="width:20%">Ordenação:</label>
                            <select id="cboEdicaoWidget_Lista_Ordenacao" style="width:60%;display:none"></select>
                        </p>

                        <p class="FormDinamica" style="display:none">
                            <label for="txtEdicaoWidget_Lista_TemplateDoItem" style="width:20.2%">Template do Item:</label>
                            <input  id="txtEdicaoWidget_Lista_TemplateDoItem" title="" type="text" style="width:62.1%" onchange="Widget_Lista_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                            <button class="BotaoIcone IconeAtualizar" title="Atualizar Itens da Lista" onclick="return btnEdicaoWidget_Lista_Atualizar_Click(this)"><span>Atualizar Itens da Lista</span></button>
                        </p>
                        

                        <p class="FormDinamica" style="display:none">
                            <label for="txtEdicaoWidget_Lista_Atributos" style="width:20.2%">Atributos:</label>
                            <input  id="txtEdicaoWidget_Lista_Atributos" title="data-ElementoPai='id_do_elemento'" type="text" style="width:62.1%" onchange="Widget_Lista_Elemento_Change(this)"  />
                        </p>

                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_Lista_AtributoClass" style="width:41%;">Classe:</label>
                            <select  id="cboEdicaoWidget_Lista_AtributoClass" style="width:30%;" onchange="Widget_Lista_Elemento_Change(this)">
                                <option value="ListaPadrao">Padrão</option>
                                <option value="acordeon-box">Acordeon Box</option>
                                <option value="ListaCom2Colunas">Com 2 Colunas</option>
                                <option value="IndicadorNumerico">Indicador Numérico</option>
                                <option value="SemIndicador">Sem Indicador</option>
                                <option value="IconesPDF">Ícones de PDF</option>
                                <option value="IconesPDFLinha">Ícones de PDF em Linha</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_Lista_AtributoStyle" style="width:10%;">Style:</label>
                            <input  id="txtEdicaoWidget_Lista_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Lista_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>


                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Lista_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Lista_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Estática</strong> é uma lista com elementos definidos; <strong>Dinâmica</strong> é uma que vem dos conteúdos dinâmicos cadastrados.
                        </p>
                        <p>
                            <strong>Template do Item</strong> suporta os atalhos de link [](href(:_blank opcional)|estilo:valor|NomeDaClasse), **negrito** e *itálico*
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Tabela" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Tabela</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            <label for="txtEdicaoWidget_Tabela_Cabecalho" style="width:20.2%">Cabeçalho:</label>
                            <input  id="txtEdicaoWidget_Tabela_Cabecalho" title="Separar os campos com |" type="text" onchange="Widget_Tabela_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" style="width:70%" />
                        </p>
                        <p>
                            <label  for="cboEdicaoWidget_Tabela_TipoDeTabela" style="width:20.2%">Tipo de Tabela:</label>
                            <select  id="cboEdicaoWidget_Tabela_TipoDeTabela" onchange="cboEdicaoWidget_Tabela_TipoDeTabela_Change(this)">
                                <option value="E" selected>Estática</option>
                                <option value="D">Dinâmica</option>
                            </select>
                        </p>

                        <p class="FormEstatica">
                            <textarea  id="txtEdicaoWidget_Tabela_ItensEstaticos" wrap="off" type="text" onchange="Widget_Tabela_Elemento_Change(this)" class="EdicaoMonospace"></textarea>
                        </p>
                        
                        <p class="FormDinamica MetadeDoTamanho" style="display:none">

                            <label style="width:41%">Tipo de Conteúdo:</label>
                            <select id="cboEdicaoWidget_Tabela_TipoDeConteudo" onchange="cboEdicaoWidget_Tabela_TipoDeConteudo_Change(this)" style="width:20%">
                                <option value="">-Selecione-</option>

                            <asp:repeater id="rptTipoConteudo_WidgetTabela" runat="server">
                            <itemtemplate>
                                <option value="<%#Eval("IdTipoConteudo") %>" data-TipoDeConteudoJson='<%#Eval("TipoDeConteudoJson") %>'><%#Eval("Descricao") %></option>
                            </itemtemplate>
                            </asp:repeater>

                            </select>
                        </p>

                        <p class="FormDinamica MetadeDoTamanho" style="display:none;height:1.8em">

                            <label for="cboEdicaoWidget_Tabela_IdListaDinamica" style="width:20%">Lista:</label>
                            <select id="cboEdicaoWidget_Tabela_IdListaDinamica" style="width:60%;display:none" onchange="Widget_Tabela_Elemento_Change(this)">
                                <option value="">(...)</option>
                            </select>
                        </p>

                        <p class="FormDinamica MetadeDoTamanho" style="display:none">
                            <label for="cboEdicaoWidget_Tabela_PropDisponiveis" style="width:41%">Propriedades:</label>
                            <select id="cboEdicaoWidget_Tabela_PropDisponiveis" style="width:20%; display:none"></select>
                        </p>

                        <p class="FormDinamica MetadeDoTamanho" style="display:none">
                            <label for="cboEdicaoWidget_Tabela_Ordenacao" style="width:20%">Ordenação:</label>
                            <select id="cboEdicaoWidget_Tabela_Ordenacao" style="width:60%;display:none"></select>
                        </p>

                        <p class="FormDinamica" style="display:none">
                            <label for="txtEdicaoWidget_Tabela_TemplateDaLinha" style="width:20.2%">Template da Linha:</label>
                            <input  id="txtEdicaoWidget_Tabela_TemplateDaLinha" title="" type="text" style="width:62.1%" onchange="Widget_Tabela_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                            <button class="BotaoIcone IconeAtualizar" title="Atualizar Itens da Lista" onclick="return btnEdicaoWidget_Tabela_Atualizar_Click(this)"><span>Atualizar Itens da Lista</span></button>
                        </p>
                        
                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_Tabela_AtributoClass">Classe:</label>
                            <select  id="cboEdicaoWidget_Tabela_AtributoClass" onchange="Widget_Tabela_Elemento_Change(this)">
                                <option value="">Padrão</option>
                                <option value="CabecalhoPorLinha1">Destaque por Linha, à Esquerda</option>
                                <option value="CabecalhoPorLinha2">Destaque por Linha, à Direita</option>
                                <option value="CabecalhoCentralizado">Cabeçalho Centralizado</option>
                                <option value="CorpoCentralizado">Corpo Centralizado</option>
                                <option value="TudoCentralizado">Tudo Centralizado</option>
                                <option value="CabecalhoADireita">Cabeçalho à Direita</option>
                                <option value="CorpoADireita">Corpo à Direita</option>
                                <option value="TudoADireita">Tudo à Direita</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_Tabela_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_Tabela_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Tabela_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>


                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Tabela_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Tabela_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Cabeçalho</strong> são os títulos dos campos, separados por "|" (pode ter espaço: Nome | Idade)
                        </p>
                        <p>
                            Para <strong>Tabela Estática</strong>, colocar cada linha com os valores separados por "|" (Rafael | 28).<br />
                            Utilizar &gt; para ocupar colunas à direita, e duplo underscore "__" para uma célula ficar vazia.<br />
                            Colocar (style:__valores__) ou (classe:_nome_da_classe_) no final do texto da célula para adicionar style ou classe (não suporta ambos; a classe que faz a célula parecer um cabeçalho é "Cabecalho")
                        </p>
                        <p>
                            Para <strong>Tabela Dinâmica</strong>, colocar as propriedades separadas por "|" ($[Nome] | $[Idade])
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Repetidor" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget Repetidor</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p class="MetadeDoTamanho">

                            <label style="width:41%">Tipo de Conteúdo:</label>
                            <select id="cboEdicaoWidget_Repetidor_TipoDeConteudo" onchange="cboEdicaoWidget_Repetidor_TipoDeConteudo_Change(this)" style="width:20%">
                                <option value="">-Selecione-</option>

                            <asp:repeater id="rptTipoConteudo_WidgetRepetidor" runat="server">
                            <itemtemplate>
                                <option value="<%#Eval("IdTipoConteudo") %>" data-TipoDeConteudoJson='<%#Eval("TipoDeConteudoJson") %>'><%#Eval("Descricao") %></option>
                            </itemtemplate>
                            </asp:repeater>

                            </select>
                        </p>

                        <p class="MetadeDoTamanho" style="height:1.8em">

                            <label for="cboEdicaoWidget_Repetidor_IdListaDinamica" style="width:20%">Lista:</label>
                            <select id="cboEdicaoWidget_Repetidor_IdListaDinamica" style="width:60%;display:none" onchange="Widget_Repetidor_Elemento_Change(this)">
                                <option value="">(...)</option>
                            </select>
                        </p>
                        
                        <p class="MetadeDoTamanho">
                            <label for="cboEdicaoWidget_Repetidor_PropDisponiveis" style="width:41%">Propriedades:</label>
                            <select id="cboEdicaoWidget_Repetidor_PropDisponiveis" style="width:20%;display:none"></select>
                        </p>
                        
                        <p class="MetadeDoTamanho">
                            <label for="cboEdicaoWidget_Repetidor_Ordenacao" style="width:20%">Ordenação:</label>
                            <select id="cboEdicaoWidget_Repetidor_Ordenacao" style="width:60%;display:none"></select>
                        </p>

                        <p>
                            <label for="txtEdicaoWidget_Repetidor_TemplateDoItem" style="width:20.2%">Template do Item:</label>
                            <textarea id="txtEdicaoWidget_Repetidor_TemplateDoItem" style="width:72%;margin-left:0px" class="EdicaoDeCodigo" onchange="Widget_Repetidor_Elemento_Change(this)"></textarea>

                            <button class="BotaoIcone IconeAtualizar" title="Atualizar Itens da Lista" onclick="return btnEdicaoWidget_Repetidor_Atualizar_Click(this)" style="float:right;margin-right:8px"><span>Atualizar Itens da Lista</span></button>
                        </p>

                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_Repetidor_AtributoClass" style="width:41%;">Classe:</label>
                            <select  id="cboEdicaoWidget_Repetidor_AtributoClass" style="width:30%;" onchange="Widget_Repetidor_Elemento_Change(this)">
                                <option value="">Padrão</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_Repetidor_AtributoStyle" style="width:10%;">Style:</label>
                            <input  id="txtEdicaoWidget_Repetidor_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Repetidor_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>


                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Repetidor_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Repetidor_Cancelar_Click(this)">Cancelar</button>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_ListaDeDefinicao" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget Lista de Definição</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p class="MetadeDoTamanho">

                            <label style="width:41%">Tipo de Conteúdo:</label>
                            <select id="cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo" onchange="cboEdicaoWidget_ListaDeDefinicao_TipoDeConteudo_Change(this)" style="width:20%">
                                <option value="">-Selecione-</option>

                            <asp:repeater id="rptTipoConteudo_WidgetListaDeDefinicao" runat="server">
                            <itemtemplate>
                                <option value="<%#Eval("IdTipoConteudo") %>" data-TipoDeConteudoJson='<%#Eval("TipoDeConteudoJson") %>'><%#Eval("Descricao") %></option>
                            </itemtemplate>
                            </asp:repeater>

                            </select>
                        </p>

                        <p class="MetadeDoTamanho" style="height:1.8em">

                            <label for="cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica" style="width:20%">Lista:</label>
                            <select id="cboEdicaoWidget_ListaDeDefinicao_IdListaDinamica" style="display:none; width:60%" onchange="Widget_ListaDeDefinicao_Elemento_Change(this)">
                                <option value="">(...)</option>
                            </select>

                        </p>

                        <p class="MetadeDoTamanho">

                            <label for="cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis" style="width:41%">Propriedades:</label>
                            <select id="cboEdicaoWidget_ListaDeDefinicao_PropDisponiveis" style="width:20%; display:none"></select>

                        </p>
                        <p class="MetadeDoTamanho">

                            <label for="cboEdicaoWidget_ListaDeDefinicao_Ordenacao" style="width:20%">Ordenação:</label>
                            <select id="cboEdicaoWidget_ListaDeDefinicao_Ordenacao" style="width:60%; display:none"></select>

                        </p>

                        <p>
                            <label for="txtEdicaoWidget_ListaDeDefinicao_TemplateDoItem" style="width:20.2%">Template do Item:</label>
                            <textarea id="txtEdicaoWidget_ListaDeDefinicao_TemplateDoItem" style="width:72%;margin-left:0px" class="EdicaoDeCodigo" onchange="Widget_ListaDeDefinicao_Elemento_Change(this)"></textarea>

                            <button class="BotaoIcone IconeAtualizar" title="Atualizar Itens da Lista" onclick="return btnEdicaoWidget_ListaDeDefinicao_Atualizar_Click(this)" style="float:right;margin-right:8px"><span>Atualizar Itens da Lista</span></button>
                        </p>

                        <p class="MetadeDoTamanho">
                            <label  for="cboEdicaoWidget_ListaDeDefinicao_AtributoClass" style="width:41%;">Classe:</label>
                            <select  id="cboEdicaoWidget_ListaDeDefinicao_AtributoClass" style="width:30%;" onchange="Widget_ListaDeDefinicao_Elemento_Change(this)">
                                <option value="">Padrão</option>
                                <option value="ItensExpansiveis">Itens Expansíveis</option>
                                <option value="Acordeao">Acordeão</option>
                            </select>
                        </p>
                        <p class="MetadeDoTamanho">
                            <label for="txtEdicaoWidget_ListaDeDefinicao_AtributoStyle" style="width:10%;">Style:</label>
                            <input  id="txtEdicaoWidget_ListaDeDefinicao_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_ListaDeDefinicao_Elemento_Change(this)" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>
                    </div>


                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_ListaDeDefinicao_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_ListaDeDefinicao_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Template do Item</strong> Separar por "|" a definição de sua explicação (Qual o nome do sofware de negociação da Gradual? | Gradual Trader Interface)
                        </p>
                    </div>
                </div>

            </div>




            <div id="pnlEstruturaContainer_PainelEdicaoWidget_Embed" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades do Widget de Código</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            <textarea  id="txtEdicaoWidget_Embed_Codigo" type="text" onchange="Widget_Embed_Elemento_Change(this)"></textarea>
                        </p>
                        <p>
                            <label for="txtEdicaoWidget_Embed_AtributoStyle">Style:</label>
                            <input  id="txtEdicaoWidget_Embed_AtributoStyle" title="atributo:valor; atributo:valor" type="text" onchange="Widget_Embed_Elemento_Change(this)" />
                        </p>

                    </div>

                    <div class="ContainerCMS_PainelComTabs_Form" style="display:none">
                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoWidget_Embed_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoWidget_Embed_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoWidget_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Código</strong> código HTML para adicionar; para YouTube, ir no vídeo, opção "share" aba "embed"
                        </p>
                    </div>
                </div>

            </div>



        </div>

        <div id="pnlConteudoDinamicoContainer" class="ContainerCMS" style="display:none">

            <div class="ContainerCMS_Painel">

                <h4>Conteúdo Dinâmico</h4>

                <p>
                    <label for="cboCMS_ConteudoDinamico_SelecionarObjeto">Tipo:</label>
                    <select id="cboCMS_ConteudoDinamico_SelecionarObjeto">

                    <asp:repeater id="rptTipoConteudo" runat="server">
                    <itemtemplate>
                        <option value="<%#Eval("IdTipoConteudo") %>" data-TipoDeConteudoJson='<%#Eval("TipoDeConteudoJson") %>'><%#Eval("Descricao") %></option>
                    </itemtemplate>
                    </asp:repeater>

                    </select>

                    <button class="BotaoIcone IconeGoItens" title="Carregar Itens" onclick="return btnCMS_ConteudoDinamico_CarregarItens_Click(this)"><span>Carregar Itens</span></button>

                    <button class="BotaoIcone IconeGoLista" title="Carregar Listas" onclick="return btnCMS_ConteudoDinamico_CarregarListas_Click(this)"><span>Carregar Listas</span></button>

                    <button class="BotaoIcone IconeMais" title="Adicionar" onclick="return btnCMS_ConteudoDinamico_Adicionar_Click(this)"><span>Adicionar</span></button>
                </p>

                
                <div class="ContainerListaItens">
                    <ul id="lstConteudoDinamico" class="ListaDeItens SomenteRemover" style="height:264px">


                    </ul>
                </div>

            </div>

            <div class="ContainerCMS_Painel" style="display:none">

                <h4 id="lblContainerCMS_Titulo">Ofertas Públicas</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div id="pnlContainerCMS_Form_Itens" class="ContainerCMS_PainelComTabs_Form">
                        
                    </div>

                    <div id="pnlContainerCMS_Form_Listas" class="ContainerCMS_PainelComTabs_Form" style="display:none">

                        <p>
                            <label for="txtEdicaoLista_Descricao">Descrição:</label>
                            <input  id="txtEdicaoLista_Descricao" type="text" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />
                        </p>

                        <p>
                            <label for="txtEdicaoLista_Regra">Regra:</label>
                            <input  id="txtEdicaoLista_Regra" type="text" onkeydown="return txtEdicaoWidget_KeyDown(this, event)" />

                            <button onclick="return btnEdicaoWidget_Lista_Verificar_Click(this)" title="Atualizar Itens da Lista" class="BotaoIcone IconeAtualizar"><span>Verificar Itens da Lista</span></button>
                        </p>
                        
                        <p class="DicaListas">
                            Todos()<br />
                            ValorEntreDataInicialFinal($[DataHoje])<br />
                            PropriedadeIgual($[FlagPublicado], $[S])<br />
                            PropriedadeDiferente($[FlagPublicado], $[S])<br />
                        </p>

                        <div class="pnlContainerCMS_Form_Listas_TabelaDeTeste" style="display:none; float: left; margin: 4px; width: 577px; height: 180px; overflow: scroll;">

                            <table id="tblEdicaoLista_Regra_ResultadosDoTeste" style="background:#fff">
                                <thead><tr><td></td></tr></thead>
                                <tbody></tbody>
                            </table>

                        </div>

                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoConteudoDinamico_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoConteudoDinamico_Cancelar_Click(this)">Cancelar</button>
                    </div>

                </div>

            </div>

        </div>


        <div id="pnlPaginasContainer" class="ContainerCMS" style="display:none">

            <div class="ContainerCMS_Painel">

                <h4>Páginas</h4>

                <p>
                    <label for="cboCMS_Pagina_SelecionarObjeto" style="width:92%">Nova</label>

                    <button class="BotaoIcone IconeMais" title="Adicionar" onclick="return btnCMS_Estrutura_AdicionarPagina_Click(this)"><span>Adicionar</span></button>
                </p>

                <div class="ContainerListaItens">

                    <ul id="lstPaginas" class="ListaDeItens RemoverVisitar">

                    <asp:repeater id="rptPaginas" runat="server">
                    <itemtemplate>

                        <li data-IdPagina="<%# Eval("CodigoPagina") %>" data-Titulo="<%# Eval("NomePagina") %>" data-Galho="<%# Eval("Galho") %>" data-GalhoSub="<%# Eval("Galho").ToString().ToLower() %>" data-Url="<%# Eval("DescURL") %>" data-TipoEstrutura="<%# Eval("TipoEstrutura") %>">
                            <label title="<%# Eval("CodigoPagina") %> - <%# Eval("NomePagina") %>" onclick="lstPaginas_LI_Click(this)"><%# Eval("DescURL") %></label>
                            <div>
                                <button title="Visitar" onclick="return lstPaginas_btnVisitar_Click(this)">  <span>Visitar</span>   </button>
                                <button title="Remover" onclick="return lstPaginas_btnRemover_Click(this)">  <span>Remover</span>   </button>
                            </div>
                        </li>

                    </itemtemplate>
                    </asp:repeater>

                    </ul>
                </div>

                <input type="hidden" id="hidListaDePastas" runat="server" />

            </div>
            
            <div id="pnlPaginasContainer_PainelEdicao" class="ContainerCMS_Painel" style="display:none">

                <h4>Propriedades da Página</h4>

                <div class="ContainerCMS_PainelComTabs">

                    <div id="pnlEdicaoDePaginas_Dados" class="ContainerCMS_PainelComTabs_Form">

                        <p>
                            <input type="hidden" id="hidEdicaoPagina_IdDaPagina" />

                            <label  for="txtEdicaoPagina_Titulo">Título:</label>
                            <input   id="txtEdicaoPagina_Titulo" type="text" onkeyup="txtEdicaoPagina_Titulo_KeyUp(this)" onblur="txtEdicaoPagina_Titulo_KeyUp(this)" />
                        </p>
                        <p>
                            <label  for="txtEdicaoPagina_Url">Url:</label>
                            <label   id="lblEdicaoPagina_Url" style="width:auto"></label>
                            <input   id="txtEdicaoPagina_Url" type="text" />
                        </p>
                        <p>
                            <label  for="cboEdicaoPagina_Modo">Modo:</label>
                            <select  id="cboEdicaoPagina_Modo" style="width:75%" onchange="cboEdicaoWidget_Abas_NovaPagina_Modo_Change(this)">
                                <option value="F">Página Única</option>
                                <option value="D">Página Separada entre Visitantes e Clientes</option>
                            </select>
                        </p>
                        <p id="pnlEdicaoDePaginas_Dados_Versao">
                            <label  for="cboEdicaoPagina_Versao">Versão:</label>
                            <select  id="cboEdicaoPagina_Versao" style="width:75%">
                                <asp:repeater id="rptEdicaoPagina_Versao" runat="server">
                                <itemtemplate>
                                <option value="<%# Container.DataItem.ToString() %>"><%# Container.DataItem.ToString() %></option>
                                </itemtemplate>
                                </asp:repeater>
                            </select>
                        </p>
                        
                        <p id="pnlEdicaoDePaginas_Dados_Manter" style="display:none" title="Estrutura que permanecerá ativa como 'todos'">
                            <label for="cboEdicaoDePaginas_Dados_Manter">Manter:</label>
                            <select id="cboEdicaoDePaginas_Dados_Manter" style="width:75%">
                                <option value="2">Visitantes</option>
                                <option value="3">Clientes</option>
                            </select>
                        </p>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_BotoesSalvar">
                        <button class="BotaoSalvar"   onclick="return btnEdicaoPagina_Salvar_Click(this)"  >Salvar</button>
                        <button class="BotaoCancelar" onclick="return btnEdicaoPagina_Cancelar_Click(this)">Cancelar</button>
                        <button class="BotaoAjuda"    onclick="return btnEdicaoPagina_Ajuda_Click(this)">Ajuda</button>
                    </div>

                    <div class="ContainerCMS_PainelComTabs_PainelAjuda" style="display:none">
                        <p>
                            <strong>Título</strong> é o título da página.
                        </p>
                        <p>
                            <strong>Url</strong> é o endereço a partir de gradualinvestimentos.com.br/(...)
                        </p>
                    </div>
                </div>

            </div>


        </div>

        <div id="pnlArquivosContainer" class="ContainerCMS" style="display:none">

            <div class="ContainerCMS_Painel" style="width:100%">

                <h4>Arquivos</h4>

                <p>
                    <label style="width:4em">Upload:</label>
                    <input id="txtGerenciadorDeArquivos_Upload" type="file" onfocus="txtGerenciadorDeArquivos_Input_Focus(this)" style="width:88px; padding:0px; border:none" />

                    <label style="width:9em">Filtrar por Tipo:</label>
                    <select id="cboGerenciadorDeArquivos_FiltroPorTipo" onfocus="cboGerenciadorDeArquivos_FiltroPorTipo_Focus(this)">
                        <option value="">[Todos]</option>
                        <option value="zip">Arquivos ZIP</option>
                        <option value="documento">Documentos do Office</option>
                        <option value="imagem">Imagens</option>
                        <option value="pdf">PDFs</option>
                        <option value="planilha">Planilhas</option>
                    </select>

                    <label style="width:9em">por Diretório:</label>
                    <select id="cboGerenciadorDeArquivos_FiltroPorDiretorio">
                    
                        <option value="">[Todos]</option>
                        <asp:repeater id="rptDiretorios" runat="server">
                        <itemtemplate>
                        <option value="<%# Container.DataItem.ToString().Replace("\\", "_") %>"><%# Container.DataItem %></option>
                        </itemtemplate>
                        </asp:repeater>

                    </select>

                    <label style="width:7em">por Texto:</label>
                    <input id="txtGerenciadorDeArquivos_Filtro" type="text" onfocus="txtGerenciadorDeArquivos_Input_Focus(this)" />
                    <button onclick="return btnGerenciadorDeArquivos_Filtrar_Click(this)" title="Filtrar" class="BotaoIcone IconeFiltro"><span>Filtrar</span></button>
                    <button onclick="return btnGerenciadorDeArquivos_LimparFiltro_Click(this)" title="Limpar Filtro" class="BotaoIcone IconeLimparFiltro"><span>Limpar Filtro</span></button>

                    <button onclick="return btnGerenciadorDeArquivos_AlterarVisualizacao_Click(this, 0)" title="Exibir com Miniaturas" class="BotaoIcone IconeExibirMiniaturas"><span>Exibir com Miniaturas</span></button>
                    <button onclick="return btnGerenciadorDeArquivos_AlterarVisualizacao_Click(this, 1)" title="Exibir Detalhes"       class="BotaoIcone IconeExibirDetalhes"><span>Exibir Detalhes</span></button>
                </p>

                <ul id="pnlArquivosContainer_lstImagens" class="Detalhes">

                    <asp:repeater id="rptImagens" runat="server">
                    <itemtemplate>
                        <li title="<%#Eval("NomeDoArquivo") %>"  data-Diretorio="<%#Eval("Diretorio").ToString().Replace("\\", "_") %>" data-Tipo="<%#Eval("Tipo") %>" onClick="return lstImagens_LI_Click(this)" onmouseover="lstImagens_LI_MouseOver(this)">
                            <img src="<%#Eval("URLDoThumbnail") %>" alt="<%#Eval("NomeDoArquivo") %>" />
                            <button title="Excluir Arquivo" class="BotaoIcone IconeLixeira" onclick="return btnImagens_Excluir_Click(this)"><span>Excluir Arquivo</span></button>
                            <button title="Copiar Link"     class="BotaoIcone IconeCopiarLink" onclick="return btnImagens_CopiarLink_Click(this)"><span>Copiar Link</span></button>
                            <input type="text" readonly value="<%#Eval("URL") %>" />
                        </li>
                    </itemtemplate>
                    </asp:repeater>

                </ul>

                <div id="pnlArquivosContainer_PreviewImagem" onclick="return pnlArquivosContainer_PreviewImagem_Click(this)" style="display:none">
                    <img alt="Placeholder" />
                </div>

            </div>

        </div>

        <div id="pnlInformacoesDaPaginaContainer" class="ContainerCMS" style="display:none">

            <div class="ContainerCMS_Painel">

                <h4>Informações da Página</h4>

                <div class="pnlInformacoesDaPagina">

                    <p>
                        <label>ID da Página:</label>
                        <span   id="lblInfoPag_IdPagina"></span>
                    </p>
                    <p>
                        <label style="padding-top:5px">ID da Estrutura:</label>
                        <span  style="float:left;margin-top:5px" id="lblInfoPag_Idestrutura"></span>

                        <button class="btn-form" onclick="return ModuloCMS_RecarregarCache()" style="float:right">Recarregar Cache</button>
                    </p>
                    <p>
                        <label>Widgets:</label>
                        <span id="lblInfoPag_Widgets"></span>
                    </p>

                </div>

            </div>

            <div class="ContainerCMS_Painel">
                <div class="pnlInformacoesDaPagina" style="padding-top:19px;">

                    <p>
                        <label>Versão:</label>
                        <span id="lblInfoPag_Versao"> <asp:literal id="lblInfoPagina_Versao" runat="server"></asp:literal> </span>

                        <img id="imgVersaoPublicada" src="~/Resc/Skin/Default/Img/v.png" runat="server" title="Versão Publicada" />

                        <button id="btnInfoPagina_Publicar" runat="server" class="btn-form" style="width:120px" onclick="return btnInfoPagina_Publicar_Click(this)">Publicar</button>

                    </p>
                    <p>
                        <label style="padding-top:5px">Versões Disponíveis:</label>
                        <select id="cboInfoPag_VersoesDisponiveis" style="width:180px; margin:5px 10px 0px 0px">
                            <asp:repeater id="rptInfoPagina_Versao" runat="server">
                            <itemtemplate>
                            <option value="<%# Container.DataItem.ToString() %>"><%# Container.DataItem.ToString() %></option>
                            </itemtemplate>
                            </asp:repeater>
                        </select>
                        <button class="btn-form" style="width:120px" onclick="return btnInfoPagina_Visualizar_Click(this)">Visualizar</button>
                    </p>
                    <p>
                        <label style="padding-top:5px">Nova Versão:</label>
                        <input type="text" id="txtInfoPag_NovaVersao" maxlength="20"  style="width:180px; margin:5px 10px 0px 0px; border:1px solid #b4ada7" />
                        <button class="btn-form" style="width:120px" onclick="return btnInfoPagina_CriarVersao_Click(this)">Criar Versão</button>
                    </p>

                </div>
            </div>

        </div>

    </div>
    
    <div id="pnlEdicaoDeHTML" style="display:none">

        <div class="BarraDeFerramentas">

            <button class="BotaoArquivos" onclick="return btnEdicaoDeHTML_PainelDeArquivos_Click(this)">Painel de Arquivos...</button>

            <label class="FakeComboBotaoDeExpansao" onclick="FakeComboBotaoDeExpansao_Click(this)">Estilos Disponíveis</label>

            <ul class="FakeComboParaCopiar" style="display:none">
                <li class="SubTitulo">Para Títulos (&lt;h1, h2, h3&gt;):</li>
                <li>SemFundo</li>
                <li class="SubTitulo">Para Parágrafos (&lt;p&gt;):</li>
                <li>ChamadaDeOfertaPublica</li>
                <li class="SubTitulo">Para Imagens (&lt;img&gt;):</li>
                <li>SemBordaSemSombra</li>
                <li>SomenteSombra</li>
                <li class="SubTitulo">Para Listas (&lt;ul, ol&gt;):</li>
                <li>ListaCom2Colunas</li>
                <li>IndicadorNumerico</li>
                <li>SemIndicador</li>
                <li>IconesPDF</li>
            </ul>

            <label class="FakeComboBotaoDeExpansao" onclick="btnEdicaoDeHTML_InserirTabela_Click(this)" style="background-position:73px -573px">Inserir Tabela</label>

            <div id="pnlEdicaoDeHTML_InserirTabela" style="display:none">
                <p>
                    <label for="txtEdicaoDeHTML_InserirTabela_Cabecalho">Cabeçalho:</label>
                    <input  id="txtEdicaoDeHTML_InserirTabela_Cabecalho" type="text" style="width:265px" onblur="txtEdicaoDeHTML_InserirTabela_Cabecalho_Blur()" title="Separe os campos do cabeçalho com '|': (Nome | Sobrenome | Data de Contratação)" />
                </p>
                <p>
                    <label for="txtEdicaoDeHTML_InserirTabela_Linhas">Linhas:</label>
                    <input  id="txtEdicaoDeHTML_InserirTabela_Linhas" type="text" class="Numerico" />

                    <label for="txtEdicaoDeHTML_InserirTabela_Colunas">Colunas:</label>
                    <input  id="txtEdicaoDeHTML_InserirTabela_Colunas" type="text" class="Numerico" />
                </p>
                <p style="text-align:center;margin-top:12px">
                    <button class="BotaoVermelho" onclick="return btnEdicaoDeHTML_InserirTabela_Cancelar_Click(this)" style="margin-right:42px;">Cancelar</button>
                    <button class="BotaoVerde"    onclick="return btnEdicaoDeHTML_InserirTabela_Ok_Click(this)"       style="margin-left:44px;">Ok</button>
                </p>
            </div>

        </div>

        <textarea id="txtEdicaoDeHTML"></textarea>

        <p>
            <button onclick="return btnEdicaoDeHTML_Cancelar_Click(this)">Cancelar</button>
            <button onclick="return btnEdicaoDeHTML_Salvar_Click(this)">OK</button>
        </p>

    </div>

    <input type="hidden" id="hidTipoDeUsuarioLogado" value="<%= this.PaginaBase.TipoDeUsuarioLogado %>" />

</div>