using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class RessumoGerenteMesAnterior
    {
        public ReceberObjetoResponse<ResumoGerenteinfo> ReceberDadosMesAnterior(ReceberEntidadeRequest<ResumoGerenteinfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoGerenteinfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            lRetorno = new ReceberObjetoResponse<ResumoGerenteinfo>();

            lRetorno.Objeto = new ResumoGerenteinfo();


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CORRETAGEM_MENSAL_ANT"))
            {


                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BOVESPA_MES_ANT"   , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BVSP_MES_ANT"          , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BMF_MES_ANT"       , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BMF_MES_ANT"           , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CORRETAGEM_BMF_BVSP_MES_ANT"  , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "VOLUME_BMF_BVSP_MES_ANT"      , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "CADASTRO_MES_ANT"             , DbType.Decimal, 0);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BVSP)
                {
                    lRetorno.Objeto.CorretagemMes   = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BOVESPA_MES_ANT"  );
                    lRetorno.Objeto.VolumeMes       = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BVSP_MES_ANT"         );
                    lRetorno.Objeto.CadastradoMes   = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES_ANT"            );
                }
                else if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF)
                {
                    lRetorno.Objeto.CorretagemMes   = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BMF_MES_ANT"  );
                    lRetorno.Objeto.VolumeMes       = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BMF_MES_ANT"      );
                    lRetorno.Objeto.CadastradoMes   = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES_ANT"        ); //verificar
                }
                else //Bovespa e BMF
                {
                    lRetorno.Objeto.CorretagemMes   = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "CORRETAGEM_BMF_BVSP_MES_ANT" );
                    lRetorno.Objeto.VolumeMes       = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "VOLUME_BMF_BVSP_MES_ANT"     );
                    lRetorno.Objeto.CadastradoMes   = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "CADASTRO_MES_ANT"            ); //verificar
                }




            }

            return lRetorno;

        }
    }
}
