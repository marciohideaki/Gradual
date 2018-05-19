using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        private static PessoaExpostaPoliticamenteInfo CriarRegistroPessoaExpostaPoliticamente(DataRow linha)
        {
            return new PessoaExpostaPoliticamenteInfo()
            {
                IdPEP = linha["id_pep"].DBToInt32(),
                DsNome = linha["ds_nome"].DBToString(),
                DsDocumento = linha["ds_documento"].DBToString(),
                DtImportacao = linha["dt_importacao"].DBToDateTime()
            };
        }

        public static ConsultarObjetosResponse<PessoaExpostaPoliticamenteInfo> ConsultarPessoaExpostaPoliticamente(ConsultarEntidadeRequest<PessoaExpostaPoliticamenteInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<PessoaExpostaPoliticamenteInfo> resposta =
                    new ConsultarObjetosResponse<PessoaExpostaPoliticamenteInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pep_lst_sp"))
                {
                    if (pParametros.Objeto != null)
                    {
                        if (pParametros.Objeto.DsDocumento != null && pParametros.Objeto.DsDocumento != string.Empty)
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_documento", DbType.String, pParametros.Objeto.DsDocumento);
                        if (pParametros.Objeto.DsNome != null && pParametros.Objeto.DsNome != string.Empty)
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);
                    }

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            resposta.Resultado.Add(CriarRegistroPessoaExpostaPoliticamente(lDataTable.Rows[i]));
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

        /// <summary>
        /// Esse método funciona passando tanto ID como Documento como Nome, nessa ordem de prioridade. 
        /// </summary>
        /// <param name="pParametros">Passando ID, ele busca por ID e ignora os outros dois. Passando ID nulo e Documento, busca por documento.</param>
        /// <returns></returns>
        public static ReceberObjetoResponse<PessoaExpostaPoliticamenteInfo> ReceberPessoaExpostaPoliticamente(ReceberEntidadeRequest<PessoaExpostaPoliticamenteInfo> pParametros)
        {
            try
            {
                ReceberObjetoResponse<PessoaExpostaPoliticamenteInfo> resposta =
                    new ReceberObjetoResponse<PessoaExpostaPoliticamenteInfo>();

                resposta.Objeto = new PessoaExpostaPoliticamenteInfo();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pep_sel_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_pep", DbType.Int32, pParametros.Objeto.IdPEP);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_documento", DbType.String, pParametros.Objeto.DsDocumento);
                    lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, pParametros.Objeto.DsNome);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        resposta.Objeto = CriarRegistroPessoaExpostaPoliticamente(lDataTable.Rows[0]);
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


        public static SalvarEntidadeResponse<PessoaExpostaPoliticamenteImportacaoInfo> SalvarPessoaExpostaPoliticamente(SalvarObjetoRequest<PessoaExpostaPoliticamenteImportacaoInfo> pParametros)
        {
            try
            {
                SalvarEntidadeResponse<PessoaExpostaPoliticamenteImportacaoInfo> lRetorno = new SalvarEntidadeResponse<PessoaExpostaPoliticamenteImportacaoInfo>();

                pParametros.Objeto.MensagensDeErro = new List<string>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pep_ins_sp"))
                {
                    foreach (PessoaExpostaPoliticamenteInfo lPessoa in pParametros.Objeto.ListaParaImportar)
                    {
                        try
                        {
                            lDbCommand.Parameters.Clear();

                            lAcessaDados.AddInParameter(lDbCommand, "@ds_documento", DbType.String, lPessoa.DsDocumento);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, lPessoa.DsNome);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_identificacao", DbType.String, lPessoa.DsIdentificacao);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_linha", DbType.String, lPessoa.DsLinha);

                            lAcessaDados.AddOutParameter(lDbCommand, "@id_pep", DbType.Int32, 8);

                            lAcessaDados.ExecuteNonQuery(lDbCommand);

                            pParametros.Objeto.RegistrosImportadosComSucesso++;
                        }
                        catch (Exception exInsert)
                        {
                            pParametros.Objeto.RegistrosImportadosComErro++;

                            pParametros.Objeto.MensagensDeErro.Add(string.Format("Erro ao cadastrar CPF [{0}]: [{1}]\r\n{2}", lPessoa.DsDocumento, exInsert.Message, exInsert.StackTrace));
                        }
                    }

                }
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir);

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Inserir, ex);
                throw ex;
            }
        }

        /*
        private static SalvarEntidadeResponse<PessoaExpostaPoliticamenteInfo> Atualizar (SalvarObjetoRequest<PessoaExpostaPoliticamenteInfo> pParametros)
        {
            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexao;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "paises_blacklist_upd_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_pais", DbType.String, pParametros.Objeto.CdPais);
                lAcessaDados.AddInParameter(lDbCommand, "@id_pais_blacklist", DbType.Int32, pParametros.Objeto.IdPaisBlackList);                

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                return new SalvarEntidadeResponse<PessoaExpostaPoliticamenteInfo>();
            }
        }*/


        private static void LogarModificacao(PessoaExpostaPoliticamenteInfo pParametro, int IdUsuarioLogado, string DescricaoUsuarioLogado)
        {
            ReceberEntidadeRequest<PessoaExpostaPoliticamenteInfo> lEntrada = new ReceberEntidadeRequest<PessoaExpostaPoliticamenteInfo>();
            lEntrada.Objeto = pParametro;
            ReceberObjetoResponse<PessoaExpostaPoliticamenteInfo> lRetorno = ReceberPessoaExpostaPoliticamente(lEntrada);
            LogCadastro.Logar(lRetorno.Objeto, IdUsuarioLogado, DescricaoUsuarioLogado, LogCadastro.eAcao.Receber);
        }


    }
}
