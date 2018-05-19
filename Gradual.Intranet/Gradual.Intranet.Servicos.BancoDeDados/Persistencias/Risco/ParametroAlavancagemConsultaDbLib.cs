using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public partial class RiscoDbLib
    {
        public ConsultarObjetosResponse<ParametroAlavancagemConsultaInfo> ConsultarClienteParametroAlavancagem(ConsultarEntidadeRequest<ParametroAlavancagemConsultaInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ParametroAlavancagemConsultaInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;
            lRetorno.Resultado = new List<ParametroAlavancagemConsultaInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_parametro_alavancagem_cliente_rel_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_grupo", DbType.Int32, pParametros.Objeto.ConsultaIdGrupo);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, pParametros.Objeto.ConsultaCdAssessor);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.Int32, pParametros.Objeto.ConsultaCdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new ParametroAlavancagemConsultaInfo() 
                        {
                            CdAssessor = lLinha["cd_assessor"].DBToInt32(),
                            CdCliente = lLinha["id_cliente"].DBToInt32(),
                            DtInclusao = lLinha["dt_inclusao"].DBToDateTime(),
                            DsGrupo = lLinha["ds_grupo"].DBToString(),
                        });
            }

            return lRetorno;
        }

        public RemoverObjetoResponse<ParametroAlavancagemConsultaInfo> ExcluirClienteParametro(RemoverEntidadeRequest<ParametroAlavancagemConsultaInfo> pParametro)
        {
            var lRetorno = new RemoverObjetoResponse<ParametroAlavancagemConsultaInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoRiscoNovoOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_parametro_alavancagem_del"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.ConsultaCdCliente);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lRetorno;
        }
    }
}
