using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        #region | ConsultarEmailDisparadoPeriodo

        /// <summary>
        /// Lista os clientes cadastrados em um certo período
        /// </summary>
        /// <param name="pParametros">Entidade do tipo ClienteCadastradoPeriodoInfo</param>
        /// <returns>Retorna uma lista de clientes cadastrados em um certo período</returns>
        public static ConsultarObjetosResponse<EmailDisparadoPeriodoInfo> ConsultarEmailDisparadoPeriodo(ConsultarEntidadeRequest<EmailDisparadoPeriodoInfo> pParametros)
        {
            ConsultarObjetosResponse<EmailDisparadoPeriodoInfo> lResposta =
                new ConsultarObjetosResponse<EmailDisparadoPeriodoInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;
            
            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_email_disparado_periodo_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@DtDe",             DbType.DateTime,    pParametros.Objeto.DtDe);
                lAcessaDados.AddInParameter(lDbCommand, "@DtAte",            DbType.DateTime,    pParametros.Objeto.DtAte.Value.AddDays(1));
                lAcessaDados.AddInParameter(lDbCommand, "@IdTipoEmail",      DbType.Int32,  (int)pParametros.Objeto.ETipoEmailDisparo);
                lAcessaDados.AddInParameter(lDbCommand, "@DsEmailDestinatario", DbType.String, pParametros.Objeto.DsEmailDestinatario);
                lAcessaDados.AddInParameter(lDbCommand, "@TipoPessoa",       DbType.String,     pParametros.Objeto.TipoPessoa);
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpj", DbType.String, pParametros.Objeto.DsCpfCnpj);

                DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        lResposta.Resultado.Add(CriarRegistroEmailDisparadoPeriodo(lDataTable.Rows[i]));

            }
            return lResposta;
        }

        public static SalvarEntidadeResponse<EmailDisparadoPeriodoInfo> SalvarEmailDisparadoPeriodoInfo(SalvarObjetoRequest<EmailDisparadoPeriodoInfo> EmailDisparadoPeriodoInfo)
        {
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_email_disparado_periodo_ins_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_tipoemail", DbType.Int64, (long)EmailDisparadoPeriodoInfo.Objeto.ETipoEmailDisparo);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_corpoemail", DbType.String, EmailDisparadoPeriodoInfo.Objeto.DsCorpoEmail);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_emailremetente", DbType.String, EmailDisparadoPeriodoInfo.Objeto.DsEmailRemetente.Trim(';'));
                lAcessaDados.AddInParameter(lDbCommand, "@ds_emaildestinatario", DbType.String, EmailDisparadoPeriodoInfo.Objeto.DsEmailDestinatario.Trim(';'));
                lAcessaDados.AddInParameter(lDbCommand, "@dt_envioemail", DbType.DateTime, EmailDisparadoPeriodoInfo.Objeto.DtEnvio);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_assuntoemail", DbType.String, EmailDisparadoPeriodoInfo.Objeto.DsAssuntoEmail);
                
                if (!EmailDisparadoPeriodoInfo.Objeto.IdCliente.Equals(0))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, EmailDisparadoPeriodoInfo.Objeto.IdCliente);
                }
                
                lAcessaDados.AddInParameter(lDbCommand, "@ds_perfil", DbType.String, EmailDisparadoPeriodoInfo.Objeto.DsPerfil);

                lAcessaDados.AddOutParameter(lDbCommand, "@id_email", DbType.Int64, 16);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                var response = new SalvarEntidadeResponse<EmailDisparadoPeriodoInfo>()
                {
                    Codigo = Convert.ToInt32(lDbCommand.Parameters["@id_email"].Value)
                };

                return response;
            }
        }

        #endregion

        #region | Métodos Apoio

        /// <summary>
        /// Método de apoio para criação/preenchimento de entidade do tipo EmailDisparadoPeriodoInfo
        /// </summary>
        /// <param name="linha">DataRow do relatório de emails disparados por período</param>
        /// <returns>Retorna uma entidade do tipo EmailDisparadoPeriodoInfo preenchida</returns>
        private static EmailDisparadoPeriodoInfo CriarRegistroEmailDisparadoPeriodo(DataRow linha)
        {
            return new EmailDisparadoPeriodoInfo()
            {
                CdCodigo            = linha["cd_codigo"].DBToString(),
                DsCpfCnpj           = linha["ds_cpfcnpj"].DBToString(),
                DsEmailRemetente    = linha["ds_emailremetente"].DBToString(),
                DsEmailDestinatario = linha["ds_emaildestinatario"].DBToString(),
                DtEnvio             = linha["dt_envioemail"].DBToDateTime(),
                DsCorpoEmail        = linha["ds_corpoemail"].DBToString(),
                DsAssuntoEmail      = linha["ds_assuntoemail"].DBToString(),
                ETipoEmailDisparo   = (eTipoEmailDisparo)linha["id_tipoemail"].DBToInt32(),
            };
        }

        #endregion
    }
}
