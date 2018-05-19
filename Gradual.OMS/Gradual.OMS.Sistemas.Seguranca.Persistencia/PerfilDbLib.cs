using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Library.Db;

namespace Gradual.OMS.Sistemas.Seguranca.Persistencias
{
    public class PerfilDbLib
    {
        private Dictionary<string, string> dicionarioDados = new Dictionary<string, string>();

        public PerfilDbLib()
        {
            // Cria o dicionario para transformar nomes da consulta em nomes do banco de dados
            dicionarioDados.Add("CodigoPerfil", "CD_PERFIL");
        }

        public PerfilInfo Salvar(PerfilInfo perfil)
        {
            // Monta parametros da procedure
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("CD_PERFIL", perfil.CodigoPerfil);
            parametros.Add("NM_PERFIL", perfil.NomePerfil);
            parametros.Add("ID_PERFIL", 0);

            // Salva o perfil
            SqlDbLib.Default.ExecutarProcedure("prc_TB_PERFIL_ins", parametros, new List<string>() { "ID_PERFIL" });

            //Exclui permissões existentes para o PERFIL
            SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_PERFIL_PERMISSAO_del",
                    "ID_PERFIL", parametros["ID_PERFIL"]);

            // Salva as permissões
            foreach (PermissaoAssociadaInfo permissaoAssociada in perfil.Permissoes)
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_PERFIL_PERMISSAO_ins",
                    "ID_PERFIL", parametros["ID_PERFIL"],
                    "CD_PERMISSAO", permissaoAssociada.CodigoPermissao,
                    "MN_PERMISSAO_STATUS", permissaoAssociada.Status.ToString());

            // Retorna a entidade com os complementos
            return perfil;
        }

        public PerfilInfo Receber(string codigo)
        {
            // Pede o detalhe do perfil
            DataSet ds = 
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_PERFIL_sel",
                    "CD_PERFIL", codigo,
                    "RECEBER_PERMISSOES", 1);

            // Apenas se encontrou o PERFIL...
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Cria o perfil
                PerfilInfo lperfil = criarPerfil(ds.Tables[0].Rows[0]);

                // Permissoes
                foreach (DataRow dr in ds.Tables[1].Rows)
                    lperfil.Permissoes.Add(
                        new PermissaoAssociadaInfo()
                        {
                            CodigoPermissao = (string)dr["CD_PERMISSAO"],
                            Status = (PermissaoAssociadaStatusEnum)Enum.Parse(typeof(PermissaoAssociadaStatusEnum), (string)dr["MN_PERMISSAO_STATUS"])
                        });

                // Retorna
                return lperfil;
            }
            else
            {
                // Não encontrou o PERFIL
                return null;
            }
        }

        public void Remover(string codigo)
        {
            SqlDbLib.Default.ExecutarProcedure("prc_TB_PERFIL_del", "CD_PERFIL", codigo);
        }

        public List<PerfilInfo> Consultar(List<CondicaoInfo> condicoes)
        {
            // Prepara o retorno
            List<PerfilInfo> retorno = new List<PerfilInfo>();

            // Monta lista de parametros para a procedure através das condições
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            foreach (CondicaoInfo condicao in condicoes)
                if (dicionarioDados.ContainsKey(condicao.Propriedade))
                    parametros.Add(dicionarioDados[condicao.Propriedade], condicao.Valores[0]);

            // Faz a chamada da procedure
            DataSet ds = SqlDbLib.Default.ExecutarProcedure("prc_TB_PERFIL_lst", parametros);

            // Monta lista de perfis
            foreach (DataRow dr in ds.Tables[0].Rows)
                retorno.Add(criarPerfil(dr));

            // Retorna
            return retorno;
        }

        private PerfilInfo criarPerfil(DataRow dr)
        {
            return
                new PerfilInfo()
                {
                    CodigoPerfil = dr["CD_PERFIL"] != DBNull.Value ? (string)dr["CD_PERFIL"] : null,
                    NomePerfil = dr["NM_PERFIL"] != DBNull.Value ? (string)dr["NM_PERFIL"] : null
                };
        }
    }
}
