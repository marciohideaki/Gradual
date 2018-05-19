using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using System.Collections.Generic;
using System.Collections;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {        
        #region | Atributos

        private static Hashtable LstPaises     = new Hashtable();
        private static Hashtable LstAtividades = new Hashtable();

        #endregion

        #region | ConsultarClienteSemEmail

        /// <summary>
        /// Relatório de clientes Suspeitos
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClienteSuspeitoInfo</param>
        /// <returns>Retorna uma lista de clientes suspeitos, atividades ilícitas, etc</returns>
        public static ConsultarObjetosResponse<ClienteSuspeitoInfo> ConsultarClienteSuspeito(ConsultarEntidadeRequest<ClienteSuspeitoInfo> pParametros)
        {
            ConsultarObjetosResponse<ClienteSuspeitoInfo> lResposta = new ConsultarObjetosResponse<ClienteSuspeitoInfo>();

            PreenchePaisesAtividadesSinacor();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_cliente_suspeito_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe",            DbType.DateTime,   pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte",           DbType.DateTime,   pParametros.Objeto.DtAte);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoBolsa",     DbType.Int64,      pParametros.Objeto.CodigoBolsa);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoPais",      DbType.String,     pParametros.Objeto.CdPais);
                lAcessaDados.AddInParameter(lDbCommand, "@CodigoAtividade", DbType.Int32,      pParametros.Objeto.CdAtividade);
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa",      DbType.AnsiString, pParametros.Objeto.TipoPessoa);
                lAcessaDados.AddInParameter(lDbCommand, "@CdAssessor",      DbType.AnsiString, pParametros.Objeto.CdAssessor); 

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroClienteSuspeito(lDataTable.Rows[i]));

            }
            return lResposta;
        }

        #endregion

        #region | Métodos Apoio

        /// <summary>
        /// Método de apoio para uso de preenchimento da entidade ClienteSuspeitoInfo
        /// </summary>
        /// <param name="linha">DataRow do relatório de clientes supeitos</param>
        /// <returns>Retorna uma entidade do tipo ClienteSuspeitoInfo preenchida</returns>
        private static ClienteSuspeitoInfo CriarRegistroClienteSuspeito(DataRow linha)
        {
            return new ClienteSuspeitoInfo()
            {
                CdPais              = linha["cd_pais"].DBToString(),
                CdAtividade         = linha["cd_atividade"].DBToInt32(),
                blnExportado        = linha["bl_exportado"].DBToBoolean(),
                DsAtividadeIlicita  = LstAtividades[linha["cd_atividade"].DBToString()].DBToString(),
                NmPaisBlackList     = LstPaises[linha["cd_pais"].ToString()].DBToString(),
                IdCliente           = linha["id_cliente"].DBToInt32(),
                DsNomeCliente       = linha["ds_nome"].DBToString(),
                TipoPessoa          = linha["tp_pessoa"].DBToString(),
                DsCpfCnpj           = linha["ds_cpfcnpj"].DBToString(),
                IdEndereco          = linha["id_endereco"].DBToInt64(),
                DtCadastro          = linha["dt_cadastro"].DBToDateTime(),
                CdAssessor          = linha["cd_assessor"].DBToString(),
                CdBovespa           = linha["cd_codigo"].DBToString(),                
            };
        }

        /// <summary>
        /// Método de apoio para preenchimento das listas de atividades e países
        /// </summary>
        private static void PreenchePaisesAtividadesSinacor ()
        {
            LstAtividades.Clear();
            LstPaises.Clear();

            ConsultarObjetosResponse<SinacorListaInfo> lRespostaAtividade =  
                ConsultarListaSinacor(new ConsultarEntidadeRequest<SinacorListaInfo>(){Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.AtividadePFePJ
                    }
                }
            );

            ConsultarObjetosResponse<SinacorListaInfo> lRespostaPaises =
                ConsultarListaSinacor(new ConsultarEntidadeRequest<SinacorListaInfo>()
                {
                    Objeto = new SinacorListaInfo()
                    {
                        Informacao = eInformacao.Pais
                    }
                }
            );

            lRespostaAtividade.Resultado.ForEach(delegate(SinacorListaInfo item) { LstAtividades.Add(item.Id, item.Value); });
            lRespostaPaises.Resultado.ForEach(delegate(SinacorListaInfo item) { LstPaises.Add(item.Id, item.Value); });
        }

        #endregion
    }
}
