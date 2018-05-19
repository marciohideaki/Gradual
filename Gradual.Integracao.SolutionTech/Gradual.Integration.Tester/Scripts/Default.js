function Carregar(pUrl, pDadosDoRequest, pCallBackDeSucesso) {
    ///<summary>Carrega o JSON de uma chamada ajax.</summary>
    ///<param name="pUrl"  type="String">URL que será chamada.</param>
    ///<param name="pDadosDoRequest"  type="Objeto_JSON">Dados para a chamada ajax.</param>
    ///<param name="pCallBackDeSucesso"  type="Função_Javascript">(opcional) Função para chamar em caso de sucesso.</param>
    ///<returns>void</returns>

    if (!gBloquearAjax) {
        //console.log("GradSite_CarregarJsonVerificandoErro(" + pUrl + ")");

        $.ajax({
            url: "http://localhost:51533/IntegracaoPortalHB.aspx"
                , type: "post"
                , cache: false
                , dataType: "json"
                , data: pDadosDoRequest
                , success: function (pResposta) { GradSite_CarregarVerificandoErro_CallBack(pResposta, pCallBackDeSucesso); }
                , error: GradSite_TratarRespostaComErro
        });

        gBloquearAjax = true;

        //por segurança, caso um request não retorne ou dê problema, a flag é removida de qualquer forma depois de 3 segundos:

        window.setTimeout(function () { gBloquearAjax = false; }, 3000);
    }
    else {
        //console.log("Chamada ajax bloqueada: " + pUrl);
    }
}

function Redirecionar()
{
    $().redirect('demo.php', { 'Host': 'hb.gradualinvestimentos.com.br', 'TokenType': 'Integration2','Token':'-----BEGIN PGP MESSAGE-----\r\nVersion: BCPG C# v1.8.1.0\r\n\r\nhQIMAyXvYYMOcMZ4AQ//fauKMrugJbnd6gOfIkJztyAc0J4XfJd4WSB6lmN48D53\r\nrq5EVb6XLhr8qGozGwxzRyTLUbOhPqDGiFr8vnXk8UHm1z7ey+qGPJtKZExofaMu\r\nPCPazV856mwiPz1t+uPYGkvom5bSQKjB7XGeTiKiI1J1JGkk/5598Xi2Lwple/LM\r\nYvLeW8MQ9XOzERUc2Rydg0ToZvrTDlSI2jRg3D8WzR6PHHdbXIxGSSrpQ6dKz0mM\r\n2bHANDS0TFQzlf5cLcq88lAWlPAJajioQf0ackJbLTX7K5te98seVl3OI/EbWE/6\r\nHc7Y4uhPc7z/zNKUChhHbZQn5USzDB7cNQz+2XccttJwL7diKadOszM6yfAfM6OC\r\ns0p5yRcjGhyKp8qP1FjhjySbcXqsfRSsJZtqh9rj/28Y3wRwC5lzOFqv309gJJvC\r\n++brsXgANnSWhXW72a1TT3EB6idpzpEh8oTggTBxTNLwJnAmhFlopnMpAGgERfgS\r\nqhhsSsANo/HRy0wCLWp5vPts7emlC+xoODlxCBqVsqHavl17lScG8wYRdOZYYezh\r\n7GLMMeTUENL2fP3UT/lnb9lJ5v1NAQpEpyydb4hBvuEtL1pomm99ZwbVeflfGMxZ\r\nFmWwUtI/mUxX1ZjZRIPRDqdKvfMAjMIqEl533XGXSDa9KIvtwWqA85NeVBGvnyXS\r\nwIIBraN2ZLWlyy67k9Uhu4n06oYmAIgyJbhOLfWNOr5VdtDLKs+IAszlt/Didkum\r\nprhsMWLmOMNWuIN6eJQuIIqyRP7JB9edSj+kiBNm1dD2G3Ie81T4IX5teiORGrKf\r\nNJ+ixV3u7KOz4s9iDbJbZbEbEGe2gwM5DupoHKttQXnDon9XGYf+niJ1jh3sRs/U\r\nE3zhBPCw0uYNvllMvBImiLesNTg2WSfm/SKwOz7zOi0on4TGaAOdN10TJpNAGExM\r\nQSMCmAAE1Sv6Z8NrB0llsA4Tbn2Rj32RBwr2QsB8xKSgQuLy/Y9u7GB9crXGYxLq\r\nObv0wo2RfjybYkidF6gziBlSG/AIvMHGOdgUmxPIi3T9lttp7oAQGntIysDDT8Bo\r\nQyGLv3cqiz9xpqjN7TBdRrKbNspjY+hDZTkPnfw5ixA/wRi+\r\n=dtdh\r\n-----END PGP MESSAGE-----\r\n' });
}