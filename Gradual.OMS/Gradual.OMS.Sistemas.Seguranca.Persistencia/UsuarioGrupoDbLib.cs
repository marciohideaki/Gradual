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
    public class UsuarioGrupoDbLib
    {
        private Dictionary<string, string> dicionarioDados = new Dictionary<string, string>();

        public UsuarioGrupoDbLib()
        {
            // Cria o dicionario para transformar nomes da consulta em nomes do banco de dados
            dicionarioDados.Add("CodigoGrupo", "CD_GRUPO");
        }

        public UsuarioGrupoInfo Salvar(UsuarioGrupoInfo UsuarioGrupo)
        {
            // Monta parametros da procedure
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("CD_GRUPO", UsuarioGrupo.CodigoUsuarioGrupo);
            parametros.Add("NM_GRUPO", UsuarioGrupo.NomeUsuarioGrupo);
            parametros.Add("ID_GRUPO", 0);

            // Salva o grupo
            SqlDbLib.Default.ExecutarProcedure("prc_TB_GRUPO_ins", parametros, new List<string>() { "ID_GRUPO" });

            //Exclui permissões existentes para o grupo
            SqlDbLib.Default.ExecutarProcedure(
                "prc_TB_GRUPO_PERMISSAO_del", 
                "ID_GRUPO", parametros["ID_GRUPO"]);
            
            // Salva as permissões
            foreach (PermissaoAssociadaInfo permissaoAssociada in UsuarioGrupo.Permissoes)
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_GRUPO_PERMISSAO_ins",
                    "ID_GRUPO", parametros["ID_GRUPO"],
                    "CD_PERMISSAO", permissaoAssociada.CodigoPermissao,
                    "MN_PERMISSAO_STATUS", permissaoAssociada.Status.ToString());

            //Exclui os perfis existentes para o grupo
            SqlDbLib.Default.ExecutarProcedure(
                "prc_TB_GRUPO_PERFIL_del",
                "ID_GRUPO", parametros["ID_GRUPO"]);

            // Salva os perfis
            foreach (string codigoPerfil in UsuarioGrupo.Perfis)
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_GRUPO_PERFIL_ins",
                    "ID_GRUPO", parametros["ID_GRUPO"],
                    "CD_PERFIL", codigoPerfil);

            // Retorna a entidade com os complementos
            return UsuarioGrupo;
        }

        public UsuarioGrupoInfo Receber(string codGrupo)
        {
            // Pede o detalhe do usuário
            DataSet ds = 
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_GRUPO_sel",
                    "CD_GRUPO", codGrupo,
                    "RECEBER_PERMISSOES",1,
                    "RECEBER_PERFIL",1);

            // Apenas se encontrou o grupo.
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Cria o GRUPO
                UsuarioGrupoInfo UsuarioGrupo = criarUsuarioGrupo(ds.Tables[0].Rows[0]);

                // Permissoes
                foreach (DataRow dr in ds.Tables[1].Rows)
                    UsuarioGrupo.Permissoes.Add(
                        new PermissaoAssociadaInfo()
                        {
                            CodigoPermissao = (string)dr["CD_PERMISSAO"],
                            Status = (PermissaoAssociadaStatusEnum)Enum.Parse(typeof(PermissaoAssociadaStatusEnum), (string)dr["MN_PERMISSAO_STATUS"])
                        });

                // Perfis
                foreach (DataRow dr in ds.Tables[2].Rows)
                    UsuarioGrupo.Perfis.Add((string)dr["CD_PERFIL"]);

                // Retorna
                return UsuarioGrupo;
            }
            else
            {
                // Não encontrou o grupo
                return null;
            }
        }

        public void Remover(string codigo)
        {
            SqlDbLib.Default.ExecutarProcedure("prc_TB_UsuarioGrupo_del", "CD_Grupo", codigo);
        }

        public List<UsuarioGrupoInfo> Consultar(List<CondicaoInfo> condicoes)
        {
            // Prepara o retorno
            List<UsuarioGrupoInfo> retorno = new List<UsuarioGrupoInfo>();

            // Monta lista de parametros para a procedure através das condições
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            foreach (CondicaoInfo condicao in condicoes)
                if (dicionarioDados.ContainsKey(condicao.Propriedade))
                    parametros.Add(dicionarioDados[condicao.Propriedade], condicao.Valores[0]);

            // Faz a chamada da procedure
            DataSet ds = SqlDbLib.Default.ExecutarProcedure("prc_TB_USUARIO_GRUPO_lst", parametros);

            // Monta lista de UsuarioGrupos
            foreach (DataRow dr in ds.Tables[0].Rows)
                retorno.Add(criarUsuarioGrupo(dr));

            // Retorna
            return retorno;
        }

        private UsuarioGrupoInfo criarUsuarioGrupo(DataRow dr)
        {
            return
                new UsuarioGrupoInfo()
                {
                    CodigoUsuarioGrupo = dr["CD_GRUPO"] != DBNull.Value ? (string)dr["CD_GRUPO"] : null,
                    NomeUsuarioGrupo = dr["NM_GRUPO"] != DBNull.Value ? (string)dr["NM_GRUPO"] : null
                };
        }
    }
}
