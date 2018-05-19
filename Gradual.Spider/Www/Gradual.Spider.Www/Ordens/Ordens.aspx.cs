using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

using System.Collections;

namespace Gradual.Spider.Www
{
    public partial class Ordens : PaginaBase
    {
        #region Propriedades
        public static int ItensPorPagina = 40;

        public int TotalDePaginas { get; set; }

        public int TotalDeItens { get; set; }

        public int PaginaAtual { get; set; }

        public IEnumerable Itens { get; set; }
        #endregion

        #region Métodos Private
        private string CarregarDadosIniciais()
        {
            string resp = "páginaok";

            return resp;
        }
        
        private string PesquisarOrdens()
        {



            string a = "\'{\"page\": \"1\", \"total\": \"3\", \"records\": \"13\", \"rows\": [ { \"id\": \"order1\",\"sessao\": \"123451\", \"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\":  \"1000\", \"qtdeExec\":  \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-01\" }, { \"id\": \"order2\",\"sessao\": \"123452\", \"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\":  \"1100\", \"qtdeExec\":  \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-02\" }, { \"id\": \"order3\",\"sessao\": \"123463\", \"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\":  \"1200\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-09-01\" }, { \"id\": \"order4\",\"sessao\": \"1234564\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\":  \"1300\", \"qtdeExec\":  \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-04\" }, { \"id\": \"order5\",\"sessao\": \"1234565\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"1400\", \"qtdeExec\":  \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-05\" }, { \"id\": \"order6\",\"sessao\": \"1234566\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"1500\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-09-06\" }, { \"id\": \"order7\",\"sessao\": \"1234567\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"1600\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-04\" }, { \"id\": \"order8\",\"sessao\": \"1234568\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"1700\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-03\" }, { \"id\": \"order9\",\"sessao\": \"1234569\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"1800\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-09-01\" }, { \"id\": \"order10\", sessao: \"1234560\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"1900\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-03\" }, { \"id\": \"order11\", sessao: \"1234570\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"2000\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-09-01\" }, { \"id\": \"order12\", sessao: \"1234572\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"2100\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-10-02\" }, { \"id\": \"order13\", sessao: \"1234673\",\"ClodId\":  \"123456789123456789\", \"cliente\": \"123\", \"papel\":  \"vale5\", \"qtdeTotal\": \"2200\", \"qtdeExec\": \"800\", \"qtdeRes\": \"200\", \"status\": \"parc. executada\", \"hora\": \"12:00\", \"tipo\": \"válida para o dia\", \"validade\": \"2007-09-01\" }]}\'";

            return a;
        }

        private string PesquisarDetalhes() {

            return " {order1: [	{ id: \"1\",  qtdeExec:100, hora: \"12:00\" },	{ id: \"2\", qtdeExec: 200, hora: \"12:00\" },	{ id: \"3\", qtdeExec: 300, hora: \"12:00\" },	{ id: \"4\", qtdeExec: 400, hora: \"12:00\" },	{ id: \"5\", qtdeExec: 500, hora: \"12:00\" }],order2: [	{ id: \"1\", qtdeExec: 100, hora: \"12:00\" },	{ id: \"2\", qtdeExec: 200, hora: \"12:00\" },	{ id: \"3\", qtdeExec: 300, hora: \"12:00\" }]}";
        }

        #endregion



        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            base.RegistrarRespostasAjax(new string[]{
                CONST_FUNCAO_CASO_NAO_HAJA_ACTION
                , "PesquisarOrdens"
                , "PesquisarDetalhes"
            }
            ,
            new ResponderAcaoAjaxDelegate[]
            {
                CarregarDadosIniciais
                , PesquisarOrdens
                , PesquisarDetalhes

            });
        }

        #endregion
    }
}