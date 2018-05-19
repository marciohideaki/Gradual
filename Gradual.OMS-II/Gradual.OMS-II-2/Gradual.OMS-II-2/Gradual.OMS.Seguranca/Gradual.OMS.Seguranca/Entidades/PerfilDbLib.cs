using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Seguranca.Lib;
using Gradual.OMS.Library;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class PerfilDbLib : IEntidadeDbLib<PerfilInfo>
    {
        private DbLib _dbLib = new DbLib("Seguranca");

        public const string NomeProcSalvar = "prc_Perfil_salvar";
        public const string NomeProcSel = "prc_Perfil_sel";
        public const string NomeProcLst = "prc_Perfil_sel";
        public const string NomeProcDel = "prc_Perfil_del";

        public PerfilDbLib() { }

        #region IEntidadeDbLib<PerfilInfo> Members

        public ConsultarObjetosResponse<PerfilInfo> ConsultarObjetos(ConsultarObjetosRequest<PerfilInfo> parametros)
        {
            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

           // Monta a execução da procedure e executa

            foreach (CondicaoInfo condicao in parametros.Condicoes)
            {
                paramsProc.Add("@" + condicao.Propriedade, condicao.Valores[0]);
            }

            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcLst, paramsProc, new List<string>());

            // Preenche a coleção resultado
            List<PerfilInfo> resultado = new List<PerfilInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
                resultado.Add(this.MontarObjeto(dr));

            // Retorna
            return
                new ConsultarObjetosResponse<PerfilInfo>()
                {
                    Resultado = resultado
                };

        }

        public ReceberObjetoResponse<PerfilInfo> ReceberObjeto(ReceberObjetoRequest<PerfilInfo> parametros)
        {
            // Faz a consulta no banco
            DataSet ds =
                _dbLib.ExecutarProcedure(
                    NomeProcSel, "@CodigoPerfil", parametros.CodigoObjeto);

            // Monta o objeto
            PerfilInfo perfil = ds.Tables[0].Rows.Count > 0 ? this.MontarObjeto(ds.Tables[0].Rows[0]) : null;

            // Achou o usuário?
            if (perfil != null)
            {
                // Carrega permissoes
                perfil.Permissoes =
                    new PerfilPermissaoDbLib().ConsultarObjetos(
                        new List<CondicaoInfo>() 
                        { 
                            new CondicaoInfo("CodigoPerfil", CondicaoTipoEnum.Igual, perfil.CodigoPerfil)
                        });
            }

            // Retorna
            return
                new ReceberObjetoResponse<PerfilInfo>()
                {
                    Objeto = perfil
                };
        }

        public RemoverObjetoResponse<PerfilInfo> RemoverObjeto(RemoverObjetoRequest<PerfilInfo> parametros)
        {
            // Monta a execução da procedure
            _dbLib.ExecutarProcedure(
                NomeProcDel,
                "@CodigoPerfil", parametros.CodigoObjeto);

            // Retorna
            return new RemoverObjetoResponse<PerfilInfo>();
        }

        public SalvarObjetoResponse<PerfilInfo> SalvarObjeto(SalvarObjetoRequest<PerfilInfo> parametros)
        {
            DataSet ds = null;

            // Monta parametros
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            int codigoPerfil = 0;

            if (int.TryParse(parametros.Objeto.CodigoPerfil, out codigoPerfil))
                paramsProc.Add("@CodigoPerfil", codigoPerfil);
            paramsProc.Add("@NomePerfil", parametros.Objeto.NomePerfil);

            ds = _dbLib.ExecutarProcedure(NomeProcSalvar, paramsProc, new List<string>());

            // Execução a procedure

            // Monta o objeto
            PerfilInfo perfil =
                this.ReceberObjeto(new ReceberObjetoRequest<PerfilInfo>()
                {
                    CodigoObjeto = this.MontarObjeto(ds.Tables[0].Rows[0]).CodigoPerfil
                }).Objeto;
                

            // Salva permissões
            salvarPermissoes(parametros.Objeto, perfil);

            parametros.Objeto = perfil;

            // Retorna
            return
                new SalvarObjetoResponse<PerfilInfo>()
                {
                    Objeto = perfil
                };

        }

        public PerfilInfo MontarObjeto(System.Data.DataRow dr)
        {
            return new PerfilInfo()
            {
                CodigoPerfil = dr["CodigoPerfil"].ToString(),
                NomePerfil = dr["NomePerfil"].ToString()
            };
        }

        #endregion

        /// <summary>
        /// Salva a lista de permissoes
        /// </summary>
        /// <param name="usuarioOriginal"></param>
        /// <param name="usuarioSalvo"></param>
        private void salvarPermissoes(PerfilInfo perfilOriginal, PerfilInfo perfilSalvo)
        {
            // Inicializa
            PerfilPermissaoDbLib perfilPermissaoDbLib = new PerfilPermissaoDbLib();

            // Pega lista de contas atuais
            List<PermissaoAssociadaInfo> permissoesAtuais =
                perfilPermissaoDbLib.ConsultarObjetos(
                    new List<CondicaoInfo>() 
                    { 
                        new CondicaoInfo("CodigoPerfil", CondicaoTipoEnum.Igual, perfilSalvo.CodigoPerfil)
                    });

            // Varre a lista de que foi pedido para salvar
                foreach (PermissaoAssociadaInfo permissaoAssociada in perfilOriginal.Permissoes)
                {
                    if (permissoesAtuais.Find(p => p.CodigoPermissao == permissaoAssociada.CodigoPermissao) == null)
                    {
                        perfilPermissaoDbLib.SalvarObjeto(permissaoAssociada, perfilSalvo.CodigoPerfil);
                    }
                }
                

            // Verifica se existem contas a remover
            foreach (PermissaoAssociadaInfo permissaoAssociada in permissoesAtuais)
                if (perfilOriginal.Permissoes.Find(p => p.CodigoPermissao == permissaoAssociada.CodigoPermissao) == null)
                    perfilPermissaoDbLib.RemoverObjeto(perfilSalvo.CodigoPerfil, permissaoAssociada.CodigoPermissao);

            // Atribui a coleção ao cliente salvo
            perfilSalvo.Permissoes = perfilOriginal.Permissoes;
        }

    }
}
