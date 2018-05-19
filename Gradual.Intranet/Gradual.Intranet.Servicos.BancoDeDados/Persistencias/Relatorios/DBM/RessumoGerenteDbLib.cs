using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class RessumoGerenteDbLib
    {
        public ReceberObjetoResponse<ResumoGerenteinfo> ReceberDadosMesAtual(ReceberEntidadeRequest<ResumoGerenteinfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoGerenteinfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            lRetorno = new ReceberObjetoResponse<ResumoGerenteinfo>();

            lRetorno.Objeto = new ResumoGerenteinfo();


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CORRETAGEM_MENSAL"))
            {
                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BOVESPA_MES"   , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BVSP_MES"          , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BMF_MES"       , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BMF_MES"           , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BMF_BVSP_MES"  , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BMF_BVSP_MES"      , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CADASTRO_MES"             , DbType.Decimal, 0);
                
                lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL"                 , DbType.Int32, pParametro.Objeto.CodigoFilial);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BVSP)
                {
                    lRetorno.Objeto.CorretagemMes   = lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BOVESPA_MES").DBToDecimal();
                    lRetorno.Objeto.VolumeMes       = lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BVSP_MES").DBToDecimal()       ;
                    lRetorno.Objeto.CadastradoMes   = lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES").DBToDecimal()          ;
                }
                else if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF)
                {
                    lRetorno.Objeto.CorretagemMes   = lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BMF_MES").DBToDecimal();
                    lRetorno.Objeto.VolumeMes       = lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BMF_MES").DBToDecimal()    ;     
                    lRetorno.Objeto.CadastradoMes   = lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES").DBToDecimal()      ;
                }
                else //Bovespa e BMF
                {
                    lRetorno.Objeto.CorretagemMes   = lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BMF_BVSP_MES").DBToDecimal()   ;
                    lRetorno.Objeto.VolumeMes       = lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BMF_BVSP_MES").DBToDecimal()       ;
                    lRetorno.Objeto.CadastradoMes   = lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES").DBToDecimal()              ;
                }




            }

            return lRetorno;

        }

        public ReceberObjetoResponse<ResumoGerenteMesAnteriorInfo> ReceberDadosMesAnterior(ReceberEntidadeRequest<ResumoGerenteMesAnteriorInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoGerenteMesAnteriorInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            lRetorno = new ReceberObjetoResponse<ResumoGerenteMesAnteriorInfo>();

            lRetorno.Objeto = new ResumoGerenteMesAnteriorInfo();


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CORRETAGEM_MENSAL_ANT"))
            {


                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BOVESPA_MES_ANT"   , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BVSP_MES_ANT"          , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BMF_MES_ANT"       , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BMF_MES_ANT"           , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BMF_BVSP_MES_ANT"  , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BMF_BVSP_MES_ANT"      , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CADASTRO_MES_ANT"             , DbType.Decimal, 0);
                
                lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL"                     , DbType.Int32, pParametro.Objeto.CodigoFilial);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BVSP)
                {
                    lRetorno.Objeto.CorretagemMesAnterior   = lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BOVESPA_MES_ANT").DBToDecimal();
                    lRetorno.Objeto.VolumeMesAnterior       = lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BVSP_MES_ANT").DBToDecimal()       ;
                    lRetorno.Objeto.CadastradoMesAnterior   = lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES_ANT").DBToDecimal()          ;
                }
                else if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF)
                {
                    lRetorno.Objeto.CorretagemMesAnterior   = lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BMF_MES_ANT").DBToDecimal();
                    lRetorno.Objeto.VolumeMesAnterior       = lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BMF_MES_ANT").DBToDecimal()    ;
                    lRetorno.Objeto.CadastradoMesAnterior   = lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES_ANT").DBToDecimal()      ;
                }
                else //Bovespa e BMF
                {
                    lRetorno.Objeto.CorretagemMesAnterior   = lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BMF_BVSP_MES_ANT").DBToDecimal()   ;
                    lRetorno.Objeto.VolumeMesAnterior       = lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BMF_BVSP_MES_ANT").DBToDecimal()       ;
                    lRetorno.Objeto.CadastradoMesAnterior   = lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES_ANT").DBToDecimal()              ;
                }




            }

            return lRetorno;

        }

        public ReceberObjetoResponse<ResumoGerentePeriodoinfo> ReceberDadosPorPeriodo(ReceberEntidadeRequest<ResumoGerentePeriodoinfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoGerentePeriodoinfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            lRetorno = new ReceberObjetoResponse<ResumoGerentePeriodoinfo>();

            lRetorno.Objeto = new ResumoGerentePeriodoinfo();


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CORRETAGEM_PERIODO"))
            {

                lAcessaDados.AddInParameter(lDbCommand, "DATA_INICIO"   , DbType.Date, pParametro.Objeto.DataInicial);
                lAcessaDados.AddInParameter(lDbCommand, "DATA_FINAL"    , DbType.Date, pParametro.Objeto.DataFinal  );

                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BVSP_CORRETAGEM", DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BVSP_VOLUME"    , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BMF_CORRETAGEM" , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BMF_VOLUME"     , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_BMF_BOVESPA_CORRETAGEM" , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_BMF_BOVESPA_VOLUME"     , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_CADASTRO"               , DbType.Decimal, 0);
                
                lAcessaDados.AddInParameter(lDbCommand , "ID_FILIAL"                     , DbType.Int32, pParametro.Objeto.CodigoFilial);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BVSP)
                {
                    lRetorno.Objeto.CorretagemIntervaloData = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BVSP_CORRETAGEM").DBToDecimal() ;
                    lRetorno.Objeto.VolumeIntervaloData     = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BVSP_VOLUME").DBToDecimal()     ;
                    lRetorno.Objeto.CadastradoIntervaloData = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CADASTRO").DBToDecimal()                ;
                }
                else if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF)
                {
                    lRetorno.Objeto.CorretagemIntervaloData = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BMF_CORRETAGEM").DBToDecimal()  ;
                    lRetorno.Objeto.VolumeIntervaloData     = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BMF_VOLUME").DBToDecimal()      ;
                    lRetorno.Objeto.CadastradoIntervaloData = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CADASTRO").DBToDecimal()                ; 
                }
                else //Bovespa e BMF
                {
                    lRetorno.Objeto.CorretagemIntervaloData = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_BMF_BOVESPA_CORRETAGEM").DBToDecimal();
                    lRetorno.Objeto.VolumeIntervaloData     = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_BMF_BOVESPA_VOLUME").DBToDecimal()     ;
                    lRetorno.Objeto.CadastradoIntervaloData = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CADASTRO").DBToDecimal()               ;
                }




            }

            return lRetorno;

        }

        public ReceberObjetoResponse<ResumoGerenteClienteInfo> ReceberDadosClientes(ReceberEntidadeRequest<ResumoGerenteClienteInfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoGerenteClienteInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            lRetorno = new ReceberObjetoResponse<ResumoGerenteClienteInfo>();

            lRetorno.Objeto = new ResumoGerenteClienteInfo();


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CLIENTES"))
            {

                lAcessaDados.AddOutParameter(lDbCommand, "PORCENT_CLIENTES_MES"         , DbType.Decimal, 0                             );
                lAcessaDados.AddOutParameter(lDbCommand, "PORCENT_CLIENTE_CUSTODIA"     , DbType.Decimal, 0                             );
                lAcessaDados.AddOutParameter(lDbCommand, "PORCENT_CLIENTE_NOVENTA"      , DbType.Decimal, 0                             );
                lAcessaDados.AddInParameter(lDbCommand , "ID_FILIAL"                    , DbType.Int32, pParametro.Objeto.CodigoFilial  );
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_CORRETAGEM_BMF"         , DbType.Decimal, 0                             );
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_CORRETAGEM_BVSP"        , DbType.Decimal, 0                             );
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_CORRETAGEM_BMF_BVSP"    , DbType.Decimal, 0                             );
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_CUSTODIA"               , DbType.Decimal, 0                             );


                lAcessaDados.ExecuteNonQuery(lDbCommand);


                lRetorno.Objeto.Porcentagemclientes             = lAcessaDados.GetParameterValue(lDbCommand, "PORCENT_CLIENTES_MES").DBToDecimal();
                lRetorno.Objeto.PorcentagemClienteCustodia      = lAcessaDados.GetParameterValue(lDbCommand, "PORCENT_CLIENTE_CUSTODIA").DBToDecimal();
                lRetorno.Objeto.ClienteNaoOperaramNoventaDia    = lAcessaDados.GetParameterValue(lDbCommand, "PORCENT_CLIENTE_NOVENTA").DBToDecimal();
                lRetorno.Objeto.MediaCustodia                   = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CUSTODIA").DBToDecimal();


                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF)
                    lRetorno.Objeto.MediaCorretagem                 = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CORRETAGEM_BMF").DBToDecimal();
                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BVSP)
                    lRetorno.Objeto.MediaCorretagem                 = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CORRETAGEM_BVSP").DBToDecimal();
                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF_BVSP)
                    lRetorno.Objeto.MediaCorretagem                 = lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CORRETAGEM_BMF_BVSP").DBToDecimal();



            }

            return lRetorno;
        }

        public ConsultarObjetosResponse<ResumoGerenteinfo> ConsultarAssessor(ConsultarEntidadeRequest<ResumoGerenteinfo> pParametro)
        {
            try
            {
                ConsultarObjetosResponse<ResumoGerenteinfo> resposta =
                    new ConsultarObjetosResponse<ResumoGerenteinfo>();

                List<ResumoAssessorInfo> listaAssessor = new List<ResumoAssessorInfo>();
                

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF)
                {

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_BREAK_ASSESSOR_BMF"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "DATA_INICIO", DbType.Date, pParametro.Objeto.DataInicial.ToString("dd/MM/yyyy"));
                        lAcessaDados.AddInParameter(lDbCommand, "DATA_FINAL", DbType.Date, pParametro.Objeto.DataFinal.ToString("dd/MM/yyyy"));
                        lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                        DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["cd_assessor"]    = lDataTable.Rows[i]["cd_assessor"] ;
                                linha["nm_assessor"]    = lDataTable.Rows[i]["nm_assessor"] ;
                                linha["CORRETAGEM"]     = lDataTable.Rows[i]["CORRETAGEM"]  ;
                                linha["VOLUME"]         = lDataTable.Rows[i]["VOLUME"]      ;
                                linha["Tipo"]           = lDataTable.Rows[i]["Tipo"]        ;
                                linha["CUSTODIA"]       = lDataTable.Rows[i]["CUSTODIA"]    ;
                                
                                listaAssessor.Add(CriarRegistroTipoPendencia(linha));
                                
                            }

                        }
                    }
                }
                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BVSP)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_BREAK_ASSESSOR_BVSP"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "DATA_INICIO", DbType.Date, pParametro.Objeto.DataInicial.ToString("dd/MM/yyyy"));
                        lAcessaDados.AddInParameter(lDbCommand, "DATA_FINAL", DbType.Date, pParametro.Objeto.DataFinal.ToString("dd/MM/yyyy"));
                        lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                        DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["cd_assessor"] = lDataTable.Rows[i]["cd_assessor"];
                                linha["nm_assessor"] = lDataTable.Rows[i]["nm_assessor"];
                                linha["CORRETAGEM"] = lDataTable.Rows[i]["CORRETAGEM"];
                                linha["VOLUME"] = lDataTable.Rows[i]["VOLUME"];
                                linha["Tipo"] = lDataTable.Rows[i]["Tipo"];
                                linha["CUSTODIA"] = lDataTable.Rows[i]["CUSTODIA"];

                                listaAssessor.Add(CriarRegistroTipoPendencia(linha));

                            }

                        }
                    }
                }
                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF_BVSP)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_BREAK_BMF_BVSP"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "DATA_INICIO", DbType.Date, pParametro.Objeto.DataInicial.ToString("dd/MM/yyyy"));
                        lAcessaDados.AddInParameter(lDbCommand, "DATA_FINAL", DbType.Date, pParametro.Objeto.DataFinal.ToString("dd/MM/yyyy"));
                        lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                        DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["cd_assessor"] = lDataTable.Rows[i]["cd_assessor"];
                                linha["nm_assessor"] = lDataTable.Rows[i]["nm_assessor"];
                                linha["CORRETAGEM"] = lDataTable.Rows[i]["CORRETAGEM"];
                                linha["VOLUME"] = lDataTable.Rows[i]["VOLUME"];
                                linha["Tipo"] = lDataTable.Rows[i]["Tipo"];
                                linha["CUSTODIA"] = lDataTable.Rows[i]["CUSTODIA"];

                                listaAssessor.Add(CriarRegistroTipoPendencia(linha));

                            }

                        }
                    }
                }

                resposta.Resultado.Add(new ResumoGerenteinfo());

                resposta.Resultado[0].ListaRessumoAssessor = listaAssessor;

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametro.Objeto, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public ConsultarObjetosResponse<DBMClienteInfo> ConsultarCliente(ConsultarEntidadeRequest<DBMClienteInfo> pParametro)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<DBMClienteInfo>();

                resposta.Resultado = new List<DBMClienteInfo>();
                DBMClienteInfo cliente;

                var lAcessaDados = new ConexaoDbHelper();

                var lDataTable = new DataTable();

                lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

                if (pParametro.Objeto.Mercado == DBMClienteInfo.TipoMercado.BVSP)
                {
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CLIENTES_BVSP"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                        lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                    }
                }
                else if (pParametro.Objeto.Mercado == DBMClienteInfo.TipoMercado.BMF)
                {

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CLIENTES_BMF"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                        lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                    }
                }
                else if (pParametro.Objeto.Mercado == DBMClienteInfo.TipoMercado.BMF_BVSP)
                {

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CLIENTES_BMF_BVSP"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                        lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);
                    }
                }

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        cliente = new DBMClienteInfo();

                        cliente.NomeAssessor = lDataTable.Rows[i]["NM_CLIENTE"].ToString();
                        cliente.CPF_CNPJ = lDataTable.Rows[i]["CD_CPFCGC"].ToString();
                        cliente.CodigoAssessor = lDataTable.Rows[i]["CD_ASSESSOR"].DBToInt32();
                        cliente.Corretagem = lDataTable.Rows[i]["CORRETAGEM"].DBToDecimal();
                        cliente.Volume = lDataTable.Rows[i]["VOLUME"].DBToDecimal();

                        resposta.Resultado.Add(cliente);
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametro.Objeto, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ResumoAssessorInfo CriarRegistroTipoPendencia(DataRow linha)
        {
            ResumoAssessorInfo ressumoAssessorInfo = new ResumoAssessorInfo();

            ressumoAssessorInfo.NomeAssessor    = linha["nm_assessor"].ToString();
            ressumoAssessorInfo.CodigoAssessor  = linha["cd_assessor"].ToString();
            ressumoAssessorInfo.Corretagem      = linha["CORRETAGEM"].ToString() != "" ?  Convert.ToDecimal(linha["CORRETAGEM"].ToString()) : 0;
            ressumoAssessorInfo.Volume          = linha["VOLUME"].ToString() != "" ? Convert.ToDecimal(linha["VOLUME"].ToString()) : 0;
            ressumoAssessorInfo.Custodia        = linha["CUSTODIA"].ToString() != "" ? Convert.ToDecimal(linha["CUSTODIA"].ToString()) : 0;
            ressumoAssessorInfo.Tipo            = linha["Tipo"].ToString();

            return ressumoAssessorInfo;

        }
    }
}
