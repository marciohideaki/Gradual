using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using Gradual.OMS.Persistencia;
using System;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public partial class RiscoDbLib
    {
        public static SalvarEntidadeResponse<RiscoClienteGrupoInfo> SalvarClienteGrupo(SalvarObjetoRequest<RiscoClienteGrupoInfo> pParametros)
        {
            /*
            return (null != pParametros.Objeto.CdAssessor) ? InserirViaAssessor(pParametros)
                                                           : InserirViaCliente(pParametros);
             */
            return InserirViaCliente(pParametros);
        }

        private static SalvarEntidadeResponse<RiscoClienteGrupoInfo> InserirViaAssessor(SalvarObjetoRequest<RiscoClienteGrupoInfo> pParametros)
        {
            var lRetorno = new SalvarEntidadeResponse<RiscoClienteGrupoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_alavancagem_cliente_assessor_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.Objeto.IdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, pParametros.Objeto.CdAssessor);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("percente a um grupo"))
                    throw new ArgumentException(ex.Message);
                else if(ex.Message.ToLower().Contains("existe no sistema ou"))
                    throw new ArgumentException(ex.Message);
                else throw ex;
            }

            return lRetorno;
        }

        private static SalvarEntidadeResponse<RiscoClienteGrupoInfo> InserirViaCliente(SalvarObjetoRequest<RiscoClienteGrupoInfo> pParametros)
        {
            var lRetorno = new SalvarEntidadeResponse<RiscoClienteGrupoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            try
            {
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_alavancagem_cliente_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.Objeto.IdGrupo);
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.CdCliente);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("percente a um grupo"))
                    throw new ArgumentException(ex.Message);

                else if (ex.Message.ToLower().Contains("existe no sistema ou"))
                    throw new ArgumentException(ex.Message);

                else throw ex;
            }

            return lRetorno;
        }
    }
}
