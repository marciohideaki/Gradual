using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Fundos;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static List<FundosInfo> ListarFundos()
        {
            var lRetorno = new List<FundosInfo>();
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoFundos;

                string lSql = "select idProduto, dsNomeProduto from tbProduto order by dsNomeProduto asc";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        var lFundo = new FundosInfo();

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++) 
                        {
                            lFundo = new FundosInfo();

                            lFundo.CodigoFundo = lDataTable.Rows[i]["idProduto"].DBToInt32();
                            lFundo.NomeFundo = lDataTable.Rows[i]["dsNomeProduto"].DBToString();

                            lRetorno.Add(lFundo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lRetorno;
        }

        public static ConsultarObjetosResponse<FundosInfo> ConsultarFundoTermo(ConsultarEntidadeRequest<FundosInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<FundosInfo> lRetorno = new ConsultarObjetosResponse<FundosInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoFundos;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_termo_sel"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoClienteFundo", DbType.Int32, pParametros.Objeto.CodigoClienteFundo);

                    
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        var lFundo = new FundosInfo();

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lFundo = new FundosInfo();

                            lFundo.CodigoClienteFundo  = lDataTable.Rows[i]["idCliente"].DBToInt32();
                            lFundo.CodigoFundo         = lDataTable.Rows[i]["idFundo"].DBToInt32();
                            lFundo.CodigoFundoAnbima   = lDataTable.Rows[i]["CodigoAnbima"].DBToString();
                            lFundo.CodigoFundoTermo    = lDataTable.Rows[i]["idTermo"].DBToInt32();
                            lFundo.NomeFundo           = lDataTable.Rows[i]["NomeFundo"].DBToString();
                            lFundo.DataAdesao          = lDataTable.Rows[i]["DataAdesao"].DBToDateTime();
                            lFundo.Origem              = lDataTable.Rows[i]["Origem"].DBToString();
                            lFundo.UsuarioLogado       = lDataTable.Rows[i]["UsuarioLogado"].DBToString();
                            lFundo.CodigoUsuarioLogado = lDataTable.Rows[i]["CodigoUsuarioLogado"].DBToInt32();

                            lRetorno.Resultado.Add(lFundo);
                        }
                    }
                }
                

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<FundosInfo> SalvarFundoTermo(SalvarObjetoRequest<FundosInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoFundos;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_fundo_termo_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoClienteFundo",  DbType.Int32,   pParametros.Objeto.CodigoClienteFundo);
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoFundo",         DbType.Int32,   pParametros.Objeto.CodigoFundo);
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_usuario_logado",   DbType.Int32,   pParametros.Objeto.CodigoUsuarioLogado);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_usuario_logado",   DbType.String,  pParametros.Objeto.UsuarioLogado);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_origem",           DbType.String,  pParametros.Objeto.Origem);

                    lAcessaDados.AddOutParameter(lDbCommand, "@id_termo", DbType.Int32, 8);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<FundosInfo> lRetorno = new SalvarEntidadeResponse<FundosInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_termo"].Value)
                    };

                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }


        //private static void LogarModificacao(ContratoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        //{
        //    ReceberEntidadeRequest<ContratoInfo> lEntrada = new ReceberEntidadeRequest<ContratoInfo>();
        //    lEntrada.Objeto = pParametro;
        //    ReceberObjetoResponse<ContratoInfo> lRetorno = ReceberContrato(lEntrada);
        //    LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        //}


    }
}
