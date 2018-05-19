using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class UsuarioGrupoDbLib : IEntidadeDbLib<UsuarioGrupoInfo>
    {

        public const string NomeProcSalvar = "prc_Grupo_salvar";
        public const string NomeProcSel = "prc_Grupo_sel";
        public const string NomeProcLst = "prc_Grupo_sel";
        public const string NomeProcDel = "prc_Grupo_del";

        private DbLib _dbLib = new DbLib("Seguranca");

        public UsuarioGrupoDbLib() { }

        #region IEntidadeDbLib<UsuarioGrupoInfo> Members

        public ConsultarObjetosResponse<UsuarioGrupoInfo> ConsultarObjetos(ConsultarObjetosRequest<UsuarioGrupoInfo> parametros)
        {
            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            //// Verifica se a mensagem pede mais algum filtro
            //Type tipoMensagem = parametros.GetType();
            //if (tipoMensagem == typeof(ConsultarUsuariosPorGrupoRequest))
            //    paramsProc.Add("COD_USUA_GRUP", ((ConsultarUsuariosPorGrupoRequest)parametros).CodigoUsuarioGrupo);

            foreach (CondicaoInfo condicao in parametros.Condicoes)
            {
                if(condicao.Propriedade == "CodigoUsuarioGrupo")
                    paramsProc.Add("@CodigoGrupo", condicao.Valores[0]);
                if(condicao.Propriedade == "NomeUsuarioGrupo")
                    paramsProc.Add("@NomeGrupo", condicao.Valores[0]);
            }

            // Monta a execução da procedure e executa
            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcLst, paramsProc, new List<string>());

            // Preenche a coleção resultado
            List<UsuarioGrupoInfo> resultado = new List<UsuarioGrupoInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
                resultado.Add(this.MontarObjeto(dr));

            // Retorna
            return
                new ConsultarObjetosResponse<UsuarioGrupoInfo>()
                {
                    Resultado = resultado
                };
        }

        public ReceberObjetoResponse<UsuarioGrupoInfo> ReceberObjeto(ReceberObjetoRequest<UsuarioGrupoInfo> parametros)
        {
            // Faz a consulta no banco
            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcSel, "@CodigoGrupo", parametros.CodigoObjeto);

            // Monta o objeto
            UsuarioGrupoInfo usuarioGrupo = ds.Tables[0].Rows.Count > 0 ? this.MontarObjeto(ds.Tables[0].Rows[0]) : null;

            // Achou o usuário?
            if (usuarioGrupo != null)
            {
                // Carrega permissoes
                usuarioGrupo.Permissoes =
                    new UsuarioGrupoPermissaoDbLib().ConsultarObjetos(
                        new List<CondicaoInfo>() 
                        { 
                            new CondicaoInfo("CodigoGrupo", CondicaoTipoEnum.Igual, usuarioGrupo.CodigoUsuarioGrupo)
                        });

               // Carrega perfis
                usuarioGrupo.Perfis =
                    new UsuarioGrupoPerfilDbLib().ConsultarObjetos(
                        usuarioGrupo.CodigoUsuarioGrupo);
            }

            // Retorna
            return
                new ReceberObjetoResponse<UsuarioGrupoInfo>()
                {
                    Objeto = usuarioGrupo
                };

        }

        public RemoverObjetoResponse<UsuarioGrupoInfo> RemoverObjeto(RemoverObjetoRequest<UsuarioGrupoInfo> parametros)
        {
            // Monta a execução da procedure
            _dbLib.ExecutarProcedure(
                NomeProcDel,
                "@CodigoGrupo", parametros.CodigoObjeto);

            // Retorna
            return new RemoverObjetoResponse<UsuarioGrupoInfo>();
        }

        public SalvarObjetoResponse<UsuarioGrupoInfo> SalvarObjeto(SalvarObjetoRequest<UsuarioGrupoInfo> parametros)
        {
            DataSet ds = null;

            // Monta parametros
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            int codigoUsuarioGrupo = 0;

            if (int.TryParse(parametros.Objeto.CodigoUsuarioGrupo, out codigoUsuarioGrupo))
                paramsProc.Add("@CodigoGrupo", codigoUsuarioGrupo);
            paramsProc.Add("@NomeGrupo", parametros.Objeto.NomeUsuarioGrupo);

            ds = _dbLib.ExecutarProcedure(NomeProcSalvar, paramsProc, new List<string>());

            // Execução a procedure

            // Monta o objeto
            UsuarioGrupoInfo usuarioGrupo = this.ReceberObjeto( new ReceberObjetoRequest<UsuarioGrupoInfo>()
            {
                CodigoObjeto = this.MontarObjeto(ds.Tables[0].Rows[0]).CodigoUsuarioGrupo
            }).Objeto;

            // Salva permissões
            salvarPermissoes(parametros.Objeto, usuarioGrupo);

            // Salva perfis
            salvarPerfis(parametros.Objeto, usuarioGrupo);

            // Retorna
            return
                new SalvarObjetoResponse<UsuarioGrupoInfo>()
                {
                    Objeto = usuarioGrupo
                };

        }

        public UsuarioGrupoInfo MontarObjeto(System.Data.DataRow dr)
        {
            return new UsuarioGrupoInfo()
            {
                CodigoUsuarioGrupo = dr["CodigoGrupo"].ToString(),
                NomeUsuarioGrupo = dr["NomeGrupo"].ToString()
            };
        }

        #endregion

        /// <summary>
        /// Salva a lista de permissoes
        /// </summary>
        /// <param name="usuarioOriginal"></param>
        /// <param name="usuarioSalvo"></param>
        private void salvarPermissoes(UsuarioGrupoInfo usuarioGrupoOriginal, UsuarioGrupoInfo usuarioGrupoSalvo)
        {
            // Inicializa
            UsuarioGrupoPermissaoDbLib usuarioGrupoPermissaoDbLib = new UsuarioGrupoPermissaoDbLib();

            // Pega lista de contas atuais
            List<PermissaoAssociadaInfo> permissoesAtuais =
                usuarioGrupoPermissaoDbLib.ConsultarObjetos(
                    new List<CondicaoInfo>() 
                    { 
                        new CondicaoInfo("CodigoGrupo", CondicaoTipoEnum.Igual, usuarioGrupoSalvo.CodigoUsuarioGrupo)
                    });

            // Varre a lista de que foi pedido para salvar
            foreach (PermissaoAssociadaInfo permissaoAssociada in usuarioGrupoOriginal.Permissoes)
                if (permissoesAtuais.Find(p => p.CodigoPermissao == permissaoAssociada.CodigoPermissao) == null)
                {
                    usuarioGrupoPermissaoDbLib.SalvarObjeto(permissaoAssociada, usuarioGrupoSalvo.CodigoUsuarioGrupo);
                }
                

            // Verifica se existem contas a remover
            foreach (PermissaoAssociadaInfo permissaoAssociada in permissoesAtuais)
                if (usuarioGrupoOriginal.Permissoes.Find(p => p.CodigoPermissao == permissaoAssociada.CodigoPermissao) == null)
                    usuarioGrupoPermissaoDbLib.RemoverObjeto(usuarioGrupoSalvo.CodigoUsuarioGrupo, permissaoAssociada.CodigoPermissao);

            // Atribui a coleção ao cliente salvo
            usuarioGrupoSalvo.Permissoes = usuarioGrupoOriginal.Permissoes;
        }

        /// <summary>
        /// Salva a lista de perfis
        /// </summary>
        /// <param name="usuarioOriginal"></param>
        /// <param name="usuarioSalvo"></param>
        private void salvarPerfis(UsuarioGrupoInfo usuarioGrupoOriginal, UsuarioGrupoInfo usuarioGrupoSalvo)
        {
            // Inicializa
            UsuarioGrupoPerfilDbLib usuarioGrupoPerfilDbLib = new UsuarioGrupoPerfilDbLib();

            // Pega lista de grupos atuais
            List<string> perfisAtuais =
                usuarioGrupoPerfilDbLib.ConsultarObjetos(usuarioGrupoSalvo.CodigoUsuarioGrupo);

            // Varre a lista de que foi pedido para salvar
            foreach (string codigoPerfil in usuarioGrupoOriginal.Perfis)
                if (perfisAtuais.Find(p => p == codigoPerfil) == null)
                    usuarioGrupoPerfilDbLib.SalvarObjeto(usuarioGrupoSalvo.CodigoUsuarioGrupo, codigoPerfil);

            // Verifica se existem grupos a remover
            foreach (string codigoPerfil in perfisAtuais)
                if (usuarioGrupoOriginal.Perfis.Find(p => p == codigoPerfil) == null)
                    usuarioGrupoPerfilDbLib.RemoverObjeto(usuarioGrupoSalvo.CodigoUsuarioGrupo, codigoPerfil);

            // Atribui a coleção ao cliente salvo
            usuarioGrupoSalvo.Perfis = usuarioGrupoOriginal.Perfis;
        }

    }
}
