using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.OMS.Library;
using System.Data.Common;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.Spider.LimiteRestricao.Lib.Mensagens;
using Gradual.Spider.LimiteRestricao.Lib.Dados;
using Gradual.Spider.LimiteRestricao.Lib;

namespace Gradual.Spider.LimiteRestricao.DbLib
{
    public class RiscoPlataformaDbLib
    {
        #region Propriedades
        LogSpiderDbLib lDbLog = new LogSpiderDbLib();
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public const string NomeConexaoSpider = "GradualSpider";

        public RiscoListarPlataformaResponse ListaPlataformaSpider()
        {
            var lRetorno = new RiscoListarPlataformaResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_lst"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    lRetorno.ListaPlataforma = new List<RiscoPlataformaInfo>();

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.ListaPlataforma.Add(new RiscoPlataformaInfo() 
                            {
                                IdPlataforma = lDataTable.Rows[i]["id_plataforma"].DBToInt32(),
                                DsPlataforma = lDataTable.Rows[i]["ds_plataforma"].DBToString()
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

        public RiscoListarOperadorResponse ListarOperadorSpider()
        {
            var lRetorno = new RiscoListarOperadorResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_operador_lst"))
                {
                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    lRetorno.Resultado = new List<RiscoOperadorInfo>();

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lRetorno.Resultado.Add(new RiscoOperadorInfo()
                            {
                                CodigoOperador = lDataTable.Rows[i]["id_operador"].DBToInt32(),
                                Sigla          = lDataTable.Rows[i]["ds_sigla"].DBToString(),
                                Nome           = lDataTable.Rows[i]["ds_nome"].DBToString(),
                                Email          = lDataTable.Rows[i]["ds_email"].DBToString(),
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

        public RiscoSelecionarPlataformaClienteResponse SelecionarPlataformaClienteSpider   (RiscoSelecionarPlataformaClienteRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_cliente_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    var lPlataforma = new RiscoPlataformaClienteInfo();

                    lPlataforma.ListaPlataforma = new List<RiscoPlataformaInfo>();

                    lPlataforma.TipoPlataforma = eTipoParametroPlataforma.SessaoNormal;

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lPlataforma.CodigoCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32();
                            
                            lPlataforma.TipoPlataforma = this.RetornaPlataformaParametroTrader(lPlataforma.CodigoCliente, (int)eTipoTrader.Cliente);
                            
                            lPlataforma.ListaPlataforma.Add(new RiscoPlataformaInfo()
                            {
                                IdPlataforma = lDataTable.Rows[i]["id_plataforma"].DBToInt32(),
                                DsPlataforma = lDataTable.Rows[i]["ds_plataforma"].DBToString()
                            });
                        }

                    }

                    VerificaContaBroker lContaBroker = RetornaListaPlataformaSeForContaMaster(pParametro.Objeto.CodigoCliente);

                    if (lContaBroker.EhContaMaster)
                    {
                        lPlataforma.ListaPlataforma.Clear();

                        lContaBroker.ListaPlataforma.ForEach(plataforma=> 
                        {
                            lPlataforma.ListaPlataforma.Add( new RiscoPlataformaInfo() 
                            {
                                IdPlataforma = plataforma.IdPlataforma,
                                DsPlataforma = plataforma.DsPlataforma
                            });    
                        });

                        lRetorno.Resultado.EhContaMaster = true;
                    }

                    lRetorno.Resultado = lPlataforma;
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RiscoSelecionarPlataformaOperadorResponse SelecionarPlataformaOperadorSpider (RiscoSelecionarPlataformaOperadorRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaOperadorResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_operador_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_operador", DbType.Int32, pParametro.Objeto.CodigoOperador);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    var lPlataforma = new RiscoPlataformaOperadorInfo();

                    lPlataforma.ListaPlataforma = new List<RiscoPlataformaInfo>();

                    lPlataforma.TipoPlataforma = eTipoParametroPlataforma.SessaoNormal;

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lPlataforma.CodigoOperador = lDataTable.Rows[i]["id_operador"].DBToInt32();
                            
                            lPlataforma.Sigla          = lDataTable.Rows[i]["ds_sigla"].DBToString();

                            lPlataforma.Nome = lDataTable.Rows[i]["ds_nome"].DBToString();

                            lPlataforma.Email = lDataTable.Rows[i]["ds_email"].DBToString();

                            lPlataforma.TipoPlataforma = this.RetornaPlataformaParametroTrader(lPlataforma.CodigoOperador, (int)eTipoTrader.Operador);
                            
                            lPlataforma.ListaPlataforma.Add(new RiscoPlataformaInfo()
                            {
                                IdPlataforma = lDataTable.Rows[i]["id_plataforma"].DBToInt32(),
                                DsPlataforma = lDataTable.Rows[i]["ds_plataforma"].DBToString(),
                            });
                        }
                    }

                    lRetorno.Resultado = lPlataforma;
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;

            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }
        
        public RiscoSelecionarPlataformaAssessorResponse SelecionarPlataformaAssessorSpider (RiscoSelecionarPlataformaAssessorRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaAssessorResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_assessor_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, pParametro.Objeto.CodigoAssessor);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    var lPlataforma = new RiscoPlataformaAssessorInfo();

                    lPlataforma.ListaPlataforma = new List<RiscoPlataformaInfo>();

                    lPlataforma.TipoPlataforma = eTipoParametroPlataforma.SessaoNormal;

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lPlataforma.CodigoAssessor = lDataTable.Rows[i]["id_assessor"].DBToInt32();
                            
                            lPlataforma.Sigla          = lDataTable.Rows[i]["ds_sigla"].DBToString();
                            
                            lPlataforma.TipoPlataforma = this.RetornaPlataformaParametroTrader(lPlataforma.CodigoAssessor, (int)eTipoTrader.Assessor);

                            lPlataforma.ListaPlataforma.Add(new RiscoPlataformaInfo()
                            {
                                IdPlataforma = lDataTable.Rows[i]["id_plataforma"].DBToInt32(),
                                DsPlataforma = lDataTable.Rows[i]["ds_plataforma"].DBToString()
                            });
                        }
                    }

                    lRetorno.Resultado = lPlataforma;
                }

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }
        
        public RiscoSelecionarPlataformaContaMasterResponse SelecionarPlataformaContaMasterSpider(RiscoSelecionarPlataformaContaMasterRequest pParametro)
        {
            var lRetorno = new RiscoSelecionarPlataformaContaMasterResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            var lPlataforma = new RiscoPlataformaContaMasterInfo();
            
            lPlataforma.TipoPlataforma = eTipoParametroPlataforma.SessaoNormal;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_contamaster_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contamaster", DbType.Int32, pParametro.Objeto.CodigoContaMaster);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    lPlataforma.ListaPlataforma = new List<RiscoPlataformaInfo>();

                    lPlataforma.ListaCliente = new List<RiscoContaMasterFilhoInfo>();

                    

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lPlataforma.CodigoContaMaster = lDataTable.Rows[i]["id_contamaster"].DBToInt32();

                            //lPlataforma.TipoPlataforma = this.RetornaPlataformaParametroTrader(lPlataforma.CodigoContaMaster, (int)eTipoTrader.ContaMaster);

                            lPlataforma.ListaPlataforma.Add(new RiscoPlataformaInfo()
                            {
                                IdPlataforma = lDataTable.Rows[i]["id_plataforma"].DBToInt32(),
                                DsPlataforma = lDataTable.Rows[i]["ds_plataforma"].DBToString()
                            });
                        }
                    }
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_contamaster_filho_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contamaster", DbType.Int32, pParametro.Objeto.CodigoContaMaster);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            lPlataforma.ListaCliente.Add(new RiscoContaMasterFilhoInfo()
                            {
                                Ativo = lDataTable.Rows[i]["stAtivo"].DBToString(),
                                CodigoCliente = lDataTable.Rows[i]["idCliente"].DBToInt32(),
                                NomeCliente = lDataTable.Rows[i]["dsCliente"].DBToString(),
                            });
                        }
                    }
                }

                gLogger.InfoFormat("Tipo de Plataforma -> {0}", lPlataforma.TipoPlataforma);

                lPlataforma.TipoPlataforma = eTipoParametroPlataforma.SessaoNormal;

                lRetorno.Resultado = lPlataforma;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RiscoSalvarPlataformaClienteResponse SalvarPlataformaClienteSpider(RiscoSalvarPlataformaClienteRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaClienteResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_cliente_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.CodigoCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_cliente_ins"))
                {
                    for (int i = 0; i < pParametro.Objeto.ListaPlataforma.Count; i++)
                    {
                        int lIdPlataforma = pParametro.Objeto.ListaPlataforma[i].IdPlataforma;

                        lDbCommand.Parameters.Clear();

                        lAcessaDados.AddInParameter(lDbCommand, "@id_cliente"   , DbType.Int32, pParametro.Objeto.CodigoCliente);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma", DbType.Int32, lIdPlataforma);

                        var lCodigo = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_parametro_ins"))
                {
                    lDbCommand.Parameters.Clear();

                    lAcessaDados.AddInParameter(lDbCommand, "@id_trader",          DbType.Int32, pParametro.Objeto.CodigoCliente);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_parametro",   DbType.Int32, (int)pParametro.Objeto.TipoPlataforma);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_trader",      DbType.Int32, eTipoTrader.Cliente);

                    var lCodigo = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lRetorno.Resultado = pParametro.Objeto;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        public RiscoSalvarPlataformaContaMasterResponse SalvarPlataformaContaMasterSpider(RiscoSalvarPlataformaContaMasterRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaContaMasterResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_contamaster_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contamaster", DbType.Int32, pParametro.Objeto.CodigoContaMaster);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_contamaster_ins"))
                {
                    for (int i = 0; i < pParametro.Objeto.ListaPlataforma.Count; i++)
                    {
                        var lIdPlataforma = pParametro.Objeto.ListaPlataforma[i].IdPlataforma;

                        lDbCommand.Parameters.Clear();

                        lAcessaDados.AddInParameter(lDbCommand, "@id_contamaster", DbType.Int32, pParametro.Objeto.CodigoContaMaster);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma", DbType.Int32, lIdPlataforma);

                        var lDataTable = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_parametro_ins"))
                {
                    lDbCommand.Parameters.Clear();

                    lAcessaDados.AddInParameter(lDbCommand, "@id_trader",           DbType.Int32, pParametro.Objeto.CodigoContaMaster);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_parametro",   DbType.Int32, pParametro.Objeto.TipoPlataforma);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_trader",      DbType.Int32, eTipoTrader.ContaMaster);

                    var lCodigo = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lRetorno.Resultado = pParametro.Objeto;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }
        
        public RiscoSalvarPlataformaAssessorResponse SalvarPlataformaAssessorSpider(RiscoSalvarPlataformaAssessorRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaAssessorResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_assessor_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, pParametro.Objeto.CodigoAssessor);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_assessor_ins"))
                {
                    for (int i = 0; i < pParametro.Objeto.ListaPlataforma.Count; i++)
                    {
                        var lIdPlataforma = pParametro.Objeto.ListaPlataforma[i].IdPlataforma;

                        lDbCommand.Parameters.Clear();

                        lAcessaDados.AddInParameter(lDbCommand, "@id_assessor",     DbType.Int32, pParametro.Objeto.CodigoAssessor);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma",   DbType.Int32, lIdPlataforma);
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_sigla",        DbType.String, pParametro.Objeto.Sigla);

                        var lDataTable = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_parametro_ins"))
                {
                    lDbCommand.Parameters.Clear();

                    lAcessaDados.AddInParameter(lDbCommand, "@id_trader",           DbType.Int32, pParametro.Objeto.CodigoAssessor);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_parametro",   DbType.Int32, pParametro.Objeto.TipoPlataforma);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_trader",      DbType.Int32, eTipoTrader.Assessor);

                    var lCodigo = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lRetorno.Resultado = pParametro.Objeto;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }
        
        public RiscoSalvarPlataformaOperadorResponse SalvarPlataformaOperadorSpider(RiscoSalvarPlataformaOperadorRequest pParametro)
        {
            var lRetorno = new RiscoSalvarPlataformaOperadorResponse();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_operador_del"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_operador", DbType.Int32, pParametro.Objeto.CodigoOperador);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_operador_ins"))
                {
                    lDbCommand.Parameters.Clear();

                    lAcessaDados.AddInParameter(lDbCommand, "@id_operador", DbType.Int32, pParametro.Objeto.CodigoOperador);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome",     DbType.String, pParametro.Objeto.Nome);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_email",    DbType.String, pParametro.Objeto.Email);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_sigla",    DbType.String, pParametro.Objeto.Sigla);

                    var lDataTable = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_operador_ins"))
                {
                    for (int i = 0; i < pParametro.Objeto.ListaPlataforma.Count; i++)
                    {
                        var lIdPlataforma = pParametro.Objeto.ListaPlataforma[i].IdPlataforma;

                        lDbCommand.Parameters.Clear();

                        lAcessaDados.AddInParameter(lDbCommand, "@id_operador",     DbType.Int32, pParametro.Objeto.CodigoOperador);
                        lAcessaDados.AddInParameter(lDbCommand, "@id_plataforma",   DbType.Int32, lIdPlataforma);
                        lAcessaDados.AddInParameter(lDbCommand, "@ds_sigla",        DbType.String, pParametro.Objeto.Sigla);

                        gLogger.InfoFormat("Inserindo código plataforma [{0}]", lIdPlataforma);

                        var lDataTable = lAcessaDados.ExecuteNonQuery(lDbCommand);
                    }
                }

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_parametro_ins"))
                {
                    lDbCommand.Parameters.Clear();

                    lAcessaDados.AddInParameter(lDbCommand, "@id_trader",           DbType.Int32, pParametro.Objeto.CodigoOperador);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_parametro",   DbType.Int32, pParametro.Objeto.TipoPlataforma);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_trader",      DbType.Int32, eTipoTrader.Operador);

                    var lCodigo = lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                lRetorno.Resultado = pParametro.Objeto;

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lRetorno;
        }

        #region Métodos auxiliares internos
        public eTipoParametroPlataforma RetornaPlataformaParametroTrader(int CodigoTrader, int eTipoEnumTrader)
        {
            eTipoParametroPlataforma lRetorno = eTipoParametroPlataforma.SessaoNormal;

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_parametro_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_trader", DbType.Int32, CodigoTrader);
                lAcessaDados.AddInParameter(lDbCommand, "@id_tipo_trader", DbType.Int32, eTipoEnumTrader);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno = (eTipoParametroPlataforma)Enum.Parse(typeof(eTipoParametroPlataforma), lDataTable.Rows[i]["id_tipo_parametro"].DBToString());
                    }
                }

            }

            return lRetorno;
        }

        private VerificaContaBroker RetornaListaPlataformaSeForContaMaster(int CodigoCliente)
        {
            VerificaContaBroker lRetorno = new VerificaContaBroker();

            lRetorno.EhContaMaster = false;

            lRetorno.ListaPlataforma = new List<RiscoPlataformaInfo>();

            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "GradualSpider";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, "select * from tb_conta_broker where idContaBroker = " + CodigoCliente))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.EhContaMaster = true;
                    }
                }
            }

            if (lRetorno.EhContaMaster)
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_plataforma_contamaster_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contamaster", DbType.Int32, CodigoCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            //lRetorno.CodigoContaMaster = lDataTable.Rows[i]["id_contamaster"].DBToInt32();

                            //lPlataforma.TipoPlataforma = this.RetornaPlataformaParametroTrader(lPlataforma.CodigoContaMaster, (int)eTipoTrader.ContaMaster);

                            lRetorno.ListaPlataforma.Add(new RiscoPlataformaInfo()
                            {
                                IdPlataforma = lDataTable.Rows[i]["id_plataforma"].DBToInt32(),
                                DsPlataforma = lDataTable.Rows[i]["ds_plataforma"].DBToString()
                            });
                        }
                    }
                }
            }

            return lRetorno;
        }
        #endregion

        struct VerificaContaBroker
        {
            public bool EhContaMaster { get; set; }

            public List<RiscoPlataformaInfo> ListaPlataforma { get; set; }
        }
    }

    
}
