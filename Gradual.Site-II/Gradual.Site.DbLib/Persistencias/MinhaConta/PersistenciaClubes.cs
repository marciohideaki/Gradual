using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Dados.MinhaConta;

namespace Gradual.Site.DbLib.Persistencias.MinhaConta
{
    public class PersistenciaClubes
    {
        public DateTime SelecionaUltimoDiaUtil()
        {
            AcessaDados _AcessaDados = new AcessaDados();

            DateTime dataRetorno;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoTrade;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RETORNA_ULTIMO_DIA_UTIL"))
            {
                DataTable tabela = _AcessaDados.ExecuteOracleDataTable(_DbCommand);

                dataRetorno = Convert.ToDateTime(tabela.Rows[0]["dataUtil"]);

            }
            return dataRetorno;
        }

        public ClubeResponse SelecionarClube(ClubeRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            ClubeResponse retorno = new ClubeResponse();

            retorno.ListaClube = new List<ClubeInfo>();

            ClubeInfo clube;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSisfinance;


            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MC_SELECIONAR_CLUBES"))
            {
                if (pRequest.Clube.IdBovespaClube > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@CD_BOVESPA", DbType.Int32, pRequest.Clube.IdBovespaClube);

                if (pRequest.Clube.IdBMFClube > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@CD_BMF", DbType.Int32, pRequest.Clube.IdBMFClube);

                if (pRequest.Clube.IdCliente > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@ID_CLIENTE", DbType.Int32, pRequest.Clube.IdCliente);

                _AcessaDados.AddInParameter(_DbCommand, "@NM_CLUBE", DbType.String, pRequest.Clube.NomeClube);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    clube = new ClubeInfo();

                    clube.IdBovespaClube = linha["CD_BOVESPA"].DBToInt32();

                    clube.IdBMFClube = linha["CD_BMF"].DBToInt32();

                    clube.NomeClube = linha["NM_CLUBE"].ToString();

                    retorno.ListaClube.Add(clube);

                }

            }
            return retorno;
        }

        public ClubeResponse SelecionarExtratoClube(ClubeRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            ClubeResponse retorno = new ClubeResponse();

            retorno.ListaClube = new List<ClubeInfo>();

            ClubeInfo clube;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSisfinance;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MC_SELECIONAR_CLUBES_EXTRATO"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@ID_CLIENTE", DbType.Int32, pRequest.Clube.IdBovespaClube);
                _AcessaDados.AddInParameter(_DbCommand, "@NM_CLUBE", DbType.String, pRequest.Clube.NomeClube);
                _AcessaDados.AddInParameter(_DbCommand, "@DT_INICIO_FUNDO", DbType.DateTime, pRequest.Clube.DataInicioPesquisa);
                _AcessaDados.AddInParameter(_DbCommand, "@DT_FINAL_FUNDO", DbType.DateTime, pRequest.Clube.DataFimPesquisa);
                _AcessaDados.AddInParameter(_DbCommand, "@CD_CLUBE", DbType.Int32, pRequest.Clube.IdClube);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    clube = new ClubeInfo()
                    {
                        IdCliente = linha["cd_cliente"].DBToInt32(),
                        Cota = linha["vl_cota"].DBToDecimal(),
                        DataAtualizacao = linha["dt_atualizacao"].DBToDateTime(),
                        IOF = linha["vl_iof"].DBToDecimal(),
                        IR = linha["vl_ir"].DBToDecimal(),
                        NomeClube = linha["ds_nome_clube"].ToString(),
                        Quantidade = linha["vl_quantidade"].DBToDecimal(),
                        ValorBruto = linha["vl_bruto"].DBToDecimal(),
                        ValorLiquido = linha["vl_liquido"].DBToDecimal(),
                    };

                    retorno.ListaClube.Add(clube);

                }

            }
            return retorno;
        }

        public ClubeResponse SelecionarPosicaoClube(ClubeRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            ClubeResponse retorno = new ClubeResponse();

            retorno.ListaClube = new List<ClubeInfo>();

            ClubeInfo clube;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoSisfinance;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MC_CLUBES_SEL"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@ID_CLIENTE", DbType.Int32, pRequest.Clube.IdCliente);
                _AcessaDados.AddInParameter(_DbCommand, "@NM_CLUBE", DbType.String, pRequest.Clube.NomeClube);
                _AcessaDados.AddInParameter(_DbCommand, "@DT_POSICAO", DbType.DateTime, pRequest.Clube.DataPosicao);

                if (pRequest.Clube.IdClube > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@CD_CLUBE_DV", DbType.Int32, pRequest.Clube.IdClube);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    clube = new ClubeInfo()
                        {
                            IdCliente = linha["cd_cliente"].DBToInt32(),
                            Cota = linha["vl_cota"].DBToDecimal(),
                            DataAtualizacao = linha["dt_atualizacao"].DBToDateTime(),
                            IOF = linha["vl_iof"].DBToDecimal(),
                            IR = linha["vl_ir"].DBToDecimal(),
                            NomeClube = linha["ds_nome_clube"].ToString(),
                            Quantidade = linha["vl_quantidade"].DBToDecimal(),
                            ValorBruto = linha["vl_bruto"].DBToDecimal(),
                            ValorLiquido = linha["vl_liquido"].DBToDecimal(),
                        };

                    retorno.ListaClube.Add(clube);

                }
            }

            return retorno;
        }

        public FundoResponse SelecionarFundo(FundoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            FundoResponse retorno = new FundoResponse();

            retorno.ListaFundo = new List<FundoInfo>();

            FundoInfo Fundo;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoOMS;

            
            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_MC_FUNDOS_SEL"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@ID_CLIENTE", DbType.Int32 , pRequest.Fundo.IdCliente);

                _AcessaDados.AddInParameter(_DbCommand, "@NM_FUNDO", DbType.String  , pRequest.Fundo.NomeFundo);

                if (pRequest.Fundo.CodigoFundo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@CD_CARTEIRA", DbType.UInt32, pRequest.Fundo.CodigoFundo);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    Fundo = new FundoInfo()
                    {
                        IdCliente       = linha["ID_CLIENTE"].DBToInt32()           ,
                        Cota            = linha["VL_COTA"].DBToDecimal()            ,
                        DataAtualizacao = linha["DT_ATUALIZACAO"].DBToDateTime()    ,
                        IOF             = linha["VL_IOF"].DBToDecimal()             ,
                        IR              = linha["VL_IR"].DBToDecimal()              ,
                        NomeFundo       = linha["DS_NOME_FUNDO"].ToString()         ,
                        Quantidade      = linha["VL_QUANTIDADE"].DBToDecimal()      ,
                        ValorBruto      = linha["VL_BRUTO"].DBToDecimal()           ,
                        ValorLiquido    = linha["VL_LIQUIDO"].DBToDecimal()         ,

                    };

                    retorno.ListaFundo.Add(Fundo);

                }

            }

            return retorno;
        }

        public FundoResponse SelecionarFundoItau(FundoRequest pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            FundoResponse lRetorno = new FundoResponse();

            lRetorno.ListaFundo = new List<FundoInfo>();

            FundoInfo lFundo;

            lAcessaDados.ConnectionStringName = "FundosItau";

            using (DbCommand _DbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
            {
                lAcessaDados.AddInParameter(_DbCommand, "@dsCpfCnpj", DbType.String, pRequest.CpfDoCliente.PadLeft(15, '0'));

                DataTable lTable = lAcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow lLinha in lTable.Rows)
                {
                    lFundo = new FundoInfo()
                    {
                          Cota            = lLinha["valorCota"].DBToDecimal()
                        , DataAtualizacao = lLinha["dtReferencia"].DBToDateTime()
                        , IOF             = lLinha["valorIOF"].DBToDecimal()
                        , IR              = lLinha["valorIR"].DBToDecimal()
                        , NomeFundo       = lLinha["dsRazaoSocial"].ToString()
                        , Quantidade      = lLinha["quantidadeCotas"].DBToDecimal()
                        , ValorBruto      = lLinha["valorBruto"].DBToDecimal()
                        , ValorLiquido    = lLinha["valorLiquido"].DBToDecimal()
                        , CodigoFundoItau = lLinha["dsCodFundo"].DBToString()
                    };

                    lRetorno.ListaFundo.Add(lFundo);
                }

            }

            return lRetorno;
        }


        public FundoResponse SelecionaFundoPorCliente(FundoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            FundoResponse Retorno = new FundoResponse();

            Retorno.ListaFundo = new List<FundoInfo>();

            FundoInfo Fundo;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoDrive;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SMC_FUNDOS_CLIENTES"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.Fundo.IdCliente);

                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    Fundo = new FundoInfo()
                    {
                        IdCliente       = linha["ID_CLIENTE"].DBToInt32()   ,
                        NomeFundo       = linha["NOME"].DBToString()        ,
                        CGC             = linha["CGC"].DBToInt32()          ,
                        Administrador   = linha["ADMINISTRADOR"].DBToInt32(),
                        CodigoFundo     = linha["CARTEIRA"].DBToInt32()     ,
                        TipoCarteira    = linha["TIPOCARTEIRA"].DBToInt32() ,

                    };

                    Retorno.ListaFundo.Add(Fundo);

                }

            }

            return Retorno;
        }

        public FundoResponse SelecionaFundoPorCotasClientes(FundoRequest pRequest)
        {
            AcessaDados _AcessaDados = new AcessaDados();

            FundoResponse Retorno = new FundoResponse();

            Retorno.ListaFundo = new List<FundoInfo>();
            
            FundoInfo Fundo;

            _AcessaDados.ConnectionStringName = ServicoPersistenciaSite.ConexaoDrive;

            using (DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SMC_FUNDOS_COTAS"))
            {
                _AcessaDados.AddInParameter(_DbCommand, "@P_ID_CLIENTE", DbType.Int32, pRequest.Fundo.IdCliente);

                if (pRequest.Fundo.CodigoFundo > 0)
                    _AcessaDados.AddInParameter(_DbCommand, "@P_CODIGO_FUNDO", DbType.Int32, pRequest.Fundo.CodigoFundo);

                _AcessaDados.AddInParameter(_DbCommand, "@P_DATA_INICIAL", DbType.DateTime, pRequest.Fundo.DataInicioPesquisa);

                _AcessaDados.AddInParameter(_DbCommand, "@P_DATA_FINAL", DbType.DateTime, pRequest.Fundo.DataFimPesquisa);
                
                DataTable tabela = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow linha in tabela.Rows)
                {
                    Fundo = new FundoInfo()
                    {
                        IdCliente       = linha["CODCLI"].DBToInt32()       ,
                        Cota            = linha["VALORCOTA"].DBToDecimal()  ,
                        DataAtualizacao = linha["DATA"].DBToDateTime()      ,
                        IOF             = linha["VALORIOF"].DBToDecimal()   ,
                        IR              = linha["VALORIR"].DBToDecimal()    ,
                        NomeFundo       = linha["NOME"].DBToString()        ,
                        ValorBruto      = linha["VALORBRUTO"].DBToDecimal() ,
                        ValorLiquido    = linha["VALORLIQ"].DBToDecimal()   ,
                        CodigoFundo     = linha["CODCART"].DBToInt32()      ,
                        Operacao        = linha["OPERACAO"].DBToString()    ,
                        Quantidade      = linha["QTDCOTA"].DBToDecimal()   ,

                    };

                    Retorno.ListaFundo.Add(Fundo);

                }

            }

            return Retorno;
        }
    }
}
