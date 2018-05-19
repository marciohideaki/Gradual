using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Library.Db;

namespace Gradual.OMS.Sistemas.Seguranca.Persistencias
{
    public class UsuarioDbLib
    {
        private Dictionary<string, string> dicionarioDados = new Dictionary<string, string>();

        public UsuarioDbLib()
        {
            // Cria o dicionario para transformar nomes da consulta em nomes do banco de dados
            dicionarioDados.Add("CodigoUsuario", "CD_USUARIO");
        }

        public UsuarioInfo Salvar(UsuarioInfo usuario)
        {
            // Monta parametros da procedure
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("CD_USUARIO", usuario.CodigoUsuario);
            parametros.Add("NM_USUARIO", usuario.Nome);
            parametros.Add("NM_ABREVIADO", usuario.NomeAbreviado);
            parametros.Add("DS_ORIGEM", usuario.Origem);
            parametros.Add("VL_SENHA", usuario.Senha);
            parametros.Add("VL_ASSINATURA_ELETRONICA", usuario.AssinaturaEletronica);
            parametros.Add("MN_USUARIO_STATUS", usuario.Status.ToString());
            parametros.Add("ID_USUARIO", 0);

            // Salva o usuário
            SqlDbLib.Default.ExecutarProcedure("prc_TB_USUARIO_ins", parametros, new List<string>() { "ID_USUARIO" });

            //Exclui permissões existentes para o usuário
            SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_USUARIO_PERMISSAO_del",
                    "ID_USUARIO", parametros["ID_USUARIO"]);
            
            // Salva as permissões
            foreach (PermissaoAssociadaInfo permissaoAssociada in usuario.Permissoes)
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_USUARIO_PERMISSAO_ins", 
                    "ID_USUARIO", parametros["ID_USUARIO"], 
                    "CD_PERMISSAO", permissaoAssociada.CodigoPermissao,
                    "MN_USUARIO_PERMISSAO", permissaoAssociada.Status.ToString());

            //Exclui os grupos existentes para o usuário
            SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_USUARIO_GRUPO_del",
                    "ID_USUARIO", parametros["ID_USUARIO"]);
           
            // Salva os grupos
            foreach (string codigoGrupo in usuario.Grupos)
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_USUARIO_GRUPO_ins",
                    "ID_USUARIO", parametros["ID_USUARIO"],
                    "CD_GRUPO", codigoGrupo);

            //Exclui os grupos existentes para o usuário
            SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_USUARIO_PERFIL_del",
                    "ID_USUARIO", parametros["ID_USUARIO"]);
            
            // Salva os perfis
            foreach (string codigoPerfil in usuario.Perfis)
                SqlDbLib.Default.ExecutarProcedure(
                    "prc_TB_USUARIO_PERFIL_ins",
                    "ID_USUARIO", parametros["ID_USUARIO"],
                    "CD_PERFIL", codigoPerfil);

            // Retorna a entidade com os complementos
            return usuario;
        }

        public UsuarioInfo Receber(string codigo)
        {
            // Pede o detalhe do usuário
            DataSet ds = SqlDbLib.Default.ExecutarProcedure(
                "prc_TB_USUARIO_sel", 
                "CD_USUARIO", codigo,
                "RETORNAR_PERMISSOES", 1, 
                "RETORNAR_GRUPOS", 1,
                "RETORNAR_PERFIS", 1);

            // Apenas se encontrou o usuário...
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Cria o usuário
                UsuarioInfo usuario = criarUsuario(ds.Tables[0].Rows[0]);

                // Permissoes
                foreach (DataRow dr in ds.Tables[1].Rows)
                    usuario.Permissoes.Add(
                        new PermissaoAssociadaInfo() 
                        {
                            CodigoPermissao = (string)dr["CD_PERMISSAO"],
                            Status = (PermissaoAssociadaStatusEnum)Enum.Parse(typeof(PermissaoAssociadaStatusEnum), (string)dr["MN_USUARIO_PERMISSAO"])
                        });

                // Grupos
                foreach (DataRow dr in ds.Tables[2].Rows)
                    usuario.Grupos.Add((string)dr["CD_GRUPO"]);

                // Perfis
                foreach (DataRow dr in ds.Tables[3].Rows)
                    usuario.Perfis.Add((string)dr["CD_PERFIL"]);

                // Retorna
                return usuario;
            }
            else
            {
                // Não encontrou o usuário
                return null;
            }
        }

        public void Remover(string codigo)
        {
            SqlDbLib.Default.ExecutarProcedure("prc_TB_USUARIO_del", "CD_USUARIO", codigo);
        }

        public List<UsuarioInfo> Consultar(List<CondicaoInfo> condicoes)
        {
            // Prepara o retorno
            List<UsuarioInfo> retorno = new List<UsuarioInfo>();

            // Monta lista de parametros para a procedure através das condições
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            foreach (CondicaoInfo condicao in condicoes)
                if (dicionarioDados.ContainsKey(condicao.Propriedade))
                    parametros.Add(dicionarioDados[condicao.Propriedade], condicao.Valores[0]);

            // Faz a chamada da procedure
            DataSet ds = SqlDbLib.Default.ExecutarProcedure("prc_TB_USUARIO_lst", parametros);

            // Monta lista de usuarios
            foreach (DataRow dr in ds.Tables[0].Rows)
                retorno.Add(criarUsuario(dr));

            // Retorna
            return retorno;
        }

        private UsuarioInfo criarUsuario(DataRow dr)
        {
            return 
                new UsuarioInfo()
                {
                    CodigoUsuario = dr["CD_USUARIO"] != DBNull.Value ? (string)dr["CD_USUARIO"] : null,
                    AssinaturaEletronica = dr["VL_ASSINATURA_ELETRONICA"] != DBNull.Value ? (string)dr["VL_ASSINATURA_ELETRONICA"] : null,
                    Nome = dr["NM_USUARIO"] != DBNull.Value ? (string)dr["NM_USUARIO"] : null,
                    NomeAbreviado = dr["NM_ABREVIADO"] != DBNull.Value ? (string)dr["NM_ABREVIADO"] : null,
                    Origem = dr["DS_ORIGEM"] != DBNull.Value ? (string)dr["DS_ORIGEM"] : null,
                    Senha = dr["VL_SENHA"] != DBNull.Value ? (string)dr["VL_SENHA"] : null,
                    Status = dr["MN_USUARIO_STATUS"] != DBNull.Value ? (UsuarioStatusEnum)Enum.Parse(typeof(UsuarioStatusEnum), dr["MN_USUARIO_STATUS"].ToString()) : UsuarioStatusEnum.NaoInformado
                };
        }
    }
}
