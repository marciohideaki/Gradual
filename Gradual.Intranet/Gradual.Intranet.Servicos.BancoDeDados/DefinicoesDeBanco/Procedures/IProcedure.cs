using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data.Common;

namespace Gradual.Intranet.Servicos.BancoDeDados.DefinicoesDeBanco.Procedures
{
    public interface IProcedure
    {

        /// <summary>
        /// Nome da Proc
        /// </summary>
        string NomeProcedure { get; set; }
        /// <summary>
        /// Lista de parametros para qua a camada de dados adicione cada uma 
        /// </summary>
        List<Parametro> Parametros { get; set; }
        /// <summary>
        /// Metodo para adicionar os parametros "tipados" na lista
        /// </summary>
        void Formatar();

        /// <summary>
        /// Preenche uma classe AcessoDados com os parâmetros de si mesmo
        /// </summary>
        /// <param name="pAcessaDados">Instancia do AcessaDados para preencher</param>
        /// <param name="pComando">Comando que irá rodar</param>
        void PreencherAcessoDados(ref AcessaDados pAcessaDados, ref DbCommand pComando);
    }
}
