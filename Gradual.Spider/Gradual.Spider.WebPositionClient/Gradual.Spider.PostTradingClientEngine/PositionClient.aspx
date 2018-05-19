<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PositionClient.aspx.cs" Inherits="Gradual.Spider.PostTradingClientEngine.PositionClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--Includes Css--%>
    <link href="Skins/css/bootstrap.css"                        rel="stylesheet" />
    <link href="Skins/css/Principal.css"                        rel="stylesheet" />
    <link href="Skins/ui-darkness/jquery-ui-1.9.2.custom.css"   rel="stylesheet" />
    <link href="Skins/css/custom.css"                           rel="stylesheet" />
    <link href="/Scripts/jqGrid/src/css/ui.jqgrid.css"          rel="stylesheet" />
    <link href="Skins/css/Positionclient.css"                   rel="stylesheet" />

    <%--Includes Javascript--%>
    <script type="text/javascript" src="Scripts/jquery-2.1.4.min.js"                                ></script>
    <script type="text/javascript" src="/Scripts/jqGrid/src/i18n/grid.locale-pt-br.js"              ></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.9.2.custom/js/jquery-ui-1.9.2.custom.js"></script>
    <script type="text/javascript" src="/Scripts/jqGrid/src/jquery.jqGrid.js"                       ></script>

    <script type="text/javascript" src="/Scripts/PositionClient/01-PositionClientBase.js"           ></script>
    <script type="text/javascript" src="/Scripts/PositionClient/02-OperacoesIntraday.js"            ></script>
    <script type="text/javascript" src="/Scripts/PositionClient/03-RiscoResumido.js"                ></script>
    <script type="text/javascript" src="/Scripts/PositionClient/04-Acompanhamento.js"               ></script>
    <script type="text/javascript" src="/Scripts/PositionClient/05-Vendasdescobertas.js"            ></script>
    <%--<script type="text/javascript" src="/Scripts/PositionClient/GridWorker.js"                      ></script>--%>

    <script type="text/javascript" src="Scripts/jqGridExportToExcel.js"                             ></script>
    <script type="text/javascript" src="Scripts/jquery.maskedinput-1.2.2.js"                        ></script>

    <title>Position Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="hddConnectionSocketOperacoesIntraday"/>
    <input type="hidden" runat="server" id="hddConnectionSocketRiscoResumido"/>
    <input type="hidden" runat="server" id="hddConnectionRESTRiscoResumido"/>
    <input type="hidden" runat="server" id="hddConnectionRESTOperacoesIntraday"/>

    <div class="row">
        <div class="col-md-11">
            <div class="row cabecalho-cliente">
                <div class="col-md-11 titulo">
                    <label>Controle Risco</label>
                    <span class="lblMensagem" ><label id="lblMensagem"></label></span>
                    <span class="lblStatusConnection"><label id="lblStatusConnection"></label></span>
                </div>
            </div>
            <div id="nav-tabs" style="font-size:11px;">
                <ul  style="margin: 0px 12px 0px 12px;">
                    <li id="liOperacoesIntraday"  runat="server" ><a href="#pnlOperacoesIntraday"  >Operações Intraday</a></li>
                    <li id="liRiscoResumido" runat="server"><a href="#pnlRiscoResumido">Risco Resumido</a></li>
                    <li id="liAcompanhamento" runat="server"><a href="#divAcompanhamento">Acompanhamento de Ordens</a></li>
                    <li id="liVendasDescobertas" runat="server"><a href="#divVendasDescobertas">Vendas Descobertas</a></li>
                </ul>

                <div id="pnlOperacoesIntraday" runat="server">
                    <input type="hidden" id="hddOperacoes_Intraday_Seleted_Row" />
                    <fieldset class="filter-border">
                        <legend class="filter-border">Filtros</legend>
                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                <label for="txtOperacoesIntradayCliente">Cliente:</label> 
                                <input type="text" id="txtOperacoesIntradayCliente" onkeydown="return txtOperacoesIntraday_Cliente_KeyDown(this)" class="ui-widget ui-widget-content ui-corner-all SearchFor"  />
                                </div>
                                <div class="form-group">
                                <label for="txtOperacoesIntradayAtivo">Ativo:&nbsp; &nbsp;</label>
                                <input type="text" id="txtOperacoesIntradayAtivo" onkeydown="return txtOperacoesIntraday_Ativo_KeyDown(this)" class="ui-widget ui-widget-content ui-corner-all SearchFor" style="text-transform:uppercase"  />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <%--FieldSet Market--%>
                                <fieldset class="filter-border-small">
                            <legend class="filter-border-small">Market</legend>
                                <div class="checkbox">
                                    <label>
                                         <input type="checkbox" id="chkOperacoesIntradayTodosMercados" />Todos os Mercados
                                    </label>
                                </div>
                                <div class="checkbox">
                                    <label class="checkbox-inline"><input type="checkbox" id="chkOperacoesIntradayAvista" />À Vista</label>
                                    <label class="checkbox-inline"><input type="checkbox" id="chkOperacoesIntradayFuturo" />Futuro</label>
                                </div>
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" id="chkOperacoesIntradayOpcoes" />Opções
                                    </label>
                                </div>
                                <div class="pull-right">
                                   <a href="#" onclick="return OperacoesIntradayRemoveFiltersMarket()"> Remover Filtro </a>
                                </div>
                            </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <%--FieldSet Parametros do Intraday--%>
                                <fieldset class="filter-border-small">
                            <legend class="filter-border-small">Parametros do Intraday</legend>
                                <div class="checkbox form-inline">
                                    <label>
                                    <input type="checkbox" id="chkOperacoesIntradayOfertasPedra" />Ofertas na Pedra
                                    </label>
                                    <label class="align-to-right">*net book > 0</label>
                                </div>
                                <div class="checkbox">
                                    <label>
                                    <input type="checkbox" id="chkOperacoesIntradayNetIntradiarioNegativo" />Net Intradiário Negativo
                                    </label>
                                    <label class="align-to-right">*net Qtde < 0</label>
                                </div>
                                <div class="checkbox">
                                    <label>
                                    <input type="checkbox" id="chkOperacoesIntradayPLNegativo" />P & L Negativo
                                    </label>
                                    <label class="align-to-right">*P&L < 0</label>
                                </div>
                                <div class="pull-right">
                                    <a href="#" onclick="return OperacoesIntradayRemoveFiltersParametrosIntraday()"> Remover Filtro </a>
                                </div>
                            </fieldset>
                            </div>
                            <div class="col-sm-3">
                            <div class="row">
                                <div class="col-sm-8">
                                <button type="button" id="btnOperacoesIntradayAplicarTodosFiltros" class="btn btn-primary btn-lg btn-block" onclick="return OperacoesIntraday_Aplicar_Filtros_Click(this)" > Aplicar Filtros </button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-8">
                                   &nbsp;
                                </div>
                            </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="row">
                                    <div class="col-sm-8">
                                        <button type="button" id="btnOperacoesIntradayRemoverTodosFiltros" class="btn btn-danger btn-xs btn-block"  onclick="return OperacoesIntraday_Remover_Todos_Filtros_Click(this)"> Remover todos os filtros</button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-8">
                                        <div class="checkbox">
                                    <label>
                                                <input type="checkbox" id="chkCarregarComSocketOperacoesIntraday" />Carregar via Socket
                                    </label>
                            </div>
                        </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <table id="jqGrid_OperacoesIntraday"><tr><td/></tr></table>
                    <div id="jqGrid_OperacoesIntraday_Pager"></div>
                </div>
                
                <div id="pnlRiscoResumido" runat="server" >
                    <fieldset class="filter-border">
                    <legend class="filter-border">Filtros</legend>
                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                <label>Cliente:</label> 
                                <input type="text" id="txtRiscoResumidoCliente" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtRiscoResumido_Cliente_KeyDown(this)" />
                                    <div class="pull-right">
                                    <button onclick="return RiscoResumido_Adicionar_Cliente_Click()" class="btn btn-primary btn-xs btn-block" alt="Adicionar Clientes" >[+]</button>
                                </div>
                                <div class="checkbox">
                                    <label>
                                         <input type="checkbox" id="chkSomentePLNegativo" />Somente P&L negativo
                                    </label>
                                </div>
                                <div class="checkbox">
                                    <label>
                                         <input type="checkbox" id="chkSomenteComLucro" />Somente com Lucro
                                    </label>
                                </div>
                            </div>
                            </div>
                            <div class="col-sm-3">
                                <%--FieldSet % SFP Atingido--%>
                                <fieldset class="filter-border-small">
                            <legend class="filter-border-small">% SFP Atingido</legend>
                                <div class="checkbox row">
                                    <div class="col-sm-6"><label class="checkbox-inline"><input type="checkbox" id="chkSFPAtingidoAte25" />Até 25%</label></div>
                                    <div class="col-sm-6"><label class="checkbox-inline"><input type="checkbox" id="chkSFPAtingidoEntre50e75" />Entre 50 e 75%</label></div>
                                </div>
                                <div class="checkbox row">
                                    <div class="col-sm-6"><label class="checkbox-inline"><input type="checkbox" id="chkSFPAtingidoEntre25e50" />Entre 25 e 50%</label></div>
                                    <div class="col-sm-6"><label class="checkbox-inline"><input type="checkbox" id="chkSFPAtingidoAcima75" />Acima de 75%</label></div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="pull-right">
                                    <a href="#" onclick="return RiscoResumidoRemoveFiltersSFPAtingido()"> Remover Filtro </a>
                                </div>
                                
                            </fieldset>
                            </div>
                            <div class="col-sm-5">
                                <fieldset class="filter-border-small">
                            <legend class="filter-border-small">R$ Prejuízo Atingido (PL Total)</legend>
                                <div class="checkbox row">
                                    <div class="col-sm-4"><label class="checkbox-inline"><input type="checkbox" id="chkRSAtingidoAte2K" />Ate 2K</label></div>
                                    <div class="col-sm-4"><label class="checkbox-inline"><input type="checkbox" id="chkRSAtingidoEntre5Ke10K" />Entre 5K e 10K</label></div>
                                    <div class="col-sm-4"><label class="checkbox-inline"><input type="checkbox" id="chkRSAtingidoEntre20Ke50K" />Entre 20K e 50K</label></div>
                                </div>
                                <div class="checkbox row">
                                    <div class="col-sm-4"><label class="checkbox-inline"><input type="checkbox" id="chkRSAtingidoEntre2ke5k" />Enter 2K e 5K</label>         </div>
                                    <div class="col-sm-4"><label class="checkbox-inline"><input type="checkbox" id="chkRSAtingidoEntre10Ke20K" />Entre 10K e 20K</label> </div>
                                    <div class="col-sm-4"><label class="checkbox-inline"><input type="checkbox" id="chkRSAtingidoAcima50K" />Acima de 50K</label>        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="pull-right">
                                    <a href="#" onclick="return RiscoResumidoRemoveFiltersPrejuizoAtingido()"> Remover Filtro </a>
                                </div>
                            </fieldset>
                            </div>
                            <div class="col-sm-2">
                            <div class="row">
                                <div class="col-sm-12">
                                <button type="button" id="Button1" class="btn btn-primary btn-lg btn-block" onclick="return RiscoResumido_Aplicar_Filtros_Click(this)" > Aplicar Filtros </button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    &nbsp;
                                </div>
                            </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <button type="button" id="btnRiscoResumido_Remover_Filtro" class="btn btn-danger btn-xs btn-block"  onclick="return RiscoResumido_Remover_Todos_Filtros_Click(this)"> Remover todos os filtros</button>
                                    </div>
                                </div>
                                <%--<div class="row">
                                    <div class="col-sm-12">
                                        <div class="checkbox">
                                            <label>
                                                <input type="checkbox" id="chkCarregarViaSocketRiscoResumido" />Carregar via Socket
                                            </label>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </fieldset>
                    <table id="jqGrid_RiscoResumido"></table>
                    <div id="jqGrid_RiscoResumido_Pager"></div>
                    
                </div>

                <div id="divAcompanhamento" class=".relativo { position: relative; }" runat="server">
                
                    <fieldset class="filter-border">
                    <legend class="filter-border">Filtros</legend>

                        <div class="row">

                            <%-- Filtro de cliente --%>
                            <div id="divCliente" class="col-sm-2" style="width: 150px;">
                                <div class="form-group">
                                    <label class="control-label" style="width:40px;">Cliente:</label>
                                    <input type="text" style="width:60px;" id="txtAcompanhamentoCodigoCliente" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtOperacoesIntraday_Cliente_KeyDown(this)" onkeypress="return SomenteNumero(event)"/>
                                    <label class="control-label" style="width:40px;">Ativo:</label>
                                    <input type="text" style="width:60px;" id="txtAcompanhamentoCodigoInstrumento" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtOperacoesIntraday_Cliente_KeyDown(this)" />
                                    <label class="control-label" style="width:40px;">Início:</label>
                                    <input type="text" style="width:60px;" id="txtAcompanhamentoHoraInicio" class="ui-widget ui-widget-content ui-corner-all SearchFor Mascara_Hora" onkeydown="return txtOperacoesIntraday_Cliente_KeyDown(this)" value="08:00"/>
                                    <label class="control-label" style="width:40px;">Fim:</label>    
                                    <input type="text" style="width:60px;" id="txtAcompanhamentoHoraFim" class="ui-widget ui-widget-content ui-corner-all SearchFor Mascara_Hora" onkeydown="return txtOperacoesIntraday_Cliente_KeyDown(this)" value="18:00"/>
                                </div>
                            </div>
                            
                            <%-- Filtro de sentido --%>
                            <div id="divSentido" class="col-sm-1" style="padding-left:2px; padding-right: 2px">
                                <fieldset class="filter-border-small">
                                <legend class="filter-border-small">Sentido</legend>
                                    <div class="form-group">
                                        <div class="col-sm-1" >
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoSentidoAmbos" onclick="return chkSentido_Click(this);" />Ambos</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoSentidoCompra" onclick="return chkSentido_Click(this);" />Compra</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoSentidoVenda" onclick="return chkSentido_Click(this);" />Venda</label>
                                        </div>
                                        </div>
                                </fieldset>
                            </div>
                            
                            <%-- Filtro de bolsa --%>
                            <div id="divExchange" class="col-sm-1" style="padding-left:2px; padding-right: 2px">
                                <fieldset class="filter-border-small">
                                <legend class="filter-border-small">Exchange</legend>
                                    <div class="form-group">
                                        <div class="col-sm-1">
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoExchangeAmbos" onclick="return chkExchange_Click(this);"/>Ambos</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoExchangeBOV" onclick="return chkExchange_Click(this);"/>BOV</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoExchangeBMF" onclick="return chkExchange_Click(this);"/>BMF</label>
                                            </div>
                                            </div>
                                </fieldset>
                            </div>
                            
                            <%-- Filtro de status --%>
                            <div id="divStatus" class="col-sm-3" style="width:210px; padding-left:2px; padding-right: 2px"">
                                <fieldset class="filter-border-small">
                                <legend class="filter-border-small">Status</legend>
                                    <div class="form-group">
                                        <div class="col-sm-1">
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusExecutada"             value="2"              onclick="return chkAcompanhamentoStatus_Click(this);"/>Executada</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusParcialmenteExecutada" value="1"              onclick="return chkAcompanhamentoStatus_Click(this);"/>Parcial</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusSubstituida"           value="5"              onclick="return chkAcompanhamentoStatus_Click(this);"/>Substituida</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusRejeitada"             value="8"              onclick="return chkAcompanhamentoStatus_Click(this);"/>Rejeitada</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusSuspensa"              value="9"              onclick="return chkAcompanhamentoStatus_Click(this);"/>Suspensa</label>
                                    </div>
                                        <div class="col-sm-1" style="margin-left: 55px;">
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusAberta"                value="0"              onclick="return chkAcompanhamentoStatus_Click(this);"/>Aberta</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusCancelada"             value="4"              onclick="return chkAcompanhamentoStatus_Click(this);"/>Cancelada</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusExpirada"              value="67"             onclick="return chkAcompanhamentoStatus_Click(this);"/>Expirada</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusWaiting"               value="54;65;69;"      onclick="return chkAcompanhamentoStatus_Click(this);"/>Waiting</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoStatusSolicitada"            value="101;"           onclick="return chkAcompanhamentoStatus_Click(this);"/>Solicitada</label>
                                    </div>
                                    </div>
                                </fieldset>
                            </div>
                            <%-- Filtro de origem --%>
                            <%-- 
                            <div id="divAcompanhamentoExchange" class="col-sm-2" style="width:200px;padding-left:2px; padding-right: 2px">
                                <fieldset class="filter-border-small">
                                <legend class="filter-border-small">DMA de origem</legend>
                                    <div class="form-group">
                                        <div class="col-sm-1">
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoOrigemHomeBroker" value="1"/>HB</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoOrigemProfitChart"/>ProfitChart</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoOrigemAgenciaEstado"/>AE</label>
                                    </div>
                                        <div class="col-sm-1" style="margin-left: 55px;">
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoOrigemGLTrade" value=""/>GL</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoOrigemGTI"/>GTI</label>
                                            <label class="checkbox"><input type="checkbox" id="chkAcompanhamentoOrigemTryd"/>Tryd</label>
                                    </div>
                                    </div>
                                </fieldset>
                            </div>
                            --%>

                            <%-- Botão de consulta --%>
                            <div class="col-sm-4" style="top:5px;">
                                <div class="row">
                                    <div class="col-sm-8">
                                        <button type="button" id="btnConsultarAcompanhamento" class="btn btn-primary btn-lg btn-block" onclick="return ConsultarAcompanhamento_Click(this)" > Consultar </button>
                                    </div>
                                </div>
                            </div>
                            <%-- Espaçador --%>
                            <div class="col-sm-4">
                                <div class="row">
                                    <div class="col-sm-8">&nbsp;</div>
                                </div>
                            </div>
                            <%-- Botão de limpeza de filtros --%>
                            <div class="col-sm-4">
                                <div class="row">
                                    <div class="col-sm-8">
                                        <button type="button" id="btnRemoverTodosFiltrosAcompanhamento" class="btn btn-danger btn-xs btn-block"  onclick="return RemoverTodosFiltrosAcompanhamento_Click(this)"> Remover todos os filtros </button>
                                    </div>
                                </div>
                                
                            </div>
                        </div>

                     </fieldset>

                    <table class=".ui-jqgrid-btable .ui-state-highlight { background: yellow; } GridAcompanhamento GridAcompanhamento_CheckSemFundo" id="jqGridAcompanhamento" style="position:relative;"></table>
                    <div id="jqGridAcompanhamentoPager" style="position: relative;"></div>
                            
                </div>

                <div id="divVendasDescobertas" runat="server">

                    <fieldset class="filter-border">
                    <legend class="filter-border">Filtros</legend>

                        <div id="divVendasDescobertasCliente" class="row">

                            <div class="col-sm-2" style="width: 150px;">
                                <div class="form-group">
                                    <label class="control-label" style="width:40px;">Cliente:</label>
                                    <input type="text"style="width:60px;" id="txtVendasDescobertasCodigoCliente" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtOperacoesIntraday_Cliente_KeyDown(this)" onkeypress="return SomenteNumero(event)"/>
                                    <label class="control-label"  style="width:40px;">Ativo:</label>
                                    <input type="text" style="width:60px;" id="txtVendasDescobertasCodigoInstrumento" class="ui-widget ui-widget-content ui-corner-all SearchFor" onkeydown="return txtOperacoesIntraday_Cliente_KeyDown(this)" />
                                </div>
                            </div>

                            <div id="divVendasDescobertasMercado" class="col-sm-2" style="padding-left:2px; padding-right: 2px; width: 200px;">
                                <fieldset class="filter-border-small">
                                <legend class="filter-border-small">Mercado</legend>
                                    <div class="checkbox">
                                        <label><input type="checkbox" id="chkMercadoTodos" onclick="return chkMercado_Click(this);" value=""/>Todos os Mercados</label>
                                    </div>
                                    <div class="checkbox">
                                        <label class="checkbox-inline"><input type="checkbox" id="chkMercadoVista" onclick="return chkMercado_Click(this);" value="VIS;"/>À Vista</label>
                                        <label class="checkbox-inline"><input type="checkbox" id="chkMercadoFuturo" onclick="return chkMercado_Click(this);" value="FUT;"/>Futuro</label>
                                    </div>
                                    <div class="checkbox">
                                        <label><input type="checkbox" id="chkMercadoOpcoes" onclick="return chkMercado_Click(this);" value="OPC;OPV;"/>Opções</label>
                                        <label style="font-size:0.8em;">*Qt.Total < 0</label>
                                    </div>
                                </fieldset>
                            </div>

                            <div class="col-sm-3" style="top:5px;">
                                <div class="row">
                                    <div class="col-sm-8">
                                        <button type="button" id="btnConsultarVendasDescobertas" class="btn btn-primary btn-lg btn-block" onclick="return ConsultarVendasDescobertas_Click(this)" > Consultar </button>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-8">
                                <div class="row">
                                    <div class="col-sm-8">&nbsp;</div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="row">
                                    <div class="col-sm-8">
                                        <button type="button" id="btnRemoverTodosFiltrosVendasDescobertas" class="btn btn-danger btn-xs btn-block"  onclick="return RemoverTodosFiltrosVendasDescobertas_Click(this)"> Remover todos os filtros </button>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </fieldset>
                    
                    <table  class=".ui-jqgrid-btable .ui-state-highlight { background: yellow; }" id="jqGridVendasDescobertas"></table>
                    <div id="jqGridVendasDescobertasPager"></div>

                </div>

            </div>
        </div>
    </div>
    </form>
</body>
</html>

<script type="text/javascript">
    $(function() {
        $( "#nav-tabs" ).tabs();
    });

    $("input.datepicker").datepicker({
        dateFormat: "dd/mm/yy"
            , dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
            , monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"]
            , monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
    });

    function Page_Load_CodeBehind()
    {
        <asp:literal id="litJavascriptOnLoad" runat="server"></asp:literal>
    }

    

    $(document).ready(Page_Load);
</script>


