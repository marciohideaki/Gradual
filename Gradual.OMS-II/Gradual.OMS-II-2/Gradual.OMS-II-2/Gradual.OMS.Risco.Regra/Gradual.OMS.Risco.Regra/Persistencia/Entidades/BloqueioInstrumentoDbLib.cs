using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Risco.Regra.Persistencia.Entidades;
using Gradual.OMS.Risco.Regra.Lib.Dados;
using Gradual.Generico.Dados;
using Gradual.OMS.Persistencia;
using System.Data.Common;
using System.Data;
using Gradual.OMS.Risco.Regra.Persistencia.DB;
using Gradual.OMS.Risco.Regra;

namespace Gradual.OMS.Risco.Persistencia.Entidades
{
    public class BloqueioInstrumentoDbLib : IEntidadeDbLib<BloqueioInstrumentoInfo>
    {
        public ConsultarObjetosResponse<BloqueioInstrumentoInfo> ConsultarObjetos(OMS.Persistencia.ConsultarObjetosRequest<BloqueioInstrumentoInfo> pParametro)
        {
            var lRetorno = new ConsultarObjetosResponse<BloqueioInstrumentoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Condicoes[0].Valores[0]);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.Resultado.Add(
                            new BloqueioInstrumentoInfo()
                            {
                                CdAtivo = lDataTable.Rows[i]["cd_ativo"].DBToString(),
                                Direcao = lDataTable.Rows[i]["direcao"].DBToString(),
                                IdCliente = lDataTable.Rows[i][""].DBToInt32(),
                            });
                    }
                }
            }

            return lRetorno;
        }

        public ReceberObjetoResponse<BloqueioInstrumentoInfo> ReceberObjeto(OMS.Persistencia.ReceberObjetoRequest<BloqueioInstrumentoInfo> lRequest)
        {
            return null;
        }

        public RemoverObjetoResponse<BloqueioInstrumentoInfo> RemoverObjeto(OMS.Persistencia.RemoverObjetoRequest<BloqueioInstrumentoInfo> lRequest)
        {
            return null;
        }

        public SalvarObjetoResponse<BloqueioInstrumentoInfo> SalvarObjeto(OMS.Persistencia.SalvarObjetoRequest<BloqueioInstrumentoInfo> pParametro)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new SalvarObjetoResponse<BloqueioInstrumentoInfo>();

            lAcessaDados.ConnectionStringName = "RISCO_GRADUALOMS";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_bloqueio_instrumento_ins"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametro.Objeto.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@cd_ativo", DbType.String, pParametro.Objeto.CdAtivo);
                lAcessaDados.AddInParameter(lDbCommand, "@direcao", DbType.String, pParametro.Objeto.Direcao);

                lAcessaDados.ExecuteNonQuery(lDbCommand);
            }

            return lRetorno;
        }
    }
}
