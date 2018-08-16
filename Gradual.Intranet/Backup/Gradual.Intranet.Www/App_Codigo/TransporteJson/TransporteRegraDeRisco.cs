using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRegraDeRisco
    {
        #region Propriedades

        public int Id { get; set; }

        public string Descricao { get; set; }

        public List<TransporteRegraDeRiscoParametro> Parametros { get; set; }

        #endregion
    }

    public class TransporteRegraDeRiscoParametro
    {
        #region Propriedades

        public int Id { get; set; }

        public string Nome { get; set; }

        /// <summary>
        /// Utilizar validade[opções], sendo que as opções mais comuns são "required", "custom[tipo]", "funcCall[tipo]", length; separá-las por vírgula, não dar espaços
        /// </summary>
        public string Validacoes { get; set; }
        
        /*
         * required se o campo for obrigatório
         * custom[tipo] é pra validar contra uma regex, e elas estão definidas dentro do \Js\Lib\jQuery\Dev\jquery.validationEngine-ptBR.js
         * funcCall[tipo] é pra validar contra uma função js, também tem que ter uma entrada para [tipo] dentro do switch no jquery.validationEngine-ptBR.js
         * length[min,max] é pra validar comprimento mínimo e máximo.
         * 
         * todas essas opções têm que ser passadas dentro de um "container" validade[], como string e sem espaços,. Ex:
         * "validade[required,custom[cnpj]]"
         * "validade[required,length[5,10]]"
         * "validade[funcCall[validatecpfcnpj]]"
         * 
         * para filtrar somente teclas numéricas, incluir a flag "ProibirLetras" antes ou depois do validade[]:
         * "ProibirLetras validade[required]"
         * "validade[required,custom[onlyNumber]] ProibirLetras"
         * 
         * para formatação de data, incluir a flag "Mascara_Data" antes ou depois do validade[]
         * 
         */

        #endregion
    }
}