using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<ArquivoContratoInfo> ConsultarArquivosContratos(ConsultarEntidadeRequest<ArquivoContratoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ArquivoContratoInfo> resposta =
                    new ConsultarObjetosResponse<ArquivoContratoInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "arquivo_contrato_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ArquivoContrato", DbType.Int32, pParametros.Objeto.IdArquivoContrato == 0 ? (object)System.DBNull.Value : pParametros.Objeto.IdArquivoContrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pParametros.Objeto.IdContrato == 0 ? (object)System.DBNull.Value : pParametros.Objeto.IdContrato);
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow item in lDataTable.Rows)
                        {
                            resposta.Resultado.Add(CriarRegistroArquivoContrato(item, false));
                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public static ReceberObjetoResponse<ArquivoContratoInfo> ReceberArquivosContratos(ReceberEntidadeRequest<ArquivoContratoInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<ArquivoContratoInfo> resposta =
                    new ReceberObjetoResponse<ArquivoContratoInfo>();

                resposta.Objeto = new ArquivoContratoInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "arquivo_contrato_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ArquivoContrato", DbType.Int32, pParametros.Objeto.IdArquivoContrato);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto = CriarRegistroArquivoContrato(lDataTable.Rows[0], true);
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Receber, ex);
                throw ex;
            }
        }

        private static ArquivoContratoInfo CriarRegistroArquivoContrato(DataRow linha, Boolean TrazerArquivo)
        {
            ArquivoContratoInfo lArquivoContratoInfo = new ArquivoContratoInfo();

            lArquivoContratoInfo.IdContrato = linha["id_contrato"].DBToInt32();
            lArquivoContratoInfo.IdArquivoContrato = linha["id_ArquivoContrato"].DBToInt32();
            lArquivoContratoInfo.MIMEType = linha["mime_type"].DBToString();
            lArquivoContratoInfo.Extensao = linha["extensao"].DBToString();
            lArquivoContratoInfo.Tamanho = linha["tamanho"].DBToInt32();
            lArquivoContratoInfo.Nome = linha["nome"].DBToString();
            if (TrazerArquivo)
                lArquivoContratoInfo.Arquivo = (byte[])linha["arquivo"];


            return lArquivoContratoInfo;
        }

        public static RemoverObjetoResponse<ArquivoContratoInfo> RemoverArquivosContratos(RemoverEntidadeRequest<ArquivoContratoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "arquivo_contrato_del_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ArquivoContrato", DbType.Int32, pParametros.Objeto.IdArquivoContrato);
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }

                RemoverEntidadeResponse<ArquivoContratoInfo> response = new RemoverEntidadeResponse<ArquivoContratoInfo>()
                {
                    lStatus = true
                };
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir);

                return response;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Excluir, ex);
                throw ex;
            }
        }

        public static SalvarEntidadeResponse<ArquivoContratoInfo> SalvarArquivosContratos(SalvarObjetoRequest<ArquivoContratoInfo> pParametros)
        {
            if (pParametros.Objeto.IdArquivoContrato > 0)
            {
                return Atualizar(pParametros);
            }
            else
            {
                return Salvar(pParametros);
            }
        }

        private static SalvarEntidadeResponse<ArquivoContratoInfo> Salvar(SalvarObjetoRequest<ArquivoContratoInfo> pParametros)
        {
            try
            {
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "arquivo_contrato_ins_sp"))
                {
                    lAcessaDados.AddOutParameter(lDbCommand, "@id_ArquivoContrato", DbType.Int32, 8);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pParametros.Objeto.IdContrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@arquivo", DbType.Binary, pParametros.Objeto.Arquivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@extensao", DbType.String, pParametros.Objeto.Extensao);
                    lAcessaDados.AddInParameter(lDbCommand, "@mime_type", DbType.String, pParametros.Objeto.MIMEType);
                    lAcessaDados.AddInParameter(lDbCommand, "@tamanho", DbType.Int32, pParametros.Objeto.Tamanho);
                    lAcessaDados.AddInParameter(lDbCommand, "@nome", DbType.String, pParametros.Objeto.Nome);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);

                    SalvarEntidadeResponse<ArquivoContratoInfo> response = new SalvarEntidadeResponse<ArquivoContratoInfo>()
                    {
                        Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_ArquivoContrato"].Value)
                    };
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        private static SalvarEntidadeResponse<ArquivoContratoInfo> Atualizar(SalvarObjetoRequest<ArquivoContratoInfo> pParametros)
        {
            try
            {
                LogarModificacao(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado);
                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "arquivo_contrato_upd_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_contrato", DbType.Int32, pParametros.Objeto.IdContrato);
                    lAcessaDados.AddInParameter(lDbCommand, "@arquivo", DbType.Binary, pParametros.Objeto.Arquivo);
                    lAcessaDados.AddInParameter(lDbCommand, "@extensao", DbType.String, pParametros.Objeto.Extensao);
                    lAcessaDados.AddInParameter(lDbCommand, "@mime_type", DbType.String, pParametros.Objeto.MIMEType);
                    lAcessaDados.AddInParameter(lDbCommand, "@tamanho", DbType.Int32, pParametros.Objeto.Tamanho);
                    lAcessaDados.AddInParameter(lDbCommand, "@nome", DbType.String, pParametros.Objeto.Nome);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_ArquivoContrato", DbType.Int32, pParametros.Objeto.IdArquivoContrato);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                    LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar);

                    return new SalvarEntidadeResponse<ArquivoContratoInfo>();
                }
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Alterar, ex);
                throw ex;
            }
        }


        private static void LogarModificacao(ArquivoContratoInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<ArquivoContratoInfo> lEntrada = new ReceberEntidadeRequest<ArquivoContratoInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<ArquivoContratoInfo> lRetorno = ReceberArquivosContratos(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}

