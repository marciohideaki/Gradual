using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.Risco
{
    public partial class RiscoRelDbLib
    {
        #region | Métodos CRUD

        public ConsultarObjetosResponse<RiscoClienteMercadoVistaOpcaoRelInfo> ConsultarRiscoMercadoVistaOpcao(ConsultarEntidadeRequest<RiscoClienteMercadoVistaOpcaoRelInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<RiscoClienteMercadoVistaOpcaoRelInfo>();
            var lAcessaDados = new AcessaDados();
            lAcessaDados.ConnectionStringName = "SinacorExportacao"; 

            lRetorno.Resultado = new List<RiscoClienteMercadoVistaOpcaoRelInfo>();

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_REL_MERC_VIS_X_OPC"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pDs_ativo", DbType.String, pParametros.Objeto.ConsultaDsAtivo);
                
                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Resultado.Add(this.CarregarEntidadeRiscoClienteMercadoVistaOpcao(lDataTable.Rows[i]));
                    }
            }

            return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        private RiscoClienteMercadoVistaOpcaoRelInfo CarregarEntidadeRiscoClienteMercadoVistaOpcao(DataRow pLinha)
        {
            return new RiscoClienteMercadoVistaOpcaoRelInfo()
            {
                CodigoAssessor  =  pLinha["COD_ASSE"].DBToInt32(),
                CodigoCliente   =  pLinha["COD_CLI"].DBToInt32(),
                NomeCliente     =  pLinha["NOME_CLI"].ToString(),
                CodigoNegocio   =  pLinha["COD_NEG"].ToString(),
                QtdeTotal       =  pLinha["QTDE_TOT"].ToString(),
                QtdeD1          =  pLinha["QTDE_DA1"].ToString(),
                QtdeD2          =  pLinha["QTDE_DA2"].ToString(),
                QtdeD3          =  pLinha["QTDE_DA3"].ToString(),
            };
        }

        #endregion
    }
}
