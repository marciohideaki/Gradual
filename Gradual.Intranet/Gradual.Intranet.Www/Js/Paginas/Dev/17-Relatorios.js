/// <reference path="01-GradIntra-Principal.js" />
/// <reference path="02-GradIntra-Navegacao.js" />
/// <reference path="03-GradIntra-Busca.js" />
/// <reference path="04-GradIntra-Cadastro.js" />
/// <reference path="05-GradIntra-Relatorio.js" />

var gRelatorios_UrlDoUltimo = "";

var gRelatorios_UltimaParte = 1;

function GradIntra_Relatorios_AoSelecionarSistema() { }

function GradIntra_Relatorios_ConfiguraCamposObrigatorios(pRelatorio)
{
    var lArrReturn ;
    switch(pRelatorio)
    {
        case "R001":
            lArrReturn =
                [
                    "txtClientes_FiltroRelatorio_DataInicial",
                    "txtClientes_FiltroRelatorio_DataFinal"
                ];    
        break;
    }

    return lArrReturn;
}

function GradIntra_Relatorios_IniciarRecebimentoProgressivo(pPainelFiltro, pPainelResultados, pURL, pRelatorio)
{
    gRelatorios_UrlDoUltimo = pURL;

    gRelatorios_UltimaParte = 0;

    GradIntra_Busca_BuscarItensParaListagemSimples( pPainelFiltro
                                                  , pPainelResultados
                                                  , pURL
                                                  , {
                                                       CamposObrigatorios: GradIntra_Relatorios_ConfiguraCamposObrigatorios(pRelatorio)
                                                    }
                                                  , GradIntra_Relatorios_IniciarRecebimentoProgressivo_CallBack
                                                  );
}

function GradIntra_Relatorios_IniciarRecebimentoProgressivo_CallBack()
{
    window.setTimeout(function()
    {
        gRelatorios_UltimaParte++;

        if (gGradIntra_Navegacao_SistemaAtual == "Relatorios"
        && (gGradIntra_Navegacao_SubSistemaAtual == "Gerais")
        && ((gRelatorios_UrlDoUltimo.indexOf("R017.aspx") > -1)
        || ((gRelatorios_UrlDoUltimo.indexOf("R018.aspx") > -1))
        || ((gRelatorios_UrlDoUltimo.indexOf("R019.aspx") > -1))))
        {
//            GradIntra_CarregarHtmlVerificandoErro( gRelatorios_UrlDoUltimo
//                                                 , { Acao : "BuscarParte", Parte: gRelatorios_UltimaParte }
//                                                 , null
//                                                 , GradIntra_Relatorios_ReceberParte_CallBack);
        }
        else if (gRelatorios_UrlDoUltimo.indexOf("R013.aspx") == -1)
        {
            GradIntra_CarregarJsonVerificandoErro( gRelatorios_UrlDoUltimo
                                                 , {  Acao : "BuscarParte"
                                                   ,  Parte: gRelatorios_UltimaParte 
                                                   }
                                                 , GradIntra_Relatorios_ReceberParte_CallBack);
        }
        else
        {
            GradIntra_Relatorios_GerarGraficos();
        }

    }, 350);
}

function GradIntra_Relatorios_ReceberParte_CallBack(pResposta)
{
    if (pResposta.ObjetoDeRetorno)
    {
        var lRows = "";

        if ("RelatoriosDBM" == gGradIntra_Navegacao_SubSistemaAtual)
            for (var a = 0; a < pResposta.ObjetoDeRetorno.length; a++)
                lRows += GradIntra_Relatorios_DBM_AdicionarLinha(pResposta.ObjetoDeRetorno[a]);
        else
            for (var a = 0; a < pResposta.ObjetoDeRetorno.length; a++)
                lRows += GradIntra_Relatorios_AdicionarLinha(pResposta.ObjetoDeRetorno[a]);
        
        $("#rowLinhaCarregandoMais").before(lRows);

        DefinirCoresValores_Load();

        if (pResposta.Mensagem.indexOf("TemMais") == 0)
        {
            $("#rowLinhaCarregandoMais td").html(pResposta.Mensagem.substr(pResposta.Mensagem.indexOf(":") + 1));

            GradIntra_Relatorios_IniciarRecebimentoProgressivo_CallBack();
        }
        else
        {
            $("#rowLinhaCarregandoMais").hide();

            $("#rowLinhaCarregandoMais").remove();

            gRelatorios_UltimaParte = 1;
        }
    }
}

function GradIntra_Relatorios_Complexos_ReceberParte()
{
    if (gGradIntra_Navegacao_SistemaAtual    == "Relatorios"
    &&  gGradIntra_Navegacao_SubSistemaAtual == "Gerais"
    && (gRelatorios_UrlDoUltimo.indexOf("R017.aspx") > -1
    ||  gRelatorios_UrlDoUltimo.indexOf("R018.aspx") > -1
    ||  gRelatorios_UrlDoUltimo.indexOf("R019.aspx") > -1))
    {
        GradIntra_CarregarHtmlVerificandoErro( gRelatorios_UrlDoUltimo
                                             , { Acao : "BuscarParte", Parte: gRelatorios_UltimaParte }
                                             , null
                                             , GradIntra_Relatorios_Complexos_ReceberParte_CallBack);
    }
}

function GradIntra_Relatorios_Complexos_ReceberParte_CallBack(pResposta)
{
    window.setTimeout(function()
    {
        if (pResposta != '')
        {
            $("#divGradIntra_Relatorios_Gerais_ContaCorrente_CarregandoConteudo").hide();
            $(".MensagemCarregarQuantoDeTanto").hide();
            $("#pnlRelatorios_Resultados #divCarregandoMais").hide();
            $("#pnlRelatorios_Resultados").append(pResposta);
            $(".Relatorios_Gerais_NotaDeCorretagem_QuebraDePagina").eq(0).hide();
            $(".DivNavegacaoIniciais:gt(0)").hide();

            if ($("#cmbClientes_FiltroAssessor").val() != "")
                $(".DivNavegacaoIniciais").hide();

            gRelatorios_UltimaParte++;

            if ($("#pnlRelatorios_Resultados #divCarregandoMais").filter(":visible").length > 0)
                GradIntra_Relatorios_Complexos_ReceberParte();
        }
    }, 500);
}

function GradIntra_Relatorios_AdicionarLinha(pLinha)
{
    if (gRelatorios_UrlDoUltimo.indexOf("001.aspx") != -1)
    {
        return "<tr>" +
                    "<td style='text-align:center'>" + pLinha.Id             + "</td>" +
                    "<td style='text-align:right'>"  + pLinha.Bovespa        + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.Nome           + "</td>" +
                    "<td style='text-align:right'>"  + pLinha.CpfCnpj        + "</td>" +
                    "<td style='text-align:center'>" + pLinha.Assessor       + "</td>" +
                    "<td style='text-align:center'>" + pLinha.TipoDePessoa   + "</td>" +
                    "<td style='text-align:center'>" + pLinha.DataDeCadastro + "</td>" +
                    "<td style='text-align:center'>" + pLinha.DataUltAtualizacao + "</td>" +
                    "<td style='text-align:center'>" + pLinha.PassoAtual     + "</td>" +
                    "<td style='text-align:center'>" + pLinha.Exportado      + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.DsEmail        + "</td>" +
                    "<td style='text-align:center'>" + pLinha.Telefones      + "</td>" +
                "</tr>";                                                                    
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("002.aspx") != -1)                              
    {                                                                                       
      return "<tr>" +                                                                       
                "<td style='text-align:center'>" + pLinha.Id                   + "</td>" +
                "<td style='text-align:center'>" + pLinha.Bovespa              + "</td>" +
                "<td style='text-align:left'>"   + pLinha.Nome                 + "</td>" +
                "<td style='text-align:right'>"  + pLinha.CpfCnpj              + "</td>" +
                "<td style='text-align:center'>" + pLinha.Assessor             + "</td>" +
                "<td style='text-align:center'>" + pLinha.TipoPessoa           + "</td>" +
                "<td style='text-align:center'>" + pLinha.DataDaPendencia      + "</td>" +
                "<td style='text-align:center'>" + pLinha.DataDaResolucao      + "</td>" +
                "<td style='text-align:left'>"   + pLinha.TipoDePendencia      + "</td>" +
                "<td style='text-align:left'>"   + pLinha.DescricaoDaPendencia + "</td>" +
             "</tr>";                                                                        
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("003.aspx") != -1)                              
    {                                                                                       
         return "<tr>" +                                                                    
                    "<td style='text-align:center'>" +  pLinha.Id                + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.Bovespa           + "</td>" +
                    "<td style='text-align:left'>"   +  pLinha.Nome              + "</td>" +
                    "<td style='text-align:right'>"  +  pLinha.CpfCnpj           + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.Assessor          + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.TpPessoa          + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.DataDaSolicitacao + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.DataDaResolucao   + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.TipoDeSolicitacao + "</td>" +
                    "<td style='text-align:left'>"   +  pLinha.Campo             + "</td>" +
               "</tr>";                                                                        
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("004.aspx") != -1)                              
    {                                                                         
         return "<tr>" +                                                      
                    "<td style='text-align:center'>" + pLinha.Id              + "</td>" +
                    "<td style='text-align:center'>" + pLinha.CodigoBovespa   + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.Nome            + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.CpfCnpj         + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.Assessor        + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.TipoPessoa      + "</td>" +
                    "<td style='text-align:center'>" + pLinha.Telefone        + "</td>" +
                    "<td style='text-align:center'>" + pLinha.DataDeRenovacao + "</td>" +
               "</tr>";                                                                         
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("005.aspx") != -1)                              
    {                                                                                       
        return "<tr>" +                                                                     
                    "<td style='text-align:center'>" + pLinha.Id                 + "</td>" +
                    "<td style='text-align:center'>" + pLinha.CodigoDeBolsa      + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.Nome               + "</td>" +
                    "<td style='text-align:right'>"  + pLinha.CpfCnpj            + "</td>" +
                    "<td style='text-align:center'>" + pLinha.Assessor           + "</td>" +
                    "<td style='text-align:center'>" + pLinha.TipoDePessoa       + "</td>" +
                    "<td style='text-align:center'>" + pLinha.DataDeCadastro     + "</td>" +
                    "<td style='text-align:center'>" + pLinha.PrimeiraExportacao + "</td>" +
                    "<td style='text-align:center'>" + pLinha.UltimaExportacao   + "</td>" +
                    "<td style='text-align:center'>" + pLinha.Telefones          + "</td>" +
               "</tr>";
    }

    else if(gRelatorios_UrlDoUltimo.indexOf("006.aspx") != -1)
    {
        return "<tr>" +
                   "<td style='text-align:center'>" + pLinha.Id                         + "</td>" +
                   "<td style='text-align:center'>" + pLinha.CodigoBovespa              + "</td>" +
                   "<td style='text-align:left'>"   + pLinha.Nome                       + "</td>" +
                   "<td style='text-align:left'>"   + pLinha.CpfCnpj                    + "</td>" +
                   "<td style='text-align:center'>" + pLinha.Assessor                   + "</td>" +
                   "<td style='text-align:center'>" + pLinha.UltimaAlteracaoSuitability + "</td>" +
                   "<td style='text-align:left'>"   + pLinha.Status                     + "</td>" +
                   "<td style='text-align:left'>"   + pLinha.ResultadoDaAnalise         + "</td>" +
                   "<td style='text-align:left'>"   + pLinha.Local                      + "</td>" +
                   "<td style='text-align:center'>" + pLinha.Peso                       + "</td>" +
                   "<td style='text-align:center'>" + pLinha.Respostas                  + "</td>" +
                   "<td style='text-align:center'>" + pLinha.RealizadoPeloCliente       + "</td>" +
                   "<td style='text-align:center'>" + pLinha.ArquivoCienciaLink         + "</td>" +
               "</tr>";                                                                       
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("007.aspx") != -1)                              
    {                                                                                       
      return "<tr>" +                                                       
                 "<td style='text-align:right'>"  + pLinha.Id               + "</td>" +
                 "<td style='text-align:right'>"  + pLinha.Bovespa          + "</td>" +
                 "<td style='text-align:left'>"   + pLinha.Nome             + "</td>" +
                 "<td style='text-align:right'>"  + pLinha.CpfCnpj          + "</td>" +
                 "<td style='text-align:right'>"  + pLinha.Assessor         + "</td>" +
                 "<td style='text-align:center'>" + pLinha.TipoDePessoa     + "</td>" +
                 "<td style='text-align:center'>" + pLinha.DataDeCadastro   + "</td>" +
                 "<td style='text-align:center'>" + pLinha.Exportado        + "</td>" +
                 "<td style='text-align:center'>" + pLinha.Pais             + "</td>" +
                 "<td style='text-align:center'>" + pLinha.AtividadeIlicita + "</td>" +
             "</tr>";                                                                         
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("008.aspx") != -1)                              
    {
        return "<tr>" +
                    "<td style='text-align:left'>"   + pLinha.Nome                 + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.DsEmailRemetente     + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.DsEmailDestinatario  + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.Bovespa              + "</td>" +
                    "<td style='text-align:center'>" + pLinha.CpfCnpj              + "</td>" +
                    "<td style='text-align:center'>" + pLinha.DataDeEnvio          + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.Assunto              + "</td>" +
                    "<td style='text-align:left'>"   + pLinha.Perfil               + "</td>" +
               "</tr>";                                                                         
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("009.aspx") != -1)                              
    {                                                                                       
      return "<tr>" +                                                                       
                 "<td style='text-align:center'>" +  pLinha.Id             + "</td>" +
                 "<td style='text-align:center'>" +  pLinha.Bovespa        + "</td>" +
                 "<td style='text-align:left'>"   +  pLinha.Nome           + "</td>" +
                 "<td style='text-align:left'>"   +  pLinha.CpfCnpj        + "</td>" +
                 "<td style='text-align:center'>" +  pLinha.Assessor       + "</td>" +
                 "<td style='text-align:center'>" +  pLinha.TipoDePessoa   + "</td>" +
                 "<td style='text-align:center'>" +  pLinha.DataDeCadastro + "</td>" +
                 "<td style='text-align:left'>"   +  pLinha.Email          + "</td>" +
             "</tr>";                                                      
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("010.aspx") != -1)                              
    {                                                                                       
         return "<tr>" +                                                                    
                   "<td style='text-align:center'>" +  pLinha.Id             + "</td>" +
                   "<td style='text-align:left'>"   +  pLinha.Nome           + "</td>" +
                   "<td style='text-align:left'>"   +  pLinha.CpfCnpj        + "</td>" +
                   "<td style='text-align:center'>" +  pLinha.TipoDePessoa   + "</td>" +
                   "<td style='text-align:center'>" +  pLinha.DataDeCadastro + "</td>" +
                   "<td style='text-align:left'>"   +  pLinha.Email          + "</td>" +
                   "<td style='text-align:center'>" +  pLinha.Assessor       + "</td>" +
                   "<td style='text-align:center'>" +  pLinha.Conta          + "</td>" +
                   "<td style='text-align:center'>" +  pLinha.TipoConta      + "</td>" +
               "</tr>";                                                                         
    }                                                                                       
                                                                                            
    else if(gRelatorios_UrlDoUltimo.indexOf("011.aspx") != -1)                              
    {                                                                      
         return "<tr>" +                                                   
                    "<td style='text-align:center'>" +  pLinha.CodBolsa    + "</td>" +
                    "<td style='text-align:left'>"   +  pLinha.NomeCliente + "</td>" +
                    "<td style='text-align:left'>"   +  pLinha.CodAssessor + "</td>" +
                    "<td style='text-align:left'>"   +  pLinha.CpfCnpj     + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.Produto     + "</td>" +
                    "<td style='text-align:center'>" +  pLinha.DataAdesao  + "</td>" +
                "</tr>";
     }
     else if (gRelatorios_UrlDoUltimo.indexOf("015.aspx") != -1)
     {
         return "<tr>" +
                   "<td style=\"text-align:center\">" + pLinha.CodBolsa      + "</td>" +
                   "<td style=\"text-align:left\">"   + pLinha.NomeCliente   + "</td>" +
                   "<td style=\"text-align:left\">"   + pLinha.CpfCnpj       + "</td>" +
                   "<td style=\"text-align:left\">"   + pLinha.Produto       + "</td>" +
                   "<td style=\"text-align:center\">" + pLinha.DataAdesao    + "</td>" +
                   "<td style=\"text-align:center\">" + pLinha.DataFimAdesao + "</td>" +
                   "<td style=\"text-align:center\">" + pLinha.DataCobranca  + "</td>" +
                   "<td style=\"text-align:center\">" + pLinha.ValorCobranca + "</td>" +
               "</tr>";
     }
     else if (gRelatorios_UrlDoUltimo.indexOf("016.aspx") != -1)
     {
        return "<tr>" +
                    "<td style=\"text-align: left;   width: auto;\">" + pLinha.Cliente                   + "</td>" +
                    "<td style=\"text-align: left;   width: auto;\">" + pLinha.CpfCnpj                   + "</td>" +
                    "<td style=\"text-align: left;   width: auto;\">" + pLinha.Produto                   + "</td>" +
                    "<td style=\"text-align: left;   width: auto;\">" + pLinha.Ativo                     + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.DataAdesao                + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.DataRetroativaTrocaAtivo  + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.DataVencimento            + "</td>" +
                "</tr>";
    }
    else if (gRelatorios_UrlDoUltimo.indexOf("027.aspx") != -1) {
        return "<tr>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.CodigoGradual    + "</td>" +
                    "<td style=\"text-align: left;   width: auto;\">" + pLinha.Nome             + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.CodigoAssessor   + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.CodigoExterno    + "</td>" +
                "</tr>";
    }
    else if (gRelatorios_UrlDoUltimo.indexOf("029.aspx") != -1) {
        return "<tr>" +
                    "<td style=\"text-align: center;    width: 6em; \">" + pLinha.DataMovimento     + "</td>" +
                    "<td style=\"text-align: left;      width: 6em; \">" + pLinha.CodigoCliente     + "</td>" +
                    "<td style=\"text-align: left;      width: auto;\">" + pLinha.NomeCliente       + "</td>" +
                    "<td style=\"text-align: left;      width: 6em; \">" + pLinha.NumeroLancamento  + "</td>" +
                    "<td style=\"text-align: left;      width: auto;\">" + pLinha.Descricao         + "</td>" +
                    "<td style=\"text-align: right;     width: 6em; \">" + pLinha.Valor             + "</td>" +
                "</tr>";
    }
}

function GradIntra_Relatorios_DBM_AdicionarLinha(pLinha)
{
    if (gRelatorios_UrlDoUltimo.indexOf("006.aspx") != -1)
    {
        return "<tr>" +
                    "<td style=\"text-align: right; width: auto;\">" + pLinha.CodigoCliente + "</td>" +
                    "<td style=\"text-align: left;  width: auto;\">" + pLinha.NomeCliente   + "</td>" +
                    "<td style=\"text-align: right; width: auto;\">" + pLinha.Total         + "</td>" +
                    "<td style=\"text-align: right; width: auto;\">" + pLinha.ALiquidar     + "</td>" +
                    "<td style=\"text-align: right; width: auto;\">" + pLinha.Disponivel    + "</td>" +
                    "<td style=\"text-align: right; width: auto;\">" + pLinha.D1            + "</td>" +
                    "<td style=\"text-align: right; width: auto;\">" + pLinha.D2            + "</td>" +
                "</tr>";        
    }
    else if (gRelatorios_UrlDoUltimo.indexOf("007.aspx") != -1)
    {
        return "<tr>" +
                    "<td style=\"text-align: left;   width: auto;\">" + pLinha.Cliente    + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.DataLanc   + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.DataRef    + "</td>" +
                    "<td style=\"text-align: center; width: auto;\">" + pLinha.DataLiq    + "</td>" +
                    "<td style=\"text-align: left;   width: auto;\">" + pLinha.Lancamento + "</td>" +
                    "<td style=\"text-align: right;  width: auto;\">" + pLinha.Valor      + "</td>" +
                "</tr>";
    }

}

function GradItra_Clientes_Relatorios_Financeiro_FecharRelatorio_Click(pSender) 
{
    $('#pnlCliente_Posicao_Relatorio').hide();
    
    return false;
}

function GradIntra_Relatorios_Gerais_Load(pSender) 
{
    var lIdAssessorLogado = $("#hddIdAssessorLogado").val();

    if (lIdAssessorLogado && "" != lIdAssessorLogado) {
        var lComboBusca_FiltroRelatorioRisco_Assessor = $("#cmbBusca_FiltroRelatorio_Assessor");
        lComboBusca_FiltroRelatorioRisco_Assessor.prop("disabled", true);
        lComboBusca_FiltroRelatorioRisco_Assessor.val(lIdAssessorLogado);
    }
}

function GradIntra_Relatorios_GerarGraficos()
{
    try
    {
        var values = [],
            labels = [];

        var lTabela, lTR;

        lTbody = $("table.Grafico_PorAssessor tbody");

        if(lTbody.length > 0)
        {
            lTbody.children("tr").each(function()
            {
                lTR = $(this);

                labels.push(lTR.children("th").html());
                values.push(parseInt(lTR.children("td").html(), 10));
            });

            var r = Raphael("pnlGrafico_PorAssessor");

            r.g.txtattr.font = "10px 'Tahoma', sans-serif";

            r.g.text(306, 14, "Distribuição por Assessor").attr({"font-size": 14});

            var pie = r.g.piechart( 110
                                  , 140
                                  , 100
                                  , values
                                  , { legend   : labels
                                    , legendpos: "east"
                                       //, href: ["http://raphaeljs.com", "http://g.raphaeljs.com"]
                                    }
                                  );


            pie.hover(function () {
                this.sector.stop();
                this.sector.scale(1.1, 1.1, this.cx, this.cy);
                if (this.label) {
                    this.label[0].stop();
                    this.label[0].scale(1.5);
                    this.label[1].attr({"font-weight": 800});
                }
            }, function () {
                this.sector.animate({scale: [1, 1, this.cx, this.cy]}, 500, "bounce");
                if (this.label) {
                    this.label[0].animate({scale: 1}, 500, "bounce");
                    this.label[1].attr({"font-weight": 400});
                }
            });

            //    raphael("pnlGrafico_PorAssessor", 300, 300).pieChart(0, 0, 150, values, labels, "#fff");
        }

        lTbody = $("table.Grafico_PorEstado tbody");

        if(lTbody.length > 0)
        {
            values = [];
            labels = [];

            lTbody.children("tr").each(function()
            {
                lTR = $(this);

                labels.push(  lTR.children("th").html()  );
                values.push(  parseInt(lTR.children("td").html(), 10)  );
            });

            var r = Raphael("pnlGrafico_PorEstado");

            r.g.txtattr.font = "10px 'Tahoma', sans-serif";

            r.g.text(240, 14, "Distribuição por Estado").attr({"font-size": 14});

            var pie = r.g.piechart(  180
                                   , 140
                                   , 100
                                   , values
                                   , {   legend:    labels
                                       , legendpos: "east"
                                       //, href: ["http://raphaeljs.com", "http://g.raphaeljs.com"]
                                     }
                                  );

            pie.hover(function () {
                this.sector.stop();
                this.sector.scale(1.1, 1.1, this.cx, this.cy);
                if (this.label) {
                    this.label[0].stop();
                    this.label[0].scale(1.5);
                    this.label[1].attr({"font-weight": 800});
                }
            }, function () {
                this.sector.animate({scale: [1, 1, this.cx, this.cy]}, 500, "bounce");
                if (this.label) {
                    this.label[0].animate({scale: 1}, 500, "bounce");
                    this.label[1].attr({"font-weight": 400});
                }
            });

            //    raphael("pnlGrafico_PorAssessor", 300, 300).pieChart(0, 0, 150, values, labels, "#fff");
        }
    }
    catch(erro)
    {
        try
        {
            console.log(erro);
        }
        catch(erro2){}
    }

    window.setTimeout(function()
    { 
        if ($("#pnlGrafico_PorEstado").children().length == 0
        || ($("#pnlGrafico_PorAssessor").children().length == 0))
        {
            GradIntra_Relatorios_GerarGraficos();
        }

    }, 1000);
}

function btnMonitoramento_Relatorios_FiltrarRelatorio_Click(pSender)
{
    var lRelatorio = $("#cboMonitoramento_RelatorioTipo").val();
    
    $(".pnlRelatorio").show();

    if(lRelatorio != "")
    {
        var lUrl = "Monitoramento/Relatorios/" + lRelatorio + ".aspx";
    
        GradIntra_Busca_BuscarItensParaListagemSimples( $("#pnlMonitoramento_FiltroRelatorio")
                                                      , $("#pnlConteudo_Monitoramento_Relatorios_Resultados")
                                                      , lUrl
                                                      , null);
    }
    else
    {
        //não deve aparecer, porque o botão se oculta quando não tem um selecionado; #JIC.
        GradIntra_ExibirMensagem("A", "Favor selecionar um relatório");
    }

    return false;
}

function btnSistema_Relatorios_FiltrarRelatorio_Click(pSender, pOrdernarPor)
{
    var lRelatorio = $("#cboSistema_RelatorioTipo").val();
    
    $(".pnlRelatorio").show();

    if(lRelatorio != "")
    {
        var lUrl = "Sistema/Relatorios/" + lRelatorio + ".aspx";
    
        GradIntra_Busca_BuscarItensParaListagemSimples(   $("#pnlSistema_FiltroRelatorio")
                                                        , $("#pnlConteudo_Sistema_Relatorios_Resultados")
                                                        , lUrl
                                                        , null
                                                        , pOrdernarPor);
    }
    else
    {
        //não deve aparecer, porque o botão se oculta quando não tem um selecionado; #JIC.
        GradIntra_ExibirMensagem("A", "Favor selecionar um relatório");
    }

    return false;
}

function cboMonitoramento_RelatorioTipo_Change(pSender)
{
    pSender = $(pSender);
    
    var lDivFormulario = pSender.closest(".Busca_Formulario");
    var lValor = pSender.val();

    $(".pnlRelatorio").hide();

    GradIntra_Relatorio_Monitoramento_OcultarExibirFiltros(lDivFormulario, lValor);
        
    $('#btnClienteRelatorioImprimir').prop("disabled", true);
}

function cboSistema_RelatorioTipo_Change(pSender)
{
    pSender = $(pSender);
    
    var lDivFormulario = pSender.closest(".Busca_Formulario");
    var lValor = pSender.val();

    $(".pnlRelatorio").hide();

    Relatorio_Sistema_OcultarExibirFiltros(lDivFormulario, lValor);
        
    $('#btnClienteRelatorioImprimir').prop("disabled", true);
}

function Relatorio_Sistema_OcultarExibirFiltros(pDivDeFormulario, pCodigoDoRelatorio)
{
    var lEntidadeBtnBusca = pDivDeFormulario.find(".btnBusca");
    
    var lParagrafoBotao = lEntidadeBtnBusca.closest("p");

    pDivDeFormulario.find("p[class^=R0]").hide();

    if(pCodigoDoRelatorio != "")
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = pCodigoDoRelatorio;
        
        pDivDeFormulario.find("p." + pCodigoDoRelatorio).show();

        lParagrafoBotao.show();
    }
    else
    {
        gGradIntra_Relatorios_RelatorioAtualSelecionado = null;

        lParagrafoBotao.hide();
    }
    
    if (pCodigoDoRelatorio == "R001")
    {
        pDivDeFormulario.removeClass("Busca_Formulario_2Linhas");
        pDivDeFormulario.addClass("Busca_Formulario_3Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_4Linhas");
        pDivDeFormulario.removeClass("Busca_Formulario_5Linhas");
    }
}

function btnClientes_Relatorios_Direct_Click(pRelatorio)
{
    var lUrl = "Clientes/Relatorios/" + pRelatorio + ".aspx?Acao=CarregarComoCSV";  
    
    if(pRelatorio == "R011")
    {
        lUrl += "&DataInicial="  + $("#txtClientes_FiltroRelatorio_DataInicial").val()
             +  "&DataFinal="    + $("#txtClientes_FiltroRelatorio_DataFinal").val()
             +  "&Produtos="     + $("#cboClientes_FiltroRelatorio_Produtos").val()
             +  "&Passo="        + $("#cboClientes_FiltroRelatorio_Passo").val()
             +  "&CodCliente="   + $("#txtClientes_FiltroRelatorio_CodCliente").val()
             +  "&ClientePasso=" + $("#cboClientes_FiltroRelatorio_Passo").val();
    }

    if (pRelatorio == "R015")
    {
        lUrl += "&DataInicial="  + $("#txtClientes_FiltroRelatorio_DataInicial").val()
             +  "&DataFinal="    + $("#txtClientes_FiltroRelatorio_DataFinal").val()
             +  "&Produtos="     + $("#cboClientes_FiltroRelatorio_Produtos").val()
             +  "&Passo="        + $("#cboClientes_FiltroRelatorio_Passo").val()
             +  "&CodCliente="   + $("#txtClientes_FiltroRelatorio_CodCliente").val()
             +  "&ClientePasso=" + $("#cboClientes_FiltroRelatorio_Passo").val();
    }

    if (pRelatorio == "R006") {
        lUrl += "&DataInicial="     + $("#txtClientes_FiltroRelatorio_DataInicial").val()
             + "&DataFinal="        + $("#txtClientes_FiltroRelatorio_DataFinal").val()
             + "&TipoPessoa="       + $("#cboClientes_FiltroRelatorio_TipoPessoa").val()
             + "&CodAssessor="      + $("#cmbClientes_FiltroAssessor").val()
             + "&CpfCnpj="          + $("#txtClientes_FiltroRelatorio_CpfCnpj").val()
             + "&CodCliente="       + $("#txtClientes_FiltroRelatorio_CodCliente").val();
    }

    if (pRelatorio == "R008") {
        lUrl += "&DataInicial="     + $("#txtClientes_FiltroRelatorio_DataInicial").val()
             + "&DataFinal="        + $("#txtClientes_FiltroRelatorio_DataFinal").val()
             + "&TipoEmail="        + $("#cboClientes_FiltroRelatorio_TipoEmailDisparado").val()
             + "&EmailDisparado="   + $("#txtClientes_FiltroRelatorio_Email_Disparado").val()
             + "&TipoPessoa="       + $("#cboClientes_FiltroRelatorio_TipoPessoa").val()
             + "&CodCliente="       + $("#txtClientes_FiltroRelatorio_CodCliente_R008").val()
             + "&CpfCnpj="          + $("#txtClientes_FiltroRelatorio_CpfCnpj").val();
    }

    if (pRelatorio == "R027") {
        lUrl += "&CodigoGradual=" + $("#txtCodigoGradual").val()
             + "&CodigoExterno=" + $("#txtCodigoExterno").val();
    }

    if (pRelatorio == "R029") {
        lUrl += "&DataInicial=" + $("#txtClientes_FiltroRelatorio_DataInicial").val()
             + "&DataFinal=" + $("#txtClientes_FiltroRelatorio_DataFinal").val()
             +"&CodCliente=" + $("#txtClientes_FiltroRelatorio_CodCliente").val();
    }

    window.open(lUrl);

    return false;
}

function btnDBM_ImprimirRelatorio_Click()
{
    var lUrl = "DBM/Relatorios/" + $("#cboDBM_FiltroRelatorio_Relatorio").val() + ".aspx?Acao=CarregarComoCSV";  
    
    lUrl += "&DataInicial="   + $("#txtDBM_FiltroRelatorio_DataInicial").val()
         +  "&DataFinal="     + $("#txtDBM_FiltroRelatorio_DataFinal").val()
         +  "&DataOperacao="  + $("#txtDBM_FiltroRelatorio_Operacao").val()
         +  "&CodAssessor="   + $("#txtDBM_FiltroRelatorio_CodAssessor").val()
         +  "&mercado="       + $("#cboDBM_FiltroRelatorio_Mercado").val()
         +  "&Filial="        + $("#cboDBM_FiltroRelatorio_Filial").val()
         +  "&CodigoCliente=" + $("#txtDBM_FiltroRelatorio_CodigoCliente").val();

    window.open(lUrl);

    return false;
}

function GradIntra_RelatoriosDBM_DesabilitarFiltrosParaAssessores(pSender)
{
    if ($("#hddIdAssessorLogadoFilial").val() != "")
    {
        $("#cboDBM_FiltroRelatorio_Filial").prop("disabled", true)
                                           .val($("#hddIdAssessorLogadoFilial").val());
    }

    if ($("#hddIdAssessorLogado").val() != "")
    {
        $("#txtDBM_FiltroRelatorio_CodAssessor").prop("disabled", true)
                                                .val($("#hddIdAssessorLogado").val());
    }
}

function GradIntra_Relatorios_Gerais_PorAssessor_FiltrarPorLetra(pSender)
{
    var lSender = $(pSender);

    lSender.parent().find("label").removeClass("DestaqueLetraSelecaoRelatorioContaCorrente");

    lSender.addClass("DestaqueLetraSelecaoRelatorioContaCorrente");
    
    var lRelatorio = $("#cboClientes_FiltroRelatorio_Relatorio").val();

    $(".pnlRelatorio").show();

    var Assessor = "";

    if (lRelatorio == "R018") 
    {
        lAssessor = $("#cmbRelatorio018_Assessores").val();
    }

    var lClientes = "";
    if (lRelatorio == "R018")
    {
        lClientes = $("#lstRelatorio018_ClientesAssessor").val();
    }
    else 
    {
        lClientes = $("#cmbClientes_FiltroAssessor").val();
    }

    var lUrl   = "Clientes/Relatorios/" + lRelatorio + ".aspx";
    var lDados = { Acao                : "BuscarPorLetra"
                 , DataFinal           : $("#txtClientes_FiltroRelatorio_DataFinal").val()
                 , DataInicial         : $("#txtClientes_FiltroRelatorio_DataInicial").val()
                 , DataVencimentoTermo : $("#txtClientes_FiltroRelatorio_Custodia_DataVencimentoTermo").val()
                 , TipoMercado         : $("#rdbRelatorio_Cliente_NotaCorretagem_TipoMercado_Movimento").val()
                 , LetraSelecionada    : lSender.html()
                 , CustodiaMercado     : $("#cboClientes_FiltroRelatorio_Custodia_Mercado").val()
                 , Bolsa               : $("#cboClientes_FiltroRelatorio_Custodia_Bolsa").val()
                 , cliente             : lClientes
                 , Assessor            : lAssessor
                 , Parte               : gRelatorios_UltimaParte };

    gRelatorios_UltimaParte = 0;
    
    $(".divRelatorioFinanceiroPorAssessorResultado").remove(); //--> Limpando a tela para insersão dos novos resultados.
    $(".divGradualAssinaturaRodape").remove();

    $("#divGradIntra_Relatorios_Gerais_ContaCorrente_CarregandoConteudo").show();

    GradIntra_CarregarHtmlVerificandoErro( gRelatorios_UrlDoUltimo
                                         , lDados
                                         , null
                                         , GradIntra_Relatorios_Complexos_ReceberParte_CallBack);
    return false;
}

function Clientes_FiltroRelatorio_Custodia_Mercado_OnChange(pSender)
{
    if ("TER" == $(pSender).val())
    {
        $("#divClientes_FiltroRelatorio_Custodia_DataVencimentoTermo").show();
    }
    else
    {
        $("#divClientes_FiltroRelatorio_Custodia_DataVencimentoTermo").hide();
    }
}

function Carregar_FiltroClientesPorAssessor(pSender) 
{
    var lUrl = "Clientes/Relatorios/R018.aspx";

    if (gGradIntra_Navegacao_SistemaAtual == "Relatorios" && gGradIntra_Navegacao_SubSistemaAtual == "Gerais")
    {
        if (pSender.id == "cmbRelatorio018_Assessores") 
        {
            GradIntra_CarregarJsonVerificandoErro(lUrl
                , { Acao: "CarregarClientes", Assessor: $(pSender).val() }
                , Carregar_FiltroClientesPorAssessor_CallBack);
        }
    }
}

function Carregar_FiltroClientesPorAssessor_CallBack(pResposta) 
{
    if (pResposta.ObjetoDeRetorno) 
    {
        $('#lstRelatorio018_ClientesAssessor').empty();

        $.each(pResposta.ObjetoDeRetorno, function (i, item) 
        {
            $('#lstRelatorio018_ClientesAssessor').append($('<option>',
            {
                value: item.codigo,
                text: item.nome
            }));
        });
    }
}





