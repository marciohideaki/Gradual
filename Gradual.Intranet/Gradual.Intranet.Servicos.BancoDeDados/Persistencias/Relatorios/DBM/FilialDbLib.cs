using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.OMS.Persistencia;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class FilialDbLib
    {
        public ConsultarObjetosResponse<FilialInfo> ConsultarFilial(ConsultarEntidadeRequest<FilialInfo> pParametro)
        {
            try
            {
                ConsultarObjetosResponse<FilialInfo> resposta =
                    new ConsultarObjetosResponse<FilialInfo>();

                FilialInfo filial = new FilialInfo();


                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_STB_FILIAL"))
                {

                    if(pParametro.Objeto.CodigoFilial > 0 )
                        lAcessaDados.AddInParameter(lDbCommand, "@P_ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                    if (pParametro.Objeto.NomeFilial != null && pParametro.Objeto.NomeFilial  != string.Empty)
                        lAcessaDados.AddInParameter(lDbCommand, "@p_ds_filial", DbType.String, pParametro.Objeto.NomeFilial);


                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            filial = new FilialInfo();

                            filial.NomeFilial = lDataTable.Rows[i]["ds_filial"].ToString();
                            filial.CodigoFilial = lDataTable.Rows[i]["ID_FILIAL"].DBToInt32();
                            

                            resposta.Resultado.Add(filial);

                        }

                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametro.Objeto, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        public ConsultarObjetosResponse<FilialAssessorInfo> ConsultarFilial(ConsultarEntidadeRequest<FilialAssessorInfo> pParametro)
        {
            try
            {
                ConsultarObjetosResponse<FilialAssessorInfo> resposta =
                    new ConsultarObjetosResponse<FilialAssessorInfo>();

                FilialAssessorInfo FilialAssessor = new FilialAssessorInfo();


                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeFilialAssessor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_STB_ASSESSOR_FILIAL"))
                {

                    if (pParametro.Objeto.CodigoFilial > 0)
                        lAcessaDados.AddInParameter(lDbCommand, "@P_ID_FILIAL", DbType.Int32, pParametro.Objeto.CodigoFilial);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            FilialAssessor = new FilialAssessorInfo();

                            FilialAssessor.CodigoAssessor   = lDataTable.Rows[i]["id_assessor"].DBToInt32() ;
                            FilialAssessor.CodigoFilial     = lDataTable.Rows[i]["ds_filial"].DBToInt32()   ;


                            resposta.Resultado.Add(FilialAssessor);

                        }

                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametro.Objeto, pParametro.IdUsuarioLogado, pParametro.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }
    }
}
