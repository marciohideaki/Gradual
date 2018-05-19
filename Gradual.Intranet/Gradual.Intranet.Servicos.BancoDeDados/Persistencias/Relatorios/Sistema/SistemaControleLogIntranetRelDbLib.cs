using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.Sistema;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Sistema
{
    public static partial class SistemaRelDbLib
    {
        public static ConsultarObjetosResponse<SistemaControleLogIntranetRelInfo> ConsultarControleLogIntranet(ConsultarEntidadeRequest<SistemaControleLogIntranetRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<SistemaControleLogIntranetRelInfo>();
            var lAcessaDados = new ConexaoDbHelper();
            lRetorno.Resultado = new List<SistemaControleLogIntranetRelInfo>();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "rel_LogIntranet_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@dt_ate", DbType.Date, pParametros.Objeto.ConsultaDataAte);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_de", DbType.Date, pParametros.Objeto.ConsultaDataDe);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_emailUsuario", DbType.String, pParametros.Objeto.ConsultaEmailUsuario);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_tela", DbType.String, pParametros.Objeto.ConsultaTela);
                lAcessaDados.AddInParameter(lDbCommand, "@id_acao", DbType.Int32, pParametros.Objeto.ConsultaTipoAcao);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new SistemaControleLogIntranetRelInfo() 
                        {
                            IdEvento = lLinha["id_acao"].DBToInt32(),
                            DsCpfCnpj = lLinha["ds_cpfcnpj_cliente"].DBToString(),
                            DsEmailUsuario = lLinha["ds_email"].DBToString(),
                            DsIp = lLinha["ds_ip"].DBToString(),
                            DsNomeUsuario = lLinha["ds_nome_usuario"].DBToString(),
                            DsObservacao = lLinha["ds_observacao"].DBToString(),
                            NmCliente = lLinha["ds_nome_cliente"].DBToString(),
                            DtEvento = lLinha["dt_evento"].DBToDateTime(),
                            NmTela = lLinha["ds_tela"].DBToString(),
                        });
            }

            return lRetorno;
        }
    }
}
