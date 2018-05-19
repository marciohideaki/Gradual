using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Interface.Dados;
using Gradual.OMS.Library.Db;

namespace Gradual.OMS.Sistemas.Interface.Persistencias
{
    public class GrupoFuncionalidadeDbLib
    {
        private Dictionary<string, string> dicionarioDados = new Dictionary<string, string>();

        public GrupoFuncionalidadeDbLib()
        {
            // Cria o dicionario para transformar nomes da consulta em nomes do banco de dados
            dicionarioDados.Add("CodigoFuncionalidade", "ID_FUNCIONALIDADE");
        }
         
        public GrupoFuncionalidadeInfo Salvar(GrupoFuncionalidadeInfo pGrupoFuncionalidade)
        {
            // Varre as funcionalidades
            foreach (FuncionalidadeInfo pFuncionalidade in pGrupoFuncionalidade.Funcionalidades)
            {
                // Monta parametros da procedure
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("CD_FUNCIONALIDADE", pFuncionalidade.CodigoFuncionalidade);
                parametros.Add("ID_SISTEMA", pFuncionalidade.CodigoSistema);
                parametros.Add("DS_FUNCIONALIDADE", pFuncionalidade.NomeFuncionalidade);
                parametros.Add("CD_FUNCIONALIDADE_GRUPO", pGrupoFuncionalidade.CodigoGrupoFuncionalidade);
                parametros.Add("ID_FUNCIONALIDADE", 0);

                // Salva o usuário
                SqlDbLib.Default.ExecutarProcedure("prc_TB_FUNCIONALIDADE_ins", parametros, new List<string>() { "ID_FUNCIONALIDADE" });

                //Exclui permissões existentes para a funcionalidade
                SqlDbLib.Default.ExecutarProcedure(
                        "prc_TB_FUNCIONALIDADE_PERMISSAO_del",
                        "ID_FUNCIONALIDADE", parametros["ID_FUNCIONALIDADE"]);

                // Salva as permissões
                foreach (string permissaoAssociada in pFuncionalidade.Permissoes)
                    SqlDbLib.Default.ExecutarProcedure(
                        "prc_TB_FUNCIONALIDADE_PERMISSAO_ins",
                        "ID_FUNCIONALIDADE", parametros["ID_FUNCIONALIDADE"],
                        "ID_PERMISSAO", permissaoAssociada);

                //Exclui os perfis associados a esta funcionalidade
                SqlDbLib.Default.ExecutarProcedure(
                        "prc_TB_FUNCIONALIDADE_PERFIL_del",
                        "ID_FUNCIONALIDADE", parametros["ID_FUNCIONALIDADE"]);

                // Salva os grupos
                foreach (string itemPerfil in pFuncionalidade.Perfis)
                    SqlDbLib.Default.ExecutarProcedure(
                        "prc_TB_FUNCIONALIDADE_PERFIL_ins",
                        "ID_FUNCIONALIDADE", parametros["ID_FUNCIONALIDADE"],
                        "ID_PERFIL", itemPerfil);
            }

            // Apaga do banco eventuais funcionalidades removidas
            string codigosFuncionalide =
                string.Join(
                    ",",
                    (from f in pGrupoFuncionalidade.Funcionalidades
                     select f.CodigoFuncionalidade).ToArray());
            SqlDbLib.Default.ExecutarProcedure(
                "prc_TB_FUNCIONALIDADE_PERFIL_del", 
                "CD_FUNCIONALIDADE_GRUPO", pGrupoFuncionalidade.CodigoGrupoFuncionalidade, 
                "NOT_IN_LISTA_CD_FUNCIONALIDADE", codigosFuncionalide);

            // Retorna a entidade com os complementos
            return pGrupoFuncionalidade;
        }

        public GrupoFuncionalidadeInfo Receber(string codigo)
        {
            // Pede o detalhe do grupo
            DataSet ds = SqlDbLib.Default.ExecutarProcedure(
                "prc_TB_FUNCIONALIDADE_GRUPO_sel",
                "CD_FUNCIONALIDADE_GRUPO", codigo,
                "@RECEBER_FUNCIONALIDADES", 1,
                "@RECEBER_PERMISSOES", 1,
                "@RECEBER_PERFIL", 1);

            // Apenas se encontrou o grupo...
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Cria o grupo
                GrupoFuncionalidadeInfo grupo = 
                    new GrupoFuncionalidadeInfo() 
                    { 
                        CodigoGrupoFuncionalidade = (string)ds.Tables[0].Rows[0]["CD_FUNCIONALIDADE_GRUPO"] 
                    };

                // Varre a lista de funcionalidades
                foreach (DataRow drFuncionalidade in ds.Tables[1].Rows)
                {
                    // Cria a funcionalidade
                    FuncionalidadeInfo lFuncionalidade = criarFuncionalidade(drFuncionalidade);

                    // Localiza as permissoes e adiciona
                    DataRow[] drsPermissoes = ds.Tables[2].Select("ID_FUNCIONALIDADE = " + drFuncionalidade["ID_FUNCIONALIDADE"].ToString());
                    foreach (DataRow drPermissao in drsPermissoes)
                        lFuncionalidade.Permissoes.Add((string)drPermissao["CD_PERMISSAO"]);

                    // Localiza os perfis
                    DataRow[] drsPerfis = ds.Tables[3].Select("ID_FUNCIONALIDADE = " + drFuncionalidade["ID_FUNCIONALIDADE"].ToString());
                    foreach (DataRow drPerfil in drsPerfis)
                        lFuncionalidade.Perfis.Add((string)drPerfil["CD_PERFIL"]);

                    // Adiciona funcionalidade no grupo
                    grupo.Funcionalidades.Add(lFuncionalidade);
                }

                // Retorna
                return grupo;
            }
            else
            {
                // Não encontrou o grupo
                return null;
            }
        }

        public void Remover(string codigo)
        {
            SqlDbLib.Default.ExecutarProcedure("prc_TB_FUNCIONALIDADE_del", "CD_FUNCIONALIDADE", codigo);
        }

        public List<FuncionalidadeInfo> Consultar(List<CondicaoInfo> condicoes)
        {
            // Prepara o retorno
            List<FuncionalidadeInfo> retorno = new List<FuncionalidadeInfo>();

            // Monta lista de parametros para a procedure através das condições
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            foreach (CondicaoInfo condicao in condicoes)
                if (dicionarioDados.ContainsKey(condicao.Propriedade))
                    parametros.Add(dicionarioDados[condicao.Propriedade], condicao.Valores[0]);

            // Faz a chamada da procedure
            DataSet ds = SqlDbLib.Default.ExecutarProcedure("prc_TB_FUNCIONALIDADE_lst", parametros);

            // Monta lista de usuarios
            foreach (DataRow dr in ds.Tables[0].Rows)
                retorno.Add(criarFuncionalidade(dr));

            // Retorna
            return retorno;
        }

        private FuncionalidadeInfo criarFuncionalidade(DataRow dr)
        {
            return
                new FuncionalidadeInfo()
                {
                    CodigoFuncionalidade = dr["CD_FUNCIONALIDADE"] != DBNull.Value ? (string)dr["CD_FUNCIONALIDADE"] : null,
                    CodigoSistema = dr["ID_SISTEMA"] != DBNull.Value ? (int)dr["ID_SISTEMA"] : 0,
                    NomeFuncionalidade = dr["DS_FUNCIONALIDADE"] != DBNull.Value ? (string)dr["DS_FUNCIONALIDADE"] : null
                };
        }
    }
}

