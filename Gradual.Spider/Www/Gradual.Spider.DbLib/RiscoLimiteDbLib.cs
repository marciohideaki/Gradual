using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.IntranetCorp.Lib.Dados;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Spider.Lib.Dados;
using Gradual.Spider.Lib;
using Gradual.Spider.Lib.Mensagens;
using Gradual.OMS.Library;


namespace Gradual.Spider.DbLib
{
    public class RiscoLimiteDbLib
    {
        #region Propriedades
        LogSpiderDbLib lDbLog = new LogSpiderDbLib();
        #endregion
        public const string NomeConexaoSpider = "GradualSpider";

        #region Métodos Risco Limite
        
        public RiscoLimiteInfo ConsultarRiscoLimitePorClienteSpider(RiscoLimiteInfo pParametros)
        {
            var lRetorno = new RiscoLimiteInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_limite_alocado_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.ConsultaIdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                lRetorno.Resultado = new List<RiscoLimiteInfo>();

                if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoLimiteInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        private RiscoLimiteInfo CarregarEntidadeRiscoLimiteInfo(DataRow pLinha)
        {
            return new RiscoLimiteInfo()
            {
                DsParametro  = pLinha["ds_parametro"].DBToString(),
                IdParametro  = pLinha["id_parametro"].DBToInt32(),
                VlAlocado    = pLinha["vl_alocado"].DBToDecimal(),
                VlDisponivel = pLinha["vl_disponivel"].DBToDecimal(),
                VlParametro  = pLinha["vl_parametro"].DBToDecimal(),
            };
        }
        #endregion

        #region Métodos Risco Permissão

        public RiscoPermissaoInfo SalvarEntidadeRiscoPermissaoInfo(RiscoPermissaoInfo pParametros)
        {
            var lRetorno = new RiscoPermissaoInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_salvar"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_permissao", DbType.Int32, pParametros.CodigoPermissao);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_permissao", DbType.String, pParametros.CodigoPermissao);
                lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.Int32, pParametros.Bolsa);

                var Id_permissao = lAcessaDados.ExecuteNonQuery(lDbCommand);

                //lRetorno = pParametros;
                //lRetorno.CodigoPermissao

                //lRetorno.Resultado = new List<RiscoPermissaoInfo>();

                //if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                //        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoLimiteInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        public RiscoPermissaoInfo RemoverEntidadeRiscoPermissaoInfo(RiscoPermissaoInfo pParametros)
        {
            var lRetorno = new RiscoPermissaoInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_permissao", DbType.Int32, pParametros.CodigoPermissao);
                //lAcessaDados.AddInParameter(lDbCommand, "@ds_permissao", DbType.String, pParametros.CodigoPermissao);
                //lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.Int32, pParametros.Bolsa);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                //lRetorno = pParametros;
                //lRetorno.CodigoPermissao

                //lRetorno.Resultado = new List<RiscoPermissaoInfo>();

                //if (null != lDataTable) for (int i = 0; i < lDataTable.Rows.Count; i++)
                //        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoLimiteInfo(lDataTable.Rows[i]));
            }

            return lRetorno;
        }

        public RiscoPermissaoInfo ReceberEntidadeRiscoPermissaoInfo(RiscoPermissaoInfo pParametros)
        {
            var lRetorno = new RiscoPermissaoInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_permissao", DbType.Int32, pParametros.CodigoPermissao);
                
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno = (this.CarregarEntidadeRiscoPermissaoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lRetorno;
        }

        public List<RiscoPermissaoInfo> ListarEntidadeRiscoPermissaoInfo(RiscoPermissaoInfo pParametros)
        {
            var lRetorno = new List<RiscoPermissaoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = NomeConexaoSpider;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.Int32, (int)pParametros.Bolsa);
                lAcessaDados.AddInParameter(lDbCommand, "@id_bolsa", DbType.String, pParametros.NomePermissao);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Add(this.CarregarEntidadeRiscoPermissaoInfo(lDataTable.Rows[i]));
                    }
                }
            }

            return lRetorno;
        }

        private RiscoPermissaoInfo CarregarEntidadeRiscoPermissaoInfo(DataRow dr)
        {
            var lRetorno = new RiscoPermissaoInfo();

            lRetorno.Bolsa           = (BolsaInfo)((int)dr["id_bolsa"]);
            lRetorno.CodigoPermissao = (int)dr["id_permissao"];
            lRetorno.Metodo          = dr["nome_metodo"].ToString();
            lRetorno.NameSpace       = dr["url_namespace"].ToString();
            lRetorno.NomePermissao   = dr["dscr_permissao"].ToString();

            return lRetorno;
        }
        #endregion

        #region Métodos Risco Parametros
        public RiscoListarParametrosClienteResponse ListarLimitePorClienteSpider(RiscoListarParametrosClienteRequest pParametro)
        {
            var lRetorno = new RiscoListarParametrosClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {

                            var lRiscoParametro = new RiscoParametroClienteInfo()
                            {
                                CodigoCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                DataValidade  = lDataTable.Rows[i]["dt_validade"].DBToDateTime(),
                                IdBolsa       = lDataTable.Rows[i]["id_bolsa"].DBToInt32(),
                                StAtivo       = lDataTable.Rows[i]["st_ativo"].DBToChar(),
                                Valor         = lDataTable.Rows[i]["vl_parametro"].DBToDecimal(),
                                Parametro = new RiscoParametroInfo()
                                {
                                    CodigoParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                                    NomeParametro   = lDataTable.Rows[i]["dscr_parametro"].DBToString(),
                                },
                                Grupo = new RiscoGrupoInfo()
                                {
                                    CodigoGrupo = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                }
                            };

                            lRetorno.ParametrosRiscoCliente.Add(lRiscoParametro);
                        }
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                //lDbLog.RegistrarLog()
                //LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.Message;
            }

            return lRetorno;
        }

        public RiscoListarPermissoesResponse ListarPermissoesRisco(RiscoListarPermissoesRequest pParametro)
        {
            var lRetorno = new RiscoListarPermissoesResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_permissao_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@dscr_permissao", DbType.Int32, pParametro.FiltroNomePermissao);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Permissoes.Add(MontarObjetoPermicaoRisco(lDataTable.Rows[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        private RiscoPermissaoInfo MontarObjetoPermicaoRisco(DataRow dr)
        {
            RiscoPermissaoInfo lRetorno = new RiscoPermissaoInfo();

            lRetorno.Bolsa           = (BolsaInfo)((int)dr["id_bolsa"]);
            lRetorno.CodigoPermissao = (int)dr["id_permissao"];
            lRetorno.Metodo          = dr["nome_metodo"].ToString();
            lRetorno.NameSpace       = dr["url_namespace"].ToString();
            lRetorno.NomePermissao   = dr["dscr_permissao"].ToString();

            return lRetorno;
        }

        public RiscoListarPermissoesClienteResponse ListarPermissoesRiscoClienteSpider(RiscoListarPermissoesClienteRequest pParametro)
        {

            var lRetorno = new RiscoListarPermissoesClienteResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.PermissoesAssociadas.Add(MontarObjetoPermicaoRiscoAssociada(lDataTable.Rows[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LogRisco.Logar(pParametro, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogRisco.eAcao.Listar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        private RiscoPermissaoAssociadaInfo MontarObjetoPermicaoRiscoAssociada(DataRow dr)
        {
            RiscoPermissaoAssociadaInfo lRetorno = new RiscoPermissaoAssociadaInfo();

            lRetorno.CodigoCliente                 = (int)dr["id_cliente"];

            lRetorno.CodigoPermissaoRiscoAssociada = (int)dr["id_cliente_permissao"];

            if (!Convert.IsDBNull(dr["id_grupo"]))
            {
                lRetorno.Grupo = new RiscoGrupoDbLib().ReceberEntidadeRiscoGrupoInfo(new RiscoGrupoInfo() { CodigoGrupo = dr["id_grupo"].DBToInt32() });
            }

            lRetorno.PermissaoRisco = this.ReceberEntidadeRiscoPermissaoInfo(new RiscoPermissaoInfo() { CodigoPermissao = dr["id_permissao"].DBToInt32() });

            return lRetorno;
        }

        public RiscoListarBloqueiroInstrumentoResponse ListarBloqueioPorClienteSpider(RiscoListarBloqueiroInstrumentoRequest pParametro)
        {
            var lRetorno = new RiscoListarBloqueiroInstrumentoResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(
                                new RiscoBloqueioInstrumentoInfo()
                                {
                                    CdAtivo   = lDataTable.Rows[i]["cd_ativo"].DBToString(),
                                    Direcao   = lDataTable.Rows[i]["direcao"].DBToString(),
                                    IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                });
                        }
                    }
                }
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RiscoListarClienteParametroGrupoResponse ListarClienteParametroGrupoSpider(RiscoListarClienteParametroGrupoRequest pParametro)
        {
            var lRetorno = new RiscoListarClienteParametroGrupoResponse();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_grupo_lst"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable != null && lDataTable.Rows.Count > 0)
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                            lRetorno.ListaObjeto.Add(new RiscoClienteParametroGrupoInfo()
                            {
                                IdCliente   = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                IdGrupo     = lDataTable.Rows[i]["id_grupo"].DBToInt32(),
                                IdParametro = lDataTable.Rows[i]["id_parametro"].DBToInt32(),
                            });
                }
            }
            catch (Exception ex)
            {
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }

            return lRetorno;
        }

        public RiscoReceberParametroClienteResponse RiscoReceberParametroCliente(RiscoReceberParametroClienteRequest pRequest, Boolean pEfetuarLog = false)
        {
            var lRetorno = new RiscoReceberParametroClienteResponse();
            var lAcessaDados = new AcessaDados();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro", DbType.Int32, pRequest.CodigoParametroRiscoCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (lDataTable != null && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        DataRow lRow                   = lDataTable.Rows[0];
                        lRetorno.ParametroRiscoCliente = MontarObjetoParametroCliente(lRow);
                    }
                }
            }

            return lRetorno;
        }

        private RiscoParametroClienteInfo MontarObjetoParametroCliente(DataRow dr)
        {
            var lRetorno = new RiscoParametroClienteInfo();

            lRetorno.CodigoCliente          = (int)dr["id_cliente"];

            lRetorno.CodigoParametroCliente = (int)dr["id_cliente_parametro"];

            if (!Convert.IsDBNull(dr["dt_validade"]))
            {
                lRetorno.DataValidade = DateTime.Parse(dr["dt_validade"].ToString());
            }

            if (!Convert.IsDBNull(dr["vl_parametro"]))
            {
                lRetorno.Valor = decimal.Parse(dr["vl_parametro"].ToString());
            }

            lRetorno.Parametro = new RiscoParametroInfo()
            {
                CodigoParametro = (int)dr["id_parametro"],
                NomeParametro   = dr["dscr_parametro"].ToString(),
                Bolsa           = ((BolsaInfo)(int)dr["id_bolsa"])
            };

            lRetorno.ListaParametroClienteValores = ListarRiscoParametroClienteValor(new RiscoParametroClienteValorInfo() { CodigoParametroClienteValor = (int)dr["id_cliente_parametro"] });

            return lRetorno;
        }

        public List<RiscoParametroClienteValorInfo> ListarRiscoParametroClienteValor(RiscoParametroClienteValorInfo pRequest)
        {
            var lRetorno = new List<RiscoParametroClienteValorInfo>();

            var lAcessaDados = new AcessaDados();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_valor_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro", DbType.Int32, pRequest.CodigoParametroClienteValor);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                for (int i = 0; i < lDataTable.Rows.Count; i++)
                {
                    DataRow lRow = lDataTable.Rows[i];

                    lRetorno.Add(MontarObjetoParametroClienteValor(lRow));
                }
            }

            return lRetorno;
        }

        private RiscoParametroClienteValorInfo MontarObjetoParametroClienteValor(DataRow dr)
        {
            var lRetorno = new RiscoParametroClienteValorInfo();

            lRetorno.CodigoParametroClienteValor = (int)dr["id_cliente_parametro_valor"];
            lRetorno.Descricao                   = dr["ds_historico"].ToString();
            lRetorno.ValorAlocado                = (decimal)dr["vl_alocado"];
            lRetorno.ValorDisponivel             = (decimal)dr["vl_disponivel"];
            lRetorno.DataMovimento               = (DateTime)dr["dt_movimento"];

            return lRetorno;
        }

        public RiscoSalvarParametroClienteResponse SalvarParametroRiscoClienteSpider(RiscoSalvarParametroClienteRequest pRequest)
        {
            var lRetorno     = new RiscoSalvarParametroClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                {
                    RiscoReceberParametroCliente(new RiscoReceberParametroClienteRequest() 
                        { 
                            DescricaoUsuarioLogado      = pRequest.DescricaoUsuarioLogado, 
                            IdUsuarioLogado             = pRequest.IdUsuarioLogado, 
                            CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente 
                        }, true);
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_salvar"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_parametro",    DbType.Int32, pRequest.ParametroRiscoCliente.CodigoParametroCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",              DbType.Int32, pRequest.ParametroRiscoCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro",            DbType.Int32, pRequest.ParametroRiscoCliente.Parametro.CodigoParametro);

                    if (pRequest.ParametroRiscoCliente.Grupo != null)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pRequest.ParametroRiscoCliente.Grupo);
                    }

                    if (pRequest.ParametroRiscoCliente.Valor != null)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@vl_parametro", DbType.Decimal, pRequest.ParametroRiscoCliente.Valor);
                    }

                    if (pRequest.ParametroRiscoCliente.DataValidade != null)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@dt_validade", DbType.DateTime, pRequest.ParametroRiscoCliente.DataValidade);
                    }

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                //LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                //LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }

        public MensagemResponseBase SalvarPermissoesRiscoAssociadasSpider(RiscoSalvarPermissoesAssociadasRequest pRequest)
        {

            var lRetorno = new RiscoSalvarParametroClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pRequest.PermissoesAssociadas[0].CodigoCliente);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            foreach (RiscoPermissaoAssociadaInfo item in pRequest.PermissoesAssociadas)
            {
                try
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_permissao_salvar"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",              DbType.Int32, item.CodigoCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente_permissao",    DbType.Int32, item.CodigoPermissaoRiscoAssociada);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_permissao",            DbType.Int32, item.PermissaoRisco.CodigoPermissao);

                        if (item.Grupo != null)
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, item.Grupo);
                        }

                        lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }
                catch (Exception ex)
                {
                    //LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                    lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                    lRetorno.DescricaoResposta += ex.Message;
                }
            }

            return lRetorno;
        }

        public RiscoSalvarParametroClienteResponse SalvarExpirarLimiteSpider(RiscoSalvarParametroClienteRequest pRequest)
        {

            var lRetorno = new RiscoSalvarParametroClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                //LOG
                if (pRequest.ParametroRiscoCliente.CodigoParametroCliente != 0)
                    RiscoReceberParametroCliente(new RiscoReceberParametroClienteRequest() 
                    { 
                        DescricaoUsuarioLogado = pRequest.DescricaoUsuarioLogado, 
                        IdUsuarioLogado = pRequest.IdUsuarioLogado, 
                        CodigoParametroRiscoCliente = pRequest.ParametroRiscoCliente.CodigoParametroCliente 
                    }, true);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_parametro_expirarlimite"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente",      DbType.Int32, pRequest.ParametroRiscoCliente.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_parametro",    DbType.Int32, pRequest.ParametroRiscoCliente.CodigoParametroCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                //LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                //LogRisco.Logar(pRequest, pRequest.IdUsuarioLogado, pRequest.DescricaoUsuarioLogado, LogRisco.eAcao.Salvar, ex);
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                lRetorno.DescricaoResposta = ex.ToString();
            }
            return lRetorno;
        }


        #endregion
    }
}
