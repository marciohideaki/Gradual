using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Dados;
using Gradual.OMS.Library.Db;

namespace Gradual.OMS.Sistemas.Interface.Persistencias
{
    public class SistemaDbLib
    {
        private Dictionary<string, string> dicionarioDados = new Dictionary<string, string>();

        public SistemaDbLib()
        {
            // Cria o dicionario para transformar nomes da consulta em nomes do banco de dados
            dicionarioDados.Add("CodigoSistema", "CD_SISTEMA");
        }

        public SistemaInfo Salvar(SistemaInfo sistema)
        {
            // Monta parametros da procedure
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("NM_SISTEMA", sistema.NomeSistema);
            parametros.Add("CD_SISTEMA", sistema.CodigoSistema);
            parametros.Add("ID_SISTEMA", 0);

            // Salva o sistema
            SqlDbLib.Default.ExecutarProcedure("prc_TB_SISTEMA_ins", parametros, new List<string>() { "ID_SISTEMA" });

            // Retorna
            return sistema;
        }

        public SistemaInfo Receber(string codigo)
        {
            // Pede o detalhe do sistema
            DataSet ds = 
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_SISTEMA_sel",
                    "CD_SISTEMA", codigo);

            // Apenas se encontrou o sistema...
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Cria o sistema
                SistemaInfo lSistema = criarSistema(ds.Tables[0].Rows[0]);

                // Retorna
                return lSistema;
            }
            else
            {
                // Não encontrou o Sistema
                return null;
            }
        }

        public void Remover(string codigo)
        {
            SqlDbLib.Default.ExecutarProcedure("prc_TB_SISTEMA_del", "CD_SISTEMA", codigo);
        }

        public List<SistemaInfo> Consultar(List<CondicaoInfo> condicoes)
        {
            // Prepara o retorno
            List<SistemaInfo> retorno = new List<SistemaInfo>();

            // Monta lista de parametros para a procedure através das condições
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            foreach (CondicaoInfo condicao in condicoes)
                if (dicionarioDados.ContainsKey(condicao.Propriedade))
                    parametros.Add(dicionarioDados[condicao.Propriedade], condicao.Valores[0]);

            // Faz a chamada da procedure
            DataSet ds = SqlDbLib.Default.ExecutarProcedure("prc_TB_SISTEMA_lst", parametros);

            // Monta lista de sistemas
            foreach (DataRow dr in ds.Tables[0].Rows)
                retorno.Add(criarSistema(dr));

            // Retorna
            return retorno;
        }

        private SistemaInfo criarSistema(DataRow dr)
        {
            return
                new SistemaInfo()
                {
                    CodigoSistema = dr["CD_SISTEMA"] != DBNull.Value ? (string)dr["CD_SISTEMA"] : null,
                    NomeSistema = dr["NM_SISTEMA"] != DBNull.Value ? (string)dr["NM_SISTEMA"] : null
                };
        }
    }
}
