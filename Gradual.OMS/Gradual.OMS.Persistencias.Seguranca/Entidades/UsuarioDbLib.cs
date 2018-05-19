using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using System.Data;

using Gradual.OMS.Library;
using Gradual.OMS.Contratos.Integracao.Sinacor.OMS.Dados;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    class UsuarioDbLib : IEntidadeDbLib<UsuarioInfo>
    {
        public const string NomeProcSalvar = "login_salvar_sp";
        public const string NomeProcSel = "login_sel_sp";
        public const string NomeProcLst = "login_lst_sp";
        public const string NomeProcDel = "login_del_sp";
        public const string NomeProcLstGrupo = "prc_UsuariosPorGrupo_sel";
        public const string NomeProcLstPerfil = "prc_UsuariosPorPerfil_sel";


        private DbLib _dbLib = new DbLib("Cadastro");

        public UsuarioDbLib() { }

        #region IEntidadeDbLib<UsuarioInfo> Members

        public ConsultarObjetosResponse<UsuarioInfo> ConsultarObjetos(ConsultarObjetosRequest<UsuarioInfo> parametros)
        {
            // Lista de parametros para a procedure
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            string nomeProc = NomeProcLst;
            string lEmail = string.Empty;
            foreach (CondicaoInfo condicao in parametros.Condicoes)   
            {
                switch (condicao.Propriedade)
                {
                    case "FiltroNomeOuEmail":
                    case "Email":
                    case "NomeUsuario":
                        paramsProc.Add("@ds_email", condicao.Valores[0]);
                        if(condicao.Propriedade == "Email")
                        {
                            lEmail = condicao.Valores[0].ToString();
                        }
                        break;
                    case "CodigoUsuario":
                        int outParam = 0;
                        if(int.TryParse(condicao.Valores[0].ToString(), out outParam))
                            paramsProc.Add("@id_login", outParam);
                        break;
                    case "Status":
                        int outParamStatus = 0;
                        if(int.TryParse(condicao.Valores[0].ToString(), out outParamStatus))
                            paramsProc.Add("@st_ativo", outParamStatus);
                        break;
                    case "CodigoUsuarioGrupo":
                        int outParamGrupo = 0;
                        if (int.TryParse(condicao.Valores[0].ToString(), out outParamGrupo))
                        {
                            paramsProc.Add("@CodigoGrupo", outParamGrupo);
                            nomeProc = NomeProcLstGrupo;
                        }
                        break;
                    case "CodigoPerfil":
                        int outParamPerfil = 0;
                        if (int.TryParse(condicao.Valores[0].ToString(), out outParamPerfil))
                        {
                            paramsProc.Add("@CodigoPerfil", outParamPerfil);
                            nomeProc = NomeProcLstPerfil;
                        }
                        break;
                }
            }

            //// Verifica se a mensagem pede mais algum filtro
            //Type tipoMensagem = parametros.GetType();
            //if (tipoMensagem == typeof(ConsultarUsuariosPorGrupoRequest))
            //    paramsProc.Add("COD_USUA_GRUP", ((ConsultarUsuariosPorGrupoRequest)parametros).CodigoUsuarioGrupo);

            // Monta a execução da procedure e executa
            DataSet ds =
                _dbLib.ExecutarProcedure(
                    nomeProc, paramsProc, new List<string>());

            // Preenche a coleção resultado
            List<UsuarioInfo> resultado = new List<UsuarioInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
                resultado.Add(this.MontarObjeto(dr));

            if(lEmail != string.Empty)
            {
                string codigoUsuario = resultado.Find(delegate(UsuarioInfo p) { return p.Email == lEmail.Trim(); }).CodigoUsuario;
                resultado.Clear();

                ReceberObjetoResponse<UsuarioInfo> lResponse = ReceberObjeto(new ReceberObjetoRequest<UsuarioInfo>()
                {
                    CodigoObjeto = codigoUsuario
                });

                resultado.Add(lResponse.Objeto);
            }

            // Retorna
            return
                new ConsultarObjetosResponse<UsuarioInfo>()
                {
                    Resultado = resultado
                };

        }

        public ReceberObjetoResponse<UsuarioInfo> ReceberObjeto(ReceberObjetoRequest<UsuarioInfo> parametros)
        {
            DataSet ds = null;

            try
            {
                // Faz a consulta no banco
                ds =
                    _dbLib.ExecutarProcedure(
                        NomeProcSel, "@id_login", int.Parse(parametros.CodigoObjeto));
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, parametros, "Persistencias.Seguranca");
                return null;
            }

            // Monta o objeto
            UsuarioInfo usuario = ds.Tables[0].Rows.Count > 0 ? this.MontarObjeto(ds.Tables[0].Rows[0]) : null;

            // Achou o usuário?
            if (usuario != null)
            {
                // Carrega permissoes
                usuario.Permissoes =
                    new UsuarioPermissaoDbLib().ConsultarObjetos(
                        new List<CondicaoInfo>() 
                        { 
                            new CondicaoInfo("CodigoUsuario", CondicaoTipoEnum.Igual, usuario.CodigoUsuario)
                        });

                // Carrega grupos
                usuario.Grupos =
                    new UsuarioUsuarioGrupoDbLib().ConsultarObjetos(
                        usuario.CodigoUsuario);

                // Carrega perfis
                usuario.Perfis =
                    new UsuarioPerfilDbLib().ConsultarObjetos(
                        usuario.CodigoUsuario);
            }

            // Retorna
            return
                new ReceberObjetoResponse<UsuarioInfo>()
                {
                    Objeto = usuario
                };
        }

        public RemoverObjetoResponse<UsuarioInfo> RemoverObjeto(RemoverObjetoRequest<UsuarioInfo> parametros)
        {
            // Monta a execução da procedure
            _dbLib.ExecutarProcedure(
                NomeProcDel,
                "@id_login", parametros.CodigoObjeto);

            // Retorna
            return new RemoverObjetoResponse<UsuarioInfo>();
        }

        public SalvarObjetoResponse<UsuarioInfo> SalvarObjeto(SalvarObjetoRequest<UsuarioInfo> parametros)
        {
            DataSet ds = null;

            // Monta parametros
            Dictionary<string, object> paramsProc = new Dictionary<string, object>();

            int codigoUsuario = 0;

            if (int.TryParse(parametros.Objeto.CodigoUsuario, out codigoUsuario))
                paramsProc.Add("@id_login", codigoUsuario);
            paramsProc.Add("@ds_nome", parametros.Objeto.Nome);

            if ((parametros.Objeto.Email == null || parametros.Objeto.Email == string.Empty) && parametros.Objeto.NomeAbreviado == "Adm")
                parametros.Objeto.Email = "Admin";

            paramsProc.Add("@ds_email", parametros.Objeto.Email);
            paramsProc.Add("@cd_senha", parametros.Objeto.Senha);
            paramsProc.Add("@cd_assinaturaeletronica", parametros.Objeto.AssinaturaEletronica);

            paramsProc.Add("@st_ativo", parametros.Objeto.Status == UsuarioStatusEnum.Desabilitado ? 0 : 1);

            if (parametros.Objeto.Complementos.Colecao.Count > 0)
            {
                foreach (object loContextoOMS in parametros.Objeto.Complementos.Colecao)
                {
                    if (loContextoOMS is ContextoOMSInfo)
                    {
                        ContextoOMSInfo lcontextoOMS = (ContextoOMSInfo)loContextoOMS;
                        paramsProc.Add("@CodigoPerfilRisco", lcontextoOMS.CodigoPerfilRisco);
                        paramsProc.Add("@CodigoContaCorrente", lcontextoOMS.CodigoContaCorrente);
                        paramsProc.Add("@CodigoCustodia", lcontextoOMS.CodigoCustodia);
                    }
                }
            }
            try
            {
                ds = _dbLib.ExecutarProcedure(NomeProcSalvar, paramsProc, new List<string>());
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, parametros, "Persistencias.Seguranca");
                return null;
            }

            // Execução a procedure

            // Monta o objeto
            UsuarioInfo usuario = this.ReceberObjeto( new ReceberObjetoRequest<UsuarioInfo>()
            {
                CodigoObjeto = this.MontarObjeto(ds.Tables[0].Rows[0]).CodigoUsuario
            }).Objeto;

            // Salva permissões
            salvarPermissoes(parametros.Objeto, usuario);

            // Salva grupos
            salvarGrupos(parametros.Objeto, usuario);

            // Salva perfis
            salvarPerfis(parametros.Objeto, usuario);

            // Retorna
            return
                new SalvarObjetoResponse<UsuarioInfo>()
                {
                    Objeto = usuario
                };
        }

        public UsuarioInfo MontarObjeto(DataRow dr)
        {
            // Cria o usuário
            UsuarioInfo usuario = 
                new UsuarioInfo()
                {
                    CodigoUsuario = dr["id_login"].ToString(),
                    Status = ((int)dr["st_ativo"]) == 0 ? UsuarioStatusEnum.Desabilitado : UsuarioStatusEnum.Habilitado,
                    Nome = dr["ds_nome"].ToString().Trim(),
                    Email = dr["ds_email"].ToString().Trim(),
                    Senha = dr["cd_senha"].ToString(),
                    AssinaturaEletronica = dr["cd_assinaturaeletronica"].ToString()
                };

            
            // Adiciona o contexto do oms
            usuario.Complementos.AdicionarItem<ContextoOMSInfo>(
                new ContextoOMSInfo()
                {
                    CodigoCBLC = dr["CodigoCBLC"].ToString(),
                    CodigoCBLCAssessor = dr["CodigoCBLCAssessor"].ToString(),
                    CodigoContaCorrente = dr["CodigoContaCorrente"].ToString(),
                    CodigoCustodia = dr["CodigoCustodia"].ToString(),
                    CodigoPerfilRisco = dr["CodigoPerfilRisco"].ToString()
                });

            // Retorna
            return usuario;
        }
        #endregion

        #region Rotinas Locais

        /// <summary>
        /// Salva a lista de permissoes
        /// </summary>
        /// <param name="usuarioOriginal"></param>
        /// <param name="usuarioSalvo"></param>
        private void salvarPermissoes(UsuarioInfo usuarioOriginal, UsuarioInfo usuarioSalvo)
        {
            // Inicializa
            UsuarioPermissaoDbLib usuarioPermissaoDbLib = new UsuarioPermissaoDbLib();

            // Pega lista de contas atuais
            List<PermissaoAssociadaInfo> permissoesAtuais =
                usuarioPermissaoDbLib.ConsultarObjetos(
                    new List<CondicaoInfo>() 
                    { 
                        new CondicaoInfo("CodigoUsuario", CondicaoTipoEnum.Igual, usuarioSalvo.CodigoUsuario)
                    });

            // Varre a lista de que foi pedido para salvar
            foreach (PermissaoAssociadaInfo permissaoAssociada in usuarioOriginal.Permissoes)
            {
                if (permissoesAtuais.Find(p => p.CodigoPermissao == permissaoAssociada.CodigoPermissao) == null)
                {
                    usuarioPermissaoDbLib.SalvarObjeto(permissaoAssociada, usuarioSalvo.CodigoUsuario);
                }
            }

            // Verifica se existem contas a remover
            foreach (PermissaoAssociadaInfo permissaoAssociada in permissoesAtuais)
                if (usuarioOriginal.Permissoes.Find(p => p.CodigoPermissao == permissaoAssociada.CodigoPermissao) == null)
                    usuarioPermissaoDbLib.RemoverObjeto(usuarioSalvo.CodigoUsuario, permissaoAssociada.CodigoPermissao);

            // Atribui a coleção ao cliente salvo
            usuarioSalvo.Permissoes = usuarioOriginal.Permissoes;
        }

        /// <summary>
        /// Salva a lista de grupos
        /// </summary>
        /// <param name="usuarioOriginal"></param>
        /// <param name="usuarioSalvo"></param>
        private void salvarGrupos(UsuarioInfo usuarioOriginal, UsuarioInfo usuarioSalvo)
        {
            // Inicializa
            UsuarioUsuarioGrupoDbLib usuarioGrupoDbLib = new UsuarioUsuarioGrupoDbLib();

            // Pega lista de grupos atuais
            List<string> gruposAtuais =
                usuarioGrupoDbLib.ConsultarObjetos(usuarioSalvo.CodigoUsuario);

            // Varre a lista de que foi pedido para salvar
            foreach (string codigoUsuarioGrupo in usuarioOriginal.Grupos)
                if (gruposAtuais.Find(p => p == codigoUsuarioGrupo) == null)
                    usuarioGrupoDbLib.SalvarObjeto(usuarioSalvo.CodigoUsuario, codigoUsuarioGrupo);

            // Verifica se existem grupos a remover
            foreach (string codigoUsuarioGrupo in gruposAtuais)
                if (usuarioOriginal.Grupos.Find(g => g == codigoUsuarioGrupo) == null)
                    usuarioGrupoDbLib.RemoverObjeto(usuarioSalvo.CodigoUsuario, codigoUsuarioGrupo);

            // Atribui a coleção ao cliente salvo
            usuarioSalvo.Grupos = usuarioOriginal.Grupos;
        }

        /// <summary>
        /// Salva a lista de perfis
        /// </summary>
        /// <param name="usuarioOriginal"></param>
        /// <param name="usuarioSalvo"></param>
        private void salvarPerfis(UsuarioInfo usuarioOriginal, UsuarioInfo usuarioSalvo)
        {
            // Inicializa
            UsuarioPerfilDbLib usuarioPerfilDbLib = new UsuarioPerfilDbLib();

            // Pega lista de grupos atuais
            List<string> perfisAtuais =
                usuarioPerfilDbLib.ConsultarObjetos(usuarioSalvo.CodigoUsuario);

            // Varre a lista de que foi pedido para salvar
            foreach (string codigoPerfil in usuarioOriginal.Perfis)
                if (perfisAtuais.Find(p => p == codigoPerfil) == null)
                    usuarioPerfilDbLib.SalvarObjeto(usuarioSalvo.CodigoUsuario, codigoPerfil);

            // Verifica se existem grupos a remover
            foreach (string codigoPerfil in perfisAtuais)
                if (usuarioOriginal.Perfis.Find(p => p == codigoPerfil) == null)
                    usuarioPerfilDbLib.RemoverObjeto(usuarioSalvo.CodigoUsuario, codigoPerfil);

            // Atribui a coleção ao cliente salvo
            usuarioSalvo.Perfis = usuarioOriginal.Perfis;
        }


        #endregion

    }
}
