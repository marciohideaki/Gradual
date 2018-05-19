using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Core.OMS.LimiteManager.Db.Dados
{
    public class ErrorMessages
    {
        public static string OK = "OK";
        public static string ERRO = "ERRO";

        public static string ERR_UPDT_MVTO_BVSP = "Problemas na atualizacao das informacoes de movimentacao Bovespa. Registro nao existe ou erro de query.";
        public static string ERR_UPDT_MVTO_BMF_CONTRATO = "Problemas na atualizacao das informacoes de movimentacao BMF contrato. Registro nao existe ou erro de query.";
        public static string ERR_UPDT_MVTO_BMF_INSTRUMENTO = "Problemas na atualizacao das informacoes de movimentacao BMF contrato. Registro nao existe ou erro de query.";
    }
}
